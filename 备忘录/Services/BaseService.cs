using MaterialDesignColors;
using MyToDo.Shared;
using MyToDo.Shared.Paremeters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
    {
        private readonly HttpRestClient _client;
        private readonly string _serviceName;

        public BaseService(HttpRestClient client, string serviceName)
        {
            this._client = client;
            this._serviceName = serviceName;
        }

        public async Task<ApiResponse<TEntity>?> AddAsync(TEntity entity)
        {
            var request = new BaseRequest()
            {
                Method = RestSharp.Method.Post,
                Route = $"api/{_serviceName}/Add",
                Parameter = entity,
            };

            return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse?> DeleteAsync(int id)
        {
            var request = new BaseRequest()
            {
                Method = RestSharp.Method.Delete,
                Route = $"api/{_serviceName}/Delete?id={id}",
            };

            return await _client.ExecuteAsync(request);
        }

        public async Task<ApiResponse<PagedList<TEntity>>?> GetAllAsync(QueryParameter param)
        {
            var request = new BaseRequest()
            {
                Method = RestSharp.Method.Get,
                Route = $"api/{_serviceName}/GetAll?PageIndex={param.PageIndex}" +
                $"&PageSize={param.PageSize}&Search={param.Search}",
            };

            return await _client.ExecuteAsync<PagedList<TEntity>>(request);
        }

        public async Task<ApiResponse<TEntity>?> GetFirstOfDefaultAsync(int id)
        {
            var request = new BaseRequest()
            {
                Method = RestSharp.Method.Get,
                Route = $"api/{_serviceName}/Get?id={id}",
            };

            return await _client.ExecuteAsync<TEntity>(request);
        }

        public async Task<ApiResponse<TEntity>?> UpdateAsync(TEntity entity)
        {
            var request = new BaseRequest()
            {
                Method = RestSharp.Method.Post,
                Route = $"api/{_serviceName}/Update",
                Parameter = entity,
            };

            return await _client.ExecuteAsync<TEntity>(request);
        }
    }
}
