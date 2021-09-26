using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RandomCappuccino.Server.Data;
using RandomCappuccino.Server.Data.Models;
using RandomCappuccino.Server.Services.GroupManager;
using RandomCappuccino.Server.Services.ParticipantManager;
using RandomCappuccino.Server.Services.ParticipantManager.DTOs;
using RandomCappuccino.Server.Services.TourManager.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.TourManager
{
    public class TourManager : ServiceBase, ITourManager
    {
        private readonly DataBaseContext context;
        private readonly IMapper mapper;
        private readonly IGroupManager groupManager;
        private readonly IParticipantManager participantManager;

        private ParticipantDTO[] groupParticipants;
        private HashSet<Pair> allPairs;

        public TourManager(DataBaseContext context, IMapper mapper, IGroupManager groupManager, IParticipantManager participantManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.groupManager = groupManager;
            this.participantManager = participantManager;
        }

        private async Task<ServiceResponse> ComputeAllPairs(string groupId)
        {
            var response = await participantManager.GetGroupParticipants(groupId);
            if(response.Succeed == false)
            {
                return Decline<IEnumerable<Pair>>(response.Messages);
            }

            groupParticipants = response.Content.ToArray();     
            allPairs = new HashSet<Pair>();

            for(int i=0; i< groupParticipants.Length - 1; i++)
            {
                for(int j = i + 1; j < groupParticipants.Length; j++)
                {
                    allPairs.Add(new Pair { Participant1 = groupParticipants[i].Id, Participant2 = groupParticipants[j].Id });
                }
            }

            return Accept();
        }

        public async Task<ServiceContentResponse<ExtendedTourDTO>> CreateTour(CreateTourDTO model)
        {
            const int toursNumber = 10;

            var groupResponse = await groupManager.GetGroup(model.GroupId);
            if (groupResponse.Succeed == false)
            {
                return Decline<ExtendedTourDTO>(groupResponse.Messages);
            }            
            
            var tours = await context.Tours.Where(v => v.GroupId == model.GroupId)
                                            .OrderByDescending(v => v.CreationTime)
                                            .Take(toursNumber)
                                            .Include(v => v.Pairs)
                                            .ToListAsync();

            var allPairsRequest = await ComputeAllPairs(model.GroupId);
            if (allPairsRequest.Succeed == false)
            {
                return Decline<ExtendedTourDTO>(allPairsRequest.Messages);
            }

            List<Pair> pairs;

            do
            {
                var usedPairs = new HashSet<Pair>();
                foreach(var tour in tours)
                {
                    foreach(var pair in tour.Pairs.Select(v=>mapper.Map<Pair>(v)))
                    {
                        usedPairs.Add(pair);
                    }
                }

                var freePairs = allPairs.Where(v => usedPairs.Contains(v) == false).ToArray();

                pairs = new List<Pair>();
                for (int i = 0; i < freePairs.Length; i++)
                {
                    if (pairs.Any(v => v.HasCrossing(freePairs[i])) == false)
                    {
                        pairs.Add(freePairs[i]);
                    }
                }

                if (tours.Count > 0)
                {
                    tours.Remove(tours.Last());
                }
            } 
            while (pairs.Count < groupParticipants.Length / 2);

            if(groupParticipants.Length % 2 != 0)
            {
                var usedParticipants = new List<string>();
                usedParticipants.AddRange(pairs.Select(v => v.Participant1));
                usedParticipants.AddRange(pairs.Select(v => v.Participant2));
                var participant = groupParticipants.First(v => usedParticipants.Contains(v.Id) == false);
                var lastPair = pairs.Last();
                pairs.Add(new Pair { Participant1 = participant.Id, Participant2 = lastPair.Participant1 });
                pairs.Add(new Pair { Participant1 = participant.Id, Participant2 = lastPair.Participant2 });
            }

            try
            {
                var createdTour = new Tour { GroupId = model.GroupId };                
                var createdTourPairs = pairs.Select(v => new TourPair { TourId = createdTour.Id, Participant1Id = v.Participant1, Participant2Id = v.Participant2 });
                await context.AddAsync(createdTour);
                await context.AddRangeAsync(createdTourPairs);
                await context.SaveChangesAsync();

                var tourDTO = new ExtendedTourDTO
                {
                    Id = createdTour.Id,
                    CreationTime = createdTour.CreationTime,
                    Pairs = createdTourPairs.Select(v => mapper.Map<TourPairDTO>(v)).ToArray()
                };

                return Accept(tourDTO);
            }
            catch
            {
                return Decline<ExtendedTourDTO>("Tour creation failed");
            }                
        }       


        public async Task<ServiceResponse> DeleteTour(string tourId)
        {
            var tour = await context.Tours.FindAsync(tourId);
            if(tour == null)
            {
                return Decline("Tour is not found");
            }

            var groupResponse = await groupManager.GetGroup(tour.Id);
            if(groupResponse.Succeed == false)
            {
                return Decline(groupResponse.Messages);
            }

            try
            {
                var tourPairs = await context.TourPairs.Where(v => v.TourId == tour.Id).ToArrayAsync();
                context.RemoveRange(tourPairs);
                context.Remove(tour);
                await context.SaveChangesAsync();
            }
            catch
            {
                return Decline("Tour delete is failed");
            }
            return Accept();
        }

        public async Task<ServiceContentResponse<IEnumerable<TourDTO>>> GetGroupTours(string groupId)
        {
            var groupResponse = await groupManager.GetGroup(groupId);
            if (groupResponse.Succeed == false)
            {
                return Decline<IEnumerable<TourDTO>>(groupResponse.Messages);
            }

            IEnumerable<TourDTO> tours;

            try
            {
                tours = await context.Tours.Where(v => v.GroupId == groupId).Select(v => mapper.Map<TourDTO>(v)).ToArrayAsync();
            }
            catch
            {
                return Decline<IEnumerable<TourDTO>>("Tours request is failed");
            }
            return Accept(tours);
        }

        public async Task<ServiceContentResponse<ExtendedTourDTO>> GetTour(string tourId)
        {
            var tour = await context.Tours.FindAsync(tourId);
            if (tour == null)
            {
                return Decline<ExtendedTourDTO>("Tour is not found");
            }

            var groupResponse = await groupManager.GetGroup(tour.Id);
            if (groupResponse.Succeed == false)
            {
                return Decline<ExtendedTourDTO>(groupResponse.Messages);
            }

            var tourDTO = new ExtendedTourDTO
            {
                Id = tour.Id,
                CreationTime = tour.CreationTime,
                Pairs = await context.TourPairs.Where(v => v.TourId == tour.Id).Select(v => mapper.Map<TourPairDTO>(v)).ToArrayAsync()
            };

            return Accept(tourDTO);
        }
    }
}
