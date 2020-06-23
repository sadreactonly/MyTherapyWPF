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
	}
}
