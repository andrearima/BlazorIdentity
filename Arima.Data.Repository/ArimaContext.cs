using Arima.Domain.Entidades.Autenticacao;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Arima.Data.Repository
{
    public class ArimaContext : IdentityDbContext<Usuario>
    {
        private readonly IConfiguration _configuration;
        public ArimaContext(IConfiguration Configuration, DbContextOptions options) : base(options)
        {
            _configuration = Configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //varre tudo que implementa IEntityTypeConfiguration
            builder.ApplyConfigurationsFromAssembly(typeof(ArimaContext).Assembly);
        }
    }
}
