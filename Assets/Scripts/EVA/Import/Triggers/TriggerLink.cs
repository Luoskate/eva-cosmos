using System;
using System.Collections.Generic;
using UnityEngine;

namespace Veery.Import.Triggers {
    public class TriggerLink : MonoBehaviour {
        [SerializeField]
        private LineRenderer _lineRenderer;

        [SerializeField]
        private Material _material;

        public Delegate LinkedDelegate { get; set; }
        public Dictionary<string, Tuple<Type, object>> Parameters { get; set; }
        public Action TriggerDelegate { get; set; }
        public ImportObject Trigger { get; set; }
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

        public void SetColor(Color color) {
            _material.color = color;
        }

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
