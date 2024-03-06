using FICCI_API.Interface;
using FICCI_API.Models.Services;
using FICCI_API.ModelsEF;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace FICCI_API.Extension
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
           IConfiguration config)
        {
            services.AddDbContext<FICCI_DB_APPLICATIONSContext>(option =>
                option.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );

            services.AddControllers()
                    .AddJsonOptions(options =>
           options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
            });
            services.AddHttpContextAccessor();
            // services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddCors();
            //services.AddScoped<ITokenService, TokenService>();
            //services.AddSingleton<EmployeeDataUpdateService>();

            return services;
        }
    }
}
