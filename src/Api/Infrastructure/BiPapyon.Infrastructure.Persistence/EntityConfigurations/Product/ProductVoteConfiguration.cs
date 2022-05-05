using BiPapyon.Api.Domain.Models;
using BiPapyon.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiPapyon.Infrastructure.Persistence.EntityConfigurations.Product
{
    public class ProductVoteConfiguration : BaseEntityConfiguration<Api.Domain.Models.ProductVote>
    {
        public override void Configure(EntityTypeBuilder<ProductVote> builder)
        {
            base.Configure(builder);

            builder.ToTable("productvote", BiPapyonContext.DEFAULT_SCHEMA);

            builder.HasOne(i => i.Product)
                .WithMany(i => i.ProductVotes)
                .HasForeignKey(i => i.ProductId);
        }
    }
}
