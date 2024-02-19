using App.Application.AdminArea.Users.Queries;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers.AdminArea
{
    public class UsersController : BaseAdminController
    {
        [Produces("application/json")]
        [ProducesResponseType(typeof(List<ListUsersResponse>), 200)]
        [HttpGet]
        public async Task<IActionResult> List(CancellationToken cancellationToken)
        {
            var query = new ListUsersQuery();
            var response = await Mediator.Send(query, cancellationToken);
            return Ok(response);
        }
    }
}
