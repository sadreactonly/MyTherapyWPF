using Common.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace MyTherapyWPF.Contexts
{
	public class AppDbContext:DbContext
	{
		private static readonly object Lock = new object();

		private static AppDbContext _instance;
		public static AppDbContext Instance
		{
			get
			{
				lock(Lock)
				{
					return _instance ?? (_instance = new AppDbContext());
				}
			}
		}
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder?.Entity<DailyTherapy>().Property(m => m.Id)
					 .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
			base.OnModelCreating(modelBuilder);
		}
		
		public DbSet<DailyTherapy> Therapies { get; set; }
		public DbSet<DoctorAppointment> DoctorAppointments { get; set; }
	}
}
