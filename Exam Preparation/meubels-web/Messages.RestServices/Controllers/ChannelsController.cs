namespace MessagesRestService.Controllers
{
    using System.Web.Http;
    using System.Linq;
    using System.Net.Http;
    using System.Net;

    using Messages.Data;
    using MessagesRestService.DataModels;
    using MessagesRestService.BindingModels;
    using Messages.Models;

    public class ChannelsController : BaseApiController
    {
        public ChannelsController() : this(new MessagesData(new MessagesDbContext()))
        {

        }

        public ChannelsController(IMessagesData data) : base(data)
        {
        }

        [HttpGet]
        public IHttpActionResult GetChannels()
        {
            var channels = this.Data.Channels
                .All()
                .OrderBy(x => x.Name)
                .Select(ChannelDataModel.DataModel);

            return this.Ok(channels);
        }

        [HttpGet]
        public IHttpActionResult GetChannelById(int id)
        {
            var channel = this.Data.Channels
                .All()
                .Select(ChannelDataModel.DataModel)
                .FirstOrDefault(x => x.Id == id);
            if (channel == null)
            {
                return this.NotFound();
            }

            return this.Ok(channel);
        }

        [HttpPost]
        public IHttpActionResult PostChannel(ChannelBindingModel model)
        {
            if (model == null || !this.ModelState.IsValid)
	        {
		        return this.BadRequest(this.ModelState);
	        }

            if (this.Data.Channels.All().Any(x => x.Name == model.Name))
            {
                return this.Conflict();
            }

            var channel = new Channel
            {
                Name = model.Name
            };

            this.Data.Channels.Add(channel);
            this.Data.SaveChanges();

            return this.CreatedAtRoute("DefaultApi", new { id = channel.Id }, channel);
        }

        [HttpPut]
        public IHttpActionResult EditChannel(int id, ChannelBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (this.Data.Channels.All().Any(x => x.Name == model.Name))
            {
                return this.Conflict();
            }

            var channel = this.Data.Channels.All().FirstOrDefault(x => x.Id == id);
            if (channel == null)
            {
                return this.NotFound();
            }

            channel.Name = model.Name;
            this.Data.SaveChanges();

            return this.Ok(new 
            { 
                Message = string.Format("Channel #{0} edited successfully.", channel.Id)
            });
        }

        [HttpDelete]
        public IHttpActionResult DeleteChannel(int id)
        {
            var channel = this.Data.Channels.Find(id);
            if (channel == null)
            {
                return this.NotFound();
            }

            if (channel.Messages.Any())
            {
                return this.Content(HttpStatusCode.Conflict, new { Message = string.Format("Cannot delete channel #{0} because it is not empty!", channel.Id) });
            }

            this.Data.Channels.Delete(channel);
            this.Data.SaveChanges();

            return this.Ok(new
            {
                Message = string.Format("Channel #{0} deleted.", channel.Id)
            });
        }
    }
}
