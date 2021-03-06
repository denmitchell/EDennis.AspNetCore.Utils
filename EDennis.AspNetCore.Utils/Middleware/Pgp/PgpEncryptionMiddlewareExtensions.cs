﻿using Microsoft.AspNetCore.Builder;
using System;

namespace EDennis.AspNetCore.Utils.Middleware.Pgp {

    /// <summary>
    /// This class is needed to allow the Configure method to
    /// call app.UsePgpEncryption(options=> { ... });
    /// </summary>
    public static class PgpEncryptionMiddlewareExtensions {

        /// <summary>
        /// Invokes PgpEncryptionMiddleware at some point in the pipeline
        /// </summary>
        /// <param name="builder">application builder</param>
        /// <param name="configureOptions">header name options (defaults used, if null)</param>
        /// <returns>a reference to the applicaton builder (for fluent construction)</returns>
        public static IApplicationBuilder UsePgpEncryption(
                this IApplicationBuilder builder,
                Action<PgpEncryptionMiddlewareOptions> configureOptions = null) {

            var options = new PgpEncryptionMiddlewareOptions();

            //configure the options, unless configureOptions is null
            configureOptions?.Invoke(options);

            //use the middleware
            return builder.UseMiddleware<PgpEncryptionMiddleware>(options);
        }

    }
}
