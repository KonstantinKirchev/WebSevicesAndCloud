using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Register
{
    class Program
    {
        private const string RegisterEndPoint = "http://localhost:62859/api/account/register";
        
        static void Main()
        {
            RegisterUser(new []{"kosta@yahoo.com","K0sta!","K0sta!"});
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static async void RegisterUser(string[] parameters)
        {
            string email = parameters[1];
            string password = parameters[2];
            string confirmPassword = parameters[3];
            
            var httpClient = new HttpClient();
            
            using (httpClient)
            {
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("Email", email), 
                    new KeyValuePair<string, string>("Password", password), 
                    new KeyValuePair<string, string>("ConfirmPassword", confirmPassword), 
                });

                var response = await httpClient.PostAsync(RegisterEndPoint, content);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
