using NUnit.Framework;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.IdentityManager;
using RandomCappuccino.Server.Services.ParticipantManager;
using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using RandomCappuccino.Tests.InterfaceImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Tests.Services
{
    public class ParticipantManagerTests : TestBase
    {
        private IIdentityManager identityManager;       

        private IParticipantManager participantManager;

        private string groupId;

        private string participantId;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = new User { Email = "email@mail.com", Password = "123456" };
            var group = new Group { Name = "Group 1", UserId = user.Id };
            var participant = new Participant { Name = "Participant 1", GroupId = group.Id };
            context.Users.Add(user);
            context.Groups.Add(group);
            context.Participants.Add(participant);
            context.SaveChanges();

            groupId = group.Id;
            participantId = participant.Id;
            identityManager = new TestIdentityManager(user.Id);
            var groupManager = new GroupManager(mapper, context, identityManager);
            participantManager = new ParticipantManager(mapper, context, groupManager);
        }

        [Test]
        public async Task CreateParticipant()
        {
            var model = new CreateParticipantDTO { GroupId = groupId, Name = "Participant" };

            var response = await participantManager.CreateParticipant(model);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(model.Name, response.Content.Name);
        }

        [Test]
        public async Task CreateParticipantWrongUserId()
        {
            var identityManager = new TestIdentityManager("id");
            var groupManager = new GroupManager(mapper, context, identityManager);
            var participantManager = new ParticipantManager(mapper, context, groupManager);

            var model = new CreateParticipantDTO { GroupId = groupId, Name = "Participant" };

            var response = await participantManager.CreateParticipant(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task UpdateParticipant()
        {
            var model = new ParticipantDTO { Id = participantId, Name = "Participant 2" };

            var response = await participantManager.UpdateParticipant(model);

            Assert.IsTrue(response.Succeed);            
        }

        [Test]
        public async Task UpdateParticipantWrongParticipantId()
        {
            var model = new ParticipantDTO { Id = "id", Name = "Participant 2" };

            var response = await participantManager.UpdateParticipant(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Participant is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task UpdateParticipantAccessForbiden()
        {
            var identityManager = new TestIdentityManager("id");
            var groupManager = new GroupManager(mapper, context, identityManager);
            var participantManager = new ParticipantManager(mapper, context, groupManager);

            var model = new ParticipantDTO { Id = participantId, Name = "Participant 2" };

            var response = await participantManager.UpdateParticipant(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task DeleteParticipant()
        {           
            var response = await participantManager.DeleteParticipant(participantId);

            Assert.IsTrue(response.Succeed);
        }

        [Test]
        public async Task DeleteParticipantWrongParticipantId()
        {
            var response = await participantManager.DeleteParticipant("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Participant is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task DeleteParticipantAccessForbiden()
        {
            var identityManager = new TestIdentityManager("id");
            var groupManager = new GroupManager(mapper, context, identityManager);
            var participantManager = new ParticipantManager(mapper, context, groupManager);            

            var response = await participantManager.DeleteParticipant(participantId);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task GetParticipant()
        {
            var response = await participantManager.GetParticipant(participantId);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(participantId, response.Content.Id);
        }

        [Test]
        public async Task GetParticipantWrongParticipantId()
        {
            var response = await participantManager.GetParticipant("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Participant is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task GetParticipantAccessForbiden()
        {
            var identityManager = new TestIdentityManager("id");
            var groupManager = new GroupManager(mapper, context, identityManager);
            var participantManager = new ParticipantManager(mapper, context, groupManager);

            var response = await participantManager.GetParticipant(participantId);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task GetGroupParticipants()
        {
            var response = await participantManager.GetGroupParticipants(groupId);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(1, response.Content.Count());
        }

        [Test]
        public async Task GetGroupParticipantsWrongGroupId()
        {
            var response = await participantManager.GetGroupParticipants("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group is not found", response.Messages.FirstOrDefault());
        }

        [Test]
        public async Task GetGroupParticipantsAccessForbiden()
        {
            var identityManager = new TestIdentityManager("id");
            var groupManager = new GroupManager(mapper, context, identityManager);
            var participantManager = new ParticipantManager(mapper, context, groupManager);

            var response = await participantManager.GetGroupParticipants(groupId);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.FirstOrDefault());
        }
    }
}
