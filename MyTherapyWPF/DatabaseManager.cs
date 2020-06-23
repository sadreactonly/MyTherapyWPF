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

		public DbUpdatedEventHandler DbUpdatedEvent;

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
			foreach(var tmp in therapies)
			{
				db.Therapies.AddOrUpdate(tmp);
			}
			db.SaveChanges();
			DbUpdatedEvent?.Invoke();
		}

		public void DeleteTherapy(DailyTherapy therapy)
		{
			db.Therapies.Remove(therapy);
			db.SaveChanges();
		}

	}
}
