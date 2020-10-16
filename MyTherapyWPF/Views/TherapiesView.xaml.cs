using MyTherapyWPF.ViewModels;
using System.Text.RegularExpressions;
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
			
		}

		private void textBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
		{
			Regex regex = new Regex("^[.][0-9]+$|^[0-9]*[.]{0,1}[0-9]*$");
			e.Handled = !regex.IsMatch((sender as TextBox).Text.Insert((sender as TextBox).SelectionStart, e.Text));
		}
	}
}
