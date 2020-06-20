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
using MyTherapyWPF.Common;

namespace MyTherapy
{
	[Activity(Label = "SchemaActivity")]
	public class SchemaActivity : Activity
	{
		private List<float> dosage = new List<float>();
		private List<DateTime> dates = new List<DateTime>();
		private List<DailyTherapy> therapies = new List<DailyTherapy>();

		ITherapyDatabase db = TherapyDatabase.Instance;
		Button addButton;
		EditText editText;
		ListView listView;
		Button startDateButton;
		Button endDateButton;
		Button saveSchemaButton;

		DateTime startDate = DateTime.MinValue;
		DateTime endDate = DateTime.MinValue;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.therapy_schema_layout);

			addButton = FindViewById<Button>(Resource.Id.btnAdd);
			addButton = FindViewById<Button>(Resource.Id.btnAdd);
			editText = FindViewById<EditText>(Resource.Id.edtName);
			listView = FindViewById<ListView>(Resource.Id.listView);
			startDateButton = FindViewById<Button>(Resource.Id.startDateButton);
			endDateButton = FindViewById<Button>(Resource.Id.EndDateButton);
			saveSchemaButton = FindViewById<Button>(Resource.Id.saveSchemaButton);

			startDateButton.Click += StartDateButton_Click;
			endDateButton.Click += EndDateButton_Click;
			addButton.Click += AddButton_Click;
			saveSchemaButton.Click += SaveSchemaButton_Click;
			editText.TextChanged += EditText_TextChanged;

			addButton.Enabled = false;
			saveSchemaButton.Enabled = false;

		}

		private void EditText_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
		{
			if(e.Text.Count()>0)
			{
				addButton.Enabled = true;
			}
			else
			{
				addButton.Enabled = false;
			}
		}


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
				var therapy = new DailyTherapy() { Date = dates[i], Dose = newList[i] };
				therapies.Add(therapy);
			}

			db.AddTherapySchema(therapies);
		}

		#region Buttons
		private void SaveSchemaButton_Click(object sender, EventArgs e)
		{
			CreateScheme();
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
			float dose = -1;
			float.TryParse(editText.Text, out dose);

			if (dose != -1)
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