using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours
{
    public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;
        private readonly IUserContext _userContext;

        public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger, IUserContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошло необработанное исключение. Пользователь: {0}", _userContext.Username);
                throw;
            }
        }
    }
}
