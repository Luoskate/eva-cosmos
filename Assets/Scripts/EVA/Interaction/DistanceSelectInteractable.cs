using Oculus.Interaction;
using UnityEngine;

namespace EVA.Interaction {
    public class DistanceSelectInteractable :
        PointerInteractable<DistanceSelectInteractor, DistanceSelectInteractable>,
        IRigidbodyRef,
        IRelativeToRef,
        ICollidersRef {
        [SerializeField]
        private Rigidbody _rigidbody;

        [SerializeField]
        [Optional]
        private Transform _grabSource;

        #region Properties
        public Collider[] Colliders { get; private set; }
        public Rigidbody Rigidbody => _rigidbody;
        public Transform RelativeTo => _grabSource;

        #endregion Properties

        #region Editor Events
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

        #region Inject

        public void InjectAllGrabInteractable(Rigidbody rigidbody) {
            InjectRigidbody(rigidbody);
        }

        public void InjectRigidbody(Rigidbody rigidbody) {
            _rigidbody = rigidbody;
        }

        public void InjectOptionalGrabSource(Transform grabSource) {
            _grabSource = grabSource;
        }
        #endregion Inject
    }
}
