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
    public class AnchorsManager : MonoBehaviour {
        [SerializeField]
        private DistanceSelectInteractor _interactor;

        [SerializeField]
        public TextMeshProUGUI _debugText;

        private DistanceSelectInteractable interactable;

        private List<string> uuids = new();

        private void Start() {
            _interactor.WhenStateChanged += Select;
            string uuidsCat = PlayerPrefs.GetString("uuids", "");
            string[] tmp = uuidsCat.Split(" ");
            uuids.AddRange(tmp);
        }

        private void Select(InteractorStateChangeArgs args) {
            if (args.NewState != InteractorState.Select) {
                return;
            }

            Debug.Log($"select {uuids.Count}");
            // Remove any existing anchors
            interactable = _interactor.Interactable;
            if (interactable != null
                && interactable.transform.parent.gameObject
                    .TryGetComponent(component: out OVRSpatialAnchor oVRSpatialAnchor)) {
                Debug.Log("Destroy any anchor");

                Destroy(oVRSpatialAnchor);
                OnEraseButtonPressed();
                _debugText.text = "Destroy any anchor";
            }

            _debugText.text = "Select";

            // Assign the new interactable and attach the deselection handler
            interactable.WhenInteractorRemoved.Action += Deselect;
        }

        private void Deselect(DistanceSelectInteractor dsi) {
            if (dsi != _interactor) {
                return;
            }

            if (interactable.transform.parent.gameObject.TryGetComponent(component: out OVRSpatialAnchor _)) {
                // Un OVRSpatialAnchor est déjà attaché à l'objet interactable
                Debug.Log("Spatial anchor already exists");
                return;
            }
            // Ajouter un nouveau spatial anchor à l'objet parent
            _ = interactable.transform.parent.gameObject.AddComponent<OVRSpatialAnchor>();
            Debug.Log("Spatial anchor created");
            _debugText.text = "Deselect : Spatial anchor created";

            //CheckSpatialAnchor();
            _ = StartCoroutine(CheckSpatialAnchor());
        }

        private IEnumerator CheckSpatialAnchor() {
            while (interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>()
                && !interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>().Created) {
                yield return null;
            }

            if (interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>()) {
                Debug.Log(
                    "UUID = "
                        + interactable
                            .transform
                            .parent
                            .gameObject
                            .GetComponent<OVRSpatialAnchor>()
                            .Uuid
                            .ToString("D"));
                _debugText.text = "UUID = "
                    + interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>().Uuid.ToString("D");
                OnSaveLocalButtonPressed();
            } else {
                Destroy(gameObject);
            }
        }

        public void OnEraseButtonPressed() {
            if (!interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>()) {
                return;
            }

            interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>().Erase((anchor, success) => {
                if (success) {
                    Debug.Log($"OnEraseButtonPressed {uuids.Count}");
                    foreach (string uuid in uuids) {
                        Debug.Log($"OnEraseButtonPressed {uuid}");
                    }

                    Debug.Log($"OnEraseButtonPressed {anchor.Uuid}");

                    bool removed = uuids.Remove(anchor.Uuid.ToString());

                    foreach (string uuid in uuids) {
                        Debug.Log($"OnEraseButtonPressed after {uuid}");
                    }

                    if (removed) {
                        Debug.Log($"erase uuid | uuids = {uuids.Count}");
                        PlayerPrefs.DeleteKey(anchor.Uuid.ToString());
                        PlayerPrefs.SetString("uuids", string.Join(" ", uuids));
                        Debug.Log("Success Erase");
                        _debugText.text = "Success Erase";
                    }
                }
            });
        }

        public void RemoveAnchorUUIDFromPlayerPrefs() {
            foreach (string uuid in uuids) {
                PlayerPrefs.DeleteKey(uuid);
            }

            Debug.Log("Clear all uuids");
            uuids.Clear();
            PlayerPrefs.DeleteKey("uuids");

            _debugText.text = "All Erase";
        }

        public void OnSaveLocalButtonPressed() {
            if (!interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>()) {
                return;
            }

            interactable.transform.parent.gameObject.GetComponent<OVRSpatialAnchor>().Save((anchor, success) => {
                if (!success) {
                    return;
                }

                Debug.Log("Save True");
                _debugText.text = "Save True";

                uuids.Add(anchor.Uuid.ToString());
                PlayerPrefs.SetString("uuids", string.Join(" ", uuids));
                PlayerPrefs.SetString(anchor.Uuid.ToString(), GetUrlModel());

                Debug.Log($"OnSaveLocalButtonPressed {uuids.Count}");
                foreach (string uuid in uuids) {
                    Debug.Log($"OnSaveLocalButtonPressed {uuid}");
                }
            });
        }

        //LOAD

        private Action<OVRSpatialAnchor.UnboundAnchor, bool, string> _onLoadAnchor;

        public void OnLoadAnchorsButtonPressed() {
            LoadAnchorsByUuid();
        }

        public void LoadAnchorsByUuid() {
            Debug.Log($"LoadAnchorsByUuid {uuids.Count}");
            foreach (string uuid in uuids) {
                Debug.Log($"LoadAnchorsByUuid {uuid}");
            }

            Load(new OVRSpatialAnchor.LoadOptions() {
                Timeout = 0,
                StorageLocation = OVRSpace.StorageLocation.Local,
                Uuids = uuids.ConvertAll(Guid.Parse)
            });
        }

        private void Awake() {
            _onLoadAnchor = OnLocalized;
        }

        private void Load(OVRSpatialAnchor.LoadOptions options) {
            _ = OVRSpatialAnchor.LoadUnboundAnchors(
                options,
                anchors => {
                    if (anchors == null) {
                        Debug.Log("Query failed.");
                        return;
                    }

                    string[] modelsUrls = new string[anchors.Length];
                    for (int i = 0; i < anchors.Length; ++i) {
                        modelsUrls[i] = PlayerPrefs.GetString(anchors[i].Uuid.ToString(), "");
                    }

                    Debug.Log("YUPP" + anchors.Length);
                    for (int i = 0; i < anchors.Length; ++i) {
                        Debug.Log("YUPP" + anchors.Length);
                        Debug.Log("YUPP" + i + anchors[i].Uuid + " path =" + modelsUrls[i]);
                        _debugText.text = "LOAD" + anchors[i].Uuid + " path =" + modelsUrls[i];

                        string modelPath = modelsUrls[i]; // Copie locale de la variable 'model'

                        if (anchors[i].Localized) {
                            _onLoadAnchor(anchors[i], true, modelPath);
                        } else if (!anchors[i].Localizing) {
                            // Utilisez une fonction lambda pour capturer le tableau 'modelsUrls' et passer la copie locale à la fonction '_onLoadAnchor'
                            anchors[i].Localize((anchor, success) => _onLoadAnchor(anchor, success, modelPath));
                        }
                    }
                });
        }

        private async void OnLocalized(OVRSpatialAnchor.UnboundAnchor unboundAnchor, bool success, string model) {
            if (!success) {
                Debug.Log($"{unboundAnchor} Localization failed!");
                _debugText.text = $"{unboundAnchor} Localization failed!";

                return;
            }

            Debug.Log("NAME UUID" + unboundAnchor.Uuid.ToString());
            Debug.Log("NAME MODEL" + model);
            _debugText.text = "OnLocalized:  NAME UUID" + unboundAnchor.Uuid.ToString();

            Pose pose = unboundAnchor.Pose;

            Tuple<bool, GameObject> result = await FileExplorerManager.Instance.Import(model, pose);
            GameObject modelPrefab = result.Item2;
            _ = modelPrefab.AddComponent<OVRSpatialAnchor>();
            unboundAnchor.BindTo(modelPrefab.GetComponent<OVRSpatialAnchor>());
        }

        public string GetUrlModel() {
            return interactable.transform.parent.gameObject.GetComponent<ImportObject>().Url;
        }

        public void TestUnique() {
            Debug.Log("NAME" + interactable.transform.parent.gameObject.GetComponent<ImportObject>().Url);
            Test(interactable.transform.parent.gameObject.GetComponent<ImportObject>().Url);
        }

        private async void Test(string url) {
            _ = await FileExplorerManager.Instance.Import(url);
            Debug.Log("NAME YUP");
        }
    }
}
