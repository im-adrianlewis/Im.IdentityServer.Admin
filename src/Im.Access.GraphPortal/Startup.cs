using System;
using GraphiQl;
using GraphQL;
using GraphQL.Http;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Im.Access.GraphPortal.Data;
using Im.Access.GraphPortal.Graph;
using Im.Access.GraphPortal.Graph.Mutations;
using Im.Access.GraphPortal.Graph.Queries;
using Im.Access.GraphPortal.Graph.Queries.TenantGroup;
using Im.Access.GraphPortal.Graph.Subscriptions;
using Im.Access.GraphPortal.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

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

            services.AddScoped<TenantType>();
            services.AddScoped<UserTenantType>();
            services.AddScoped<UserSearchCriteriaType>();
            services.AddScoped<UserType>();
            services.AddScoped<UserClaimType>();

            services.AddScoped<IUserStore, UserStore>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddDbContext<IdentityDbContext>(
                options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:Admin"]);
                });

            services
                .AddAuthentication(
                    options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                    })
                .AddCookie(
                    options =>
                    {
                        options.Cookie.Name = ".im.access.graph";
                        options.Cookie.HttpOnly = true;
                        options.Cookie.SameSite = SameSiteMode.Strict;
                        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                        options.ExpireTimeSpan = TimeSpan.FromHours(1);
                    })
                .AddOpenIdConnect(
                    options =>
                    {
                        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        options.Authority = Configuration["Security:IdentityServer"];
                        options.ClientId = "Admin API";
                        options.ClientSecret = Configuration["Security:IdentityGraphClientSecret"];
                        options.RequireHttpsMetadata = true;
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.ResponseMode = OpenIdConnectResponseMode.FormPost;
                        options.ResponseType = OpenIdConnectResponseType.Code;

                        options.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash");

                        options.Scope.Add("GraphApi");

                        options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                NameClaimType = "name",
                                RoleClaimType = "role"
                            };
                    });

            services
                .AddMvc(
                    options =>
                    {
                        options.InputFormatters.Add(new GraphQlMediaTypeFormatter(false));
                    })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
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
            app.UseGraphiQl();
            app.UseMvc();
        }
    }
}
