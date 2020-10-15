using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace Common.Models
{
	[Table("TherapyChanges")]
	public class TherapyChanges
	{

		[JsonIgnore]
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "operation")]
		public Operation Operation { get; set; }

		[JsonIgnore]
		public Guid TherapyGuid { get; set; }

		[JsonProperty(PropertyName = "therapy")]
		[Ignore]
		public DailyTherapy Therapy { get; set; }

		public TherapyChanges()
		{

		}

		public TherapyChanges(Operation operation, Guid guid)
		{
			TherapyGuid = guid;
			Operation = operation;
		}
	}

	public class AppointmentChanges
	{
		public Operation Operation { get; set; }
		public DoctorAppointment Appointment { get; set; }
	}

	public enum Operation
	{
		Add,
		Update,
		Delete
	}
}
