using System.ComponentModel.DataAnnotations;

namespace TimeZoneManager.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
