using ODTradePromotion.API.Models.Base;
using ODTradePromotion.API.Models.Discount;
using ODTradePromotion.API.Models.Promotion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.TpDiscount
{
    public interface IDiscountService
    {
        public IQueryable<TpDiscountModel> GetListTpDiscount();
        public IQueryable<TpDiscountModel> GetListTpDiscountForSettlement();
        public Task<BaseResultModel> CreateTpDiscount(TpDiscountModel input, string userlogin);
        public Task<BaseResultModel> UpdateTpDiscount(TpDiscountModel input, string userlogin);
        public bool DeleteTpDiscountByCode(string code, string userlogin);
        public TpDiscountModel GetGeneralTpDiscountByCode(string code);
        public TpDiscountModel GetTpDiscountDetailByCode(string code);
        public TpDiscountModel GetTpDiscountDetailById(Guid Id);
        public Task<TpDiscountModel> GetGeneralDiscountByCode(string code);
        public Task<List<TpDiscountModel>> CheckExistScopeObjectDiscount(TpDiscountModel input);
        #region External API
        public Task<DiscountExternalModel> GetDiscountByCustomer(ListPromotionAndDiscountRequestModel request);

        public Task<DiscountResultDetailModel> DiscountResult(DiscountResultParameters request);
        #endregion
    }
}
