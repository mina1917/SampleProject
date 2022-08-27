using Microsoft.Extensions.FileProviders;
using SampleProject.Application.Configuration;
using SampleProject.Framework;
using SampleProject.Messaging;
using SampleProject.Messaging.Receivers;
using SampleProject.Messaging.Senders;
using SampleProject.Persistence.Configuration;
using SampleProject.WebApi.Extensions;
using SampleProject.WebApi.Swagger;

namespace SampleProject.WebApi
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly SiteSettings _siteSetting;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _siteSetting = configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddControllers();
            services.AddHttpClient();
            services.AddHttpContextAccessor();
            services.AddPersistance(Configuration);
            services.AddCustomVersioningSwagger();

            var _siteSetting = Configuration.GetSection(nameof(SiteSettings)).Get<SiteSettings>();
            services.Configure<SiteSettings>(Configuration.GetSection(nameof(SiteSettings)));
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection("RabbitMq"));

            services.AddApplicationCore(Configuration);

            services.AddScoped<IClaimHelper, ClaimHelper>();
            services.AddSingleton<IUserCreateSender, UserCreateSender>();
            services.AddHostedService<UserCreateReceiver>();
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseCustomExceptionHandler();
            else
                app.UseCustomExceptionHandler();

            app.InitializeDatabase();

            app.UseCustomSwagger();

            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin();
                policy.AllowAnyMethod();
                policy.AllowAnyHeader();
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.ContentRootPath, "files")),
                RequestPath = "/files"
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }
    }
}
