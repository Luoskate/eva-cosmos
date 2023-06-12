using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using UnityEngine;
using Veery.Import;
using Veery.Interaction;
using Veery.UI;

namespace Veery.Anchors {
    /// <summary>
    /// Manages the creation, deletion, and loading of spatial anchors.
    /// </summary>
    public class AnchorsManager : MonoBehaviour {
        #region Serialized Fields
        [SerializeField]
        [Tooltip("The distance select interactor.")]
        /// <summary>
        /// The distance select interactor used to select and deselect interactables.
        /// </summary>
        private DistanceSelectInteractor _interactor;

        [SerializeField]
        [Tooltip("The text with information about anchors states.")]
        /// <summary>
        /// The TextMeshProUGUI component used to display debug information about the anchors.
        /// </summary>
        private TextMeshProUGUI _debugText;
        #endregion Serialized Fields

        private OVRSpatialAnchor _anchor;

        #region Properties
        /// <summary>
        /// The URL of the import object associated with the current interactable.
        /// </summary>
        private string ImportUrl => Interactable.transform.parent.gameObject.GetComponent<ImportObject>().Url;

        /// <summary>
        /// The current distance select interactable.
        /// </summary>
        public DistanceSelectInteractable Interactable { get; set; }

        /// <summary>
        /// A list of UUIDs representing the saved anchors.
        /// </summary>
        public List<string> Uuids { get; } = new();
        #endregion Properties

