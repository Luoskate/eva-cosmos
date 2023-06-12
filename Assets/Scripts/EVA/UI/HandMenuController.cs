using UnityEngine;
using Veery.Gameplay;
using static OVRInput;

namespace Veery.UI {
    /// <summary>
    /// Controls the positioning and parenting of a menu based on the active <see cref="Controller"/> and <see cref="Handedness"/>.
    /// </summary>
    public class HandMenuController : MonoBehaviour {
        #region Serialized Fields

        [SerializeField]
        [Tooltip("The scale applied to the menu.")]
        /// <summary>
        /// The scale applied to the menu.
        /// </summary>
        private Vector3 _scale;

        [Header("Controller")]
        [SerializeField]
        [Tooltip("The transform representing the left controller.")]
        /// <summary>
        /// The transform representing the left controller.
        /// </summary>
        private Transform _controllerLeft;

        [SerializeField]
        [Tooltip("The transform representing the right controller.")]
        /// <summary>
        /// The transform representing the right controller.
        /// </summary>
        private Transform _controllerRight;

        [SerializeField]
        [Tooltip("The offset applied to the controller's position.")]
        /// <summary>
        /// The offset applied to the controller's position.
        /// </summary>
        private Vector3 _controllerOffset;

        [SerializeField]
        [Tooltip("The offset applied to the controller's rotation.")]
        /// <summary>
        /// The offset applied to the controller's rotation.
        /// </summary>
        private Quaternion _controllerOffsetRotation;

        [Header("Hand")]
        [SerializeField]
        [Tooltip("The transform representing the left hand.")]
        /// <summary>
        /// The transform representing the left hand.
        /// </summary>
        private Transform _handLeft;

        [SerializeField]
        [Tooltip("The transform representing the right hand.")]
        /// <summary>
        /// The transform representing the right hand.
        /// </summary>
        private Transform _handRight;

        [SerializeField]
        [Tooltip("The offset applied to the hand's position.")]
        /// <summary>
        /// The offset applied to the hand's position.
        /// </summary>
        private Vector3 _handOffset;

        [SerializeField]
        [Tooltip("The offset applied to the hand's rotation.")]
        /// <summary>
        /// The offset applied to the hand's rotation.
        /// </summary>
        private Quaternion _handOffsetRotation;
        #endregion Serialized Fields

        #region Methods
        private void OnEnable() {
            // Subscribe to the events triggered by the GameplayManager.
            GameplayManager.ControllerChanged += ChangeParent;
            GameplayManager.HandednessChanged += ChangeParent;
        }

        private void OnDisable() {
            // Unsubscribe from the events triggered by the GameplayManager.
            GameplayManager.ControllerChanged -= ChangeParent;
            GameplayManager.HandednessChanged -= ChangeParent;
        }

        /// <summary>
        /// Changes the parent of the menu controller based on the active <see cref="Controller"/> and <see cref="Handedness"/>.
        /// </summary>
        private void ChangeParent() {
            Controller controller = GetActiveController();
            Handedness dominantHand = GameplayManager.Instance.DominantHand;

            Debug.Log(
                $"[{GetType().Name}] ChangeParent() | active controller: {controller}, dominant hand: {dominantHand}");

            Transform parentTransform;
            switch (controller) {
                case Controller.Hands:
                    parentTransform = (dominantHand == Handedness.LeftHanded) ? _handRight : _handLeft;
                    transform.SetLocalPositionAndRotation(_handOffset, _handOffsetRotation);
                    break;

                case Controller.Touch:
                    parentTransform = (dominantHand == Handedness.LeftHanded) ? _controllerRight : _controllerLeft;
                    transform.SetLocalPositionAndRotation(_controllerOffset, _controllerOffsetRotation);
                    break;

                default:
                    Debug.Log($"[{GetType().Name}] ChangeParent() | Not implemented for {controller}");
                    return;
            }

            // Set the parent and scale of the menu controller.
            transform.SetParent(parentTransform);
            transform.localScale = _scale;
        }
        #endregion Methods
    }
}
