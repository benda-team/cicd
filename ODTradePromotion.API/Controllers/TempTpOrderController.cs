using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Models.Temp;
using ODTradePromotion.API.Services.TempOrder;
using System;
using static Sys.Common.Constants.ErrorCodes;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class TempTpOrderController : BaseController
    {
        private readonly ITempTpOrderService _tempTpOrderService;

        public TempTpOrderController(ITempTpOrderService tempTpOrderService)
        {
            _tempTpOrderService = tempTpOrderService;
        }

        /// <summary>
        /// Get Order Distributor Info By Promotion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDistributorInfoByPromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult GetOrderDistributorInfoByPromotion(DistForPromotionByPromotionRequest request)
        {
            try
            {
                var data = _tempTpOrderService.GetOrderDistributorInfoByPromotion(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TempOrderError.GetOrderDistributorInfoByPromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Order Distributor Info By List Promotion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDistributorInfoByListPromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult GetOrderDistributorInfoByListPromotion(DistForPromotionByListPromotionRequest request)
        {
            try
            {
                var data = _tempTpOrderService.GetOrderDistributorInfoByListPromotion(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TempOrderError.GetOrderDistributorInfoByListPromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Order Distributor Info By Discount
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetOrderDistributorInfoByDiscount")]
        [MapToApiVersion("1.0")]
        public IActionResult GetOrderDistributorInfoByDiscount(DistForDiscountRequest request)
        {
            try
            {
                var data = _tempTpOrderService.GetOrderDistributorInfoByDiscount(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TempOrderError.GetOrderDistributorInfoByPromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Create Order Header
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateOrderHeader")]
        [MapToApiVersion("1.0")]
        public IActionResult CreateOrderHeader(TempTpOrderHeaderModel input)
        {
            try
            {
                return Ok(_tempTpOrderService.CreateOrderHeader(input));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.CreatePromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Create Order Detail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateOrderDetail")]
        [MapToApiVersion("1.0")]
        public IActionResult CreateOrderDetail(TempTpOrderDetailModel input)
        {
            try
            {
                return Ok(_tempTpOrderService.CreateOrderDetail(input));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.CreatePromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
