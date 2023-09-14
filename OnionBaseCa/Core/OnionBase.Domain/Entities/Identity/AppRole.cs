using Microsoft.AspNetCore.Identity;

namespace OnionBase.Domain.Entities.Identity
{
    public class AppRole : IdentityRole
    {
        public AppRole() : base()
        {
        }

        public AppRole(string roleName) : base(roleName)
        {
        }

    }
}
