using RandomCappuccino.Server.Services.TourManager.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RandomCappuccino.Server.Services.TourManager
{
    public interface ITourManager
    {
        Task<ServiceContentResponse<ExtendedTourDTO>> CreateTour(CreateTourDTO model);

        Task<ServiceContentResponse<IEnumerable<TourDTO>>> GetGroupTours(string groupId);

        Task<ServiceContentResponse<ExtendedTourDTO>> GetTour(string tourId);

        Task<ServiceResponse> DeleteTour(string tourId);
    }
}
