using Angular_2.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = "angular_2_server",
        ValidateAudience = true,
        ValidAudience = "angular_2_client",
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superpasswordsuperpassword")),
        ValidateIssuerSigningKey = true,
    });

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseNpgsql(connectionString));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "MyAngular", Version= "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter token",
        Name = "Aithorization",
        Type =  Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "bearer"
                }
            },
            new string[]{}
        }
    });
});
builder.Services.AddCors();

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options => options.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .Build());

app.MapControllers();

app.Run();
