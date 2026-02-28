using ControlTicket.Application.Common.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace ControlTicket.Infrastructure.Messaging;

public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _sp;

    public Mediator(IServiceProvider sp) => _sp = sp;

    public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        // Handler: IRequestHandler<TRequest, TResponse>
        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);

        var handler = _sp.GetService(handlerType)
            ?? throw new InvalidOperationException(
                $"No handler registered for request '{requestType.Name}' -> '{responseType.Name}'.");

        // Base invoker: call handler.Handle((TRequest)request, ct)
        RequestHandlerDelegate<TResponse> next = () =>
        {
            var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle))
                ?? throw new InvalidOperationException($"Handler '{handlerType.Name}' has no Handle method.");

            var taskObj = method.Invoke(handler, new object[] { request, ct })
                ?? throw new InvalidOperationException("Handler invocation returned null.");

            return (Task<TResponse>)taskObj;
        };

        // Pipeline behaviors: IPipelineBehavior<TRequest, TResponse>
        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
        var behaviors = _sp.GetServices(behaviorType).Reverse().ToArray();

        foreach (var behavior in behaviors)
        {
            var current = next;
            next = () =>
            {
                var method = behaviorType.GetMethod(nameof(IPipelineBehavior<IRequest<TResponse>, TResponse>.Handle))
                    ?? throw new InvalidOperationException($"Behavior '{behaviorType.Name}' has no Handle method.");

                var taskObj = method.Invoke(behavior, new object[] { request, ct, current })
                    ?? throw new InvalidOperationException("Behavior invocation returned null.");

                return (Task<TResponse>)taskObj;
            };
        }

        return next();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
        where TNotification : INotification
    {
        if (notification is null) throw new ArgumentNullException(nameof(notification));

        var notifType = typeof(TNotification);
        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notifType);

        var handlers = _sp.GetServices(handlerType).ToArray();

        foreach (var h in handlers)
        {
            var method = handlerType.GetMethod(nameof(INotificationHandler<TNotification>.Handle))
                ?? throw new InvalidOperationException($"Notification handler '{handlerType.Name}' has no Handle method.");

            var taskObj = method.Invoke(h, new object[] { notification, ct })
                ?? throw new InvalidOperationException("Notification handler invocation returned null.");

            await (Task)taskObj;
        }
    }
}