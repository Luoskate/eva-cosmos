using System;
using System.Collections.Generic;
using UnityEngine;
using Veery.Import.Properties;

namespace Veery.Import.Triggers {
    /// <summary>
    /// Represents a trigger that is activated when the player enters or exits a specified area.
    /// </summary>
    public class AreaTrigger : Trigger, ITriggerEndable {
        /// <summary>
        /// Event that is triggered when the area trigger is exited.
        /// </summary>
        public event Action WhenTriggerEnded;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AreaTrigger"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        private bool Enabled { get; set; }

        /// <summary>
        /// Gets the list of trigger links associated with this <see cref="AreaTrigger"/>.
        /// </summary>
        /// <value>The list of trigger links.</value>
        private List<TriggerLink> Links { get; } = new();

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

        /// <summary>
        /// Called when the trigger collider of this object collides with another collider.
        /// If the other collider is the player's head, this method triggers the area trigger.
        /// </summary>
        /// <param name="other">The collider that collided with the trigger collider.</param>
        public void OnTriggerEnter(Collider other) {
            if (other != TriggersManager.Instance.Head) {
                return;
            }

            ((ITriggerable)this).Trigger();
        }

        /// <summary>
        /// Called when the trigger collider of this object stops colliding with another collider.
        /// If the other collider is the player's head, this method triggers the end of the area trigger.
        /// </summary>
        /// <param name="other">The collider that stopped colliding with the trigger collider.</param>
        public void OnTriggerExit(Collider other) {
            if (other != TriggersManager.Instance.Head) {
                return;
            }

            ((ITriggerEndable)this).Trigger();
        }
    }
}
