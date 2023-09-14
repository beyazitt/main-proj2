using OnionBase.Domain.Entities.Identity;

namespace OnionBase.Presentation.ViewModels
{
    public class RoleManagmentViewModel
    {
        public List<AppUser> Users { get; set; }
        public List<AppRole> Roles { get; set; }
    }
}
