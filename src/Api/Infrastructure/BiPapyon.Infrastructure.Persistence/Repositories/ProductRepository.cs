using BiPapyon.Api.Application.Interfaces.Repositories;
using BiPapyon.Api.Domain.Models;
using BiPapyon.Infrastructure.Persistence.Context;

namespace BiPapyon.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(BiPapyonContext context) : base(context)
        {
        }
    }
}
