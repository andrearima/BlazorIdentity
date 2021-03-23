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
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<Usuario> _userManager;
        public UsuarioController(UserManager<Usuario> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Get(string usuarioId)
        {
            try
            {
                List<Usuario> usuarios = new List<Usuario>();
                if (string.IsNullOrWhiteSpace(usuarioId))
                    return Ok(_userManager.Users.ToList());
                else
                    return Ok(_userManager.Users.Where(x => x.Id.Equals(usuarioId)).ToList());
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Exception while connecting"))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Falha ao conectar com o banco de dados." } });

                return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { ex.Message } });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            try
            {
                var result = await _userManager.CreateAsync(usuario);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description).ToList();

                    return BadRequest(new DefaultResponse() { Success = false, Mensagens = errors });
                }

                return Ok(new DefaultResponse() { Success = true, Mensagens = new List<string> { $"Cadastro da role '{usuario.Nome}' com sucesso!" } });
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.Contains("Exception while connecting"))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Falha ao conectar com o banco de dados." } });

                return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { ex.Message } });
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string usuarioId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(usuarioId))
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "É necessário informar o Id do Usuário." } });

                var usuario = _userManager.Users.Where(x => x.Id.Equals(usuarioId)).ToList().FirstOrDefault();
                if (usuario == null)
                    return BadRequest(new DefaultResponse { Success = false, Mensagens = new List<string> { "Usuario não existe." } });

                var result = await _userManager.DeleteAsync(usuario);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(x => x.Description).ToList();

                    return BadRequest(new DefaultResponse() { Success = false, Mensagens = errors });
                }

                return Ok(new DefaultResponse() { Success = true, Mensagens = new List<string> { $"Usuario '{usuario.Nome}' excluída com sucesso!" } });
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
