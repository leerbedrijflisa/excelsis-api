using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Security.Cryptography;

namespace Lisa.Excelsis.Api
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        //const string TokenAudience = "Excelsis";
        //const string TokenIssuer = "Excelsis";
        //private RsaSecurityKey key;
        //private TokenAuthOptions tokenOptions;

        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddApplicationInsightsTelemetry(Configuration);

            // Replace this with some sort of loading from config / file.
            //RSAParameters keyParams = RSAKeyUtils.GetRandomKey();

            // Create the key, and a set of token options to record signing credentials 
            // using that key, along with the other parameters we will need in the 
            // token controlller.
            //key = new RsaSecurityKey(keyParams);
            //tokenOptions = new TokenAuthOptions()
            //{
            //    Audience = TokenAudience,
            //    Issuer = TokenIssuer,
            //    SigningCredentials = new SigningCredentials(key,
            //        SecurityAlgorithms.RsaSha256Signature)
            //};

            // Save the token options into an instance so they're accessible to the 
            // controller.
            //services.AddInstance<TokenAuthOptions>(tokenOptions);

            // Enable the use of an [Authorize("Bearer")] attribute on methods and
            // classes to protect.
            /*services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });*/

            // Assembly Microsoft.Extensions.OptionsModel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=adb9793829ddae60
            services.Configure<TableStorageSettings>(options => options = null);
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            services.AddCors();

            //services.AddScoped<Database>();
        }

        public void Configure(IApplicationBuilder app)
        {
            /*app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;
                    // This should be much more intelligent - at the moment only expired 
                    // security tokens are caught - might be worth checking other possible 
                    // exceptions such as an invalid signature.
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        // What you choose to return here is up to you, in this case a simple 
                        // bit of JSON to say you're no longer authenticated.
                        context.Response.ContentType = "application/json";
                    }
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                    }
                    // We're not trying to handle anything else so just let the default 
                    // handler handle.
                    else
                    {
                        await next();
                    }
                });
            });*/

            /*var options = new JwtBearerOptions();
            // Basic settings - signing key to validate with, audience and issuer.
            options.TokenValidationParameters.IssuerSigningKey = key;
            options.TokenValidationParameters.ValidAudience = tokenOptions.Audience;
            options.TokenValidationParameters.ValidIssuer = tokenOptions.Issuer;

            // When receiving a token, check that we've signed it.
            //options.TokenValidationParameters.ValidateSignature = true;

            // When receiving a token, check that it is still valid.
            options.TokenValidationParameters.ValidateLifetime = true;

            // This defines the maximum allowable clock skew - i.e. provides a tolerance on the 
            // token expiry time when validating the lifetime. As we're creating the tokens locally
            // and validating them on the same machines which should have synchronised 
            // time, this can be set to zero. Where external tokens are used, some leeway here 
            // could be useful.
            options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
            app.UseJwtBearerAuthentication(options);*/
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseCors(cors =>
            {
                cors.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            app.UseMvc();
        }

        public static void Main(string[] args)
        {
           var host = new WebHostBuilder()
             .UseKestrel()
             .UseContentRoot(Directory.GetCurrentDirectory())
             .UseIISIntegration()
             .UseStartup<Startup>()
             .Build();
 
           host.Run();
        }
    }
}