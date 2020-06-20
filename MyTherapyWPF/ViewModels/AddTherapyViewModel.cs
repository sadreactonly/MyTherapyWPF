using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using MyTherapyWPF.Commands;
using System.Collections.ObjectModel;
using MyTherapyWPF.Common;
using System.Windows;
using MyTherapyWPF.Contexts;

namespace MyTherapyWPF.ViewModels
{
	public class AddTherapyViewModel 
	{
		AppDbContext db = new AppDbContext();
		public ObservableCollection<double> Schematic { get; set; }
		public ObservableCollection<DailyTherapy> Therapies { get; set; }

		private RelayCommand _addCommand;

		private RelayCommand _generateCommand;

		public string Dosage { get; set; }
		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime EndDate { get; set; } = DateTime.Now;

		public AddTherapyViewModel()
		{
			Schematic = new ObservableCollection<double>();
			Therapies = new ObservableCollection<DailyTherapy>(db.Therapies);
		}

		public ICommand AddCommand
		{
			get
			{
				if (_addCommand == null)
				{
					_addCommand = new RelayCommand(SaveCommandAction);
				}
				return _addCommand;
			}
		}

		public ICommand GenerateCommand
		{
			get
			{
				if (_generateCommand == null)
				{
					_generateCommand = new RelayCommand(GenerateCommandAction);
				}
				return _generateCommand;
			}
		}

		private void GenerateCommandAction(object obj)
		{
			if(StartDate!=null && EndDate!=null)
			{
				CreateScheme();
			}
		}

		private void SaveCommandAction(object obj)
		{
			if (double.TryParse(Dosage, out double dose))
				Schematic.Add(dose);
			else
				MessageBox.Show("Wrong format!");
		}


		/// <summary>
		/// Create therapy schema based on dosage schema and dates.
		/// </summary>
		private void CreateScheme()
		{
			List<DateTime> dates = new List<DateTime>();
			for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1))
				dates.Add(date);

			int x = dates.Count / Schematic.Count + 1;

			var newList = new List<double>();

			for (int i = 0; i < x; i++)
			{
				newList.AddRange(Schematic);
			}

			for (int i = 0; i < dates.Count; i++)
			{
				var therapy = new DailyTherapy() { Date = dates[i], Dose = newList[i] };
				Therapies.Add(therapy);
			}


			db.Therapies.AddRange(Therapies);
			db.SaveChanges();
		}

	}
}
