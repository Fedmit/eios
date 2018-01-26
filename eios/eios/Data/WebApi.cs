using eios.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eios.Data
{
    class WebApi
    {
        public static WebApi Instance { get; } = new WebApi();

        static string _baseUrl { get { return "http://q9875032.beget.tech/hp_api/api.php"; } }

        public async Task<List<Occupation>> GetOccupationsAsync()
        {
            // Это нужно изменить
            // --------------------
            var login = "test";
            var password = "test1";
            var type = "get_info";
            var id = "1";
            // --------------------

            StringContent stringContent = new StringContent(
                "{ \"login\": \"" + login + "\"," +
                "  \"password\": \"" + password + "\"," +
                "  \"type\": \"" + type + "\"," +
                "  \"id_group\": \"" + id + "\" }",
                UnicodeEncoding.UTF8,
                "application/json");

            List<Occupation> ocupations = null;
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(_baseUrl, stringContent);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                ocupations = JsonConvert.DeserializeObject<List<Occupation>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Whooops! " + ex.Message);
            }

            return ocupations;
        }
    }
}
