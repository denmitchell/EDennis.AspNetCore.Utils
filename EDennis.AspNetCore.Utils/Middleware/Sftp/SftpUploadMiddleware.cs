using EDennis.AspNetCore.Utils.Middleware.Sftp;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Sftp {

    /// <summary>
    /// This class performs PGP encryption, using a public key
    /// and parameters passed in through HTTP headers.
    /// NOTE: ensure that the public key header uses the
    /// ascii armor (PEM) format with a space at the end of each line
    /// </summary>
    public class SftpUploadMiddleware : ResponseMiddleware {

        private readonly SftpUploadMiddlewareOptions _options;

        private SftpClient _client;
        private string _fileName;

        /// <summary>
        /// Constructs a new SftpUploadMiddleware object
        /// with the required delegate and options
        /// </summary>
        /// <param name="next">delegate used to invoke the next middleware in the pipeline</param>
        /// <param name="options">header names for the connection params</param>
        public SftpUploadMiddleware(
                RequestDelegate next, SftpUploadMiddlewareOptions options):
            base(next){ _options = options; }


        /// <summary>
        /// Sets up the SFTP connection, using parameters retrieved
        /// from HTTP headers
        /// </summary>
        /// <param name="context">The object through which headers can be retrieved</param>
        protected override void Setup(HttpContext context) {

            //get the PGP public key and params from the headers
            var headers = context.Request.Headers;
            _fileName = headers[(_options.FileNameHeader)]; //store the file name
            var host = headers[_options.HostHeader];
            var port = int.Parse(headers[_options.PortHeader]);
            var username = headers[_options.UserNameHeader];
            var password = headers[_options.PasswordHeader];

            //remove the headers once the keys have been used
            headers.Remove(_options.FileNameHeader);
            headers.Remove(_options.HostHeader);
            headers.Remove(_options.PortHeader);
            headers.Remove(_options.UserNameHeader);
            headers.Remove(_options.PasswordHeader);

            //create a new SftpClient and connect to the server
            _client = new SftpClient();
            _client.Connect(host, port, username, password);

        }

        /// <summary>
        /// Sends the response stream to an SFTP site as a file
        /// </summary>
        /// <param name="inStream">pre-encrypted stream</param>
        /// <param name="outStream">encrypted stream</param>
        protected override void Process(Stream inStream, Stream outStream) {
            _client.Upload(inStream, outStream, _fileName);
        }


        /// <summary>
        /// Disconnect the SFTP client from the server
        /// </summary>
        protected override void Teardown() {
            _client.Disconnect();
        }

    }
}
