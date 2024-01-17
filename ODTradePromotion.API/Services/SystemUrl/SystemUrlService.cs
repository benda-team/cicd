using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Services.Base;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ODTradePromotion.API.Services.SystemUrl
{
    public class SystemUrlService : ISystemUrlService
    {
        private IRestClient _client;
        public SystemUrlService()
        {
            _client = new RestClient(Environment.GetEnvironmentVariable("ECOGATEWAY"));
        }

        public async Task<SystemUrlListModel> GetAllSystemUrl()
        {
            try
            {
                var request = new RestRequest($"systemurl/getallsystemurl", Method.GET, DataFormat.Json);
                var response = _client.Execute(request);
                if (response == null || response.Content == string.Empty) return new SystemUrlListModel();
                var returnObject = JsonConvert.DeserializeObject<SystemUrlListModel>(JsonConvert.DeserializeObject(response.Content).ToString());
                return returnObject;
            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
