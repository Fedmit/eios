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

            LoadData();
		}

        private async void LoadData()
        {
            string url = "http://q9875032.beget.tech/hp_api/api.php";

            var login = "test";
            var password = "test1";
            var type = "get_info";
            var id = "1";
            StringContent stringContent = new StringContent(
                "{ \"login\": \""+login+"\"," +
                "  \"password\": \""+password+"\"," +
                "  \"type\": \"" + type + "\"," +
                "  \"id_group\": \"" + id + "\" }",
                UnicodeEncoding.UTF8,
                "application/json");

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(url, stringContent);
            response.EnsureSuccessStatusCode();

            // Десериализация ответа
            var content = await response.Content.ReadAsStringAsync();

            var ocupations = JsonConvert.DeserializeObject<List<Occupation>>(content);

            listView.ItemsSource = ocupations;
        }
    }
}