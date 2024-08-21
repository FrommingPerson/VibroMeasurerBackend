using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using WebApplication2;
using WebApplication2.Controllers;
using WebApplication2.Hubs;
using WebApplication2.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("Default")!);
builder.Services.AddHostedService<MqttHostedService>();

builder.Services.AddHostedService<MyBackgroundTask>();
    builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Version = "v1",
                    Title = "SPKB API",
                    Description = "SPKB Kpp",
                });
            // Add the bearer token authentication option to Swagger
            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                    },
                    new string[] { }
                }
            });
        });

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = builder.Configuration.GetValue<bool>("Identity:ValidateIssuer"),
                    ValidIssuer = builder.Configuration["Identity:ValidIssuer"],
                    ValidateAudience = builder.Configuration.GetValue<bool>("Identity:ValidateAudience"),
                    ValidAudience = builder.Configuration["Identity:ValidAudience"],
                    ValidateLifetime = builder.Configuration.GetValue<bool>("Identity:ValidateLifetime"),
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Identity:IssuerSigningKey"])),
                    ValidateIssuerSigningKey = builder.Configuration.GetValue<bool>("Identity:ValidateIssuerSigningKey"),
                };
            });


builder.Services.AddMySqlDataSource(builder.Configuration.GetConnectionString("Default")!);

// builder.Services.AddTransient<MySqlConnection>(_ =>
    // new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<MineDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddSignalR();
// builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddScoped<IdentityService>();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
    options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffZ";
    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ" });
});;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
        builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)
                // .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<MainHub>("/mainHub");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();

