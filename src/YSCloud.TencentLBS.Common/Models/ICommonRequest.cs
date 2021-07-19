using System.Net.Http;

namespace YSCloud.TencentLBS.Common.Models
{
    public interface ICommonRequest
    {
        string ResultRoot { get; }

        HttpRequestMessage HttpRequestMessage { get; }

        void SetKey(string key);

        void SetEndpoint(string endpoint);
    }
}