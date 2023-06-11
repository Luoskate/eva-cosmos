using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Veery.Import.Triggers;

namespace Veery.UI.Selection {
    public class BooleanParameter : Parameter {
        [SerializeField]
        private Toggle _toggle;

        [SerializeField]
        private TextMeshProUGUI _name;

        public override void SetUI() {
            Debug.Log($"{Link_}");
            Debug.Log($"{Link_.Parameters}");
            Debug.Log($"{Link_.Parameters[Name]}");
            Debug.Log($"{Link_.Parameters[Name].Item1}");
            Debug.Log($"{Link_.Parameters[Name].Item2}");

            if (Link_.Parameters[Name].Item2 != null) {
                _toggle.isOn = (bool)Link_.Parameters[Name].Item2;
            } else {
                _toggle.isOn = false;
            }

            _toggle.onValueChanged.AddListener(Toggle);
            _name.text = Name;
        }

        public void OnDestroy() {
            _toggle.onValueChanged.RemoveListener(Toggle);
        }

        public void Toggle(bool value) {
            Debug.Log($"Toggle {Name} to {value}");
            List<TriggerLink> links = (GetLinks.DynamicInvoke() as List<TriggerLink>);
            TriggerLink originalLink = links.Find(
                link => link.LinkedDelegate == Link_.LinkedDelegate);
            if (originalLink == null) {
                Debug.LogError($"[{GetType().Name}] originalLink is null");
                return;
            }

            originalLink.Parameters[Name] = new(Link_.Parameters[Name].Item1, value);
        }
    }
}
