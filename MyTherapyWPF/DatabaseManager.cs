using MyTherapyWPF.Contexts;
using MyTherapyWPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTherapyWPF
{
	public class DatabaseManager
	{
		private AppDbContext db = AppDbContext.Instance;

		public DailyTherapy GetDailyTherapy()
		{
			var therapies = db.Therapies.ToList();
			return therapies.Where(x => x.Date.Date == DateTime.Now.Date).FirstOrDefault();
		}
	}
}
