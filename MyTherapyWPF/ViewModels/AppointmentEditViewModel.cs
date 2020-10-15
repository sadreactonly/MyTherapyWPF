using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using MyTherapyWPF.Commands;
using System.Collections.ObjectModel;
using Common.Models;
using System.Windows;

namespace MyTherapyWPF.ViewModels
{
	public class AppointmentEditViewModel
	{
		readonly DatabaseManager DatabaseManager;

		public event EventHandler Saved;

		private RelayCommand addCommand;
		private DoctorAppointment doctorAppointment;

		public DateTime Date { get; set; }
		public double? INR { get; set; }

		public AppointmentEditViewModel(DoctorAppointment doctorAppointment)
		{
			this.doctorAppointment = doctorAppointment ?? default;
			
			Date = doctorAppointment.Date;
			INR = doctorAppointment.INR;

			DatabaseManager = DatabaseManager.Instance;
		}

		public ICommand UpdateCommand => addCommand ?? (addCommand = new RelayCommand(SaveCommandAction));

		private void SaveCommandAction(object obj)
		{
			doctorAppointment.Date = Date;
			doctorAppointment.INR = INR;

			DatabaseManager.UpdateAppointment(doctorAppointment);

			Saved.Invoke(this, null);
		}
	}
}
