using Arima.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arima.Client.Services
{
    public interface IRoleService
    {
        Task<DefaultResponse> Cadastrar(string roleName);
        Task<DefaultResponse> GetRoles(string roleId);
        Task<DefaultResponse> DeleteRole(string roleId);
    }
}
