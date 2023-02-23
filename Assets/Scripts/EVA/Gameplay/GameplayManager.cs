using System;
using UnityEngine;
using static OVRInput;

namespace EVA.Gameplay {
    public class GameplayManager : MonoBehaviour {
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

        public static GameplayManager Instance {
            get {
                if (_instance == null) {
                    _instance = FindObjectOfType<GameplayManager>();
                }

                return _instance;
            }
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
