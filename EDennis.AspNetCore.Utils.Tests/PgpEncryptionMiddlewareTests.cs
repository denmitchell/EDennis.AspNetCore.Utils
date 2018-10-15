using EDennis.AspNetCore.Utils.Middleware.Pgp;
using EDennis.AspNetCore.Utils.TestApp1;
using EDennis.AspNetCore.Utils.TestApp1.Utils;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EDennis.AspNetCore.Utils.Tests {
    public class PgpEncryptionMiddlewareTests {

        static HttpClient _client;

        static PgpEncryptionMiddlewareTests() {
            var server = new TestServer(
                WebHost.CreateDefaultBuilder(null)
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }


        [Fact]
        public async Task CheckEncryption() {

            var plaintext = "Mary had a little lamb";

            //ENCRYPT
            var publicKey = Encoding.UTF8.GetBytes(File.ReadAllText("receiver-public.txt"));
            byte[] bytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] encryptedBytes = null;

            using (var outStream = new MemoryStream()) {
                using (var inStream = new MemoryStream(bytes)) {

                    //call encryption with compression
                    PgpCryptor.Encrypt(inStream, outStream,
                        publicKey, CompressionType.GZip, false);
                    encryptedBytes = outStream.ToArray();
                }
            }
            File.WriteAllBytes("mary-receiver.enc", encryptedBytes);


            //DECRYPT
            var encrypted = File.ReadAllBytes("mary-receiver.enc");
            var privateKey = Encoding.UTF8.GetBytes(File.ReadAllText("receiver-private.txt"));

            string recoveredText = null;

            using (var outStream = new MemoryStream()) {
                using (var inStream = new MemoryStream(encrypted)) {

                    //call encryption with compression
                    PgpCryptor.Decrypt(inStream, outStream,
                        privateKey, "Receiver123!@#",
                        CompressionType.GZip,
                        false);

                    var recoveredBytes = outStream.ToArray();
                    recoveredText = Encoding.UTF8.GetString(recoveredBytes);
                }
            }

            //COMPARE DECRYPTED TEXT TO THE ORIGINAL
            Assert.Equal(plaintext, recoveredText);
        }



        [Fact]
        public async Task Get() {

            var plaintext = "ABCDEFG";

            //ENCRYPT
            var publicKey = File.ReadAllText("receiver-public.txt");
            publicKey = publicKey.Replace("\r\n", "\r\n ");

            var request = new HttpRequestMessage();
            request.Headers.Add("X-PgpPublicKey", publicKey);
            request.Headers.Add("X-PgpCompressionType", "GZip");
            request.Headers.Add("X-PgpUseArmor", "false");
            request.RequestUri = new Uri("api/values",UriKind.Relative);

            var response = await _client.SendAsync(request);
            byte[] encrypted = await response.Content.ReadAsByteArrayAsync();

            File.WriteAllBytes("mary-receiver.enc", encrypted);


            //DECRYPT
            encrypted = File.ReadAllBytes("mary-receiver.enc");
            var privateKey = Encoding.UTF8.GetBytes(File.ReadAllText("receiver-private.txt"));

            string recoveredText = null;
            using (var outStream = new MemoryStream()) {
                using (var inStream = new MemoryStream(encrypted)) {
                    //call encryption with compression
                    PgpCryptor.Decrypt(inStream, outStream,
                        privateKey, "Receiver123!@#",
                        CompressionType.GZip,
                        false);
                    var recoveredBytes = outStream.ToArray();
                    recoveredText = Encoding.UTF8.GetString(recoveredBytes);

                }
            }

            //COMPARE DECRYPTED TEXT TO THE ORIGINAL
            Assert.Equal(plaintext, recoveredText);
        }



        [Fact]
        public async Task Post() {

            var plaintext = "Mary had a little lamb";

            //ENCRYPT

            var publicKey = File.ReadAllText("receiver-public.txt");
            publicKey = publicKey.Replace("\r\n", "\r\n ");

            var request = new HttpRequestMessage();
            request.Headers.Add("X-PgpPublicKey", publicKey);
            request.Headers.Add("X-PgpCompressionType", "GZip");
            request.Headers.Add("X-PgpUseArmor", "false");
            request.RequestUri = new Uri("api/values", UriKind.Relative);


            var plaintextBytes = Encoding.UTF8.GetBytes(plaintext);
            var content = new ByteArrayContent(plaintextBytes);

            request.Content = content;
            request.Method = HttpMethod.Post;

            var response = await _client.SendAsync(request);
            byte[] encrypted = await response.Content.ReadAsByteArrayAsync();

            File.WriteAllBytes("mary-receiver.enc", encrypted);



            //DECRYPT

            encrypted = File.ReadAllBytes("mary-receiver.enc");
            var privateKey = Encoding.UTF8.GetBytes(File.ReadAllText("receiver-private.txt"));

            string recoveredText = null;
            using (var outStream = new MemoryStream()) {
                using (var inStream = new MemoryStream(encrypted)) {

                    //call encryption with compression
                    PgpCryptor.Decrypt(inStream, outStream,
                        privateKey, "Receiver123!@#",
                        CompressionType.GZip,
                        false);

                    var recoveredBytes = outStream.ToArray();
                    var unflippedBytes = ByteManipulator.FlipBytes(recoveredBytes);
                    recoveredText = Encoding.UTF8.GetString(unflippedBytes);
                }
            }

            //COMPARE DECRYPTED TEXT TO THE ORIGINAL
            Assert.Equal(plaintext, recoveredText);
        }
    }
}


