using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Services.RegisterPromotion;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    //[AllowAnonymous]
    public class RegisterPromotionController : ControllerBase
    {
        private readonly IOrdinalNumberRegistrationService _ordinalNumberRegistrationService;
        private readonly IResgiterPromotion _resgiterPromotion;
        public RegisterPromotionController(IOrdinalNumberRegistrationService ordinalNumberRegistrationService, IResgiterPromotion resgiterPromotion)
        {
            _ordinalNumberRegistrationService = ordinalNumberRegistrationService;
            _resgiterPromotion = resgiterPromotion;
        }
        [HttpPost]
        [Route("RegisterQueueForPromotions")]
        [MapToApiVersion("1.0")]
        public IActionResult RegisterQueueForPromotions(RegistrationQueue request)
        {
            var result = _ordinalNumberRegistrationService.RegisterQueueForPromotions(request);
            return Ok(result);
        }
        [HttpDelete]
        [Route("CancelPromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult CancelPromotion(string key)
        {
            var result = _resgiterPromotion.CancelPromotion(key);
            return Ok(result);
        }
        [HttpGet]
        [Route("CheckRegistrationForSuccessfulPromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult CheckRegistrationForSuccessfulPromotion(string key)
        {
            var result = _resgiterPromotion.CheckRegistrationForSuccessfulPromotion(key);
            return Ok(result);
        }
    }
}

