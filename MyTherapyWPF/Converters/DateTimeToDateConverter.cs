using System;
using System.Globalization;
using System.Windows.Data;

namespace MyTherapyWPF.Converters
{
	public class DateTimeToDateConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return "None.";
			return ((DateTime)value).ToShortDateString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return DateTime.Parse((string)value, CultureInfo.CurrentCulture);
		}
	}
}
