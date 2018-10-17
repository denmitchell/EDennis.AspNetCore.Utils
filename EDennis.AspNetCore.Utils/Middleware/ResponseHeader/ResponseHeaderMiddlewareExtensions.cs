using Microsoft.AspNetCore.Builder;
using System;

namespace EDennis.AspNetCore.Utils.Middleware.ResponseHeader {

    /// <summary>
    /// This class is needed to allow the Configure method to
    /// call app.UseResponseHeader(options=> { ... });
    /// </summary>
    public static class ResponseHeaderMiddlewareExtensions {

        /// <summary>
        /// Invokes ResponseHeaderMiddleware at some point in the pipeline
        /// </summary>
        /// <param name="builder">application builder</param>
        /// <param name="configureOptions">header name options (defaults used, if null)</param>
        /// <returns>a reference to the applicaton builder (for fluent construction)</returns>
        public static IApplicationBuilder UseResponseHeader(
                this IApplicationBuilder builder,
                Action<ResponseHeaderMiddlewareOptions> configureOptions = null) {

            var options = new ResponseHeaderMiddlewareOptions();

            //configure the options, unless configureOptions is null
            configureOptions?.Invoke(options);

            //use the middleware
            return builder.UseMiddleware<ResponseHeaderMiddleware>(options);
        }

    }
}
