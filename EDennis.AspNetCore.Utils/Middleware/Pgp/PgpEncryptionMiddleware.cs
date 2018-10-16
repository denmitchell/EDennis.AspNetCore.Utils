using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Pgp {

    /// <summary>
    /// This class performs PGP encryption, using a public key
    /// and parameters passed in through HTTP headers.
    /// NOTE: ensure that the public key header uses the
    /// ascii armor (PEM) format with a space at the end of each line
    /// </summary>
    public class PgpEncryptionMiddleware : ResponseMiddleware {

        private readonly PgpEncryptionMiddlewareOptions _options;
        private byte[] _publicKey;
        private CompressionType _compressionType;
        private bool _useArmor;

        /// <summary>
        /// Constructs a new PgpEncryptionMiddleware object
        /// with the required delegate and options
        /// </summary>
        /// <param name="next">delegate used to invoke the next middleware in the pipeline</param>
        /// <param name="options">header names for the public key and params</param>
        public PgpEncryptionMiddleware(
                RequestDelegate next, PgpEncryptionMiddlewareOptions options):
            base(next){ _options = options; }


        /// <summary>
        /// retrieves and stores parameters used for 
        /// the PGP encryption
        /// </summary>
        /// <param name="context">The object through which headers can be retrieved</param>
        protected override void Setup(HttpContext context) {

            //get the PGP public key and params from the headers
            var headers = context.Request.Headers;
            _publicKey = Encoding.UTF8.GetBytes(headers[_options.PublicKeyHeader]);
            _compressionType = new CompressionType().Parse(headers[_options.CompressionTypeHeader]);
            _useArmor = headers[_options.UseArmorHeader].ToString().ToLower() == "true";

            //remove the headers once the keys have been used
            headers.Remove(_options.PublicKeyHeader);
            headers.Remove(_options.CompressionTypeHeader);
            headers.Remove(_options.UseArmorHeader);

        }

        /// <summary>
        /// Transforms the response stream into a PGP-encrypted stream
        /// </summary>
        /// <param name="inStream">pre-encrypted stream</param>
        /// <param name="outStream">encrypted stream</param>
        protected override void Process(Stream inStream, Stream outStream) {
            PgpCryptor.Encrypt(inStream, outStream, _publicKey, _compressionType, _useArmor);
        }


        protected override void Teardown() {}

    }
}
