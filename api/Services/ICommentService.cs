using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entity;

namespace api.Services
{
    public interface ICommentService
    {
        Task<bool> ExistsAsync(Guid id);
        Task<Comment> GetAsync(Guid id);
        Task<List<Comment>> GetAllAsync();
        Task<(bool IsSuccess, Exception exception)> InsertAsync(Comment comment);
        Task<(bool IsSuccess, Exception exception)> UpdateAsync(Comment comment);
        Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id);
    }
}