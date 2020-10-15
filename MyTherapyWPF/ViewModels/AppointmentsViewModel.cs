using System;
using System.Windows.Input;
using System.ComponentModel;
using MyTherapyWPF.Commands;
using System.Collections.ObjectModel;
using Common.Models;
using System.Windows;
using MyTherapyWPF.Views;

namespace MyTherapyWPF.ViewModels
{
	public class AppointmentsViewModel : INotifyPropertyChanged
	{
		readonly DatabaseManager DatabaseManager;

		private RelayCommand addCommand;
		private RelayCommand editCommand;
		private RelayCommand deleteCommand;
		private string inr;
		private ObservableCollection<DoctorAppointment> appointments;
		private DateTime startDate = DateTime.Now;
		private bool deleteIsEnabled = false;
		private DoctorAppointment selectedItem;
		private bool editIsEnabled = false;

		public AppointmentsViewModel()
		{
			DatabaseManager = DatabaseManager.Instance;
			DatabaseManager.DbUpdatedEvent += RefreshGui;
			Appointments = new ObservableCollection<DoctorAppointment>(DatabaseManager.GetAppointments());
		}

		public DoctorAppointment SelectedItem 
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;
				DeleteIsEnabled = true;
				EditIsEnabled = true;
			}
		}
		public bool DeleteIsEnabled
		{
			get => deleteIsEnabled;
			set
			{
				deleteIsEnabled = value;
				OnPropertyChanged(nameof(DeleteIsEnabled));
			}
		}
		public bool EditIsEnabled
		{
			get => editIsEnabled;
			set
			{
				editIsEnabled = value;
				OnPropertyChanged(nameof(EditIsEnabled));
			}
		}
		public DateTime StartDate
		{
			get => startDate;
			set
			{
				startDate = value;
				OnPropertyChanged(nameof(StartDate));
			}
		}
		public string INR
		{
			get => inr;
			set
			{
				inr = value;
				OnPropertyChanged(nameof(INR));

			}
		}
		public ObservableCollection<DoctorAppointment> Appointments
		{
			get => appointments;
			set
			{
				appointments = value;
				OnPropertyChanged(nameof(Appointments));
			}
		}

		public ICommand AddCommand => addCommand ?? (addCommand = new RelayCommand(SaveCommandAction));
		public ICommand EditCommand => editCommand ?? (editCommand = new RelayCommand(EditAction));
		public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new RelayCommand(DeleteAction));

		private void SaveCommandAction(object obj)
		{
			if (double.TryParse(INR, out double inr))
			{
				var appointment = new DoctorAppointment()
				{
					INR = inr,
					Date = StartDate
				};

				DatabaseManager.AddAppointment(appointment);

			}
			else
				MessageBox.Show("Wrong format!");

		}
		private void DeleteAction(object obj)
		{
			DatabaseManager.DeleteAppointment(SelectedItem);
			Appointments.Remove(SelectedItem);
			
			DeleteIsEnabled = false;
			EditIsEnabled = false;
			SelectedItem = null;
		}
		private void EditAction(object obj)
		{
			AppointmentEditWindow win = new AppointmentEditWindow();
			AppointmentEditViewModel aevm = new AppointmentEditViewModel(SelectedItem);
			aevm.Saved += delegate (object o, EventArgs e) { win.Close(); };
			win.DataContext = aevm;
			win.Show();

		}
		private void RefreshGui()
		{
			Appointments = new ObservableCollection<DoctorAppointment>(DatabaseManager.GetAppointments());
			INR = "0";
			StartDate = DateTime.Now;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
