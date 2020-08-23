using System;
using System.ComponentModel;
using Newtonsoft.Json;
using SQLite;

namespace Common.Models
{
	[Table(nameof(DoctorAppointment))]
	public class DoctorAppointment:INotifyPropertyChanged
	{
		[JsonProperty(PropertyName = "id")]
		[PrimaryKey, AutoIncrement]
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
				OnPropertyChanged(nameof(INR));
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
				OnPropertyChanged(nameof(Date));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
