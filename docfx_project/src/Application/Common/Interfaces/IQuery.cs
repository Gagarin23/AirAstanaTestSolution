using MediatR;

namespace Application.Common.Interfaces;

/// <summary>
/// Контракт для запросов на чтение
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface IQuery<out TResponse> : IRequest<TResponse>
{
    
}
