﻿using Microsoft.AspNetCore.Mvc;

namespace UrlShortner.Interface
{
    public interface IMongoRepository<T> where T : class
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }

   
}
