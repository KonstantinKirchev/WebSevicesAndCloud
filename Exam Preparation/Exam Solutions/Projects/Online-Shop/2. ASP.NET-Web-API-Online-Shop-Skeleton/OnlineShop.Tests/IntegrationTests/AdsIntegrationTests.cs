using System;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using Owin;

namespace OnlineShop.Tests.IntegrationTests
{
    [TestClass]
    public class AdsIntegrationTests
    {
        private const string TestUserUsername = "blueeagle";
        private const string TestUserPassword = "Moti4ka!";

        private static TestServer httpTestServer;
        private static HttpClient httpClient;

        [AssemblyInitialize]
        public static void AssemblyInIt(TestContext context)
        {
            httpTestServer = TestServer.Create(appBuilder =>
            {
                var config = new HttpConfiguration();
                WebApiConfig.Register(config);
                var startup = new Startup();

                startup.Configuration(appBuilder);
                appBuilder.UseWebApi(config);
            });

            httpClient = httpTestServer.HttpClient;
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            if (httpTestServer != null)
            {
                httpTestServer.Dispose();
            }
        }

        private static void SeedDatabase()
        {
            var context = new OnlineShopContext();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);

            var user = new ApplicationUser()
            {
                UserName = TestUserUsername,
                Email = "prakash@yahoo.in"
            };

            var result = userManager.CreateAsync(user, TestUserPassword).Result;

            if (!result.Succeeded)
            {
                Assert.Fail(String.Join(Environment.NewLine, result.Errors));
            }

            //SeedCategories(context);
            //SeedAdTypes(context);
        }
    }
}
