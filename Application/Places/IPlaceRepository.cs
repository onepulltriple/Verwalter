using Application.Common;

namespace Application.Places
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
