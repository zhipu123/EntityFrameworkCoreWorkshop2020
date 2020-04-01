using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirst.Models
{
    [Table("Post_Comment")]
    public class Comment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CommentId { get; set; }
        public User User { get; set; }
        public string CommentContent { get; set; }
    }
}