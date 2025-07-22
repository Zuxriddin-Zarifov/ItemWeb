using Anjir.Zuhriddin.Items.DataAccess;
using Anjir.Zuhriddin.Items.Services;
using Microsoft.EntityFrameworkCore;
using System;

namespace Anjir.Zuhriddin.Items.Web;

public static class Extension
{
    public static void ConfigureDataContext(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<MainContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Anjir.Zuhriddin.Items.DataAccess") 
            )
        );
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IItemService, ItemService>();
    }
}
