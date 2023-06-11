using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Veery.UI {
    /// <summary>
    /// Represents a button that can collapse or open a list of <see cref="Collapsable"/> components.
    /// </summary>
    public class CollapseButton : MonoBehaviour {
        #region Serialized Fields
        [SerializeField]
        [Tooltip("The Image component representing the button notch.")]
        /// <summary>
        /// The <see cref="Image"/> component representing the button notch.
        /// </summary>
        private Image _notchImage;

        [SerializeField]
        [Tooltip("The sprite used for the button when collapsed.")]
        /// <summary>
        /// The <see cref="Sprite"/> used for the button when collapsed.
        /// </summary>
        private Sprite _notchCollapsedSprite;

        [SerializeField]
        [Tooltip("The sprite used for the button when opened.")]
        /// <summary>
        /// The <see cref="Sprite"/> used for the button when opened.
        /// </summary>
        private Sprite _notchOpenedSprite;

        [SerializeField]
        [Tooltip("A list of Collapsable components affected by this button.")]
        /// <summary>
        /// A list of <see cref="Collapsable"/> components affected by this button.
        /// </summary>
        private List<Collapsable> _collapsables;
        #endregion Serialized Fields

        private EventSystem _eventSystem;

        #region Methods
        private void Start() {
            _eventSystem = FindObjectOfType<EventSystem>();
            if (_eventSystem == null) {
                Debug.LogError($"[{GetType().Name}] No EventSystem found in the scene.");
            }

            if (_collapsables.Count == 0) {
                return;
            }
            // Set the initial sprite of the button based on the collapsed state of the first collapsable component.
            _notchImage.sprite = (_collapsables[0].IsCollapsed) ? _notchCollapsedSprite : _notchOpenedSprite;
        }

        /// <summary>
        /// Event handler for the button click event. Collapses or opens the collapsable components in the list.
        /// </summary>
        public void OnClick() {
            Debug.Log($"[{GetType().Name}] OnClick()");

            // Collapse or open each collapsable component in the list.
            foreach (Collapsable collapsable in _collapsables) {
                collapsable.Collapse();
            }

            // Update the sprite of the button based on the collapsed state of the first collapsable component.
            if (_collapsables.Count > 0) {
                // Toggle the sprite of the button between the collapsed and opened states.
                _notchImage.sprite = (_collapsables[0].IsCollapsed) ? _notchCollapsedSprite : _notchOpenedSprite;
            }

            // Clear the selected game object in the EventSystem.
            _eventSystem.SetSelectedGameObject(null);
        }
        #endregion Methods
    }
}
