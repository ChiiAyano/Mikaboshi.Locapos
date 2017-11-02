日本語は [こちら](README_ja.md)

# Status
## Build
|Branch|Build Status|NuGet Package|
|------|------------|-------------|
|master|[![Build status](https://ci.appveyor.com/api/projects/status/08v2a3nv3ne032o0/branch/master?svg=true)](https://ci.appveyor.com/project/ChiiAyano/mikaboshi-locapos/branch/master)|[![NuGet](https://img.shields.io/nuget/v/Mikaboshi.Locapos.svg)](https://www.nuget.org/packages/Mikaboshi.Locapos/)|

# Mikaboshi.Locapos
This is a Locapos client library.

## Locapos is...
Locapos is the service of sharing your location information to other Locapos users or worldwide users via official website.

[Official website is here (Japanese)](https://locapos.com)

## Client Library Platform
.NET Standard 1.4

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
        var result = await this.client.Locations.UpdateAsync(latitude, longitude);
    }
}
```
