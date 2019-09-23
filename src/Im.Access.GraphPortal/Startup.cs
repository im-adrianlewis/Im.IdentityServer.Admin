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
using GraphQL.Types;
using GraphQL.Types.Relay;
using Im.Access.GraphPortal.Data;
using Im.Access.GraphPortal.Graph;
using Im.Access.GraphPortal.Graph.Mutations;
using Im.Access.GraphPortal.Graph.Queries;
using Im.Access.GraphPortal.Graph.Queries.ClientGroup;
using Im.Access.GraphPortal.Graph.Queries.UserGroup;
using Im.Access.GraphPortal.Graph.Subscriptions;
using Im.Access.GraphPortal.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.Swagger;

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

            services.AddScoped<ISchema, IdentitySchema>();
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

            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IClientStore, ClientStore>();
            services.AddScoped<IClientRepository, ClientRepository>();

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

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
                            new Info
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
                            new OAuth2Scheme
                            {
                                Type = "oauth2",
                                Flow = "implicit",
                                AuthorizationUrl = $"{Configuration["IdentityServer"]}/connect/authorize",
                                TokenUrl = $"{Configuration["IdentityServer"]}/connect/token",
                                Scopes =
                                    new Dictionary<string, string>
                                    {
                                        { "im-access-graph-api_user:manage", "Graph API access to users with manage permissions." },
                                        { "im-access-graph-api_user:read", "Graph API access to users with read permissions." },
                                        { "im-access-graph-api_client:manage", "Graph API access to clients with manage permissions." },
                                        { "im-access-graph-api_client:read", "Graph API access to clients with read permissions." }
                                    }
                            });
                        options.AddSecurityRequirement(
                            new Dictionary<string, IEnumerable<string>>
                            {
                                { "oauth2", new string[0] }
                            });
                    });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseAuthentication();
            app.UseWebSockets();
            app.UseGraphQLWebSockets<IdentitySchema>();
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
            app.UseMvc();
        }
    }
}
