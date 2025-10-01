using CounterApi.Data;
using CounterApi.DataAccess.Repos;
using Microsoft.EntityFrameworkCore;

namespace CounterApi.DataAccess
{
    public static class CounterStorageRegistration
    {
        public static IServiceCollection AddCounterStorage(this IServiceCollection services, string connstring)
        {
            services.AddScoped<ICounterRepository, CounterRepository>();
            services.AddScoped<ICounterManager, CounterManager>();
            services.AddDbContext<CounterContext>(o =>
            {
                o.UseSqlServer(connstring);
            });
            return services;
        }
    }
}
