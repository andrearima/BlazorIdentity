using Microsoft.AspNetCore.Identity;
using System;

namespace Arima.Domain.Entidades.Autenticacao
{
    public class Usuario : IdentityUser
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Celular { get; set; }
    }
}
