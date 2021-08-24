using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace eShopSolution.Data.EF
{
    public class eShopDbContextFactory : IDesignTimeDbContextFactory<eShopDbContext>
    {
        public eShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var connectionstring = configuration.GetConnectionString("eShopSolutionDb");
            var OptionBuilder = new DbContextOptionsBuilder<eShopDbContext>();
            OptionBuilder.UseSqlServer(connectionstring);

            return new eShopDbContext(OptionBuilder.Options);


        }
    }
}
