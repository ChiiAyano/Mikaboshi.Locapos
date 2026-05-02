using System.Text.Json;
using Mikaboshi.Locapos.Response;

namespace Mikaboshi.Locapos.Tests;

public class UserPositionDataTests
{
    [Fact]
    public void Deserialize_WhenValidJson_MapsAllProperties()
    {
        var json = """
            {
              "provider": "github",
              "id": "user-1",
              "name": "Kasumi",
              "latitude": 35.5,
              "longitude": 139.5,
              "heading": 270.0
            }
            """;

        var target = JsonSerializer.Deserialize<UserPositionData>(json);

        Assert.NotNull(target);
        Assert.Equal("github", target!.AuthProvider);
        Assert.Equal("user-1", target.ID);
        Assert.Equal("Kasumi", target.UserName);
        Assert.Equal(35.5, target.Latitude);
        Assert.Equal(139.5, target.Longitude);
        Assert.Equal(270.0, target.Heading);
    }

    [Fact]
    public void Deserialize_WhenMissingOptionalStrings_LeavesNulls()
    {
        var json = """
            {
              "latitude": 1.0,
              "longitude": 2.0,
              "heading": 3.0
            }
            """;

        var target = JsonSerializer.Deserialize<UserPositionData>(json);

        Assert.NotNull(target);
        Assert.Null(target!.AuthProvider);
        Assert.Null(target.ID);
        Assert.Null(target.UserName);
        Assert.Equal(1.0, target.Latitude);
        Assert.Equal(2.0, target.Longitude);
        Assert.Equal(3.0, target.Heading);
    }
}
