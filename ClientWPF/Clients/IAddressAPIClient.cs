using AnwendungWPF.Models;

namespace AnwendungWPF.Clients
{
    public interface IAddressApiClient
    {
        Task<AddressModel?> GetByIdAsync(Guid addressId);

        Task<IReadOnlyList<AddressModel>> GetAllAsync();

        Task DeleteAsync(Guid id);
    }
}
