namespace Zenox.Wpf.Core.Common.Converter
{
    public class TimeToDecimalConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime datetime)
            {
                DateTime time = datetime;

                int minutesInDec = (datetime.Minute * 100) / 60;

                string outTime = $"{time.Hour}.{minutesInDec:D2}";
                return outTime;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double hours)
            {
                return TimeSpan.FromHours(hours);
            }
            return TimeSpan.Zero;
        }
    }
}
