using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotNet3dsToolkit;
using DotNetNdsToolkit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectPokemon.Pokedex.Models.Games.Eos;
using ProjectPokemon.Pokedex.Models.Games.Psmd;
using SkyEditor.Core.IO;

namespace ProjectPokemon.Pokedex
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddSingleton<EosDataCollection>(LoadEosDataCollection().GetAwaiter().GetResult());

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
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task<EosDataCollection> LoadEosDataCollection()
        {
            var rom = new NdsRom();
            await rom.OpenFile(Path.Combine(Environment.CurrentDirectory, Configuration.GetValue<string>("EosRom")), new PhysicalIOProvider());

            return await EosDataCollection.LoadEosData(rom);
        }

        private async Task<PsmdDataCollection> LoadPsmdDataCollection()
        {
            var rom = new ThreeDsRom();
            await rom.OpenFile(Path.Combine(Environment.CurrentDirectory, Configuration.GetValue<string>("PsmdRom")), new PhysicalIOProvider());

            return await PsmdDataCollection.LoadPsmdData(rom);
        }
    }
}
