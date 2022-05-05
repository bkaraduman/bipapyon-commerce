using BiPapyon.Api.Application.Interfaces.Repositories;
using BiPapyon.Api.Domain.Models;
using BiPapyon.Infrastructure.Persistence.Context;

namespace BiPapyon.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(BiPapyonContext context) : base(context)
        {
        }
    }
}
