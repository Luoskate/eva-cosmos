using System;
using System.Collections.Generic;

namespace EVA.Import.Properties {
    public interface ITriggerable {
        public void Trigger();
        public void Toggle(bool value);
        public bool IsEnabled();
        public void AddLink(TriggerLink triggerLink);
        public void RemoveLink(TriggerLink triggerLink);
        public List<TriggerLink> GetLinks();
    }

    public class TriggerLink {
        public Delegate LinkedDelegate { get; set; }
        public Dictionary<string, Tuple<Type, object>> Parameters { get; set; }
        public Action TriggerDelegate { get; set; }

        public TriggerLink(Delegate linkedDelegate, Dictionary<string, Tuple<Type, object>> parameters) {
            LinkedDelegate = linkedDelegate;
            Parameters = parameters;
            TriggerDelegate = new(Invoke);
        }

        public void Invoke() {
            List<object> args = new();
            foreach (Tuple<Type, object> parameter in Parameters.Values) {
                args.Add(parameter.Item2);
            }

            _ = LinkedDelegate.DynamicInvoke(args.ToArray());
        }
    }
}
