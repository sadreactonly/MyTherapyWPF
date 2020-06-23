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
	public class TherapiesViewModel : INotifyPropertyChanged
	{
		readonly DatabaseManager DatabaseManager;
	
		private RelayCommand addCommand;
		private RelayCommand generateCommand;
		private RelayCommand takeCommand;
		private RelayCommand editCommand;
		private RelayCommand deleteCommand;

		private ObservableCollection<DailyTherapy> therapies; 

		public DailyTherapy SelectedItem { get; set; }
		public string Dosage { get; set; }
		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime EndDate { get; set; } = DateTime.Now;

		public ObservableCollection<double> Schematic { get; set; }
		public ObservableCollection<DailyTherapy> Therapies
		{
			get => therapies;
			set
			{
				therapies = value;
				OnPropertyChanged(nameof(Therapies));
			}
		}

		public TherapiesViewModel()
		{
			DatabaseManager = DatabaseManager.Instance;
			DatabaseManager.DbUpdatedEvent += RefreshGui;
			Schematic = new ObservableCollection<double>();
			Therapies = new ObservableCollection<DailyTherapy>(DatabaseManager.GetTherapies());
		}


		public ICommand AddCommand => addCommand ?? (addCommand = new RelayCommand(SaveCommandAction));

		public ICommand GenerateCommand => generateCommand ?? (generateCommand = new RelayCommand(GenerateCommandAction));

		public ICommand TakeCommand => takeCommand ?? (takeCommand = new RelayCommand(TakeAction));

		public ICommand EditCommand => editCommand ?? (editCommand = new RelayCommand(EditAction));

		public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new RelayCommand(DeleteAction));

		private void TakeAction(object obj)
		{
			if (SelectedItem != null)
				DatabaseManager.TakeTherapy(SelectedItem);
		}
		private void GenerateCommandAction(object obj)
		{
			CreateScheme();
		}
		private void SaveCommandAction(object obj)
		{
			if (double.TryParse(Dosage, out double dose))
				Schematic.Add(dose);
			else
				MessageBox.Show("Wrong format!");
		}
		private void DeleteAction(object obj)
		{
			DatabaseManager.DeleteTherapy(SelectedItem);
			Therapies.Remove(SelectedItem);
		}
		private void EditAction(object obj)
		{
			throw new NotImplementedException();
		}

		private void RefreshGui()
		{
			Therapies = new ObservableCollection<DailyTherapy>(DatabaseManager.GetTherapies());
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


			DatabaseManager.AddTherapies(Therapies);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
