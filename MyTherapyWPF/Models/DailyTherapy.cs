using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Models
{
	public class DailyTherapy : INotifyPropertyChanged
	{
		private decimal dose;
		private DateTime date;
		private bool isTaken;

		[JsonIgnore]
		[Key]
		public int Id { get; set; }


		public Guid Guid{get;set;}

		/// <summary>
		/// Gets or sets daily dosage of therapy.
		/// </summary>
		[JsonProperty(PropertyName="dose")]
		public decimal Dose
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
