using AuthLab.Api.Data;
using AuthLab.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerUI();
builder.Services.AddSingleton<TokenService>();
    JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero, // no grace period past expiration, for now

            RoleClaimType = "role" // tell ASP.NET Core: use OUR "role" claim for [Authorize(Roles=...)]
        };
    });

    builder.Services.AddAuthorization();
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularDev", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
    });


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularDev");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();