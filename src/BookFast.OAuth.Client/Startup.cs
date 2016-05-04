using BookFast.OAuth.Client.Infrastructure;
using BookFast.OAuth.Client.Infrastructure.OAuth;
using BookFast.OAuth.Client.Proxy;
using Microsoft.AspNet.Authentication.Cookies;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.OptionsModel;

namespace BookFast.OAuth.Client
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets();
            }

            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthenticationOptions>(Configuration.GetSection("Authentication:AzureAd"));
            services.AddMvc();

            services.Configure<BookFastApiOptions>(Configuration.GetSection("Api"));
            services.AddScoped<BookFastApiProxy>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, 
            IOptions<AuthenticationOptions> authOptions)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseIISPlatformHandler();

            app.UseStaticFiles();

            app.UseCookieAuthentication(options => options.AutomaticAuthenticate = true);
            app.UseAzureAD(options =>
                           {
                               options.AuthenticationScheme = "AzureAD";
                               options.AutomaticChallenge = true;

                               options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                               options.AuthorizationEndpoint = authOptions.Value.AuthorizationEndpoint;
                               options.TokenEndpoint = authOptions.Value.TokenEndpoint;
                               options.ClientId = authOptions.Value.ClientId;
                               options.ClientSecret = authOptions.Value.ClientSecret;
                               options.CallbackPath = new Microsoft.AspNet.Http.PathString("/oauth");

                               options.Resource = authOptions.Value.Resource;

                               options.SaveTokensAsClaims = true;
                           });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
        
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
