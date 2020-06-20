using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using PCLStorage;
using MyTherapyWPF.Common;

namespace MyTherapy
{
	/// <summary>
	/// Represent class for database of all therapies.
	/// </summary>
	public class TherapyDatabase : ITherapyDatabase
	{
		SQLiteConnection database;
		private static TherapyDatabase instance = null;
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
			SQLiteConnection sqlitConnection;
			var sqliteFilename = "Therapys.db3";
			IFolder folder = FileSystem.Current.LocalStorage;
			string path = PortablePath.Combine(folder.Path.ToString(), sqliteFilename);
			sqlitConnection = new SQLiteConnection(path);
			return sqlitConnection;
		}

		/// <summary>
		/// Creates instance of TherapyDatabase class.
		/// </summary>
		public static TherapyDatabase Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
					{
						instance = new TherapyDatabase();
					}
					return instance;
				}
			}
		}

		/// <summary>
		/// Get all therapies from database.
		/// </summary>
		/// <returns>
		/// List of therapies.
		/// </returns>
		public List<DailyTherapy> GetTherapies()
		{
			return database.Table<DailyTherapy>().ToList();
		}

		/// <summary>
		/// Add all therapies in database.
		/// </summary>
		/// <param name="therapiesSchema"></param>
		public void AddTherapySchema(List<DailyTherapy> therapiesSchema)
		{
			database.InsertAll(therapiesSchema);
		}

		/// <summary>
		/// Gets todays therapy.
		/// </summary>
		/// <returns></returns>
		public DailyTherapy GetTodayTherapy()
		{
			return database.Table<DailyTherapy>().ToList().Where(x => x.Date == DateTime.Now.Date).FirstOrDefault();

		}
		//public List<DailyTherapy> GetTherapys()
		//{
		//	var list = database.Table<DailyTherapy>().ToList();
		//	list.Sort((x, y) => y.StartTime.CompareTo(x.StartTime));
		//	return list;
		//}
		//public List<DailyTherapy> GetTherapysFromDay(DateTime date)
		//{
		//	var list = database.Table<DailyTherapy>().ToList().Where(x => x.Date == date).ToList();
		//	list.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
		//	return list;
		//}
		//public long AddTherapy(DailyTherapy item)
		//{
		//	database.Insert(item);
		//	return item.Id;
		//}
		//public long UpdateTherapy(DailyTherapy item)
		//{
		//	database.InsertOrReplace(item);
		//	return item.Id;
		//}
		//public int DeleteTherapy(long id)
		//{

		//	return database.Delete<DailyTherapy>(id);

		//}

		//public int DeleteTherapys()
		//{
		//	return database.DeleteAll<DailyTherapy>();
		//}

	}
}