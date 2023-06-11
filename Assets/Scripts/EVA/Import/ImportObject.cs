using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using Veery.Import.Triggers;
using Veery.Import.Triggers.Triggers;
using Veery.Interaction;

namespace Veery.Import {
    public class ImportObject : MonoBehaviour, IEnableable {
        #region Serialized Fields
        [SerializeField]
        private List<GameObject> _deferredGOs = new();
        #endregion Serialized Fields

        private DistanceSelectInteractable interactable;
        private GameObject head;
        private bool selected;

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

        public void IsSelected(InteractableStateChangeArgs args) {
            GameObject selectedObject = interactable.transform.parent.gameObject;
            if (args.NewState == InteractableState.Select) {
                selected = true;
            }

            interactable.WhenInteractorRemoved.Action += IsDeselected;
        }

        private void IsDeselected(DistanceSelectInteractor dsi) {
            if (dsi != TriggersManager.Instance.Interactor) {
                return;
            }

            selected = false;
        }

        public void LoadDeferredGOs() {
            // Activate all deferred game objects
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
