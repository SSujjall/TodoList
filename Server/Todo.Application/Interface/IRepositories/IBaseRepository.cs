using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Helpers;

namespace Todo.Application.Interface.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<T>? GetById(object entityId);
        Task<IEnumerable<T>> GetAll(GetRequest<T>? request);
        Task SaveChangesAsync();
    }
}