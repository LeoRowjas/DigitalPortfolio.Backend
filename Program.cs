using System.Security.Claims;
using DigitalPortfolio.API;
using DigitalPortfolio.API.Data;
using DigitalPortfolio.API.Helpers;
using DigitalPortfolio.API.Services;
using DigitalPortfolio.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var securityKey = builder.Configuration.GetSection("Security")["SecurityKey"];

ArgumentNullException.ThrowIfNull(securityKey);
AuthorizationOptions.Initialize(securityKey);

builder.Services.AddControllers();

builder.Services.AddDbContext<DigitalPortfolioDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAuthentication();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = AuthorizationOptions.TOKEN_ISSUER,
            ValidAudience = AuthorizationOptions.TOKEN_AUDIENCE,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = AuthorizationOptions.SymmetricSecurityKey,
        };
    });

builder.Services.AddTransient<IPasswordHasher, Sha256PasswordHasher>();
builder.Services.AddScoped<IAccountService, AccountService>();
        
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
        
app.UseSwagger();
app.UseSwaggerUI();
        
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();