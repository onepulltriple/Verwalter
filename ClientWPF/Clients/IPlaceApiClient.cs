using AnwendungWPF.Models;

namespace AnwendungWPF.Clients
{
    public interface IPlaceApiClient
    {
        Task<IReadOnlyList<PlaceModel>> GetAllAsync();

        Task<PlaceModel?> GetByIdAsync(Guid placeId);

        Task<PlaceModel> CreateAsync(
            Guid countryId,
            string city,
            string postcode,
            string? nameEnGb);

        Task UpdateAsync(
            Guid placeId,
            Guid countryId,
            string city,
            string postcode,
            string? nameEnGb);

        Task DeleteAsync(Guid placeId);
    }
}