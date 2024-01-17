using Microsoft.AspNetCore.Http;
using ODTradePromotion.API.Constants;
using ODTradePromotion.API.Services.Principals;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.Promotion
{
    public class PromotionExternalService : IPromotionExternalService
    {
        private readonly IPrincipalService principalService;
        private readonly IPromotionService promotionService;
        private readonly IHttpContextAccessor httpContext;

        public PromotionExternalService(
            IPrincipalService principalService,
            IPromotionService promotionService,
            IHttpContextAccessor httpContext
            )
        {
            this.principalService = principalService;
            this.promotionService = promotionService;
            this.httpContext = httpContext;
        }

        public async Task HandleSyncToODAsync(string promotionCode, string userName, string token)
        {
            // get
            var promotion = await promotionService.GetDetailPromotionByCodeDapper(promotionCode);

            // map
            promotion.UserName = userName;

            // call rest api sync to OD system
            string BaseInitialODAPI =
               AppConstant.SystemUrlConstant?
               .Where(d => d.Code == SystemUrlCode.ODTpAPI)
               .Select(d => d.Url)
               .FirstOrDefault();
            string ODCode = await principalService.LinkODSystemCode();
            var BaseODAPI = BaseInitialODAPI.Replace(AppConstant.PrincipleCodeServiceUrlConstant.ToLower(), ODCode.ToLower());
            RestClient client = new RestClient(BaseODAPI);
            string tokenSplit = token.Split(" ").Last();
            client.Authenticator = new JwtAuthenticator($"Rdos {tokenSplit}");
            RestRequest request = new RestRequest("Sync/CreatePromotion", Method.POST, DataFormat.Json);
            request.AddJsonBody(promotion);
            await client.ExecuteAsync(request);
        }
    }
}
