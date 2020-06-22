using MyTherapyWPF.Contexts;
using MyTherapyWPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Collections.ObjectModel;

namespace MyTherapyWPF
{
	public class DatabaseManager
	{
		private static object _lock = new object();
		private static DatabaseManager _instance;

		public static DatabaseManager Instance
		{
			get
			{
				lock (_lock)
				{
					if (_instance == null)
						_instance = new DatabaseManager();

					return _instance;
				}

			}
		}


		private AppDbContext db = AppDbContext.Instance;

		public delegate void DbUpdatedEventHandler();

		public DbUpdatedEventHandler DbUpdatedEvent;

		public DailyTherapy GetDailyTherapy()
		{
			var therapies = db.Therapies.ToList();
			return therapies.Where(x => x.Date.Date == DateTime.Now.Date).FirstOrDefault();
		}

		public void TakeTherapy(DailyTherapy therapy)
		{
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
			db.Therapies.AddRange(therapies);
			db.SaveChanges();
			DbUpdatedEvent?.Invoke();
		}

		public void DeleteTherapy(DailyTherapy therapy)
		{
			db.Therapies.Remove(therapy);
			db.SaveChanges();
			//DatabaseUpdatedEvent?.Invoke();

		}
	}
}
