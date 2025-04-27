using System;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mikaboshi.Locapos
{
    internal class LocaposClientInternal
    {
        internal static string BaseUri => "https://locapos.com/";
        internal static string BaseUriBeta => "https://beta.locapos.com/";
        internal static string ApiUri => BaseUri + "api/";
        internal static string ApiUriBeta => BaseUriBeta + "api/";

        private static HttpClient? http;

        private static HttpClientHandler clientHandler = new() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip };

        internal static HttpClientHandler? ClientHandler
        {
            get => clientHandler;
            set
            {
                if (value is null)
                {
                    return;
                }

                clientHandler = value;
                clientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            }
        }


        internal static HttpClient GetHttpClient(ClientToken token)
        {
            if (http is not null) return http;

            http = new HttpClient(clientHandler);

            http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);

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
                RequestUri = uri
            };

            return request;
        }

        internal static async Task<HttpRequestMessage> CreatePostRequestAsync(string uri, HttpContent content, bool gzipCompress = false)
        {
            return await CreatePostRequestAsync(new Uri(uri), content, gzipCompress);
        }


        internal static async Task<HttpRequestMessage> CreatePostRequestAsync(Uri uri, HttpContent content, bool gzipCompress = false)
        {
            HttpContent httpContent;

            if (gzipCompress)
            {
                // GZIP
                var data = await content.ReadAsByteArrayAsync();
                byte[] compressed;

                using (var mr = new MemoryStream())
                {
                    await using (var gzip = new GZipStream(mr, CompressionMode.Compress))
                    {
                        await gzip.WriteAsync(data);
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
    }
}
