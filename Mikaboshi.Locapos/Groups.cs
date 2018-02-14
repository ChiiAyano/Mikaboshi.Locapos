using Mikaboshi.Locapos.Response;
using System.Threading.Tasks;

namespace Mikaboshi.Locapos
{
    /// <summary>
    /// Locapos グループ送信に関する処理を表します。
    /// </summary>
    public class Groups
    {
        private static readonly string groupsUri = LocaposClientInternal.ApiUri + "groups/";
        private static readonly string joinUri = groupsUri + "join";
        private static readonly string newUri = groupsUri + "new";

        private readonly LocaposClient client;

        internal Groups(LocaposClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// 新しくグループを作成します。
        /// </summary>
        /// <returns>グループのハッシュが含まれるレスポンス情報。</returns>
        public async Task<GroupHashResponse> New()
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken);
            var request = LocaposClientInternal.CreateGetRequest(newUri);

            var response = await http.SendAsync(request);

            var result = new GroupHashResponse();
            await result.SetResponseAsync(response);

            return result;
        }
    }
}
