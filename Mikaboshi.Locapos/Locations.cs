using Mikaboshi.Locapos.Response;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using System.Net.Http;

namespace Mikaboshi.Locapos
{
    /// <summary>
    /// Locapos の位置情報に関する処理を表します。
    /// </summary>
    public class Locations
    {
        private static readonly string locationsUri = LocaposClientInternal.ApiUri + "locations/";
        private static readonly string updateUri = locationsUri + "update";

        private readonly LocaposClient client;

        internal Locations(LocaposClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// Locapos へ位置情報を送信します。
        /// </summary>
        /// <param name="latitude">送信する位置の緯度。</param>
        /// <param name="longitude">送信する位置の経度。</param>
        /// <param name="heading">送信する移動時の向き。真北を 0 とし、0 - 359 または -180 - 0 - 180 のどれかで指定ができ、また null を指定した場合は、相手には前の位置からの推測で表示されます。</param>
        /// <param name="privatePost">Locapos の公開地図に表示するかどうか。</param>
        /// <param name="groupId">任意グループに対して送信する場合はその ID を指定します。</param>
        /// <returns></returns>
        public async Task<BaseResponse> UpdateAsync(double latitude, double longitude, double? heading = null, bool privatePost = false, string groupId = "")
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken);

            var contentsDict = new Dictionary<string, string>
                {
                    { "latitude", latitude.ToString(CultureInfo.InvariantCulture) },
                    { "longitude", longitude.ToString(CultureInfo.InvariantCulture) }
                };
            if (heading.HasValue) contentsDict.Add("heading", heading.Value.ToString(CultureInfo.InvariantCulture));
            if (privatePost) contentsDict.Add("private", "true");
            if (!string.IsNullOrWhiteSpace(groupId)) contentsDict.Add("key", groupId);

            var contents = new FormUrlEncodedContent(contentsDict);
            var request = await LocaposClientInternal.CreatePostRequestAsync(updateUri, contents, true);
            var response = await http.SendAsync(request);

            var result = new BaseResponse();
            await result.SetResponseAsync(response);

            return result;
        }
    }
}
