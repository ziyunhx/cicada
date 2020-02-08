﻿using Cicada.EFCore.Shared.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Cicada.EFCore.Mysql.Extensions
{
    public static class DatabaseExtensions
    {
        /// <summary>
        /// register mysql ef context from config.
        /// </summary>
        /// <typeparam name="TIdentityDbContext"></typeparam>
        /// <typeparam name="TCicadaContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="cicadaConnectionString"></param>
        /// <param name="identityConnectionString"></param>
        public static void RegisterMySqlDbContexts<TIdentityDbContext, TCicadaContext>(this IServiceCollection services, string cicadaConnectionString, string identityConnectionString)
            where TIdentityDbContext : DbContext
            where TCicadaContext : CicadaDbContext
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo().Assembly.GetName().Name;

            // Config DB for identity
            services.AddDbContext<CicadaIdentityDbContext>(options =>
                options.UseMySql(identityConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            services.AddDbContext<CicadaDbContext>(options =>
                options.UseMySql(cicadaConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}
