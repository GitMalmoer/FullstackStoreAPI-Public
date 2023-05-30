using Microsoft.AspNetCore.Identity;

namespace FullstackStoreAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
