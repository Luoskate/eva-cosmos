using Oculus.Interaction;
using UnityEngine;

namespace Veery.Interaction {
    /// <summary>
    /// This class provides an outline effect for an interactable object. It listens to the state changes of an <see cref="IInteractableView"/> and updates the outline accordingly.
    /// </summary>
    public class InteractableOutline : MonoBehaviour {
        [SerializeField]
        [Interface(typeof(IInteractableView))]
        [Tooltip("The MonoBehaviour that represents the interactable view.")]
        /// <summary>
        /// The MonoBehaviour that represents the interactable view.
        /// </summary>
        private MonoBehaviour _interactableView;

        [SerializeField]
        [Optional]
        [Tooltip("The outline to use for the interactable.")]
        /// <summary>
        /// The outline component used to display the outline effect for the interactable object.
        /// </summary>
        private Outline _outline;

        [SerializeField]
        [Tooltip("The color of the outline when the interactable is hovered.")]
        /// <summary>
        /// The color of the outline when the interactable is being hovered over.
        /// </summary>
        private Color _hoverColor = Color.white;

        [SerializeField]
        [Tooltip("The size of the outline when the interactable is hovered.")]
        /// <summary>
        /// The size of the outline when the interactable is being hovered over.
        /// </summary>
        private float _hoverSize = 2;

        [SerializeField]
        [Tooltip("The color of the outline when the interactable is selected.")]
        /// <summary>
        /// The color of the outline when the interactable is selected.
        /// </summary>
        private Color _selectColor = new(0, 0.2901961f, 0.7254902f);

        [SerializeField]
        [Tooltip("The size of the outline when the interactable is selected.")]
        /// <summary>
        /// The size of the outline when the interactable is selected.
        /// </summary>
        private float _selectSize = 10;

        private IInteractableView InteractableView;
        protected bool _started;

        protected virtual void Awake() {
            InteractableView = _interactableView as IInteractableView;
        }

        protected virtual void Start() {
            this.BeginStart(ref _started);
            this.AssertField(InteractableView, nameof(InteractableView));

            UpdateVisual();
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable() {
            if (!_started) {
                return;
            }

            InteractableView.WhenStateChanged += UpdateVisualState;
            UpdateVisual();
        }

        protected virtual void OnDisable() {
            if (!_started) {
                return;
            }

            InteractableView.WhenStateChanged -= UpdateVisualState;
        }

        /// <summary>
        /// Updates the visual state of the interactable object based on its current state.
        /// </summary>
        private void UpdateVisual() {
            switch (InteractableView.State) {
                case InteractableState.Normal:
                    _outline.enabled = false;
                    break;

                case InteractableState.Hover:
                    _outline.enabled = true;
                    _outline.OutlineColor = _hoverColor;
                    _outline.OutlineWidth = _hoverSize;
                    break;

                case InteractableState.Select:
                    _outline.enabled = true;
                    _outline.OutlineColor = _selectColor;
                    _outline.OutlineWidth = _selectSize;
                    break;

                case InteractableState.Disabled:
                    _outline.enabled = false;
                    break;
            }
        }

        /// <summary>
        /// Updates the visual state of the interactable object based on its current state change arguments.
        /// </summary>
        /// <param name="args">The state change arguments for the interactable object.</param>
        private void UpdateVisualState(InteractableStateChangeArgs args) {
            UpdateVisual();
        }

        #region Inject
        public void InjectInteractableView(IInteractableView interactableView) {
            _interactableView = interactableView as MonoBehaviour;
            InteractableView = interactableView;
        }

        public void InjectOutline(Outline outline) {
            _outline = outline;
        }
        #endregion Inject
    }
}
