using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Common.Models;

namespace MyTherapy
{
	[Activity(Label = "Create therapy schema", Theme = "@style/AppTheme")]

	public class SchemaActivity : Activity
	{
		private AppManager appManager;
		private List<float> dosage = new List<float>();
		private List<DateTime> dates = new List<DateTime>();
		private List<DailyTherapy> therapies = new List<DailyTherapy>();

		private Button addButton;
		private ListView listView;
		private Button startDateButton;
		private Button endDateButton;
		private Button saveSchemaButton;

		private DateTime startDate = DateTime.MinValue;
		private DateTime endDate = DateTime.MinValue;
		private Spinner spinner;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.therapy_schema_layout);

			addButton = FindViewById<Button>(Resource.Id.btnAdd);
			addButton = FindViewById<Button>(Resource.Id.btnAdd);
			listView = FindViewById<ListView>(Resource.Id.listView);
			startDateButton = FindViewById<Button>(Resource.Id.startDateButton);
			endDateButton = FindViewById<Button>(Resource.Id.EndDateButton);
			saveSchemaButton = FindViewById<Button>(Resource.Id.saveSchemaButton);

			startDateButton.Click += StartDateButton_Click;
			endDateButton.Click += EndDateButton_Click;
			addButton.Click += AddButton_Click;
			saveSchemaButton.Click += SaveSchemaButton_Click;

			addButton.Enabled = false;
			saveSchemaButton.Enabled = false;

			spinner = FindViewById<Spinner>(Resource.Id.spinner1);

			spinner.ItemSelected += Spinner_ItemSelected;
			var adapterSpinner = new ArrayAdapter<double>(this, Android.Resource.Layout.SimpleSpinnerItem,
				new double[9] {0, 0.25, 0.5, 0.75, 1, 1.25, 1.5, 1.75, 2});
			adapterSpinner.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
			spinner.Adapter = adapterSpinner;

			appManager = new AppManager(this);

		}
		private void Spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e) => addButton.Enabled = true;

		/// <summary>
		/// Create therapy schema based on dosage schema and dates.
		/// </summary>
		private void CreateScheme()
		{

			for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
				dates.Add(date);

			int x = dates.Count / dosage.Count + 1;

			var newList = new List<float>();

			for(int i = 0; i < x; i++)
			{
				newList.AddRange(dosage);
			}


			for (int i=0; i < dates.Count; i++)
			{
				var therapy = new DailyTherapy() {Guid = Guid.NewGuid(), Date = dates[i], Dose = newList[i] };
				therapies.Add(therapy);
			}

			appManager.AddTherapies(therapies);
		}

		#region Buttons
		private void SaveSchemaButton_Click(object sender, EventArgs e)
		{
			var alert = new Android.Support.V7.App.AlertDialog.Builder(this);

			alert.SetTitle("Save schema");
			alert.SetMessage("Do you want to save schema?");

			alert.SetPositiveButton("Yes", (senderAlert, args) =>
			{
				CreateScheme();
				Finish();
			});

			alert.SetNegativeButton("No", (senderAlert, args) =>
			{

			});

			Dialog dialog = alert.Create();
			dialog.Show();

		}

		private void EndDateButton_Click(object sender, EventArgs e)
		{
			DatePickerDialog dateDialog = new DatePickerDialog(this);
			dateDialog.DateSet += EndDateDialog_DateSet;
			dateDialog.Show();
		}

		private void StartDateButton_Click(object sender, EventArgs e)
		{
			DatePickerDialog dateDialog = new DatePickerDialog(this);
			dateDialog.DateSet += DateDialog_DateSet;
			dateDialog.Show();
		}

		private void AddButton_Click(object sender, EventArgs e)
		{
			float.TryParse(spinner.SelectedItem.ToString(), out var dose);

			if (dose >= 0)
			{
				dosage.Add(dose);
			}

			listView.Adapter = new ArrayAdapter<float>(this, Android.Resource.Layout.SimpleListItem1, dosage);
		}

		#endregion

		private void EndDateDialog_DateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			endDate = e.Date;
			endDateButton.Text = endDate.ToShortDateString();
			saveSchemaButton.Enabled = true;
		}

		private void DateDialog_DateSet(object sender, DatePickerDialog.DateSetEventArgs e)
		{
			startDate = e.Date;
			startDateButton.Text = startDate.ToShortDateString();
		}

	}
}