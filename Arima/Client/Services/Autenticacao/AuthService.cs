using Blazored.LocalStorage;
using Arima.Shared;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text;
using Arima.Client.Providers;
using System.Net.Http.Headers;
using Arima.Domain.Entidades.Autenticacao;
using Arima.Utilitarios;

namespace Arima.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        public AuthService(HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<DefaultResponse> Login(LoginModel loginModel)
        {
            var loginJson = loginModel.SerializeToJson();
            var response = await _httpClient.PostAsync("api/login", new StringContent(loginJson, Encoding.UTF8, "application/json"));
            var defaultResponse = (await response.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
            if (!response.IsSuccessStatusCode)
                return defaultResponse;

            await _localStorage.SetItemAsync("authToken", defaultResponse.Mensagens.FirstOrDefault());
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAuthenticated(loginModel.Email);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", defaultResponse.Mensagens.FirstOrDefault());

            return defaultResponse;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggerOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<DefaultResponse> Register(Usuario registerModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/account", registerModel);
            return (await result.Content.ReadAsStringAsync()).DeserializeJsonToObject<DefaultResponse>();
        }

        public async Task<string> GetToken()
        {
            return await _localStorage.GetItemAsStringAsync("authToken");
        }

        public async Task<bool> IsAuhenticated()
        {
            var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return state.User.Identity.IsAuthenticated;
        }
    }
}
