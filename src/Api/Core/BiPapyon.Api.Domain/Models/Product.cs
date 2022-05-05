namespace BiPapyon.Api.Domain.Models
{
    public class Product:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public int Status { get; set; }
        public decimal Price { get; set; }
        public string Tags { get; set; }

        public virtual ICollection<ProductFavorite> ProductFavorites { get; set; }
        public virtual ICollection<ProductVote> ProductVotes { get; set; }

    }
}
