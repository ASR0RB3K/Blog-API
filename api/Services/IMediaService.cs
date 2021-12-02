using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entity;

namespace api.Services
{
    public interface IMediaService
    {
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(IEnumerable<Guid> id);
        Task<List<Media>> GetAllAsync();
        Task<List<Media>> GetAllAsync(IEnumerable<Guid> id);
        Task <Media> GetAsync(Guid id);
        Task<(bool IsSuccess, Exception exception)> InsertAsync(List<Media> media);
        Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id);
    }
}