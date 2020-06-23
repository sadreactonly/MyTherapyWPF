using Newtonsoft.Json;
using SQLite;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyTherapyWPF.Common
{
	//[Serializable]
	[Table("DailyTherapy")]
	public class DailyTherapy : INotifyPropertyChanged
	{
		private double dose;
		private DateTime date;
		private bool isTaken = false;
		private DateTime lastModified;

		[JsonProperty(PropertyName = "id")]
		[PrimaryKey, AutoIncrement]
		[Key]
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
		/// Gets or sets date for last modification of object.
		/// </summary>
		[JsonProperty(PropertyName = "lastModified")]
		public DateTime LastModified
		{
			get => lastModified;
			set
			{
				lastModified = value;
				OnPropertyChanged("LastModified");
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
				lastModified = DateTime.Now;
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
