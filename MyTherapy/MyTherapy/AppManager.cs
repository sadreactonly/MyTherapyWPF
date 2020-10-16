using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Android.Content;
using Common.Models;
using MyAppointment;

namespace MyTherapy
{
	public class AppManager
	{
		private ChangesDatabase changesDatabase = ChangesDatabase.Instance;
		private DoctorAppointmentDatabase appointmentDatabase = DoctorAppointmentDatabase.Instance;
		private TherapyDatabase therapyDatabase = TherapyDatabase.Instance;
		private Context appContext;

		public event EventHandler TherapyTaken;

		public AppManager()
		{
			
		}

		public AppManager(Context context)
		{
			appContext = context;
		}

		public void AddAppointmentChange(AppointmentChanges appointmentChanges)
		{
			changesDatabase.AddAppointmentChange(appointmentChanges);
		}

		public List<AppointmentChanges> GetAppointmentChanges()
		{
			return changesDatabase.GetAppointmentsChanges();
		}

		public void AddTherapyChange(TherapyChanges therapyChanges)
		{
			changesDatabase.AddTherapyChange(therapyChanges);
		}

		public List<TherapyChanges> GetTherapyChanges()
		{
			return changesDatabase.GetTherapyChanges();
		}

		public void AddAppointments(DoctorAppointment appointments)
		{
			appointmentDatabase.AddAppointment(appointments);
			
		}

		public List<DoctorAppointment> GetAppointments()
		{
			return appointmentDatabase.GetAppointments();
		}

		public List<DailyTherapy> GetTherapies()
		{
			return therapyDatabase.GetTherapies();
		}

		public void AddTherapies(List<DailyTherapy> therapies)
		{
			therapyDatabase.AddTherapySchema(therapies);
			foreach (var tmp in therapies)
			{
				var tmpCh = new TherapyChanges()
				{
					Operation = Operation.Add,
					TherapyGuid = tmp.Guid,
					Therapy = tmp
				};

				AddTherapyChange(tmpCh);

			}
		}

		internal void DeleteTherapy(DailyTherapy item)
		{
			DailyTherapy clonedObject = item.CloneObject() as DailyTherapy;

			AddTherapyChange(new TherapyChanges(Operation.Delete, clonedObject.Guid));
			therapyDatabase.DeleteTherapy(item);
		}

		public DailyTherapy GetTodayTherapy()
		{
			return GetTherapies().FirstOrDefault(x => x.Date == DateTime.Now.Date);
		}

		internal void TakeTherapy(DailyTherapy todayTherapy)
		{
			therapyDatabase.UpdateTherapy(todayTherapy);
			TherapyTaken?.Invoke(this,null);
			var x = new TherapyChanges
			{
				Operation = Operation.Update,
				TherapyGuid = todayTherapy.Guid
			};
			AddTherapyChange(x);
		}

		internal void SetAllData(out string lastInrText, out string nextAppointmentText, out string todayTherapyTextText, out bool takeTherapyButtonEnabled)
		{
			var lastInr = appointmentDatabase.GetLastAppointment().INR;
			if (lastInr != null)
				lastInrText = lastInr.ToString();
			else
				lastInrText = appContext.Resources.GetString(Resource.String.not_set);

			var nextApp = appointmentDatabase.GetNextAppointment().Date;
			if(nextApp.Equals(DateTime.MinValue))
				nextAppointmentText = appContext.Resources.GetString(Resource.String.not_set);
			else
			nextAppointmentText = nextApp.ToShortDateString();

			var todayTherapy = GetTodayTherapy();
			todayTherapyTextText = todayTherapy !=null ? todayTherapy.Dose.ToString(CultureInfo.InvariantCulture): appContext.Resources.GetString(Resource.String.not_set);
			takeTherapyButtonEnabled = todayTherapy != null && todayTherapy.IsTaken;
		}

		internal DailyTherapy GetTherapyById(Guid id)
		{
			return therapyDatabase.GetTherapy(id);
		}

		internal void DeleteTherpyChanges()
		{
			changesDatabase.ClearTherapyChanges();
		}
	}
}