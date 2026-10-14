using Application.Common.Repositories;
using Application.Places.Models;

namespace Application.Places.Repositories
{
    public interface IPlaceRepository : IBaseRepository<PlaceDto>
    {
        Task<List<PlaceDto>> GetByPostcodeIdAsync(
            Guid postcodeId);

        Task<List<PlaceDto>> GetByCityIdAsync(
            Guid cityId);

        Task<List<PlaceDto>> GetByPostcodeAndCityAsync(
            Guid postcodeId,
            Guid cityId);
    }
}
