using MediatR;

namespace Application.Common.Interfaces;

/// <summary>
/// Контракт для запросов на изменение данных
/// </summary>
/// <typeparam name="TResponse"></typeparam>
public interface ICommand<out TResponse> : IRequest<TResponse>
{
    
}
