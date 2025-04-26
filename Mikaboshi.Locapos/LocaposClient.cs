using System;
using System.Linq;
using System.Net.Http;

namespace Mikaboshi.Locapos
{
    public class LocaposClient
    {
        private static string AuthUri => LocaposClientInternal.BaseUri + "oauth/authorize?response_type=token&client_id={0}&redirect_uri=";

        /// <summary>
        /// サービスの利用に必要なトークンを取得または設定します。
        /// </summary>
        public ClientToken? ClientToken { get; set; }

        #region 認証

        /// <summary>
        /// ユーザー認証をする URI を取得します。
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        public Uri GetAuthenticationUri(string apiKey, Uri redirectUri)
        {
            var escaped = Uri.EscapeDataString(redirectUri.ToString());
            var uri = new Uri(string.Format(AuthUri, Uri.EscapeDataString(apiKey)) + escaped);

            return uri;
        }

        /// <summary>
        /// ユーザー認証後のレスポンス クエリを解析し、<see cref="ClientToken"/> を生成します。
        /// </summary>
        /// <param name="responseQuery"></param>
        /// <returns></returns>
        public ClientToken ParseAuthenticationResponse(string responseQuery)
        {
            var queryString = responseQuery.Substring(responseQuery.IndexOf('#') + 1).Trim();
            var query = queryString.Split('&').Select(s => s.Split('=')).ToDictionary(x => x[0], y => y[1]);

            var token = new ClientToken { Token = query["access_token"] };

            this.ClientToken = token;

            return token;
        }

        #endregion

        #region API プロパティ

        /// <summary>
        /// 位置情報に関する通信をするためのアクセス ポイント情報を取得します。
        /// </summary>
        public Locations Locations { get; }

        /// <summary>
        /// 自分または現在アクティブなユーザーの情報を取得するためのアクセス ポイント情報を取得します。
        /// </summary>
        public Users Users { get; }

        /// <summary>
        /// グループに関するアクセス ポイント情報を取得します。
        /// </summary>
        public Groups Groups { get; }

        #endregion

        #region 共通処理

        public LocaposClient(HttpClientHandler? clientHandler = null, bool isBeta = false)
        {
            this.Locations = new Locations(this);
            this.Users = new Users(this);
            this.Groups = new Groups(this);

            LocaposClientInternal.ClientHandler = clientHandler;
            LocaposClientInternal.IsBeta = isBeta;
        }

        /// <summary>
        /// トークンが設定されているかを確認し、されていない場合は例外を発生させます。
        /// </summary>
        internal void CheckToken()
        {
            if (string.IsNullOrWhiteSpace(this.ClientToken?.Token))
            {
                throw new TokenNotFoundException();
            }
        }

        #endregion
    }
}
