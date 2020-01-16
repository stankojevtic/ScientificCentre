using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NaucnaCentralaBackend.DataContextNamespace;
using NaucnaCentralaBackend.Email;
using NaucnaCentralaBackend.Interfaces;
using NaucnaCentralaBackend.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NaucnaCentralaBackend.ExternalTaskWorkerNamespace;
using NaucnaCentralaBackend.CamundaExternalTaskSyncNamespace;

namespace NaucnaCentralaBackend
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
            var key = Encoding.ASCII.GetBytes("2124hj421gh214f241");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContextPool<DataContext>(
                options => options.UseSqlServer(Configuration["ConnectionString:NaucnaCentralaDB"]));

            services.AddSingleton<ICamundaExecutor, CamundaExecutor>();
            services.AddSingleton<IExternalTaskWorker, ExternalTaskWorker>();
            services.AddSingleton<ICamundaExternalTaskSync, CamundaExternalTaskSync>();
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {                        
                        var dbContext = context.HttpContext.RequestServices.GetRequiredService<DataContext>();
                        var username = context.Principal.Identity.Name;
                        var user = dbContext.Users.FirstOrDefault(y => y.Username == username);

                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.ApplicationServices.GetService<IExternalTaskWorker>().Start();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseMvc();
        }
    }
}
