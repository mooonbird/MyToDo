using DryIoc;
using MyToDo.Shared;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Services
{
    public class HttpRestClient
    {
        private readonly string _apiUrl;
        protected readonly RestClient _client;

        public HttpRestClient(string apiUrl)
        {
            this._apiUrl = apiUrl;
            _client = new RestClient();
        }

        public async Task<ApiResponse?> ExecuteAsync(BaseRequest baseRequest)
        {
            var request = new RestRequest(new Uri(_apiUrl + baseRequest.Route), baseRequest.Method);

            if (baseRequest.ContentType != null && baseRequest.Parameter != null)
            {
                request.AddHeader("Content-Type", baseRequest.ContentType);
                //request.AddParameter("Parameter", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);
                request.AddBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            }

            var response = await _client.ExecuteAsync(request);

            if (response.Content != null)
            {
                return JsonConvert.DeserializeObject<ApiResponse>(response.Content);
            }

            return null;
        }

        public async Task<ApiResponse<T>?> ExecuteAsync<T>(BaseRequest baseRequest)
        {
            var request = new RestRequest(new Uri(_apiUrl + baseRequest.Route), baseRequest.Method);

            if (baseRequest.ContentType != null && baseRequest.Parameter != null)
            {
                request.AddHeader("Content-Type", baseRequest.ContentType);
                //request.AddParameter("Parameter", JsonConvert.SerializeObject(baseRequest.Parameter), ParameterType.RequestBody);

                //把Dto转换成Json格式，添加到请求Body中
                request.AddBody(JsonConvert.SerializeObject(baseRequest.Parameter));
            }

            var response = await _client.ExecuteAsync(request);

            if (response.Content != null)
            {
                //把收到的string类型的Json格式的内容(原始类型是ToDo类型)解析成ToDoDto类型
                return JsonConvert.DeserializeObject<ApiResponse<T>>(response.Content);
            }

            return null;
        }
    }
}
