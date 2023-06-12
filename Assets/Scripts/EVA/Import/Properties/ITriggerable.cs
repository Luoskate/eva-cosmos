using System.Collections.Generic;
using Veery.Import.Triggers;

namespace Veery.Import.Properties {
    /// <summary>
    /// Interface for objects that can be triggered and linked to another object via a <see cref="TriggerLink"/>.
    /// </summary>
    public interface ITriggerable {
        /// <summary>
        /// Triggers the object, causing it to perform its associated action(s).
        /// </summary>
        public void Trigger();

        /// <summary>
        /// Toggles the state of the trigger based on the given value.
        /// </summary>
        /// <param name="value">The value to set the state to.</param>
        public void Toggle(bool value);

        /// <summary>
        /// Returns whether the trigger is currently enabled or not.
        /// </summary>
        /// <returns>True if the trigger is enabled, false otherwise.</returns>
        public bool IsEnabled();

        /// <summary>
        /// Adds a <see cref="TriggerLink"/> to the object, linking it to another object.
        /// </summary>
        /// <param name="triggerLink">The <see cref="TriggerLink"/> to add.</param>
        public void AddLink(TriggerLink triggerLink);

        /// <summary>
        /// Removes a <see cref="TriggerLink"/> from the object, unlinking it from another object.
        /// </summary>
        /// <param name="triggerLink">The <see cref="TriggerLink"/> to remove.</param>
        public void RemoveLink(TriggerLink triggerLink);

        /// <summary>
        /// Returns a list of all <see cref="TriggerLink"/> objects linked to this <see cref="ITriggerable"/> object.
        /// </summary>
        /// <returns>A list of <see cref="TriggerLink"/> objects.</returns>
        public List<TriggerLink> GetLinks();
    }
}
