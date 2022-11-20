using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;

namespace MyToDo.Api.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoginService(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }
        public async Task<ApiResponse> LoginAsync(UserDto userDto)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: e =>
                e.Account.Equals(userDto.Account) && e.Password.Equals(userDto.Password));

                if (user == null)
                {
                    return new ApiResponse("账号或密码错误,请重试！");
                }

                return new ApiResponse(true, user);
            }
            catch (Exception)
            {
                return new ApiResponse("登录失败");
            }
        }

        public async Task<ApiResponse> RegisterAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var userCurrent = await _unitOfWork.GetRepository<User>().GetFirstOrDefaultAsync(predicate: e =>
                e.Account.Equals(user.Account));
                if (userCurrent != null)
                {
                    return new ApiResponse($"用户账号: {user.Account} 已存在，不能重复注册！");
                }

                user.CreateDate = DateTime.Now;
                var result = await _unitOfWork.GetRepository<User>().InsertAsync(user);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, user);
                }
                return new ApiResponse("注册失败，请重新注册！");
            }
            catch (Exception)
            {
                return new ApiResponse("注册失败!");
            }
        }
    }
}
