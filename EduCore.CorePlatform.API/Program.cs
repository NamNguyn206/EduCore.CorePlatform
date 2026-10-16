using EduCore.CorePlatform.API.Correlation;
using EduCore.CorePlatform.API.Extensions;
using EduCore.CorePlatform.API.Logging;
using EduCore.CorePlatform.API.Middlewares;
using EduCore.CorePlatform.Application.Extensions;
using EduCore.CorePlatform.Domain.Common.Settings.Auth;
using EduCore.CorePlatform.Infrastructure.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================================
// Add CORS
// ================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("https://testtbduni.bhs-technology.net")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // nếu có cookie/token
    });
});

// ================================
// 1️ Core ASP.NET services
// ================================
builder.Services.AddControllers()
                .AddJsonOptions(o => {
                    // options for patch engine etc
                });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);
});


// ================================
// 2️ Localization
// ================================
//builder.Services.AddLocalization(options =>
//{
//    options.ResourcesPath = "Common/Localization";
//});
builder.Services.AddLocalization();

// ================================
// Authentication
// ================================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // LẤY SETTINGS RA ĐỂ CONFIG JWT
                    var jwtSettings = builder.Configuration
                                            .GetSection("ApplicationSettings:Jwt")
                                            .Get<JwtSettings>()!;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });




// ================================
// 6️ Application & Infrastructure
// ================================
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiVersioningConfig();


// 🔹 SERILOG
builder.UseSerilogLogging();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();


var app = builder.Build();

// ================================
// Use CORS
// ================================
app.UseCors("AllowFrontend");

// ================================
// 🌍 Localization
// ================================

var supportedCultures = new[] { "en", "vi" };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new("vi"),
    SupportedCultures = supportedCultures
        .Select(c => new CultureInfo(c))
        .ToList(),
    SupportedUICultures = supportedCultures
        .Select(c => new CultureInfo(c))
        .ToList()
});

// =====================
// 🔑 INIT AUTHORIZATION (CHỖ NÀY)
// =====================
await app.InitAuthorizationAsync();

// ================================
// ❗ Global Exception Middleware
// ================================
//app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
//app.UseMiddleware<ExceptionHandlingMiddleware>();

//app.UseMiddleware<CorrelationIdMiddleware>();
//app.UseMiddleware<RequestLoggingMiddleware>();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
app.MapOpenApi();
//}

app.UseHttpsRedirection();

// ================================
// 🔐 Security
// ================================

app.UseAuthentication();

//app.UseMiddleware<TenantResolutionMiddleware>();

app.UseAuthorization();

// ================================
// 🚀 Endpoints
// ================================

app.MapControllers();

app.Run();
