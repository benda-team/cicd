using ODTradePromotion.API.HttpClients;
using ODTradePromotion.API.Models.UserWithDistributor;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.UserWithDistributor
{
    public class UserWithDistributorService : IUserWithDistributorService
    {
        private readonly HttpClientCommon httpClientCommon;

        public UserWithDistributorService(
            HttpClientCommon httpClientCommon
            )
        {
            this.httpClientCommon = httpClientCommon;
        }
        /// <summary>
        /// Gọi từ API common: /v{version}/permission/getuserwithdistributorshipto/{UserCode}
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public async Task<List<UserWithDistributorModel>> GetUserWithDistributorsAsync(string UserName)
        {
            // gọi api /v{version}/permission/getuserwithdistributorshipto/{UserCode}
            List<UserWithDistributorModel> list = await httpClientCommon.GetAync<List<UserWithDistributorModel>>($"permission/getuserwithdistributorshipto/{UserName}", null);
           
            return list;
        }
    }
}
