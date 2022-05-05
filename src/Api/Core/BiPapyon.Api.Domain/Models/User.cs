namespace BiPapyon.Api.Domain.Models
{
    public class User:BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }
        public int Status { get; set; }
        public virtual ICollection<ProductFavorite>  ProductFavorites { get; set; }
    }
}
