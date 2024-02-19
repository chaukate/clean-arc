using App.Application.Common.Interfaces;
using MediatR;

namespace App.Application.Accounts.Commands
{
    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public ChangePasswordHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.ChangePasswordAsync(request.Email, request.Token, request.Password, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class ChangePasswordCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string ClientUrl { get; set; }
    }
}
