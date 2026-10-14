using Application.Addresses.Models;
using Application.Addresses.Queries;
using Application.Common;
using Application.Common.Repositories;

namespace Application.Addresses.Repositories
{
    public interface IAddressRepository : IBaseRepository<AddressDto>
    {
        Task<PagedResult<AddressDto>> SearchAsync(
            AddressSearchCriteriaQuery criteria);

        Task<List<AddressDto>> GetByPlaceIdAsync(
            Guid placeId);
    }
}
