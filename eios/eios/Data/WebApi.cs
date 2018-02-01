using eios.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eios.Data
{
    class WebApi
    {
        public static WebApi Instance { get; } = new WebApi();

        static string _baseUrl { get { return "http://q9875032.beget.tech/hp_api/api.php"; } }

        public async Task<List<Occupation>> GetOccupationsAsync(int id_group)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Current.Properties["Login"];
            dynamicJson.password = App.Current.Properties["Password"];
            dynamicJson.type = "get_info";
            dynamicJson.id_group = id_group;
            dynamicJson.date = "2018-02-01 13:46:30";
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<Occupation> ocupations = null;
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(
                    _baseUrl, 
                    new StringContent(
                        json, 
                        UnicodeEncoding.UTF8, 
                        "application/json"
                    )
                );
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

        public async Task<List<Mark>> GetMarksAsync(int id_group)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Current.Properties["Login"];
            dynamicJson.password = App.Current.Properties["Password"];
            dynamicJson.type = "get_mark";
            dynamicJson.id_group = id_group;
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<Mark> marks = null;
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(
                    _baseUrl,
                    new StringContent(
                        json,
                        UnicodeEncoding.UTF8,
                        "application/json"
                    )
                );
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                marks = JsonConvert.DeserializeObject<List<Mark>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Whooops! " + ex.Message);
            }

            return marks;
        }

        public async Task<List<Group>> GetGroupsAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Current.Properties["Login"];
            dynamicJson.password = App.Current.Properties["Password"];
            dynamicJson.type = "get_group";
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<Group> groups = null;
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(
                    _baseUrl,
                    new StringContent(
                        json,
                        UnicodeEncoding.UTF8,
                        "application/json"
                    )
                );
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                groups = JsonConvert.DeserializeObject<List<Group>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Whooops! " + ex.Message);
            }

            return groups;
        }

        public async Task<List<Student>> GetStudentsAsync(int id_group)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Current.Properties["Login"];
            dynamicJson.password = App.Current.Properties["Password"];
            dynamicJson.type = "get_students";
            dynamicJson.id_group = id_group;
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<Student> students = null;
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.PostAsync(
                    _baseUrl,
                    new StringContent(
                        json,
                        UnicodeEncoding.UTF8,
                        "application/json"
                    )
                );
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                students = JsonConvert.DeserializeObject<List<Student>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Whooops! " + ex.Message);
            }

            return students;
        }

        public async Task SetAttendAsync()
        {
            throw new NotImplementedException();
        }
    }
}
