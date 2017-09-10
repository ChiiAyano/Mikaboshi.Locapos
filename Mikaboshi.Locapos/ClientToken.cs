using System;

namespace Mikaboshi.Locapos
{
    public class ClientToken
    {
        /// <summary>
        /// 認証に使用するトークンを取得または設定します。
        /// </summary>
        public string Token { get; set; }
        /// <summary>
        /// トークンの期限を取得します。
        /// </summary>
        public DateTimeOffset ExpireDate { get; internal set; }
        /// <summary>
        /// トークンが期限切れになったときの再発行に使用するトークンを取得します。
        /// </summary>
        public string RefreshToken { get; internal set; }
    }
}
