using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using Sys.Common.Models;

namespace ODTradePromotion.API.Services.RegisterPromotion
{
    public interface IOrdinalNumberRegistrationService
    {
        Result<bool> RegisterQueueForPromotions(RegistrationQueue request);
    }
}
