﻿using LambdaForum.Data.Models;
using System;

namespace LambdaForums.Data.Models
{
    public class PostReply
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string Content { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Post Post { get; set; }
    }
}
