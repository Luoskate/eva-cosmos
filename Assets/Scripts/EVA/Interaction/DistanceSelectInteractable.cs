using Oculus.Interaction;
using UnityEngine;

namespace Veery.Interaction {
    /// <summary>
    /// Allows the user to select and interact with this object from a distance using the DistanceSelectInteractor.
    /// </summary>
    public class DistanceSelectInteractable :
        PointerInteractable<DistanceSelectInteractor, DistanceSelectInteractable>,
        IRigidbodyRef,
        IRelativeToRef,
        ICollidersRef {
        [SerializeField]
        [Tooltip("The Rigidbody component of the object.")]
        /// <summary>
        /// The Rigidbody component of the object.
        /// </summary>
        private Rigidbody _rigidbody;

        [SerializeField]
        [Optional]
        [Tooltip("The Transform to use as the relative transform for the DistanceSelectInteractor.")]
        /// <summary>
        /// The Transform to use as the relative transform for the DistanceSelectInteractor. If not set, the Rigidbody transform will be used.
        /// </summary>
        private Transform _grabSource;

        #region Properties
        /// <summary>
        /// The colliders attached to the Rigidbody of this object.
        /// </summary>
        public Collider[] Colliders { get; private set; }

        /// <summary>
        /// The Rigidbody component of the object.
        /// </summary>
        public Rigidbody Rigidbody => _rigidbody;

        /// <summary>
        /// The Transform to use as the relative transform for the DistanceSelectInteractor.
        /// </summary>
        public Transform RelativeTo => _grabSource;

        #endregion Properties

        #region Editor Events
        /// <summary>
        /// This method is called when the script is first attached to an object and when the Reset command is used in the Inspector's context menu.
        /// It sets the Rigidbody component of the object to the first Rigidbody component found in the parent hierarchy.
        /// </summary>
        protected virtual void Reset() {
            _rigidbody = GetComponentInParent<Rigidbody>();
        }
        #endregion Editor Events

        protected override void Awake() {
            base.Awake();
        }

        protected override void Start() {
            this.BeginStart(ref _started, () => base.Start());
            this.AssertField(Rigidbody, nameof(Rigidbody));
            Colliders = Rigidbody.GetComponentsInChildren<Collider>();

            if (_grabSource == null) {
                _grabSource = Rigidbody.transform;
            }

            this.EndStart(ref _started);
        }
    }
}
