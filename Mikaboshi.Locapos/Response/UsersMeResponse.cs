using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.Web.Http;
#else
using System.Net.Http;
#endif

namespace Mikaboshi.Locapos.Response
{
    public class UsersMeResponse : BaseResponse
    {
        /// <summary>
        /// 自分自身の情報を取得します。
        /// </summary>
        public UserPositionData Me { get; private set; }

        internal override async Task SetResponseAsync(HttpResponseMessage response)
        {
            await base.SetResponseAsync(response);

            if (!this.Succeeded) return;

            var content = await response.Content.ReadAsStringAsync();
            var me = JsonConvert.DeserializeObject<UserPositionData>(content);
            this.Me = me;
        }
    }
}
