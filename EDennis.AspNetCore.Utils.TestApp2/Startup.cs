using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Utils.Middleware.ResponseHeader;
using EDennis.AspNetCore.Utils.Middleware.Sftp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EDennis.AspNetCore.Utils.TestApp2 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options=> {
                options.EnableEndpointRouting = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.EnvironmentName == "Development") {
                app.UseDeveloperExceptionPage();
            }

            app.UseSftpUpload(options => {
                options.FileNameHeader = "X-SftpFileName";
                options.HostHeader = "X-SftpHost";
                options.PortHeader = "X-SftpPort";
                options.UserNameHeader = "X-SftpUserName";
                options.PasswordHeader = "X-SftpPassword";
            });

            app.UseResponseHeader(options => {
                options.Headers = new Dictionary<string, string> {
                    { "X-SftpFileName", "testfile" },
                    { "X-SftpHost", "localhost" },
                    { "X-SftpPort", "22" },
                    { "X-SftpUserName", "root" },
                    { "X-SftpPassword", "root" }
                };
            });


            app.UseMvc();
        }
    }
}
