using NUnit.Framework;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.GroupManager.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Tests.Services
{
    public class GroupManagerTests : TestBase
    {
        private IGroupManager groupManager;

        private string userId;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            groupManager = new GroupManager(mapper, context);

            var user = new User { Email = "email@mail.com", Password = "123456" };
            context.Users.Add(user);
            context.SaveChanges();
            userId = user.Id;
        }

        [Test]
        public async Task CreateGroup()
        {
            var model = new CreateGroupDTO { Name = "Group 1" };

            var response = await groupManager.CreateGroup(userId, model);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Name, response.Content.Name);
        }

        [Test]
        public async Task CreateGroupWrongUserId()
        {
            var model = new CreateGroupDTO { Name = "Group 1" };

            var response = await groupManager.CreateGroup("id", model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group creation is failed", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task UpdateGroup()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            var model = new GroupDTO { Id = group.Id, Name = "Group 2" };

            var response = await groupManager.UpdateGroup(userId, model);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Name, response.Content.Name);
        }

        [Test]
        public async Task UpdateGroupWrongGroupId()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            var model = new GroupDTO { Id = "id", Name = "Group 2" };

            var response = await groupManager.UpdateGroup(userId, model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task UpdateGroupWrongUserId()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            var model = new GroupDTO { Id = group.Id, Name = "Group 2" };

            var response = await groupManager.UpdateGroup("id", model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task UpdateGroupAccessForbiden()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);

            var user = new User { Email = "email1@mail.com", Password = "123456" };
            await context.Users.AddAsync(user);           
            
            await context.SaveChangesAsync();



            var model = new GroupDTO { Id = group.Id, Name = "Group 2" };

            var response = await groupManager.UpdateGroup(user.Id, model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task DeleteGroup()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();            

            var response = await groupManager.DeleteGroup(userId, group.Id);

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task DeleteeGroupWrongGroupId()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            var response = await groupManager.DeleteGroup(userId, "id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task DeleteGroupWrongUserId()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);
            await context.SaveChangesAsync();

            var response = await groupManager.DeleteGroup("id", group.Id);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task DeleteGroupAccessForbiden()
        {
            var group = new Group { Name = "Group 1", UserId = userId };
            await context.Groups.AddAsync(group);

            var user = new User { Email = "email1@mail.com", Password = "123456" };
            await context.Users.AddAsync(user);

            await context.SaveChangesAsync();           

            var response = await groupManager.DeleteGroup(user.Id, group.Id);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
