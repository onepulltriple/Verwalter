namespace Application.Places
{
    public interface IPlaceService
    {
        Task<IReadOnlyList<PlaceDto>> GetAllAsync();

        Task<PlaceDto?> GetByIdAsync(Guid placeId);

        Task<PlaceDto> CreateAsync(PlaceDto place);

        Task<bool> UpdateAsync(PlaceDto place);

        Task<bool> DeleteAsync(Guid placeId);
    }
}