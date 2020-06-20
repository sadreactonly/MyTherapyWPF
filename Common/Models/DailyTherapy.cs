using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace MyTherapyWPF.Common
{
	//[Serializable]
	public class DailyTherapy : INotifyPropertyChanged
	{
		private double dose;
		private DateTime date;

		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }


		/// <summary>
		/// Gets or sets daily dosage of therapy.
		/// </summary>
		[JsonProperty(PropertyName="dose")]
		public double Dose
		{
			get => dose;
			set
			{
				dose = value;
				OnPropertyChanged("Dose");
			}
		}

		/// <summary>
		/// Gets or sets date for daily therapy.
		/// </summary>
		[JsonProperty(PropertyName = "date")]
		public DateTime Date {
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
