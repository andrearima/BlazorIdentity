using Arima.Domain.Entidades.Autenticacao;
using Arima.Shared;
using Arima.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Arima.Client.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly HttpClient _httpClient;
        public UsuarioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<DefaultResponse> Cadastrar(Usuario usuario)
        {
            usuario.UserName = usuario.Cpf;
            var usuarioJson = usuario.SerializeToJson();

            var response = await _httpClient.PostAsync("api/usuario", new StringContent(usuarioJson, Encoding.UTF8, "application/json"));

            return (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
        }

        public async Task<DefaultResponse> DeleteUsuario(string usuarioId)
        {
            if (string.IsNullOrWhiteSpace(usuarioId))
                return new DefaultResponse() { Success = false, Mensagens = new List<string> { "Necessário informa Id do Usuario" } };


            var response = await _httpClient.DeleteAsync($"/api/usuario?roleId={usuarioId}");
            return (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
        }

        public async Task<DefaultResponse> GetUsuarios(string usuarioId)
        {
            var uri = "api/usuario";
            if (!string.IsNullOrWhiteSpace(usuarioId))
                uri += $"?usuarioId={usuarioId}";

            var response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var usuarios = (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<List<Usuario>>();
                return new DefaultResponse() { Success = true, Mensagens = new List<string> { usuarios.SerializeToJson() } };
            }

            return (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
        }
    }
}
