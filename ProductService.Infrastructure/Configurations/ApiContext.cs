using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure.EFCoreInMemory.Models;

namespace ProductService.Infrastructure.Configurations
{
    public class ApiContext : DbContext
    {
        public DbSet<ProductModel> Products { get; set; }

        public ApiContext(DbContextOptions options) : base(options)
        {
            if (Products.Any())
            {
                return;
            }

            LoadProducts();
        }

        public void LoadProducts()
        {
            var productList = new List<ProductModel>()
            { new ProductModel { Id = 1, Description = "", Discount = 25, FinalPrice = 100, Name = "Wallet", 
                                Price = 75, StatusName = "", Stock = 100 },
              new ProductModel { Id = 2, Description = "", Discount = 25, FinalPrice = 100, Name = "Purse",
                                Price = 75, StatusName = "", Stock = 100 }
            };

            Products.AddRange(productList);
            SaveChanges();
        }

        //public List<ProductModel> GetProducts()
        //{
        //    return Products.Local.ToList<ProductModel>();
        //}
    }
}
