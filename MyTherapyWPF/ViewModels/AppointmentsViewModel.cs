using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using MyTherapyWPF.Commands;
using System.Collections.ObjectModel;
using Common.Models;
using System.Windows;
using System.Security.RightsManagement;
using System.Windows.Navigation;

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

		public AppointmentsViewModel()
		{
			DatabaseManager = DatabaseManager.Instance;
			DatabaseManager.DbUpdatedEvent += RefreshGui;
			Appointments = new ObservableCollection<DoctorAppointment>(DatabaseManager.GetAppointments());
		}

		public DoctorAppointment SelectedItem { get; set; }
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
		}
		private void EditAction(object obj)
		{
			throw new NotImplementedException();
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
