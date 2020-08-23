using Common.Models;
using System.Collections.Generic;

namespace MyTherapy
{
	public interface ITherapyDatabase
	{
		public List<DailyTherapy> GetTherapies();
		public void AddTherapySchema(List<DailyTherapy> therapiesSchema);
		public DailyTherapy GetTodayTherapy();
		void UpdateTherapy(DailyTherapy therapy);
	}
}