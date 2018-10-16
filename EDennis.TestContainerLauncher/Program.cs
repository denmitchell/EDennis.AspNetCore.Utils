using System;
using System.Diagnostics;
using System.Security;

namespace EDennis.TestContainerLauncher {

    class Program {
        static void Main(string[] args) {
            RunDockerStart();
            Console.WriteLine("Press any key to stop");
            Console.ReadKey();
            RunDockerStop();
        }


        private static void RunDockerStart() {
            var config = new ProcessStartInfo() {
                FileName = "docker",
                Arguments = "start sftp_server",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,

            };
            using (var process = Process.Start(config)) {
                using (var reader = process.StandardOutput) {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
                using (var reader = process.StandardError) {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }

        private static void RunDockerStop() {
            var config = new ProcessStartInfo() {
                FileName = "docker",
                Arguments = "stop sftp_server",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,

            };
            using (var process = Process.Start(config)) {
                using (var reader = process.StandardOutput) {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
                using (var reader = process.StandardError) {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }


        private static SecureString getPasswordFromConsole(String displayMessage) {
            SecureString pass = new SecureString();
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (!char.IsControl(key.KeyChar)) {
                    pass.AppendChar(key.KeyChar);
                    Console.Write("*");
                } else {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0) {
                        pass.RemoveAt(pass.Length - 1);
                        Console.Write("\b \b");
                    }
                }
            }

            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            return pass;
        }


    }
}
