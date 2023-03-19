using System.Threading;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Flights.Commands.Add;
using Application.Flights.Commands.UpdateStatus;
using Application.Flights.Queries.Get;
using Domain.Entities.FlightAggregate;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Api.Controllers
{
    /// <summary>
    /// My api
    /// </summary>
    public class ExampleController : ApiController
    {
        /// <inheritdoc />
        public ExampleController(IMediator mediator)
            : base(mediator) { }

        /// <summary>
        /// Send and get message
        /// </summary>
        /// <param name="request">Message</param>
        /// <returns>Message</returns>
        /// <response code="200">Returns message</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromBody] GetFlightsQuery request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddFlightCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }

        [HttpPatch]
        public async Task<IActionResult> ChangeStatus([FromBody] UpdateFlightStatusCommand request, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(request, cancellationToken));
        }
    }
}
