using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace ODTradePromotion.API.Services.Base
{
    public interface IDapperRepositories
    {
        Task<IEnumerable<T>> Query<T>(string query);
        Task<IEnumerable<T>> QueryWithParams<T>(string query, DynamicParameters parameters);
        Task<T> GetAll<T>(string tableName);
        Task<T> QuerySingle<T>(string query) where T : new();
        void BulkInsert<T>(string tableName, List<T> data);
        Task<GridReader> QueryMultiple(string query);
        object QueryParams<T>(string query, DynamicParameters parameters);
        object QueryWithoutSync<T>(string query);
    }
}
