using MyTherapyWPF.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

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

		private void UserControl_Loaded(object sender, RoutedEventArgs e)
		{

		}


		private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			buttonTake.IsEnabled = true;
			buttonDelete.IsEnabled = true;

		}
	}
}
