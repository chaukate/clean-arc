using App.Application.Common.Interfaces;
using MediatR;

namespace App.Infrastructure.Services
{
    public class EventDispatcherService : IEventDispatcherService
    {
        private readonly IMediator _mediator;
        private readonly List<INotification> _notificationQueue = new List<INotification>();
        public EventDispatcherService(IMediator mediatoer)
        {
            _mediator = mediatoer;
        }

        public void ClearQueue()
        {
            _notificationQueue.Clear();
        }

        public async Task Dispatch(CancellationToken cancellationToken)
        {
            while (_notificationQueue.Count > 0)
            {
                try
                {
                    await _mediator.Publish(_notificationQueue[0], cancellationToken);
                    //break;
                }
                catch (Exception e)
                {
                    // todo : let each notification define what to do on failure
                    Console.WriteLine(e);
                }
                finally
                {
                    _notificationQueue.RemoveAt(0);
                }
            }
        }

        public void QueueNotification(INotification notification)
        {
            _notificationQueue.Add(notification);
        }
    }
}
