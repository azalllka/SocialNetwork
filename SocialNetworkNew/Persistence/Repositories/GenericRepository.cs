using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Interfaces.Repositories;
using SocialNetwork.Domain.Common.Interfaces;
using SocialNetwork.Persistence.Contexts;

namespace SocialNetwork.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        private readonly ApplicationDbContext _dbContext;
        public IQueryable<T> Entities => _dbContext.Set<T>();

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbContext.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Set<T>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
} 