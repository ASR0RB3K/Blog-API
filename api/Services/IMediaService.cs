using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.Entity;

namespace api.Services
{
    public interface IMediaService
    {
        Task<bool> ExistAsync(Guid id);
        Task<Media> GetAsync(Guid id);
        Task<List<Media>> GetAllAsync();
        Task<(bool IsSuccess, Exception exception)> InsertAsync(List<Media> media);
        Task<(bool IsSuccess, Exception exception)> DeleteAsync(Guid id);
    }
}