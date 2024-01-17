using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ODTradePromotion.API.Constants;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace ODTradePromotion.API.HttpClients
{
    public class HttpClientCommon: IHttpClientCommon
    {
        private RestClient _client;
   
        public RestClient Client
        {
            get
            {
                if (_client != null)
                {
                    return _client;
                }
                _client = new RestClient(AppConstant.CommonApiBaseUrl);
                _client.Timeout = (int)TimeSpan.FromMinutes(2).TotalMicroseconds;
                return _client;
            }
        }
        public Task<T> PostAync<T>(string path, object param)
        {
            var request = new RestRequest(path, Method.POST);
            return ExecuteAsync<T>(Client, request, param);
        }
        public Task<T> GetAync<T>(string path, object param)
        {
            var request = new RestRequest(path, Method.GET);
            return ExecuteAsync<T>(Client, request, param);
        }
        public Task<T> PutAync<T>(string path, object param)
        {
            var request = new RestRequest(path, Method.PUT);
            return ExecuteAsync<T>(Client, request, param);
        }
        public Task<T> DeleteAync<T>(string path, object param)
        {
            var request = new RestRequest(path, Method.DELETE);
            return ExecuteAsync<T>(Client, request, param);
        }
        private Task<T> ExecuteAsync<T>(RestClient client, RestRequest restRequest, object param)
        {            
            if (param != null)
            {
                if (restRequest.Method != Method.GET)
                {
                    var json = JsonConvert.SerializeObject(param);
                    restRequest.AddJsonBody(json);
                }
                else
                {
                    if (param is JObject)
                    {
                        foreach (var x in (JObject)param)
                        {
                            string name = x.Key;
                            JToken value = x.Value;
                            restRequest.AddParameter(name, value.ToString());
                        }
                    }
                }
            }
            var taskSource = new TaskCompletionSource<T>();           
            var data = client.Execute<T>(restRequest);         
            if (data.StatusCode == System.Net.HttpStatusCode.OK)
            { 
                taskSource.SetResult(data.Data);
            }
            else
            {
                taskSource.SetException(new Exception(data.StatusDescription, data.ErrorException));
            }

            return taskSource.Task;
        }
    }
}
