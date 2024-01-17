using System;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.Promotion
{
    public interface IPromotionExternalService
    {
        /// <summary>
        /// Điều kiện sync
        /// - Là chương trình khuyến mãi
        /// - Từ hệ thống CP
        /// - Xác nhận triển khai toàn quốc
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        Task HandleSyncToODAsync(string promotionCode, string userName, string token);
    }
}
