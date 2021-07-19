using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Json;
using Volo.Abp.Modularity;

namespace YSCloud.TencentLBS.Common
{
    [DependsOn(typeof(AbpJsonModule))]
    public class AbpTencentLBSCommonModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClient(TencentCommonConsts.DefaultHttpClientName);
        }
    }
}