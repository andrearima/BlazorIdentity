using Arima.Domain.Entidades.Autenticacao;
using Arima.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arima.Server.Controllers.Autenticacao
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        public AccountController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario model)
        {
            var result = await _userManager.CreateAsync(model, model.PasswordHash);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();

                return Ok(new DefaultResponse() { Success = false, Mensagens = errors });
            }

            return Ok(new DefaultResponse() { Success = false, Mensagens = new List<string> { "Cadastro realizado com sucesso!"} });
        }
    }
}
