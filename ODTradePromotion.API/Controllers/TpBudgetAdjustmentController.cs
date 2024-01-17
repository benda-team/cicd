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
    public class TpBudgetAdjustmentController : BaseController
    {
        private readonly IBudgetService _budgetService;
        private readonly IBudgetAdjustmentService _budgetAdjustmentService;
        public TpBudgetAdjustmentController(IBudgetService service, IBudgetAdjustmentService budgetAdjustmentService)
        {
            _budgetService = service;
            _budgetAdjustmentService = budgetAdjustmentService;
        }

        /// <summary>
        /// Get List Budget Adjustment
        /// </summary>
        /// <param name="type"></param>
        /// <param name="budgetCode"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListBudgetAdjustment/{type}/{budgetCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListBudgetAdjustment([FromRoute] int type, [FromRoute] string budgetCode, EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _budgetAdjustmentService.GetListBudgetAdjustment(type, budgetCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpBudgetAdjustmentListModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpBudgetAdjustmentListModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpBudgetAdjustmentListModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    featureListTemp = featureListTemp.OrderByDescending(x => x.AdjustmentDate);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpBudgetAdjustmentListModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListTpBudgetAdjustmentListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpBudgetAdjustmentListModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListTpBudgetAdjustmentListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetAdjustmentError.GetListBudgetAdjustmentFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get History Budget Adjustment
        /// </summary>
        /// <param name="budgetCode"></param>
        /// <param name="type"></param>
        /// <param name="countAdjustment"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetHistoryBudgetAdjustment/{budgetCode}/{type}/{countAdjustment}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetHistoryBudgetAdjustment(string budgetCode, int type, int countAdjustment)
        {
            try
            {
                var data = _budgetAdjustmentService.GetHistoryBudgetAdjustment(budgetCode, type, countAdjustment);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetAdjustmentError.GetBudgetAdjustmentFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Create Budget Adjustment
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateBudgetAdjustment")]
        [MapToApiVersion("1.0")]
        public IActionResult CreateBudgetAdjustment(TpBudgetAdjustmentModel input)
        {
            try
            {
                // Check Exist Function Code
                var existItemCode = _budgetService.GetBudgetByCodeGeneral(input.BudgetCode);
                if (existItemCode != null)
                {
                    _budgetAdjustmentService.CreateBudgetAdjustment(input, input.Account);
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
                        Code = Convert.ToInt32(BudgetAdjustmentError.CreateBudgetAdjustmentFailedBudgetCodeNotExist),
                        Message = "BudgetCodeNotExist" //"Function code not exist, please use code exist."
                    });
                }

            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(BudgetAdjustmentError.CreateBudgetAdjustmentFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
