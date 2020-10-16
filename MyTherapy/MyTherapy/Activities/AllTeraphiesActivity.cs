using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Common.Models;

namespace MyTherapy.Activities
{
	[Activity(Label = "AllTherapiesActivity")]
	public class AllTherapiesActivity : Activity
	{
        private AppManager appManager;

        ListView listView;
		SchemaAdapter adapter;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			SetContentView(Resource.Layout.therapies_list_card);
			base.OnCreate(savedInstanceState);

			listView = FindViewById<ListView>(Resource.Id.mainlistview);
			listView.ItemLongClick += ListView_ItemLongClick;
            appManager = new AppManager(this);
		}

		protected override void OnResume()
		{
			base.OnResume();
            var allTherapies = appManager.GetTherapies();
            adapter = new SchemaAdapter(this, allTherapies);
            listView.Adapter = adapter;

        }
        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {

            Android.Support.V7.App.AlertDialog.Builder alert = new Android.Support.V7.App.AlertDialog.Builder(this);
            DailyTherapy item = adapter.GetFromItem(e.Position);
            DailyTherapy dbsb = (DailyTherapy) item.Clone();


            alert.SetTitle($"Delete { item.Date.ToShortDateString()} therapy");
            alert.SetMessage("Do you want to delete item?");

            alert.SetPositiveButton("Yes", (senderAlert, args) =>
            {
                appManager.DeleteTherapy(item);
                adapter.RemoveItemAt(e.Position);
                adapter.NotifyDataSetChanged();
            });

            alert.SetNegativeButton("No", (senderAlert, args) =>
            {

            });

            Dialog dialog = alert.Create();
            dialog.Show();

        }

    }
}