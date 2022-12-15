using System.Text.Json.Serialization;

namespace Mikaboshi.Locapos.Response
{
    public class UserPositionData
    {
        /// <summary>
        /// このユーザーが認証に使用したサービス
        /// </summary>
        [JsonPropertyName("provider")]
        public string AuthProvider { get; init; }

        /// <summary>
        /// ユーザー ID
        /// </summary>
        [JsonPropertyName("id")]
        public string ID { get; init; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [JsonPropertyName("name")]
        public string UserName { get; init; }

        /// <summary>
        /// 緯度
        /// </summary>
        [JsonPropertyName("latitude")]
        public double Latitude { get; init; }

        /// <summary>
        /// 経度
        /// </summary>
        [JsonPropertyName("longitude")]
        public double Longitude { get; init; }

        /// <summary>
        /// 進行方向
        /// </summary>
        [JsonPropertyName("heading")]
        public double Heading { get; init; }
    }
}
