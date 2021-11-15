namespace VirsTimer.DesktopApp.ValueConverters
{
    public interface IExplicitValueConverter<T, K>
    {
        public K Convert(T value);
    }
}
