using MyTherapyWPF.Contexts;
using MyTherapyWPF.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyTherapyWPF.ViewModels
{
	public class GeneralWindowViewModel
	{
        DatabaseManager dbManager = new DatabaseManager();
        public DoctorAppointment LastDoctorAppointment { get; set; }
        public DoctorAppointment NextDoctorAppointment { get; set; }

        public DailyTherapy DailyTherapy { get; set; }

        public GeneralWindowViewModel()
		{
            LastDoctorAppointment = new DoctorAppointment()
            {
                INR = 2.50,
                Date = DateTime.Now
            };

            DailyTherapy = dbManager.GetDailyTherapy();
		}

    }

}
