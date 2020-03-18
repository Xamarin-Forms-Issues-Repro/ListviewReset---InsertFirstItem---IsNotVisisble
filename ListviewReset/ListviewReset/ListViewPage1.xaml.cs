using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ListviewReset
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ListViewPage1 : ContentPage
	{
		public ListViewPage1()
		{
			InitializeComponent();
			BindingContext = new ViewModel();
		}

		async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.Item == null)
				return;

			await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

			//Deselect Item
			((ListView)sender).SelectedItem = null;
		}
	}

	public class ViewModel
	{
		public ReactiveList<string> ListViewItems { get; set; } = new ReactiveList<string>();
		public ReactiveList<string> CollectionViewItems { get; set; } = new ReactiveList<string>();

		public ViewModel()
		{
			
			ListViewAddRangeCommand = new Command(_ => AddRange(ListViewItems));
			ListViewInsertFirstCommand = new Command(_ => InsertFirst(ListViewItems));

			CollectionViewAddRangeCommand = new Command(_ => AddRange(CollectionViewItems));
			CollectionViewInsertFirstCommand = new Command(_ => InsertFirst(CollectionViewItems));
		}

		private void AddRange(ReactiveList<string> source)
		{
			using var s = source.SuppressChangeNotifications();
			var last = source.LastOrDefault() != null ? int.Parse(source.Last()) : 0;
			var items = Enumerable.Range(last, 10).Select(i => i.ToString());
			source.AddRange(items);
		}

		private void InsertFirst(ReactiveList<string> source)
		{
			using var s = source.SuppressChangeNotifications();
			var rnd = new Random();
			var item = rnd.Next(int.MaxValue - 1);
			var allItems = new List<string>(source?.AsEnumerable() ?? new List<string>() {"default"});

			source.Clear();
			allItems.Insert(0, item.ToString());
			source.AddRange(allItems);
		}

		public ICommand ListViewAddRangeCommand { get; }
		public ICommand ListViewInsertFirstCommand { get; }
		public ICommand CollectionViewAddRangeCommand { get; }
		public ICommand CollectionViewInsertFirstCommand { get; }
	}
}
