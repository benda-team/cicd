using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace ODTradePromotion.API.Services.Base
{
    public class DapperRepositories : IDapperRepositories, IDisposable
    {
        private NpgsqlConnection _connection;
        private readonly string _cnn;
        private readonly ILogger<DapperRepositories> _logger;
        public DapperRepositories(ILogger<DapperRepositories> logger)
        {
            _logger = logger;
            _cnn = Environment.GetEnvironmentVariable("CONNECTION");

            OpenConnection();
        }

        public IDbConnection OpenConnection()
        {
            try
            {
                _connection = new NpgsqlConnection(_cnn);
                _connection.Open();
                return _connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }
        public async Task<IEnumerable<T>> Query<T>(string query)
        {
            IEnumerable<T> Result;
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                    OpenConnection();
                Result = await _connection.QueryAsync<T>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Result = null;
            }
            finally
            {
                _connection.Close();
            }
            return Result;
        }
        public async Task<IEnumerable<T>> QueryWithParams<T>(string query, DynamicParameters parameters)
        {
            IEnumerable<T> Result;
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                    OpenConnection();
                Result = await _connection.QueryAsync<T>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Result = null;
            }
            finally
            {
                _connection.Close();
            }
            return Result;
        }
        public async Task<T> GetAll<T>(string tableName)
        {
            string commandText = $"SELECT * FROM {tableName} FOR UPDATE SKIP LOCKED";
            var game = await _connection.QueryFirstAsync<T>(commandText);
            return game;
        }

        public async Task<T> QuerySingle<T>(string query) where T : new()
        {
            T Result;
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                    OpenConnection();
                Result = await _connection.QuerySingleAsync<T>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Result = new T();
            }
            finally
            {
                _connection.Close();
            }
            return Result;
        }

        public void BulkInsert<T>(string tableName, List<T> data)
        {

            using (var writer = _connection.BeginBinaryImport($"copy {tableName} from STDIN (FORMAT BINARY)"))
            {
                foreach (var element in data)
                {
                    writer.StartRow();

                }

                writer.Complete();
            }
        }

        public async Task<GridReader> QueryMultiple(string query)
        {
            GridReader Result;
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                    OpenConnection();
                Result = await _connection.QueryMultipleAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Result = null;
            }
            finally
            {
            }
            return Result;
        }

        private bool _disposed = false;

        ~DapperRepositories() =>
            Dispose();

        public void Dispose()
        {
            if (!_disposed)
            {
                _connection.Close();
                _connection.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }

        public object QueryParams<T>(string query, DynamicParameters parameters)
        {
            object Result;
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                    OpenConnection();
                Result = _connection.Query<T>(query, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Result = null;
            }
            finally
            {
                _connection.Close();
            }
            return Result;
        }

        public object QueryWithoutSync<T>(string query)
        {
            object Result;
            try
            {
                if (_connection == null || _connection.State != ConnectionState.Open)
                    OpenConnection();
                Result = _connection.Query<T>(query);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Result = null;
            }
            finally
            {
                _connection.Close();
            }
            return Result;
        }
    }
}
