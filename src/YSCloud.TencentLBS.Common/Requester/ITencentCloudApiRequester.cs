using System.Threading.Tasks;
using YSCloud.TencentLBS.Common.Models;

namespace YSCloud.TencentLBS.Common.Requester
{
    public interface ITencentLBSApiRequester
    {
        Task<TResponse> SendRequestAsync<TResponse>(ICommonRequest request, string endpoint) where TResponse : ICommonResponse;
        
        Task<TResponse> SendRequestAsync<TResponse>(ICommonRequest request, string endpoint, AbpTencentLBSCommonOptions options) where TResponse : ICommonResponse;
    }
}