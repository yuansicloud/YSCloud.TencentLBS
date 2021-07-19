using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Volo.Abp.DependencyInjection;
using YSCloud.TencentLBS.Common.Models;

namespace YSCloud.TencentLBS.Common.Requester
{
    public class TencentLBSApiRequester : ITencentLBSApiRequester, ITransientDependency
    {
        protected HttpClient HttpClient { get; }
        protected IHttpClientFactory HttpClientFactory { get; }

        private readonly AbpTencentLBSCommonOptions _sdkCommonOptions;

        public TencentLBSApiRequester(IHttpClientFactory httpClientFactory, IOptions<AbpTencentLBSCommonOptions> sdkCommonOptions)
        {
            HttpClientFactory = httpClientFactory;
            HttpClient = httpClientFactory.CreateClient(TencentCommonConsts.DefaultHttpClientName);

            _sdkCommonOptions = sdkCommonOptions.Value;
        }

        public virtual async Task<TResponse> SendRequestAsync<TResponse>(ICommonRequest request, string endpoint)
            where TResponse : ICommonResponse
        {
            return await SendRequestAsync<TResponse>(request, endpoint, _sdkCommonOptions);
        }

        public virtual async Task<TResponse> SendRequestAsync<TResponse>(ICommonRequest request, string endpoint,
            AbpTencentLBSCommonOptions options) where TResponse : ICommonResponse
        {
            request.SetEndpoint(endpoint);
            request.SetKey(options.Key);

            using var response = await HttpClient.SendAsync(request.HttpRequestMessage);
            
            var result = await response.Content.ReadAsStringAsync();

            return request.ResultRoot.IsNullOrEmpty()
                ? JObject.Parse(result).ToObject<TResponse>()
                : JObject.Parse(result).SelectToken($"$.{request.ResultRoot}").ToObject<TResponse>();
        }
    }
}