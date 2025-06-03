namespace Zenox.Wpf.Core.Common.Converter
{
    public class TimeToDecimalMinusWorkBreakConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DateTime datetime)
            {
                if (datetime.Minute != 0)
                {
                    // Subtract 37 minutes
                    datetime = datetime.AddMinutes(-37);

                    DateTime time = datetime;

                    int minutesInDec = (datetime.Minute * 100) / 60;

                    string outTime = $"{time.Hour}.{minutesInDec:D2}";
                    return outTime;
                }
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
