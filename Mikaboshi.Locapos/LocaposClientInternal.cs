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
        internal string BaseUri => "https://locapos.com/";
        internal string BaseUriBeta => "https://beta.locapos.com/";
        internal string ApiUri => this.BaseUri + "api/";
        internal string ApiUriBeta => this.BaseUriBeta + "api/";

        private readonly HttpClient http;
        private readonly HttpClientHandler clientHandler;

        internal LocaposClientInternal(HttpClientHandler? clientHandler = null)
        {
            this.clientHandler = clientHandler ?? new HttpClientHandler();
            this.clientHandler.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            this.http = new HttpClient(this.clientHandler);
        }

        internal HttpClient GetHttpClient(ClientToken token)
        {
            this.http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.Token);
            return this.http;
        }

        internal HttpRequestMessage CreateGetRequest(string uri)
        {
            return CreateGetRequest(new Uri(uri));
        }

        internal HttpRequestMessage CreateGetRequest(Uri uri)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            return request;
        }

        internal async Task<HttpRequestMessage> CreatePostRequestAsync(string uri, HttpContent content, bool gzipCompress = false)
        {
            return await CreatePostRequestAsync(new Uri(uri), content, gzipCompress);
        }

        internal async Task<HttpRequestMessage> CreatePostRequestAsync(Uri uri, HttpContent content, bool gzipCompress = false)
        {
            HttpContent httpContent;

            if (gzipCompress)
            {
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
