using App.Application.Common.Interfaces;
using App.Domain.Enumerations;
using MediatR;

namespace App.Application.Accounts.Commands
{
    public class LoginAccountHandler : IRequestHandler<LoginAccountCommand, LoginAccountResponse>
    {
        private readonly IIdentityService _identityService;
        public LoginAccountHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginAccountResponse> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
        {
            var authResult = await _identityService.AuthenticateAsync(request.Email, request.Password, cancellationToken);

            var response = new LoginAccountResponse
            {
                FullName = authResult.FullName,
                Token = authResult.Token,
                Area = authResult.Area
            };

            return response;
        }
    }

    public class LoginAccountCommand : IRequest<LoginAccountResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginAccountResponse
    {
        public string FullName { get; set; }
        public string Token { get; set; }
        public Area Area { get; set; }
    }
}
