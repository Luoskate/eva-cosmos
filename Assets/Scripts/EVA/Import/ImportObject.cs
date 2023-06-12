using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using Veery.Import.Properties;
using Veery.Import.Triggers;
using Veery.Interaction;

namespace Veery.Import {
    /// <summary>
    /// Represents an object that can be imported into the scene.
    /// </summary>
    public class ImportObject : MonoBehaviour, IEnableable {
        #region Serialized Fields
        [SerializeField]
        [Tooltip("The GameObjects to enable after the object has been imported.")]
        private List<GameObject> _deferredGOs = new();
        #endregion Serialized Fields

        private DistanceSelectInteractable interactable;
        private GameObject head;
        private bool selected;

        /// <summary>
        /// Gets or sets the URL of the object to be imported.
        /// </summary>
        public string Url { get; set; }

        #region Methods
        public IEnumerator Start() {
            while (true) {
                interactable = GetComponentInChildren<DistanceSelectInteractable>();

                if (interactable != null) {
                    interactable.WhenStateChanged += IsSelected;
                    head = TriggersManager.Instance.Head.gameObject;

                    yield break;
                }

                yield return null;
            }
        }

        public void Update() {
            if (!selected) {
                return;
            }

            Vector2 thumbstickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick); // Get right thumbstick input

            // Calculate the movement direction relative to the head position
            const float moveSpeed = 0.1f; // Set move speed

            if (thumbstickInput.y > 0f) {
                Vector3 newDirection = Mathf.Abs(thumbstickInput.y)
                    * moveSpeed
                    * (transform.position - head.transform.position).normalized;
                transform.position += newDirection;
            } else if (thumbstickInput.y < 0f) { // change direction condition from greater than to less than
                Vector3 newDirection = Mathf.Abs(thumbstickInput.y) * moveSpeed * (head.transform.position - transform.position).normalized; // reverse the calculation of newDirection and multiply by the absolute value of the thumbstick input
                transform.position += newDirection;
            }

            if (thumbstickInput.x < 0f) { // change to checking for left input
                Vector3 newDirection = -1 * Mathf.Abs(thumbstickInput.x) * moveSpeed * head.transform.right; // set new direction to the left of the head
                transform.position += newDirection;
            } else if (thumbstickInput.x > 0f) { // change to checking for left input
                Vector3 newDirection = Mathf.Abs(thumbstickInput.x) * moveSpeed * head.transform.right; // set new direction to the left of the head
                transform.position += newDirection;
            }
        }

        /// <summary>
        /// Callback method that is called when the object is selected by an interactor.
        /// </summary>
        /// <param name="args">The arguments for the state change.</param>
        public void IsSelected(InteractableStateChangeArgs args) {
            GameObject selectedObject = interactable.transform.parent.gameObject;
            if (args.NewState == InteractableState.Select) {
                selected = true;
            }

            interactable.WhenInteractorRemoved.Action += IsDeselected;
        }

        /// <summary>
        /// Callback method that is called when the object is deselected by the specified interactor.
        /// </summary>
        /// <param name="dsi">The interactor that deselected the object.</param>
        private void IsDeselected(DistanceSelectInteractor dsi) {
            if (dsi != TriggersManager.Instance.Interactor) {
                return;
            }

            selected = false;
        }

        /// <summary>
        /// Activates all deferred game objects.
        /// </summary>
        public void LoadDeferredGOs() {
            foreach (GameObject go in _deferredGOs) {
                go.SetActive(true);
            }
        }

        void IEnableable.Toggle(bool value) {
            Debug.Log($"[{GetType().Name}] Toggle({value})");
            gameObject.SetActive(value);
        }

        bool IEnableable.IsEnabled() {
            return gameObject.activeSelf;
        }
        #endregion Methods
    }
}
