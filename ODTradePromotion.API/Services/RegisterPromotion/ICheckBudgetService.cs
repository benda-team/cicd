using Sys.Common.Models;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.RegisterPromotion
{
    public interface ICheckBudgetService
    {
        Task<Result<bool>> CheckBudgetAndResponeForRegisterPromotion();
    }
}
