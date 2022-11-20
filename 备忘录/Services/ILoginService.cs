using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Services
{
    public interface ILoginService
    {
        Task<ApiResponse<UserDto>?> LoginAsync(UserDto userDto);
        Task<ApiResponse?> RegiterAsync(UserDto userDto);
    }
}
