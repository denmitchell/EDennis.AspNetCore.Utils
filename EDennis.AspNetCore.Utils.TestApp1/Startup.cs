using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using EDennis.AspNetCore.Utils.Middleware.Pgp;
using EDennis.AspNetCore.Utils.ByteArray;

namespace EDennis.AspNetCore.Utils.TestApp1 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options => {
                options.OutputFormatters.Insert(0, new ByteArrayOutputFormatter());
            }                
                ).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
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
