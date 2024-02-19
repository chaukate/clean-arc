using App.Application.Common.Events;
using App.Application.Common.Interfaces;
using App.Domain.Enumerations;
using App.Domain.Interfaces;
using MediatR;

namespace App.Application.Events
{
    public class UserEventHandler : INotificationHandler<CreatedEvent>, INotificationHandler<UpdatedEvent>
    {
        private readonly IIdentityService _identityService;
        public UserEventHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Handle(CreatedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.GetEntity<IUser>();
            if (user != null)
            {
                if (user.EventActivity == UserEventActivity.Invite)
                {
                    var token = await _identityService.GenerateEmailConfirmationTokenAsync(user);
                    user.Link = $"{user.Link}/client-area/accept-invitation?email={user.Email}&token={token}";
                }
            }
        }

        public async Task Handle(UpdatedEvent notification, CancellationToken cancellationToken)
        {
            var user = notification.GetEntity<IUser>();
            if (user != null)
            {

            }
        }
    }
}
