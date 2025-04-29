using Mikaboshi.Locapos.Response;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mikaboshi.Locapos
{
    public class Users
    {
        private const string Endpoint = "users/";
        private const string ShowKey = "show";
        private const string MeKey = "me";
        private const string ShareKey = "share";
        private const string UpdateNameKey = "update";

        private string EndpointUri => (client.IsBeta ? LocaposClientInternal.ApiUriBeta : LocaposClientInternal.ApiUri) + Endpoint;
        private string ShowUri => this.EndpointUri + ShowKey;
        private string MeUri => this.EndpointUri + MeKey;
        private string ShareUri => this.EndpointUri + ShareKey;
        private string UpdateNameUri => this.EndpointUri + UpdateNameKey;

        private readonly LocaposClient client;

        internal Users(LocaposClient client)
        {
            this.client = client;
        }

        /// <summary>
        /// 現在アクティブなユーザーの情報を取得します。<paramref name="groupId"/> を使用して、該当するグループの絞り込みをおこないます。
        /// </summary>
        /// <param name="groupId">指定した場合、該当するグループ ID でアクティブなユーザー一覧を取得します。</param>
        /// <returns></returns>
        public async Task<UsersShowResponse> Show(string groupId = "")
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken!);
            var request = LocaposClientInternal.CreateGetRequest(this.ShowUri +
                (!string.IsNullOrWhiteSpace(groupId) ? "?key=" + groupId : string.Empty));
            var response = await http.SendAsync(request);
            var result = new UsersShowResponse();
            await result.SetResponseAsync(response);

            return result;
        }

        /// <summary>
        /// 自分自身の情報を取得します。
        /// </summary>
        /// <returns></returns>
        public async Task<UsersMeResponse> Me()
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken!);
            var request = LocaposClientInternal.CreateGetRequest(this.MeUri);
            var response = await http.SendAsync(request);
            var result = new UsersMeResponse();
            await result.SetResponseAsync(response);

            return result;
        }

        /// <summary>
        /// 自分自身を示す暗黙的なグループ ハッシュを取得します。
        /// </summary>
        /// <returns></returns>
        public async Task<GroupHashResponse> Share()
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken!);
            var request = LocaposClientInternal.CreateGetRequest(this.ShareUri);
            var response = await http.SendAsync(request);
            var result = new GroupHashResponse();
            await result.SetResponseAsync(response);

            return result;
        }

        public async Task<BaseResponse> Update(string screenName)
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken!);
            var contentDict = new Dictionary<string, string>
                {
                    { "screen_name", screenName }
                };

            var content = new FormUrlEncodedContent(contentDict);

            var request = await LocaposClientInternal.CreatePostRequestAsync(this.UpdateNameUri, content);
            var response = await http.SendAsync(request);
            var result = new BaseResponse();
            await result.SetResponseAsync(response);

            return result;
        }
    }
}
