using Microsoft.EntityFrameworkCore;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Services.Base;
using System;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.Principals
{
    public class PrincipalService : IPrincipalService
    {
        private readonly IBaseRepository<Principal> _principalRepo;
        private readonly string _principleCode;
        public PrincipalService(IBaseRepository<Principal> principalRepo)
        {
            _principalRepo = principalRepo;
            _principleCode = Environment.GetEnvironmentVariable("PRINCIPALCODE");
        }
        public async Task<Principal> GetPrincipal()
        {
            return await _principalRepo.GetAllQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.Code == _principleCode);
        }

        public async Task<bool> IsPrincipalSystem()
        {
            var principalSetting = await GetPrincipal();
            bool isODSystem = principalSetting.IsODSystem == true;
            return !isODSystem;
        }

        public async Task<string> LinkODSystemCode()
        {
            var principalSetting = await GetPrincipal();
            return principalSetting.LinkODSystem;
        }
    }
}
