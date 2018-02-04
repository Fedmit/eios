using eios.Model;
using eios.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace eios
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OccupationsPage : ContentPage
	{
		public OccupationsPage ()
		{
			InitializeComponent ();

            var occupationsViewModel = new OccupationsListViewModel();
			BindingContext = occupationsViewModel;

            listView.ItemTapped += async (sender, e) =>
            {
                listView.SelectedItem = null;

                if (e.Item is Occupation item)
                {
                    if (item.TargetType == null)
                    {
                        return;
                    }

                    await Navigation.PushAsync((Page)Activator.CreateInstance(item.TargetType, item));
                }
            };
        }
    }
}