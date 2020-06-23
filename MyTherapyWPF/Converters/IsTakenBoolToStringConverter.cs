using System;
using System.Globalization;
using System.Windows.Data;

namespace MyTherapyWPF.Converters
{
	public class IsTakenBoolToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return "Not taken";
			if((bool)value)
			{
				return "Taken";
			}
			else
			{
				return "Not taken";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return false;
			string val = (string)value;
			if (val == null)
				throw new ArgumentNullException(nameof(value));

			if(val.Equals("Taken",StringComparison.CurrentCulture))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
