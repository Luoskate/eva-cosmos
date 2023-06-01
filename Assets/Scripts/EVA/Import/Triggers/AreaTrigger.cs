using System;
using System.Collections.Generic;
using UnityEngine;

namespace EVA.Import.Triggers.Triggers {
    public class AreaTrigger : Trigger, ITriggerEndable {
        public event Action WhenTriggerEnded;

        private bool Enabled { get; set; }
        private List<TriggerLink> Links { get; set; }

        public new void Start() {
            base.Start();

            Enabled = false;
            Links = new();
        }

        void ITriggerEndable.Trigger() {
            Debug.Log($"[{GetType().Name}] (AreaTrigger) Trigger() | enabled {Enabled}");
            if (!Enabled) {
                return;
            }

            WhenTriggerEnded?.Invoke();
        }

        void ITriggerEndable.Toggle(bool value) {
            Enabled = value;
        }

        void ITriggerEndable.AddLink(TriggerLink triggerLink) {
            Debug.Log($"[{GetType().Name}] (AreaTrigger) AddLink({triggerLink})");
            Debug.Log($"[{GetType().Name}] (AreaTrigger) AddLink() | {triggerLink.LinkedDelegate.Method.Name}");
            Links.Add(triggerLink);
            WhenTriggerEnded += triggerLink.TriggerDelegate;
        }

        void ITriggerEndable.RemoveLink(TriggerLink triggerLink) {
            _ = Links.Remove(triggerLink);
            WhenTriggerEnded -= triggerLink.TriggerDelegate;
        }

        bool ITriggerEndable.IsEnabled() {
            return Enabled;
        }

        List<TriggerLink> ITriggerEndable.GetLinks() {
            return Links;
        }

        public void OnTriggerEnter(Collider other) {
            if (other != TriggersManager.Instance.Head) {
                return;
            }

            ((ITriggerable)this).Trigger();
        }

        public void OnTriggerExit(Collider other) {
            if (other != TriggersManager.Instance.Head) {
                return;
            }

            ((ITriggerEndable)this).Trigger();
        }
    }
}
