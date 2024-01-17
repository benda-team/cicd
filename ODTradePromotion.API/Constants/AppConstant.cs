using ODTradePromotion.API.Models;
using System.Collections.Generic;

namespace ODTradePromotion.API.Constants
{
    public static class AppConstant
    {
        public static List<SystemUrlModel> SystemUrlConstant { get; set; }
        public static string CommonApiBaseUrl { get; set; }
        /// <summary>
        /// Biến này để define, sẽ được replace OD system Code
        /// </summary>
        public const string PrincipleCodeServiceUrlConstant = "principal";
    }
}
