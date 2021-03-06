﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.ComponentModel;
using MyTherapyWPF.Commands;
using System.Collections.ObjectModel;
using Common.Models;
using System.Windows;
using MyTherapyWPF.Views;
using System.Globalization;

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
		private bool deleteIsEnabled = false;
		private bool takeIsEnabled = false;
		private bool editIsEnabled = false;
		private ObservableCollection<DailyTherapy> therapies;
		private ObservableCollection<decimal> schematic;
		private string dosage = "0";
		private DailyTherapy selectedItem;

		public DailyTherapy SelectedItem
		{
			get { return selectedItem; }
			set
			{
				selectedItem = value;
				TakeIsEnabled = true;
				DeleteIsEnabled = true;
				EditIsEnabled = true;
			}
		}
		public DateTime StartDate { get; set; } = DateTime.Now;
		public DateTime EndDate { get; set; } = DateTime.Now;

		public string Dosage
		{
			get =>  dosage;
			set
			{
				dosage = value;
				OnPropertyChanged(nameof(Dosage));
			}
		}

		public ObservableCollection<decimal> Schematic
		{
			get => schematic;
			set
			{
				schematic = value;
				OnPropertyChanged(nameof(Schematic));
			}
		}
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
			Schematic = new ObservableCollection<decimal>();
			Therapies = new ObservableCollection<DailyTherapy>(DatabaseManager.GetTherapies());
		}

		public bool DeleteIsEnabled
		{
			get => deleteIsEnabled;
			set
			{
				deleteIsEnabled = value;
				OnPropertyChanged(nameof(DeleteIsEnabled));

			}
		}

		public bool EditIsEnabled
		{
			get => editIsEnabled;
			set
			{
				editIsEnabled = value;
				OnPropertyChanged(nameof(EditIsEnabled));

			}
		}

		public bool TakeIsEnabled
		{
			get => takeIsEnabled;
			set
			{
				takeIsEnabled = value;
				OnPropertyChanged(nameof(TakeIsEnabled));

			}
		}

		public ICommand AddCommand => addCommand ?? (addCommand = new RelayCommand(SaveCommandAction));

		public ICommand GenerateCommand => generateCommand ?? (generateCommand = new RelayCommand(GenerateCommandAction));

		public ICommand TakeCommand => takeCommand ?? (takeCommand = new RelayCommand(TakeAction));

		public ICommand EditCommand => editCommand ?? (editCommand = new RelayCommand(EditAction));

		public ICommand DeleteCommand => deleteCommand ?? (deleteCommand = new RelayCommand(DeleteAction));

		private void TakeAction(object obj)
		{
			if (SelectedItem != null)
			{
				DatabaseManager.TakeTherapy(SelectedItem);
				TakeIsEnabled = false;
			}
				
		}
		private void GenerateCommandAction(object obj)
		{
			CreateScheme();
			Schematic.Clear();
			Dosage = "0";
		}
		private void SaveCommandAction(object obj)
		{
			if (decimal.TryParse(Dosage,NumberStyles.Any,CultureInfo.InvariantCulture, out decimal dose))
				Schematic.Add(dose);
			else
				MessageBox.Show("Wrong format!");
		}
		private void DeleteAction(object obj)
		{
			DatabaseManager.DeleteTherapy(SelectedItem);
			Therapies.Remove(SelectedItem);

			DeleteIsEnabled = false;
			EditIsEnabled = false;
		}
		private void EditAction(object obj)
		{
			TherapyEditWindow win = new TherapyEditWindow();
			TherapyEditViewModel tevm = new TherapyEditViewModel(SelectedItem);
			tevm.Saved += delegate (object sender, EventArgs e) { win.Close(); };
			win.DataContext = tevm;
			win.Show();	
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

			int i = 0;
			for (DateTime date = StartDate; date <= EndDate; date = date.AddDays(1), i++)
			{
				if (i == Schematic.Count)
				{
					i = 0;
				}

				var therapy = new DailyTherapy() { Guid = Guid.NewGuid(), Date = date, Dose = Schematic[i] };
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
