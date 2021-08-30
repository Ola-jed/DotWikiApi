using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DotWikiApi.Models;

namespace DotWikiApi.Dtos
{
    public record UserReadDto
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }

        public ICollection<Article> Articles { get; set; }
    }
}