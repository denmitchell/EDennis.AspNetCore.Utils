using EDennis.AspNetCore.Utils.Middleware.Sftp;
using EDennis.AspNetCore.Utils.TestApp2;
using EDennis.NetCoreTestingUtilities.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace EDennis.AspNetCore.Utils.Tests {


    public class SftpUploadTests : IClassFixture<SftpTestFixture> {

        private ITestOutputHelper _output;
        private SftpTestFixture _fixture;

        public SftpUploadTests(SftpTestFixture fixture, ITestOutputHelper output) {
            _output = output;
            _fixture = fixture;
        }

        static HttpClient _client;

        static SftpUploadTests() {
            var server = new TestServer(
                WebHost.CreateDefaultBuilder(null)
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Fact]
        public async void Get() {
            var plaintext = "ABCDEFG";
            var expectedSize = Encoding.UTF8.GetByteCount(plaintext);

            var request = new HttpRequestMessage();
            request.Headers.Add("X-SftpFileName", "ABCDEFG.txt");
            request.RequestUri = new Uri("api/values", UriKind.Relative);
            request.Method = HttpMethod.Get;

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var confirmation = new Confirmation().FromJsonString(content);

            //Examine Confirmation variables
            Assert.Equal("localhost", confirmation.HostName);
            Assert.Equal("ABCDEFG.txt", confirmation.FileName);
            Assert.Equal(expectedSize, confirmation.FileSize);
        }


        [Fact]
        public async void Post() {
            var plaintext = "{\"LastName\":\"Dennis\",\"FirstName\":\"James\"}";
            var expectedSize = Encoding.UTF8.GetByteCount(plaintext);

            var request = new HttpRequestMessage();
            request.Headers.Add("X-SftpFileName", "DennisJames.txt");
            request.RequestUri = new Uri("api/values", UriKind.Relative);
            request.Content = new StringContent(plaintext,Encoding.UTF8,"application/json");
            request.Method = HttpMethod.Post;

            var response = await _client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            var confirmation = new Confirmation().FromJsonString(content);

            //Examine Confirmation variables
            Assert.Equal("localhost", confirmation.HostName);
            Assert.Equal("DennisJames.txt", confirmation.FileName);
            Assert.Equal(expectedSize, confirmation.FileSize);
        }
    }

}
