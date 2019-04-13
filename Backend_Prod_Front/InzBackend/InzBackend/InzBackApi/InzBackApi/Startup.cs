using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InzBackCore.Repositories;
using InzBackInfrastructure.InzBackDb;
using InzBackInfrastructure.Mappers;
using InzBackInfrastructure.Repositories;
using InzBackInfrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;


namespace InzBackApi
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

            //var config2 = Configuration.GetConnectionString("DefaultConnection"); //tak moge sobie sprawdzic czy nie wyrzuca mi jakiegos nulla i czy zwraca to co chce
            services.AddDbContext<InzBackContext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))); //dodajemy baze danych do kontenera zeby mozna bylo pozniej z niej korzysta uzywajac Dependency injection
            services.AddMvc().AddJsonOptions(x => x.SerializerSettings.Formatting =Formatting.Indented);//odpowiada za formatke wyswietlanego Jsona jako rezultat w przegladarce(zeby bylo uporządkowane a nie w sposob ciagly)
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCors(options =>
            {
                options.AddPolicy("AllowClientOrigin",
                    builder => builder.WithOrigins("http://localhost:49152").AllowAnyOrigin()
            .AllowAnyMethod()
            .WithExposedHeaders("content-disposition")
            .AllowAnyHeader()
            .AllowCredentials()
            .SetPreflightMaxAge(TimeSpan.FromSeconds(3600)));
            });

            services.AddScoped<IPlayerRepository, PlayerRepository>(); //addscoped startuje z kazdym wywolaniem http
            services.AddScoped<IMatchRepository, MatchRepository>();
            //dzieki temu wstrzykniety interfejs przypisze sobie implementacje metod z klasyRepozytoria.
            services.AddSingleton(AutoMapperConfig.Initialize());   //addsigleton uruchamia sie tylko raz przy starcie
            //programu a to wystarzy bo przeciez konfiguracja mappera nam sie nie zmieni
            //wiec nalozy nam inicjalizacje ktora jest zawarta w klasie ktorastworzylismy AUtomapperconfig
            services.AddScoped<IPlayerService, PlayerService>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });



            services.AddScoped<IMatchService,MatchService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) => {  // dodany wycinek kodu aby przy odswieżaniu strony nie wyrzucalo tylko wracalo do pliku index
                await next();

                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseStaticFiles();   // to uruchamia pliki frontu
            app.UseDefaultFiles(new DefaultFilesOptions
            {
                DefaultFileNames = new
                List<string> { "index.html" }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseCors("AllowClientOrigin");


            app.UseMvc();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller}/{action}/{id?}",
                defaults: new { controller = "Players", action = "GetAllPlayers" }
            );
            });




            //app.UseCors(options => options.WithOrigins("https://localhost:49152", "https://localhost:44356").AllowAnyMethod());
            //app.UseMvc();
        }
    }
}
