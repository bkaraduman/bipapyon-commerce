using BiPapyon.Api.Domain.Models;
using BiPapyon.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BiPapyon.Infrastructure.Persistence.EntityConfigurations.Product
{
    public class ProductFavoriteEntityConfiguration : BaseEntityConfiguration<ProductFavorite>
    {
        public override void Configure(EntityTypeBuilder<ProductFavorite> builder)
        {
            base.Configure(builder);

            builder.ToTable("productfavorite", BiPapyonContext.DEFAULT_SCHEMA);

            builder.HasOne(i => i.Product)
                .WithMany(i => i.ProductFavorites)
                .HasForeignKey(i => i.ProductId);


            builder.HasOne(i => i.User)
                .WithMany(i => i.ProductFavorites)
                .HasForeignKey(i => i.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
