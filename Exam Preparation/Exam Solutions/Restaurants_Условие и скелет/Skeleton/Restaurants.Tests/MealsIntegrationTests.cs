using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;
using Restaurants.Services;

namespace Restaurants.Tests
{
    [TestClass]
    public static class MealsIntegrationTests
    {
        private static TestServer TestWebServer { get; set; }

        public static HttpClient HttpClient { get; private set; }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            // Start OWIN testing HTTP server with Web API support
            TestWebServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var webAppStartup = new Startup();
                webAppStartup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });
            HttpClient = TestWebServer.HttpClient;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Stop the OWIN testing HTTP server
            if (TestWebServer != null)
            {
                TestWebServer.Dispose();
            }
        }
    }
}
