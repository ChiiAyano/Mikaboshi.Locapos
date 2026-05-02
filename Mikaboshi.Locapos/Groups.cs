using Mikaboshi.Locapos.Response;
using System.Threading;
using System.Threading.Tasks;

namespace Mikaboshi.Locapos
{
    /// <summary>
    /// Locapos グループ送信に関する処理を表します。
    /// </summary>
    public class Groups
    {
        private const string Endpoint = "groups/";
        private const string JoinKey = "join";
        private const string NewKey = "new";

        private string EndpointUri => (client.IsBeta ? internalClient.ApiUriBeta : internalClient.ApiUri) + Endpoint;
        private string JoinUri => this.EndpointUri + JoinKey;
        private string NewUri => this.EndpointUri + NewKey;

        private readonly LocaposClient client;
        private readonly LocaposClientInternal internalClient;

        internal Groups(LocaposClient client, LocaposClientInternal internalClient)
        {
            this.client = client;
            this.internalClient = internalClient;
        }

        /// <summary>
        /// 新しくグループを作成します。
        /// </summary>
        /// <param name="cancellationToken">キャンセルトークン</param>
        /// <returns>グループのハッシュが含まれるレスポンス情報。</returns>
        public async Task<GroupHashResponse> New(CancellationToken cancellationToken = default)
        {
            this.client.CheckToken();

            var http = this.internalClient.GetHttpClient(this.client.ClientToken!);
            var request = this.internalClient.CreateGetRequest(this.NewUri);
            var response = await http.SendAsync(request, cancellationToken);
            var result = new GroupHashResponse();
            await result.SetResponseAsync(response);

            return result;
        }
    }
}
