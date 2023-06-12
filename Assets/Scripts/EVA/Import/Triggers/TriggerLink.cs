using System;
using System.Collections.Generic;
using UnityEngine;

namespace Veery.Import.Triggers {
    /// <summary>
    /// Represents a link between two import objects that can trigger a delegate with parameters.
    /// </summary>
    public class TriggerLink : MonoBehaviour {
        [SerializeField]
        [Tooltip("The line renderer used to draw the link between the two objects.")]
        /// <summary>
        /// The LineRenderer component used to draw the link between the two objects.
        /// </summary>
        private LineRenderer _lineRenderer;

        [SerializeField]
        [Tooltip("The material used to draw the link between the two objects.")]
        /// <summary>
        /// The material used to draw the link between the two objects.
        /// </summary>
        private Material _material;

        /// <summary>
        /// The delegate that will be triggered when the linked object is activated.
        /// </summary>
        public Delegate LinkedDelegate { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of parameters for the linked delegate.
        /// </summary>
        public Dictionary<string, Tuple<Type, object>> Parameters { get; set; }

        /// <summary>
        /// The delegate that will be triggered when the trigger object is activated.
        /// </summary>
        public Action TriggerDelegate { get; set; }

        /// <summary>
        /// The import object that triggers the linked delegate when activated.
        /// </summary>
        public ImportObject Trigger { get; set; }

        /// <summary>
        /// The import object that is linked to this trigger.
        /// </summary>
        public ImportObject LinkedObject { get; set; }

        private void Awake() {
            TriggerDelegate = new(Invoke);
            _lineRenderer.material = _material;
        }

        public void Update() {
            if (LinkedObject == null || Trigger == null) {
                return;
            }

            _lineRenderer.SetPosition(0, Trigger.transform.position);
            _lineRenderer.SetPosition(1, LinkedObject.transform.position);
        }

        /// <summary>
        /// Sets the color of the material used to draw the link between the two objects.
        /// </summary>
        /// <param name="color">The color to set.</param>
        public void SetColor(Color color) {
            _material.color = color;
        }

        /// <summary>
        /// Invokes the linked delegate with the parameters specified in the <see cref="Parameters"/> dictionary.
        /// </summary>
        public void Invoke() {
            Debug.Log("TriggerLink.Invoke");
            List<object> args = new();
            foreach (Tuple<Type, object> parameter in Parameters.Values) {
                Debug.Log($"TriggerLink.Invoke: {parameter.Item2} ({parameter.Item1})");
                args.Add(parameter.Item2);
            }

            _ = LinkedDelegate.DynamicInvoke(args.ToArray());
        }
    }
}
