using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Paging;
using Sys.Common.Extensions;
using System;
using System.Linq;
using static Sys.Common.Constants.ErrorCodes;
using System.Linq.Dynamic.Core;
using ODTradePromotion.API.Services.Common;
using ODTradePromotion.API.Models.External;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class ExternalController : BaseController
    {
        private readonly IExternalService _externalService;
        public ExternalController(IExternalService externalService)
        {
            _externalService = externalService;
        }

        /// <summary>
        /// Get List Calendar By Sale Period
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListCalendarBySalePeriod")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListCalendarBySalePeriod(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _externalService.GetListCalendarBySalePeriod();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(SalePeriodModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<SalePeriodModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<SalePeriodModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    featureListTemp = featureListTemp.OrderByDescending(x => x.StartDate);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<SalePeriodModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListSalePeriodModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<SalePeriodModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListSalePeriodModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(ExternalError.GetListCalendarBySalePeriodFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Sale Calendar
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListCalendar")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListCalendar(GetListCalendarEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _externalService.GetListCalendar(parameters);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(SalePeriodModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<SalePeriodModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<SalePeriodModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    featureListTemp = featureListTemp.OrderByDescending(x => x.StartDate);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<SalePeriodModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListSalePeriodModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<SalePeriodModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListSalePeriodModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(ExternalError.GetListCalendarBySalePeriodFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Calendar Generate By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCalendarGenerateByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetCalendarGenerateByCode(string code)
        {
            try
            {
                var data = _externalService.GetCalendarGenerateByCode(code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(ExternalError.GetCalendarGenerateByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Distributor By UserCode
        /// </summary>
        /// <param name="usercode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDistributorByUserCode/{usercode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetDistributorByUserCode(string usercode)
        {
            try
            {
                var data = _externalService.GetDistributorByUserCode(usercode);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(ExternalError.GetListDistributorByUserCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
