using MyTherapyWPF.ViewModels;
using System.Windows;
using System.Windows.Controls;


namespace MyTherapyWPF.Views
{
	/// <summary>
	/// Interaction logic for TherapiesView.xaml
	/// </summary>
	public partial class TherapiesView : UserControl
	{
		TherapiesViewModel addTherapyViewModel;
		public TherapiesView()
		{
			InitializeComponent();
			addTherapyViewModel = new TherapiesViewModel();
			this.DataContext = addTherapyViewModel;
		}

		private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			buttonTake.IsEnabled = true;
			buttonDelete.IsEnabled = true;

		}
	}
}
