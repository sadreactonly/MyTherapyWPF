using MyTherapyWPF.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTherapyWPF.Contexts
{
	public class AppDbContext:DbContext
	{
		private static readonly object _lock = new object();

		private static AppDbContext _instance;
		public static AppDbContext Instance
		{
			get
			{
				lock(_lock)
				{ 
					if(_instance == null)
					{
						_instance = new AppDbContext();
					}
					return _instance;
				}
			}
		}
		public DbSet<DailyTherapy> Therapies { get; set; }
		public DbSet<DoctorAppointment> DoctorAppointments { get; set; }
	}
}
