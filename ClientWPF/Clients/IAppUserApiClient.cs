using System.Threading.Tasks;
using AnwendungWPF.Models;

namespace AnwendungWPF.Clients
{
    public interface IAppUserApiClient
    {
        /// <summary>
        /// Attempts to log in. Returns the authenticated user model on success, otherwise null.
        /// </summary>
        Task<(bool Success, string? ErrorMessage, AppUserModel? User, string? JwtToken)> LoginAsync(string username, string password);
    }
}
