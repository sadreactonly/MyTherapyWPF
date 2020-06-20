using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Widget;
using MyTherapyWPF.Common;

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
		public override long GetItemId(int position)
		{
			return position;
		}
		public override DailyTherapy this[int position]
		{
			get { return items[position]; }
		}
		public override int Count
		{
			get { return items.Count; }
		}
		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			var item = items[position];
			View view = convertView;
			if (view == null) // no view to re-use, create new
				view = context.LayoutInflater.Inflate(Resource.Layout.schema_list_item, null);
			view.FindViewById<TextView>(Resource.Id.Text1).Text = item.Date.ToShortDateString();
			view.FindViewById<TextView>(Resource.Id.Text2).Text = item.Dose.ToString();
			return view;
		}
	}
}