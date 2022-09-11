using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MovieFinder.Extensions;
using MovieFinder.Models;
using MovieFinder.Services;
using MovieFinder.Services.Implementation;
using MovieFinder.Services.Interface;
using MovieFinder.Settings;
using MovieFinder.Utils;
using System;
using System.Text;

namespace MovieFinder
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // get configuration data from appsettings.json
            var jwtSecret = builder.Configuration["JwtSecret"];
            var allowOrigin = builder.Configuration["CORSOrigins"];

            builder.Services.AddHttpClient();
            builder.Services.ConfigureCors();
            builder.Services.ConfigureIISIntegration();
            builder.Services.ConfigureDatabase(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddTransient<IIdentityService, IdentityService>();
            builder.Services.AddScoped<IRateLimitsService, RateLimitsService>();
            builder.Services.AddScoped<IStreamingDataService, StreamingDataService>();
            builder.Services.AddScoped<IMoviesService, MoviesService>();
            builder.Services.AddScoped<IImdbIdsService, ImdbIdsService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddIdentity<Users, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<MovieFinderContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options => // Default Lockout settings.
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!#$%&'*+-/=?^_`{|}~.@");

            builder.Services.Configure<MvcOptions>(options =>
                options.EnableEndpointRouting = true);

            MoviePrestoSettings.Configuration = builder.Configuration;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            builder.Services.AddSingleton(tokenValidationParameters);
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            });

            var app = builder.Build();

            if (builder.Environment.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseCors(builder =>
            {
                builder
                .AllowAnyOrigin()
                .WithOrigins(allowOrigin)
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
            app.UseAuthentication();
            app.UseForwardedHeaders();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(opts =>
            {
                opts.MapControllers();
            });

            // seed database with movies from imdb
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<MovieFinderContext>();
                    DataSeeder.SeedData(context);
                }
                catch
                {
                    Console.WriteLine("An error occurred while seeding the database.");
                }
            }

            app.Run(); 
        }
    }
}
