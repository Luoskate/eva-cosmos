using System;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using static OVRInput;

namespace Veery.Gameplay {
    /// <summary>
    /// Manages gameplay-related functionality, such as controller input and interactor management.
    /// </summary>
    public class GameplayManager : MonoBehaviour {
        [SerializeField]
        [Interface(typeof(IInteractor))]
        [Tooltip("The grab interactor on the left controller.")]
        /// <summary>
        /// The grab interactor on the left controller.
        /// </summary>
        private MonoBehaviour _leftControllerGrabInteractor;

        [SerializeField]
        [Interface(typeof(IInteractor))]
        [Tooltip("The grab interactor on the right controller.")]
        /// <summary>
        /// The grab interactor on the right controller.
        /// </summary>
        private MonoBehaviour _rightControllerGrabInteractor;

        /// <summary>
        /// Event that is triggered when the active controller changes.
        /// </summary>
        public static event Action ControllerChanged;

        /// <summary>
        /// Event that is triggered when the dominant hand changes.
        /// </summary>
        public static event Action HandednessChanged;

        private static GameplayManager _instance;
        private Controller _currentController;
        private Handedness _currentHandedness;

        /// <summary>
        /// Gets or sets the dominant hand for the player.
        /// </summary>
        public Handedness DominantHand {
            get => _currentHandedness;

            set {
                try {
                    Debug.Log($"[{GetType().Name}] HandednessChanged event");
                    _currentHandedness = value;
                    HandednessChanged?.Invoke();
                } catch (Exception e) {
                    Debug.LogError("Caught Exception: " + e);
                }
            }
        }

        /// <summary>
        /// The list of grab interactors on the controllers.
        /// </summary>
        public List<IInteractor> GrabInteractors { get; private set; }

        /// <summary>
        /// The singleton instance of the <see cref="GameplayManager"/> class.
        /// </summary>
        public static GameplayManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<GameplayManager>();
                }

                return _instance;
            }
        }

        private void Start() {
            GrabInteractors = new() {
                _leftControllerGrabInteractor as IInteractor,
                _rightControllerGrabInteractor as IInteractor
            };
        }

        private void Update() {
            HandleActiveController();
            HandleDominantHand();
        }

        /// <summary>
        /// Manages the dominant hand and triggers the <see cref="HandednessChanged"/> event when the dominant hand changes.
        /// </summary>
        private void HandleDominantHand() {
            if (_currentHandedness == GetDominantHand()) {
                return;
            }

            DominantHand = GetDominantHand();
        }

        /// <summary>
        /// Manages the active controller and triggers the <see cref="ControllerChanged"/> event when the active controller changes.
        /// </summary>
        private void HandleActiveController() {
            if (_currentController == GetActiveController()) {
                return;
            }

            try {
                Debug.Log($"[{GetType().Name}] ControllerChanged event");
                _currentController = GetActiveController();
                ControllerChanged?.Invoke();
            } catch (Exception e) {
                Debug.LogError("Caught Exception: " + e);
            }
        }
    }
}
