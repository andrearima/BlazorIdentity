using Arima.Domain.Entidades.Autenticacao;
using Arima.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arima.Client.Services
{
    public interface IAuthService
    {
        Task<DefaultResponse> Login(LoginModel loginModel);
        Task Logout();
        Task<DefaultResponse> Register(Usuario registerModel);
        Task<string> GetToken();
        Task<bool> IsAuhenticated();
    }
}
