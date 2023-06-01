using System;
using System.Collections.Generic;
using EVA.Import;
using EVA.Import.Triggers.Triggers;
using EVA.Interaction;
using Oculus.Interaction;
using UnityEngine;

namespace EVA.UI.Selection {
    public class SelectionProperties : MonoBehaviour {
        [Serializable]
        private struct SelectionPrefab {
            [SerializeField]
            public string _interfaceName;

            [SerializeField]
            public GameObject _prefab;
        }

        [Serializable]
        private struct ParameterPrefab {
            [SerializeField]
            public string _typeName;

            [SerializeField]
            public GameObject _prefab;
        }

        #region Serialized Fields
        [SerializeField]
        private SelectionPrefab[] _PropertyPrefabs;

        [SerializeField]
        private ParameterPrefab[] _ParameterPrefabs;

        [SerializeField]
        private GameObject _contentGO;

        [SerializeField]
        private DistanceSelectInteractor _distanceSelectInteractor1;

        [SerializeField]
        private DistanceSelectInteractor _distanceSelectInteractor2;
        #endregion Serialized Fields

        #region Properties
        public GameObject ContentGO => _contentGO;
        public DistanceSelectInteractor DistanceSelectInteractor2 => _distanceSelectInteractor2;
        public Dictionary<string, GameObject> PropertyPrefabs { get; } = new();
        public Dictionary<Type, GameObject> ParameterPrefabs { get; } = new();
        #endregion Properties

        #region Methods
        public void Start() {
            foreach (SelectionPrefab prefab in _PropertyPrefabs) {
                PropertyPrefabs.Add(prefab._interfaceName, prefab._prefab);
            }

            foreach (ParameterPrefab prefab in _ParameterPrefabs) {
                // debug type
                Debug.Log($"[{GetType().Name}] Start() | {prefab._typeName} | {Type.GetType(prefab._typeName)}");
                ParameterPrefabs.Add(Type.GetType(prefab._typeName), prefab._prefab);
            }

            _distanceSelectInteractor1.WhenStateChanged += OnStateChanged;
            Debug.Log($"[{GetType().Name}] Start() | Subscribed to {_distanceSelectInteractor1.name}");
        }

        private void OnStateChanged(InteractorStateChangeArgs args) {
            Debug.Log($"[{GetType().Name}] OnStateChanged({args.NewState})");
            if (args.NewState != InteractorState.Select) {
                ClearUI();
                // FF mental
                Links links = transform.parent.gameObject.GetComponentInChildren<Links>();
                if (links != null) {
                    Destroy(links.gameObject);
                }

                Methods methods = transform.parent.gameObject.GetComponentInChildren<Methods>();
                if (methods != null) {
                    Destroy(methods.gameObject);
                }

                Parameters parameters = transform.parent.gameObject.GetComponentInChildren<Parameters>();
                if (parameters != null) {
                    Destroy(parameters.gameObject);
                }

                return;
            }

            DistanceSelectInteractable interactable = _distanceSelectInteractor1.Interactable;
            if (interactable == null) {
                return;
            }

            ImportObject selection = interactable.gameObject.GetComponentInParent<ImportObject>();

            // FIX tmp
            // if (OVRInput.Get(OVRInput.Button.SecondaryThumbstick)) {
            //     Debug.Log($"[{GetType().Name}] OnStateChanged() | PrimaryThumbstick");
            //     DistanceSelectInteractable obj = _distanceSelectInteractor2.Interactable;
            //     if (obj == null) return;
            //     ImportObject objsel = obj.gameObject.GetComponentInParent<ImportObject>();
            //     if (objsel == null) return;
            //     if (selection is not AreaTrigger) return;
            //     AreaTrigger trigger = (AreaTrigger)selection;
            //     trigger.WhenTriggered += () => objsel.ToggleEnabled(false);
            //     trigger.WhenExited += () => objsel.ToggleEnabled(true);
            // }
            // tmp

            SetUI(selection);
        }

        public void SetUI(ImportObject selection) {
            Debug.Log($"[{GetType().Name}] SetUI({selection})");
            foreach (Type interface_ in selection.GetType().GetInterfaces()) {
                if (PropertyPrefabs.TryGetValue(interface_.Name, out GameObject prefab)) {
                    SelectionProperty selectionProperty = Instantiate(prefab, _contentGO.transform)
                        .GetComponent<SelectionProperty>();
                    selectionProperty.Selection = selection;
                    selectionProperty.SelectionRootHandler = this;
                }
            }
        }

        public void ClearUI() {
            Debug.Log($"[{GetType().Name}] ClearUI()");
            foreach (Transform child in _contentGO.transform) {
                Destroy(child.gameObject);
            }
        }
        #endregion Methods
    }
}
