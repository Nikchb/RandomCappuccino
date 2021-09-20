using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Mapper;
using System;
using System.Data.Common;

namespace RandomCappuccino.Tests
{
    public class TestBase : IDisposable
    {
        private DbConnection connection;

        protected DataBaseContext context;

        protected IMapper mapper;

        public virtual void SetUp()
        {
            connection = CreateInMemoryDatabase();
            context = new DataBaseContext(
                new DbContextOptionsBuilder<DataBaseContext>()
                .UseSqlite(connection)
                .Options);
            context.Database.Migrate();
            mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
        }

        private static DbConnection CreateInMemoryDatabase()
        {
            var connection = new SqliteConnection("Filename=:memory:");

            connection.Open();

            return connection;
        }

        public virtual void Dispose()
        {
            context.Dispose();
            connection.Dispose();
        }
    }
}