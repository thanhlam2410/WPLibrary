namespace WPCore.Http.Models
{
    public class HttpResultModel
    {
        public string ResponseCode { get; set; }
        
        public string ResposeData { get; set; }
        
        public string ErrorMessage { get; set; }
        
        public string Tag { get; set; }
        
        public bool IsSuccess { get; set; }
    }
}
