using Newtonsoft.Json;

namespace Mikaboshi.Locapos.Response
{
    public class UserPositionData
    {
        /// <summary>
        /// このユーザーが認証に使用したサービス
        /// </summary>
        [JsonProperty("provider")]
        public string AuthProvider { get; set; }

        /// <summary>
        /// ユーザー ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        [JsonProperty("name")]
        public string UserName { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        /// <summary>
        /// 経度
        /// </summary>
        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        /// <summary>
        /// 進行方向
        /// </summary>
        [JsonProperty("heading")]
        public double Heading { get; set; }
    }
}
