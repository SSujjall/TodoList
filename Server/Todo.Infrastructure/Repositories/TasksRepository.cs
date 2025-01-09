using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Application.Interface.IRepositories;
using Todo.Domain.Entities;
using Todo.Infrastructure.Context;

namespace Todo.Infrastructure.Repositories
{
    public class TasksRepository : BaseRepository<Tasks>, ITasksRepository
    {
        private readonly AppDbContext _context;

        public TasksRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
