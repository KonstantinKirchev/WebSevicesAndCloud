using Messages.Data;
using Messages.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace MessagesRestService.DataModels
{
    public class UserMessageDataModel
    {
        public static Expression<Func<UserMessage, UserMessageDataModel>> DataModel 
        { 
            get        
            {
                return x => new UserMessageDataModel
                {
                    Text = x.Text,
                    SentToUsername = x.SendToUser.UserName,
                    SentByUsername  = x.SendByUser.UserName
                };
            } 
        }

        public string Text { get; set; }

        public string SentToUsername { get; set; }

        public string SentByUsername { get; set; }
    }
}