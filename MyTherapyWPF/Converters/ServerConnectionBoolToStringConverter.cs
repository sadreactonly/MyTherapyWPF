using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MyTherapyWPF.Converters
{
	public class ServerConnectionBoolToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return (bool)value ? "Server is started" : "Server is not started.";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if ((value as string).Contains("not"))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
	}
}
