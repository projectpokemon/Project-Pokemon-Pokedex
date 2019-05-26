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
using ProjectPokemon.Pokedex.Models.Games;
using ProjectPokemon.Pokedex.Models.Games.Eos;
using ProjectPokemon.Pokedex.Models.Games.Gen7;
using ProjectPokemon.Pokedex.Models.Games.Psmd;
using SkyEditor.Core.IO;
using SkyEditor.IO.FileSystem;

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
                .AddSingleton<DataCollection>(LoadCombinedDataCollection().GetAwaiter().GetResult());

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
            app.UseCookiePolicy(new CookiePolicyOptions {
                CheckConsentNeeded = _ => false
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private async Task<DataCollection> LoadCombinedDataCollection()
        {
            var eosTask = LoadEosDataCollection();
            var psmdTask = LoadPsmdDataCollection();
            var gen7Task = LoadGen7DataCollection();
            var ultraGen7Task = LoadUltraGen7DataCollection();

            var data = new DataCollection
            {
                EosData = await eosTask,
                PsmdData = await psmdTask,
                SMData = await gen7Task,
                UltraSMData = await ultraGen7Task
            };

            data.EosData.ParentCollection = data;
            data.PsmdData.ParentCollection = data;
            data.SMData.ParentCollection = data;
            data.UltraSMData.ParentCollection = data;

            return data;
        }

        private async Task<EosDataCollection> LoadEosDataCollection()
        {
            var rom = new NdsRom();
            await rom.OpenFile(Path.Combine(Environment.CurrentDirectory, Configuration.GetValue<string>("EosRom")), new PhysicalFileSystem());

            return await EosDataCollection.LoadEosData(rom);
        }

        private async Task<PsmdDataCollection> LoadPsmdDataCollection()
        {
            var rom = new ThreeDsRom();
            await rom.OpenFile(Path.Combine(Environment.CurrentDirectory, Configuration.GetValue<string>("PsmdRom")), new PhysicalFileSystem());

            return await PsmdDataCollection.LoadPsmdData(rom);
        }

        private async Task<Gen7DataCollection> LoadGen7DataCollection()
        {
            var path = Path.Combine(Environment.CurrentDirectory, Configuration.GetValue<string>("MoonRom"));
            return await Gen7DataCollection.LoadGen7Data(path, false);
        }

        private async Task<Gen7DataCollection> LoadUltraGen7DataCollection()
        {
            var path = Path.Combine(Environment.CurrentDirectory, Configuration.GetValue<string>("UltraMoonRom"));
            return await Gen7DataCollection.LoadGen7Data(path, true);
        }
    }
}
