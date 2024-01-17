using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Services.SystemUrl;
using System;
using System.Linq;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class SystemUrlController : BaseController
    {
        private readonly ISystemUrlService _service;
        public SystemUrlController(ISystemUrlService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get All SystemUrl
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSystemUrl")]
        public IActionResult GetAllSystemUrl()
        {
            try
            {
                var featureListTemp = _service.GetAllSystemUrl().Result;
                return Ok(featureListTemp);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = 500,
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
