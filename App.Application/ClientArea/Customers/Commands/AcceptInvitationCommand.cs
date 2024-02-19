using App.Application.Common.Interfaces;
using App.Domain.Entities;
using MediatR;

namespace App.Application.ClientArea.Customers.Commands
{
    public class AcceptInvitationHandler : IRequestHandler<AcceptInvitationCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public AcceptInvitationHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(AcceptInvitationCommand request, CancellationToken cancellationToken)
        {
            var userProfile = new UserProfile
            {
                Suffix = request.Suffix,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                DateOfBirth = request.DateOfBirth,
                IsActive = true,
                LastUpdatedAt = DateTimeOffset.UtcNow,
                LastUpdatedBy = request.Email
            };

            var result = await _identityService.AcceptCustomerInvitationAsync(userProfile, request.Email, request.Password, request.Token, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class AcceptInvitationCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Suffix { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Password { get; set; }
        public string ClientUrl { get; set; }
    }
}
