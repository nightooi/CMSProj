using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentDatabase.DIExtensions
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddContentContext(this IServiceCollection collection, string connectionString)
        {
            collection.AddDbContext<ContentContext>(opts => opts.UseSqlServer(connectionString));
            return collection;
        }
    }
}
