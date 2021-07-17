using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Platform.IOC;
using System;

namespace TicketSystem
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
            services.AddOptions();
            services.AddControllersWithViews();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();

            var register = new RegisterIOC();
            register.DependencyInjection(
                (interfaceType, imType, life) =>
                {
                    switch (life)
                    {
                        case IocType.Scoped:
                            services.AddScoped(interfaceType, imType);
                            break;

                        case IocType.Singleton:
                            services.AddSingleton(interfaceType, imType);
                            break;

                        case IocType.Transient:
                            services.AddTransient(interfaceType, imType);
                            break;

                        default:
                            services.AddScoped(interfaceType, imType);
                            break;
                    }
                }, "TicketSystem");

            double LoginExpireMinute = this.Configuration.GetValue<double>("LoginExpireMinute");
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
            {
                option.LoginPath = new PathString("/Home/Login"); // 登入頁
                option.LogoutPath = new PathString("/Home/Logout"); // 登出Action                
                option.ExpireTimeSpan = TimeSpan.FromMinutes(LoginExpireMinute); // 沒給預設14天

                option.SlidingExpiration = true;
            });
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
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default",
                            pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
