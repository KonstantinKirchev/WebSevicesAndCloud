using System;
using System.Collections.Generic;
using System.Net.Http;

namespace CreateGame
{
    class Program
    {
        private const string CreateGameEndPoint = "http://localhost:62859/api/games/create";

        static void Main()
        {
            CreateGame();
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static async void CreateGame()
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "{token}");
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>(), 
                });

                var response = await httpClient.PostAsync(CreateGameEndPoint, content);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
