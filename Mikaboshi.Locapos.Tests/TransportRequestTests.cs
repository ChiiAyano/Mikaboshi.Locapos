using System.Net;
using System.Net.Http;
using System.Text;
using Mikaboshi.Locapos.Response;

namespace Mikaboshi.Locapos.Tests;

public class TransportRequestTests
{
    private sealed class CaptureHandler(HttpResponseMessage response) : HttpMessageHandler
    {
        public HttpRequestMessage? LastRequest { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            this.LastRequest = request;
            return Task.FromResult(response);
        }
    }

    private static LocaposClient CreateClient(CaptureHandler handler, bool isBeta = false)
    {
        var httpHandler = new HttpClientHandler();
        var internalClient = new LocaposClientInternal(httpHandler);

        var field = typeof(LocaposClientInternal).GetField("http", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        field!.SetValue(internalClient, new HttpClient(handler));

        var client = (LocaposClient)Activator.CreateInstance(typeof(LocaposClient), System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, [internalClient, isBeta], null)!;
        client.ClientToken = new ClientToken { Token = "token" };
        return client;
    }

    [Fact]
    public async Task UsersShow_WhenGroupSpecified_AddsKeyQuery()
    {
        var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("[]")
        });
        var client = CreateClient(handler);

        await client.Users.Show("group1");

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("users/show?key=group1", handler.LastRequest.RequestUri!.ToString());
        Assert.Equal("Bearer", handler.LastRequest.Headers.Authorization!.Scheme);
        Assert.Equal("token", handler.LastRequest.Headers.Authorization!.Parameter);
    }

    [Fact]
    public async Task UsersMe_UsesMeEndpoint()
    {
        var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        });
        var client = CreateClient(handler);

        await client.Users.Me();

        Assert.NotNull(handler.LastRequest);
        Assert.Contains("users/me", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task UsersShare_UsesShareEndpoint()
    {
        var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        });
        var client = CreateClient(handler);

        await client.Users.Share();

        Assert.NotNull(handler.LastRequest);
        Assert.Contains("users/share", handler.LastRequest!.RequestUri!.ToString());
    }

    [Fact]
    public async Task UsersUpdate_PostsScreenName()
    {
        var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        });
        var client = CreateClient(handler);

        await client.Users.Update("kasumin");

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        var body = await handler.LastRequest.Content!.ReadAsStringAsync();
        Assert.Contains("screen_name=kasumin", body);
    }

    [Fact]
    public async Task LocationsUpdate_PostsExpectedParameters()
    {
        var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{}")
        });
        var client = CreateClient(handler, isBeta: true);

        await client.Locations.UpdateAsync(35.1, 139.2, heading: 90, privatePost: true, groupId: "group1", deadReckoning: true);

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Post, handler.LastRequest!.Method);
        Assert.Contains("beta.locapos.com/api/locations/update", handler.LastRequest.RequestUri!.ToString());

        var body = await handler.LastRequest.Content!.ReadAsByteArrayAsync();
        Assert.NotEmpty(body);
        Assert.Contains("gzip", handler.LastRequest.Content.Headers.ContentEncoding);
    }

    [Fact]
    public async Task GroupsNew_UsesNewEndpoint()
    {
        var handler = new CaptureHandler(new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"Key\":\"abc\"}")
        });
        var client = CreateClient(handler);

        var result = await client.Groups.New();

        Assert.NotNull(handler.LastRequest);
        Assert.Equal(HttpMethod.Get, handler.LastRequest!.Method);
        Assert.Contains("groups/new", handler.LastRequest.RequestUri!.ToString());
        Assert.Equal("abc", result.Key);
    }
}
