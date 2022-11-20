using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Paremeters;

namespace MyToDo.Api.Services
{
    /// <summary>
    /// 代办事项服务
    /// </summary>
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public ToDoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._UnitOfWork = unitOfWork;
            this._Mapper = mapper;
        }
        public async Task<ApiResponse> AddAsync(ToDoDto toDoDto)
        {
            try
            {
                //ToDoDto-->ToDo
                var todo = _Mapper.Map<ToDo>(toDoDto);
                _ = await _UnitOfWork.GetRepository<ToDo>().InsertAsync(todo);
                if (await _UnitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, todo);
                }

                return new ApiResponse("添加数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
            
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            try
            {
                var repository = _UnitOfWork.GetRepository<ToDo>();
                var todo = await repository
                    .GetFirstOrDefaultAsync(predicate: e => e.Id.Equals(id));

                repository.Delete(todo);

                if (await _UnitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, "");
                }

                return new ApiResponse("删除数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetAllAsync(QueryParameter parameter)
        {
            try
            {
                var repository = _UnitOfWork.GetRepository<ToDo>();
                var todos = await repository.GetPagedListAsync(predicate: e =>
                string.IsNullOrWhiteSpace(parameter.Search) ? true : e.Title!.Contains(parameter.Search),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                ignoreQueryFilters:true,
                orderBy: result => result.OrderByDescending(e => e.CreateDate));

                return new ApiResponse(true,todos);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> GetSingleAsync(int id)
        {
            try
            {
                var repository = _UnitOfWork.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate:e=>e.Id.Equals(id));

                return new ApiResponse(true, todo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(ToDoDto toDoDto)
        {
            try
            {
                var dbTodo = _Mapper.Map<ToDo>(toDoDto);
                var repository = _UnitOfWork.GetRepository<ToDo>();
                var todo = await repository.GetFirstOrDefaultAsync(predicate: e => e.Id.Equals(dbTodo.Id));


                todo.Title = dbTodo.Title;
                todo.Content = dbTodo.Content;
                todo.UpdateDate = DateTime.Now;
                todo.Status = dbTodo.Status;

                repository.Update(todo);

                if (await _UnitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, todo);
                }

                return new ApiResponse("更新数据失败");
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }
    }
}
