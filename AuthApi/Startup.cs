using System;
using AuthApi.Data;
using AuthApi.Providers;
using AuthApi.Providers.Impl;
using AuthApi.Services;
using AuthApi.Services.Impl;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthApi
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;

            _hostingEnvironment = hostingEnvironment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine($"'{Configuration["ConnectionString"]}'");

            services.AddDbContext<IUserDbContext, UserDbContext>(opt =>
            {
                opt.UseSqlServer(Configuration["ConnectionString"]);
            });

            services.AddTransient<IUserCreateService, UserCreateService>();
            services.AddTransient<IGetUserProvider, GetUserProvider>();

            services.AddTransient<ITelemetryService, AppInsightsTelemetryService>();
            services.AddTransient<ICreateTokenService, JwtCreateTokenService>();
            services.AddTransient<IPasswordHasher, Rfc2898DeriveBytesPasswordHasher>();

            var aiKey = Configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
            services.AddApplicationInsightsTelemetry(aiKey);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ApiBehaviorOptions>(o =>
            {
                o.InvalidModelStateResponseFactory = actionContext =>
                    new BadRequestObjectResult(actionContext.ModelState);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<IUserDbContext>();
                context.Database.Migrate();

                if (!_hostingEnvironment.IsEnvironment("production"))
                    context.SeedTestUsers().GetAwaiter().GetResult();
            }

            app.UseDeveloperExceptionPage();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
