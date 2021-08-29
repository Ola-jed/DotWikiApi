using System.ComponentModel.DataAnnotations;

namespace DotWikiApi.Dtos
{
    public class UserReadDto
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
    }
}