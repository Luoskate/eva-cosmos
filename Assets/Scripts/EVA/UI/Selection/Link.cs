using System;
using EVA.Import.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EVA.UI.Selection {
    public class Link : SelectionProperty {
        [SerializeField]
        private Button _deleteButton;

        [SerializeField]
        private Button _editButton;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private GameObject _parametersPrefab;

        private TriggerLink _link;

        public TriggerLink Link_ {
            get => _link;

            set {
                _link = value;
                Debug.Log($"Link set to: {value.LinkedDelegate.Method.Name}");
                string[] tmp = value.LinkedDelegate.Method.Name.Split('.');
                tmp = (tmp.Length >= 2) ? new string[] { tmp[^2], tmp[^1] } : tmp;
                _text.text = $"{string.Join(".", tmp)}({string.Join(", ", value.Parameters.Keys)})";
                Debug.Log($"Link set to: {value.LinkedDelegate.Method.DeclaringType.Name}");
                // _text.text = $"{value.LinkedDelegate.Method.DeclaringType.Name}.{value.LinkedDelegate.Method.Name}()";
            }
        }

        public Links Parent { get; set; }
        public Delegate AddLink { get; set; }
        public Delegate RemoveLink { get; set; }
        public Delegate GetLinks { get; set; }

        public void Start() {
            _deleteButton.onClick.AddListener(Delete);
            _editButton.onClick.AddListener(Edit);
        }

        public void OnDestroy() {
            _deleteButton.onClick.RemoveListener(Delete);
            _editButton.onClick.RemoveListener(Edit);
        }

        public void Delete() {
            Debug.Log($"[{GetType().Name}] Delete()");
            RemoveLink.DynamicInvoke(Link_);
            Parent.ClearUI();
            Parent.SetUI();
        }

        public void Edit() {
            Debug.Log($"[{GetType().Name}] Edit()");
            Parameters parameters = Instantiate(_parametersPrefab, SelectionRootHandler.transform.parent)
                .GetComponent<Parameters>();
            parameters.SelectionRootHandler = SelectionRootHandler;
            parameters.Selection = Selection;
            parameters.Link_ = Link_;
            parameters.Parent = Parent;
            parameters.AddLink = AddLink;
            parameters.RemoveLink = RemoveLink;
            parameters.GetLinks = GetLinks;
            parameters.SetUI();
            Parent.gameObject.SetActive(false);
        }
    }
}
