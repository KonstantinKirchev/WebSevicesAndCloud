using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using BidSystem.Data;
using BidSystem.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BidSystem.Tests.IntegrationTests
{
    [TestClass]
    public class OfferDetailsIntegrationTests
    {
        [TestMethod]
        public void GetOffer_ExistingOffer_ShouldReturn200_OK()
        {
            // Arrange -> clean the database and register new user
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("kosta", "pAssW@rd#123456");
            var context = new BidSystemDbContext();

            // Act -> create a few offers
            var offersToAdds = new OfferModel[]
            {
                new OfferModel() { Title = "First Offer (Expired)", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(-5)},
                new OfferModel() { Title = "Another Offer (Expired)", InitialPrice = 15.50m, ExpirationDateTime = DateTime.Now.AddDays(-1)},
                new OfferModel() { Title = "Second Offer (Active 3 months)", Description = "Description", InitialPrice = 500, ExpirationDateTime = DateTime.Now.AddMonths(3)},
                new OfferModel() { Title = "Third Offer (Active 6 months)", InitialPrice = 120, ExpirationDateTime = DateTime.Now.AddMonths(6)},
            };
            foreach (var offer in offersToAdds)
            {
                var httpResult = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offer.Title,
                    offer.Description, offer.InitialPrice, offer.ExpirationDateTime);
                Assert.AreEqual(HttpStatusCode.Created, httpResult.StatusCode);
            }

            // Assert -> offers created correctly
            var existingOffer = context.Offers.FirstOrDefault();

            if (existingOffer == null)
            {
                Assert.Fail("Cannot perform tests - no bugs in DB.");
            }

            var endpoint = string.Format("api/offers/details/{0}", existingOffer.Id);

            var httpResponse = TestingEngine.HttpClient.GetAsync(endpoint).Result;

            Assert.AreEqual(HttpStatusCode.OK, httpResponse.StatusCode);
        }

        [TestMethod]
        public void GetOffer_NonExistingOffer_ShouldReturn404_NotFound()
        {
            // Arrange -> clean the database and register new user
            TestingEngine.CleanDatabase();
            var userSession = TestingEngine.RegisterUser("kosta", "pAssW@rd#123456");
            var context = new BidSystemDbContext();

            // Act -> create a few offers
            var offersToAdds = new OfferModel[]
            {
                new OfferModel() { Title = "First Offer (Expired)", Description = "Description", InitialPrice = 200, ExpirationDateTime = DateTime.Now.AddDays(-5)},
                new OfferModel() { Title = "Another Offer (Expired)", InitialPrice = 15.50m, ExpirationDateTime = DateTime.Now.AddDays(-1)},
                new OfferModel() { Title = "Second Offer (Active 3 months)", Description = "Description", InitialPrice = 500, ExpirationDateTime = DateTime.Now.AddMonths(3)},
                new OfferModel() { Title = "Third Offer (Active 6 months)", InitialPrice = 120, ExpirationDateTime = DateTime.Now.AddMonths(6)},
            };
            foreach (var offer in offersToAdds)
            {
                var httpResult = TestingEngine.CreateOfferHttpPost(userSession.Access_Token, offer.Title,
                    offer.Description, offer.InitialPrice, offer.ExpirationDateTime);
                Assert.AreEqual(HttpStatusCode.Created, httpResult.StatusCode);
            }

            // Assert -> offers created correctly

            var endpoint = string.Format("api/offers/details/{0}", -1);

            var httpResponse = TestingEngine.HttpClient.GetAsync(endpoint).Result;

            Assert.AreEqual(HttpStatusCode.NotFound, httpResponse.StatusCode);
        }
    }
}
