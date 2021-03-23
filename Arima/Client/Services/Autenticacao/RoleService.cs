using Arima.Shared;
using Arima.Utilitarios;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;

namespace Arima.Client.Services
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _httpClient;
        public RoleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<DefaultResponse> Cadastrar(string roleName)
        {
            var role = new IdentityRole(roleName);
            var roleNameJson = role.SerializeToJson();

            var response = await _httpClient.PostAsync("api/roles", new StringContent(roleNameJson, Encoding.UTF8, "application/json")); 

            return (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
        }
        public async Task<DefaultResponse> GetRoles(string roleId)
        {
            var uri = "api/roles";
            if (!string.IsNullOrWhiteSpace(roleId))
                uri += $"?roleId={roleId}";

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var roles = (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<List<IdentityRole>>();
                return new DefaultResponse() { Success = true, Mensagens = new List<string> { roles.SerializeToJson() } };
            }

            return (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
        }

        public async Task<DefaultResponse> DeleteRole(string roleId)
        {
            if(string.IsNullOrWhiteSpace(roleId))
                return new DefaultResponse() { Success = false, Mensagens = new List<string> { "Necessário informa Id da Role" } };


            var response = await _httpClient.DeleteAsync($"/api/roles?roleId={roleId}");
            return (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
        }
    }
}
