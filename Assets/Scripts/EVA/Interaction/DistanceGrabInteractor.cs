using Oculus.Interaction;
using UnityEngine;

namespace Veery.Interaction {
    public class DistanceGrabInteractor : Oculus.Interaction.DistanceGrabInteractor {
        [SerializeField]
        [Optional]
        private DistanceSelectInteractor _distanceSelectInteractor;

        private bool _alreadySelected;

        protected override void InteractableSelected(DistanceGrabInteractable interactable) {
            if (_distanceSelectInteractor != null && _distanceSelectInteractor.State == InteractorState.Select) {
                _alreadySelected = true;
                _distanceSelectInteractor.Unselect();
            } else {
                _alreadySelected = false;
            }

            base.InteractableSelected(interactable);
            if (_distanceSelectInteractor == null) {
                return;
            }

            _distanceSelectInteractor.Select();
        }

        protected override void InteractableUnselected(DistanceGrabInteractable interactable) {
            base.InteractableUnselected(interactable);
            if (_distanceSelectInteractor != null && !_alreadySelected) {
                _distanceSelectInteractor.Unselect();
            }
        }

        #region Inject

        public void InjectOptionalDistanceSelectInteractor(DistanceSelectInteractor distanceSelectInteractor) {
            _distanceSelectInteractor = distanceSelectInteractor;
        }
        #endregion Inject
    }
}
