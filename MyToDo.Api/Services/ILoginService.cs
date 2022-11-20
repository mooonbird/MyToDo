using MyToDo.Shared;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services
{
    public interface ILoginService
    {
        Task<ApiResponse> LoginAsync(UserDto userDto);
        Task<ApiResponse> RegisterAsync(UserDto userDto);
    }
}
