using System.Collections.Generic;

namespace EVA.Import.Triggers {
    public interface ITriggerable {
        public void Trigger();
        public void Toggle(bool value);
        public bool IsEnabled();
        public void AddLink(TriggerLink triggerLink);
        public void RemoveLink(TriggerLink triggerLink);
        public List<TriggerLink> GetLinks();
    }
}
