English version is [here](README.md)

# Mikaboshi.Locapos
Locapos クライアント ライブラリです。

## Locapos is...
Locapos (ろけぽす) は、自分の位置情報を他のユーザーと共有したり、公式サイトの地図を通して全世界へ位置情報を共有できるサービスです。

[公式サイト](https://locapos.com)

## クライアント ライブラリの実装環境
.NET Standard 1.4

## ライブラリの使い方

認証および位置情報の送信
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