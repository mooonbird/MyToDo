﻿using MyToDo.Shared;
using MyToDo.Shared.Paremeters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyToDo.Services
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<ApiResponse<TEntity>?> AddAsync(TEntity entity);
        Task<ApiResponse<TEntity>?> UpdateAsync(TEntity entity);
        Task<ApiResponse?> DeleteAsync(int id);
        Task<ApiResponse<TEntity>?> GetFirstOfDefaultAsync(int id);
        Task<ApiResponse<PagedList<TEntity>>?> GetAllAsync(QueryParameter parameter);
    }
}
