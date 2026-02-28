namespace ControlTicket.Application.Common.Messaging;

public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();