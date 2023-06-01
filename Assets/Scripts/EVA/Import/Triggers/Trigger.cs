using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace EVA.Import.Triggers.Triggers {
    public class Trigger : ImportObject, ITriggerable {
        private event Action WhenTriggered;

        private bool Enabled { get; set; }
        private List<TriggerLink> Links { get; set; }

        public void Start() {
            Enabled = false;
            Links = new();
        }

        public Task<bool> Init() {
            LoadDeferredGOs();

            return Task.FromResult(true);
        }

        void ITriggerable.Trigger() {
            Debug.Log($"[{GetType().Name}] (Trigger) Trigger()");
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
            Debug.Log($"[{GetType().Name}] (Trigger) AddLink({triggerLink})");
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
