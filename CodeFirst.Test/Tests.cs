using System;
using System.Collections.Generic;
using System.Linq;
using CodeFirst.Data;
using CodeFirst.Models;
using NUnit.Framework;
using TestSupport.EfHelpers;

namespace TestCodeFirst
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShouldBeAbleToSaveDataToDB()
        {
            var logs = new List<LogOutput>();
            var uniqueClassOptions = SqliteInMemory.CreateOptionsWithLogging<MyDbContext>(output =>
            {
                Console.WriteLine(output);
                logs.Add(output);
            });

            using (var context = new MyDbContext(uniqueClassOptions))
            {
                context.CreateEmptyViaWipe();
                context.Database.EnsureCreated();
                var author = new Author {NickName = "Dr.Who", FirstName = "Doctor", LastName = "Who", Email = "doctor@who.com"};
                var userA = new User {Email = "A@user.com", FirstName = "Ted", LastName = "Test", NickName = "TestA"};
                var userB = new User {Email = "B@user.com", FirstName = "Jim", LastName = "Test", NickName = "TestB"};
                var post = new Post
                {
                    Content = "first post", 
                    Comments = new[]
                    {
                        new Comment {User = userA, CommentContent = "hhh"},
                        new Comment {User = userB, CommentContent = "bbb"},
                    },
                    Author = author
                };
                context.Posts.Add(post);
                context.SaveChanges();
                Assert.That(context.Posts.First().PostId, Is.Not.Empty);
            }
        }
    }
}