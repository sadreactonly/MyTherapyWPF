using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using Common.Models;

namespace MyTherapy
{
	public class SchemaAdapter : BaseAdapter<DailyTherapy>
	{
		List<DailyTherapy> items;
		Activity context;
		public SchemaAdapter(Activity context, List<DailyTherapy> items)
			: base()
		{
			this.context = context;
			this.items = items;
		}

		public override long GetItemId(int position) => position;

		public override DailyTherapy this[int position] => items[position];

		public override int Count => items.Count; 

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			var view = convertView ?? context.LayoutInflater.Inflate(Resource.Layout.schema_list_item, null);
			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Date.ToShortDateString();
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Dose.ToString();
			view.FindViewById<TextView>(Resource.Id.Text3).Text = item.IsTaken ? "Taken." : "Not taken.";

			return view;
		}
		public void RemoveItemAt(int position) => items.RemoveAt(position);

		internal DailyTherapy GetFromItem(int position) => items[position];
	}
}