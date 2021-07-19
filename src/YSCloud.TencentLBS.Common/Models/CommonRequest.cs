using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace YSCloud.TencentLBS.Common.Models
{
    public abstract class CommonRequest : ICommonRequest
    {
        public virtual string ResultRoot { get; } = "Result";

        public HttpRequestMessage HttpRequestMessage { get; }

        protected string Endpoint { get; private set; }
        
        protected string Scheme { get; private set; }
        
        protected string ApiPath { get; private set; }

        protected string RequestBody { get; private set; }

        protected Dictionary<string, object> RequestParamsRecord { get; private set; } = new();

        protected string ServiceName
        {
            get
            {
                if (string.IsNullOrEmpty(Endpoint)) throw new ArgumentNullException(nameof(Endpoint), "终端点地址不能够为空。");
                var data = Endpoint.Split('.');
                if (data.Length <= 0) throw new ArgumentOutOfRangeException(nameof(Endpoint), "终端点地址不正确。");
                return data[0];
            }
        }

        protected long Timestamp { get; }

        public CommonRequest(string apiPath = null, string scheme = "https")
        {
            ApiPath = apiPath;

            Scheme = scheme;

            Timestamp = DateTimeExtensions.GetUtcUnixTimestamp();

            HttpRequestMessage = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
            };

        }

        public virtual void SetEndpoint(string endpoint)
        {
            Endpoint = endpoint;
            
            HttpRequestMessage.RequestUri = new Uri($"{Scheme}://{Endpoint}/{ApiPath}");
        }

        public virtual void SetKey(string key)
        {
            HttpRequestMessage.Headers.TryAddWithoutValidation("key", key);
        }

        protected void SetRequestBody(object paramsObj)
        {
            RequestBody = JsonConvert.SerializeObject(paramsObj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
            
            RequestParamsRecord = JsonConvert.DeserializeObject<Dictionary<string, object>>(RequestBody);

            HttpRequestMessage.Content = new StringContent(RequestBody);
            HttpRequestMessage.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
            HttpRequestMessage.Content.Headers.Add("charset", "utf-8");
        }
    }
}