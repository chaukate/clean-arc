using App.Application.AdminArea.Customer.Commands;
using App.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.AdminArea
{
    public class CustomersController : BaseAdminController
    {
        [Produces("application/json")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("invite")]
        public async Task<IActionResult> Invite([FromBody] InviteCustomerCommand command, CancellationToken cancellationToken)
        {
            try
            {
                command.CurrentUserEmail = CurrentUserEmail;
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
