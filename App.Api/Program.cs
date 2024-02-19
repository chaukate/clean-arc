using App.Application;
using App.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment}.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.UseDateOnlyTimeOnlyStringConverters();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = $"CTARA - {builder.Environment.EnvironmentName}"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token. Token is generated from Accounts/Login.",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{ }
        }
    });
    options.CustomSchemaIds(x => x.FullName);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName == "Local")
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CTARA Service");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
