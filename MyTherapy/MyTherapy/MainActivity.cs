using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Button;
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
        TherapyDatabase db = TherapyDatabase.Instance;
        ListView listView;
        TextView todayTherapyText;
        MaterialButton takeTherapyButton;
        DailyTherapy todayTherapy;
        SchemaAdapter adapter;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            MaterialButton connect = FindViewById<MaterialButton>(Resource.Id.button1);
			connect.Click += Connect_Click;
            listView = FindViewById<ListView>(Resource.Id.mainlistview);
			listView.ItemLongClick += ListView_ItemLongClick;
            todayTherapyText = FindViewById<TextView>(Resource.Id.textView2);
            takeTherapyButton = FindViewById<MaterialButton>(Resource.Id.materialButton1);
			takeTherapyButton.Click += TakeTherapyButton_Click;
            db.TherapyTakenEvent += TherapyTaken;
            todayTherapy = db.GetTodayTherapy();
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            
				Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
                DailyTherapy item = adapter.GetFromItem(e.Position);

                alert.SetTitle($"Delete { item.Date.ToShortDateString()} therapy");
                alert.SetMessage("Do you want to delete item?");

                alert.SetPositiveButton("Yes", (senderAlert, args) =>
                {
                    db.DeleteTherapy(item);
                    adapter.RemoveItemAt(e.Position);
                    adapter.NotifyDataSetChanged();
                });

                alert.SetNegativeButton("No", (senderAlert, args) =>
                {
                  
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            
		}

		private void TherapyTaken()
		{

		}

		private void TakeTherapyButton_Click(object sender, EventArgs e)
		{
            todayTherapy.IsTaken = true;
            db.UpdateTherapy(todayTherapy);
            adapter.NotifyDataSetChanged();
            takeTherapyButton.Text = "Taken";
            takeTherapyButton.Enabled = false;

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
            todayTherapy = allTherapies.Where(x => x.Date.Date == DateTime.Now.Date).FirstOrDefault();
            if (todayTherapy != null)
			{
                todayTherapyText.Text = todayTherapy.Dose.ToString();
                takeTherapyButton.Enabled = !todayTherapy.IsTaken;
            }
            else
			{
                todayTherapyText.Text = "None.";
                takeTherapyButton.Enabled = false;
            }

            adapter = new SchemaAdapter(this, allTherapies);
            listView.Adapter = adapter;

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
