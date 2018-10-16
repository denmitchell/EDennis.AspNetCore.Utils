using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Sftp {
    
    /// <summary>
    /// Encapsulates a set of names for headers that hold the
    /// PGP public key and other parameters
    /// </summary>
    public class SftpUploadMiddlewareOptions {

        /// <summary>
        /// The filename to be used for the uploaded file.
        /// </summary>
        public string FileNameHeader { get; set; } = "X-FileName";

        /// <summary>
        /// The name of the header holding the SFTP server's host name
        /// </summary>
        public string HostHeader { get; set; } = "X-SftpHost";

        /// <summary>
        /// The name of the header holding the SFTP server's port number
        /// </summary>
        public string PortHeader { get; set; } = "X-SftpPort";

        /// <summary>
        /// The name of the header holding the user name for the SFTP login
        /// </summary>
        public string UserNameHeader { get; set; } = "X-SftpUserName";

        /// <summary>
        /// The name of the header holding the password for the SFTP login
        /// </summary>
        public string PasswordHeader { get; set; } = "X-SftpPassword";

    }
}
