using System;
using System.Linq;
using System.Web.Http;
using Messages.Data.Models;
using Messages.Data.UnitOfWork;
using Messages.RestServices.Models.BindingModels;
using Messages.RestServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace Messages.RestServices.Controllers
{
    [RoutePrefix("api/channel-messages")]
    public class ChannelMessagesController : BaseApiController
    {
        public ChannelMessagesController(IMessagesUnitOfWork data)
            : base(data)
        {
        }

        public ChannelMessagesController()
        {
        }

        // GET api/channel-messages/{channelName}
        [Route("{channelName}")]
        public IHttpActionResult GetChannelMessages(string channelName, [FromUri]string limit = null)
        {
            var channel = data.Channels.All().FirstOrDefault(c => c.Name == channelName);

            if (channel == null)
            {
                return NotFound();
            }

            var channelMessages = channel.ChannelMessages
                .OrderByDescending(m => m.DateSent)
                .ThenByDescending(m => m.Id)
                .Select(m => new ChannelMessageViewModel
                {
                    Id = m.Id,
                    Text = m.Text,
                    DateSent = m.DateSent,
                    Sender = m.Sender != null ? m.Sender.UserName : null
                });

            if (limit != null)
            {
                var maxCount = 0;
                int.TryParse(limit, out maxCount);
                if (maxCount < 1 || maxCount > 1000)
                {
                    return this.BadRequest("Limit is out of range [1...1000].");
                }
                channelMessages = channelMessages.Take(maxCount);
            }

            return Ok(channelMessages);
        }

        // POST api/channel-messages
        [Route("{channelName}")]
        public IHttpActionResult PostChannelMessages(string channelName, MessageBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var channel = data.Channels.All().FirstOrDefault(c => c.Name == channelName);

            if (channel == null)
            {
                return this.NotFound();
            }

            var currentUserId = User.Identity.GetUserId();

            var message = new ChannelMessage
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                ChannelId = channel.Id,
                SenderId = currentUserId
            };

            data.ChannelMessages.Add(message);
            data.SaveChanges();

            if (currentUserId != null)
            {
                return Ok(new MessageViewModel()
                {
                    Id = message.Id,
                    Sender = User.Identity.GetUserName(),
                    Message = "Message sent to channel " + channel.Name + "."
                });
            }

            return Ok(new 
            {
                message.Id,
                Message = "Anonimous message sent to channel " + channel.Name + "."
            });
        }
    }
}
