namespace Veery.Import.Properties {
    /// <summary>
    /// Interface for objects that can be enabled or disabled.
    /// </summary>
    public interface IEnableable {
        /// <summary>
        /// Toggles the enabled/disabled state of the object.
        /// </summary>
        /// <param name="value">The new enabled/disabled state of the object.</param>
        public void Toggle(bool value);

        /// <summary>
        /// Returns whether the object is currently enabled or disabled.
        /// </summary>
        /// <returns>True if the object is enabled, false if it is disabled.</returns>
        public bool IsEnabled();
    }
}
