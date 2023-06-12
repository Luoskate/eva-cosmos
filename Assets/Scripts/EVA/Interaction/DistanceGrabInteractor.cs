using Oculus.Interaction;
using UnityEngine;

namespace Veery.Interaction {
    /// <summary>
    /// A custom implementation of the Oculus Interaction DistanceGrabInteractor that allows for optional selection of interactables using a DistanceSelectInteractor.
    /// </summary>
    public class DistanceGrabInteractor : Oculus.Interaction.DistanceGrabInteractor {
        [SerializeField]
        [Optional]
        [Tooltip("The DistanceSelectInteractor to use for selection.")]
        /// <summary>
        /// The DistanceSelectInteractor to use for optional selection of interactables. If not set, interactables will only be selectable by grabbing.
        /// </summary>
        private DistanceSelectInteractor _distanceSelectInteractor;

        private bool _alreadySelected;

        /// <summary>
        /// Called when an interactable is selected by this interactor. If a DistanceSelectInteractor is set and in the Select state, the interactable will be unselected from the DistanceSelectInteractor. Otherwise, the interactable will be selected by the DistanceSelectInteractor if it is set.
        /// </summary>
        /// <param name="interactable">The interactable that was selected.</param>
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

        /// <summary>
        /// Called when an interactable is unselected by this interactor. If a DistanceSelectInteractor is set and the interactable was not already selected by this interactor, the interactable will be unselected from the DistanceSelectInteractor.
        /// </summary>
        /// <param name="interactable">The interactable that was unselected.</param>
        protected override void InteractableUnselected(DistanceGrabInteractable interactable) {
            base.InteractableUnselected(interactable);
            if (_distanceSelectInteractor == null || _alreadySelected) {
                return;
            }

            _distanceSelectInteractor.Unselect();
        }
    }
}
