using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpRestClient _httpRestClient;

        public LoginService(HttpRestClient httpRestClient)
        {
            this._httpRestClient = httpRestClient;
        }
        public Task<ApiResponse<UserDto>?> LoginAsync(UserDto userDto)
        {
            var request = new BaseRequest()
            {
                Method = RestSharp.Method.Get,
                Route = $"api/Login/Login",
                Parameter = userDto,
            };
            return _httpRestClient.ExecuteAsync<UserDto>(request);

        }

        public Task<ApiResponse?> RegiterAsync(UserDto userDto)
        {
            var request = new BaseRequest()
            {
                Method = RestSharp.Method.Post,
                Route = $"api/Login/Register",
                Parameter = userDto,
            };
            return _httpRestClient.ExecuteAsync(request);
        }
    }
}
