using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using MyTherapyWPF.Commands;
using System.Collections.ObjectModel;
using Common.Models;
using System.Windows;

namespace MyTherapyWPF.ViewModels
{
	public class TherapyEditViewModel
	{
		readonly DatabaseManager DatabaseManager;

		public event EventHandler Saved;

		private RelayCommand addCommand;
		private DailyTherapy therapy;

		public DateTime Date { get; set; }
		public decimal Dose{ get; set; }
		public bool IsTaken { get; set; }

		public TherapyEditViewModel(DailyTherapy therapy)
		{
			this.therapy = therapy??default;

			Date = therapy.Date;
			Dose = therapy.Dose;
			IsTaken = therapy.IsTaken;

			DatabaseManager = DatabaseManager.Instance;
		}

		public ICommand UpdateCommand => addCommand ?? (addCommand = new RelayCommand(SaveCommandAction));
		
		private void SaveCommandAction(object obj)
		{
			therapy.Date=Date;
		    therapy.Dose = Dose;
			therapy.IsTaken = IsTaken;

			DatabaseManager.UpdateTherapy(therapy);

			Saved.Invoke(this,null);
		}
	}
}
