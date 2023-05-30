using System;
using System.Collections.Generic;
using EVA.Import.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace EVA.UI.Selection {
    public class Parameters : SelectionProperty {
        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private GameObject _contentGO;

        public Links Parent { get; set; }
        public TriggerLink Link_ { get; set; }
        public Delegate AddLink { get; set; }
        public Delegate RemoveLink { get; set; }
        public Delegate GetLinks { get; set; }


        public void Start() {
            _backButton.onClick.AddListener(Back);
        }

        public void OnDestroy() {
            _backButton.onClick.RemoveListener(Back);
        }

        public void Back() {
            Debug.Log($"[{GetType().Name}] Back()");
            Parent.ClearUI();
            Parent.SetUI();
            Parent.gameObject.SetActive(true);
            Destroy(gameObject);
        }

        public void SetUI() {
            Debug.Log($"[{GetType().Name}] SetUI()");
            foreach (KeyValuePair<string, Tuple<Type, object>> param in Link_.Parameters) {
                string paramName = param.Key;
                Type paramType = param.Value.Item1;

                // debug paramType
                object paramValue = param.Value.Item2;
                Debug.Log($"[{GetType().Name}] SetUI() paramName: {paramName}, paramType: {paramType}, paramValue: {paramValue}");

                if (SelectionRootHandler.ParameterPrefabs.TryGetValue(paramType, out GameObject parameterPrefab)) {
                    Parameter parameter = Instantiate(parameterPrefab, _contentGO.transform)
                        .GetComponent<Parameter>();
                    parameter.SelectionRootHandler = SelectionRootHandler;
                    parameter.Selection = Selection;
                    parameter.SecondarySelection = SecondarySelection;
                    parameter.Name = paramName;
                    parameter.Link_ = Link_;
                    parameter.AddLink = AddLink;
                    parameter.RemoveLink = RemoveLink;
                    parameter.GetLinks = GetLinks;
                    parameter.SetUI();
                }
            }
        }
    }
}
