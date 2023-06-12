using System;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using UnityEngine;

namespace Veery.Interaction {
    /// <summary>
    /// A selector that allows for advanced selection logic based on controller input.
    /// </summary>
    public class AdvancedControllerSelector : MonoBehaviour, ISelector {
        /// <summary>
        /// The logic operator used to determine whether to select an object based on the button usage.
        /// </summary>
        public enum ControllerSelectorLogicOperator {
            /// <summary>
            /// Selects the object if any of the button usages are active.
            /// </summary>
            Any = 0,

            /// <summary>
            /// Selects the object only if all of the button usages are active.
            /// </summary>
            All = 1
        }

        /// <summary>
        /// The logic used to determine how the selector should behave when the button usage is held.
        /// </summary>
        public enum ControllerSelectorHoldLogic {
            /// <summary>
            /// The selector will select the object when the button usage is held.
            /// </summary>
            Hold = 0,

            /// <summary>
            /// The selector will toggle the selection state of the object when the button usage is pressed depending if there is an object hovered.
            /// </summary>
            SmartToggle = 1,

            /// <summary>
            /// The selector will toggle the selection state of the object when the button usage is pressed.
            /// </summary>
            Toggle = 2
        }

        [SerializeField]
        [Interface(typeof(IController))]
        [Tooltip("The controller to use for selection.")]
        /// <summary>
        /// The MonoBehaviour that represents the controller to use for selection.
        /// </summary>
        private MonoBehaviour _controller;

        [SerializeField]
        [Tooltip("The button usage to use for selection.")]
        /// <summary>
        /// The button usage to use for selection.
        /// </summary>
        private ControllerButtonUsage _controllerButtonUsage;

        [SerializeField]
        [Tooltip("The logic used to determine how the selector should behave when the button usage is held.")]
        /// <summary>
        /// The logic used to determine how the selector should behave when the button usage is held.
        /// </summary>
        private ControllerSelectorHoldLogic _controllerHoldLogic;

        [SerializeField]
        [Tooltip("The logic operator used to determine whether to select an object based on the button usage.")]
        /// <summary>
        /// The logic operator used to determine whether to require all or any of the button usages to be active for selection.
        /// </summary>
        private ControllerSelectorLogicOperator _requireButtonUsages = ControllerSelectorLogicOperator.Any;

        #region Properties
        /// <summary>
        /// The button usage to use for selection.
        /// </summary>
        public ControllerButtonUsage ControllerButtonUsage {
            get => _controllerButtonUsage;

            set => _controllerButtonUsage = value;
        }

        /// <summary>
        /// The logic used to determine how the selector should behave when the button usage is held.
        /// </summary>
        public ControllerSelectorHoldLogic ControllerHoldLogic {
            get => _controllerHoldLogic;

            set => _controllerHoldLogic = value;
        }

        /// <summary>
        /// The logic operator used to determine whether to require all or any of the button usages to be active for selection.
        /// </summary>
        public ControllerSelectorLogicOperator RequireButtonUsages {
            get => _requireButtonUsages;

            set => _requireButtonUsages = value;
        }
        #endregion Properties

        /// <summary>
        /// The controller used for selection.
        /// </summary>
        public IController Controller { get; private set; }

        /// <summary>
        /// Event that is triggered when an object is selected by the controller.
        /// </summary>
        public event Action WhenSelected = delegate { };

        /// <summary>
        /// Event that is triggered when an object is unselected by the controller.
        /// </summary>
        public event Action WhenUnselected = delegate { };

        private bool _prevPressed;
        private bool _selected;

        /// <summary>
        /// Gets or sets a value indicating whether a late selection should be performed.
        /// </summary>
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

        /// <summary>
        /// Handles the hold logic for the controller selector. If the button usage is pressed, the object is selected. If it is released, the object is unselected.
        /// </summary>
        /// <param name="pressed">A boolean indicating whether the button usage is currently pressed.</param>
        private void HandleHoldLogic(bool pressed) {
            if (pressed) {
                WhenSelected();
            } else {
                WhenUnselected();
            }
        }

        /// <summary>
        /// Handles the smart toggle logic for the controller selector. If the button usage is pressed, the object is unselected and a late selection is performed.
        /// </summary>
        /// <param name="pressed">A boolean indicating whether the button usage is currently pressed.</param>
        private void HandleSmartToggleLogic(bool pressed) {
            if (!pressed) {
                return;
            }

            WhenUnselected();
            LateSelection = true;
        }

        /// <summary>
        /// Handles the toggle logic for the controller selector. If the button usage is pressed, the object is toggled between selected and unselected.
        /// </summary>
        /// <param name="pressed">A boolean indicating whether the button usage is currently pressed.</param>
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
    }
}
