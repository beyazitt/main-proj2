using OnionBase.Domain.Entities.Identity;

namespace OnionBase.Presentation.ViewModels
{
    public class SmsViewModel
    {
        public int code { get; set; }
        public AppUser appUser { get; set; }
    }
}
