using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.HttpClients;
using ODTradePromotion.API.Models.External;
using ODTradePromotion.API.Models.User;
using ODTradePromotion.API.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.User
{
    public class UserService : IUserService
    {
        #region Property
        private readonly ILogger<UserService> _logger;
        private readonly IBaseRepository<Infrastructure.User> _serviceUser;
        private readonly IBaseRepository<Infrastructure.Role> _serviceRole;
        private readonly IBaseRepository<Infrastructure.UserRole> _serviceUserRole;
        private readonly IHttpClientCommon httpClientCommon;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public UserService(ILogger<UserService> logger,
            IBaseRepository<Infrastructure.User> serviceUser,
            IBaseRepository<Infrastructure.Role> serviceRole,
            IBaseRepository<Infrastructure.UserRole> serviceUserRole,
            IHttpClientCommon httpClientCommon,
            IMapper mapper
            )
        {
            _logger = logger;
            _serviceUser = serviceUser;
            _serviceRole = serviceRole;
            _serviceUserRole = serviceUserRole;
            this.httpClientCommon = httpClientCommon;
            _mapper = mapper;
        }
        #endregion

        public UserModel GetUserInfoByUserName(string UserName)
        {
            return _mapper.Map<UserModel>(_serviceUser.GetAllQueryable(x => x.UserName.ToLower().Equals(UserName.ToLower()) && x.IsActive).FirstOrDefault());
        }

        public List<RoleModel> GetListRoleInfoByUserName(string UserName)
        {
            return (from r in _serviceRole.GetAllQueryable(x => x.IsActive).AsNoTracking()
                    join ur in _serviceUserRole.GetAllQueryable().AsNoTracking()
                    on r.Id equals ur.RoleId
                    join u in _serviceUser.GetAllQueryable(x => x.IsActive).AsNoTracking()
                    on ur.UserId equals u.Id
                    where u.UserName.ToLower().Equals(UserName.ToLower())
                    select new RoleModel()
                    {
                        Code = r.Name,
                        FeatureAction = r.FeatureAction
                    }).ToList();
        }

        public async Task<string> GetDistributorCodeOfLoginUserAsync(string userName)
        {
            // gọi api /v{version}/permission/getuserwithdistributorshipto/{UserCode}
            List<UserWithDistributorModel> list = await httpClientCommon.GetAync<List<UserWithDistributorModel>>($"permission/getuserwithdistributorshipto/{userName}", null);
            if (list == null) return string.Empty;
            return list.Select(d => d.DistributorCode).FirstOrDefault();
        }
    }
}
