using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Messages.Data.Models;
using Messages.Data.UnitOfWork;
using Messages.RestServices.Models.BindingModels;
using Messages.RestServices.Models.ViewModels;

namespace Messages.RestServices.Controllers
{
    public class ChannelsController : BaseApiController
    {
        public ChannelsController(IMessagesUnitOfWork data)
            : base(data)
        {
        }

        public ChannelsController()
        {
        }

        // GET: api/Channels
        public IHttpActionResult GetChannels()
        {
            var channels = data.Channels.All()
                .OrderBy(c => c.Name)
                .Select(c => new ChannelsViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                });
            
            return this.Ok(channels);
        }

        // GET: api/Channels/5
        [ResponseType(typeof(ChannelsViewModel))]
        public IHttpActionResult GetChannel(int id)
        {
            var channel = data.Channels.All()
                .Where(c => c.Id == id)
                .Select(c=>new ChannelsViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .FirstOrDefault();

            if (channel == null)
            {
                return NotFound();
            }

            return this.Ok(channel);
        }

        // PUT: api/Channels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutChannel(int id, ChannelBindingModel  channelData)
        {
            if (channelData == null)
            {
                return this.BadRequest("ChannelName cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var channel = data.Channels.Find(id);

            if (channel == null)
            {
                return this.NotFound();
            }

            var duplicatedChannel = data.Channels.All().Any(c => c.Name == channelData.Name && c.Id != channel.Id);

            if (duplicatedChannel)
            {
                return this.Conflict();
            }

            channel.Name = channelData.Name;
            data.SaveChanges();

            return this.Ok(new
            {
                Message = "Channel #" + channel.Id + " edited successfully."
            });
        }

        // POST: api/Channels
        [ResponseType(typeof(ChannelsViewModel))]
        public IHttpActionResult PostChannel(ChannelBindingModel channelData)
        {
            if (channelData == null)
            {
                return this.BadRequest("Channel name cannot be empty.");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (data.Channels.All().Any(c => c.Name == channelData.Name))
            {
                return this.Conflict();
            }
            
            var channel = new Channel()
            {
                Name = channelData.Name
            };

            data.Channels.Add(channel);
            data.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = channel.Id}, new ChannelsViewModel{Id = channel.Id, Name = channel.Name});
        }

        // DELETE: api/Channels/5
        [ResponseType(typeof(ChannelsViewModel))]
        public IHttpActionResult DeleteChannel(int id)
        {
            var channel = data.Channels.Find(id);
            
            if (channel == null)
            {
                return NotFound();
            }

            if (channel.ChannelMessages.Any())
            {
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.Conflict,
                    new { Message = "Cannot delete channel #" + channel.Id + " because it is not empty." }));
            }

            data.Channels.Remove(channel);
            data.SaveChanges();

            return Ok(new {Message = "Channel #" + channel.Id + " deleted."});
        }
    }
}