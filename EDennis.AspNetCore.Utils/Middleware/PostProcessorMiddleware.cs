﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Utils.Middleware {

    /// <summary>
    /// This class is a base class for middleware that
    /// transforms or otherwise processes a response body.
    /// The class is useful because there is some non-obvious
    /// code required to transform and return a response body
    /// in the ASP.NET Core processing pipeline.
    /// 
    /// This class is based upon https://exceptionnotfound.net/using-middleware-to-log-requests-and-responses-in-asp-net-core/
    ///      and informed by https://www.billbogaiv.com/posts/using-aspnet-cores-middleware-to-modify-response-body
    ///          and https://forums.asp.net/t/2137853.aspx
    /// </summary>
    public abstract class PostProcessorMiddleware {

        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructs a new instance of this class.  Provide
        /// your own constructor if you wish to dependency
        /// inject one or more Singletons into this class.
        /// </summary>
        /// <param name="next"></param>
        public PostProcessorMiddleware(RequestDelegate next) {
            _next = next;
        }

        /// <summary>
        /// Implement this method in order to perform some
        /// setup operations (e.g., database queries) prior 
        /// to doing the actual transformation of the 
        /// response body.
        /// </summary>
        /// <param name="context">The HTTP context object</param>
        protected abstract void SetupTransformation(HttpContext context);

        /// <summary>
        /// Implement this method in order to transform the
        /// response body after it has been generated by the
        /// relevant controller action method.
        /// </summary>
        /// <param name="inStream">The input stream that delivers the pre-transformed response content</param>
        /// <param name="outStream">The transformed stream to copy to the response body</param>
        protected abstract void Transform(Stream inStream, Stream outStream);


        /// <summary>
        /// This method is called by the framework when the subclass
        /// is added as middleware to the processing pipeline.
        /// </summary>
        /// <param name="context">The HTTP context, which holds the request and response</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context) {

            //create reference to original stream, which must be returned
            var officialBodyStream = context.Response.Body;

            //generate a new stream for internal processing, 
            //and attach it to the response body object
            using (var delegatedBodyStream = new MemoryStream()) {
                context.Response.Body = delegatedBodyStream;

                //yield to other middleware
                await _next(context);

                //read the delegated stream into a method that does
                //a transformation and writes to an output stream
                using (var transformOutputStream = new MemoryStream()) {

                    //rewind stream
                    delegatedBodyStream.Seek(0, SeekOrigin.Begin);

                    //asynchronously setup and invoke the transformation
                    await Task.Run(() => { SetupTransformation(context); });
                    await Task.Run(() => { Transform(delegatedBodyStream, transformOutputStream); });

                    //rewind stream
                    transformOutputStream.Seek(0, SeekOrigin.Begin);

                    //write to the official body stream, which is the
                    //only way to return a response
                    await transformOutputStream.CopyToAsync(officialBodyStream);
                }
            }
        }

    }
}
