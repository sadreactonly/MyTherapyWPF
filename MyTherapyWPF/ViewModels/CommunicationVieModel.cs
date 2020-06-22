using MyTherapyWPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyTherapyWPF;
using System.ComponentModel;
using MyTherapyWPF.Common;
using System.Windows;
using System.Data.Entity;
using System.Collections.ObjectModel;

namespace MyTherapyWPF.ViewModels
{
	public class CommunicationViewModel :INotifyPropertyChanged
	{
		DatabaseManager db =  DatabaseManager.Instance;
		private RelayCommand startServiceCommand;
		private RelayCommand stopServiceCommand;
		private Thread thread;
		private bool isStarted;
		private bool isStartButtonEnabled;
		private bool isStopButtonEnabled;
		private AsynchronousSocketListener service;
	
		public CommunicationViewModel()
		{
			IsStarted = false;
			IsStartButtonEnabled = true;
			IsStopButtonEnabled = false;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public bool IsStarted
		{
			get => isStarted;
			set
			{
				isStarted = value;
				OnPropertyChanged("IsStarted");
			}
		}
		public bool IsStartButtonEnabled
		{
			get => isStartButtonEnabled;
			set
			{
				isStartButtonEnabled = value;
				OnPropertyChanged("IsStartButtonEnabled");
			}
		}
		public bool IsStopButtonEnabled
		{
			get => isStopButtonEnabled;
			set
			{
				isStopButtonEnabled = value;
				OnPropertyChanged("IsStopButtonEnabled");
			}
		}
		
		public RelayCommand StopServiceCommand
		{
			get
			{
				if (stopServiceCommand == null)
				{
					stopServiceCommand = new RelayCommand(StopServiceAction);
				}
				return stopServiceCommand;
			}
		}
		public RelayCommand StartServiceCommand
		{
			get
			{
				if (startServiceCommand == null)
				{
					startServiceCommand = new RelayCommand(StartServiceAction);
				}
				return startServiceCommand;
			}
		}

		private void StartServiceAction(object obj)
		{
			thread = new Thread(StartService);
			thread.Start();
		}

		private void StartService()
		{
			service = new AsynchronousSocketListener();
			service.ConnectedEvent += Connected;
			service.TherapiesReceived += TherapiesRecieved;	
			service.StartListening();
		}

		private void TherapiesRecieved(List<DailyTherapy> dailyTherapies)
		{
			MessageBox.Show($"Therapies recieved. Count:{dailyTherapies.Count}");
			db.AddTherapies(new ObservableCollection<DailyTherapy>(dailyTherapies)); 
		}

		private void Connected()
		{
			IsStarted = true;
			IsStopButtonEnabled = true;
			IsStartButtonEnabled = false;
		}

		private void StopServiceAction(object obj)
		{
			if(thread.IsAlive)
			{
				service.Stop();
				thread.Abort();
				IsStarted = false;
				IsStopButtonEnabled = false;
				IsStartButtonEnabled = true;
			}
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
