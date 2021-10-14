using NUnit.Framework;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.GroupManager.DTOs;
using RandomCappuccino.Server.Services.IdentityManager;
using RandomCappuccino.Tests.InterfaceImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Tests.Services
{
    public class GroupManagerTests : TestBase
    {
        private IIdentityManager identityManager;

        private IGroupManager groupManager;        

        private string groupId;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = new User { Email = "email@mail.com", Password = "123456" };
            var group = new Group { Name = "Group 1", UserId = user.Id };                
            context.Users.Add(user);
            context.Groups.Add(group);
            context.SaveChanges();

            groupId = group.Id;
            identityManager = new TestIdentityManager(user.Id);
            groupManager = new GroupManager(mapper, context, identityManager);                     
        }

        [Test]
        public async Task CreateGroup()
        {
            var model = new CreateGroupDTO { Name = "Group" };

            var response = await groupManager.CreateGroup(model);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Name, response.Content.Name);
        }

        [Test]
        public async Task CreateGroupWrongUserId()
        {
            var groupManager = new GroupManager(mapper, context, new TestIdentityManager("id"));

            var model = new CreateGroupDTO { Name = "Group" };

            var response = await groupManager.CreateGroup(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task UpdateGroup()
        {
            var model = new GroupDTO { Id = groupId, Name = "Group 2" };

            var response = await groupManager.UpdateGroup(model);

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task UpdateGroupWrongGroupId()
        {
            var model = new GroupDTO { Id = "id", Name = "Group 2" };

            var response = await groupManager.UpdateGroup(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task UpdateGroupAccessForbiden()
        {
            var groupManager = new GroupManager(mapper, context, new TestIdentityManager("id"));

            var model = new GroupDTO { Id = groupId, Name = "Group 2" };

            var response = await groupManager.UpdateGroup(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task DeleteGroup()
        {           
            var response = await groupManager.DeleteGroup(groupId);

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task DeleteGroupWrongGroupId()
        {
            var response = await groupManager.DeleteGroup("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task DeleteGroupAccessForbiden()
        {
            var groupManager = new GroupManager(mapper, context, new TestIdentityManager("id"));

            var response = await groupManager.DeleteGroup(groupId);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task GetGroup()
        {
            var response = await groupManager.GetGroup(groupId);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(groupId, response.Content.Id);
        }

        [Test]
        public async Task GetGroupWrongGroupId()
        {
            var response = await groupManager.GetGroup("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task GetGroupAccessForbiden()
        {
            var groupManager = new GroupManager(mapper, context, new TestIdentityManager("id"));

            var response = await groupManager.GetGroup(groupId);

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
