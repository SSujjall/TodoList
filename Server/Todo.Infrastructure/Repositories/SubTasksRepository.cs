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
    public class SubTasksRepository : BaseRepository<SubTask>, ISubTasksRepository
    {
        private readonly AppDbContext _context;
        public SubTasksRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
