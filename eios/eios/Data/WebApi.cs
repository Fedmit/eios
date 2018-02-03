using eios.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public async Task<List<Occupation>> GetOccupationsAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_info";
            dynamicJson.id_group = App.Current.Properties["IdGroupCurrent"];
            dynamicJson.date = App.Date.ToString("yyyy-MM-dd");

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

                //ocupations.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetOccupationsAsync(): " + ex.Message);
            }

            return ocupations;
        }

        public async Task<List<Mark>> GetMarksAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_mark";
            dynamicJson.date = App.Date.ToString("yyyy-MM-dd HH:mm:ss");
            dynamicJson.id_group = App.Current.Properties["IdGroupCurrent"];

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
                Console.WriteLine("GetMarksAsync(): " + ex.Message);
            }

            return marks;
        }

        public async Task<List<Group>> GetGroupsAsync(string login, string password)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = login;
            dynamicJson.password = password;
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
                Console.WriteLine("GetGroupsAsync(): " + ex.Message);
            }

            return groups;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_students";
            dynamicJson.id_group = App.Current.Properties["IdGroupCurrent"];
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
                Console.WriteLine("GetStudentsAsync(): " + ex.Message);
            }

            return students;
        }

        public async Task<DateTime> GetDateAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_date";
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            DateTime time = new DateTime();
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
                JArray arr = JArray.Parse(content);
                string str = (string)arr[0].SelectToken("date");

                time = DateTime.Parse(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDateAsync(): " + ex.Message);
            }

            return time;
        }

        public async Task<Attendance> GetAttendanceAsync(int idOccupation)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_attend_info";
            dynamicJson.id_timetable = idOccupation;
            dynamicJson.id_group = App.Current.Properties["IdGroupCurrent"];
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            Attendance attendance = null;
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
                JArray arr = JArray.Parse(content);

                attendance = JsonConvert.DeserializeObject<Attendance>(arr[0].ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAttendanceAsync(): " + ex.Message);
            }

            return attendance;
        }

        public async Task<bool> SetAttendAsync(int idTimeTable, List<SelectedStudent> list)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Current.Properties["Login"];
            dynamicJson.password = App.Current.Properties["Password"];
            dynamicJson.type = "set_attend";
            dynamicJson.id_group = App.Current.Properties["IdGroupCurrent"];
            dynamicJson.id_timetable = idTimeTable;
            dynamicJson.data = list;
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            try {
                Console.WriteLine(json);
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

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SetAttendAsync(): " + ex.Message);
            }

            return false;
        }
    }
}
