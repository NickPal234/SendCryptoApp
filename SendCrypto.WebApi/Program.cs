
using AutoMapper;
using SendCrypto.Application.Integration;
using SendCrypto.Application.Services.Implementation;
using SendCrypto.Application.Services.Interfaces;
using SendCrypto.WebApi.Mapper;

namespace SendCrypto.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.ConfigureRandomGeneratorWebClient();

            builder.Services.AddScoped<IRpslsService, RpslsService>();

            ConfigureAutoMapper(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        public static void ConfigureAutoMapper(IServiceCollection services)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelViewProfile>();
            });

            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);
        }
    }
}
