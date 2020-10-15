using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Common.Models;
using PCLStorage;
using SQLite;

namespace MyAppointment
{
	/// <summary>
	/// Represent class for database of all therapies.
	/// </summary>
	public class DoctorAppointmentDatabase
	{
		readonly SQLiteConnection database;
		private static DoctorAppointmentDatabase _instance;
		private static readonly object padlock = new object();



		public DoctorAppointmentDatabase()
		{
			database = GetConnection();
			database.CreateTable<DoctorAppointment>();
		}

		/// <summary>
		/// Gets connection for database.
		/// </summary>
		/// <returns></returns>
		public SQLiteConnection GetConnection()
		{
			var sqliteFilename = "Appointments.db3";
			IFolder folder = FileSystem.Current.LocalStorage;
			string path = PortablePath.Combine(folder.Path, sqliteFilename);
			var sqLiteConnection = new SQLiteConnection(path);
			return sqLiteConnection;
		}

		/// <summary>
		/// Creates instance of AppointmentDatabase class.
		/// </summary>
		public static DoctorAppointmentDatabase Instance
		{
			get
			{
				lock (padlock)
					return _instance ??= new DoctorAppointmentDatabase();
			}
		}

		/// <summary>DailyAppointment
		/// Get all therapies from database.
		/// </summary>
		/// <returns>
		/// List of therapies.
		/// </returns>
		public List<DoctorAppointment> GetAppointments() => database.Table<DoctorAppointment>().OrderBy(x => x.Date).ToList();

	
		/// <summary>
		/// Gets today's therapy.
		/// </summary>
		/// <returns></returns>
		public DoctorAppointment GetTodayAppointment() => database.Table<DoctorAppointment>().FirstOrDefault(x => x.Date == DateTime.Now.Date);

		public DoctorAppointment GetLastAppointment()
		{
			DoctorAppointment doctorAppointment = new DoctorAppointment();
			var list = GetAppointments();
			list.Reverse();
			foreach (var tmp in list)
			{
				if (tmp.INR != 0)
				{ 
					doctorAppointment = tmp;
					break;
				}

			}

			return doctorAppointment;
		}

		public DoctorAppointment GetNextAppointment()
		{
			DoctorAppointment doctorAppointment = new DoctorAppointment();
			var list = GetAppointments();
			list.Reverse();
			foreach (var tmp in list)
			{
				if (tmp.INR == 0)
				{
					doctorAppointment = tmp;
					break;
				}

			}

			return doctorAppointment;
		}

		public void DeleteAppointment(DoctorAppointment therapy)
		{
			database.Delete(therapy);
		}

		public void UpdateAppointment(DoctorAppointment therapy)
		{
			database.InsertOrReplace(therapy);
		}

		public void AddAppointment(DoctorAppointment appointment)
		{
			database.Insert(appointment);
		}
	}
}