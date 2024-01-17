using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ODTradePromotion.API.Models.Base;
using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.Report;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Models.User;
using ODTradePromotion.API.Services.Promotion;
using ODTradePromotion.API.Services.Promotion.Report;
using ODTradePromotion.API.Services.User;
using Sys.Common.Extensions;
using Sys.Common.Models;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using static Sys.Common.Constants.ErrorCodes;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class TpPromotionReportController : BaseController
    {
        #region Property
        private readonly IPromotionSyntheticReportService _promotionSyntheticReportService;
        private readonly IPromotionDetailReportPointSaleService _promotionDetailReportPointSaleService;
        private readonly IPromotionDetailReportRouteZoneService _promotionDetailReportRouteZoneService;
        private readonly IPromotionDetailReportOrderService _promotionDetailReportOrderService;
        private readonly IPromotionSyntheticReportSettlementService _promotionSyntheticReportSettlementService;
        #endregion

        #region Constructor
        public TpPromotionReportController(
            IPromotionSyntheticReportService promotionSyntheticReportService,
            IPromotionDetailReportPointSaleService promotionDetailReportPointSaleService,
            IPromotionDetailReportRouteZoneService promotionDetailReportRouteZoneService,
            IPromotionDetailReportOrderService promotionDetailReportOrderService,
            IPromotionSyntheticReportSettlementService promotionSyntheticReportSettlementService
            )
        {
            _promotionSyntheticReportService = promotionSyntheticReportService;
            _promotionDetailReportPointSaleService = promotionDetailReportPointSaleService;
            _promotionDetailReportRouteZoneService = promotionDetailReportRouteZoneService;
            _promotionDetailReportOrderService = promotionDetailReportOrderService;
            _promotionSyntheticReportSettlementService = promotionSyntheticReportSettlementService;
        }
        #endregion

        #region Synthetic
        /// <summary>
        /// Get List Promotion Synthetic Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotionSyntheticReport")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotionSyntheticReport(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionSyntheticReportService.GetListPromotionSyntheticReport();
                var budgetListTemp = _promotionSyntheticReportService.GetListBudget();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionSyntheticReportListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionSyntheticReportListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    var filterExpressionTemp1 = CSharpScript.EvaluateAsync<Func<BudgetInfo, bool>>(($"s=> {parameters.Filter.Split("&&")[0]}"), optionsAssembly);
                    Func<PromotionSyntheticReportListModel, bool> filterExpression = filterExpressionTemp.Result;
                    Func<BudgetInfo, bool> filterExpression1 = filterExpressionTemp1.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    var checkCondition1 = budgetListTemp.Where(filterExpression1);

                    featureListTemp = checkCondition.AsQueryable();
                    budgetListTemp = checkCondition1.AsQueryable();
                }
                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var itemsBudget = featureListTemp.ToList();

                    foreach (var item in itemsBudget)
                    {
                        item.BudgetInfos = new();
                        var add = budgetListTemp.Where(x => x.Code == item.Code
                        && (x.DonateApplyBudgetCode == item.DonateApplyBudgetCode
                        || x.GiftApplyBudgetCode == item.GiftApplyBudgetCode)).ToList();
                        item.BudgetInfos.AddRange(add);
                    }

                    var featureListTempPagged1 = PagedList<PromotionSyntheticReportListModel>.ToPagedList(itemsBudget, 0, itemsBudget.Count());

                    return Ok(new ListPromotionSyntheticReportListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var itemBudget = budgetListTemp.ToList();

                foreach (var item in items)
                {
                    item.BudgetInfos = new();
                    var add = budgetListTemp.Where(x => x.Code == item.Code
                    && (x.DonateApplyBudgetCode == item.DonateApplyBudgetCode
                    || x.GiftApplyBudgetCode == item.GiftApplyBudgetCode)).ToList();
                    item.BudgetInfos.AddRange(add);
                }

                var result = new PagedList<PromotionSyntheticReportListModel>(items, totalCount, (skip / top) + 1, top);
                
                return Ok(new ListPromotionSyntheticReportListModel { Items = result, MetaData = result.MetaData});
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        #endregion

        #region Point Sale
        /// <summary>
        /// Get Customers Order For Popup Promotion Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCustomersOrderForPopupPromotionReport")]
        [MapToApiVersion("1.0")]
        public IActionResult GetCustomersOrderForPopupPromotionReport(PromotionReportEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionDetailReportPointSaleService.GetCustomersOrderForPopupPromotionReport(parameters);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDetailReportPointSaleListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDetailReportPointSaleListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDetailReportPointSaleListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.CustomerID);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDetailReportPointSaleListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionDetailReportPointSaleListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDetailReportPointSaleListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionDetailReportPointSaleListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Customers Order For Promotion Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetCustomersOrderForPromotionReport")]
        [MapToApiVersion("1.0")]
        public IActionResult GetCustomersOrderForPromotionReport(PromotionReportEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionDetailReportPointSaleService.GetCustomersOrderForPromotionReport(parameters);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDetailReportPointSaleListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDetailReportPointSaleListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDetailReportPointSaleListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.CustomerID);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDetailReportPointSaleListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionDetailReportPointSaleListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDetailReportPointSaleListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionDetailReportPointSaleListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        #endregion

        #region Order
        /// <summary>
        /// Get Orders For Popup Promotion Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrdersForPopupPromotionReport")]
        [MapToApiVersion("1.0")]
        public IActionResult GetOrdersForPopupPromotionReport(PromotionReportEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionDetailReportOrderService.GetOrdersForPopupPromotionReport(parameters);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDetailReportOrderListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDetailReportOrderListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDetailReportOrderListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.OrdNbr);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDetailReportOrderListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionDetailReportOrderListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDetailReportOrderListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionDetailReportOrderListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Orders For Promotion Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrdersForPromotionReport")]
        [MapToApiVersion("1.0")]
        public IActionResult GetOrdersForPromotionReport(PromotionReportEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionDetailReportOrderService.GetOrdersForPromotionReport(parameters);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDetailReportOrderListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDetailReportOrderListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDetailReportOrderListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.OrdNbr);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDetailReportOrderListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionDetailReportOrderListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDetailReportOrderListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionDetailReportOrderListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        #endregion

        #region Route Zone
        /// <summary>
        /// Get RouteZones Order For Popup Promotion Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRouteZonesOrderForPopupPromotionReport")]
        [MapToApiVersion("1.0")]
        public IActionResult GetRouteZonesOrderForPopupPromotionReport(PromotionReportEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionDetailReportRouteZoneService.GetRouteZonesOrderForPopupPromotionReport(parameters);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDetailReportRouteZoneListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDetailReportRouteZoneListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDetailReportRouteZoneListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.RouteZoneId);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDetailReportRouteZoneListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionDetailReportRouteZoneListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDetailReportRouteZoneListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionDetailReportRouteZoneListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get RouteZones Order For Promotion Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetRouteZonesOrderForPromotionReport")]
        [MapToApiVersion("1.0")]
        public IActionResult GetRouteZonesOrderForPromotionReport(PromotionReportEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionDetailReportRouteZoneService.GetRouteZonesOrderForPromotionReport(parameters);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDetailReportRouteZoneListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDetailReportRouteZoneListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDetailReportRouteZoneListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    //featureListTemp = featureListTemp.OrderBy(x => x.RouteZoneId);
                    featureListTemp = featureListTemp.OrderBy(x => x.RouteZoneId).ThenBy(x => x.PromotionLevelName);//.ThenBy(x => x.OrdDate);
                    //featureListTemp = featureListTemp.OrderBy(x => new { x.RouteZoneId, x.PromotionLevel, x.ProductName, x.OrdDate });
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDetailReportRouteZoneListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionDetailReportRouteZoneListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDetailReportRouteZoneListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionDetailReportRouteZoneListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        #endregion

        #region Settlement
        /// <summary>
        /// Get List Promotion Discount Settlement For Popup Report
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotionDiscountSettlementForPopup")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotionDiscountSettlementForPopup(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionSyntheticReportSettlementService.GetListPromotionSettlementForPopup();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDiscountForPopupReportModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDiscountForPopupReportModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDiscountForPopupReportModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDiscountForPopupReportModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new PromotionDiscountForPopupReportListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDiscountForPopupReportModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new PromotionDiscountForPopupReportListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Count Settlement Quantity By Promotion Discount Code
        /// </summary>
        /// <param name="promotionDiscountCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("CountSettlementQuantity/{promotionDiscountCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult CountSettlementQuantity(string promotionDiscountCode)
        {
            try
            {
                var data = _promotionSyntheticReportSettlementService.CountSettlementQuantity(promotionDiscountCode);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Promotion Synthetic Report Settlement
        /// </summary>
        /// <param name="promotionDiscountCode"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotionSyntheticReportSettlement/{promotionDiscountCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotionSyntheticReportSettlement(string promotionDiscountCode, EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionSyntheticReportSettlementService.GetListPromotionSyntheticReportSettlement(promotionDiscountCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionSyntheticReportSettlementListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionSyntheticReportSettlementListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionSyntheticReportSettlementListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionSyntheticReportSettlementListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionSyntheticReportSettlementListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionSyntheticReportSettlementListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionSyntheticReportSettlementListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Distributor Popup Report Settlement
        /// </summary>
        /// <param name="settlementCode"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListDistributorPopupReportSettlement/{settlementCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListDistributorPopupReportSettlement(string settlementCode, EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionSyntheticReportSettlementService.GetListDistributorPopupReportSettlement(settlementCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(DistributorPopupReportSettlementListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<DistributorPopupReportSettlementListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<DistributorPopupReportSettlementListModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.DistributorCode);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<DistributorPopupReportSettlementListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListDistributorPopupReportSettlementListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<DistributorPopupReportSettlementListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListDistributorPopupReportSettlementListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        #endregion
    }
}
