using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace api.Models
{
    public class NewMedia
    {
        [Required]
        [MaxLength(3 * 1024 * 1024)]
        public IFormFile Data { get; set; }
    }
}