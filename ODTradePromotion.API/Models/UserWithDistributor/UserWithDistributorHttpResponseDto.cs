using Newtonsoft.Json;

namespace ODTradePromotion.API.Models.UserWithDistributor
{
    public class UserWithDistributorHttpResponseDto
    {
        public int ObjectId { get; set; }
        public string ObjectGuidId { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string Data { get; set; }
        public UserWithDistributorDataHttpResponseDto DataModel => !string.IsNullOrEmpty(Data) ? JsonConvert.DeserializeObject<UserWithDistributorDataHttpResponseDto>(Data) : default;
    }
    public class UserWithDistributorDataHttpResponseDto
    {
        public string UserName{ get; set; }
        public string UserCode { get; set; }
    }
}
