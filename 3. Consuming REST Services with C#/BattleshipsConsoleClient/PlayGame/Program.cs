using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PlayGame
{
    class Program
    {
        private const string PlayGameEndPoint = "http://localhost:62859/api/games/play";

        static void Main()
        {
            PlayGame(new[] { "D14B4F1C-2DA0-44FA-BF22-F5A71AF3EE3B", "5", "4" });
            Console.WriteLine("Done.");
            Console.ReadLine();
        }

        private static async void PlayGame(string[] parameters)
        {
            string gameId = parameters[1];
            string positionX = parameters[2];
            string positionY = parameters[3];

            var httpClient = new HttpClient();

            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "{token}");
                
                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("GameId", gameId), 
                    new KeyValuePair<string, string>("PositionX", positionX), 
                    new KeyValuePair<string, string>("PositionY", positionY), 
                });

                var response = await httpClient.PostAsync(PlayGameEndPoint, content);
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }
    }
}
