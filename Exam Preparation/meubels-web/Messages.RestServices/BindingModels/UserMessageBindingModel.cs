﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MessagesRestService.BindingModels
{
    public class UserMessageBindingModel
    {
        [Required]
        public string Recipient { get; set; }

        [Required]
        public string Text { get; set; }
    }
}