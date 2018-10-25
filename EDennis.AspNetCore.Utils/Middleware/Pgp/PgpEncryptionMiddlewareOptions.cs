using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Pgp {
    
    /// <summary>
    /// Encapsulates a set of names for headers that hold the
    /// PGP public key and other parameters
    /// </summary>
    public class PgpEncryptionMiddlewareOptions {

        /// <summary>
        /// The name of the header holding the PGP public key.
        /// Note that the header value must be in ASCII-armor
        /// format and it must pad the end of each line with 
        /// a space.
        /// </summary>
        public string PublicKeyHeader { get; set; } = "X-PgpPublicKey";

        /// <summary>
        /// The type of compression to apply prior to encryption
        /// </summary>
        public string CompressionTypeHeader { get; set; } = "X-PgpCompressionType";

        /// <summary>
        /// Whether to output the encrypted bytes as ASCII armor
        /// (base-64)
        /// </summary>
        public string UseArmorHeader { get; set; } = "X-PgpUseArmor";

        /// <summary>
        /// The name of an optional header that, when present, tells 
        /// middleware to not do anything.
        /// </summary>
        public string BypassMiddlewareHeader { get; set; } = "X-PgpBypass";

    }
}
