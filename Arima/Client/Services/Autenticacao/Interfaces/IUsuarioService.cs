using Arima.Domain.Entidades.Autenticacao;
using Arima.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arima.Client.Services
{
    public interface IUsuarioService
    {
        Task<DefaultResponse> Cadastrar(Usuario usuario);
        Task<DefaultResponse> GetUsuarios(string usuarioId);
        Task<DefaultResponse> DeleteUsuario(string usuarioId);
    }
}
