using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;

namespace CodeFirst.Models
{
    public class Post
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PostId { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(1)")]
        public string Content { get; set; }
        public Author Author { get; set; }
        public IList<Comment> Comments { get; set; }
        public static void ApplyPost(ModelBuilder  modelBuilder)
        {
            modelBuilder.Entity<Post>(builder =>builder.Property(post => post.Content).HasColumnType("VARCHAR(10)"));

        }
      
    }
}