using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using MyToDo.Api.Context;
using MyToDo.Api.Services;
using MyToDo.Shared;
using MyToDo.Shared.Dtos;
using MyToDo.Shared.Paremeters;

namespace MyToDo.Api.Services
{
    public class MemoService:IMemoService
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _Mapper;

        public MemoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._UnitOfWork = unitOfWork;
            this._Mapper = mapper;
        }


        public async Task<ApiResponse> AddAsync(MemoDto model)
        {
            try
            {
                var memo = _Mapper.Map<Memo>(model);
                await _UnitOfWork.GetRepository<Memo>().InsertAsync(memo);
                if (await _UnitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, memo);
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
                var repository = _UnitOfWork.GetRepository<Memo>();
                var memo = await repository
                    .GetFirstOrDefaultAsync(predicate: e => e.Id.Equals(id));

                repository.Delete(memo);

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
                var repository = _UnitOfWork.GetRepository<Memo>();
                var todos = await repository.GetPagedListAsync(predicate: e =>
                string.IsNullOrWhiteSpace(parameter.Search) ? true : e.Title.Equals(parameter.Search),
                pageIndex: parameter.PageIndex,
                pageSize: parameter.PageSize,
                orderBy: result => result.OrderByDescending(e => e.CreateDate));

                return new ApiResponse(true, todos);
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
                var repository = _UnitOfWork.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: e => e.Id.Equals(id));

                return new ApiResponse(true, memo);
            }
            catch (Exception ex)
            {
                return new ApiResponse(ex.Message);
            }
        }

        public async Task<ApiResponse> UpdateAsync(MemoDto model)
        {
            try
            {
                var dbMemo = _Mapper.Map<Memo>(model);
                var repository = _UnitOfWork.GetRepository<Memo>();
                var memo = await repository.GetFirstOrDefaultAsync(predicate: e => e.Id.Equals(dbMemo.Id));


                memo.Title = dbMemo.Title;
                memo.Content = dbMemo.Content;
                memo.UpdateDate = DateTime.Now;

                repository.Update(memo);

                if (await _UnitOfWork.SaveChangesAsync() > 0)
                {
                    return new ApiResponse(true, memo);
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
