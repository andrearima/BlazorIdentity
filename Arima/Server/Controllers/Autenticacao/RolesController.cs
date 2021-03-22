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
    public class RolesController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesController(
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] IdentityRole role)
        {
            try
            {
                var result = await _roleManager.CreateAsync(role);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description).ToList();

                    return BadRequest(new DefaultResponse() { Success = false, Mensagens = errors });
                }

                return Ok(new DefaultResponse() { Success = true, Mensagens = new List<string> { $"Cadastro da role '{role.Name}' com sucesso!" } });
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Exception while connecting"))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Falha ao conectar com o banco de dados." } });

                return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { ex.Message } });
            }
        }
        [HttpGet]
        public IActionResult Get(string nome, string roleId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(roleId))
                    return Ok(_roleManager.Roles.Where(x => x.Id == roleId).ToList());

                if (string.IsNullOrWhiteSpace(nome))
                    return Ok(_roleManager.Roles.ToList());

                return Ok(_roleManager.Roles.Where(x => x.NormalizedName.Contains(nome.ToUpper())).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Exception while connecting"))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Falha ao conectar com o banco de dados." } });

                return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { ex.Message } });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string roleId)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(roleId))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "É necessário informar o Id da Role." } });

                var role = _roleManager.Roles.Where(x => x.Id.Equals(roleId)).FirstOrDefault();
                if(role == null)
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Role não existe." } });

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description).ToList();

                    return BadRequest(new DefaultResponse() { Success = false, Mensagens = errors });
                }

                return Ok(new DefaultResponse() { Success = true, Mensagens = new List<string> { $"Role '{role.Name}' excluída com sucesso!" } });
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Exception while connecting"))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Falha ao conectar com o banco de dados." } });

                return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { ex.Message } });
            }
        }
    }
}
