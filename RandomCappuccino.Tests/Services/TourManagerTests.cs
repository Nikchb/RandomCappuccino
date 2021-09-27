
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.IdentityManager;
using RandomCappuccino.Server.Services.ParticipantManager;
using RandomCappuccino.Server.Services.TourManager;
using RandomCappuccino.Server.Services.TourManager.DTOs;
using RandomCappuccino.Tests.InterfaceImplementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomCappuccino.Tests.Services
{
    public class TourManagerTests : TestBase
    {
        private IIdentityManager identityManager;        

        private ITourManager tourManager;       

        private string groupId;

        private string tourId;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            var user = new User { Email = "email@mail.com", Password = "123456" };
            var group = new Group { Name = "Group 1", UserId = user.Id };
            var participant1 = new Participant { Name = "Participant 1", GroupId = group.Id };
            var participant2 = new Participant { Name = "Participant 2", GroupId = group.Id };
            var participant3 = new Participant { Name = "Participant 3", GroupId = group.Id };
            var participant4 = new Participant { Name = "Participant 4", GroupId = group.Id };
            var tour = new Tour { GroupId = group.Id };
            var pair1 = new TourPair { TourId = tour.Id, Participant1Id = participant1.Id, Participant2Id = participant2.Id };
            var pair2 = new TourPair { TourId = tour.Id, Participant1Id = participant3.Id, Participant2Id = participant4.Id };

            context.Users.Add(user);
            context.Groups.Add(group);
            context.Participants.AddRange(participant1, participant2, participant3, participant4);            
            context.Tours.Add(tour);            
            context.TourPairs.AddRange(pair1, pair2);
            context.SaveChanges();

            groupId = group.Id;
            tourId = tour.Id;
            identityManager = new TestIdentityManager(user.Id);
            var groupManager = new GroupManager(mapper, context, identityManager);            
            tourManager = new TourManager(context, mapper, groupManager);
        }

        [Test]
        public async Task GetGroupTours()
        {
            var response = await tourManager.GetGroupTours(groupId);

            Assert.IsTrue(response.Succeed);
            Assert.AreEqual(1, response.Content.Count());
            Assert.AreEqual(tourId, response.Content.FirstOrDefault().Id);            
        }

        [Test]
        public async Task GetGroupToursWrongGroupId()
        {
            var response = await tourManager.GetGroupTours("id");

            Assert.IsFalse(response.Succeed);            
            Assert.AreEqual("Group is not found", response.Messages.First());
        }

        [Test]
        public async Task GetGroupToursAccessForbiden()
        {
            var groupMananger = new GroupManager(mapper, context, new TestIdentityManager("id"));
            var tourManager = new TourManager(context, mapper, groupMananger);

            var response = await tourManager.GetGroupTours(groupId);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.First());
        }

        [Test]
        public async Task GetTour()
        {
            var response = await tourManager.GetTour(tourId);

            Assert.IsTrue(response.Succeed);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual(tourId, response.Content.Id);
            foreach (var pair in response.Content.Pairs)
            {
                Console.WriteLine($"{pair.Participant1} - {pair.Participant2}");
            }
        }

        [Test]
        public async Task GetTourWithRemovedParticipant()
        {
            var participant = await context.Participants.FirstOrDefaultAsync(v => v.Name == "Participant 1");
            participant.IsActive = false;
            await context.SaveChangesAsync();

            var response = await tourManager.GetTour(tourId);

            Assert.IsTrue(response.Succeed);
            Assert.IsNotNull(response.Content);
            Assert.AreEqual(tourId, response.Content.Id);
            foreach (var pair in response.Content.Pairs)
            {
                Console.WriteLine($"{pair.Participant1} - {pair.Participant2}");
            }
        }

        [Test]
        public async Task GetTourWrongTourId()
        {
            var response = await tourManager.GetTour("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Tour is not found", response.Messages.First());
        }

        [Test]
        public async Task GetTourAccessForbiden()
        {
            var groupMananger = new GroupManager(mapper, context, new TestIdentityManager("id"));
            var tourManager = new TourManager(context, mapper, groupMananger);

            var response = await tourManager.GetTour(tourId);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.First());
        }

        [Test]
        public async Task DeleteTour()
        {
            var response = await tourManager.DeleteTour(tourId);

            Assert.IsTrue(response.Succeed);
        }

        [Test]
        public async Task DeleteTourWrongTourId()
        {
            var response = await tourManager.DeleteTour("id");

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Tour is not found", response.Messages.First());
        }

        [Test]
        public async Task DeleteTourAccessForbiden()
        {
            var groupMananger = new GroupManager(mapper, context, new TestIdentityManager("id"));
            var tourManager = new TourManager(context, mapper, groupMananger);

            var response = await tourManager.DeleteTour(tourId);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.First());
        }

        [Test]
        public async Task CreateTour()
        {
            var model = new CreateTourDTO { GroupId = groupId };

            var response = await tourManager.CreateTour(model);
            
            Assert.IsTrue(response.Succeed);
            Assert.IsNotNull(response.Content.Pairs);
            Assert.AreEqual(2, response.Content.Pairs.Length);

            foreach(var p in response.Content.Pairs)
            {
                Console.WriteLine($"{p.Participant1} - {p.Participant2}");
            }

            var pair = response.Content.Pairs.FirstOrDefault(v => v.Participant1 == "Participant 1" || v.Participant2 == "Participant 1");
            Assert.IsNotNull(pair);
            Assert.IsTrue(pair.Participant1 != "Participant 2" && pair.Participant2 != "Participant 2");
        }

        [Test]
        public async Task CreateTourFiveParticipants()
        {
            var participant5 = new Participant { Name = "Participant 5", GroupId = groupId };
            await context.AddAsync(participant5);
            await context.SaveChangesAsync();

            var model = new CreateTourDTO { GroupId = groupId };

            var response = await tourManager.CreateTour(model);

            Assert.IsTrue(response.Succeed);
            Assert.IsNotNull(response.Content.Pairs);
            Assert.AreEqual(4, response.Content.Pairs.Length);

            foreach (var p in response.Content.Pairs)
            {
                Console.WriteLine($"{p.Participant1} - {p.Participant2}");
            }

            var pair = response.Content.Pairs.FirstOrDefault(v => v.Participant1 == "Participant 1" || v.Participant2 == "Participant 1");
            Assert.IsNotNull(pair);
            Assert.IsTrue(pair.Participant1 != "Participant 2" && pair.Participant2 != "Participant 2");
        }

        [Test]
        public async Task CreateTourRemovedParticipant()
        {
            var participant5 = new Participant { Name = "Participant 5", GroupId = groupId, IsActive = false };
            await context.AddAsync(participant5);
            await context.SaveChangesAsync();

            var model = new CreateTourDTO { GroupId = groupId };

            var response = await tourManager.CreateTour(model);

            Assert.IsTrue(response.Succeed);
            Assert.IsNotNull(response.Content.Pairs);
            Assert.AreEqual(2, response.Content.Pairs.Length);

            foreach (var p in response.Content.Pairs)
            {
                Console.WriteLine($"{p.Participant1} - {p.Participant2}");
            }

            var pair = response.Content.Pairs.FirstOrDefault(v => v.Participant1 == "Participant 1" || v.Participant2 == "Participant 1");
            Assert.IsNotNull(pair);
            Assert.IsTrue(pair.Participant1 != "Participant 2" && pair.Participant2 != "Participant 2");
        }

        [Test]
        public async Task CreateTourWrongGroupId()
        {
            var model = new CreateTourDTO { GroupId = "id" };

            var response = await tourManager.CreateTour(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Group is not found", response.Messages.First());
        }

        [Test]
        public async Task CreateTourAccessForbiden()
        {
            var groupMananger = new GroupManager(mapper, context, new TestIdentityManager("id"));
            var tourManager = new TourManager(context, mapper, groupMananger);

            var model = new CreateTourDTO { GroupId = groupId };

            var response = await tourManager.CreateTour(model);

            Assert.IsFalse(response.Succeed);
            Assert.AreEqual("Access is forbiden", response.Messages.First());
        }

    }
}
