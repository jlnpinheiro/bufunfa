using JNogueira.Bufunfa.Api.Filters;
using JNogueira.Bufunfa.Api.Middlewares;
using JNogueira.Bufunfa.Api.Swagger.Filters;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Servicos;
using JNogueira.Bufunfa.Infraestrutura;
using JNogueira.Bufunfa.Infraestrutura.Dados;
using JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios;
using JNogueira.Bufunfa.Infraestrutura.Integracoes.AlphaVantage;
using JNogueira.Logger.Discord;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

namespace JNogueira.Bufunfa.Api
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

            services.AddScoped<EfDataContext, EfDataContext>(x => new EfDataContext(configHelper.BancoDadosStringConnection));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAgendamentoRepositorio, AgendamentoRepositorio>();
            services.AddScoped<IAtalhoRepositorio, AtalhoRepositorio>();
            services.AddScoped<ICartaoCreditoRepositorio, CartaoCreditoRepositorio>();
            services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
            services.AddScoped<IContaRepositorio, ContaRepositorio>();
            services.AddScoped<IFaturaRepositorio, FaturaRepositorio>();
            services.AddScoped<ILancamentoAnexoRepositorio, LancamentoAnexoRepositorio>();
            services.AddScoped<ILancamentoDetalheRepositorio, LancamentoDetalheRepositorio>();
            services.AddScoped<ILancamentoRepositorio, LancamentoRepositorio>();
            services.AddScoped<IParcelaRepositorio, ParcelaRepositorio>();
            services.AddScoped<IPeriodoRepositorio, PeriodoRepositorio>();
            services.AddScoped<IPessoaRepositorio, PessoaRepositorio>();
            services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

            services.AddScoped<AgendamentoServico>();
            services.AddScoped<AtalhoServico>();
            services.AddScoped<CartaoCreditoServico>();
            services.AddScoped<CategoriaServico>();
            services.AddScoped<ContaServico>();
            services.AddScoped<GraficoServico>();
            services.AddScoped<LancamentoServico>();
            services.AddScoped<PeriodoServico>();
            services.AddScoped<PessoaServico>();
            services.AddScoped<UsuarioServico>();

            services.AddScoped<ApiAlphaVantageProxy>();

            services
                // AddAuthentication: especificará os schemas utilizados para a autenticação do tipo Bearer
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // AddJwtBearer: definidas configurações como a chave e o algoritmo de criptografia utilizados, a necessidade de analisar se um token ainda é válido e o tempo de tolerância para expiração de um token
                .AddJwtBearer(options =>
                {
                    var paramsValidation              = options.TokenValidationParameters;
                    paramsValidation.IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(configHelper.JwtTokenConfig.SecurityKey));
                    paramsValidation.ValidAudience    = configHelper.JwtTokenConfig.Audience;
                    paramsValidation.ValidIssuer      = configHelper.JwtTokenConfig.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Verifica se um token recebido ainda é válido
                    paramsValidation.ValidateLifetime = true;

                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            // AddAuthorization: ativará o uso de tokens com o intuito de autorizar ou não o acesso a recursos da aplicação
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services
                .AddControllers(options => options.Filters.Add(typeof(CustomModelStateValidationFilterAttribute)))
                .AddNewtonsoftJson();

            var swaggerDescricao = "API que disponibiliza o acesso as informações geridas pelo sistema \"Bufunfa!\".<br>" +
                                    "<ul>" +
                                        $"<li>Ambiente atual: <b>{Environment.EnvironmentName}</b></li>" +
                                    "</ul>";

            var gitInfoHelper = new GitInfoHelper();

            swaggerDescricao += "<p>Informa&ccedil;&otilde;es do Git:</p>" +
                                "<ul>" +
                                    "<li>Reposit&oacute;rio: <a href=\"https://github.com/jlnpinheiro/bufunfa/tree/master/src/backend\" target =\"_blank\">https://github.com/jlnpinheiro/bufunfa/tree/master/src/backend</a></li>" +
                                    $"<li>Branch: {gitInfoHelper.Branch}</li>" +
                                    $"<li>Commit: <a href=\"https://github.com/jlnpinheiro/bufunfa/commit/{gitInfoHelper.Commit}\" target=\"_blank\">{gitInfoHelper.Versao}</a> <i>(por {gitInfoHelper.ResponsavelCommit} em {gitInfoHelper.DataComit})</i></li>" +
                                "</ul>";

            // Configuração do Swagger para documentação da API
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Description = swaggerDescricao
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Obtenha o token através da rota \"v1/usuarios/autenticar\" . Digite \"Bearer \" e cole o token obtido no campo abaixo.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>(); // Adds "(Auth)" to the summary so that you can see which endpoints have Authorization

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.ExampleFilters();
                options.DocumentFilter<EnumDocumentFilter>();
                options.IncludeXmlComments(xmlPath);
                options.EnableAnnotations();
            });

            services.AddSwaggerExamplesFromAssemblyOf<Startup>();

            // Habilita a compressão do response
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        }

        public void Configure(IApplicationBuilder app, IHttpContextAccessor httpContextAccessor, ILoggerFactory loggerFactory, ConfigurationHelper configHelper)
        {
            app.UsePathBase("/api");

            // Entende que a página default é a "index.html" dentro da pasta "wwwroot"
            app.UseDefaultFiles();

            app.UseStaticFiles();

            if (!string.IsNullOrEmpty(configHelper.DiscordWebhookUrl))
            {
                loggerFactory
                    // Adiciona o logger provider para o Discord.
                    .AddDiscord(new DiscordLoggerOptions(configHelper.DiscordWebhookUrl) { ApplicationName = "Backend", EnvironmentName = Environment.EnvironmentName, UserName = "bufunfa-bot" }, httpContextAccessor);
            }

            // Definindo a cultura padrão: pt-BR
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            //Middleware customizado para interceptar erros HTTP e exceptions não tratadas
            app.UseCustomExceptionHandler();

            // Middleware para utilização do Swagger.
            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });

            // Middleware para utilização do Swagger UI (HTML, JS, CSS, etc.)
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs"; // Define a documentação no endereço http://{url}/docs/
                options.SwaggerEndpoint("v1/swagger.json", "v1");
                options.DefaultModelsExpandDepth(-1); // Oculta a sessão "Models"
                options.DocExpansion(DocExpansion.None);
                options.InjectStylesheet("/api/swagger-ui/custom.css");
                options.DocumentTitle = "Bufunfa! Backend";
                options.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("JNogueira.Bufunfa.Api.Swagger.UI.index.html"); // Permite a utilização de um index.html customizado
            });

            // Utiliza a compressão do response
            app.UseResponseCompression();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
