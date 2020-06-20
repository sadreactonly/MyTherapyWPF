using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTherapyWPF.Common
{
	public class DoctorAppointment:INotifyPropertyChanged
	{
		public int Id { get; set; }
		private double? inr;
		private DateTime date;
		/// <summary>
		/// Gets or sets value of INR.
		/// </summary>
		public double? INR {
			get => inr;
			set
			{
				inr = value;
				OnPropertyChanged("INR");
			} 
		}

		/// <summary>
		/// Gets or sets date of doctors appointment.
		/// </summary>
		public DateTime Date
		{
			get => date;
			set
			{
				date = value;
				OnPropertyChanged("Date");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
