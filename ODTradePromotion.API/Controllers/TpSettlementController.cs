using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Paging;
using System;
using System.Linq;
using static Sys.Common.Constants.ErrorCodes;
using System.Linq.Dynamic.Core;
using ODTradePromotion.API.Services.Settlement;
using ODTradePromotion.API.Models.Settlement;
using System.Collections.Generic;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class TpSettlementController : BaseController
    {
        private readonly ISettlementService _settlementService;
        public TpSettlementController(ISettlementService settlementService)
        {
            _settlementService = settlementService;
        }

        /// <summary>
        /// Get List Settlement
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSettlement")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSettlement(EcoParameters parameters)
        {
            try
            {
                IQueryable<TpSettlementModel> featureListTemp;
                featureListTemp = _settlementService.GetListSettlement().AsQueryable();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSettlementModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSettlementModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSettlementModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    var featureListTempPagged1 = PagedList<TpSettlementModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListSettlemenListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSettlementModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListSettlemenListModel { Items = result, MetaData = result.MetaData });
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
        [HttpPost]
        [Route("GetListSettlementForPopup")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSettlementForPopup(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _settlementService.GetListSettlementForPopup();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSettlementModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSettlementModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSettlementModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    var featureListTempPagged1 = PagedList<TpSettlementModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListSettlemenListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSettlementModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListSettlemenListModel { Items = result, MetaData = result.MetaData });
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
        /// Get List Settlement Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListSettlementCode/{status}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSettlementCode(string status)
        {
            try
            {
                var data = _settlementService.GetListSettlementCode(status).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.GetBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        /// <summary>
        /// Get Settlement By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSettlementByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetSettlementByCode(string code)
        {
            try
            {
                return Ok(_settlementService.GetSettlementByCode(code));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.GetTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Settlement Detail By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSettlementDetailByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetSettlementDetailByCode(string code)
        {
            try
            {
                return Ok(_settlementService.GetSettlementDetailByCode(code));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.GetTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Suggestion Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SuggestionCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult SuggestionCode(string code)
        {
            try
            {
                var data = _settlementService.SuggestionCode(code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.SuggestionCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Create Settlement
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateSettlement")]
        [MapToApiVersion("1.0")]
        public IActionResult CreateSettlement(TpSettlementModel input)
        {
            try
            {
                var existItem = _settlementService.GetSettlementByCode(input.Code);
                if (existItem == null)
                {
                    return Ok(_settlementService.CreateSettlement(input, string.Empty));
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = Convert.ToInt32(TpSettlementError.SettlementCodeIsExist),
                        Message = "SettlementCodeIsExist"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.CreateSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Update Settlement
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateSettlement")]
        [MapToApiVersion("1.0")]
        public IActionResult UpdateSettlement(TpSettlementModel input)
        {
            try
            {
                string userLogin = "";
                // Check Exist Function Code
                var existItemCode = _settlementService.GetSettlementByCode(input.Code);
                if (existItemCode != null)
                {
                    return Ok(_settlementService.UpdateSettlement(input, userLogin));
                }

                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.SettlementCodeDoesNotExist),
                    Message = "SettlementCodeDoesNotExist" //"Function code not exist, please use code exist."
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.UpdateSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Confirm Settlement By Distributor
        /// </summary>
        /// <param name="distributorCode"></param>
        /// <param name="lstInput"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("ConfirmSettlementByDistributor/{distributorCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult ConfirmSettlementByDistributor(string distributorCode, List<string> lstInput)
        {
            try
            {
                return Ok(_settlementService.ConfirmSettlementByDistributor(distributorCode, lstInput));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.UpdateSettlementFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Delete Settlement By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteSettlementByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult DeleteSettlementByCode(string code)
        {
            try
            {
                string userLogin = "";
                // Check Exist Function Code
                var existItem = _settlementService.GetSettlementByCode(code);
                if (existItem != null && _settlementService.Delete(code, userLogin))
                {
                    return Ok(new BaseResultModel
                    {
                        ObjectGuidId = existItem.Id,
                        IsSuccess = true
                    });
                }
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.DeleteBudgetByCodeFailed),
                    Message = "SettlementPromotionCodeIsNotExist"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.DeleteBudgetByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Settlement
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSettlementConfirm")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSettlementConfirm(EcoParameters parameters)
        {
            try
            {
                IQueryable<TpSettlementConfirmModel> featureListTemp = _settlementService.GetListSettlementConfirm().AsQueryable();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSettlementConfirmModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSettlementConfirmModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSettlementConfirmModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    featureListTemp = featureListTemp.OrderBy(x => x.SettlementCode);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpSettlementConfirmModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpSettlementConfirmListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSettlementConfirmModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpSettlementConfirmListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementConfirmFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Settlement Confirm By Distributor
        /// </summary>
        /// <param name="distributorCode"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSettlementConfirmByDistributor/{distributorCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSettlementConfirmByDistributor(string distributorCode, EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _settlementService.GetListSettlementConfirmByDistributor(distributorCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSettlementConfirmModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSettlementConfirmModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSettlementConfirmModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    featureListTemp = featureListTemp.OrderBy(x => x.SettlementCode);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpSettlementConfirmModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpSettlementConfirmListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSettlementConfirmModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpSettlementConfirmListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementConfirmFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Detail Settlement Confirm By Distributor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="distributorCode"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListDetailSettlementConfirmByDistributor/{code}/{distributorCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListDetailSettlementConfirmByDistributor(string code, string distributorCode, EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _settlementService.GetListDetailSettlementConfirmByDistributor(code, distributorCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSettlementDetailModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSettlementDetailModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSettlementDetailModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    var featureListTempPagged1 = PagedList<TpSettlementDetailModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListTpSettlementDetailModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSettlementDetailModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListTpSettlementDetailModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.ListSettlementConfirmFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Gets TpDefinitionStructures For Settlement
        /// </summary>
        /// <param name="distributorCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetsTpDefinitionStructuresForSettlement/{distributorCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetsTpDefinitionStructuresForSettlement(string distributorCode)
        {
            try
            {
                return Ok(_settlementService.GetsTpDefinitionStructuresForSettlement(distributorCode));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDataDefinitionStructureByPromotionCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Settlement By Promotion Code By Sale Calendar
        /// </summary>
        /// <param name="code"></param>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSettlementByPromotionCodeBySaleCalendar/{code}/{calendar}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetSettlementByPromotionCodeBySaleCalendar(string code, string calendar)
        {
            try
            {
                return Ok(_settlementService.GetSettlementByPromotionCodeBySaleCalendar(code, calendar));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.GetSettlementByPromotionCodeBySaleCalendarFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Settlement By Discount Code By Sale Calendar
        /// </summary>
        /// <param name="code"></param>
        /// <param name="calendar"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSettlementByDiscountCodeBySaleCalendar/{code}/{calendar}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetSettlementByDiscountCodeBySaleCalendar(string code, string calendar)
        {
            try
            {
                return Ok(_settlementService.GetSettlementByDiscountCodeBySaleCalendar(code, calendar));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpSettlementError.GetSettlementByDiscountCodeBySaleCalendarFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
