using MessagesRestService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Messages.IntegrationTests
{
    [TestClass]
    public class ChannelsControllerTests
    {
        [TestMethod]
        public void GetAllChannelsShouldReturnOkResponse()
        {
            var controller = new ChannelsController();

            var response = controller.GetChannels();
            var res = response.ExecuteAsync(new System.Threading.CancellationToken()).Result;

            Assert.IsNotNull(response);
        }
    }
}
