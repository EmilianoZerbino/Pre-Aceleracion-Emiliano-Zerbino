using ApiDisney_ChallengeAlkemy.DTOs.AuthenticationDTO.Login;
using ApiDisney_ChallengeAlkemy.DTOs.AuthenticationDTO.Register;
using ApiDisney_ChallengeAlkemy.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiDisney_ChallengeAlkemy.Controllers.AuthenticationController
{

    /*
    Punto 2. Autenticación de Usuarios

    Para realizar peticiones a los endpoints subsiguientes el usuario deberá contar con un token que obtendrá al autenticarse.
    Para ello, deberán desarrollarse los endpoints de registro y login, que permitan obtener el token.

    Los endpoints encargados de la autenticación deberán ser:

        ● /auth/login
        ● /auth/register
    */

    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<Usuario> userManager;
        private readonly SignInManager<Usuario> signInManager;

        public AuthenticationController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpPost]
        [Route("registro")]
        public async Task<IActionResult> Register(RegisterRequestDTO registerRequestDTO)
        {
            var userExist = await userManager.FindByNameAsync(registerRequestDTO.UserName);

            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            var usuario = new Usuario
            { 
                UserName = registerRequestDTO.UserName,
                Email = registerRequestDTO.Email,
                IsActive=true
            };

            var result = await userManager.CreateAsync(usuario, registerRequestDTO.Password);

            if (!result.Succeeded)
            { 
            return StatusCode(StatusCodes.Status500InternalServerError, new
                { 
                Status = "Error",
                Message = $"Error al crear Usuario Errores: {String.Join(", ",result.Errors.Select(x=> x.Description))}"
                });
            }

            enviarMail(usuario.Email, usuario.UserName); // Punto 11 Enviar mail de Bienvanida con SendGrid.

            return Ok(new
                 {
                     Status = "Succes",
                     Message = "El Usuario se creo Correctamente"
                 });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            var result = await signInManager.PasswordSignInAsync(loginRequestDTO.UserName, loginRequestDTO.Password, false, false);

            if (result.Succeeded)
            {
                var usuarioActual = await userManager.FindByNameAsync(loginRequestDTO.UserName);

                if (usuarioActual.IsActive)
                {
                    return Ok(await GetToken(usuarioActual));
                }
            }

            return StatusCode(StatusCodes.Status401Unauthorized,
                new
                {
                    Status = "Error",
                    Message = $"El Usuario {loginRequestDTO.UserName} no esta autorizado!"
                });
        }

        private async Task<LoginResponseDTO> GetToken(Usuario usuarioActual)
        {
            var userRoles = await userManager.GetRolesAsync(usuarioActual);

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, usuarioActual.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            authClaims.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KeySecretaSuperLargaDeAUTORIZACION"));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ValidTo = token.ValidTo
            };

        }

        private async void enviarMail(String mail, String nombre)
        {
            var client = new SendGridClient("Acac va el Código de la página web de SendGrid");

            var from = new EmailAddress("miMail@gmail.com", "Disney APP");
            var subject = "Bienvenido al Maravilloso mundo de Disney";
            var to = new EmailAddress(mail, nombre);
            var plainTextContent = "Hola " + nombre + " estamos muy contentos de que te sumes a nuestra aplicacion y " +
                                   "esperamos que disfrutes de todo el contenido que Disney App tiene para vos.";
            var htmlContent = "<strong>Hola " + nombre + " estamos muy contentos de que te sumes a nuestra aplicacion y " +
                                   "esperamos que disfrutes de todo el contenido que Disney App tiene para vos.</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

        }
    }
}
