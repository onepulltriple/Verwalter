using Application.Places.Models;
using Application.Places.Repositories;

namespace Application.Places.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IPlaceRepository _placeRepository;

        public PlaceService(IPlaceRepository placeRepository)
        {
            _placeRepository = placeRepository;
        }

        public async Task<IReadOnlyList<PlaceDto>> GetAllAsync()
        {
            return await _placeRepository.GetAllAsync();
        }

        public async Task<PlaceDto?> GetByIdAsync(Guid placeId)
        {
            return await _placeRepository.GetByIdAsync(placeId);
        }

        public async Task<PlaceDto> CreateAsync(PlaceDto place)
        {
            await _placeRepository.AddAsync(place);

            return place;
        }

        public async Task<bool> UpdateAsync(PlaceDto place)
        {
            var existing = await _placeRepository.GetByIdAsync(
                place.PlaceId);

            if (existing is null)
            {
                return false;
            }

            await _placeRepository.UpdateAsync(place);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid placeId)
        {
            var existing = await _placeRepository.GetByIdAsync(placeId);

            if (existing is null)
            {
                return false;
            }

            await _placeRepository.DeleteAsync(placeId);

            return true;
        }
    }
}