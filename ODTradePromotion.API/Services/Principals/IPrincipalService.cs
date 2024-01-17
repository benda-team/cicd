using System.Threading.Tasks;
using ODTradePromotion.API.Infrastructure;

namespace ODTradePromotion.API.Services.Principals
{
    public interface IPrincipalService
    {
        public Task<Principal> GetPrincipal();
        public Task<bool> IsPrincipalSystem();
        public Task<string> LinkODSystemCode();
    }
}
