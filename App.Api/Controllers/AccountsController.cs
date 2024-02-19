using App.Application.Accounts.Commands;
using App.Application.Common.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [AllowAnonymous]
    public class AccountsController : BaseController
    {
        [Produces("application/json")]
        [ProducesResponseType(typeof(LoginAccountResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginAccountCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [HttpPost("request-change-password")]
        public async Task<IActionResult> RequestPasswordChange([FromBody] RequestPasswordChangeCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException)
            {
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Produces("application/json")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(404)]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var response = await Mediator.Send(command, cancellationToken);
                return Ok(response);
            }
            catch (NotFoundException ex)
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
