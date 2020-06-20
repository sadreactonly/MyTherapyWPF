using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MyTherapyWPF.Common;
using Newtonsoft.Json;

namespace SimpleTestClient
{


	class Program
	{
		static void Main(string[] args)
		{
			List<DailyTherapy> therapies = GetList();
			NewMethod(therapies);
		}

		private static void NewMethod(List<DailyTherapy> therapies)
		{
			try
			{
				TcpClient tcpclnt = new TcpClient();
				Console.WriteLine("Connecting.....");

				tcpclnt.Connect("192.168.0.132", 11000);
				
				Stream stm = tcpclnt.GetStream();
				//BinaryFormatter bf = new BinaryFormatter();
				//bf.Serialize(stm, therapies);
				string output = JsonConvert.SerializeObject(therapies);
				byte[] dataBytes = Encoding.Default.GetBytes(output);

				stm.Write(dataBytes, 0, dataBytes.Length);


				Console.WriteLine("Transmitting.....");

				tcpclnt.Close();
			}

			catch (Exception e)
			{
				Console.WriteLine("Error..... " + e.StackTrace);
			}
		}

		private static List<DailyTherapy> GetList()
		{
			return new List<DailyTherapy>()
			{
				new DailyTherapy()
				{
					Id = 1,
					Dose = 2.4
				},
				new DailyTherapy()
				{
					Id = 2,
					Dose = 1.4
				},
				new DailyTherapy()
				{
					Id = 3,
					Dose = 0.4
				},

			};
		}
	}
}
