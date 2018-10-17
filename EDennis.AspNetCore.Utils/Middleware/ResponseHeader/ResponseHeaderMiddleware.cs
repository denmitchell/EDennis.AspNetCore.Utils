using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EDennis.AspNetCore.Utils.Middleware.ResponseHeader {

    /// <summary>
    /// This class adds headers to an HTTP response.  The class
    /// is configured with default values in the Startup class, 
    /// but these default values are overridden with header
    /// values passed in the HTTP request -- allowing for
    /// per-request variability.
    /// </summary>
    public class ResponseHeaderMiddleware {

        private readonly ResponseHeaderMiddlewareOptions _options;
        private readonly RequestDelegate _next;


        /// <summary>
        /// Constructs a new ResponseHeaderMiddleware object
        /// with the required delegate and options
        /// </summary>
        /// <param name="next">delegate used to invoke the next middleware in the pipeline</param>
        /// <param name="options">a dictionary of new headers to add</param>
        public ResponseHeaderMiddleware(
                RequestDelegate next, ResponseHeaderMiddlewareOptions options) {
            _next = next;
            _options = options;
        }


        /// <summary>
        /// This method is called by the framework when the subclass
        /// is added as middleware to the processing pipeline.
        /// </summary>
        /// <param name="context">The HTTP context, which holds the request and response</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context) {


            //yield to other middleware
            await _next(context);

            //add response headers
            await Task.Run(() => {
                var headers = context.Request.Headers;

                foreach (var key in _options.Headers.Keys) {

                    //if the request headers contain a value
                    //for the target header, do not replace
                    //with the configured default value
                    if (!headers.ContainsKey(key)) {
                        headers.Add(key, _options.Headers[key]);
                    }
                }
            });
        }

    }

}

