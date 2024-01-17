using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Customer;
using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Services.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Sys.Common.Constants.ErrorCodes;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [ApiVersion("1.0")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Get List Customer Attribute By Customer Setting
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListCustomerAttributeByCustomerSetting")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListCustomerAttributeByCustomerSetting(CustomerSettingEcoParameters parameters)
        {
            try
            {
                var featureListTemp = _customerService.GetListCustomerAttributeByCustomerSetting(parameters.CustomerSettingCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(CustomerAttributeModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<CustomerAttributeModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<CustomerAttributeModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    var featureListTempPagged1 = PagedList<CustomerAttributeModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new CustomerAttributeListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<CustomerAttributeModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new CustomerAttributeListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(CustomerError.GetListCustomerAttributeByCustomerSettingIdFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// GetListCustomerShipto
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListCustomerShipto")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListCustomerShipto(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _customerService.GetListCustomerShipto();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(CustomerShiptoModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<CustomerShiptoModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<CustomerShiptoModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    featureListTemp = featureListTemp.OrderBy(x => x.CustomerCode).ThenBy(x=>x.ShiptoCode);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<CustomerShiptoModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new CustomerShiptoListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<CustomerShiptoModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new CustomerShiptoListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(CustomerError.GetListCustomerShiptoFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
