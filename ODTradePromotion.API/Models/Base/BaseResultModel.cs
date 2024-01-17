using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Base
{
    public interface IResponseModel
    {

    }
    public class BaseResultModel : IResponseModel
    {
        public int ObjectId { get; set; } = 0;
        public Guid ObjectGuidId { get; set; }
        public int Code { get; set; } = 0;
        public string Message { get; set; } = "";
        public bool IsSuccess { get; set; } = true;
        public object Data { get; set; }
    }
}
