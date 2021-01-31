using System.ComponentModel.DataAnnotations;

namespace CommandAPI.Dto
{
    public class CommandCreateDto
    {
        [Required]
        [MaxLength(250)]
        public string HowTo { get; set; }
        [Required]
        public string Platform { get; set; }
        [Required]
        public string CLI { get; set; }
    }
}