using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Security.Cryptography;
using System.IdentityModel.Tokens;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Authentication.JwtBearer;
using System;

namespace Lisa.Excelsis.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        const string TokenAudience = "Excelsis";
        const string TokenIssuer = "Excelsis";
        private RsaSecurityKey key;
        private TokenAuthOptions tokenOptions;

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            // Replace this with some sort of loading from config / file.
            RSAParameters keyParams = RSAKeyUtils.GetRandomKey();

            // Create the key, and a set of token options to record signing credentials 
            // using that key, along with the other parameters we will need in the 
            // token controlller.
            key = new RsaSecurityKey(keyParams);
            tokenOptions = new TokenAuthOptions()
            {
                Audience = TokenAudience,
                Issuer = TokenIssuer,
                SigningCredentials = new SigningCredentials(key,
                    SecurityAlgorithms.RsaSha256Signature)
            };

            // Save the token options into an instance so they're accessible to the 
            // controller.
            services.AddInstance<TokenAuthOptions>(tokenOptions);

            // Enable the use of an [Authorize("Bearer")] attribute on methods and
            // classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });
            services.AddOptions();
            services.Configure<TableStorageSettings>(Configuration.GetSection("TableStorage"));
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddCors();

            services.AddScoped<Database>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseJwtBearerAuthentication(options =>
            {
                // Basic settings - signing key to validate with, audience and issuer.
                options.TokenValidationParameters.IssuerSigningKey = key;
                options.TokenValidationParameters.ValidAudience = tokenOptions.Audience;
                options.TokenValidationParameters.ValidIssuer = tokenOptions.Issuer;

                // When receiving a token, check that we've signed it.
                options.TokenValidationParameters.ValidateSignature = true;

                // When receiving a token, check that it is still valid.
                options.TokenValidationParameters.ValidateLifetime = true;

                // This defines the maximum allowable clock skew - i.e. provides a tolerance on the 
                // token expiry time when validating the lifetime. As we're creating the tokens locally
                // and validating them on the same machines which should have synchronised 
                // time, this can be set to zero. Where external tokens are used, some leeway here 
                // could be useful.
                options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            });

            app.UseIISPlatformHandler();
            app.UseCors(cors =>
            {
                cors.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            app.UseMvc();
        }

        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}