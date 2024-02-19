using App.Application.Common.Interfaces;
using MediatR;
using System.Text.Json.Serialization;

namespace App.Application.AdminArea.Customer.Commands
{
    public class InviteCustomerHandler : IRequestHandler<InviteCustomerCommand, int>
    {
        private readonly IIdentityService _identityService;
        public InviteCustomerHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(InviteCustomerCommand request, CancellationToken cancellationToken)
        {
            var customerId = await _identityService.CreateCustomerAsync(request.Email, request.CurrentUserEmail, request.ClientUrl, cancellationToken);
            return customerId;
        }
    }

    public class InviteCustomerCommand : IRequest<int>
    {
        public string Email { get; set; }
        public string ClientUrl { get; set; }
        [JsonIgnore]
        public string CurrentUserEmail { get; set; }
    }
}
