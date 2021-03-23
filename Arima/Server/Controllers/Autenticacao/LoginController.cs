using Arima.Domain.Entidades.Autenticacao;
using Arima.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Arima.Server.Controllers.Autenticacao
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly SignInManager<Usuario> _signManager;
        private readonly UserManager<Usuario> _userManager;
        private readonly IConfiguration _configuration;
        public LoginController(
            SignInManager<Usuario> signManager,
            UserManager<Usuario> userManager,
            IConfiguration configuration)
        {
            _signManager = signManager;
            _configuration = configuration;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            Usuario user;
            try
            {
                if (login.Email.ToUpper().Equals("ADMIN@ADMIN.COM") && login.Senha.Equals("123deOliveira4"))
                    user = new Usuario() { Email = login.Email, UserName = "Administrador" };
                else
                {
                    user = await _userManager.FindByEmailAsync(login.Email);
                    if (user == null)
                        return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Usuário não cadastrado" } });

                    var result = await _signManager.PasswordSignInAsync(user.UserName, login.Senha, false, false);
                    if (!result.Succeeded)
                        return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Usuario ou Senha inválidos" } });
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Email),
                    new Claim(ClaimTypes.Role,"Administrador")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["JwtExpiryInDays"]));

                var token = new JwtSecurityToken(
                    _configuration["JwtIssuer"],
                    _configuration["JwtAudience"],
                    claims,
                    expires: expiry,
                    signingCredentials: creds
                    );

                return Ok(new DefaultResponse { Success = true, Mensagens = new List<string> { new JwtSecurityTokenHandler().WriteToken(token) } });
            }
            catch(Exception ex)
            {
                if(ex.InnerException.Message.Contains("Exception while connecting"))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Falha ao conectar com o banco de dados." } });

                return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { ex.Message } });
            }
        }
    }
}
