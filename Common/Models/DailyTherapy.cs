using Newtonsoft.Json;
using SQLite;
using System;
using System.ComponentModel;

namespace Common.Models
{
	[Table("DailyTherapy")]
	public class DailyTherapy : INotifyPropertyChanged
	{
		private double dose;
		private DateTime date;
		private bool isTaken;

		[JsonProperty(PropertyName = "id")]
		[PrimaryKey, AutoIncrement]
		//[Key]
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
				OnPropertyChanged(nameof(Dose));
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
				OnPropertyChanged(nameof(Dose));
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
				OnPropertyChanged(nameof(IsTaken)); 
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
