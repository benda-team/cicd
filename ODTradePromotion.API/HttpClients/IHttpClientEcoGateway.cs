using System.Threading.Tasks;

namespace ODTradePromotion.API.HttpClients
{
    public interface IHttpClientEcoGateway
    {
        Task<T> PostAync<T>(string path, object param);
        Task<T> GetAync<T>(string path, object param);
        Task<T> PutAync<T>(string path, object param);
        Task<T> DeleteAync<T>(string path, object param);
    }
}
