using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure.TpTemTable;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Models.Temp;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.TempOrder
{
    public class TempTpOrderService : ITempTpOrderService
    {
        #region Property
        private readonly ILogger<TempTpOrderService> _logger;
        private readonly IBaseRepository<Temp_TpOrderHeaders> _dbOrderHeader;
        private readonly IBaseRepository<Temp_TpOrderDetails> _dbOrderDetail;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public TempTpOrderService(ILogger<TempTpOrderService> logger,
            IBaseRepository<Temp_TpOrderHeaders> dbOrderHeader,
            IBaseRepository<Temp_TpOrderDetails> dbOrderDetail,
            IMapper mapper
            )
        {
            _logger = logger;
            _dbOrderHeader = dbOrderHeader;
            _dbOrderDetail = dbOrderDetail;
            _mapper = mapper;
        }

        public BaseResultModel CreateOrderHeader(TempTpOrderHeaderModel input)
        {
            try
            {
                var order = _dbOrderHeader.GetAllQueryable(x => x.OrdNbr.ToLower().Equals(input.OrdNbr.ToLower())).FirstOrDefault();
                if (order == null)
                {
                    var orderHeader = _mapper.Map<Temp_TpOrderHeaders>(input);
                    orderHeader.Id = Guid.NewGuid();

                    _dbOrderHeader.Insert(orderHeader);
                    return new BaseResultModel
                    {
                        IsSuccess = true,
                        Code = 201,
                        Message = "CreateSuccess"
                    };
                }
                else
                {
                    return new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = 201,
                        Message = "OrdNbr Is Exist"
                    };
                }

            }
            catch (System.Exception ex)
            {
                return new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        public BaseResultModel CreateOrderDetail(TempTpOrderDetailModel input)
        {
            try
            {
                var order = _mapper.Map<Temp_TpOrderDetails>(input);
                order.Id = Guid.NewGuid();

                _dbOrderDetail.Insert(order);
                return new BaseResultModel
                {
                    IsSuccess = true,
                    Code = 201,
                    Message = "CreateSuccess"
                };
            }
            catch (System.Exception ex)
            {
                return new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        public List<TpSettlementDetailModel> GetOrderDistributorInfoByDiscount(DistForDiscountRequest request)
        {
            return (from th in _dbOrderHeader.GetAllQueryable(x => x.Status.ToLower().Equals(CommonData.Status.Active.ToLower())
                    && string.IsNullOrEmpty(x.RecallOrderCode)
                    && x.OrdDate >= request.StartDate && x.OrdDate <= request.EndDate).AsNoTracking()
                    where request.Discount.Code.ToLower().Equals(th.DiscountCode.ToLower())
                    select new TpSettlementDetailModel()
                    {
                        SettlementCode = request.SettlementCode,
                        OrdNbr = th.OrdNbr,
                        ProgramType = request.ProgramType,
                        PromotionDiscountCode = request.Discount.Code,
                        DistributorCode = th.Disty_BilltoCode,
                        DistributorName = th.Disty_BilltoName,
                        Amount = th.SO_Shipped_Disc_Amt
                    }).ToList();
        }
        #endregion
        public List<TpSettlementDetailModel> GetOrderDistributorInfoByListPromotion(DistForPromotionByListPromotionRequest request)
        {
            List<TpSettlementDetailModel> orderDetails = new List<TpSettlementDetailModel>();
            foreach (var item in request.ListPromotion)
            {
                DistForPromotionByPromotionRequest requestByPromotion = new DistForPromotionByPromotionRequest()
                {
                    SettlementCode = request.SettlementCode,
                    ProgramType = request.ProgramType,
                    FrequencySettlement = request.FrequencySettlement,
                    Promotion = item,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                };

                var details = GetOrderDistributorInfoByPromotion(requestByPromotion);
                orderDetails.AddRange(details);
            }

            return orderDetails;
        }

        public List<TpSettlementDetailModel> GetOrderDistributorInfoByPromotion(DistForPromotionByPromotionRequest request)
        {
            if (request.FrequencySettlement == CommonData.PromotionSetting.AccordingToTheProgram)
            {
                return (from td in _dbOrderDetail.GetAllQueryable(x => x.IsFree).AsNoTracking()
                        join th in _dbOrderHeader.GetAllQueryable(x => x.Status.ToLower().Equals(CommonData.Status.Active.ToLower())
                        && string.IsNullOrEmpty(x.RecallOrderCode)
                        && x.OrdDate >= request.Promotion.EffectiveDateFrom && x.OrdDate <= request.Promotion.ValidUntil).AsNoTracking()
                        on td.OrdNbr equals th.OrdNbr
                        where request.Promotion.Code.ToLower().Equals(td.DiscountID.ToLower())
                        select new TpSettlementDetailModel()
                        {
                            SettlementCode = request.SettlementCode,
                            OrdNbr = td.OrdNbr,
                            ProgramType = request.ProgramType,
                            PromotionDiscountCode = request.Promotion.Code,
                            DistributorCode = th.Disty_BilltoCode,
                            DistributorName = th.Disty_BilltoName,
                            ProductCode = td.InventoryID,
                            ProductName = td.InventoryName,
                            Package = td.UOM,
                            PackageName = td.UOMName,
                            Quantity = td.ShippedQty,
                            Amount = td.ShippedLineDiscAmt,
                            OrdDate = th.OrdDate,
                            CustomerID = th.CustomerID,
                            PromotionLevel = td.PromotionLevel,
                            PromotionLevelName = td.PromotionLevelName,
                            ReferenceLink = th.ReferenceLink,
                            SalesRepCode = th.SalesRepCode,
                            ShiptoID = th.ShiptoID,
                            ShiptoName = th.ShiptoName,
                            PromotionDiscountName = td.DiscountName,
                        }).ToList();
            }
            else
            {
                return (from td in _dbOrderDetail.GetAllQueryable(x => x.IsFree).AsNoTracking()
                        join th in _dbOrderHeader.GetAllQueryable(x => x.Status.ToLower().Equals(CommonData.Status.Active.ToLower())
                        && string.IsNullOrEmpty(x.RecallOrderCode)
                        && x.OrdDate >= request.StartDate && x.OrdDate <= request.EndDate).AsNoTracking()
                        on td.OrdNbr equals th.OrdNbr
                        where request.Promotion.Code.ToLower().Equals(td.DiscountID.ToLower())
                        select new TpSettlementDetailModel()
                        {
                            SettlementCode = request.SettlementCode,
                            OrdNbr = td.OrdNbr,
                            ProgramType = request.ProgramType,
                            PromotionDiscountCode = request.Promotion.Code,
                            DistributorCode = th.Disty_BilltoCode,
                            DistributorName = th.Disty_BilltoName,
                            ProductCode = td.InventoryID,
                            ProductName = td.InventoryName,
                            Package = td.UOM,
                            PackageName = td.UOMName,
                            Quantity = td.ShippedQty,
                            Amount = td.ShippedLineDiscAmt,
                            OrdDate = th.OrdDate,
                            CustomerID = th.CustomerID,
                            PromotionLevel = td.PromotionLevel,
                            PromotionLevelName = td.PromotionLevelName,
                            ReferenceLink = th.ReferenceLink,
                            SalesRepCode = th.SalesRepCode,
                            ShiptoID = th.ShiptoID,
                            ShiptoName = th.ShiptoName,
                            PromotionDiscountName = td.DiscountName,
                        }).ToList();
            }
        }
    }
}
