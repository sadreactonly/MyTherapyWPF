
using MyTherapyWPF.ViewModels;
using System.Windows.Controls;


namespace MyTherapyWPF.Views
{
	/// <summary>
	/// Interaction logic for AppointmentsView.xaml
	/// </summary>
	public partial class AppointmentsView : UserControl
	{
		AppointmentsViewModel appointmentsViewModel;
		public AppointmentsView()
		{
			InitializeComponent();
			appointmentsViewModel = new AppointmentsViewModel();
			this.DataContext = appointmentsViewModel;
		}
		private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			//buttonDelete.IsEnabled = true;

		}

	}
}
