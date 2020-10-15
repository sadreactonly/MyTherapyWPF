using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Models
{
	public class TherapyChanges
	{
		public TherapyChanges()
		{

		}

		public TherapyChanges(Operation operation, DailyTherapy therapy)
		{
			Operation = operation;
			Therapy = therapy;
		}
		[JsonProperty(PropertyName = "id")]
		public int Id { get; set; }

		[JsonProperty(PropertyName = "therapyGuid")]
		public Guid TherapyGuid { get; set; }

		[JsonProperty(PropertyName = "operation")]
		public Operation Operation { get; set; }

		[JsonProperty(PropertyName = "therapy")]
		public DailyTherapy Therapy { get; set; }
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
