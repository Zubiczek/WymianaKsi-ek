using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WymianaKsiazek.Database;
using WymianaKsiazek.Database.Entities;
using WymianaKsiazek.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using WymianaKsiazek.Models.EmailModels;
using Microsoft.AspNetCore.Http;
using WymianaKsiazek.Queries.UserQueries;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using WymianaKsiazek.Queries.OfferQueries;
using WymianaKsiazek.Queries.BookQueries;
using Microsoft.AspNetCore.SignalR;
using WymianaKsiazek.Hubs;
using WymianaKsiazek.Queries.MessageQueries;
using WymianaKsiazek.Queries.CommentQueries;
using WymianaKsiazek.Queries.LikeQueries;
using WymianaKsiazek.Queries.OpinionQueries;
using WymianaKsiazek.Queries.EmailQueries;
using WymianaKsiazek.Functions;

namespace WymianaKsiazek
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<Context>(options => options.UseSqlServer(Configuration.GetConnectionString("WymianaKsiazekContext")));
            services.AddSingleton(AutoMapperConfig.Initialize());
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IUserQueries, UserQueries>();
            services.AddScoped<IOfferQueries, OfferQueries>();
            services.AddScoped<IBookQueries, BookQueries>();
            services.AddScoped<IMessageQueries, MessageQueries>();
            services.AddScoped<ICommentQueries, CommentQueries>();
            services.AddScoped<ILikeQueries, LikeQueries>();
            services.AddScoped<IOpinionQueries, OpinionQueries>();
            services.AddScoped<IEmailQueries, EmailQueries>();
            services.AddScoped<ILoggedInUser, LoggedInUser>();
            services.AddScoped<IReportQueries, ReportQueries>();
            services.AddScoped<IEditQueries, EditQueries>();
            services.AddScoped<ISaveImg, SaveImg>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(x => {
                var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
                var factory = x.GetRequiredService<IUrlHelperFactory>();
                return factory.GetUrlHelper(actionContext);
            });
            services
                .AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = true;
            });
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            services.AddSignalR();
            services.AddAuthorization();
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddMemoryCache();
            services.AddSession();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chatHub");
            });

            app.Use(async (context, next) =>
            {
                var token = context.Session.GetString("Token");
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + token);
                }
                await next();
            });
        }
    }
}
