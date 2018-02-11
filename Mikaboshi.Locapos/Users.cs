using System;
using Mikaboshi.Locapos.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

#if WINDOWS_UWP
using Windows.Web.Http;
#else
using System.Net.Http;
#endif

namespace Mikaboshi.Locapos
{
    public class Users
    {
        private static readonly string usersUri = LocaposClientInternal.ApiUri + "users/";
        private static readonly string showUri = usersUri + "show";
        private static readonly string meUri = usersUri + "me";
        private static readonly string shareUri = usersUri + "share";
        private static readonly string updateNameUri = usersUri + "update";

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

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken);
            var request = LocaposClientInternal.CreateGetRequest(showUri +
                (!string.IsNullOrWhiteSpace(groupId) ? "?key=" + groupId : string.Empty));

#if WINDOWS_UWP
            var response = await http.SendRequestAsync(request);
#else
            var response = await http.SendAsync(request);
#endif
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

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken);
            var request = LocaposClientInternal.CreateGetRequest(meUri);

#if WINDOWS_UWP
            var response = await http.SendRequestAsync(request);
#else
            var response = await http.SendAsync(request);
#endif

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

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken);
            var request = LocaposClientInternal.CreateGetRequest(shareUri);

#if WINDOWS_UWP
            var response = await http.SendRequestAsync(request);
#else
            var response = await http.SendAsync(request);
#endif

            var result = new GroupHashResponse();
            await result.SetResponseAsync(response);

            return result;
        }

        public async Task<BaseResponse> Update(string screenName)
        {
            this.client.CheckToken();

            var http = LocaposClientInternal.GetHttpClient(this.client.ClientToken);
            var contentDict = new Dictionary<string, string>
                {
                    { "screen_name", screenName }
                };


#if WINDOWS_UWP
            var content = new HttpFormUrlEncodedContent(contentDict);
            var request = LocaposClientInternal.CreatePostRequest(updateNameUri, content);
            var response = await http.SendRequestAsync(request);
#else
            var content = new FormUrlEncodedContent(contentDict);
            var request = await LocaposClientInternal.CreatePostRequestAsync(updateNameUri, content, true);
            var response = await http.SendAsync(request);
#endif
            var result = new BaseResponse();
            await result.SetResponseAsync(response);

            return result;
        }
    }
}
