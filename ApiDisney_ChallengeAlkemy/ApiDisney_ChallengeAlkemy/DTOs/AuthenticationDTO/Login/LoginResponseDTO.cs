using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.DTOs.AuthenticationDTO.Login
{
    public class LoginResponseDTO
    {
        public String Token { get; set; }
        public DateTime ValidTo { get; set; }
    }
}
