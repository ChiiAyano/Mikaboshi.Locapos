using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

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
        /// 例外が発生した場合、発生した例外を取得します。
        /// </summary>
        public Exception Exception { get; private set; }

        public BaseResponse()
        {

        }

        public BaseResponse(Exception ex)
        {
            this.Exception = ex;
        }

        /// <summary>
        /// ベース クラスでは、基本的なレスポンス情報を設定します。
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        internal virtual Task SetResponseAsync(HttpResponseMessage response)
        {
            this.ResponseMessage = response;
            this.StatusCode = response.StatusCode;

            return Task.CompletedTask;
        }
    }
}
