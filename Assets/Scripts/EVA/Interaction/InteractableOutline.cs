using Oculus.Interaction;
using UnityEngine;

namespace EVA.Interaction {
    public class InteractableOutline : MonoBehaviour {
        [SerializeField]
        [Interface(typeof(IInteractableView))]
        private MonoBehaviour _interactableView;

        [SerializeField]
        [Optional]
        private Outline _outline;

        [SerializeField]
        private Color _hoverColor = Color.white;

        [SerializeField]
        private float _hoverSize = 2;

        [SerializeField]
        private Color _selectColor = new(0, 0.2901961f, 0.7254902f);

        [SerializeField]
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
