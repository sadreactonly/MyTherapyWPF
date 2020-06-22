using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using MyTherapyWPF.Commands;
using System.Collections.ObjectModel;
using MyTherapyWPF.Common;
using System.Windows;

namespace MyTherapyWPF.ViewModels
{
	public class TherapiesViewModel : INotifyPropertyChanged
	{
		DatabaseManager db = DatabaseManager.Instance;
	
		private RelayCommand _addCommand;
		private RelayCommand _generateCommand;
		private RelayCommand _takeCommand;
		private RelayCommand _editCommand;
		private RelayCommand _deleteCommand;

		private ObservableCollection<DailyTherapy> _therapies; 

		public DailyTherapy SelectedItem { get; set; }
		public string Dosage { get; set; }
		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime EndDate { get; set; } = DateTime.Now;

		public ObservableCollection<double> Schematic { get; set; }
		public ObservableCollection<DailyTherapy> Therapies
		{
			get => _therapies;
			set
			{
				_therapies = value;
				OnPropertyChanged(nameof(Therapies));
			}
		}

		public TherapiesViewModel()
		{
			db = DatabaseManager.Instance;
			db.DbUpdatedEvent += RefreshGUI;
			Schematic = new ObservableCollection<double>();
			Therapies = new ObservableCollection<DailyTherapy>(db.GetTherapies());
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
		public ICommand TakeCommand
		{
			get
			{
				if (_takeCommand == null)
				{
					_takeCommand = new RelayCommand(TakeAction);
				}
				return _takeCommand;
			}
		}
		public ICommand EditCommand
		{
			get
			{
				if (_editCommand == null)
				{
					_editCommand = new RelayCommand(EditAction);
				}
				return _editCommand;
			}
		}
		public ICommand DeleteCommand
		{
			get
			{
				if (_deleteCommand == null)
				{
					_deleteCommand = new RelayCommand(DeleteAction);
				}
				return _deleteCommand;
			}
		}
		
		private void TakeAction(object obj)
		{
			if (SelectedItem != null)
				db.TakeTherapy(SelectedItem);
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
		private void DeleteAction(object obj)
		{
			db.DeleteTherapy(SelectedItem);
			Therapies.Remove(SelectedItem);
		}
		private void EditAction(object obj)
		{
			throw new NotImplementedException();
		}

		private void RefreshGUI()
		{
			Therapies = new ObservableCollection<DailyTherapy>(db.GetTherapies());
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


			db.AddTherapies(Therapies);
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
