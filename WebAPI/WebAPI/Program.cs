

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WebAPI.API.Middleware.Auth;
using WebAPI.API.Middleware.Exception;
using WebAPI.Common;
using WebAPI.DataPersistance;
using WebAPI.Interfaces;
using WebAPI.Repositories;
using WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["AuthorizationSettings:Audience"],
            ValidIssuer = builder.Configuration["AuthorizationSettings:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthorizationSettings:Secret"])),
            ClockSkew = TimeSpan.Zero

        };
    }
    ) ;

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.User, policy =>
        policy.Requirements.Add(new PrivilegeRequirement(Policies.User)));
    config.AddPolicy(Policies.Admin, policy =>
         policy.Requirements.Add(new PrivilegeRequirement(Policies.Admin)));
    config.AddPolicy(Policies.All, policy =>
        policy.Requirements.Add(new PrivilegeRequirement(Policies.All)));
});

builder.Services.Configure<AuthorizationSettings>(builder.Configuration.GetSection("AuthorizationSettings"));

builder.Services.AddSingleton<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddSingleton<IEndangeredAnimalRepository, EndangeredAnimalRepository>();
builder.Services.AddTransient<IEndangeredAnimalService, EndangeredAnimalService>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureExceptionHandler();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => endpoints.MapControllers());

//app.MapControllers();

app.Run();
