using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeworkHTTP
{
    class Program
    {
        static string URL_BASE = @"http://localhost:5000/somedata";
        static string HEADER_CONTENT_TYPE_VALUE = "application/json";
        static string HEADER_APP_ID = "appId";
        static string HEADER_APP_ID_VALUE = "campus-task";

        static string RequestGET(string url, string flags)
        {
            HttpWebRequest requestGET = (HttpWebRequest)WebRequest.Create(url);
            requestGET.Method = "GET";
            requestGET.ContentType = HEADER_CONTENT_TYPE_VALUE;
            requestGET.Headers.Add(HEADER_APP_ID, HEADER_APP_ID_VALUE);

            using (HttpWebResponse response = (HttpWebResponse)requestGET.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return response.StatusCode.ToString() + " " + reader.ReadToEnd();
            }
        }

        static string RequestPOST_INVALID(string url, string data)
        {
            // Does not work after one call for old Server version;
            // Does not work at all for new Server version;

            byte[] buffer = Encoding.ASCII.GetBytes(data);

            HttpWebRequest requestPOST = (HttpWebRequest)WebRequest.Create(url);
            requestPOST.Method = "POST";
            requestPOST.ContentType = HEADER_CONTENT_TYPE_VALUE;
            requestPOST.Headers.Add(HEADER_APP_ID, HEADER_APP_ID_VALUE);
            requestPOST.ContentLength = buffer.Length;

            using (Stream requestBody = requestPOST.GetRequestStream())
            {
                requestBody.Write(buffer, 0, buffer.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)requestPOST.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return response.StatusCode.ToString() + " " + reader.ReadToEnd();
            }
        }

        static string RequestPOST(string url, string data)
        {

            var netData = new StringContent(data, Encoding.UTF8, HEADER_CONTENT_TYPE_VALUE);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add(HEADER_APP_ID, HEADER_APP_ID_VALUE);

            var response =  client.PostAsync(url, netData);
            return response.Result.StatusCode + " | " + response.Result.Content.ReadAsStringAsync().Result;
        }

        static string RequestPUT_INVALID(string url, string id, string data)
        {
            // Does not work for old version of Server;
            // Not tested for new version of Server;

            byte[] buffer = Encoding.ASCII.GetBytes(data);

            HttpWebRequest requestPUT = (HttpWebRequest)WebRequest.Create(url + "/" + id);
            requestPUT.Method = "PUT";
            requestPUT.ContentType = HEADER_CONTENT_TYPE_VALUE;
            requestPUT.Headers.Add(HEADER_APP_ID, HEADER_APP_ID_VALUE);
            requestPUT.ContentLength = buffer.Length;

            using (Stream requestBody = requestPUT.GetRequestStream())
            {
                requestBody.Write(buffer, 0, buffer.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)requestPUT.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return response.StatusCode.ToString() + " " + reader.ReadToEnd();
            }
        }

        static string RequestPUT(string url,string data)
        {
            var netData = new StringContent(data, Encoding.UTF8, HEADER_CONTENT_TYPE_VALUE);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add(HEADER_APP_ID, HEADER_APP_ID_VALUE);

            var response = client.PutAsync(url, netData);
            return response.Result.StatusCode + " | " + response.Result.Content.ReadAsStringAsync().Result;
        }

        static string RequestDELETE(string url)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add(HEADER_APP_ID, HEADER_APP_ID_VALUE);

            var response = client.DeleteAsync(url);
            return response.Result.StatusCode + " | " + response.Result.Content.ReadAsStringAsync().Result;
        }

        static void ProcessRequestGET()
        {
            Console.WriteLine("\nInput: GET");
            string RequestOutputGET = RequestGET(URL_BASE, string.Empty);
            Console.WriteLine("Output: GET " + RequestOutputGET);
        }

        static void ProcessRequestPOST(string id, int weight)
        {
            string data = @"{ ""dataId"":""" + id + @""", ""weight"":" + weight.ToString() + "}";
            Console.WriteLine("\nInput: POST " + data);
            string RequestOutputPOST = RequestPOST(URL_BASE, data);
            Console.WriteLine("Output: POST " + RequestOutputPOST);
        }

        static void ProcessRequestPUT(string id, int weight)
        {
            string data = @"{ ""dataId"":""" + id + @""", ""weight"":" + weight.ToString() + "}";
            Console.WriteLine("\nInput: PUT " + data);
            string RequestOutputPUT = RequestPUT(URL_BASE + "/" + id, data);
            Console.WriteLine("Output: PUT " + RequestOutputPUT);
        }

        static void ProcessRequestDELETE(string id)
        {
            Console.WriteLine("\nInput: DELETE " + id);
            string RequestOutputDELETE = RequestDELETE(URL_BASE + "/" + id);
            Console.WriteLine("Output: DELETE " + RequestOutputDELETE);
        }

        static void Main(string[] args)
        {
            ProcessRequestPOST("food", 300);
            ProcessRequestPOST("wood", 500);
            ProcessRequestPOST("weed", 100);
            ProcessRequestPOST("feed", 700);

            ProcessRequestGET();

            ProcessRequestPUT("food", 400);
            ProcessRequestPUT("wood", 100);
            ProcessRequestPUT("feed", 900);

            ProcessRequestGET();

            ProcessRequestDELETE("wood");
            ProcessRequestDELETE("feed");

            ProcessRequestGET();
        }
    }
}
