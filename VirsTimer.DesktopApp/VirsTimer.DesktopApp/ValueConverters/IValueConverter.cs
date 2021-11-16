namespace VirsTimer.DesktopApp.ValueConverters
{
    public interface IValueConverter<T, K>
    {
        public K Convert(T value);
    }
}
