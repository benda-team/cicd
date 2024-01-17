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
using ODTradePromotion.API.Services.Budget;
using ODTradePromotion.API.Models.Budget;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class TpBudgetController : BaseController
    {
        private readonly IBudgetService _budgetService;
        public TpBudgetController(IBudgetService service)
        {
            _budgetService = service;
        }

        /// <summary>
        /// Get List Budget
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListBudget")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListBudget(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _budgetService.GetListBudget();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpBudgetListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpBudgetListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpBudgetListModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    var featureListTempPagged1 = PagedList<TpBudgetListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListBudgetListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpBudgetListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListBudgetListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.ListBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        /// <summary>
        /// Get List Budget For Popup
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListBudgetForPopup")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListBudgetForPopup(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _budgetService.GetListBudgetForPopup();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpBudgetListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpBudgetListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpBudgetListModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    var featureListTempPagged1 = PagedList<TpBudgetListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListBudgetListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpBudgetListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListBudgetListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.ListBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        [HttpPost]
        [Route("GetListBudgetForPopupByCode")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListBudgetForPopupByCode(EcoParameters parameters)
        {
            try
            {

                var featureListTemp = _budgetService.GetListBudgetForPopup(parameters.OrderBy).ToList();
                string search = "";
                switch (parameters.Filter)
                {
                    case "01":
                        featureListTemp = featureListTemp.Where(e => e.BudgetAllocationLevel is not null && e.BudgetAllocationLevel.Equals("NW")).ToList();
                        break;
                    case "02":
                        featureListTemp = featureListTemp.Where(e => !e.BudgetAllocationLevel.StartsWith("DSA") && !e.BudgetAllocationLevel.StartsWith("NW")).ToList(); 
                        break;
                    case "03":
                        featureListTemp = featureListTemp.Where(e => e.BudgetAllocationLevel is not null && e.BudgetAllocationLevel.StartsWith("DSA")).ToList();
                        break;
                }
                // check searching
               

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    //featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code).ToList();
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpBudgetListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListBudgetListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpBudgetListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListBudgetListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.ListBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Budget By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListBudgetCode")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListBudgetCode()
        {
            try
            {
                var data = _budgetService.GetListBudgetCode();
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
        /// Get Budget By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetBudgetByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetBudgetByCode(string code)
        {
            try
            {
                var data = _budgetService.GetBudgetByCode(code);
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
        /// Get List Budget By Type And Code Of Product
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListBudgetByTypeAndCodeOfProduct")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListBudgetByTypeAndCodeOfProduct(FilterBudgetByProductModel input)
        {
            try
            {
                var data = _budgetService.GetListBudgetByTypeAndCodeOfProduct(input);
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
        /// Create Budget
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBudget")]
        [MapToApiVersion("1.0")]
        public IActionResult CreateBudget(TpBudgetModel input)
        {
            try
            {
                // Check Exist Function Code
                var existItemCode = _budgetService.GetBudgetByCodeGeneral(input.Code);
                if (existItemCode == null)
                {
                    _budgetService.CreateBudget(input, input.UserName);
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = true
                    });
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = Convert.ToInt32(BudgetError.CreateBudgetFailedCodeExist),
                        Message = "BudgetCodeExist" //"Function code already exist, please use new code."
                    });
                }

            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.CreateBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Update Budget
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateBudget")]
        [MapToApiVersion("1.0")]
        public IActionResult UpdateBudget(TpBudgetModel input)
        {
            try
            {
                // Check Exist Function Code
                var existItemCode = _budgetService.GetBudgetByCodeGeneral(input.Code);
                if (existItemCode != null && _budgetService.UpdateBudget(input, input.UserName))
                {
                    return Ok(new BaseResultModel
                    {
                        ObjectGuidId = existItemCode.Id,
                        IsSuccess = true
                    });
                }

                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.UpdateBudgetFailedCodeExist),
                    Message = "BudgetCodeNotExist" //"Function code not exist, please use code exist."
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.UpdateBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Delete Budget By Code
        /// </summary>
        /// <param name="code"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteBudgetByCode/{code}/{userName}")]
        [MapToApiVersion("1.0")]
        public IActionResult DeleteBudgetByCode(string code, string userName)
        {
            try
            {
                // Check Exist Function Code
                var existItem = _budgetService.GetBudgetByCodeGeneral(code);
                if (existItem != null && _budgetService.DeleteBudgetByCode(code, userName))
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
                    Code = Convert.ToInt32(BudgetError.DeleteBudgetFailedBudgetNotExist),
                    Message = "BudgetCodeNotExist"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.DeleteBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Update Budget Quantity Used
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateBudgetQuantityUsed")]
        [MapToApiVersion("1.0")]
        public IActionResult UpdateBudgetQuantityUsed(BudgetQuantityUsedModel input)
        {
            try
            {
                return Ok(_budgetService.UpdateBudgetQuantityUsed(input.BudgetCode, input.BudgetQuantityUsed, input.UserLogin));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetError.CreateBudgetFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }  
    }
}
