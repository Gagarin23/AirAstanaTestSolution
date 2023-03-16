using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetOkMessage()
        {
            return Ok();
        }
    }
}
