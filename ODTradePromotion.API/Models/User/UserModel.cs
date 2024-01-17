using ODTradePromotion.API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Models.User
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public string UserCode { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Guid? ActiveSessionId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int UserTypeId { get; set; }

        public bool IsDefault { get; set; }
        public string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        /// <summary>
        /// UserTypeId == 5
        /// </summary>
        public bool IsDistributorUser => UserTypeId == (int)UserTypeCodeEnum.DistributorType;
    }
}
