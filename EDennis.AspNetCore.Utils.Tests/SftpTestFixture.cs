using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace EDennis.AspNetCore.Utils.Tests {
    public class SftpTestFixture : IDisposable{

        // NOTE: You may have to restart Docker service upon reboot of Win10 machine
        // SEE: https://github.com/docker/for-win/issues/1038

        private const string SFTP_SERVER_DOCKER_CONTAINER_NAME = "sftp_server";
        private const int SECONDS_TO_WAIT_FOR_CONTAINER_STARTUP = 3;

        public SftpTestFixture() {
            RunDockerStart();
            Thread.Sleep(1000 * SECONDS_TO_WAIT_FOR_CONTAINER_STARTUP);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    RunDockerStop();
                }
                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            Dispose(true);
        }
        #endregion


        private static void RunDockerStart() {
            var config = new ProcessStartInfo() {
                FileName = "docker",
                Arguments = $"start {SFTP_SERVER_DOCKER_CONTAINER_NAME}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,

            };
            using (var process = Process.Start(config)) {
                using (var reader = process.StandardOutput) {
                    string result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }
                using (var reader = process.StandardError) {
                    string result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }
            }
        }

        private static void RunDockerStop() {
            var config = new ProcessStartInfo() {
                FileName = "docker",
                Arguments = $"stop {SFTP_SERVER_DOCKER_CONTAINER_NAME}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,

            };
            using (var process = Process.Start(config)) {
                using (var reader = process.StandardOutput) {
                    string result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }
                using (var reader = process.StandardError) {
                    string result = reader.ReadToEnd();
                    Debug.WriteLine(result);
                }
            }
        }

    }
}
