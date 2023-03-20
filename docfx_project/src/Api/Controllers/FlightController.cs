using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Constants;
using Application.Flights.Commands.Add;
using Application.Flights.Commands.UpdateStatus;
using Application.Flights.Queries.Get;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
    /// <summary>
    /// Контроллер рейсов
    /// </summary>
    public class FlightController : ApiController
    {
        /// <inheritdoc />
        public FlightController(IMediator mediator)
            : base(mediator) { }

        /// <summary>
        /// Получение рейсов
        /// </summary>
        /// <param name="request">Обязателен один из фильтров:<br/> "origin"<br/> "destination"</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetFlightsQueryResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ValidationProblemDetails))]
        [Authorize(Roles = $"{AuthConstants.UserRoleName},{AuthConstants.ModeratorRoleName}")]
        public async Task<IActionResult> Get([FromBody] GetFlightsQuery request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Добавление рейса
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ValidationProblemDetails))]
        [Authorize(Roles = AuthConstants.ModeratorRoleName)]
        public async Task<IActionResult> Add([FromBody] AddFlightCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        /// <summary>
        /// Изменение статуса рейса
        /// </summary>
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ValidationProblemDetails))]
        [Authorize(Roles = AuthConstants.ModeratorRoleName)]
        public async Task<IActionResult> ChangeStatus([FromBody] UpdateFlightStatusCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}
