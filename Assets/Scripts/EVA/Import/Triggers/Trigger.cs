using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Veery.Import.Properties;

namespace Veery.Import.Triggers {
    /// <summary>
    /// Represents a trigger that can be enabled or disabled and can be linked to other objects.
    /// </summary>
    public class Trigger : ImportObject, ITriggerable {
        /// <summary>
        /// Event that is triggered when the trigger is activated.
        /// </summary>
        private event Action WhenTriggered = delegate { };

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Trigger"/> is enabled or disabled.
        /// </summary>
        /// <value><c>true</c> if the trigger is enabled; otherwise, <c>false</c>.</value>
        private bool Enabled { get; set; }

        /// <summary>
        /// Gets the list of <see cref="TriggerLink"/>  associated with this trigger.
        /// </summary>
        private List<TriggerLink> Links { get; } = new();

        /// <summary>
        /// Initializes the trigger by loading deferred game objects.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a value indicating whether the initialization was successful.</returns>
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
