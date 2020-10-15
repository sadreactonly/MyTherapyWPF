using MyTherapyWPF.ViewModels;
using System.Windows.Controls;


namespace MyTherapyWPF.Views
{
	/// <summary>
	/// Interaction logic for GeneralView.xaml
	/// </summary>
	public partial class GeneralView : UserControl
	{
		public GeneralView()
		{
			InitializeComponent();
			GeneralWindowViewModel generalWindowViewModel = new GeneralWindowViewModel();
			this.DataContext = generalWindowViewModel;
		}

		private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			this.DataContext = new GeneralWindowViewModel();
		}
	}
}
