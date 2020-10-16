using System;
using System.Collections.Generic;
using SQLite;
using PCLStorage;
using Common.Models;

namespace MyTherapy
{
	public class ChangesDatabase
	{
		readonly SQLiteConnection database;
		private static ChangesDatabase _instance;
		private static readonly object padlock = new object();

		public ChangesDatabase()
		{
			database = GetConnection();
			database.CreateTable<TherapyChanges>();
			//database.CreateTable<AppointmentChanges>();
		}

		/// <summary>
		/// Gets connection for database.
		/// </summary>
		/// <returns></returns>
		public SQLiteConnection GetConnection()
		{
			var sqliteFilename = "ChangeDatabase.db3";
			IFolder folder = FileSystem.Current.LocalStorage;
			string path = PortablePath.Combine(folder.Path, sqliteFilename);
			var sqLiteConnection = new SQLiteConnection(path);
			return sqLiteConnection;
		}

		/// <summary>
		/// Creates instance of AppointmentDatabase class.
		/// </summary>
		public static ChangesDatabase Instance
		{
			get
			{
				lock (padlock)
					return _instance ??= new ChangesDatabase();
			}
		}

		/// <summary>DailyAppointment
		/// Get all therapies from database.
		/// </summary>
		/// <returns>
		/// List of therapies.
		/// </returns>
		public List<AppointmentChanges> GetAppointmentsChanges() => database.Table<AppointmentChanges>().ToList();

		/// <summary>
		/// Add all therapies in database.
		/// </summary>
		/// <param name="therapiesSchema"></param>
		public void AddAppointmentChange(AppointmentChanges appointmentChange)
		{
			database.Insert(appointmentChange);
		}

		/// <summary>DailyAppointment
		/// Get all therapies from database.
		/// </summary>
		/// <returns>
		/// List of therapies.
		/// </returns>
		public List<TherapyChanges> GetTherapyChanges() => database.Table<TherapyChanges>().ToList();

		/// <summary>
		/// Add all therapies in database.
		/// </summary>
		/// <param name="therapiesSchema"></param>
		public void AddTherapyChange(TherapyChanges therapyChanges)
		{
			database.Insert(therapyChanges);
		}

		internal void ClearTherapyChanges()
		{
			database.DropTable<TherapyChanges>();
			database.CreateTable<TherapyChanges>();
		}
	}
}