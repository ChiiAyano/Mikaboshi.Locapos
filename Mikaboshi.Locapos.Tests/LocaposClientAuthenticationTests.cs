using Mikaboshi.Locapos;

namespace Mikaboshi.Locapos.Tests;

public class LocaposClientAuthenticationTests
{
    [Fact]
    public void GetAuthenticationUri_WhenNormalMode_UsesLocaposDomain()
    {
        var client = new LocaposClient(isBeta: false);

        var uri = client.GetAuthenticationUri("api-key", new Uri("mikaboshi://auth"));

        Assert.StartsWith("https://locapos.com/oauth/authorize", uri.ToString());
    }

    [Fact]
    public void GetAuthenticationUri_WhenBetaMode_UsesBetaLocaposDomain()
    {
        var client = new LocaposClient(isBeta: true);

        var uri = client.GetAuthenticationUri("api-key", new Uri("mikaboshi://auth"));

        Assert.StartsWith("https://beta.locapos.com/oauth/authorize", uri.ToString());
    }

    [Fact]
    public void GetAuthenticationUri_ContainsClientIdAndRedirectUri()
    {
        var client = new LocaposClient();

        var uri = client.GetAuthenticationUri("key+space value", new Uri("mikaboshi://auth/callback?x=1&y=2"));

        Assert.Contains("client_id=", uri.ToString());
        Assert.Contains("redirect_uri=", uri.ToString());
    }

    [Fact]
    public void ParseAuthenticationResponse_SetsClientToken()
    {
        var client = new LocaposClient();

        var token = client.ParseAuthenticationResponse("mikaboshi://auth#access_token=abc123&token_type=bearer");

        Assert.Equal("abc123", token.Token);
        Assert.NotNull(client.ClientToken);
        Assert.Equal("abc123", client.ClientToken!.Token);
    }

    [Fact]
    public void ParseAuthenticationResponse_WithUrlPrefix_ParsesFragmentToken()
    {
        var client = new LocaposClient();

        var token = client.ParseAuthenticationResponse("https://locapos.com/callback#access_token=xyz&scope=read");

        Assert.Equal("xyz", token.Token);
    }

    [Fact]
    public void ParseAuthenticationResponse_WithoutHash_StillParsesToken()
    {
        var client = new LocaposClient();

        var token = client.ParseAuthenticationResponse("access_token=abc");

        Assert.Equal("abc", token.Token);
    }
}
