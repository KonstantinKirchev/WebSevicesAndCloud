using Messages.Data;
using Messages.Models;
using System;
using System.Linq.Expressions;
namespace MessagesRestService.DataModels
{
    public class ChannelDataModel
    {
        public static Expression<Func<Channel, ChannelDataModel>> DataModel
        {
            get
            {
                return x => new ChannelDataModel()
                {
                    Id = x.Id,
                    Name = x.Name
                };
            }
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}