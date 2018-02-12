using System.Threading.Tasks;

using System.Net;
using System.Net.Http;

namespace Mikaboshi.Locapos.Response
{
    /// <summary>
    /// Locapos からの、特別な情報を持たない、ベースとなるレスポンスを格納するクラスです。
    /// </summary>
    public class BaseResponse
    {
        protected HttpResponseMessage ResponseMessage { get; private set; }

        /// <summary>
        /// Locapos からの応答コードを取得します。
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }
        /// <summary>
        /// Locapos から成功の応答があったかどうかを取得します。
        /// </summary>
        public bool Succeeded => this.ResponseMessage?.IsSuccessStatusCode ?? false;

        /// <summary>
        /// ベース クラスでは、基本的なレスポンス情報を設定します。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal virtual Task SetResponseAsync(HttpResponseMessage response)
        {
            this.ResponseMessage = response;
            this.StatusCode = response.StatusCode;

#if NETSTANDARD1_4 || NET46
            return Task.CompletedTask;
#else
            return Task.FromResult<object>(null);
#endif
        }
    }
}
