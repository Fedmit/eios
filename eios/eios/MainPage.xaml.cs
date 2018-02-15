using eios.Model;
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
                    App.DateNow = DateTime.MinValue;
                    App.DateSelected = DateTime.MinValue;
                    App.IdGroupCurrent = 0;
                    App.IsUserLoggedIn = false;
                    App.Current.Properties["Login"] = null;
                    App.Current.Properties["Password"] = null;
                    await App.Current.SavePropertiesAsync();

                    await App.Database.DropTable<Group>();
                    await App.Database.DropTable<Occupation>();
                    await App.Database.DropTable<Student>();
                    await App.Database.DropTable<StudentAbsent>();

                    Application.Current.MainPage = new NavigationPage(new LoginPage());
                    return;
                }

                // Открываем новый экран, если мы не выбрали текущий пункт меню
                var current = (NavigationPage) Detail;
                if (item.TargetType != current.CurrentPage.GetType())
                {
                    Detail = new NavigationPage((Page) Activator.CreateInstance(item.TargetType));
                }

                masterPage.MenuTop.SelectedItem = null;
                masterPage.MenuBottom.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}
