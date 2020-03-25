using System;

namespace CodeFirst.Models
{
    public class Comment
    {
        public String CommentId { get; set; }
        public User User { get; set; }
        public String CommentContent { get; set; }
    }
}