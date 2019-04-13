using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InzBackInfrastructure.DTO
{
    public class UserToRegister
    {
        [Required]  // wymagane
        public string Username { get; set; }
        [Required]
        [StringLength(8,MinimumLength =4,ErrorMessage ="string must be beetwen 4 and 8 chars")] // haslo musi miec dlugosc miedzy 4 a 8
        public string Password { get; set; }
    }
}
