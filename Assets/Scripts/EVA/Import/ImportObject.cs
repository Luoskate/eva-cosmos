using System.Collections.Generic;
using EVA.Import.Properties;
using UnityEngine;

namespace EVA.Import {
    public class ImportObject : MonoBehaviour, IEnableable {
        #region Serialized Fields
        [SerializeField]
        private List<GameObject> _deferredGOs = new();
        #endregion Serialized Fields

        #region Methods
        public void LoadDeferredGOs() {
            // Activate all deferred game objects
            foreach (GameObject go in _deferredGOs) {
                go.SetActive(true);
            }
        }

        void IEnableable.Toggle(bool value) {
            Debug.Log($"[{GetType().Name}] Toggle({value})");
            gameObject.SetActive(value);
        }

        bool IEnableable.IsEnabled() {
            return gameObject.activeSelf;
        }
        #endregion Methods
    }
}
