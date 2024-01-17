using ODTradePromotion.API.Models.UserWithDistributor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.UserWithDistributor
{
    public interface IUserWithDistributorService
    {
        Task<List<UserWithDistributorModel>> GetUserWithDistributorsAsync(string UserName);
    }
}
