using System;
using System.Collections.Generic;
using System.Reflection;
using EVA.Import.Triggers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EVA.UI.Selection {
    public class Method : SelectionProperty {
        [SerializeField]
        private Button _selectButton;

        [SerializeField]
        private TextMeshProUGUI _text;

        [SerializeField]
        private GameObject _parametersPrefab;

        [SerializeField]
        private GameObject _triggerLinkPrefab;

        public Methods Parent { get; set; }
        public Delegate Delegate_ { get; set; }
        public ParameterInfo[] Parameters_ { get; set; }
        public Delegate AddLink { get; set; }
        public Delegate RemoveLink { get; set; }
        public Delegate GetLinks { get; set; }

        public void Start() {
            _selectButton.onClick.AddListener(Select);
        }

        public void OnDestroy() {
            _selectButton.onClick.RemoveListener(Select);
        }

        public void SetText() {
            Debug.Log($"[{GetType().Name}] SetUI()");
            string[] parameterNames = Array.ConvertAll(Parameters_, p => p.Name);
            // get the name of the method and interface to avoid collisions (e.g. ITriggerable.Toggle())
            string[] tmp = Delegate_.Method.Name.Split('.');
            tmp = (tmp.Length >= 2) ? new string[] { tmp[^2], tmp[^1] } : tmp;
            _text.text = $"{string.Join(".", tmp)}({string.Join(", ", parameterNames)})";
        }

        public void Select() {
            Debug.Log($"[{GetType().Name}] Select()");

            Dictionary<string, Tuple<Type, object>> formatedParameters = new();
            foreach (ParameterInfo parameterInfo in Parameters_) {
                formatedParameters.Add(parameterInfo.Name, new Tuple<Type, object>(parameterInfo.ParameterType, null));
            }



            TriggerLink link = Instantiate(_triggerLinkPrefab, SelectionRootHandler.transform.parent)
                .GetComponent<TriggerLink>();
            link.LinkedDelegate = Delegate_;
            link.Parameters = formatedParameters;
            link.Trigger = Selection;
            link.LinkedObject = SecondarySelection;
            link.SetColor(UnityEngine.Random.ColorHSV());
            _ = AddLink.DynamicInvoke(link);

            Parameters parameters = Instantiate(_parametersPrefab, SelectionRootHandler.transform.parent)
                .GetComponent<Parameters>();
            parameters.SelectionRootHandler = SelectionRootHandler;
            parameters.Selection = Selection;
            parameters.SecondarySelection = SecondarySelection;
            parameters.Link_ = link;
            parameters.Parent = Parent.Parent;
            parameters.AddLink = AddLink;
            parameters.RemoveLink = RemoveLink;
            parameters.GetLinks = GetLinks;
            parameters.SetUI();
            Destroy(Parent.gameObject);
        }
    }
}
