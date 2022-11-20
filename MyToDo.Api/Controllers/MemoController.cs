using Arch.EntityFrameworkCore.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using MyToDo.Api.Context;
using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Paremeters;

namespace MyMemo.Api.Controllers
{
    /// <summary>
    /// 备忘录控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MemoController : Controller
    {
        private readonly IMemoService _memoService;

        public MemoController(IMemoService memoService)
        {
            this._memoService = memoService;
        }

        [HttpGet]
        public async Task<ApiResponse?> Get(int id) => await _memoService.GetSingleAsync(id);

        [HttpGet]
        public async Task<ApiResponse?> GetAll([FromQuery]QueryParameter parameter) => await _memoService.GetAllAsync(parameter);

        [HttpPost]
        public async Task<ApiResponse?> Add([FromBody]MemoDto model)=>await _memoService.AddAsync(model);

        [HttpPost]
        public async Task<ApiResponse?> Update([FromBody]MemoDto model) => await _memoService.UpdateAsync(model);

        [HttpDelete]
        public async Task<ApiResponse?> Delete(int id) => await _memoService.DeleteAsync(id);

        
    }
}
