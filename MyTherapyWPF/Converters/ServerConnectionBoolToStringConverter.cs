using System;
using System.Globalization;
using System.Windows.Data;

namespace MyTherapyWPF.Converters
{
	public class ServerConnectionBoolToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value != null && (bool)value ? "Server is started" : "Server is not started.";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return false;
			return !((string) value).Contains("not");
		}
	}
}
