using BiPapyon.Api.Domain.Models;
using BiPapyon.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiPapyon.Infrastructure.Persistence.EntityConfigurations
{
    public class UserEntityConfiguration : BaseEntityConfiguration<Api.Domain.Models.User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("user", BiPapyonContext.DEFAULT_SCHEMA);
            
            
        }
    }
}
