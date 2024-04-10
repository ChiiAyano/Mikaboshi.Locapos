using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mikaboshi.Locapos.Response
{
    public class UsersMeResponse : BaseResponse
    {
        /// <summary>
        /// 自分自身の情報を取得します。
        /// </summary>
        public UserPositionData? Me { get; private set; }

        internal override async Task SetResponseAsync(HttpResponseMessage response)
        {
            await base.SetResponseAsync(response);

            if (!this.Succeeded) return;

            var content = await response.Content.ReadAsStringAsync();
            var me = JsonSerializer.Deserialize<UserPositionData>(content);
            this.Me = me;
        }
    }
}
