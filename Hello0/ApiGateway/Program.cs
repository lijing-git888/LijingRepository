using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Values;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("ApiGateway", new OpenApiInfo { Title = "���ط���", Version = "v1" });
});

// ���Ocelot����
builder.Services.AddOcelot(new ConfigurationBuilder().AddJsonFile("ocelot.json").Build());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/BasicDataApi/swagger.json", "BasicDataApi");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

// ʹ��Ocelot�м��
app.UseOcelot().Wait();

app.MapRazorPages();
app.Run();