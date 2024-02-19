using App.Application.Common.Behaviours;
using App.Application.Common.Interfaces;
using App.Infrastructure.Common.Options;
using App.Infrastructure.Persistence;
using App.Infrastructure.Persistence.Identity;
using App.Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace App.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SECTION_NAME));

            services.AddDatabase(configuration);
            services.AddSecurity(configuration);

            services.AddScoped<IEventDispatcherService, EventDispatcherService>();
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(EventDispatcherBehavior<,>));
            services.AddScoped<IIdentityService, IdentityService>();
        }

        private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("App"));
            });
            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
        }

        private static void AddSecurity(this IServiceCollection services, IConfiguration configuration)
        {
            // For Identity
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configuration["JWT:Audience"],
                    ValidIssuer = configuration["JWT:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
