using App.Application.ClientArea.Customers.Commands;
using App.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.ClientArea
{
    public class CustomersController : BaseClientController
    {
        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPost("accept-invitation")]
        public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
