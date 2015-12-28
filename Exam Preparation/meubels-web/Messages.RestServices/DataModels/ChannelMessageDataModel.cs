namespace MessagesRestService.DataModels
{
    using System;
    using System.Linq.Expressions;

    using Messages.Data;
    using Messages.Models;

    public class ChannelMessageDataModel
    {
        public static Expression<Func<ChannelMessage, ChannelMessageDataModel>> DataModel
        {
            get     
            {
                return x => new ChannelMessageDataModel
                {
                    Text = x.Text,
                    UserName = x.User.UserName,
                    ChannelName = x.Channel.Name
                };
            }
        }

        public string Text { get; set; }

        public string UserName { get; set; }

        public string ChannelName { get; set; }
    }
}