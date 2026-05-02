using System.Net;
using System.Net.Http;
using Mikaboshi.Locapos.Response;

namespace Mikaboshi.Locapos.Tests;

public class ResponseParsingTests
{
    [Fact]
    public async Task UsersShowResponse_WhenSuccess_ParsesUsers()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""
                [
                  { "provider": "x", "id": "1", "name": "Alice", "latitude": 35.0, "longitude": 139.0, "heading": 90.0 }
                ]
                """)
        };
        var target = new UsersShowResponse();

        await target.SetResponseAsync(response);

        var user = Assert.Single(target.Users!);
        Assert.Equal("1", user.ID);
        Assert.Equal("Alice", user.UserName);
        Assert.Equal(35.0, user.Latitude);
        Assert.Equal(139.0, user.Longitude);
        Assert.Equal(90.0, user.Heading);
    }

    [Fact]
    public async Task UsersShowResponse_WhenFailure_DoesNotParseUsers()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("error")
        };
        var target = new UsersShowResponse();

        await target.SetResponseAsync(response);

        Assert.Null(target.Users);
    }

    [Fact]
    public async Task UsersMeResponse_WhenSuccess_ParsesMe()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""
                { "provider": "x", "id": "me", "name": "Kasumin", "latitude": 35.1, "longitude": 139.1, "heading": 180.0 }
                """)
        };
        var target = new UsersMeResponse();

        await target.SetResponseAsync(response);

        Assert.NotNull(target.Me);
        Assert.Equal("me", target.Me!.ID);
        Assert.Equal("Kasumin", target.Me.UserName);
    }

    [Fact]
    public async Task UsersMeResponse_WhenFailure_DoesNotParseMe()
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent("error")
        };
        var target = new UsersMeResponse();

        await target.SetResponseAsync(response);

        Assert.Null(target.Me);
    }

    [Fact]
    public async Task GroupHashResponse_WhenSuccess_ParsesKey()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("""
                { "Key": "group-hash" }
                """)
        };
        var target = new GroupHashResponse();

        await target.SetResponseAsync(response);

        Assert.Equal("group-hash", target.Key);
    }

    [Fact]
    public async Task GroupHashResponse_WhenFailure_DoesNotParseKey()
    {
        var response = new HttpResponseMessage(HttpStatusCode.Forbidden)
        {
            Content = new StringContent("forbidden")
        };
        var target = new GroupHashResponse();

        await target.SetResponseAsync(response);

        Assert.Null(target.Key);
    }
}
