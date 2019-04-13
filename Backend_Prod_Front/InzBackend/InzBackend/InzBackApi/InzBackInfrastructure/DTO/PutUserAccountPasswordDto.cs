using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InzBackInfrastructure.DTO
{
    public class PutUserAccountPasswordDto
    {
        [Required]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "string must be beetwen 4 and 12 chars")] // haslo musi miec dlugosc miedzy 4 a 8
        public string Password1 { get; set; }
        [Required]
        [StringLength(12, MinimumLength = 4, ErrorMessage = "string must be beetwen 4 and 12 chars")] // haslo musi miec dlugosc miedzy 4 a 8
        public string Password2 { get; set; }
    }
}
