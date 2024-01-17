using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sys.Common.Constants
{
    public static class CommonData
    {
        public static class SystemSetting
        {
            // Promotion
            public const string PromotionType = "PROMOTION_TYPE";
            public const string ApplicableObject = "TMK_APPLICABLE_OBJECT";
            public const string MktScope = "TMK_SCOPE";
            public const string Frequency = "PROMOTION_SALECALENDAR";
            public const string Object = "TMK_APPLICABLE_OBJECT";
            public const string TpType = "TPTYPE";
            public const string SlsFreequencySettlement = "SETTLEMENT_FREQUENCY";
            public const string PromotionStatus = "PROMOTION_STATUS";
            public const string TmkApplicable = "TMKAPPLICABLE";


            public const string TpStatus = "TPSTATUS";
            public const string BudgetType = "BUDGET_TYPE";
            public const string SlStatus = "SLSTATUS";
            public const string BudgetAllocationForm = "BUDGET_ALLOCATION_FORM";
            public const string ScopeType = "TMK_SCOPE";
            public const string ApplicationObject = "TMK_APPLICABLE_OBJECT";
            public const string SettlementPromotionStatus = "SETTLEMENT_PROMOTION_STATUS";
            public const string DistributorConfirmSettlement = "DISTRIBUTOR_CONFIRM_SETTLEMENT";

            public const string RoundingRuleBusiness = "Business";
            public const string RoundingRuleMathematics = "Mathematics";
        }

        public static class ItemStatus
        {
            public const string Active = "1";
            public const string InActive = "0";
        }

        public static class Status
        {
            public const string Active = "01";
            public const string InActive = "02";
        }

        public static class PromotionSetting
        {
            // Promotion Type
            public const string PromotionByProduct = "01";
            public const string ProductGroups = "02";
            public const string ProductSets = "03";
            public const string AccordingToOrderValue = "04";

            //ProgramType
            public const string PromotionProgram = "01";
            public const string DiscountProgram = "02";

            // Status Budget for Promotion
            public const string StatusDefining = "01";
            public const string StatusCanLinkPromotion = "02";
            public const string StatusLinkedPromotion = "03";

            // Scope
            public const string ScopeNationwide = "01";
            public const string ScopeSalesTerritoryLevel = "02";
            public const string ScopeDSA = "03";

            // Applicable Object
            public const string ObjectAllCustomer = "01";
            public const string ObjectCustomerAttributes = "02";
            public const string ObjectCustomerShipto = "03";

            //PromotionProductType
            public const string SKU = "01";
            public const string ItemGroup = "02";
            public const string ItemHierarchyValue = "03";

            // status Promotion
            public const string Inprogress = "01";
            public const string WaitConfirm = "02";
            public const string Confirmed = "03";
            public const string Refuse = "04";

            public const int AccordingToTheProgram = 1;
            public const int SaleCalendar = 2;

            // Discount Type
            public const int DiscountAmount = 1;
            public const int DiscountPercent = 2;
        }


        public static class SettlementPromotion
        {
            // status Settlement Promotion
            public const string Inprogress = "01";
            public const string WaitConfirm = "02";
            public const string confirmed = "03";

            public const string Create = "01";
            public const string Confirm = "02";
        }

        public static class TerritoryLevelSetting
        {
            public const string Branch = "TL01";
            public const string Region = "TL02";
            public const string SubRegion = "TL03";
            public const string Area = "TL04";
            public const string SubArea = "TL05";
        }

        public static class PriorityStandard
        {
            public const int Priority = 1;
            public const int PriorityByTime = 2;
            public const int Ratio = 3;
        }

        public static class ItemHierarchyLevel
        {
            public const string ItemHierarchyLevel1 = "IT01";
            public const string ItemHierarchyLevel2 = "IT02";
            public const string ItemHierarchyLevel3 = "IT03";
            public const string ItemHierarchyLevel4 = "IT04";
            public const string ItemHierarchyLevel5 = "IT05";
            public const string ItemHierarchyLevel6 = "IT06";
            public const string ItemHierarchyLevel7 = "IT07";
            public const string ItemHierarchyLevel8 = "IT08";
            public const string ItemHierarchyLevel9 = "IT09";
            public const string ItemHierarchyLevel10 = "IT10";
        }

        public static class CustomerSetting
        {
            public const string CUS01 = "CUS01";
            public const string CUS02 = "CUS02";
            public const string CUS03 = "CUS03";
            public const string CUS04 = "CUS04";
            public const string CUS05 = "CUS05";
            public const string CUS06 = "CUS06";
            public const string CUS07 = "CUS07";
            public const string CUS08 = "CUS08";
            public const string CUS09 = "CUS09";
            public const string CUS10 = "CUS10";
        }

        public static class BudgetForTradePromotion
        {
            // BudgetType
            public const string Commodity = "01";

            public const string Money = "02";

            // BugetAllocationform
            public const string AccordingToTheSalesTeam = "01";

            public const string ByPointOfSale = "02";

            //PromotionProductType
            public const string SKU = "01";

            public const string ItemGroup = "02";
            public const string ItemHierarchyValue = "03";

            // BudgetAdjustment Type
            public const int TypeBudgetNow = 0;

            public const int TypeBudgetAdjustment = 1;
            public const int TypeBudgetAllotmentAdjustment = 2;

            // Import Sale Territory Value
            public const int SaleTerritoryValueNumberSheet = 2;
        }
    }
}
