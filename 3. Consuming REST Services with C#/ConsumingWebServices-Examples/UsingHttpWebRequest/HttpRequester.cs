using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UsingHttpWebRequest
{
    class HttpRequester
    {
        public static T Get<T>(string resourceUrl)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "GET";
            
            var response = request.GetResponse(); // връща ни стрим!!!
            string responseString;
            using (StreamReader reader = new StreamReader(response.GetResponseStream())) // върнатият стрим го четем
            {
                responseString = reader.ReadToEnd();
            }
            var responseData = JsonConvert.DeserializeObject<T>(responseString); // С Json.net парсваме прочетеният стринг към нашите данни.
            return responseData; // и ги връщаме
        }

        public static void Get(string resourceUrl)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "GET";
            request.GetResponse();
        }

        public static void Post(string resourceUrl, object data)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";

            var jsonData = JsonConvert.SerializeObject(data);

            using (StreamWriter writer = 
                new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonData);
            }

            request.GetResponse();
        }

        public static T Post<T>(string resourceUrl, object data)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "POST";

            var jsonData = JsonConvert.SerializeObject(data); // от обект към  стрингифайнат JSON

            using (StreamWriter writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write(jsonData);
            }

            var response = request.GetResponse();
            string responseString;
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                responseString = reader.ReadToEnd();
            }
            var responseData = JsonConvert.DeserializeObject<T>(responseString); // от JSON към стринг
            return responseData;
        }

        public static void Delete(string resourceUrl)
        {
            var request = WebRequest.Create(resourceUrl) as HttpWebRequest;
            request.ContentType = "application/json";
            request.Method = "DELETE";
            request.GetResponse();
        }
    }
}
