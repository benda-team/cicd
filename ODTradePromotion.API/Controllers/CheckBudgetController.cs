using Microsoft.AspNetCore.Mvc;
using ODTradePromotion.API.Models;
using System;
using static Sys.Common.Constants.ErrorCodes;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Services.ExternalCheckBudgetService;
using ODTradePromotion.API.Models.External;
using Sys.Common.JWT;
using Microsoft.AspNetCore.Authorization;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/External_[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    //[AllowAnonymous]
    public class CheckBudgetController : BaseController
    {
        private readonly IExternalCheckBudgetService _externalCheckBudgetService;
        public CheckBudgetController(IExternalCheckBudgetService externalCheckBudgetService)
        {
            _externalCheckBudgetService = externalCheckBudgetService;
        }

        [HttpPost]
        [Route("SyncBudget")]
        [MapToApiVersion("1.0")]
        public IActionResult SyncBudget(FilterBudgetRemainAndCustomerBudget reques)
        {
            try
            {
                var data = _externalCheckBudgetService.SyncBudget(reques);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = data
                });
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

        [HttpPost]
        [Route("GetBudgetByCode")]
        [MapToApiVersion("1.0")]
        public IActionResult GetConfigBudget(FilterConfigBudget request)
        {
            try
            {
                var data = _externalCheckBudgetService.GetConfigBudget(request);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = data
                });
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

        [HttpPost]
        [Route("CheckBudget")]
        [MapToApiVersion("1.0")]
        public IActionResult SyncCheckBudget(SyncCheckBudgetModel request)
        {
            try
            {
                return Ok(_externalCheckBudgetService.CheckBudget(request));
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
    }
}
