using System;
using System.Collections.Generic;
using System.Net.Http;

namespace JoinGame
{
    class Program
    {
        private const string JoinGameEndPoint = "http://localhost:62859/api/games/join";

        static void Main()
        {
            JoinGame("D14B4F1C-2DA0-44FA-BF22-F5A71AF3EE3B");
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static async void JoinGame(string gameId)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {

                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "{token}");
                
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("GameId", gameId), 
                });

                var response = await httpClient.PostAsync(JoinGameEndPoint, content);
                
                Console.WriteLine(response.StatusCode);
                
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
