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
        public static IServiceCollection AddSimpleSql(this IServiceCollection serviceCollection, Action<SimpleSqlOptions> setup)
        {
            var opt = new SimpleSqlOptions();
            setup(opt);
            return serviceCollection.AddScoped(p => opt.CreateConnection());
        }

    }

}
