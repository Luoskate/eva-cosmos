using System;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

namespace EVA.Interaction {
    public class AdvancedControllerSelector : MonoBehaviour, ISelector {
        public enum ControllerSelectorLogicOperator {
            Any = 0,
            All = 1
        }

        public enum ControllerSelectorHoldLogic {
            Hold = 0,
            SmartToggle = 1,
            Toggle = 2
        }

        [SerializeField]
        [Interface(typeof(IController))]
        private MonoBehaviour _controller;

        [SerializeField]
        private ControllerButtonUsage _controllerButtonUsage;

        [SerializeField]
        private ControllerSelectorHoldLogic _controllerHoldLogic;

        [SerializeField]
        private ControllerSelectorLogicOperator _requireButtonUsages
            = ControllerSelectorLogicOperator.Any;

        #region Properties
        public ControllerButtonUsage ControllerButtonUsage {
            get => _controllerButtonUsage;

            set => _controllerButtonUsage = value;
        }

        public ControllerSelectorHoldLogic ControllerHoldLogic {
            get => _controllerHoldLogic;

            set => _controllerHoldLogic = value;
        }

        public ControllerSelectorLogicOperator RequireButtonUsages {
            get => _requireButtonUsages;

            set => _requireButtonUsages = value;
        }
        #endregion Properties

        public IController Controller { get; private set; }

        public event Action WhenSelected = delegate { };
        public event Action WhenUnselected = delegate { };

        private bool _prevPressed;
        private bool _selected;

        public bool LateSelection { get; private set; }

        protected virtual void Awake() {
            Controller = _controller as IController;
        }

        protected virtual void Start() {
            this.AssertField(Controller, nameof(Controller));
        }

        protected virtual void Update() {
            if (LateSelection) {
                WhenSelected();
                LateSelection = false;
                return;
            }

            bool pressed = (_requireButtonUsages == ControllerSelectorLogicOperator.All)
                ? Controller.IsButtonUsageAllActive(_controllerButtonUsage)
                : Controller.IsButtonUsageAnyActive(_controllerButtonUsage);

            if (pressed == _prevPressed) {
                return;
            }

            switch (_controllerHoldLogic) {
                case ControllerSelectorHoldLogic.Hold:
                    HandleHoldLogic(pressed);
                    break;

                case ControllerSelectorHoldLogic.SmartToggle:
                    HandleSmartToggleLogic(pressed);
                    break;

                case ControllerSelectorHoldLogic.Toggle:
                    HandleToggleLogic(pressed);
                    break;

                default:
                    throw new NotImplementedException($"Unsupported hold logic: {_controllerHoldLogic}");
            }

            _prevPressed = pressed;
        }

        private void HandleHoldLogic(bool pressed) {
            if (pressed) {
                WhenSelected();
            } else {
                WhenUnselected();
            }
        }

        private void HandleSmartToggleLogic(bool pressed) {
            if (!pressed) {
                return;
            }

            WhenUnselected();
            LateSelection = true;
        }

        private void HandleToggleLogic(bool pressed) {
            if (!pressed) {
                return;
            }

            _selected = !_selected;
            if (_selected) {
                WhenSelected();
            } else {
                WhenUnselected();
            }
        }

        #region Inject
        public void InjectAllControllerSelector(IController controller) {
            InjectController(controller);
        }

        public void InjectController(IController controller) {
            _controller = controller as MonoBehaviour;
            Controller = controller;
        }
        #endregion Inject
    }
}
