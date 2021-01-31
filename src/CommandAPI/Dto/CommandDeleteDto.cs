using System.ComponentModel.DataAnnotations;

namespace CommandAPI.Dto
{
    public class CommandDeleteDto
    {
        [Required]
        public int Id { get; set; }
    }
}