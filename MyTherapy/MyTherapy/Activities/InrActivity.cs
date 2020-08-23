using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Support.Design.Button;
using Android.Widget;
using Common.Models;
using MyAppointment;
using MyTherapy.Adapters;
using static Android.Widget.AdapterView;

namespace MyTherapy
{
	[Activity(Label = "InrActivity")]
	public class InrActivity : Activity
	{
		private EditText inrEditText;
		private MaterialButton saveButton;
		private MaterialButton inrDateButton;
		private DateTime date;
		private DoctorAppointmentDatabase db;
		private ListView appointmentsListView;
		private DoctorAppointmentsAdapter adapter;
		private AlertDialog.Builder ad;
		private EditText et;
		List<DoctorAppointment> allTherapies;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			SetContentView(Resource.Layout.inr_layout);
			base.OnCreate(savedInstanceState);
			db = DoctorAppointmentDatabase.Instance;

			inrEditText = FindViewById<EditText>(Resource.Id.editText1);
			inrDateButton = FindViewById<MaterialButton>(Resource.Id.materialButton1);
			saveButton = FindViewById<MaterialButton>(Resource.Id.materialButton2);
			appointmentsListView = FindViewById<ListView>(Resource.Id.listView1);
			appointmentsListView.ItemLongClick += AppointmentsListView_ItemLongClick;
			allTherapies = db.GetAppointments();
			adapter = new DoctorAppointmentsAdapter(this, allTherapies);
			appointmentsListView.Adapter = adapter;



			saveButton.Enabled = false;

			inrDateButton.Click += InrDateButton_Click;
			saveButton.Click += SaveButton_Click;

		}

		private void AppointmentsListView_ItemLongClick(object sender, ItemLongClickEventArgs e)
		{
			DoctorAppointment myitem = allTherapies[e.Position];

			LinearLayout l = new LinearLayout(this);

			et = new EditText(this)
			{
				Text = myitem.INR?.ToString()
			};



			l.AddView(et);

			ad = new AlertDialog.Builder(this);
			ad.SetTitle("Update INR value");
			ad.SetView(l); // <----
			ad.SetPositiveButton("Update", (senderAlert, args) =>
			{
				//CreateScheme();
				UpdateINR(e.Position, et.Text);
				//Finish();
			});

			ad.SetNegativeButton("Cancel", (senderAlert, args) =>
			{
				//Finish();
			});

			ad.Show();


		}

		private void UpdateINR(int position, string text)
		{
			
			allTherapies[position].INR = double.Parse(text);
			db.UpdateAppointment(allTherapies[position]);

			adapter.NotifyDataSetChanged();

		}

		private void SaveButton_Click(object sender, EventArgs e)
		{
			if (!inrEditText.Text.Any()) return;
			var inr = float.Parse(inrEditText.Text);
			var appointment = new DoctorAppointment()
			{
				INR = inr,
				Date = date
			};
			db.AddAppointment(appointment);
			allTherapies.Add(appointment);
			adapter.NotifyDataSetChanged();

		}
		
		private void InrDateButton_Click(object sender, EventArgs e)
		{
			DatePickerDialog datePickerDialog = new DatePickerDialog(this);
			datePickerDialog.DateSet += DatePickerDialog_DateSet;
			datePickerDialog.Show();

		}

		private void DatePickerDialog_DateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			date = e.Date;
			inrDateButton.Text = date.ToShortDateString();
			saveButton.Enabled = true;
		}

	}
}