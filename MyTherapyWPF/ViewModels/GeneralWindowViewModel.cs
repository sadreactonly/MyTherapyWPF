using Common.Models;

namespace MyTherapyWPF.ViewModels
{
	public class GeneralWindowViewModel
	{
        private readonly DatabaseManager dbManager = new DatabaseManager();
        public DoctorAppointment LastDoctorAppointment { get; set; }
        public DoctorAppointment NextDoctorAppointment { get; set; }

        public DailyTherapy DailyTherapy { get; set; }

        public GeneralWindowViewModel()
		{
            LastDoctorAppointment = dbManager.GetLastDoctorAppointment();
            NextDoctorAppointment = dbManager.GetNextDoctorAppointment();
            DailyTherapy = dbManager.GetDailyTherapy();
		}

    }

}
