using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Paremeters;

namespace MyToDo.Api.Controllers
{
    /// <summary>
    /// 待办事项控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ToDoController : Controller
    {
        private readonly IToDoService _toDoService;

        public ToDoController(IToDoService toDoService)
        {
            this._toDoService = toDoService;
        }

        [HttpGet]
        public async Task<ApiResponse?> Get(int id) => await _toDoService.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse?> GetAll([FromQuery]QueryParameter parameter) => await _toDoService.GetAllAsync(parameter);

        [HttpPost]
        //这里的FromBody特性会将Json格式的Body区域转换成ToDoDto类型
        public async Task<ApiResponse?> Add([FromBody]ToDoDto model)=>await _toDoService.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse?> Update([FromBody]ToDoDto model) => await _toDoService.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse?> Delete(int id) => await _toDoService.DeleteAsync(id);

        
    }
}
