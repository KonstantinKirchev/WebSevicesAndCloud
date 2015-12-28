namespace RestClient
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    class RestClient
    {
        static void Main()
        {
            var webrequest = (HttpWebRequest)WebRequest.Create(@"http://localhost:60670/api/Values?startX=4&endX=0&startY=7&endY=-10");
            webrequest.Method = "GET";
            webrequest.ContentType = "application/x-www-form-urlencoded";

            var webresponse = (HttpWebResponse)webrequest.GetResponse();
            var enc = Encoding.GetEncoding("utf-8");
            var responseStream = new StreamReader(webresponse.GetResponseStream(), enc);
            var response = string.Empty;
            response = responseStream.ReadToEnd();
            webresponse.Close();

            Console.WriteLine(response);

        }
    }
}
