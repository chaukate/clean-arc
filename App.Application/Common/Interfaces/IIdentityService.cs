using App.Application.Common.Models;
using App.Domain.Entities;
using App.Domain.Interfaces;

namespace App.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthResponseModel> AuthenticateAsync(string email, string password, CancellationToken cancellationToken);
        Task<IUser> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<int> CreateCustomerAsync(string email, string currentUserEmail, string clientUrl, CancellationToken cancellationToken);
        Task<bool> AcceptCustomerInvitationAsync(UserProfile userProfile, string email, string password, string token, string clientUrl, CancellationToken cancellationToken);
        Task<string> GenerateEmailConfirmationTokenAsync(IUser user);
        Task<bool> RequestChangePasswordAsync(string email, string clientUrl, CancellationToken cancellationToken);
        Task<bool> ChangePasswordAsync(string email, string token, string password, string clientUrl, CancellationToken cancellationToken);
    }
}
