using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Models.SalesOrg;
using ODTradePromotion.API.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.SalesOrg
{
    public class SalesOrgService : ISalesOrgService
    {
        private readonly ILogger<SalesOrgService> _logger;
        private readonly IBaseRepository<ScSalesOrganizationStructure> _serviceSalesOrg;
        private readonly IBaseRepository<ScTerritoryStructureDetail> _serviceSalesTerritoryLevel;
        private readonly IBaseRepository<ScTerritoryMapping> _serviceTerritoryMapping;
        private readonly IBaseRepository<ScTerritoryValue> _serviceTerritoryValue;
        private readonly IBaseRepository<DsaDistributorSellingArea> _serviceDsa;

        public SalesOrgService(ILogger<SalesOrgService> logger,
            IBaseRepository<ScSalesOrganizationStructure> serviceSalesOrg,
            IBaseRepository<ScTerritoryStructureDetail> serviceSalesTerritoryLevel,
            IBaseRepository<ScTerritoryMapping> serviceTerritoryMapping,
            IBaseRepository<ScTerritoryValue> serviceTerritoryValue,
            IBaseRepository<DsaDistributorSellingArea> serviceDsa)
        {
            _logger = logger;
            _serviceSalesOrg = serviceSalesOrg;
            _serviceSalesTerritoryLevel = serviceSalesTerritoryLevel;
            _serviceTerritoryMapping = serviceTerritoryMapping;
            _serviceTerritoryValue = serviceTerritoryValue;
            _serviceDsa = serviceDsa;
        }

        public IQueryable<TpSalesOrganizationModel> GetListSalesOrg()
        {
            DateTime now = DateTime.Now;

            return (from org in _serviceSalesOrg.GetAllQueryable(x => !x.IsDeleted && x.IsActive
                         && (x.EffectiveDate <= now && (!x.UntilDate.HasValue || (x.UntilDate.Value >= now))))
                    select new TpSalesOrganizationModel()
                    {
                        Id = org.Id.ToString(),
                        Code = org.Code,
                        TerritoryStructureCode = org.TerritoryStructureCode,
                        Description = org.Description,
                        EffectiveDate = org.EffectiveDate,
                        UntilDate = org.UntilDate,
                        IsActive = org.IsActive,
                        IsSpecificChanel = org.IsSpecificChanel,
                        Channel = org.Channel,
                        IsSpecificSIC = (org.IsSpecificSic != null) && org.IsSpecificSic.Value,
                        SIC = org.Sic
                    }).AsQueryable();
        }

        public IQueryable<TpTerritoryStructureLevelModel> GetListSalesTerritoryLevel(string territoryStructureCode)
        {
            return (from level in _serviceSalesTerritoryLevel.GetAllQueryable(x => !x.IsDeleted
                    && x.TerritoryStructureCode.ToLower().Equals(territoryStructureCode.ToLower()))
                    select new TpTerritoryStructureLevelModel()
                    {
                        Id = level.Id.ToString(),
                        TerritoryStructureCode = level.TerritoryStructureCode,
                        Description = level.Description,
                        Level = level.Level,
                        TerritoryLevelCode = level.TerritoryLevelCode
                    }).AsQueryable();
        }

        public IQueryable<TpSalesTerritoryValueModel> GetListSalesTerritoryValue(string territoryStructureCode, string territoryLevelCode)
        {
            DateTime now = DateTime.Now;

            return (from mp in _serviceTerritoryMapping.GetAllQueryable(x => !x.IsDeleted
                    && (x.EffectiveDate <= now && (!x.UntilDate.HasValue || (x.UntilDate.Value >= now))))
                    join v in _serviceTerritoryValue.GetAllQueryable(x => !x.IsDeleted
                    && (x.EffectiveDate <= now && (!x.UntilDate.HasValue || (x.UntilDate.Value >= now))))
                    on mp.TerritoryValueKey equals v.Key
                    where mp.TerritoryStructureCode.ToLower().Equals(territoryStructureCode.ToLower())
                    && v.TerritoryLevelCode.ToLower().Equals(territoryLevelCode.ToLower())
                    select new TpSalesTerritoryValueModel()
                    {
                        Key = v.Key,
                        Code = v.Code,
                        TerritoryLevelCode = v.TerritoryLevelCode,
                        Name = v.Name,
                        Description = v.Description,
                        EffectiveDate = v.EffectiveDate,
                        UntilDate = v.UntilDate
                    }).AsQueryable();
        }

        public IQueryable<TpSalesOrgDsaModel> GetListSalesDsaValue()
        {
            DateTime now = DateTime.Now;

            return (from dsa in _serviceDsa.GetAllQueryable(x => x.IsActive && !x.IsDeleted
                    && (x.EffectiveDate <= now && (!x.UntilDate.HasValue || (x.UntilDate.Value >= now))))
                    join mp in _serviceTerritoryMapping.GetAllQueryable(x => !x.IsDeleted
                    && (x.EffectiveDate <= now && (!x.UntilDate.HasValue || (x.UntilDate.Value >= now))))
                    on dsa.MappingNode equals mp.MappingNode
                    select new TpSalesOrgDsaModel()
                    {
                        Id = dsa.Id.ToString(),
                        Code = dsa.Code,
                        Description = dsa.Description,
                        TypeDSA = dsa.TypeDSA,
                        IsActive = dsa.IsActive,
                        MappingNode = dsa.MappingNode,
                        SOStructureCode = dsa.SOStructureCode,
                        EffectiveDate = dsa.EffectiveDate,
                        UntilDate = dsa.UntilDate
                    }).AsQueryable();
        }
    }
}
