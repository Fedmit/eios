using eios.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
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

        static string _baseUrl { get { return "http://lk.pnzgu.ru/ajax/mobile"; } }

        public async Task<List<Occupation>> GetOccupationsAsync(int idGroup)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_info";
            dynamicJson.id_group = idGroup;
            dynamicJson.date = App.DateNow.ToString("yyyy-MM-dd HH:mm:ss");

            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<Occupation> occupations = null;
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
                occupations = JsonConvert.DeserializeObject<List<Occupation>>(content);

                foreach (var occupation in occupations)
                {
                    occupation.IdGroup = idGroup;
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetOccupationsAsync(): " + ex.Message);
            }

            return occupations;
        }

        public async Task<List<Mark>> GetMarksAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_mark";
            dynamicJson.date = App.DateNow.ToString("yyyy-MM-dd HH:mm:ss");
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

        public async Task<GroupResponse> GetGroupsAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_group";
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            GroupResponse groupResponse = null;
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

                groupResponse = JsonConvert.DeserializeObject<GroupResponse>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetGroupsAsync(): " + ex.Message);
            }

            return groupResponse;
        }

        public async Task<List<Student>> GetStudentsAsync(int idGroup)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_students";
            dynamicJson.id_group = idGroup;
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

                foreach (var student in students)
                {
                    student.id_group = idGroup;
                }
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
                string str = (string)JObject.Parse(content).SelectToken("date");

                time = DateTime.Parse(str);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetDateAsync(): " + ex.Message);
            }

            return time;
        }

        public async Task<List<StudentAbsent>> GetAttendanceAsync(int idOccupation, int idGroup)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_attend_info";
            dynamicJson.id_timetable = idOccupation;
            dynamicJson.id_group = App.Current.Properties["IdGroupCurrent"];
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<StudentAbsent> attendance = null;
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

                attendance = JsonConvert.DeserializeObject<List<StudentAbsent>>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetAttendanceAsync(): " + ex.Message);
            }

            return attendance;
        }

        public async Task<bool> SetAttendAsync(int idTimeTable, List<StudentAbsent> list)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
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
