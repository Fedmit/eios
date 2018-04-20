using eios.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace eios.Data
{
    class WebApi
    {
        public static WebApi Instance { get; } = new WebApi();

        static string _baseUrl { get { return "https://lk.pnzgu.ru/ajax/mobile"; } }

        static async Task<string> GetResponseAsync(string json)
        {
            HttpWebRequest request;
            HttpWebResponse response;

            ServicePointManager.ServerCertificateValidationCallback = ValidateServerCertificate;

            //request = (HttpWebRequest) WebRequest.Create("https://lk.pnzgu.ru");
            //response = (HttpWebResponse) request.GetResponse();

            ////retrieve the ssl cert and assign it to an X509Certificate object
            //X509Certificate cert = request.ServicePoint.Certificate;

            ////convert the X509Certificate to an X509Certificate2 object by passing it into the constructor
            //X509Certificate2 cert2 = new X509Certificate2(cert);

            request = (HttpWebRequest) System.Net.WebRequest.Create(_baseUrl);

            request.Method = "POST";
            request.ContentType = "application/json";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36 OPR/52.0.2871.64";
            //request.ClientCertificates.Add(cert2);

            StreamWriter writer = new StreamWriter(request.GetRequestStream());
            writer.WriteLine(json);
            writer.Close();

            if (request != null) request.GetRequestStream().Close();

            response = (HttpWebResponse) await request.GetResponseAsync();
            System.IO.StreamReader reader =
            new System.IO.StreamReader(response.GetResponseStream());
            String data = reader.ReadToEnd();

            if (response != null) response.GetResponseStream().Close();

            return data;
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        //internal static byte[] ReadFile(string fileName)
        //{
        //    FileStream f = new FileStream(fileName, FileMode.Open, FileAccess.Read);
        //    int size = (int) f.Length;
        //    byte[] data = new byte[size];
        //    size = f.Read(data, 0, size);
        //    f.Close();
        //    return data;
        //}

        public async Task<List<Occupation>> GetOccupationsAsync(int idGroup)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_info";
            dynamicJson.id_group = idGroup;
            dynamicJson.date = App.DateSelected.ToString("yyyy-MM-dd");

            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<Occupation> occupations = null;
            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    occupations = JsonConvert.DeserializeObject<List<Occupation>>(content);

                    foreach (var occupation in occupations)
                    {
                        occupation.IdGroup = idGroup;
                    }

                    occupations = occupations.OrderBy(occup => occup.IdOccupation).ToList();

                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("GetOccupationsAsync: " + ex.Message);
                }
                catch (WebException ex)
                {
                    isResponse = true;
                    throw ex;
                }
                catch (Exception ex)
                {
                    isResponse = true;
                    Console.WriteLine("GetOccupationsAsync(): " + ex.Message);
                }
            }

            return occupations;
        }

        public async Task<MarksResponse> GetMarksAsync()
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_mark";
            dynamicJson.date = App.DateSelected.ToString("yyyy-MM-dd");
            dynamicJson.id_group = App.IdGroupCurrent;

            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            MarksResponse marksResponse = null;
            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    marksResponse = JsonConvert.DeserializeObject<MarksResponse>(content);
                    marksResponse.Data = marksResponse.Data.OrderBy(occup => occup.IdOccupation).ToList();
                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("GetMarksAsync: " + ex.Message);
                }
                catch (Exception ex)
                {
                    isResponse = true;
                    Console.WriteLine("GetMarksAsync(): " + ex.Message);
                }
            }

            return marksResponse;
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
            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    groupResponse = JsonConvert.DeserializeObject<GroupResponse>(content);
                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("GetGroupsAsync: " + ex.Message);
                }
                catch (WebException ex)
                {
                    isResponse = true;
                    throw ex;
                }
                catch (Exception ex)
                {
                    isResponse = true;
                    Console.WriteLine("GetGroupsAsync(): " + ex.Message);
                }
            }

            return groupResponse;
        }

        //public bool MyRemoteCertificateValidationCallback(System.Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    bool isOk = true;
        //    // If there are errors in the certificate chain, look at each error to determine the cause. 
        //    if (sslPolicyErrors != SslPolicyErrors.None)
        //    {
        //        for (int i = 0; i < chain.ChainStatus.Length; i++)
        //        {
        //            if (chain.ChainStatus[i].Status != X509ChainStatusFlags.RevocationStatusUnknown)
        //            {
        //                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
        //                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
        //                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
        //                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
        //                bool chainIsValid = chain.Build((X509Certificate2) certificate);
        //                if (!chainIsValid)
        //                {
        //                    isOk = false;
        //                }
        //            }
        //        }
        //    }
        //    return isOk;
        //}

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
            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    students = JsonConvert.DeserializeObject<List<Student>>(content);

                    foreach (var student in students)
                    {
                        student.id_group = idGroup;
                    }

                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("GetStudentsAsync: " + ex.Message);
                }
                catch (WebException ex)
                {
                    isResponse = true;
                    throw ex;
                }
                catch (Exception ex)
                {
                    isResponse = true;
                    Console.WriteLine("GetStudentsAsync(): " + ex.Message);
                }
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
            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    string str = (string) JObject.Parse(content).SelectToken("date");

                    time = DateTime.Parse(str);
                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("GetDateAsync: " + ex.Message);
                }
                catch (WebException ex)
                {
                    isResponse = true;
                    throw ex;
                }
                catch (Exception ex)
                {
                    isResponse = true;
                    Console.WriteLine("GetDateAsync(): " + ex.Message);
                }
            }

            return time;
        }

        public async Task<List<StudentAbsent>> GetAttendanceAsync(int idOccupation, int idGroup)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "get_attend_info";
            dynamicJson.id_occup = idOccupation;
            dynamicJson.id_group = App.IdGroupCurrent;
            dynamicJson.date = App.DateSelected.ToString("yyyy-MM-dd");
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            List<StudentAbsent> attendance = null;
            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    attendance = JsonConvert.DeserializeObject<List<StudentAbsent>>(content);
                    foreach (var student in attendance)
                    {
                        student.IdOccupation = idOccupation;
                        student.IdGroup = idGroup;
                    }

                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("GetAttendanceAsync: " + ex.Message);
                    throw ex;
                }
                catch (WebException)
                {
                    isResponse = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("GetAttendanceAsync(): " + ex.Message);
                    isResponse = true;
                }
            }

            return attendance;
        }

        public async Task SetAttendAsync(List<StudentAbsent> list, Occupation occupation)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "set_attend";
            dynamicJson.id_group = occupation.IdGroup;
            dynamicJson.date = App.DateSelected.ToString("yyyy-MM-dd");
            dynamicJson.id_occup = occupation.IdOccupation;
            dynamicJson.id_lesson = occupation.IdLesson;
            dynamicJson.id_aud = occupation.IdAud;
            dynamicJson.data = list;
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("SetAttendAsync: " + ex.Message);
                }
                catch (Exception ex)
                {
                    isResponse = true;
                    Console.WriteLine("SetAttendAsync(): " + ex.Message);
                }
            }
        }

        public async Task SetNullAttendAsync(Occupation occupation)
        {
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.login = App.Login;
            dynamicJson.password = App.Password;
            dynamicJson.type = "set_attend";
            dynamicJson.id_group = App.IdGroupCurrent;
            dynamicJson.date = App.DateSelected.ToString("yyyy-MM-dd");
            dynamicJson.id_occup = occupation.IdOccupation;
            dynamicJson.id_lesson = occupation.IdLesson;
            dynamicJson.id_aud = occupation.IdAud;
            dynamicJson.data = "Set_canceled";
            string json = "";
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);

            bool isResponse = false;
            while (!isResponse)
            {
                try
                {
                    var content = await GetResponseAsync(json);

                    isResponse = true;
                }
                catch (TaskCanceledException ex)
                {
                    Debug.WriteLine("SetNullAttendAsync: " + ex.Message);
                }
                catch (WebException ex)
                {
                    isResponse = true;
                    throw ex;
                }
                catch (Exception ex)
                {
                    isResponse = true;
                    Console.WriteLine("SetNullAttendAsync(): " + ex.Message);
                }
            }
        }
    }
}
