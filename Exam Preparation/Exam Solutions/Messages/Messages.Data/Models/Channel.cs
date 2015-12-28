using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Messages.Data.Models
{
    public class Channel
    {
        private ICollection<ChannelMessage> channelMessages;

        public Channel()
        {
            this.channelMessages = new HashSet<ChannelMessage>();
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [MinLength(1)]
        [MaxLength(200)]
        public string Name { get; set; }

        public virtual ICollection<ChannelMessage> ChannelMessages
        {
            get { return this.channelMessages; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                this.channelMessages = value;
            }
        }

    }
}
