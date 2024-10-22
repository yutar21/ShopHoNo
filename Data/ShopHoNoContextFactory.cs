using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ShopHoNo.Data
{
    public class ShopHoNoContextFactory : IDesignTimeDbContextFactory<ShopHoNoContext>
    {
        public ShopHoNoContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShopHoNoContext>();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ShopHoNoContext"));

            return new ShopHoNoContext(optionsBuilder.Options);
        }
    }
}
