using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models
{
    public class Post
    {
        public string PostId { get; set; }
        public String Content { get; set; }
        public Author Author { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}