using System;
using System.Collections.Generic;
using SQLite;
using PCLStorage;
using Common.Models;

namespace MyTherapy
{
	/// <summary>
	/// Represent class for database of all therapies.
	/// </summary>
	public class TherapyDatabase 
	{
		readonly SQLiteConnection database;
		private static TherapyDatabase _instance;
		private static readonly object padlock = new object();

		public TherapyDatabase()
		{
			database = GetConnection();
			database.CreateTable<DailyTherapy>();

		}

		/// <summary>
		/// Gets connection for database.
		/// </summary>
		/// <returns></returns>
		public SQLiteConnection GetConnection()
		{
			var sqliteFilename = "TherapiesDb.db3";
			IFolder folder = FileSystem.Current.LocalStorage;
			string path = PortablePath.Combine(folder.Path, sqliteFilename);
			var sqLiteConnection = new SQLiteConnection(path);
			return sqLiteConnection;
		}

		/// <summary>
		/// Creates instance of TherapyDatabase class.
		/// </summary>
		public static TherapyDatabase Instance
		{
			get
			{
				lock (padlock)
					return _instance ??= new TherapyDatabase();
			}
		}

		/// <summary>
		/// Get all therapies from database.
		/// </summary>
		/// <returns>
		/// List of therapies.
		/// </returns>
		public List<DailyTherapy> GetTherapies() => database.Table<DailyTherapy>().ToList();

		/// <summary>
		/// Add all therapies in database.
		/// </summary>
		/// <param name="therapiesSchema"></param>
		public void AddTherapySchema(List<DailyTherapy> therapiesSchema)
		{
			database.InsertAll(therapiesSchema);
		}

		public void DeleteTherapy(DailyTherapy therapy)
		{
			database.Delete(therapy);
		}

		public void UpdateTherapy(DailyTherapy therapy)
		{
			database.InsertOrReplace(therapy);
		}

		internal DailyTherapy GetTherapy(Guid id)
		{
			return database.Table<DailyTherapy>().FirstOrDefault(x => x.Guid == id);
		}
	}
}