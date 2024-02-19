using App.Application.Common.Interfaces;
using MediatR;

namespace App.Application.Accounts.Commands
{
    public class RequestPasswordChoangeHandler : IRequestHandler<RequestPasswordChangeCommand, bool>
    {
        private readonly IIdentityService _identityService;
        public RequestPasswordChoangeHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<bool> Handle(RequestPasswordChangeCommand request, CancellationToken cancellationToken)
        {
            var result = await _identityService.RequestChangePasswordAsync(request.Email, request.ClientUrl, cancellationToken);
            return result;
        }
    }

    public class RequestPasswordChangeCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string ClientUrl { get; set; }
    }
}
