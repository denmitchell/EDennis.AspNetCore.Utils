using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.ResponseHeader {
    
    /// <summary>
    /// Encapsulates a set of names for headers that hold the
    /// PGP public key and other parameters
    /// </summary>
    public class ResponseHeaderMiddlewareOptions {

        /// <summary>
        /// A dictionary of headers to add or replace.
        /// </summary>
        public Dictionary<string,string> Headers { get; set; }

    }
}
