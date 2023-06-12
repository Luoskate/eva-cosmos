using Oculus.Interaction;
using UnityEngine;

namespace Veery.Interaction {
    /// <summary>
    /// An interactor that selects interactables based on their distance from the interactor's pointer.
    /// </summary>
    public class DistanceSelectInteractor :
        PointerInteractor<DistanceSelectInteractor, DistanceSelectInteractable>,
        IDistanceInteractor {
        [SerializeField]
        [Interface(typeof(IInteractor))]
        /// <summary>
        /// The MonoBehaviour that represents the canvas interactor.
        /// </summary>
        private MonoBehaviour _canvasInteractor;

        [SerializeField]
        [Interface(typeof(ISelector))]
        [Tooltip("The selector to use for selection.")]
        /// <summary>
        /// The MonoBehaviour that represents the selector to use for selection.
        /// </summary>
        private MonoBehaviour _selector;

        [SerializeField]
        [Optional]
        [Tooltip("The transform to use as the center of the selection sphere.")]
        /// <summary>
        /// The transform to use as the center of the selection sphere.
        /// </summary>
        private Transform _selectCenter;

        [SerializeField]
        [Optional]
        [Tooltip("The transform to use as the target of the selection sphere.")]
        /// <summary>
        /// The transform to use as the target of the selection sphere.
        /// </summary>
        private Transform _selectTarget;

        [SerializeField]
        /// <summary>
        /// The distant candidate computer used to compute the best candidate for selection based on distance.
        /// </summary>
        private DistantCandidateComputer<DistanceSelectInteractable> _distantCandidateComputer = new();

        /// <summary>
        /// The MonoBehaviour that represents the canvas interactor.
        /// </summary>
        public IInteractor CanvasInteractor { get; private set; }

        /// <summary>
        /// Gets the pose of the origin of the interactor.
        /// </summary>
        /// <remarks>
        /// The origin is the position and rotation of the interactor's pointer.
        /// </remarks>
        public Pose Origin => _distantCandidateComputer.Origin;

        /// <summary>
        /// Gets the point in world space where the interactor's pointer hit the selected object.
        /// </summary>
        public Vector3 HitPoint { get; private set; }

        /// <summary>
        /// Gets the interactable that is currently selected by this distance interactor.
        /// </summary>
        public IRelativeToRef DistanceInteractable => Interactable;

        /// <summary>
        /// Gets the advanced controller selector used for selection, if available.
        /// </summary>
        /// <remarks>
        /// This property is only set if the selector used for selection is an instance of <see cref="AdvancedControllerSelector"/>.
        /// </remarks>
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

        /// <summary>
        /// Sets the position and rotation of the interactor to match the position and rotation of the selection center.
        /// </summary>
        protected override void DoPreprocess() {
            transform.SetPositionAndRotation(_selectCenter.position, _selectCenter.rotation);
        }

        /// <summary>
        /// Computes the best candidate for selection based on distance.
        /// </summary>
        /// <returns>The best candidate for selection based on distance.</returns>
        protected override DistanceSelectInteractable ComputeCandidate() {
            DistanceSelectInteractable bestCandidate = _distantCandidateComputer.ComputeCandidate(
                () => DistanceSelectInteractable.Registry.List(this),
                out Vector3 hitPoint);
            HitPoint = hitPoint;
            return bestCandidate;
        }

        /// <summary>
        /// Handles the selection of an interactable by this distance interactor.
        /// </summary>
        /// <remarks>
        /// If the advanced controller selector is set to late selection and no interactable is currently selected, or if the canvas interactor has a candidate, the selection is not handled.
        /// </remarks>
        protected override void HandleSelected() {
            if ((AdvancedSelector != null && AdvancedSelector.LateSelection && _interactable == null)
                || (CanvasInteractor != null && CanvasInteractor.HasCandidate)) {
                return;
            }

            base.HandleSelected();
        }

        /// <summary>
        /// Handles the deselection of an interactable by this distance interactor.
        /// </summary>
        /// <remarks>
        /// If the canvas interactor has a candidate, the deselection is not handled.
        /// </remarks>
        protected override void HandleUnselected() {
            if (CanvasInteractor != null && CanvasInteractor.HasCandidate) {
                return;
            }

            base.HandleUnselected();
        }

        /// <summary>
        /// Computes the pose of the interactor's pointer.
        /// </summary>
        /// <remarks>
        /// If an interactable is currently selected, the pointer pose is set to identity.
        /// Otherwise, the pointer pose is set to the pose of the selection target.
        /// </remarks>
        /// <returns>The pose of the interactor's pointer.</returns>
        protected override Pose ComputePointerPose() {
            return (SelectedInteractable != null) ? Pose.identity : _selectTarget.GetPose();
        }
    }
}
