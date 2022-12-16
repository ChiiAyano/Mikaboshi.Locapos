日本語は [こちら](README_ja.md)

# Status
## Build
|Branch|Build Status|NuGet Package|
|------|------------|-------------|
|master|![Build status](https://github.com/ChiiAyano/Mikaboshi.Locapos/actions/workflows/publish_nuget.yml/badge.svg)|[![NuGet](https://img.shields.io/nuget/v/Mikaboshi.Locapos.svg)](https://www.nuget.org/packages/Mikaboshi.Locapos/)|

# Mikaboshi.Locapos
This is a Locapos client library.

## Locapos is...
Locapos is the service of sharing your location information to other Locapos users or worldwide users via official website.

[Official website is here (Japanese)](https://locapos.com)

## Client Library Platform
.NET 7

## How to use this library

Authentication and Post
```cs
using Mikaboshi.Locapos;

public class SomeClass
{
    private const string apiKey = ""; // ← Insert api key.
    private LocaposClient client;

    public SomeClass()
    {
        this.client = new LocaposClient();
    }

    public void Authentication()
    {
        var authUri = this.client.GetAuthenticationUri(apiKey, "myapp://callback");

        // Authenticating
        string response = ...;
        this.client.ParseAuthenticationResponse(response);
    }

    public async Task PostAsync(double latitude, double longitude)
    {
        // Basicaly Post
        var result = await this.client.Locations.UpdateAsync(latitude, longitude);

        // Private Mode Post
        var result = await this.client.Locations.UpdateAsync(latitude, longitude, privatePost: true);

        // Group Post
        var groupHash = await this.client.Groups.New();
        var result = await this.client.Locations.UpdateAsync(latitude, longitude, groupId: groupHash.Key);
    }
}
```
