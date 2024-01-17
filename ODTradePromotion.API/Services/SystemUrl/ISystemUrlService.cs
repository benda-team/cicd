using ODTradePromotion.API.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.SystemUrl
{
    public interface ISystemUrlService
    {
        Task<SystemUrlListModel> GetAllSystemUrl();
    }
}
