using BiPapyon.Api.Domain.Models;
using BiPapyon.Common.Infrastructure;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BiPapyon.Infrastructure.Persistence.Context
{
    public class SeedData
    {
        private static List<User> GetUsers()
        {
            var result = new Faker<User>("tr")
                .RuleFor(i => i.Id, i => Guid.NewGuid())
                .RuleFor(i => i.CreatedDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(i => i.FirstName, i => i.Person.FirstName)
            .RuleFor(i => i.LastName, i => i.Person.LastName)
            .RuleFor(i => i.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
            .RuleFor(i => i.Password, i => PasswordEncryptor.Encrypt(i.Internet.Password()))
            .RuleFor(i => i.PhoneNumber, i => i.Phone.PhoneNumber())
            .RuleFor(i => i.EmailConfirmed, i => i.PickRandom(true, false))
            .RuleFor(i => i.Status, 1);


            return result.Generate(100);
        }


        public async Task SeedAsync(IConfiguration configuration)
        {
            var dbContextBuilder = new DbContextOptionsBuilder();

            dbContextBuilder.UseSqlServer(configuration["BiPapyonDbConnectionString"]);

            var context = new BiPapyonContext(dbContextBuilder.Options);

            if (context.Users.Any() || context.Products.Any())
            {
                await Task.CompletedTask;

                return;
            }

            var users = GetUsers();

            var userIds = users.Select(x => x.Id);

            await context.Users.AddRangeAsync(users);

            var guids = Enumerable.Range(0, 150).Select(x => Guid.NewGuid()).ToList();

            int counter = 0;

            var entries = new Faker<Product>("tr")
            .RuleFor(x => x.Id, i => guids[counter++])
            .RuleFor(x => x.Title, i => i.Commerce.ProductName())
            .RuleFor(x => x.Description, i => i.Lorem.Paragraph(2))
            .RuleFor(x => x.Price, i => Convert.ToDecimal(i.Commerce.Price(10, 1000)))
            .RuleFor(x => x.CreatedDate, i => i.Date.Between(DateTime.Now.AddDays(-100), DateTime.Now))
            .RuleFor(x => x.Status, 1)
            .RuleFor(x => x.ImageUrl, i => i.Internet.Avatar())
            .RuleFor(x => x.Tags, i => i.Lorem.Slug(3))
            .Generate(150);

            await context.Products.AddRangeAsync(entries);

            await context.SaveChangesAsync();
        }
    }
}
