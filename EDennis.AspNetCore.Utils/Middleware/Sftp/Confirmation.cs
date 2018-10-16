using System;
using System.Collections.Generic;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Sftp {

    /// <summary>
    /// This class is a container for information that is
    /// provided as part of a confirmed transmission
    /// </summary>
    public class Confirmation {
        public string HostName { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
    }
}
