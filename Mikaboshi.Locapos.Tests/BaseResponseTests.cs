using System.Net;
using System.Net.Http;
using Mikaboshi.Locapos.Response;

namespace Mikaboshi.Locapos.Tests;

public class BaseResponseTests
{
    [Fact]
    public async Task SetResponseAsync_WhenSuccessResponse_SetsStatusCodeContentAndSucceeded()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("{\"result\":\"ok\"}")
        };
        var target = new BaseResponse();

        await target.SetResponseAsync(response);

        Assert.Equal(HttpStatusCode.OK, target.StatusCode);
        Assert.Equal("{\"result\":\"ok\"}", target.Content);
        Assert.True(target.Succeeded);
    }

    [Fact]
    public async Task SetResponseAsync_WhenFailureResponse_SetsStatusCodeContentAndSucceededFalse()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("bad request")
        };
        var target = new BaseResponse();

        await target.SetResponseAsync(response);

        Assert.Equal(HttpStatusCode.BadRequest, target.StatusCode);
        Assert.Equal("bad request", target.Content);
        Assert.False(target.Succeeded);
    }

    [Fact]
    public void Constructor_WhenExceptionSpecified_SetsException()
    {
        var ex = new InvalidOperationException("failure");

        var target = new BaseResponse(ex);

        Assert.Same(ex, target.Exception);
    }

    [Fact]
    public void Constructor_WhenDefault_ExceptionIsNull()
    {
        var target = new BaseResponse();

        Assert.Null(target.Exception);
    }
}
