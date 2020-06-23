using MyTherapyWPF.Common;
using Newtonsoft.Json;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace MyTherapyWPF {
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket WorkSocket { get; set; } = null;
        // Size of receive Buffer.  
        public const int BufferSize = 1024*1024;
        // Receive Buffer.  
        public byte[] Buffer { get; set; }  = new byte[BufferSize];
        // Received data string.  
        public StringBuilder Sb { get; set; } = new StringBuilder();


    }

    public sealed class AsynchronousSocketListener : IDisposable
	{
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        public bool IsConnected { get; set; } = false;
        public bool IsStarted { get; set; } = false;

        public delegate void StartedEventHandler();
        public delegate void OnRecievedEventHandler(List<DailyTherapy> dailyTherapies);

        public StartedEventHandler ConnectedEvent;
        public OnRecievedEventHandler TherapiesReceived;

        Socket listener;
        public AsynchronousSocketListener()
        {

        }

        public  void StartListening()
        {
 
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[9];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            var ip = ipAddress.ToString();
             listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);
                IsStarted = true;
                ConnectedEvent?.Invoke();
                while (true)
                {
                    allDone.Reset();

                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    allDone.WaitOne();
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }


        }

		public void Stop()
		{

            listener.Close();
		}

		public  void AcceptCallback(IAsyncResult ar)
        {
			try
			{
                allDone.Set();

                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

				StateObject state = new StateObject
				{
					WorkSocket = handler
				};
				handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
            }
            catch(Exception ex)
			{
                Console.WriteLine(ex);
			}
            
        }

        public  void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;
            StateObject state = new StateObject();
  
            try
			{
                state = (StateObject)ar.AsyncState;

            }
            catch(ArgumentNullException ex)
			{
                Console.WriteLine( ex);
                return;
			}
            Socket handler = state.WorkSocket;

            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                state.Sb.Append(Encoding.ASCII.GetString(
                    state.Buffer, 0, bytesRead));

                content = state.Sb.ToString();

                if (content.IndexOf("]", StringComparison.CurrentCulture)>-1)
                {
                    TherapiesReceived?.Invoke(JsonConvert.DeserializeObject<List<DailyTherapy>>(content));

                }
                else
                {
                    handler.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

		public void Dispose()
		{
            listener.Dispose();
		}

		//private  void Send(Socket handler, String data)
		//{
		//    // Convert the string data to byte data using ASCII encoding.  
		//    byte[] byteData = Encoding.ASCII.GetBytes(data);

		//    // Begin sending the data to the remote device.  
		//    handler.BeginSend(byteData, 0, byteData.Length, 0,
		//        new AsyncCallback(SendCallback), handler);
		//}

		//private  void SendCallback(IAsyncResult ar)
		//{
		//    try
		//    {
		//        // Retrieve the socket from the state object.  
		//        Socket handler = (Socket)ar.AsyncState;

		//        // Complete sending the data to the remote device.  
		//        int bytesSent = handler.EndSend(ar);
		//        Console.WriteLine("Sent {0} bytes to client.", bytesSent);

		//        handler.Shutdown(SocketShutdown.Both);
		//        handler.Close();

		//    }
		//    catch (Exception e)
		//    {
		//        Console.WriteLine(e.ToString());
		//    }
		//}

	}
}
