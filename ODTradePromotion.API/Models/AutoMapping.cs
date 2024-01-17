using AutoMapper;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Infrastructure.TpTemTable;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Models.Temp;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.Discount;

namespace ODTradePromotion.API.Models
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<TpPromotion, Promotion.TpPromotionModel>().ReverseMap();
            CreateMap<Promotion.TpPromotionModel, TpPromotion>().ReverseMap();
            CreateMap<TpPromotion, TpPromotionGeneralModel>().ReverseMap();
            CreateMap<TpPromotionGeneralModel, TpPromotion>().ReverseMap();

            CreateMap<TpPromotionDefinitionStructure, Promotion.TpPromotionDefinitionStructureModel>().ReverseMap();
            CreateMap<TpPromotionDefinitionStructure, Promotion.PromotionDefinitionForSettlementModel>().ReverseMap();
            CreateMap<Promotion.TpPromotionDefinitionStructureModel, TpPromotionDefinitionStructure>().ReverseMap();
            CreateMap<TpPromotionDefinitionStructureModel, PromotionResultModel>().ReverseMap();

            CreateMap<TpBudget, TpBudgetListModel>();
            CreateMap<SaleCalendarGenerate, SaleCalendarByTyeModel>();
            CreateMap<TpBudgetAdjustment, TpBudgetAdjustmentListModel>();
            CreateMap<TpBudgetAdjustmentModel, TpBudgetAdjustment>().ReverseMap();
            CreateMap<TpBudgetAllotmentAdjustmentModel, TpBudgetAllotmentAdjustment>().ReverseMap();
            CreateMap<TpBudgetModel, TpBudget>().ReverseMap();
            CreateMap<TpBudgetDefineModel, TpBudgetDefine>().ReverseMap();
            CreateMap<TpBudgetAllotmentModel, TpBudgetAllotment>().ReverseMap();
            CreateMap<TpDiscountModel, TpDiscount>().ReverseMap();
            CreateMap<DiscountResultModel, TpDiscountModel>().ReverseMap();
            CreateMap<TpScopeDiscountModel, TpScopeDiscount>().ReverseMap();
            CreateMap<TpScopeDiscountDetailModel, TpScopeDiscountDetail>().ReverseMap();
            CreateMap<TpObjectDiscountModel, TpObjectDiscount>().ReverseMap();
            CreateMap<TpObjectDiscountDetailModel, TpObjectDiscountDetail>().ReverseMap();
            CreateMap<TpDiscountStructureModel, TpDiscountStructure>().ReverseMap();
            CreateMap<TpSettlementModel, TpSettlement>().ReverseMap();
            CreateMap<TpSettlement, TpSettlementModel>().ReverseMap();
            CreateMap<TpSettlementDetailModel, TpSettlementDetail>().ReverseMap();
            CreateMap<TpSettlementDetail, TpSettlementDetailModel>().ReverseMap();
            CreateMap<TpSettlementObjectModel, TpSettlementObject>().ReverseMap();
            CreateMap<TpSettlementObject, TpSettlementObjectModel>().ReverseMap();
            CreateMap<TpDiscountStructureDetailModel, TpDiscountStructureDetail>().ReverseMap();
            CreateMap<TempTpOrderHeaderModel, Temp_TpOrderHeaders>().ReverseMap();
            CreateMap<Temp_TpOrderHeaders, TempTpOrderHeaderModel>().ReverseMap();
            CreateMap<TempTpOrderDetailModel, Temp_TpOrderDetails>().ReverseMap();
            CreateMap<Temp_TpOrderDetails, TempTpOrderDetailModel>().ReverseMap();
            CreateMap<TpPromotion, PromotionExternalModel>().ReverseMap();
            CreateMap<Infrastructure.User, User.UserModel>().ReverseMap();
            //External
            CreateMap<ItemGroup, ItemGroupModel>().ReverseMap();
        }
    }
}
