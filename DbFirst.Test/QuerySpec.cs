using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DBFirst.Data;
using DBFirst.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Proxies.Internal;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using TestSupport.EfHelpers;

namespace DbFirst.Test
{
    public class QuerySpec
    {
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

        [Test]
        public void Basic()
        {
            var (context, logOutputs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");

                Assert.That(order, Is.Not.Null);
                Assert.That(order.Employee, Is.Null);
                Assert.That(logOutputs.Count, Is.EqualTo(1));
            }
        }


        [Test]
        public void Include()
        {
            var (context, logOutputs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .Include(o => o.Employee)
                    .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");

                Assert.That(order, Is.Not.Null);
                Assert.That(order.Employee, Is.Not.Null);
                Assert.That(logOutputs.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void Lazy()
        {
            var logOutputs = new List<LogOutput>();
            var optionsWithLogging =
                SqliteInMemory.CreateOptions<Northwind_smallContext>()
                        .WithExtension(new ProxiesOptionsExtension().WithLazyLoading()) as
                    DbContextOptions<Northwind_smallContext>;
            using var context = new Northwind_smallContext(optionsWithLogging);
            context.Database.ExecuteSqlRaw(CreaeSqlCommands);
            context.SaveChanges();
            SetupLogging(context, logOutputs);
            var stopwatch = Stopwatch.StartNew();
            var order = context.Order
                .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");

            Assert.That(order, Is.Not.Null);
            Assert.That(order.Employee, Is.Not.Null);
            Assert.That(order.Customer, Is.Not.Null);
            Console.WriteLine(order.Customer);
            Console.WriteLine(
                $"Employee: {order?.Employee.FirstName + " " + order?.Employee.LastName} " +
                $"Reports to {order?.Employee?.ReportsToEmployee.FirstName + " " + order?.Employee?.ReportsToEmployee.LastName} " +
                $"Service {order.Customer.ContactName}." +
                $"BTW he also take a look a {order.Employee.EmployeeTerritories.Select(territory => territory.Territory).Count()} territories." +
                $"Such as: {String.Join(",", order.Employee.EmployeeTerritories .Select(et => et.Territory.TerritoryDescription + " in " + et.Territory.Region.RegionDescription))}"
            );
            stopwatch.Stop();
            Console.WriteLine("takes {0} ms ", stopwatch.ElapsedMilliseconds);
            Assert.That(logOutputs.Count, Is.EqualTo(13));
        }
        
        [Test]
        public void IncludeOnly()
        {
            var (context, logs) = InitialContext();
            var stopwatch = Stopwatch.StartNew();

            var order =  context.Order
                .Include(o => o.Employee)
                    .ThenInclude(employee => employee.EmployeeTerritories)
                        .ThenInclude(territory => territory.Territory)
                            .ThenInclude(territory => territory.Region)
                .Include(o => o.Employee)
                    .ThenInclude(e=> e.ReportsToEmployee)
                .Include(o=>o.Customer)
                .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");

            Assert.That(order, Is.Not.Null);
            Assert.That(order.Employee, Is.Not.Null);
            Assert.That(order.Customer, Is.Not.Null);
            Console.WriteLine(order.Customer);
            Console.WriteLine(
                $"Employee: {order?.Employee.FirstName + " " + order?.Employee.LastName} " +
                $"Reports to {order?.Employee?.ReportsToEmployee.FirstName + " " + order?.Employee?.ReportsToEmployee.LastName} " +
                $"Service {order.Customer.ContactName}." +
                $"BTW he also take a look a {order.Employee.EmployeeTerritories.Select(territory => territory.Territory).Count()} territories." +
                $"Such as: {String.Join(",", order.Employee.EmployeeTerritories .Select(et => et.Territory.TerritoryDescription + " in " + et.Territory.Region.RegionDescription))}"
            );
            stopwatch.Stop();
            Console.WriteLine("takes {0} ms ", stopwatch.ElapsedMilliseconds);
            Assert.That(logs.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void IncludeAndExplicit()
        {
            var (context, logs) = InitialContext();
            var stopwatch = Stopwatch.StartNew();

            var order = context.Order
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                  .ThenInclude(employee => employee.ReportsToEmployee)
                .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");
            context.Entry(order.Employee).Collection(e=>e.EmployeeTerritories).Query().Include(territory => territory.Territory).ThenInclude(territory => territory.Region).Load();

            Assert.That(order, Is.Not.Null);
            Assert.That(order.Employee, Is.Not.Null);
            Assert.That(order.Customer, Is.Not.Null);
            Console.WriteLine(order.Customer);
            Console.WriteLine(
                $"Employee: {order?.Employee.FirstName + " " + order?.Employee.LastName} " +
                $"Reports to {order?.Employee?.ReportsToEmployee.FirstName + " " + order?.Employee?.ReportsToEmployee.LastName} " +
                $"Service {order.Customer.ContactName}." +
                $"BTW he also take a look a {order.Employee.EmployeeTerritories.Select(territory => territory.Territory).Count()} territories." +
                $"Such as: {String.Join(",", order.Employee.EmployeeTerritories .Select(et => et.Territory.TerritoryDescription + " in " + et.Territory.Region.RegionDescription))}"
                );
            stopwatch.Stop();
            Console.WriteLine("takes {0} ms ", stopwatch.ElapsedMilliseconds);
            Assert.That(logs.Count, Is.EqualTo(2));
        }

        [Test]
        public void Explicit()
        {
            var (context, logOutputs) = InitialContext();
            var order = context.Order
                .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");

            Assert.That(order, Is.Not.Null);
            Assert.That(order.Employee, Is.Null);
            Assert.That(logOutputs.Count, Is.EqualTo(1));

            context.Entry(order).Reference(o => o.Employee).Load();
            Assert.That(order.Employee, Is.Not.Null);
            Assert.That(logOutputs.Count, Is.EqualTo(2));
        }

        [Test]
        public void Projection()
        {
            var (context, logOutputs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .Where(o => o.Customer.CompanyName == "Vins et alcools Chevalier")
                    .Select(o => new
                    {
                        CustomerName = o.Customer.ContactName,
                        EmployeeName = o.Employee.FirstName + " " + o.Employee.LastName
                    })
                    .FirstOrDefault();
                Assert.That(order, Is.Not.Null);
                Assert.That(order.CustomerName, Is.Not.Null);
                Assert.That(order.EmployeeName, Is.Not.Null);
                Assert.That(logOutputs.Count, Is.EqualTo(1));
            }
        }

        string GetName(Employee e)
        {
            return e.FirstName + " " + e.LastName;
        }

        [Test]
        public void ClientEval()
        {
            var (context, logOutputs) = InitialContext();
            dynamic order;
            using (context)
            {
                Assert.Catch(() =>
                {
                    order = context.Order
                        .Select(o => new
                        {
                            CustomerName = o.Customer.ContactName,
                            EmployeeName = GetName(o.Employee)
                        })
                        .FirstOrDefault(o => o.EmployeeName == "Steven Buchanan");
                });
            }
        }

        [Test]
        public void Tracking()
        {
            var (context, logOutputs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .Include(o => o.Employee)
                    .Include(o => o.Customer)
                    .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");
                order.Customer.CompanyName = "abc";

                Assert.That(context.Entry(order.Customer).State, Is.EqualTo(EntityState.Modified));
            }
        }

        [Test]
        public void NoTracking()
        {
            var (context, logOutputs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .AsNoTracking()
                    .Include(o => o.Employee)
                    .Include(o => o.Customer)
                    .FirstOrDefault(o => o.Customer.CompanyName == "Vins et alcools Chevalier");
                order.Customer.CompanyName = "abc";

                Assert.That(context.Entry(order.Customer).State, Is.EqualTo(EntityState.Detached));
            }
        }

        [Test]
        public void RawSql()
        {
            var (context, logOutputs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .FromSqlRaw(
                        "select * from \"Order\" join Customer on CustomerId = Customer.Id\nwhere ContactName = \"Alejandra Camino\"")
                    .Include(order1 => order1.Customer)
                    .FirstOrDefault();
                Assert.That(order.Customer, Is.Not.Null);
            }
        }

        [Test]
        public void RawSqlWithParamater()
        {
            var (context, logOutputs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .FromSqlInterpolated(
                        $"select * from \"Order\" join Customer on CustomerId = Customer.Id\nwhere ContactName = {"Alejandra Camino"}")
                    .Include(order1 => order1.Customer)
                    .FirstOrDefault();
                Assert.That(order.Customer, Is.Not.Null);
            }
        }

        [Test]
        public void SearchStartWith()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .Include(o => o.Customer)
                    .FirstOrDefault(o => o.Customer.CompanyName.StartsWith("Vins et alcools"));


                Assert.That(order, Is.Not.Null);
                Assert.That(order.Customer.CompanyName, Is.EqualTo("Vins et alcools Chevalier"));
                Assert.That(logs.Count, Is.EqualTo(1));
                Assert.That(logs[0].Message.Contains("LIKE 'Vins et alcools%'"));
            }
        }

        [Test]
        public void SearchIndexOf()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .Include(o => o.Customer)
                    .FirstOrDefault(o => o.Customer.CompanyName.IndexOf("et alcools") >= 0);


                Assert.That(order, Is.Not.Null);
                Assert.That(order.Customer.CompanyName, Is.EqualTo("Vins et alcools Chevalier"));
                Assert.That(logs.Count, Is.EqualTo(1));
                Assert.That(logs[0].Message.Contains("instr"));
                Assert.That(logs[0].Message.Contains("'et alcools'"));
            }
        }

        [Test]
        public void EndWith()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .Include(o => o.Customer)
                    .FirstOrDefault(o => o.Customer.CompanyName.EndsWith("alcools Chevalier"));
                Assert.That(order, Is.Not.Null);
                Assert.That(order.Customer.CompanyName, Is.EqualTo("Vins et alcools Chevalier"));
                Assert.That(logs.Count, Is.EqualTo(1));
                Assert.That(logs[0].Message.Contains("'%alcools Chevalier"));
            }
        }

        [Test]
        public void Like()
        {
            var (context, logs) = InitialContext();
            using (context)
            {
                var order = context.Order
                    .Include(o => o.Customer)
                    .FirstOrDefault(o => EF.Functions.Like(o.Customer.CompanyName, "%et alcools%"));
                Assert.That(order, Is.Not.Null);
                Assert.That(order.Customer.CompanyName, Is.EqualTo("Vins et alcools Chevalier"));
                Assert.That(logs.Count, Is.EqualTo(1));
                Assert.That(logs[0].Message.Contains("LIKE '%et alcools%'"));
            }
        }
    }
}