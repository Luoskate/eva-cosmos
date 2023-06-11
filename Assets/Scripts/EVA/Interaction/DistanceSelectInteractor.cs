using Oculus.Interaction;
using UnityEngine;

namespace Veery.Interaction {
    public class DistanceSelectInteractor :
        PointerInteractor<DistanceSelectInteractor, DistanceSelectInteractable>,
        IDistanceInteractor {
        [SerializeField]
        [Interface(typeof(IInteractor))]
        private MonoBehaviour _canvasInteractor;
        [SerializeField]
        [Interface(typeof(ISelector))]
        private MonoBehaviour _selector;

        [SerializeField]
        [Optional]
        private Transform _selectCenter;

        [SerializeField]
        [Optional]
        private Transform _selectTarget;

        [SerializeField]
        private DistantCandidateComputer<DistanceSelectInteractable> _distantCandidateComputer = new();

        public IInteractor CanvasInteractor { get; private set; }
        public Pose Origin => _distantCandidateComputer.Origin;
        public Vector3 HitPoint { get; private set; }
        public IRelativeToRef DistanceInteractable => Interactable;
        public AdvancedControllerSelector AdvancedSelector { get; private set; }

        protected override void Awake() {
            base.Awake();
            CanvasInteractor = _canvasInteractor as IInteractor;
            Selector = _selector as ISelector;
            if (_selector is AdvancedControllerSelector) {
                AdvancedSelector = _selector as AdvancedControllerSelector;
            }
        }

        protected override void Start() {
            this.BeginStart(ref _started, () => base.Start());
            this.AssertField(Selector, nameof(Selector));
            this.AssertField(_distantCandidateComputer, nameof(_distantCandidateComputer));

            if (_selectCenter == null) {
                _selectCenter = transform;
            }

            if (_selectTarget == null) {
                _selectTarget = _selectCenter;
            }

            this.EndStart(ref _started);
        }

        protected override void DoPreprocess() {
            transform.SetPositionAndRotation(_selectCenter.position, _selectCenter.rotation);
        }

        protected override DistanceSelectInteractable ComputeCandidate() {
            DistanceSelectInteractable bestCandidate = _distantCandidateComputer.ComputeCandidate(
                () => DistanceSelectInteractable.Registry.List(this),
                out Vector3 hitPoint);
            HitPoint = hitPoint;
            return bestCandidate;
        }

        protected override void HandleSelected() {
            if ((AdvancedSelector != null && AdvancedSelector.LateSelection && _interactable == null)
                || (CanvasInteractor != null && CanvasInteractor.HasCandidate)) {
                return;
            }

            base.HandleSelected();
        }

        protected override void HandleUnselected() {
            if (CanvasInteractor != null && CanvasInteractor.HasCandidate) {
                return;
            }

            base.HandleUnselected();
        }

        protected override Pose ComputePointerPose() {
            return (SelectedInteractable != null) ? Pose.identity : _selectTarget.GetPose();
        }

        #region Inject
        public void InjectAllDistanceGrabInteractor(
            ISelector selector,
            DistantCandidateComputer<DistanceSelectInteractable> distantCandidateComputer) {
            InjectSelector(selector);
            InjectDistantCandidateComputer(distantCandidateComputer);
        }

        public void InjectSelector(ISelector selector) {
            _selector = selector as MonoBehaviour;
            Selector = selector;
        }

        public void InjectDistantCandidateComputer(
            DistantCandidateComputer<DistanceSelectInteractable> distantCandidateComputer) {
            _distantCandidateComputer = distantCandidateComputer;
        }

        public void InjectOptionalGrabCenter(Transform grabCenter) {
            _selectCenter = grabCenter;
        }

        public void InjectOptionalGrabTarget(Transform grabTarget) {
            _selectTarget = grabTarget;
        }
        #endregion Inject
    }
}
