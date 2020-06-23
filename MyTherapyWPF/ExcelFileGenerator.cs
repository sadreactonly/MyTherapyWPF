using System.Collections.Generic;
using System.Reflection;
using System.Data;
using Common.Models;
using System.Windows;
using ClosedXML.Excel;
namespace MyTherapyWPF
{
	public static class ExcelFileGenerator
	{
		private static  DataTable ConvertToDataTable<T>(List<T> models)
		{
			DataTable dataTable = new DataTable(typeof(T).Name);

			PropertyInfo[] propertyInfos = typeof(T).GetProperties(BindingFlags.Public|BindingFlags.Instance|BindingFlags.NonPublic);

			foreach (var prop in propertyInfos)
			{
				dataTable.Columns.Add(prop.Name);
			}
			
			foreach(var item in models)
			{
				var values = new object[propertyInfos.Length];
				for (int i = 0; i < propertyInfos.Length; i++)
				{
					values[i] = propertyInfos[i].GetValue(item, null);
				}
				dataTable.Rows.Add(values);
			}
			return dataTable;
		}

		public static void Generate(List<DailyTherapy> models)
		{
			using (var wb = new XLWorkbook())
			{ 
				if (models == null) 
					return;

				DataTable dt = ConvertToDataTable(models);
				wb.Worksheets.Add(dt, "Therapies");
				wb.SaveAs("MyInrReport.xlsx");
				dt.Dispose();
				wb.Dispose();
			}
			MessageBox.Show(".xlsx is generated.");
		}
	}
}
