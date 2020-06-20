using MyTherapyWPF.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyTherapyWPF.ViewModels
{
	public class CommunicationViewModel
	{
		private RelayCommand startServiceCommand;
		private RelayCommand stopServiceCommand;
		private Thread thread;
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
			AsynchronousSocketListener.StartListening();
		}

		private void StopServiceAction(object obj)
		{
			if(thread.IsAlive)
			{
				thread.Abort();
			}
		}
	}
}
