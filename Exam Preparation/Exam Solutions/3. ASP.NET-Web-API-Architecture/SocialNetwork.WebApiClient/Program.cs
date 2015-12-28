using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetwork.WebApiClient
{
    class Program
    {
        private const string GetAllPostsEndpoint = "http://localhost:54928/api/posts";
        private const string AddNewPostEndpoint = "http://localhost:54928/api/posts";
        private const string UserSearchEndpoint = "http://localhost:54928/api/users/search";

        static void Main()
        {
            
            GetAllPostsAsync();
            Console.WriteLine("Done.");
            Console.ReadLine();
            //PrintSearchUser();
           // AddNewPost();
            return;
            try
            {
                PrintAllPosts();
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Any(e => e is TaskCanceledException))
                {
                    Console.WriteLine("Connection to {0} timed out.", GetAllPostsEndpoint);
                }
                else
                {
                    throw ex;
                }
            }
            
        }

        private async static void GetAllPostsAsync()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(GetAllPostsEndpoint);
            var posts = await response.Content.ReadAsAsync<IEnumerable<PostDTO>>();
            foreach (var post in posts)
            {
                Console.WriteLine(post.Content);
            }
        }

        private static void PrintSearchUser()
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "xIw287UhzxPILpkzt-DswL394w3VKjh3OsWGeqNveHtZP0kwgY2gtd0IjvxL1Smn7HUKtlArLABDB3BBuCtbhrGFRFqNFHD9gH0HCBqQSfreZxzesnS177zs4p52I7mXMuqltNsfzh8rpspq5VK9gJ3FlBdgDd8-4zQ5LudlycImc6Z5ILWROY2O6EreckL-VVDRMxkhGwFFpYL3x-E5ibz4qzA36gyzU2ik0QKLfHTJ7l1hQZrOvs1oGU_3_miO2DJMGa4DcUqXSRM15A1FlWb3y8jaw03Khvd8-eAiKMOW65Wt-gfJSdlxtZ7t-Ewb184RKsW_G9hm_Ib1zkkSSCqRmqiF8hE_IKbpA0CdTFRJary3ZZU3x2BGBzW6mF2ACAPp75upOZBpUN0aWI269AR-3PmUpV2ztMzZEEO4NseTkXdK4aUr4LhqgHZ2NrHBxZDJ3fu8qMzbUakqVJqeAqRxKmsf_86GIJNY1Ur_dJRqDE26UIEoh8oOA6BdSoli3GzFo3XgikBdukmxaINEGg");

                var builder = new UriBuilder(UserSearchEndpoint);
                
                var query = HttpUtility.ParseQueryString(String.Empty);
                query["name"] = "kost";
                query["minAge"] = "14";
                query["maxAge"] = "34";
                
                builder.Query = query.ToString();
                
                Console.WriteLine(builder);

                var result = httpClient.GetAsync(builder.ToString()).Result;
                
                Console.WriteLine(result.Content.ReadAsStringAsync().Result);
            }
        }

        private static void AddNewPost()
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "xIw287UhzxPILpkzt-DswL394w3VKjh3OsWGeqNveHtZP0kwgY2gtd0IjvxL1Smn7HUKtlArLABDB3BBuCtbhrGFRFqNFHD9gH0HCBqQSfreZxzesnS177zs4p52I7mXMuqltNsfzh8rpspq5VK9gJ3FlBdgDd8-4zQ5LudlycImc6Z5ILWROY2O6EreckL-VVDRMxkhGwFFpYL3x-E5ibz4qzA36gyzU2ik0QKLfHTJ7l1hQZrOvs1oGU_3_miO2DJMGa4DcUqXSRM15A1FlWb3y8jaw03Khvd8-eAiKMOW65Wt-gfJSdlxtZ7t-Ewb184RKsW_G9hm_Ib1zkkSSCqRmqiF8hE_IKbpA0CdTFRJary3ZZU3x2BGBzW6mF2ACAPp75upOZBpUN0aWI269AR-3PmUpV2ztMzZEEO4NseTkXdK4aUr4LhqgHZ2NrHBxZDJ3fu8qMzbUakqVJqeAqRxKmsf_86GIJNY1Ur_dJRqDE26UIEoh8oOA6BdSoli3GzFo3XgikBdukmxaINEGg");

                var bodyData = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("content", "brand new post"), 
                    new KeyValuePair<string, string>("wallOwnerUsername", "blueeagle"), 
                });

                var response = httpClient.PostAsync(AddNewPostEndpoint, bodyData).Result;
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode +" "+ response.Content.ReadAsStringAsync().Result);
                }
                else
                {
                    var post = response.Content.ReadAsAsync<PostDTO>().Result;
                    Console.WriteLine(post.Content);
                }
            }
        }

        private static void PrintAllPosts()
        {
            var httpClient = new HttpClient();
            using (httpClient)
            {
                httpClient.Timeout = new TimeSpan(0, 0, 0, 3);
                var response = httpClient.GetAsync(GetAllPostsEndpoint).Result;
                var posts = response.Content.ReadAsAsync<IEnumerable<PostDTO>>().Result;
                foreach (var post in posts)
                {
                    Console.WriteLine(post.Content);
                }
            }
        }
    }
}
