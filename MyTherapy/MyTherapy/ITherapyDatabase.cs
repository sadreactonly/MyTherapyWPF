using MyTherapyWPF.Common;
using System.Collections.Generic;

namespace MyTherapy
{
	public interface ITherapyDatabase
	{
		public List<DailyTherapy> GetTherapies();
		public void AddTherapySchema(List<DailyTherapy> therapiesSchema);
		DailyTherapy GetTodayTherapy();
		void UpdateTherapy(DailyTherapy therapy);
	}
}