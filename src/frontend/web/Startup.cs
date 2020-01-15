using JNogueira.Bufunfa.Infraestrutura.Logging.Slack;
using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Proxy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace bufunfa_web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment   = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            services.AddTransient<HttpClientHelper, HttpClientHelper>();
            services.AddTransient<DatatablesHelper, DatatablesHelper>();
            services.AddTransient<CookieHelper, CookieHelper>();
            services.AddTransient<CustomHtmlHelper, CustomHtmlHelper>();
            services.AddTransient<BackendProxy, BackendProxy>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/login";
                        options.AccessDeniedPath = $"/feedback/{(int)HttpStatusCode.Forbidden}";
                        options.SlidingExpiration = true;
                        options.Cookie.Name = "Bufunfa";
                        options.ExpireTimeSpan = TimeSpan.FromHours(2); // Se o cookie não for persistente, a sessão ficará ativa por 2 horas
                    });

            services.AddAuthorization(options =>
            {
                // Configuração de autenticação baseada em cookies
                options.AddPolicy("CookieAuthentication", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build());
            });

            var builder = services.AddControllersWithViews(options =>
            {
                // Previne ataques CSRF - Cross-Site Request Forgery (Falsificação de solicitação entre sites) (https://www.eduardopires.net.br/2018/02/prevenindo-ataques-csrf-no-asp-net-core/)
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            if (Environment.IsDevelopment())
            {
                // Posibilita que ao atualizar um arquivo *.cshtml, a alteração seja refletida sem que seja necessário recompilar o projeto (https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-compilation?view=aspnetcore-3.1)
                builder.AddRazorRuntimeCompilation();
            }

            // Habilita a compressão do response
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json", "text/css", "text/html", "text/plain" });
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            loggerFactory
                // Adiciona o logger para mandar mensagem pelo Slack.
                .AddSlackLoggerProvider(
                    Configuration["Slack:Webhook"],
                    Configuration["Slack:Channel"],
                    httpContextAccessor,
                    env.EnvironmentName,
                    Configuration["Slack:Modulo"],
                    Configuration["Slack:UserName"]);

            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            // Define o middleware para interceptar exceptions não tratadas
            app.UseExceptionHandler($"/feedback/{(int)HttpStatusCode.InternalServerError}");

            // Customiza as páginas de erro
            app.UseStatusCodePagesWithReExecute("/feedback/{0}");

            // Utiliza a compressão do response
            app.UseResponseCompression();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
