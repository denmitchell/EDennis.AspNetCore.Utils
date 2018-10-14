using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Utils.ByteArray {

    /// <summary>
    /// This class is needed for use with returning a byte array from an 
    /// HttpClient via HttpResponseMessage.Content.ReadAsByteArrayAsync()
    /// 
    /// Use this in the Startup class as such:
    /// services.AddMvc(options=> {
    ///   options.OutputFormatters.Insert(0,new ByteArrayOutputFormatter());
    /// }) ...
    /// 
    /// This class is based upon https://github.com/danielearwicker/ByteArrayFormatters
    /// 
    /// </summary>
    public class ByteArrayOutputFormatter : OutputFormatter {

        /// <summary>
        /// Instantiates a new ByteArrayOutputFormatter object
        /// and adds a media type relevant to byte arrays
        /// </summary>
        public ByteArrayOutputFormatter() {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue(new StringSegment("application/octet-stream")));
        }

        /// <summary>
        /// Determines if the current OutputFormatter is appropriate for writing the data
        /// </summary>
        /// <param name="type">The data type to evaluate</param>
        /// <returns>true if the OutputFormatter can be used to write output</returns>
        protected override bool CanWriteType(Type type) => type == typeof(byte[]);

        /// <summary>
        /// Writes a byte array to the response stream
        /// </summary>
        /// <param name="context">context that holds information relevant to writing the response</param>
        /// <returns></returns>
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context) {

            //get the byte array to output
            var outBytes = (byte[])context.Object;
            
            //get a reference to the response body
            var body = context.HttpContext.Response.Body;

            //write the byte array to the response body
            return body.WriteAsync(outBytes, 0, outBytes.Length);
        }

    }
}
