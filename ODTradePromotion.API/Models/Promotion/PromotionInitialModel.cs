using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Models.Customer;
using ODTradePromotion.API.Models.External.Item;
using ODTradePromotion.API.Models.External.Sic;
using ODTradePromotion.API.Models.External.System;
using ODTradePromotion.API.Models.Item;
using ODTradePromotion.API.Models.SalesOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.Promotion
{
    public class PromotionInitialModel
    {
        public List<SystemSettingModel> SystemSettingModels { get; set; } = new List<SystemSettingModel>();
        public List<TpSalesOrganizationModel> TpSalesOrganizationModels { get; set; } = new List<TpSalesOrganizationModel>();
        public List<SicPrimaryModel> SicPrimaryModels { get; set; } = new List<SicPrimaryModel>();
        public List<UomsModel> UomsModels { get; set; } = new List<UomsModel>();
        public List<TpTerritoryStructureLevelModel> TpTerritoryStructureLevelModels { get; set; } = new List<TpTerritoryStructureLevelModel>();
        public List<CustomerSettingModel> CustomerSettingModels { get; set; } = new List<CustomerSettingModel>();
        public List<ItemSettingModel> ItemSettingModels { get; set; } = new List<ItemSettingModel>();
        public List<TpBudgetListModel> TpBudgetListModels { get; set; } = new List<TpBudgetListModel>();
        public List<InventoryItemModel> InventoryItemModels { get; set; } = new List<InventoryItemModel>();
    }
}
