using ODTradePromotion.API.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.User
{
    public interface IUserService
    {
        public UserModel GetUserInfoByUserName(string UserName);
        public List<RoleModel> GetListRoleInfoByUserName(string UserName);
        public Task<string> GetDistributorCodeOfLoginUserAsync(string userName);
    }
}
