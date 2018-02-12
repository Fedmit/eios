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

        async void OnItemSelected(Object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is MasterPageItem item)
            {
                // При нажатии на выход открываем экран с входом
                if (item.TargetType == typeof(LoginPage))
                {
                    App.Current.Properties["IdGroupCurrent"] = null;
                    App.Current.Properties["Fullname"] = null;
                    App.Current.Properties["IsLoggedIn"] = false;
                    App.Current.Properties["Login"] = null;
                    App.Current.Properties["Password"] = null;
                    App.Current.Properties["DateNow"] = "1970-01-01 01:02:03";
                    await App.Current.SavePropertiesAsync();

                    await App.Database.DeleteThisShits();
                    await App.Database.DeleteGroupsTable();

                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    return;
                }

                // Открываем новый экран, если мы не выбрали текущий пункт меню
                var current = (NavigationPage)Detail;
                if (item.TargetType != current.CurrentPage.GetType())
                {
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                }

                masterPage.MenuTop.SelectedItem = null;
                masterPage.MenuBottom.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
