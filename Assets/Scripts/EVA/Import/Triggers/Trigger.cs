using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Veery.Import.Triggers.Triggers {
    public class Trigger : ImportObject, ITriggerable {
        private event Action WhenTriggered = delegate { };

        private bool Enabled { get; set; }
        private List<TriggerLink> Links { get; } = new();

        public Task<bool> Init() {
            LoadDeferredGOs();

            return Task.FromResult(true);
        }

        void ITriggerable.Trigger() {
            Debug.Log($"[{GetType().Name}] (Trigger) Trigger({Enabled})");
            if (!Enabled) {
                return;
            }

            WhenTriggered?.Invoke();
        }

        void ITriggerable.Toggle(bool value) {
            Debug.Log($"[{GetType().Name}] Toggle({value})");
            Enabled = value;
        }

        void ITriggerable.AddLink(TriggerLink triggerLink) {
            Links.Add(triggerLink);
            WhenTriggered += triggerLink.TriggerDelegate;
        }

        void ITriggerable.RemoveLink(TriggerLink triggerLink) {
            Debug.Log($"[{GetType().Name}] RemoveLink({triggerLink})");
            _ = Links.Remove(triggerLink);
            WhenTriggered -= triggerLink.TriggerDelegate;
        }

        bool ITriggerable.IsEnabled() {
            return Enabled;
        }

        List<TriggerLink> ITriggerable.GetLinks() {
            return Links;
        }
    }
}
