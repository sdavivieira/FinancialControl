using FinancialControl.Domain.Interfaces.Users;
using FinancialControl.Domain.Models;
using FinancialControl.Infrastructure.DbContext;
using FinancialControl.ResponseRequest;
using Microsoft.EntityFrameworkCore;
using System.Buffers;
using System.Linq.Expressions;

namespace FinancialControl.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserReadRepository, IUserWriteRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext dbcontext) : base(dbcontext)
        {
            _context = dbcontext;
        }      
    }
}
