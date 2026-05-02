using Mikaboshi.Locapos;

namespace Mikaboshi.Locapos.Tests;

public class LocaposClientTokenTests
{
    [Fact]
    public void CheckToken_WhenClientTokenIsNull_ThrowsTokenNotFoundException()
    {
        var client = new LocaposClient();

        var method = typeof(LocaposClient).GetMethod("CheckToken", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(method);

        var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => method.Invoke(client, null));
        Assert.IsType<TokenNotFoundException>(ex.InnerException);
    }

    [Fact]
    public void CheckToken_WhenClientTokenIsEmpty_ThrowsTokenNotFoundException()
    {
        var client = new LocaposClient { ClientToken = new ClientToken { Token = "" } };

        var method = typeof(LocaposClient).GetMethod("CheckToken", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(method);

        var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => method.Invoke(client, null));
        Assert.IsType<TokenNotFoundException>(ex.InnerException);
    }

    [Fact]
    public void CheckToken_WhenClientTokenIsWhitespace_ThrowsTokenNotFoundException()
    {
        var client = new LocaposClient { ClientToken = new ClientToken { Token = "   " } };

        var method = typeof(LocaposClient).GetMethod("CheckToken", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(method);

        var ex = Assert.Throws<System.Reflection.TargetInvocationException>(() => method.Invoke(client, null));
        Assert.IsType<TokenNotFoundException>(ex.InnerException);
    }

    [Fact]
    public void CheckToken_WhenClientTokenIsValid_DoesNotThrow()
    {
        var client = new LocaposClient { ClientToken = new ClientToken { Token = "valid-token" } };

        var method = typeof(LocaposClient).GetMethod("CheckToken", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        Assert.NotNull(method);

        var exception = Record.Exception(() => method.Invoke(client, null));
        Assert.Null(exception);
    }
}
