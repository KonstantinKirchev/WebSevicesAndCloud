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
    public class UserMessagesController : BaseApiController
    {
        public UserMessagesController(IMessagesUnitOfWork data)
            : base(data)
        {
        }

        public UserMessagesController()
        {
        }

        // GET api/personal-messages
        [Authorize]
        [Route("api/user/personal-messages")]
        public IHttpActionResult GetPersonalMessages()
        {
            var currentUserId = User.Identity.GetUserId();

            if (currentUserId == null)
            {
                return this.Unauthorized();
            }

            var user = this.data.Users.Find(currentUserId);

            if (user == null)
            {
                return this.Unauthorized();
            }

            var userMessages = this.data.UserMessages.All()
                .Where(m => m.RecipientId == currentUserId)
                .OrderByDescending(m => m.DateSent)
                .ThenByDescending(m => m.Id)
                .Select(m => new UserMessageViewModel
                {
                    Id = m.Id,
                    Text = m.Text,
                    DateSent = m.DateSent,
                    Sender = m.Sender != null ? m.Sender.UserName : null
                });

            return Ok(userMessages);
        }

        // POST api/personal-messages
        [Route("api/user/personal-messages")]
        public IHttpActionResult PostPersonalMessages([FromBody] UserMessageBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var recipient = this.data.Users.All().FirstOrDefault(u => u.UserName == model.Recipient);

            if (recipient == null)
            {
                return this.NotFound();
            }

            var userId = User.Identity.GetUserId();
            User user = null;
            
            if (userId != null)
            {
                user = this.data.Users.All().FirstOrDefault(u => u.Id == userId);
            }

            if (userId != null && user == null)
            {
                return this.Unauthorized();
            }

            var userMessage = new UserMessage()
            {
                Text = model.Text,
                DateSent = DateTime.Now,
                SenderId = userId,
                RecipientId = recipient.Id
            };

            this.data.UserMessages.Add(userMessage);
            this.data.SaveChanges();

            if (user != null)
            {
                return this.Ok(new
                {
                    Id = userMessage.Id,
                    Sender = user.UserName,
                    Message = "Message sent to user " + recipient.UserName + "."
                });
            }
            return this.Ok(new
            {
                Id = userMessage.Id,
                Message = "Anonymous message sent successfully to user " + recipient.UserName + "."
            });
        }
    }
}
