using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Logger.Discord;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace JNogueira.Bufunfa.Web
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

            // Extrai as informações do arquivo de configuração (appSettings.*.json) ou das variáveis de ambiente
            var configHelper = new ConfigurationHelper(Configuration);

            // AddSingleton: instância configurada de forma que uma única referência das mesmas seja empregada durante todo o tempo em que a aplicação permanecer em execução
            services.AddSingleton(configHelper);

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

            // Configuração necessária para utilização do AntiforgeryToken (http://blog.etrupja.com/2018/08/the-antiforgery-token-could-not-be-decrypted/)
            services.AddDataProtection()
                .SetApplicationName("Bufunfa-Frontend")
                .PersistKeysToFileSystem(new DirectoryInfo(AppContext.BaseDirectory));

            var builder = services.AddControllersWithViews(options =>
            {
                // Previne ataques CSRF - Cross-Site Request Forgery (Falsificação de solicitação entre sites) (https://www.eduardopires.net.br/2018/02/prevenindo-ataques-csrf-no-asp-net-core/)
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });
            
            services.AddProgressiveWebApp();

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

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor, ConfigurationHelper configHelper)
        {
            if (!string.IsNullOrEmpty(configHelper.DiscordWebhookUrl))
            {
                loggerFactory
                    // Adiciona o logger provider para o Discord.
                    .AddDiscord(new DiscordLoggerOptions(configHelper.DiscordWebhookUrl) { ApplicationName = "Frontend", EnvironmentName = Environment.EnvironmentName, UserName = "bufunfa-bot" }, httpContextAccessor);
            }

            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            RotativaConfiguration.Setup(Environment.ContentRootPath, "wwwroot/Rotativa");

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
