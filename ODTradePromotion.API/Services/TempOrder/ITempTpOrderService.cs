using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Models.Temp;
using System.Collections.Generic;

namespace ODTradePromotion.API.Services.TempOrder
{
    public interface ITempTpOrderService
    {
        public List<TpSettlementDetailModel> GetOrderDistributorInfoByPromotion(DistForPromotionByPromotionRequest request);
        public List<TpSettlementDetailModel> GetOrderDistributorInfoByListPromotion(DistForPromotionByListPromotionRequest request);
        public List<TpSettlementDetailModel> GetOrderDistributorInfoByDiscount(DistForDiscountRequest request);

        public BaseResultModel CreateOrderHeader(TempTpOrderHeaderModel input);
        public BaseResultModel CreateOrderDetail(TempTpOrderDetailModel input);
    }
}
