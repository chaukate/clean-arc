using App.Application.Common.Interfaces;
using MediatR;

namespace App.Application.Common.Behaviours
{
    public class EventDispatcherBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEventDispatcherService _eventDispatcherService;
        public EventDispatcherBehavior(IEventDispatcherService eventDispatcherService)
        {
            _eventDispatcherService = eventDispatcherService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            await _eventDispatcherService.Dispatch(cancellationToken);

            return response;
        }
    }
}
