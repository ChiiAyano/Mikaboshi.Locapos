using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mikaboshi.Locapos.Response
{
    public class GroupHashResponse : BaseResponse
    {
        private class GroupHash
        {
            public string Key { get; init; }
        }

        /// <summary>
        /// グループ ハッシュを取得します。
        /// </summary>
        public string Key { get; private set; }

        internal override async Task SetResponseAsync(HttpResponseMessage response)
        {
            await base.SetResponseAsync(response);

            if (!this.Succeeded) return;

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<GroupHash>(content);

            this.Key = data?.Key;
        }
    }
}
