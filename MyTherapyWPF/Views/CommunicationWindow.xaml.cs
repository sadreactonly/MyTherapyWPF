using MyTherapyWPF.ViewModels;
using System.Windows.Controls;


namespace MyTherapyWPF.Views
{
	/// <summary>
	/// Interaction logic for CommunicationWindow.xaml
	/// </summary>
	public partial class CommunicationWindow : UserControl
	{
		public CommunicationWindow()
		{
			InitializeComponent();
			CommunicationViewModel communicationViewModel = new CommunicationViewModel();
			this.DataContext = communicationViewModel;
		}

	}
}
