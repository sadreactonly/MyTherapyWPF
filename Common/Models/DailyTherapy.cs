using Newtonsoft.Json;
using SQLite;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Common.Models
{
	[Table("DailyTherapy")]
	[Serializable]
	public class DailyTherapy : INotifyPropertyChanged, ICloneable
	{
		private double dose;
		private DateTime date;
		private bool isTaken;

		[JsonIgnore]
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "guid")]
		public Guid Guid { get; set; }

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

		public object Clone()
		{
			using (MemoryStream stream = new MemoryStream())
			{
				if (this.GetType().IsSerializable)
				{

					BinaryFormatter formatter = new BinaryFormatter();

					formatter.Serialize(stream, this);

					stream.Position = 0;

					return formatter.Deserialize(stream);

				}

				return null;

			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
