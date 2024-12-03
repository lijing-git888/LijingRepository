using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SqlSugar;
using StackExchange.Redis;

namespace Hello0
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("BasicDataApi", new OpenApiInfo { Title = "基础数据服务", Version = "v1" });
            });

            // 添加 SqlSugar 的服务
            builder.Services.AddScoped<UserRepository>(sp =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                var db = new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = connectionString,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true
                });
                return new UserRepository(db, sp.GetRequiredService<IConnectionMultiplexer>());
            });

            // 注册 Redis 客户端
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration["Redis:ConnectionString"]));



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "{documentName}/swagger.json";
                })
                .UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/BasicDataApi/swagger.json", "BasicDataApi");
                });
            }

            app.UseHttpsRedirection();
            app.MapControllers();



            app.Run();
        }
    }
}
