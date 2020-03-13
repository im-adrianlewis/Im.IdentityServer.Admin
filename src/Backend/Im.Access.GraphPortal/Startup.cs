using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using GraphQL;
using GraphQL.Http;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using GraphQL.Types.Relay;
using Im.Access.GraphPortal.Data;
using Im.Access.GraphPortal.Graph;
using Im.Access.GraphPortal.Graph.ClientGroup.Queries;
using Im.Access.GraphPortal.Graph.Common.Queries;
using Im.Access.GraphPortal.Graph.OperationalGroup;
using Im.Access.GraphPortal.Graph.OperationalGroup.Mutations;
using Im.Access.GraphPortal.Graph.OperationalGroup.Queries;
using Im.Access.GraphPortal.Graph.OperationalGroup.Subscriptions;
using Im.Access.GraphPortal.Graph.UserGroup.Queries;
using Im.Access.GraphPortal.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Polly.Registry;

namespace Im.Access.GraphPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDependencyResolver>(
                serviceProvider => new FuncDependencyResolver(serviceProvider.GetService));
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<IDocumentWriter>(_ => new DocumentWriter(true));

            services.AddScoped<IdentitySchema>();
            services.AddScoped(typeof(ConnectionType<>));
            services.AddScoped(typeof(EdgeType<>));
            services.AddScoped<PageInfoType>();
            services.AddScoped(typeof(PaginationType<,>));
            services.AddScoped<IdentityQuery>();
            services.AddScoped<IdentityMutation>();
            services.AddScoped<IdentitySubscription>();

            services.AddScoped<UserQueryType>();
            services.AddScoped<UserSearchCriteriaType>();
            services.AddScoped<UserType>();
            services.AddScoped<UserClaimType>();
            services.AddScoped<ClientQueryType>();
            services.AddScoped<ClientType>();
            services.AddScoped<OperationalQueryType>();
            services.AddScoped<OperationalMutationType>();
            services.AddScoped<OperationalSubscriptionType>();
            services.AddScoped<CircuitBreakerPolicyType>();
            services.AddScoped<CircuitBreakerPolicyInputType>();
            services.AddScoped<CircuitBreakerPolicySubscriptionType>();
            services.AddScoped<ChaosPolicyType>();
            services.AddScoped<ChaosPolicyInputType>();
            services.AddScoped<ChaosPolicySubscriptionType>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IChaosPolicyRepository, ChaosPolicyRepository>();
            services.AddScoped<ICircuitBreakerPolicyRepository, CircuitBreakerPolicyRepository>();

            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IClientStore, ClientStore>();
            services.AddScoped<IChaosPolicyStore, ChaosPolicyStore>();
            services.AddScoped<ICircuitBreakerPolicyStore, CircuitBreakerPolicyStore>();

            services.AddSingleton<IPolicyRegistryFactory, PolicyRegistryFactory>();
            services.AddSingleton<IReadOnlyPolicyRegistry<string>>(
                provider =>
                {
                    var factory = provider.GetRequiredService<IPolicyRegistryFactory>();
                    return factory.Create();
                });

            services.AddDbContext<IdentityDbContext>(
                options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:Admin"]);
                });
            services.AddDbContext<ConfigurationDbContext>(
                options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:Admin"]);
                });
            services.AddDbContext<OperationDbContext>(
                options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:Admin"]);
                });

            services.AddHsts(
                options =>
                {
                    var maxAgeText = Configuration["HstsMaxAgeInDays"];
                    var maxAge = string.IsNullOrEmpty(maxAgeText) ? 30 : int.Parse(maxAgeText);
                    options.MaxAge = TimeSpan.FromDays(maxAge);
                });

            services
                .AddAuthentication()
                .AddJwtBearer(
                    options =>
                    {
                        options.Authority = Configuration["IdentityServer"];
                        options.Audience = $"{Configuration["IdentityServer"]}/resources";
                    });

            services
                .AddMvc(
                    options =>
                    {
                        options.InputFormatters.Add(new GraphQlMediaTypeFormatter(false));
                    })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services
                .AddGraphQL(
                    options =>
                    {
                        options.EnableMetrics = true;
                    })
                .AddWebSockets()
                .AddDataLoader();

            services
                .AddSwaggerGen(
                    options =>
                    {
                        options.SwaggerDoc(
                            "v1",
                            new OpenApiInfo
                            {
                                Version = "v1",
                                Title = "Im Access API",
                                Description = "Comprehensive API for accessing identity information"
                            });

                        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                        options.IncludeXmlComments(xmlPath);

                        options.AddSecurityDefinition(
                            "oauth2",
                            new OpenApiSecurityScheme
                            {
                                Type = SecuritySchemeType.OAuth2,
                                Flows = new OpenApiOAuthFlows
                                {
                                    Implicit = new OpenApiOAuthFlow
                                    {
                                        AuthorizationUrl = new Uri($"{Configuration["IdentityServer"]}/connect/authorize"),
                                        TokenUrl = new Uri($"{Configuration["IdentityServer"]}/connect/token"),
                                        Scopes =
                                            new Dictionary<string, string>
                                            {
                                                { "im-access-graph-api_user:manage", "Graph API access to users with manage permissions." },
                                                { "im-access-graph-api_user:read", "Graph API access to users with read permissions." },
                                                { "im-access-graph-api_client:manage", "Graph API access to clients with manage permissions." },
                                                { "im-access-graph-api_client:read", "Graph API access to clients with read permissions." }
                                            }
                                    }
                                }
                            });
                        //options.AddSecurityRequirement(
                        //    new OpenApiSecurityRequirement
                        //    {
                        //    });
                    });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseWebSockets(
                new WebSocketOptions
                {
                    KeepAliveInterval = TimeSpan.FromMinutes(30),
                    ReceiveBufferSize = 4096
                });
            app.UseGraphQLWebSockets<IdentitySchema>("/graphql");
            app.UseGraphiQLServer(
                new GraphiQLOptions
                {
                    GraphiQLPath = "/ui/graphiql",
                    GraphQLEndPoint = "/graphql"
                });
            app.UseGraphQLPlayground(
                new GraphQLPlaygroundOptions
                {
                    Path = "/ui/playground",
                    GraphQLEndPoint = "/graphql"
                });
            app.UseGraphQLVoyager(
                new GraphQLVoyagerOptions
                {
                    Path = "/ui/voyager",
                    GraphQLEndPoint = "/graphql"
                });

            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint(
                        "/swagger/v1/swagger.json", "Im Access API v1");
                    options.OAuthClientId("ImAccessGraphSwagger");
                    options.OAuthAppName("Im.Access.GraphPortal");
                    options.OAuthScopeSeparator(" ");
                    options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                });
            app.UseEndpoints(
                configure =>
                {
                    configure.MapControllers();
                });
        }
    }
}
