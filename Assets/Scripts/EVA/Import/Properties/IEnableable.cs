namespace Veery.Import.Triggers {
    public interface IEnableable {
        public void Toggle(bool value);
        public bool IsEnabled();
    }
}
