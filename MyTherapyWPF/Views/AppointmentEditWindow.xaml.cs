using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyTherapyWPF.Views
{
	/// <summary>
	/// Interaction logic for TherapyEditWindow.xaml
	/// </summary>
	public partial class AppointmentEditWindow : Window
	{
		public AppointmentEditWindow()
		{
			InitializeComponent();
		}
		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}
	}
}
