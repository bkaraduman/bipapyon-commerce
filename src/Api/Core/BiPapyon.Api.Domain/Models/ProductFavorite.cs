namespace BiPapyon.Api.Domain.Models
{
    public class ProductFavorite : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Guid CreatedById { get; set; }
        
        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
    }
}
