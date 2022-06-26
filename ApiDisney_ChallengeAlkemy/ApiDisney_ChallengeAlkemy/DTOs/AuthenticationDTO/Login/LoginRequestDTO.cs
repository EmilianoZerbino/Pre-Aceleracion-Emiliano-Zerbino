using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.DTOs.AuthenticationDTO.Login
{
    public class LoginRequestDTO
    {
        [Required]
        [MinLength(6)]
        public String UserName { get; set; }

        [Required]
        [MinLength(6)]
        public String Password { get; set; }
    }
}
