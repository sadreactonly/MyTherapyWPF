using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using MyTherapyWPF.Common;
using Newtonsoft.Json;

namespace MyTherapy
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ITherapyDatabase db = TherapyDatabase.Instance;
        ListView listView;
        TextView todayTherapyText;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            Button connect = FindViewById<Button>(Resource.Id.button1);
			connect.Click += Connect_Click;
            listView = FindViewById<ListView>(Resource.Id.mainlistview);
            todayTherapyText = FindViewById<TextView>(Resource.Id.textView2);
        }

		private void Connect_Click(object sender, EventArgs e)
		{
			Seriealize(db.GetTherapies());
		}
		private static void Seriealize(List<DailyTherapy> therapies)
		{

			try
			{
				TcpClient tcpclnt = new TcpClient();
				tcpclnt.Connect("192.168.0.132", 11000);

				Stream stm = tcpclnt.GetStream();

                string output = JsonConvert.SerializeObject(therapies);
                byte[] dataBytes = Encoding.Default.GetBytes(output);

                stm.Write(dataBytes, 0, dataBytes.Length);
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
		public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            var allTherapies = db.GetTherapies();
            if (allTherapies.Count > 0)
                todayTherapyText.Text = db.GetTodayTherapy().Dose.ToString();
            else
                todayTherapyText.Text = "None.";

            listView.Adapter = new SchemaAdapter(this, allTherapies);

            base.OnResume();

        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            StartActivity(typeof(SchemaActivity));
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
	}
}
