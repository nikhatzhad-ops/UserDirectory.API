using System.ComponentModel.DataAnnotations;

namespace UserDirectory.Application.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
    }
}
