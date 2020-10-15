using MyTherapyWPF.Contexts;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace MyTherapyWPF
{
	public class DatabaseManager
	{
		private static readonly object Lock = new object();
		private static DatabaseManager instance;

		public static DatabaseManager Instance
		{
			get
			{
				lock (Lock)
				{
					return instance ?? (instance = new DatabaseManager());
				}

			}
		}

		private readonly AppDbContext db = AppDbContext.Instance;

		public delegate void DbUpdatedEventHandler();

		internal DoctorAppointment GetLastDoctorAppointment()
		{
			var list = GetAppointments().OrderBy(x => x.Date);
			return list.Reverse().Where(x => x.INR != 0).FirstOrDefault();
		}

		internal DoctorAppointment GetNextDoctorAppointment()
		{
			var list = GetAppointments().OrderBy(x => x.Date);
			return list.Where(x=>x.INR == 0).FirstOrDefault();
		}

		public DbUpdatedEventHandler DbUpdatedEvent;

		internal void UpdateAppointment(DoctorAppointment doctorAppointment)
		{
			db.DoctorAppointments.AddOrUpdate(doctorAppointment);
			DbUpdatedEvent?.Invoke();
		}


		#region Therapy
		public DailyTherapy GetDailyTherapy()
		{
			return db.Therapies.FirstOrDefault(x => DbFunctions.TruncateTime(x.Date) == DbFunctions.TruncateTime(DateTime.Now));
		}

		public void TakeTherapy(DailyTherapy therapy)
		{
			if (therapy == null)
				return;
			therapy.IsTaken = true;
			db.Therapies.AddOrUpdate(therapy);
			db.SaveChanges();
		}

		public List<DailyTherapy> GetTherapies()
		{
			return db.Therapies.ToList();
		}

		public void AddTherapies(ObservableCollection<DailyTherapy> therapies)
		{
			if (therapies == null)
				return;
			db.Therapies.AddRange(therapies);
			db.SaveChanges();
			DbUpdatedEvent?.Invoke();
		}

		public void DeleteTherapy(DailyTherapy therapy)
		{
			var dbTherapy = db.Therapies.Where(x => x.Guid == therapy.Guid).First();

			db.Therapies.Remove(dbTherapy);
			db.SaveChanges();
		}
		public void DeleteTherapy(Guid guid)
		{
			var dbTherapy = db.Therapies.Where(x => x.Guid == guid).First();

			db.Therapies.Remove(dbTherapy);
			db.SaveChanges();
		}
		#endregion

		public List<DoctorAppointment> GetAppointments()
		{
			return db.DoctorAppointments.ToList();
		}

		public void AddAppointment(DoctorAppointment appointment)
		{
			if (appointment == null)
				return;
			
			db.DoctorAppointments.Add(appointment);
			
			db.SaveChanges();
			DbUpdatedEvent?.Invoke();
		}

		internal void DeleteAppointment(DoctorAppointment selectedItem)
		{
			db.DoctorAppointments.Remove(selectedItem);
			db.SaveChanges();
		}

		internal void AddTherapy(DailyTherapy therapy)
		{
			db.Therapies.Add(therapy);
			db.SaveChanges();
			DbUpdatedEvent?.Invoke();
		}

		internal void UpdateTherapy(DailyTherapy therapy)
		{
			var tmp = db.Therapies.Where(x => x.Guid == therapy.Guid).First();
			tmp.IsTaken = therapy.IsTaken;
			db.Therapies.AddOrUpdate(tmp);
			DbUpdatedEvent?.Invoke();
		}
	}
}
