using UnityEngine;

namespace Veery.UI {
    /// <summary>
    /// A component that allows a UI element to be collapsed and expanded.
    /// </summary>
    public class Collapsable : MonoBehaviour {
        #region Serialized Fields
        [SerializeField]
        [Tooltip("The game object representing the collapse container.")]
        /// <summary>
        /// The <see cref="GameObject"/> representing the container that holds the collapsable object.
        /// </summary>
        private GameObject _collapseContainerGO;

        [SerializeField]
        [Tooltip("The RectTransform of the notch.")]
        /// <summary>
        /// The <see cref="RectTransform"/> of the notch used to collapse and expand the collapsable object.
        /// </summary>
        private RectTransform _notchRectTransform;

        [SerializeField]
        [Tooltip("The RectTransform of the collapsable object.")]
        /// <summary>
        /// The <see cref="RectTransform"/> of the collapsable object.
        /// </summary>
        private RectTransform _collapsableRectTransform;

        [SerializeField]
        [Tooltip("The RectTransform of the surface.")]
        /// <summary>
        /// The <see cref="RectTransform"/> of the surface used to collapse and expand the collapsable object.
        /// </summary>
        private RectTransform _surfaceRectTransform;
        #endregion Serialized Fields

        #region Properties
        /// <summary>
        /// The size of the <see cref="Collapsable"/> object when it is fully opened.
        /// </summary>
        public Vector3 OpenedSize { get; private set; }

        /// <summary>
        /// The size of the <see cref="Collapsable"/> object when it is fully collapsed.
        /// </summary>
        public Vector3 CollapsedSize { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Collapsable"/> object is currently collapsed.
        /// </summary>
        public bool IsCollapsed { get; private set; }
        #endregion Properties

        #region Methods
        private void Start() {
            // Initialize the sizes and state of the collapsible object
            OpenedSize = new(_collapsableRectTransform.sizeDelta.x, _collapsableRectTransform.sizeDelta.y, 1);
            CollapsedSize = new(OpenedSize.x, _notchRectTransform.sizeDelta.y, 1);
            IsCollapsed = true;
            SetCollapsedState(IsCollapsed);
        }

        /// <summary>
        /// Toggles the collapsed state of the <see cref="Collapsable"/> object.
        /// </summary>
        public void Collapse() {
            Debug.Log($"[{GetType().Name}] Collapse()");

            IsCollapsed = !IsCollapsed;
            SetCollapsedState(IsCollapsed);
        }

        /// <summary>
        /// Sets the collapsed state of the <see cref="Collapsable"/> object.
        /// </summary>
        /// <param name="collapsed">The new collapsed state of the <see cref="Collapsable"/> object.</param>
        public void SetCollapsedState(bool collapsed) {
            Debug.Log($"[{GetType().Name}] SetCollapsedState({collapsed})");

            // Activate or deactivate the collapse container based on the collapsed state
            _collapseContainerGO.SetActive(!collapsed);

            // Adjust the size and scale of the collapsable object and surface based on the collapsed state
            _collapsableRectTransform.sizeDelta = (collapsed) ? CollapsedSize : OpenedSize;
            _surfaceRectTransform.localScale = (collapsed) ? CollapsedSize : OpenedSize;
        }
        #endregion Methods
    }
}
