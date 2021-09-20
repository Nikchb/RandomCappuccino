using Microsoft.AspNetCore.Identity;
using NUnit.Framework;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.UserManager;
using RandomCappuccino.Server.Services.UserManager.DTOs;
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
        private UserManager userManager;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            userManager = new UserManager(context, mapper);
        }

        [Test]
        public async Task CreateUser()
        {
            var model = new CreateUserDTO("email@mail.com", "123456", "Customer");
            
            var response = await userManager.CreateUser(model);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Email, response.Content.Email);            
        }

        [Test]
        public async Task CreateUserSameEmail()
        {
            await context.Users.AddAsync(new User { Email = "email@mail.com", Password = "123456" });
            await context.SaveChangesAsync();

            var model = new CreateUserDTO("email@mail.com", "123456", "Customer");

            var response = await userManager.CreateUser(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("This email is already used", response.Messages.First());
        }

        [Test]
        public async Task GetUserInfo()
        {
            var user = new User { Email = "email@mail.com", Password = "123456" };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var response = await userManager.GetUserInfo(user.Id);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(user.Email, response.Content.Email);
        }

        [Test]
        public async Task GetUserInfoWrongId()
        {
            var response = await userManager.GetUserInfo("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task UpdateUserInfo()
        {
            var user = new User { Email = "email@mail.com", Password = "123456" };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var model = new UpdateUserInfoDTO { Email = "mail@mail.com" };

            var response = await userManager.UpdateUserInfo(user.Id, model); 

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Email, response.Content.Email);
        }

        [Test]
        public async Task UpdateUserInfoWrongId()
        {
            var model = new UpdateUserInfoDTO { Email = "mail@mail.com" };

            var response = await userManager.UpdateUserInfo("id", model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task UpdateUserPassword()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var model = new UpdateUserPasswordDTO { CurrentPassword = "123456", NewPassword = "12345678" };

            var response = await userManager.UpdateUserPassword(user.Id, model);

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task UpdateUserPasswordWrongId()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var model = new UpdateUserPasswordDTO { CurrentPassword = "123456", NewPassword = "12345678" };

            var response = await userManager.UpdateUserPassword("id", model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task UpdateUserPasswordWrongPassword()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var model = new UpdateUserPasswordDTO { CurrentPassword = "password", NewPassword = "12345678" };

            var response = await userManager.UpdateUserPassword(user.Id, model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Wrong password", response.Messages.First());
        }

        [Test]
        public async Task GetUserRoles()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            var role = new UserRole { UserId = user.Id, Role = "Customer" };
            await context.Users.AddAsync(user);
            await context.UserRoles.AddAsync(role);
            await context.SaveChangesAsync();

            var response = await userManager.GetUserRoles(user.Id);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(role.Role, response.Content.First());
        }

        [Test]
        public async Task GetUserRolesWrongId()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            var role = new UserRole { UserId = user.Id, Role = "Customer" };
            await context.Users.AddAsync(user);
            await context.UserRoles.AddAsync(role);
            await context.SaveChangesAsync();

            var response = await userManager.GetUserRoles("id");

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(0, response.Content.Count());
        }


        [Test]
        public async Task AddUserRoles()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };            
            await context.Users.AddAsync(user);            
            await context.SaveChangesAsync();

            var response = await userManager.AddUserRoles(user.Id, "Customer");

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual("Customer", context.UserRoles.Where(v=>v.UserId == user.Id).FirstOrDefault().Role);
        }

        [Test]
        public async Task AddUserRolesWrongId()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var response = await userManager.AddUserRoles("id", "Customer");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task AddUserRolesAlreadyExists()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            var role = new UserRole { UserId = user.Id, Role = "Customer" };
            await context.Users.AddAsync(user);
            await context.UserRoles.AddAsync(role);
            await context.SaveChangesAsync();

            var response = await userManager.AddUserRoles(user.Id, "Customer");

            Assert.IsTrue(response.Succeed);
        }

        [Test]
        public async Task RemoveUserRoles()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            var role = new UserRole { UserId = user.Id, Role = "Customer" };
            await context.Users.AddAsync(user);
            await context.UserRoles.AddAsync(role);
            await context.SaveChangesAsync();

            var response = await userManager.RemoveUserRoles(user.Id, "Customer");

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task RemoveUserRolesWrongId()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };
            var role = new UserRole { UserId = user.Id, Role = "Customer" };
            await context.Users.AddAsync(user);
            await context.UserRoles.AddAsync(role);
            await context.SaveChangesAsync();

            var response = await userManager.RemoveUserRoles("id", "Customer");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("User is not found", response.Messages.First());
        }

        [Test]
        public async Task RemoveUserRolesNotHaveRole()
        {
            var user = new User { Email = "email@mail.com", Password = userManager.HashPassword("123456") };           
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var response = await userManager.RemoveUserRoles(user.Id, "Customer");

            Assert.IsTrue(response.Succeed);
        }

        [TearDown]
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
