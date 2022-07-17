using DATN_Back_end.Config;
using DATN_Back_end.Configure;
using DATN_Back_end.Extensions;
using DATN_Back_end.Handlings;
using DATN_Back_end.Repositories;
using DATN_Back_end.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end
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
            services.AddControllers();
            services.ConfigType<PostgresConfig>(Configuration);
            services.ConfigType<TokenConfig>(Configuration);
            services.ConfigType<CloudinaryConfig>(Configuration);
            services.ConfigType<ApplicationConfig>(Configuration);
            services.ConfigType<MailConfig>(Configuration);
            services.AddMvc(ConfigMvc).ConfigureJson();
            services.AddHttpContextAccessor();
            services.AddAuthentication();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DATN_Back_end", Version = "v1" });
            });

            services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IRequestTypeRepository, RequestTypeRepository>();
            services.AddScoped<IFormStatusRepository, FormStatusRepository>();
            services.AddScoped<IFormRequestRepository, FormRequestRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IForgetPasswordService, ForgetPasswordService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ISendMailService, SendMailService>();
            services.AddScoped<ITimeKeepingRepository, TimeKeepingRepository>();
            services.AddScoped<IWorkingTimeRepository, WorkingTimeRepository>();

            services.ConfigDatabase();
            services.ConfigSecurity();
            services.AddHealthChecks();
            services.MigrateDatabase();
        }

        private void ConfigMvc(MvcOptions options)
        {
            options.Filters.Add(typeof(HandleExceptionHandling));
            options.Filters.Add(typeof(ValidateModelHandling));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials()); // allow credentials

            app.AddSwagger();

            app.UseHttpsRedirection();

            app.UseHealthChecks("/health");

            app.UseRouting();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMiddleware<TokenProviderMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
