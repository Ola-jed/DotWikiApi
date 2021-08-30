using System.ComponentModel.DataAnnotations;

namespace DotWikiApi.Dtos
{
    public record LoginDto
    {
        [Required(ErrorMessage = "User Name is required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}