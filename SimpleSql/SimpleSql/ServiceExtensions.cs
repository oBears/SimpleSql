using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using MySql.Data.MySqlClient;

namespace SimpleSql
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddSimpleSql(this IServiceCollection serviceCollection, Func<SimpleSqlOptions,IDbConnection> setup)
        {
            return serviceCollection.AddScoped(p => setup(new SimpleSqlOptions()));
        }

    }

}
