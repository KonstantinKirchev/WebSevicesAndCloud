using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OnlineShop.Data.UnitOfWork;
using OnlineShop.Services.Controllers;

namespace OnlineShop.Tests.UnitTests
{
    [TestClass]
    public class AdsControllerTests
    {
        private MockContainer mocks;

        [TestInitialize]
        public void InitTest()
        {
            this.mocks = new MockContainer();
            this.mocks.PrepareMocks();
        }

        [TestMethod]
        public void GetAllAds_Should_Return_Total_Ads_Ordered_By_TypeIndex()
        {
            //Arrange
            var fakeAds = this.mocks.AdRepositoryMock.Object.All();

            var mockContext = new Mock<IOnlineShopData>();

            var mockedAds = mockContext.Object.Ads.All();

            //Act

            //var response = adsController..ExecuteAsync(CancellationToken.None).Result;
            //Assert

        }
    }
}
