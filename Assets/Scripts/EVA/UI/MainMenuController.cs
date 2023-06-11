using System;
using UnityEngine;
using static OVRInput;

namespace Veery.UI {
    public class MainMenuController : MonoBehaviour {
        public static event Action Opened;
        public static event Action Closed;

        [SerializeField]
        private GameObject _MainMenuGO;

        public bool IsOpened { get; private set; }

        private void Start() {
            Close();
        }

        private void Update() {
            if (GetDown(Button.Start)) {
                if (IsOpened) {
                    Close();
                } else {
                    Open();
                }
            }
        }

        private void Open() {
            try {
                Debug.Log($"[{GetType().Name}] Opened event");
                IsOpened = true;
                _MainMenuGO.SetActive(true);
                Opened?.Invoke();
            } catch (Exception e) {
                Debug.LogError("Caught Exception: " + e);
            }
        }

        private void Close() {
            try {
                Debug.Log($"[{GetType().Name}] Closed event");
                IsOpened = false;
                _MainMenuGO.SetActive(false);
                Closed?.Invoke();
            } catch (Exception e) {
                Debug.LogError("Caught Exception: " + e);
            }
        }
    }
}
