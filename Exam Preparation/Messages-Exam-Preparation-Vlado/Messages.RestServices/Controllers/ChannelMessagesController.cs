using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;
using MessagesRestService.DataModels;
using MessagesRestService.BindingModels;
using Messages.Data;
using Microsoft.AspNet.Identity;
using Messages.Models;

namespace MessagesRestService.Controllers
{
    [RoutePrefix("api/channel-messages")]
    public class ChannelMessagesController : BaseApiController
    {
        public ChannelMessagesController() 
            : this(new MessagesData(new MessagesDbContext()))
        {
        }

        public ChannelMessagesController(IMessagesData data) 
            : base(data) 
        {
        }

        [Route("{channelName}/{limit}")]
        public IHttpActionResult GetAllChannelMessages(string channelName, int? limit = null)
        {
            if (!this.Data.Channels.All().Any(x => x.Name == channelName))
            {
                return this.BadRequest(string.Format("No existing channel {0} in db!", channelName));
            }

            var channelMessages = this.Data.ChannelMessages
                .All()
                .Where(x => x.Channel.Name == channelName)
                .Select(ChannelMessageDataModel.DataModel);

            if (limit.HasValue)
            {
                channelMessages = channelMessages
                    .Take(limit.Value);
            }

            return this.Ok(channelMessages);
        }

        [Route(Name = "GetById")]
        public IHttpActionResult GetById(int id)
        {
            var channelMessage = this.Data.ChannelMessages
                .All()
                .Where(x => x.Id == id)
                .Select(ChannelMessageDataModel.DataModel)
                .FirstOrDefault();

            return this.Ok(channelMessage);
        }

        [HttpPost]
        [Route("{channelName}")]
        public IHttpActionResult PostMessage(string channelName, ChannelMessageBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var channelId = this.Data.Channels
                .All()
                .Where(x => x.Name == channelName)
                .Select(x => x.Id)
                .FirstOrDefault();
            if (channelId == default(int))
            {
                return this.BadRequest("No existing channel!");
            }

            var channelMessage = new ChannelMessage
            {
                Text = model.Text,
                ChannelId = channelId
            };

            if (this.User.Identity.IsAuthenticated)
            {
                channelMessage.UserId = this.User.Identity.GetUserId();
            }

            this.Data.ChannelMessages.Add(channelMessage);
            this.Data.SaveChanges();

            return this.CreatedAtRoute("GetById", new { id = channelMessage.Id }, channelMessage);
        }
    }
}