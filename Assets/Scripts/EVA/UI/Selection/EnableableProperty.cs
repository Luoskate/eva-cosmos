using EVA.Import.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace EVA.UI.Selection {
    public class EnableableProperty : SelectionProperty {
        #region Serialized Fields
        [SerializeField]
        private Toggle _toggle;
        #endregion Serialized Fields

        #region Methods
        public void Start() {
            _toggle.onValueChanged.AddListener(Toggle);
            _toggle.isOn = ((IEnableable)Selection).IsEnabled();
        }

        public void OnDestroy() {
            _toggle.onValueChanged.RemoveListener(Toggle);
        }

        public void Toggle(bool value) {
            ((IEnableable)Selection).Toggle(value);
        }
        #endregion Methods
    }
}
