using Application.Common;

namespace Application.Postcodes
{
    public interface IPostcodeRepository : IBaseRepository<PostcodeDto>
    {
        Task<List<PostcodeDto>> GetByCountryIdAsync(
            Guid countryId);
    }
}
