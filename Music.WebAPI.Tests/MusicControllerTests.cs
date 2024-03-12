using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;

namespace Music.WebAPI.Tests;

[TestClass]
public class MusicControllerTests
{
    private HttpClient client;
    public MusicControllerTests()
    {
        var server = new TestServer(new WebHostBuilder().UseStartup<StartupBase>());
        client = server.CreateClient();
    }
    [TestMethod]
    public void TestMethod1Async()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "/Api/Music");
        var response = client.SendAsync(request).Result;
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
    [TestMethod]
    [DataRow("1")]
    public void TestMethod2Async(string id)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"/Api/Music/{id}");

        var response = client.SendAsync(request).Result;

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}