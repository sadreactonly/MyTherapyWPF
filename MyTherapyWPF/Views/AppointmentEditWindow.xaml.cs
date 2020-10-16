using System.Windows;
using System.Windows.Input;

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
			this.Close();
		}
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
				this.DragMove();
		}
	}
}
