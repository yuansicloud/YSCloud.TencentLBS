namespace YSCloud.TencentLBS.Common.Models
{
    public class CommonResponse : ICommonResponse
    {
        public string Message { get; set; }
        
        public int Status { get; set; }
    }
}