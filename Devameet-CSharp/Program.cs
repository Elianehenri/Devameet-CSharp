using Devameet_CSharp;
using Devameet_CSharp.Models;
using Devameet_CSharp.Repository;
using Devameet_CSharp.Repository.Impl;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//conectar banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DevameetContext>(options => options.UseSqlServer(connectionString));

//configurar injeçao de dependencia
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IMeetRepository, MeetRepositoryImpl>();
builder.Services.AddScoped<IMeetObjectRepository, MeetObjectRepositoryImpl>();
builder.Services.AddScoped<IRoomRepository, RoomRepositoryImpl>();


//configurar jwt
//na hora de inicializar a aplicaçao, ela esta pegando a configurçao do jwt la no appsettings.json e colocando na classe JWTKey
var jwtsettings = builder.Configuration.GetRequiredSection("JWT").Get<JWTKey>();
//acessando a chave de segurança
var secretKey = Encoding.ASCII.GetBytes(jwtsettings.SecretKey);
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(authentication =>
{
    authentication.RequireHttpsMetadata = false;
    authentication.SaveToken = true;
    authentication.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
