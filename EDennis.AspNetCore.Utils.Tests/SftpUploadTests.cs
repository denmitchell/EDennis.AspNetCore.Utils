using EDennis.AspNetCore.Utils.TestApp2;
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
        public void DummyTest() {
            Assert.True(true);
        }

    }
}
