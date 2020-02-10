using Microsoft.AspNetCore.Builder;
using System;

namespace EDennis.AspNetCore.Utils.Middleware.Sftp {

    /// <summary>
    /// This class is needed to allow the Configure method to
    /// call app.UseSftpUpload(options=> { ... });
    /// </summary>
    public static class SftpUploadMiddlewareExtensions {

        /// <summary>
        /// Invokes SftpUploadMiddleware at some point in the pipeline
        /// </summary>
        /// <param name="builder">application builder</param>
        /// <param name="configureOptions">header name options (defaults used, if null)</param>
        /// <returns>a reference to the applicaton builder (for fluent construction)</returns>
        public static IApplicationBuilder UseSftpUpload(
                this IApplicationBuilder builder,
                Action<SftpUploadMiddlewareOptions> configureOptions = null) {

            var options = new SftpUploadMiddlewareOptions();

            //configure the options, unless configureOptions is null
            configureOptions?.Invoke(options);

            //use the middleware
            return builder.UseMiddleware<SftpUploadMiddleware>(options);
        }

    }
}
