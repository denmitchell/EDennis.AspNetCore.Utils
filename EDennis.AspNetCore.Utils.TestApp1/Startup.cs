using EDennis.AspNetCore.Utils.ByteArray;
using EDennis.AspNetCore.Utils.Middleware.Pgp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EDennis.AspNetCore.Utils.TestApp1 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
                options.OutputFormatters.Insert(0, new ByteArrayOutputFormatter());
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.EnvironmentName == "Development") {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UsePgpEncryption(options => {
                options.PublicKeyHeader = "X-PgpPublicKey";
                options.CompressionTypeHeader = "X-PgpCompressionType";
                options.UseArmorHeader = "X-PgpUseArmor";
            });

            app.UseMvc();
        }
    }
}
