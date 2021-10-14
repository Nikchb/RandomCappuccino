using AutoMapper;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using RandomCappuccino.Server.Services.TourManager;
using RandomCappuccino.Server.Services.TourManager.DTOs;
using RandomCappuccino.Shared;
using System.Linq;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.RPC
{
    public class TourServiceProvider : TourService.TourServiceBase
    {
        private readonly IMapper mapper;

        private readonly ITourManager tourManager;

        public TourServiceProvider(IMapper mapper, ITourManager tourManager)
        {
            this.mapper = mapper;
            this.tourManager = tourManager;
        }

        public async override Task<TourResponse> CreateTour(CreateTourRequest request, ServerCallContext context)
        {
            var managerResponse = await tourManager.CreateTour(mapper.Map<CreateTourDTO>(request));

            var response = new TourResponse();
            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Tour = mapper.Map<ExtendedTourInfo>(managerResponse.Content);
            }

            return response;
        }

        public async override Task<TourResponse> GetTour(GetTourRequest request, ServerCallContext context)
        {
            var managerResponse = await tourManager.GetTour(request.Id);

            var response = new TourResponse();
            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Tour = mapper.Map<ExtendedTourInfo>(managerResponse.Content);
            }

            return response;
        }

        public async override Task<ToursResponse> GetTours(GetToursRequest request, ServerCallContext context)
        {
            var managerResponse = await tourManager.GetGroupTours(request.GroupId);
            var response = new ToursResponse();
            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);
            if (managerResponse.Content != null)
            {
                response.Tours.AddRange(managerResponse.Content.Select(v=>mapper.Map<TourInfo>(v)));
            }

            return response;
        }

        public async override Task<TourServiceResponse> DeleteTour(DeleteTourRequest request, ServerCallContext context)
        {
            var managerResponse = await tourManager.DeleteTour(request.Id);

            var response = new TourServiceResponse();
            response.Succeed = managerResponse.Succeed;
            response.Messages.AddRange(managerResponse.Messages);           

            return response;
        }
    }
}
