using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services;
using RandomCappuccino.Server.Services.IdentityManager;
using RandomCappuccino.Server.Services.UserManager;
using RandomCappuccino.Server.Services.UserManager.DTOs;
using RandomCappuccino.Tests.InterfaceImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Tests.Services
{
    [TestFixture]
    public class UserManagerTests : TestBase
    {
        private IIdentityManager identityManager;

        private IUserManager userManager;

        private readonly string email = "email@mail.com";

        private readonly string role = "Role";

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();           

            var user = new User { Email = email, Password = AppFunctions.HashPassword("123456") };                
            var role = new UserRole { UserId = user.Id, Role = this.role };
            context.Users.Add(user);
            context.UserRoles.Add(role);
            context.SaveChanges();            
            identityManager = new TestIdentityManager(user.Id);
            userManager = new UserManager(context, mapper, identityManager);
        }

        [Test]
        public async Task CreateUser()
        {
            var model = new CreateUserDTO("mail@mail.com", "123456", "Customer");
            
            var response = await userManager.CreateUser(model);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Email, response.Content.Email);            
        }

        [Test]
        public async Task CreateUserSameEmail()
        {
            var model = new CreateUserDTO("email@mail.com", "123456", "Customer");

            var response = await userManager.CreateUser(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("This email is already used", response.Messages.First());
        }

        [Test]
        public async Task GetUserInfo()
        {
            var response = await userManager.GetUserInfo();

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(email, response.Content.Email);
        }

        [Test]
        public async Task GetUserInfoWrongId()
        {
            var userManager = new UserManager(context, mapper, new TestIdentityManager("id"));

            var response = await userManager.GetUserInfo();

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task UpdateUserInfo()
        {
            var model = new UpdateUserInfoDTO { Email = "mail@mail.com" };

            var response = await userManager.UpdateUserInfo(model); 

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Email, response.Content.Email);
        }

        [Test]
        public async Task UpdateUserInfoWrongId()
        {
            var userManager = new UserManager(context, mapper, new TestIdentityManager("id"));

            var model = new UpdateUserInfoDTO { Email = "mail@mail.com" };

            var response = await userManager.UpdateUserInfo(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task UpdateUserPassword()
        {
            var model = new UpdateUserPasswordDTO { CurrentPassword = "123456", NewPassword = "12345678" };

            var response = await userManager.UpdateUserPassword(model);

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task UpdateUserPasswordWrongId()
        {
            var userManager = new UserManager(context, mapper, new TestIdentityManager("id"));

            var model = new UpdateUserPasswordDTO { CurrentPassword = "123456", NewPassword = "12345678" };

            var response = await userManager.UpdateUserPassword(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task UpdateUserPasswordWrongPassword()
        {
            var model = new UpdateUserPasswordDTO { CurrentPassword = "password", NewPassword = "12345678" };

            var response = await userManager.UpdateUserPassword(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Wrong password", response.Messages.First());
        }

        [Test]
        public async Task GetUserRoles()
        {          
            var response = await userManager.GetUserRoles(identityManager.UserId);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(role, response.Content.First());
        }

        [Test]
        public async Task GetUserRolesWrongId()
        {           
            var response = await userManager.GetUserRoles("id");

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(0, response.Content.Count());
        }


        [Test]
        public async Task AddUserRoles()
        {
            var response = await userManager.AddUserRoles("Admin");

            Assert.IsTrue(response.Succeed);
            Assert.IsTrue(context.UserRoles.Where(v=>v.UserId == identityManager.UserId).Any(v=>v.Role == "Admin"));
        }

        [Test]
        public async Task AddUserRolesWrongId()
        {
            var userManager = new UserManager(context, mapper, new TestIdentityManager("id"));

            var response = await userManager.AddUserRoles("Admin");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task AddUserRolesAlreadyExists()
        {
            var response = await userManager.AddUserRoles("Customer");

            Assert.IsTrue(response.Succeed);
        }

        [Test]
        public async Task RemoveUserRoles()
        {
            var response = await userManager.RemoveUserRoles("Customer");

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task RemoveUserRolesWrongId()
        {
            var userManager = new UserManager(context, mapper, new TestIdentityManager("id"));

            var response = await userManager.RemoveUserRoles("Customer");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task RemoveUserRolesNotHaveRole()
        {
            var response = await userManager.RemoveUserRoles("Admin");

            Assert.IsTrue(response.Succeed);
        }

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
