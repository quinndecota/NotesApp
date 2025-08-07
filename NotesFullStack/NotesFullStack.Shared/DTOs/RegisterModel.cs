using System.ComponentModel.DataAnnotations;

namespace NotesFullStack.Shared.DTOs
{
    public class RegisterModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
