using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Identity;

namespace CityApi.Models
{
    public class UserAuth : IdentityUser<long>
    {
        public string Role { get; set; }
    }
}
