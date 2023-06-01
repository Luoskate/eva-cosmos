using System;
using System.Reflection;
using EVA.Import;
using EVA.Import.Triggers;
using EVA.Interaction;
using Oculus.Interaction;
using UnityEngine;
using UnityEngine.UI;

namespace EVA.UI.Selection {
    public class Methods : SelectionProperty {
        [SerializeField]
        private Button _backButton;

        [SerializeField]
        private GameObject _contentGO;

        [SerializeField]
        private Method _methodPrefab;

        public Links Parent { get; set; }
        public Delegate AddLink { get; set; }
        public Delegate RemoveLink { get; set; }
        public Delegate GetLinks { get; set; }

        #region Methods
        public void Start() {
            _backButton.onClick.AddListener(Back);
            SelectionRootHandler.DistanceSelectInteractor2.WhenStateChanged += OnStateChanged;
        }

        public void OnDestroy() {
            ClearUI();
            _backButton.onClick.RemoveListener(Back);
            SelectionRootHandler.DistanceSelectInteractor2.WhenStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(InteractorStateChangeArgs args) {
            Debug.Log($"[{GetType().Name}] OnStateChanged({args.NewState})");
            SecondarySelection = null;
            if (args.NewState != InteractorState.Select) {
                ClearUI();
                return;
            }

            SetUI();
        }

        public void SetUI() {
            Debug.Log($"[{GetType().Name}] SetUI()");

            DistanceSelectInteractable interactable = SelectionRootHandler.DistanceSelectInteractor2.Interactable;
            if (interactable == null) {
                return;
            }

            SecondarySelection = interactable.gameObject.GetComponentInParent<ImportObject>();

            if (SecondarySelection == null) {
                return;
            }

            foreach (Type interface_ in SecondarySelection.GetType().GetInterfaces()) {
                Debug.Log($"[{GetType().Name}] SetUI() | {interface_}");
                foreach (MethodInfo methodInfo in interface_.GetMethods()) {
                    Debug.Log($"[{GetType().Name}] SetUI() | {methodInfo}");

                    ParameterInfo[] parameters = methodInfo.GetParameters();
                    Type[] paramTypes = Array.ConvertAll(parameters, p => p.ParameterType);
                    if (methodInfo.ReturnType != typeof(void)) {
                        continue;
                    }

                    Delegate actionDelegate = null;
                    // if no param
                    if (paramTypes.Length == 0) {
                        actionDelegate = Delegate.CreateDelegate(typeof(Action), SecondarySelection, methodInfo);
                    } else {
                        Type delegateType = typeof(Action<>).MakeGenericType(paramTypes);
                        actionDelegate = Delegate.CreateDelegate(delegateType, SecondarySelection, methodInfo);
                    }

                    Method method = Instantiate(_methodPrefab, _contentGO.transform)
                        .GetComponent<Method>();
                    method.Parameters_ = parameters;
                    method.Delegate_ = actionDelegate;
                    method.Parent = this;
                    method.Selection = Selection;
                    method.SecondarySelection = SecondarySelection;
                    method.SelectionRootHandler = SelectionRootHandler;
                    method.AddLink = AddLink;
                    method.RemoveLink = RemoveLink;
                    method.GetLinks = GetLinks;
                    method.SetText();
                }
            }
        }

        public void ClearUI() {
            Debug.Log($"[{GetType().Name}] ClearUI()");
            foreach (Transform child in _contentGO.transform) {
                Destroy(child.gameObject);
            }
        }

        public void Back() {
            Debug.Log($"[{GetType().Name}] Back()");
            Parent.ClearUI();
            Parent.SetUI();
            Parent.gameObject.SetActive(true);
            Destroy(gameObject);
        }
        #endregion Methods
    }
}
