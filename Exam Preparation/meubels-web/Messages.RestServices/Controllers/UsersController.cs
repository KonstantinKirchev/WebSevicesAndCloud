namespace MessagesRestService.Controllers
{
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

    [Authorize]
    [RoutePrefix("api/user/personal-messages")]
    public class UsersController : BaseApiController
    {
        public UsersController() : this(new MessagesData(new MessagesDbContext()))
        {

        }

        public UsersController(IMessagesData data) : base(data) { }

        [HttpGet]
        public IHttpActionResult GetAllPersonalMessages()
        {
            var userId = this.User.Identity.GetUserId();
            var messages = this.Data.UserMessages
                .GetUserMessages(userId)
                .Select(UserMessageDataModel.DataModel);

            return this.Ok(messages);
        }

        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult PostMessage(UserMessageBindingModel model)
        {
            if (model == null || !this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var sentToUser = this.Data.Users
                .All()
                .FirstOrDefault(x => x.UserName == model.Recipient);
            if (sentToUser == null)
            {
                return this.BadRequest("Recipient is not found!");
            }

            var userMessage = new UserMessage
            {
                SendToUserId = sentToUser.Id,
                Text = model.Text
            };

            if (this.User.Identity.IsAuthenticated)
            {
                userMessage.SendByUserId = this.User.Identity.GetUserId();
            }

            this.Data.UserMessages.Add(userMessage);
            this.Data.SaveChanges();

            return this.Ok(userMessage);
        }
    }
}