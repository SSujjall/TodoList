using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Helpers;
using Todo.Application.Interface.IRepositories;
using Todo.Infrastructure.Context;

namespace Todo.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            var addedEntity = (await _context.AddAsync(entity)).Entity;
            _context.SaveChanges();
            return addedEntity;
        }

        public async Task Delete(T entity)
        {
            if (entity != null)
            {
                _context.Remove(entity); // Remove the entity
                await _context.SaveChangesAsync(); // Commit asynchronously
            }
        }

        public async Task<IEnumerable<T>> GetAll(GetRequest<T>? request)
        {
            IQueryable<T> query = _context.Set<T>();

            if (request != null)
            {
                if (request.Filter != null)
                {
                    query = query.Where(request.Filter);
                }

                if (request.OrderBy != null)
                {
                    query = request.OrderBy(query);
                }

                if (request.Skip.HasValue)
                {
                    query = query.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    query = query.Take(request.Take.Value);
                }
            }

            return await query.ToListAsync(); // Use asynchronous database operations
        }

        public async Task<T>? GetById(object entityId)
        {
            return await _context.FindAsync<T>(entityId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            var updatedEntity = _context.Update(entity).Entity;
            await _context.SaveChangesAsync();
            return updatedEntity;
        }
    }
}