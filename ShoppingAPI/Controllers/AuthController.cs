using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.API.DTO;
using Shopping.API.Security;
using Shopping.DAL;
using Shopping.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

// api/login
namespace Shopping.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController(ShoppingContext context, ITokenManager tokenManager) : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginFormDTO dto)
        {
            // Se connecter à la db 
            // chercher l'utilsateur dont le usernmae correspond
            User? user = context.Users.SingleOrDefault(u => u.Username == dto.Username);

            // si user n'est pas trouvé
            if(user is null)
            {
                // 400 // 401
                return BadRequest();
                // return Unauthorized();
            }
            // si le user est trouvé
            // vérifier son mot de passe
            // si pas bon
            if(Encoding.UTF8.GetString(SHA512.HashData(Encoding.UTF8.GetBytes(dto.Password + user.Salt))) != user.Password)
            {
                // 400 // 401
                return BadRequest();
            }
            // si c'est bon
            // créer un token et le renvoyer
            return Ok(new
            {
                Token = tokenManager.CreateToken(user.Id, user.Email, user.Role)
            });
        }

        [HttpGet("refreshToken")]
        public IActionResult RefreshToken([FromQuery]string token)
        {
            try
            {
                int id = tokenManager.ValidateTokenWithoutLifeTime(token);
                User? user = context.Users.Find(id);
                if (user is null)
                {
                    return Unauthorized();
                }
                string newToken = tokenManager.CreateToken(user.Id, user.Email, user.Role);
                return Ok(new { Token = newToken });
            }
            catch (Exception) {
                return Unauthorized();
            }
            
        }
    }
}
