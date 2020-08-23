using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Button;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Common.Models;
using Newtonsoft.Json;
using MyTherapy.Activities;
using MyAppointment;

namespace MyTherapy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TherapyDatabase db = TherapyDatabase.Instance;
        DoctorAppointmentDatabase inrDatabase = DoctorAppointmentDatabase.Instance;
        TextView todayTherapyText;
        MaterialButton takeTherapyButton;
        DailyTherapy todayTherapy;
        TextView lastINR;
        TextView nextAppointment;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            MaterialButton connect = FindViewById<MaterialButton>(Resource.Id.button1);
			connect.Click += Connect_Click;
           
            todayTherapyText = FindViewById<TextView>(Resource.Id.textView2);
            takeTherapyButton = FindViewById<MaterialButton>(Resource.Id.materialButton1);
            lastINR = FindViewById<TextView>(Resource.Id.textView5);
            nextAppointment = FindViewById<TextView>(Resource.Id.textView6);

            takeTherapyButton.Click += TakeTherapyButton_Click;
            db.TherapyTakenEvent += TherapyTaken;
            todayTherapy = db.GetTodayTherapy();
        }


		private void TherapyTaken()
		{

		}

		private void TakeTherapyButton_Click(object sender, EventArgs e)
		{
            todayTherapy.IsTaken = true;
            db.UpdateTherapy(todayTherapy);
           // adapter.NotifyDataSetChanged();
            takeTherapyButton.Text = "Taken";
            takeTherapyButton.Enabled = false;

        }

        private void Connect_Click(object sender, EventArgs e)
		{
			Serialize(db.GetTherapies());
		}
		private static void Serialize(List<DailyTherapy> therapies)
		{

			try
			{
				TcpClient tcpClient = new TcpClient();
				tcpClient.Connect("192.168.0.132", 11000);

				Stream stm = tcpClient.GetStream();

                string output = JsonConvert.SerializeObject(therapies);
                byte[] dataBytes = Encoding.Default.GetBytes(output);

                stm.Write(dataBytes, 0, dataBytes.Length);
                tcpClient.Close();
			}
			catch (Exception e)
			{
				Console.WriteLine("Error..... " + e.StackTrace);
			}
		}


		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
	        int id = item.ItemId;
	        switch (id)
	        {
                case Resource.Id.action_schema:
                    StartActivity(typeof(SchemaActivity));
                    break;
                case Resource.Id.action_inr:                
			        StartActivity(typeof(InrActivity));
			        break;
		        case Resource.Id.action_therapies:
			        StartActivity(typeof(AllTherapiesActivity));
			        break;
	        }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            var allTherapies = db.GetTherapies();
            var lastAppointment = inrDatabase.GetLastAppointment();
            lastINR.Text = lastAppointment.INR.ToString();
            nextAppointment.Text = inrDatabase.GetNextAppointment().Date.ToShortDateString();
            todayTherapy = allTherapies.FirstOrDefault(x => x.Date.Date == DateTime.Now.Date);
            if (todayTherapy != null)
			{
                todayTherapyText.Text = todayTherapy.Dose.ToString(CultureInfo.InvariantCulture);
                takeTherapyButton.Enabled = !todayTherapy.IsTaken;
            }
            else
			{
                todayTherapyText.Text = "None.";
                takeTherapyButton.Enabled = false;
            }

           
            base.OnResume();

        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
