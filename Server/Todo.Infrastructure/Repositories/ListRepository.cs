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
    public class ListRepository : BaseRepository<List>, IListRepository
    {
        private readonly AppDbContext _context;
        public ListRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}