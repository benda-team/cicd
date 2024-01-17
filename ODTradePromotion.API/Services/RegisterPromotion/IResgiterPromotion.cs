using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Budget;
using Sys.Common.Models;

namespace ODTradePromotion.API.Services.RegisterPromotion
{
    public interface IResgiterPromotion
    {
        Result<bool> ResgiterPromotion(TpBudgetUsed request);
        Result<bool> CancelPromotion(string key);
        Result<string> CheckRegistrationForSuccessfulPromotion(string key);
    }
}
