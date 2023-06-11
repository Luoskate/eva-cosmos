using UnityEngine;
using UnityEngine.UI;
using Veery.Gameplay;
using static OVRInput;

namespace Veery.UI {
    public class HandMenuController : MonoBehaviour {
        #region Serialized Fields
        [SerializeField]
        private Transform controllerLeft;

        [SerializeField]
        private Transform controllerRight;

        [SerializeField]
        private Transform handLeft;

        [SerializeField]
        private Transform handRight;

        [SerializeField]
        private Vector3 controllerOffset;

        [SerializeField]
        private Quaternion controllerOffsetRotation;

        [SerializeField]
        private Vector3 handOffset;

        [SerializeField]
        private Quaternion handOffsetRotation;

        [SerializeField]
        private Vector3 scale;
        #endregion Serialized Fields

        private Transform _parent;

        private void Start() {
            GameplayManager.ControllerChanged += ChangeParent;
            GameplayManager.HandednessChanged += ChangeParent;
        }

        private void ChangeParent() {
            Debug.Log($"[{GetType().Name}] ChangeParent() |"
                + $" active controller: {GetActiveController()},"
                + $" dominant hand: {GameplayManager.Instance.DominantHand}");

            switch (GetActiveController()) {
                case Controller.Hands:
                    _parent = (GameplayManager.Instance.DominantHand == Handedness.LeftHanded)
                        ? handRight
                        : handLeft;
                    transform.SetParent(_parent);
                    transform.SetLocalPositionAndRotation(handOffset, handOffsetRotation);
                    transform.localScale = scale;
                    break;

                case Controller.Touch:
                    _parent = (GameplayManager.Instance.DominantHand == Handedness.LeftHanded)
                        ? controllerRight
                        : controllerLeft;
                    transform.SetParent(_parent);
                    transform.SetLocalPositionAndRotation(controllerOffset, controllerOffsetRotation);
                    transform.localScale = scale;
                    break;

                default:
                    Debug.Log($"[{GetType().Name}] ChangeParent() | Not implemented for {GetActiveController()}");
                    break;
            }
        }
    }
}
