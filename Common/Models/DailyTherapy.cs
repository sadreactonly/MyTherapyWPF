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
		private bool isTaken = false;

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
		/// <summary>
		/// Gets or sets info is therapy taken.
		/// </summary>
		[JsonProperty(PropertyName = "isTaken")]
		public bool IsTaken
		{
			get => isTaken;
			set
			{
				isTaken = value;
				OnPropertyChanged("IsTaken"); 
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
