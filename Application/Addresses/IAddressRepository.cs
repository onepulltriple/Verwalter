using Application.Common;

namespace Application.Addresses
{
    public interface IAddressRepository : IBaseRepository<AddressDto>
    {
        Task<PagedResult<AddressDto>> SearchAsync(
            AddressSearchCriteria criteria);

        Task<List<AddressDto>> GetByPlaceIdAsync(
            Guid placeId);
    }
}
