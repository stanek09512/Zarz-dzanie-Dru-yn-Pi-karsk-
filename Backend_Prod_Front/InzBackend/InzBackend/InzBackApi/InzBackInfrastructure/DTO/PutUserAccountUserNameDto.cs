using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InzBackInfrastructure.DTO
{
    public class PutUserAccountUserNameDto
    {
        [Required]  // wymagane
        public string Username { get; set; }
       
    }
}
