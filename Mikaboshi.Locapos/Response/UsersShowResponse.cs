﻿using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mikaboshi.Locapos.Response
{
    /// <summary>
    /// 現在アクティブなユーザー一覧のレスポンスを格納するクラスです。
    /// </summary>
    public class UsersShowResponse : BaseResponse
    {
        /// <summary>
        /// 現在アクティブなユーザーを列挙します。
        /// </summary>
        public IEnumerable<UserPositionData>? Users { get; private set; }

        internal override async Task SetResponseAsync(HttpResponseMessage response)
        {
            await base.SetResponseAsync(response);

            if (!this.Succeeded) return;

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<IEnumerable<UserPositionData>>(content);
            this.Users = users;
        }
    }
}
