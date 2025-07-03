using FinancialControl.Domain.Interfaces.Profiles;
using FinancialControl.Domain.Models;
using FinancialControl.Infrastructure.DbContext;

namespace FinancialControl.Infrastructure.Repositories
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileReadRepository, IProfileWriteRepository
    {
        public ProfileRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
