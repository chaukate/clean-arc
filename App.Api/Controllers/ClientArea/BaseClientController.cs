using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.Api.Controllers.ClientArea
{
    [Route("client-portal/[controller]")]
    [ApiController]
    public class BaseClientController : BaseController
    {
        protected string CurrentUserId => HttpContext.User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.NameIdentifier)?.Value;
        protected string CurrentUserName => HttpContext.User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Name)?.Value ?? "SA";
        protected string CurrentUserEmail => HttpContext.User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email)?.Value;
        protected string Role => HttpContext.User?.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Role)?.Value;
    }
}
