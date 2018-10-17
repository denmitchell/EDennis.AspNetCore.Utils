using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace EDennis.AspNetCore.Utils.Middleware.Sftp {

    //https://stackoverflow.com/questions/26700765/c-sharp-sftp-upload-files
    public class SftpClient : IDisposable {


        Renci.SshNet.SftpClient _client;


        public void Connect(string host, int port, string userName, string password) {
            _client = new Renci.SshNet.SftpClient(host, port, userName, password);
        }


        public void Disconnect() {
            _client.Dispose();
        }


        public void Upload(Stream inStream, Stream outStream, string fileName) {

            _client.Connect();

            if (_client.IsConnected) {
                _client.BufferSize = 4 * 1024;// bypass Payload error large files
                _client.UploadFile(inStream, fileName);

                if (_client.Exists(fileName)) {
                    var confirmation = new Confirmation {
                        HostName = _client.ConnectionInfo.Host,
                        FileName = fileName,
                        FileSize = _client.GetAttributes(fileName).Size
                    };
                    var outJson = JToken.FromObject(confirmation).ToString();
                    using (var b = new StreamWriter(outStream, Encoding.UTF8, 1000, true)) {
                        b.Write(outJson);
                        b.Flush();
                    }
            }
                    
            } else {
                throw new IOException($"Cannot connect to {_client.ConnectionInfo.Host}");
            }

        }

        public void Download(Stream outStream, string fileName) {

            _client.Connect();

            if (_client.IsConnected) {
                _client.BufferSize = 4 * 1024;// bypass Payload error large files
                _client.DownloadFile(fileName, outStream);
            } else {
                throw new IOException($"Cannot connect to {_client.ConnectionInfo.Host}");
            }
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    if (_client != null)
                        _client.Dispose();
                }
                disposedValue = true;
            }
        }


        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}