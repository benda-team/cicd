using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Services.Base;
using ODTradePromotion.API.Services.Promotion;
using Sys.Common.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ODTradePromotion.API.Services.RegisterPromotion
{
    public class CheckBudgetService : ICheckBudgetService
    {
        private readonly IResgiterPromotion _resgiterPromotion;
        private readonly IPromotionService _promotionService;
        private readonly IBaseRepository<RegistrationQueue> _baseRegistrationQueueRepository;
        public CheckBudgetService(IResgiterPromotion resgiterPromotion, IPromotionService promotionService, IBaseRepository<RegistrationQueue> baseRegistrationQueueRepository)
        {
            _resgiterPromotion = resgiterPromotion;
            _promotionService = promotionService;
            _baseRegistrationQueueRepository = baseRegistrationQueueRepository;

        }

        public async Task<Result<bool>> CheckBudgetAndResponeForRegisterPromotion()
        {
            var result = new Result<bool>
            {
                Data = true,
                Success = true,
            };
            try
            {
                var queueNumberOfNextElementToBeProcessedInQueue = _baseRegistrationQueueRepository.GetAll().Select(x => x.QueueNumber).DefaultIfEmpty().Min();

                var nextElementToBeProcessedInQueue = _baseRegistrationQueueRepository.Find(x => x.QueueNumber == queueNumberOfNextElementToBeProcessedInQueue).Select(
                                                                                            y => new PromotionBudgetRequest
                                                                                            {
                                                                                                PromotionCode = y.PromotionCode,
                                                                                                PromotionLevel = y.PromotionLevel,
                                                                                                CustomerCode = y.CustomerCode,
                                                                                                CustomerShiptoCode = y.CustomerShiptoCode,
                                                                                                RouteZoneCode = y.RouteZoneCode,
                                                                                                ProductSkuCode = y.ProductSkuCode,
                                                                                                ProductNumber = y.ProductNumber,
                                                                                                AmountOfDonation = y.AmountOfDonation,
                                                                                                SaleOrg = y.SaleOrg,
                                                                                                SicCode = y.SicCode,
                                                                                                DsaCode = y.DsaCode,
                                                                                                CountryCode = y.CountryCode,
                                                                                                BranchCode = y.BranchCode,
                                                                                                RegionCode = y.RegionCode,
                                                                                                SubAreaCode = y.SubAreaCode,
                                                                                                SubRegionCode = y.SubRegionCode,
                                                                                                AreaCode = y.AreaCode,
                                                                                            }).FirstOrDefault();
                if (nextElementToBeProcessedInQueue != null)
                {
                    var responeAffterCheckBudget = await _promotionService.ExtenalApiCheckBudgetInfoPromotion(nextElementToBeProcessedInQueue);

                    using (var context = new ApplicationDbContext())
                    {
                        using (var transaction = context.Database.BeginTransaction())
                        {
                            if (responeAffterCheckBudget != null)
                            {
                                if (responeAffterCheckBudget.IsEnoughBudgetForProduct == true && responeAffterCheckBudget.IsEnoughBudgetForAmount == null)
                                {
                                    try
                                    {
                                        TpBudgetUsed promotionForProduct = new TpBudgetUsed()
                                        {
                                            Id = Guid.NewGuid(),
                                            BudgetCode = responeAffterCheckBudget.BudgetForProductInfo.Code,
                                            BudgetType = responeAffterCheckBudget.BudgetForProductInfo.BudgetType,
                                            AmountUsed = 0,
                                            QuantityUsed = nextElementToBeProcessedInQueue.ProductNumber,
                                            CustomerCode = nextElementToBeProcessedInQueue.CustomerCode,
                                            ShiptoCode = nextElementToBeProcessedInQueue.CustomerShiptoCode,
                                            RouteZoneCode = nextElementToBeProcessedInQueue.RouteZoneCode,
                                            SaleOrgCode = nextElementToBeProcessedInQueue.SaleOrg,
                                            CountryCode = nextElementToBeProcessedInQueue.CountryCode,
                                            BranchCode = nextElementToBeProcessedInQueue.BranchCode,
                                            RegionCode = nextElementToBeProcessedInQueue.RegionCode,
                                            SubRegionCode = nextElementToBeProcessedInQueue.SubRegionCode,
                                            AreaCode = nextElementToBeProcessedInQueue.AreaCode,
                                            SubAreaCode = nextElementToBeProcessedInQueue.SubAreaCode,
                                            DSACodeCode = nextElementToBeProcessedInQueue.DsaCode,
                                            Key = nextElementToBeProcessedInQueue.Key,
                                        };

                                        _resgiterPromotion.ResgiterPromotion(promotionForProduct);

                                        var quequeModel = _baseRegistrationQueueRepository.Find(x => x.QueueNumber == queueNumberOfNextElementToBeProcessedInQueue).FirstOrDefault();
                                        _baseRegistrationQueueRepository.Delete(quequeModel.QueueNumber);

                                        transaction.Commit();
                                        return result;
                                    }
                                    catch (System.Exception ex)
                                    {
                                        transaction.Rollback();
                                        result.Success = false;
                                        result.Data = false;
                                        result.Messages.Add(ex.Message);
                                        return result;
                                    }
                                }

                                if (responeAffterCheckBudget.IsEnoughBudgetForAmount == true && responeAffterCheckBudget.IsEnoughBudgetForProduct == null)
                                {
                                    try
                                    {
                                        TpBudgetUsed promotionForProduct = new TpBudgetUsed()
                                        {
                                            Id = Guid.NewGuid(),
                                            BudgetCode = responeAffterCheckBudget.BudgetForAmountInfo.Code,
                                            BudgetType = responeAffterCheckBudget.BudgetForAmountInfo.BudgetType,
                                            AmountUsed = nextElementToBeProcessedInQueue.AmountOfDonation,
                                            QuantityUsed = 0,
                                            CustomerCode = nextElementToBeProcessedInQueue.CustomerCode,
                                            ShiptoCode = nextElementToBeProcessedInQueue.CustomerShiptoCode,
                                            RouteZoneCode = nextElementToBeProcessedInQueue.RouteZoneCode,
                                            SaleOrgCode = nextElementToBeProcessedInQueue.SaleOrg,
                                            CountryCode = nextElementToBeProcessedInQueue.CountryCode,
                                            BranchCode = nextElementToBeProcessedInQueue.BranchCode,
                                            RegionCode = nextElementToBeProcessedInQueue.RegionCode,
                                            SubRegionCode = nextElementToBeProcessedInQueue.SubRegionCode,
                                            AreaCode = nextElementToBeProcessedInQueue.AreaCode,
                                            SubAreaCode = nextElementToBeProcessedInQueue.SubAreaCode,
                                            DSACodeCode = nextElementToBeProcessedInQueue.DsaCode,
                                            Key = nextElementToBeProcessedInQueue.Key,
                                        };
                                        _resgiterPromotion.ResgiterPromotion(promotionForProduct);

                                        var quequeModel = _baseRegistrationQueueRepository.Find(x => x.QueueNumber == queueNumberOfNextElementToBeProcessedInQueue).FirstOrDefault();
                                        _baseRegistrationQueueRepository.Delete(quequeModel.QueueNumber);

                                        transaction.Commit();
                                        return result;
                                    }
                                    catch (System.Exception ex)
                                    {
                                        transaction.Rollback();
                                        result.Success = false;
                                        result.Data = false;
                                        result.Messages.Add(ex.Message);
                                        return result;
                                    }
                                }

                                if (responeAffterCheckBudget.IsEnoughBudgetForAmount == true && responeAffterCheckBudget.IsEnoughBudgetForProduct == true)
                                {
                                    try
                                    {
                                        TpBudgetUsed promotionForProduct = new TpBudgetUsed()
                                        {
                                            Id = Guid.NewGuid(),
                                            BudgetCode = responeAffterCheckBudget.BudgetForProductInfo.Code,
                                            BudgetType = responeAffterCheckBudget.BudgetForProductInfo.BudgetType,
                                            AmountUsed = 0,
                                            QuantityUsed = nextElementToBeProcessedInQueue.ProductNumber,
                                            CustomerCode = nextElementToBeProcessedInQueue.CustomerCode,
                                            ShiptoCode = nextElementToBeProcessedInQueue.CustomerShiptoCode,
                                            RouteZoneCode = nextElementToBeProcessedInQueue.RouteZoneCode,
                                            SaleOrgCode = nextElementToBeProcessedInQueue.SaleOrg,
                                            CountryCode = nextElementToBeProcessedInQueue.CountryCode,
                                            BranchCode = nextElementToBeProcessedInQueue.BranchCode,
                                            RegionCode = nextElementToBeProcessedInQueue.RegionCode,
                                            SubRegionCode = nextElementToBeProcessedInQueue.SubRegionCode,
                                            AreaCode = nextElementToBeProcessedInQueue.AreaCode,
                                            SubAreaCode = nextElementToBeProcessedInQueue.SubAreaCode,
                                            DSACodeCode = nextElementToBeProcessedInQueue.DsaCode,
                                            Key = nextElementToBeProcessedInQueue.Key,
                                        };

                                        _resgiterPromotion.ResgiterPromotion(promotionForProduct);

                                        TpBudgetUsed promotionForAmount = new TpBudgetUsed()
                                        {
                                            Id = Guid.NewGuid(),
                                            BudgetCode = responeAffterCheckBudget.BudgetForAmountInfo.Code,
                                            BudgetType = responeAffterCheckBudget.BudgetForAmountInfo.BudgetType,
                                            AmountUsed = nextElementToBeProcessedInQueue.AmountOfDonation,
                                            QuantityUsed = 0,
                                            CustomerCode = nextElementToBeProcessedInQueue.CustomerCode,
                                            ShiptoCode = nextElementToBeProcessedInQueue.CustomerShiptoCode,
                                            RouteZoneCode = nextElementToBeProcessedInQueue.RouteZoneCode,
                                            SaleOrgCode = nextElementToBeProcessedInQueue.SaleOrg,
                                            CountryCode = nextElementToBeProcessedInQueue.CountryCode,
                                            BranchCode = nextElementToBeProcessedInQueue.BranchCode,
                                            RegionCode = nextElementToBeProcessedInQueue.RegionCode,
                                            SubRegionCode = nextElementToBeProcessedInQueue.SubRegionCode,
                                            AreaCode = nextElementToBeProcessedInQueue.AreaCode,
                                            SubAreaCode = nextElementToBeProcessedInQueue.SubAreaCode,
                                            DSACodeCode = nextElementToBeProcessedInQueue.DsaCode,
                                            Key = nextElementToBeProcessedInQueue.Key,
                                        };
                                        _resgiterPromotion.ResgiterPromotion(promotionForAmount);

                                        var quequeModel = _baseRegistrationQueueRepository.Find(x => x.QueueNumber == queueNumberOfNextElementToBeProcessedInQueue).FirstOrDefault();
                                        _baseRegistrationQueueRepository.Delete(quequeModel.QueueNumber);

                                        transaction.Commit();
                                        return result;
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                        result.Success = false;
                                        result.Data = false;
                                        result.Messages.Add(ex.Message);
                                        return result;
                                    }
                                }
                            }
                        }
                    }
                }
                result.Success = false;
                result.Data = false;
                return result;
            }
            catch (System.Exception ex)
            {
                result.Success = false;
                result.Data = false;
                result.Messages.Add(ex.Message);
                return result;
            }
        }
    }
}
