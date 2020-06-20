using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyTherapyWPF.Converters
{
	public class IsTakenBoolToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool val = (bool)value;
			if(val)
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
			string val = (string)value;

			if(val.Equals("Taken"))
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
