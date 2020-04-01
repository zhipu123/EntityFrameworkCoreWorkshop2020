using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Json;
using DBFirst.Data;
using DBFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using TestSupport.EfHelpers;

namespace DbFirst.Test
{
    [TestFixture]
    public class ChangesSpec
    {
        #region init

        public static string CreaeSqlCommands;

        private static (Northwind_smallContext context, List<LogOutput> logs) InitialContext()
        {
            var logOutputs = new List<LogOutput>();
            var optionsWithLogging = SqliteInMemory.CreateOptions<Northwind_smallContext>();
            var context = new Northwind_smallContext(optionsWithLogging);
            context.Database.ExecuteSqlRaw(CreaeSqlCommands);
            context.SaveChanges();
            SetupLogging(context, logOutputs);
            return (context, logOutputs);
        }

        public static void SetupLogging(DbContext context, List<LogOutput> logs)
        {
            var loggerFactory = context.GetService<ILoggerFactory>();
            loggerFactory.AddProvider(new MyLoggerProvider(logs));
        }

        [OneTimeSetUpAttribute]
        public void Setup()
        {
            using var sqlCommands =
                new StreamReader(GetType()!.Assembly.GetManifestResourceStream("DbFirst.Test.create.sql"));
            CreaeSqlCommands = sqlCommands.ReadToEnd();
        }

        #endregion

        [Test]
        public void Modify()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var firstOrder = context.Order.Include(o => o.Customer).Include(o => o.Employee).FirstOrDefault();
                var previousOrderCount = context.Order.Count();
                firstOrder.ShipName = "TestShip";
                context.SaveChanges();
            }
        }

        #region

        [Test]
        public void Add()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var firstOrder = context.Order.Include(o => o.Customer).Include(o => o.Employee).FirstOrDefault();
                var previousOrderCount = context.Order.Count();
                var newOrder = JsonSerializer.Deserialize<Order>(JsonSerializer.Serialize(firstOrder));
                newOrder.Id += 10000;
                newOrder.Customer = firstOrder.Customer;
                newOrder.Employee = firstOrder.Employee;
                context.Add(newOrder);
                context.SaveChanges();
                Assert.That(context.Order.Count(), Is.EqualTo(previousOrderCount + 1));
            }
        }

        [Test]
        public void Add2()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var firstOrder = context.Order.Include(o => o.Customer).Include(o => o.Employee).FirstOrDefault();
                var previousOrderCount = context.Order.Count();
                var newOrder = JsonSerializer.Deserialize<Order>(JsonSerializer.Serialize(firstOrder));
                newOrder.Id += 10000;
                Assert.Catch(() => { context.Add(newOrder); });
            }
        }

        #endregion


        #region Delete

        [Test]
        public void DeleteWithoutInclude()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var firstOrder = context.Order.FirstOrDefault();
                var previousOrderCount = context.Order.Count();
                context.Remove(firstOrder);
                Assert.That(context.Order.Count(), Is.EqualTo(previousOrderCount ));
                context.SaveChanges();
                Assert.That(context.Order.Count(), Is.EqualTo(previousOrderCount - 1));
            }
        }
        
        
        [Test]
        public void DeleteCustomer()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var firstOrder = context.Order.Include(o=>o.Customer).FirstOrDefault();
                var previousOrderCount = context.Order.Count();
                context.Remove(firstOrder.Customer);
                context.SaveChanges();
                Assert.That(context.Order.Count(),Is.Not.EqualTo(previousOrderCount));

            }
        }

        #endregion


        #region Validation

        [Test]
        public void Validation()
        {
            var (context, logs) = InitialContext();

            var firstOrder = context.Order.Include(o => o.Customer).Include(o => o.Employee).FirstOrDefault();
            var previousOrderCount = context.Order.Count();
            var newOrder = JsonSerializer.Deserialize<Order>(JsonSerializer.Serialize(firstOrder));
            newOrder.Id += 10000;
            newOrder.Customer = firstOrder.Customer;
            newOrder.Employee = firstOrder.Employee;
            newOrder.ShipAddress = "";
            var errors = new List<ValidationResult>();

            var isOk = Validator.TryValidateObject(newOrder, new ValidationContext(newOrder), errors, true);
            context.Add(newOrder);
            context.SaveChanges();
            Assert.That(isOk, Is.EqualTo(false));
        }

        #endregion
    }
}