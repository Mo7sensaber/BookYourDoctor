using Persistanse.Context;
using Microsoft.EntityFrameworkCore;
using ServiceAbestraction;
using Service;
using Service.Mapping;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Domain.RepoInterface;
using Persistanse.Repository;
using System.Text.Json.Serialization;
namespace BookYourDoctor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
            builder.Services.AddHttpClient();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(builder =>
            {
                builder.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter Bearer Followed By token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme= "Bearer"
                });
                builder.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });
            });

            builder.Services.AddHttpClient("PaymobClient", client =>
            {
                client.BaseAddress = new Uri("https://egypt.paymob.com/api/");
            });

            // في جزء الـ Service Configuration
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.Controller.AppointmentController).Assembly);

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.AddDbContext<ContextBooking>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddDbContext<Persistanse.Identity.IdentityContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IManagerService, ManagerService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IPaymobService, PaymobService>();


            builder.Services.AddScoped<Func<IAuthenticationService>>(
                provider => () => provider.GetRequiredService<IAuthenticationService>());
            builder.Services.AddScoped<Func<ISpecialtyService>>(
                provider => () => provider.GetRequiredService<ISpecialtyService>());
            builder.Services.AddScoped<Func<IDoctorService>>(
                provider => () => provider.GetRequiredService<IDoctorService>());
            builder.Services.AddScoped<Func<IAppointmentService>>(
                provider => () => provider.GetRequiredService<IAppointmentService>());
            builder.Services.AddScoped<Func<IPaymobService>>(
                provider => () => provider.GetRequiredService<IPaymobService>());

            builder.Services.AddAutoMapper(X=>X.AddProfile<ProfileMapping>());
            builder.Services.AddIdentity<ApplicationUser,IdentityRole>()
                .AddEntityFrameworkStores<Persistanse.Identity.IdentityContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(option=>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey= new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]))
                };
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            app.UseRouting();
            // Configure the HTTP request pipeline.
            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UseAuthorization();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.MapControllers();

            app.Run();
        }
    }
}
