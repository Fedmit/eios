using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios
{
	public partial class MainPage : MasterDetailPage
	{
		public MainPage()
		{
			InitializeComponent();

            masterPage.MenuTop.ItemSelected += OnItemSelected;
            masterPage.MenuBottom.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.MenuTop.SelectedItem = null;
                masterPage.MenuBottom.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
