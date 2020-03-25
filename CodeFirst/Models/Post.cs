using System;

namespace CodeFirst.Models
{
    public class Post
    {
        public string PostId { get; set; }
        public String Content { get; set; }
        public Author Author { get; set; }
    }
}