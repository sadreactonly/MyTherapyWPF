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

namespace MyTherapy.Adapters
{
	public class DoctorAppointmentsAdapter : BaseAdapter<DoctorAppointment>
	{
		List<DoctorAppointment> items;
		Activity context;
		public DoctorAppointmentsAdapter(Activity context, List<DoctorAppointment> items)
			: base()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position) => position;

		public override DoctorAppointment this[int position] => items[position];

		public override int Count => items.Count;

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.doctor_appointments_list, null);
			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Date.ToShortDateString();
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.INR.ToString();

			return view;
		}
		public void RemoveItemAt(int position) => items.RemoveAt(position);

		internal DoctorAppointment GetFromItem(int position) => items[position];
	}
}