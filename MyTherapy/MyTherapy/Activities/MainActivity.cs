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

namespace MyTherapy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        TextView todayTherapyText;
        MaterialButton takeTherapyButton;
        TextView lastINR;
        TextView nextAppointment;

        private AppManager appManager; 

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

            appManager = new AppManager(this);
			appManager.TherapyTaken += AppManager_TherapyTaken;       
        }

		private void AppManager_TherapyTaken(object sender, EventArgs e)
		{
            takeTherapyButton.Text = "Taken";
            takeTherapyButton.Enabled = false;
        }

		private void TakeTherapyButton_Click(object sender, EventArgs e)
		{
            var todayTherapy = appManager.GetTodayTherapy();
            todayTherapy.IsTaken = true;
            appManager.TakeTherapy(todayTherapy);
        }

        private void Connect_Click(object sender, EventArgs e)
		{
			Serialize(appManager.GetTherapyChanges());
		}
		
        private void Serialize(List<TherapyChanges> therapies)
		{

			try
			{
				TcpClient tcpClient = new TcpClient();
				tcpClient.Connect("192.168.0.132", 11000);

				Stream stm = tcpClient.GetStream();
                SetChanges(therapies);
                string output = JsonConvert.SerializeObject(therapies);
                byte[] dataBytes = Encoding.Default.GetBytes(output);

                stm.Write(dataBytes, 0, dataBytes.Length);
                tcpClient.Close();
                appManager.DeleteTherpyChanges();
			}
			catch (Exception e)
			{
				Console.WriteLine("Error..... " + e.StackTrace);
			}
		}

		private void SetChanges(List<TherapyChanges> therapies)
		{
			foreach(var tmp in therapies)
			{
                tmp.Therapy = appManager.GetTherapyById(tmp.TherapyGuid);
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
			appManager.SetAllData(out string lastInrText, out string nextAppointmentText, out string todayTherapyTextText, out bool takeTherapyButtonEnabled);

			lastINR.Text = lastInrText;
            nextAppointment.Text = nextAppointmentText;
            todayTherapyText.Text = todayTherapyTextText;
            takeTherapyButton.Enabled = !takeTherapyButtonEnabled;

            base.OnResume();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}