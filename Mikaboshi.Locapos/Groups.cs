using Mikaboshi.Locapos.Response;
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

        private readonly string joinUri;
        private readonly string newUri;

        private readonly LocaposClient client;

        internal Groups(LocaposClient client)
        {
            this.client = client;

            var endpointUri = (client.IsBeta ? LocaposClientInternal.ApiUriBeta : LocaposClientInternal.ApiUri) + Endpoint;
            this.joinUri = endpointUri + JoinKey;
            this.newUri = endpointUri + NewKey;
        }

        /// <summary>
        /// 新しくグループを作成します。
        /// </summary>
        /// <returns>グループのハッシュが含まれるレスポンス情報。</returns>
        public async Task<GroupHashResponse> New()
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken!);
            var request = LocaposClientInternal.CreateGetRequest(newUri);
            var response = await http.SendAsync(request);
            var result = new GroupHashResponse();
            await result.SetResponseAsync(response);

            return result;
        }
    }
}
