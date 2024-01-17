using ODTradePromotion.API.Models.SalesOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.SalesOrg
{
    public interface ISalesOrgService
    {
        public IQueryable<TpSalesOrganizationModel> GetListSalesOrg();
        public IQueryable<TpTerritoryStructureLevelModel> GetListSalesTerritoryLevel(string territoryStructureCode);
        public IQueryable<TpSalesTerritoryValueModel> GetListSalesTerritoryValue(string territoryStructureCode, string territoryLevelCode);
        public IQueryable<TpSalesOrgDsaModel> GetListSalesDsaValue();
        
    }
}
