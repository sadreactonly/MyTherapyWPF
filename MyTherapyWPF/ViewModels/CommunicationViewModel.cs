using MyTherapyWPF.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using Common.Models;
using System.Collections.ObjectModel;
using MyTherapyWPF.TcpServer;

namespace MyTherapyWPF.ViewModels
{
	public sealed class CommunicationViewModel :INotifyPropertyChanged,IDisposable
	{
		readonly DatabaseManager db =  DatabaseManager.Instance;
		private RelayCommand startServiceCommand;
		private RelayCommand stopServiceCommand;
		private RelayCommand generateExcelTableCommand;

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
				OnPropertyChanged(nameof(IsStarted));
			}
		}
		public bool IsStartButtonEnabled
		{
			get => isStartButtonEnabled;
			set
			{
				isStartButtonEnabled = value;
				OnPropertyChanged(nameof(IsStartButtonEnabled));
			}
		}
		public bool IsStopButtonEnabled
		{
			get => isStopButtonEnabled;
			set
			{
				isStopButtonEnabled = value;
				OnPropertyChanged(nameof(IsStopButtonEnabled));
			}
		}
		
		public RelayCommand StopServiceCommand => stopServiceCommand ?? (stopServiceCommand = new RelayCommand(StopServiceAction));

		public RelayCommand StartServiceCommand => startServiceCommand ?? (startServiceCommand = new RelayCommand(StartServiceAction));

		public RelayCommand GenerateExcelTableCommand => generateExcelTableCommand ?? (generateExcelTableCommand = new RelayCommand(GenerateExcelFile));

		private void GenerateExcelFile(object obj)
		{
			ExcelFileGenerator.Generate(db.GetTherapies());
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
			service.TherapiesReceived += TherapiesReceived;	
			service.StartListening();
		}

		private void TherapiesReceived(List<TherapyChanges> therapyChanges)
		{
			var x = therapyChanges;

			foreach(var tmp in therapyChanges)
			{
				switch (tmp.Operation)
				{
					case Operation.Add:
						db.AddTherapy(tmp.Therapy);
						break;
					case Operation.Update:
						db.UpdateTherapy(tmp.Therapy);
						break;
					case Operation.Delete:
						db.DeleteTherapy(tmp.TherapyGuid);
						break;
				}
			}	
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

		public void Dispose()
		{
			this.Dispose();
		}
	}
}