        #region Methods
        private void Start() {
            _interactor.WhenStateChanged += Select;

            // Load saved UUIDs from player preferences.
            string uuidsCat = PlayerPrefs.GetString("uuids", "");
            Uuids.AddRange(uuidsCat.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Called when the state of the distance select interactor changes to "Select".
        /// Sets the current interactable and erases any existing spatial anchor associated with it.
        /// </summary>
        /// <param name="args">The arguments containing information about the state change.</param>
        private void Select(InteractorStateChangeArgs args) {
            if (args.NewState != InteractorState.Select) {
                return;
            }

            Interactable = _interactor.Interactable;

            if (Interactable.transform.parent.gameObject.TryGetComponent(out _anchor)) {
                _anchor.Erase(Erase);
                _anchor = null;
            }

            Interactable.WhenInteractorRemoved.Action += Deselect;
        }

        /// <summary>
        /// Called when the distance select interactor is removed from the current interactable.
        /// Adds an OVRSpatialAnchor component to the parent game object of the interactable and waits for it to be created.
        /// </summary>
        /// <param name="dsi">The distance select interactor that was removed.</param>
        private void Deselect(DistanceSelectInteractor dsi) {
            if (dsi != _interactor) {
                return;
            }

            if (Interactable.transform.parent.gameObject.TryGetComponent(out OVRSpatialAnchor _)) {
                return;
            }

            // Add OVRSpatialAnchor component to the parent game object of the interactable.
            _anchor = Interactable.transform.parent.gameObject.AddComponent<OVRSpatialAnchor>();

            _ = StartCoroutine(WaitForCreation());
        }


        /// <summary>
        /// Waits until the creation of the OVRSpatialAnchor component has ended, then saves the anchor.
        /// </summary>
        /// <returns>An IEnumerator used for coroutines.</returns>
        private IEnumerator WaitForCreation() {
            yield return new WaitUntil(() => !_anchor || _anchor.Created);

            if (!_anchor) {
                yield break;
            }

            _anchor.Save(Save);
        }

        /// <summary>
        /// Called when the OVRSpatialAnchor component associated with the current interactable is erased.
        /// Removes the UUID of the anchor from the list of saved anchors and updates the player preferences.
        /// </summary>
        /// <param name="anchor">The OVRSpatialAnchor component that was erased.</param>
        /// <param name="success">Whether the erasure was successful or not.</param>
        private void Erase(OVRSpatialAnchor anchor, bool success) {
            if (!success) {
                Debug.LogWarning($"[{GetType().Name}] Erase() - Failed to erase anchor {anchor.Uuid}");
                return;
            }

            // Remove the UUID from the list.
            bool removed = Uuids.Remove(anchor.Uuid.ToString());

            if (!removed) {
                Debug.LogWarning($"[{GetType().Name}] Erase() - Failed to remove anchor {anchor.Uuid} from the list of saved anchors");
                return;
            }

            // Delete the saved UUID -> import URL and update the player preferences.
            PlayerPrefs.DeleteKey(anchor.Uuid.ToString());
            PlayerPrefs.SetString("uuids", string.Join(" ", Uuids));

            _debugText.text = $"Anchor Deleted {anchor.Uuid}";
            Debug.Log($"[{GetType().Name}] Erase() - Anchor Deleted {anchor.Uuid}");
            Destroy(anchor);
        }

        /// <summary>
        /// Removes all saved anchors by deleting their UUIDs from player preferences and clearing the list of saved UUIDs.
        /// </summary>
        public void RemoveAll() {
            foreach (string uuid in Uuids) {
                PlayerPrefs.DeleteKey(uuid);
            }

            Uuids.Clear();
            PlayerPrefs.DeleteKey("uuids");

            _debugText.text = "All Anchors Are Deleted";
            Debug.Log($"[{GetType().Name}] RemoveAll() - All Anchors Are Deleted");
        }

        /// <summary>
        /// Called when the OVRSpatialAnchor component associated with the current interactable is saved.
        /// Adds the UUID of the anchor to the list of saved anchors and updates the player preferences with the import URL.
        /// </summary>
        /// <param name="anchor">The OVRSpatialAnchor component that was saved.</param>
        /// <param name="success">Whether the saving was successful or not.</param>
        private void Save(OVRSpatialAnchor anchor, bool success) {
            if (!success) {
                Debug.LogWarning($"[{GetType().Name}] Save() - Failed to save anchor {anchor.Uuid}");
                return;
            }

            // Add the UUID to the list, the import URL and update the player preferences.
            Uuids.Add(anchor.Uuid.ToString());
            PlayerPrefs.SetString("uuids", string.Join(' ', Uuids));
            PlayerPrefs.SetString(anchor.Uuid.ToString(), ImportUrl);
            _debugText.text = $"Saved Anchor {anchor.Uuid}";
            Debug.Log($"[{GetType().Name}] Save() - Saved Anchor {anchor.Uuid}");
        }

        /// <summary>
        /// Loads all saved anchors and binds them to their corresponding game objects.
        /// </summary>
        public void LoadAll() {
            _ = OVRSpatialAnchor.LoadUnboundAnchors(
                new OVRSpatialAnchor.LoadOptions() {
                    Timeout = 0,
                    StorageLocation = OVRSpace.StorageLocation.Local,
                    Uuids = Uuids.ConvertAll(Guid.Parse)
                },
                BindAll);
        }

        /// <summary>
        /// Binds all unbound anchors to their corresponding game objects.
        /// If an anchor is already localized, it calls the OnLocalized method with the anchor's information.
        /// If an anchor is not localized, it localizes it and calls the OnLocalized method with the anchor's information when it's done.
        /// </summary>
        /// <param name="unboundAnchors">The array of unbound anchors to bind.</param>
        private void BindAll(OVRSpatialAnchor.UnboundAnchor[] unboundAnchors) {
            foreach (OVRSpatialAnchor.UnboundAnchor unboundAnchor in unboundAnchors) {
                if (unboundAnchor.Localized) {
                    OnLocalized(unboundAnchor, true, PlayerPrefs.GetString(unboundAnchor.Uuid.ToString(), ""));
                } else if (!unboundAnchor.Localizing) {
                    unboundAnchor.Localize((anchor, success) => OnLocalized(
                        anchor,
                        success,
                        PlayerPrefs.GetString(anchor.Uuid.ToString(), "")));
                }
            }
        }

        /// <summary>
        /// Called when an OVRSpatialAnchor.UnboundAnchor is localized.
        /// Imports the model associated with the anchor and binds it to the anchor.
        /// </summary>
        /// <param name="unboundAnchor">The OVRSpatialAnchor.UnboundAnchor that was localized.</param>
        /// <param name="success">Whether the localization was successful or not.</param>
        /// <param name="model">The import URL of the model associated with the anchor.</param>
        private async void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success, string model) {
            if (!success) {
                Debug.LogWarning($"[{GetType().Name}] OnLocalized() - Failed to localize anchor {unboundAnchor.Uuid}");
                return;
            }

            Pose pose = unboundAnchor.Pose;

            // Import the model using the FileExplorerManager and get the resulting model prefab.
            Tuple<bool, GameObject> result = await FileExplorerManager.Instance.Import(model, pose);
            GameObject modelPrefab = result.Item2;

            // Add OVRSpatialAnchor component to the model prefab and bind it to the unbound anchor.
            OVRSpatialAnchor anchor = modelPrefab.AddComponent<OVRSpatialAnchor>();
            unboundAnchor.BindTo(anchor);

            _debugText.text = $"Last Localized Anchor: {unboundAnchor.Uuid}";
            Debug.Log($"[{GetType().Name}] OnLocalized() - Localized Anchor: {unboundAnchor.Uuid}");
        }
        #endregion Methods
    }
}
