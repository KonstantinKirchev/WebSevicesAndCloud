using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Login
{
    class Program
    {
        private const string LoginEndPoint = "http://localhost:62859/Token";

        static void Main()
        {
            LoginUser(new[] { "kosta@yahoo.com", "K0sta!" });
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static async void LoginUser(string[] parameters)
        {
            string username = parameters[1];
            string password = parameters[2];
           

            var httpClient = new HttpClient();

            using (httpClient)
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Username", username), 
                    new KeyValuePair<string, string>("Password", password) 
                });

                var response = await httpClient.PostAsync(LoginEndPoint, content);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
