using App.Domain.Enumerations;

namespace App.Application.Common.Models
{
    public class AuthResponseModel
    {
        public string FullName { get; set; }
        public string Token { get; set; }
        public Area Area { get; set; }
    }
}
