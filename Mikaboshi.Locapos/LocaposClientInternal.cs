using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Web.Http;
#else
using System.Net.Http;
#endif

namespace Mikaboshi.Locapos
{
    internal class LocaposClientInternal
    {
        internal static string BaseUri => "https://locapos.com/";
        internal static string ApiUri => BaseUri + "api/";

        private static HttpClient http;

        internal static HttpClient GetHttpClient(ClientToken token)
        {
            if (http != null) return http;

#if WINDOWS_UWP
            http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Bearer", token.Token);
#else
            var handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.Deflate | System.Net.DecompressionMethods.GZip
            };

            http = new HttpClient(handler);

            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);

#endif

            return http;
        }

        internal static HttpRequestMessage CreateGetRequest(string uri)
        {
            return CreateGetRequest(new Uri(uri));
        }

        internal static HttpRequestMessage CreateGetRequest(Uri uri)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri,
            };

            return request;
        }

#if WINDOWS_UWP
        internal static HttpRequestMessage CreatePostRequest(string uri, IHttpContent content)
        {
            return CreatePostRequest(new Uri(uri), content);
        }

        internal static HttpRequestMessage CreatePostRequest(Uri uri, IHttpContent content)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = content
            };

            return request;
        }
#else
        internal async static Task<HttpRequestMessage> CreatePostRequestAsync(string uri, HttpContent content, bool gzipCompress = false)
        {
            return await CreatePostRequestAsync(new Uri(uri), content, gzipCompress);
        }


        internal async static Task<HttpRequestMessage> CreatePostRequestAsync(Uri uri, HttpContent content, bool gzipCompress = false)
        {
            HttpContent httpContent;

            if (gzipCompress)
            {
                // GZIP
                var data = await content.ReadAsByteArrayAsync();
                byte[] compressed;

                using (var mr = new MemoryStream())
                {
                    using (var gzip = new GZipStream(mr, CompressionMode.Compress))
                    {
                        await gzip.WriteAsync(data, 0, data.Length);
                    }

                    compressed = mr.ToArray();
                }

                var byteArrayContent = new ByteArrayContent(compressed);
                byteArrayContent.Headers.ContentEncoding.Add("gzip");
                byteArrayContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                httpContent = byteArrayContent;
            }
            else
            {
                httpContent = content;
            }

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = uri,
                Content = httpContent
            };

            return request;
        }
#endif
    }
}
