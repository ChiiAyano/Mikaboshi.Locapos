using System;
using Newtonsoft.Json;
using System.Threading.Tasks;

using System.Net.Http;

namespace Mikaboshi.Locapos.Response
{
    public class GroupHashResponse : BaseResponse
    {
        private class GroupHash
        {
            public string Key { get; set; }
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
            var data = JsonConvert.DeserializeObject<GroupHash>(content);

            this.Key = data?.Key;
        }
    }
}
