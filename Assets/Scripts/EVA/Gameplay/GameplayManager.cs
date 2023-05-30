using System;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;
using static OVRInput;

namespace EVA.Gameplay {
    public class GameplayManager : MonoBehaviour {
        [SerializeField]
        [Interface(typeof(IInteractor))]
        private MonoBehaviour _leftControllerGrabInteractor;

        [SerializeField]
        [Interface(typeof(IInteractor))]
        private MonoBehaviour _rightControllerGrabInteractor;

        public static event Action ControllerChanged;
        public static event Action HandednessChanged;

        private static GameplayManager _instance;
        private Controller _currentController;
        private Handedness _currentHandedness;

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

        public List<IInteractor> GrabInteractors { get; private set; }

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
            if (_currentController != GetActiveController()) {
                try {
                    Debug.Log($"[{GetType().Name}] ControllerChanged event");
                    _currentController = GetActiveController();
                    ControllerChanged?.Invoke();
                } catch (Exception e) {
                    Debug.LogError("Caught Exception: " + e);
                }
            }

            if (_currentHandedness != GetDominantHand()) {
                DominantHand = GetDominantHand();
            }
        }
    }
}
