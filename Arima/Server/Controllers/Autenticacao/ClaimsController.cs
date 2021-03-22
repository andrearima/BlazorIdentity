using Arima.Domain.Entidades.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arima.Server.Controllers.Autenticacao
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ClaimsController(
            UserManager<Usuario> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] string roleName)
        //{
        //    _roleManager.
        //    var result = await _userManager.CreateAsync(model, model.PasswordHash);
        //    _userManager.cla
        //    if (!result.Succeeded)
        //    {
        //        var errors = result.Errors.Select(x => x.Description).ToList();

        //        return Ok(new DefaultResponse() { Success = false, Mensagens = errors });
        //    }

        //    return Ok(new DefaultResponse() { Success = false, Mensagens = new List<string> { "Cadastro realizado com sucesso!" } });
        //}
    }
}
