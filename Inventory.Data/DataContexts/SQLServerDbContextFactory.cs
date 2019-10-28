using Microsoft.EntityFrameworkCore.Design;

namespace Inventory.Data.Services
{
    public class SQLServerDbContextFactory : IDesignTimeDbContextFactory<SQLServerDb>
    {
        public SQLServerDb CreateDbContext(string[] args)
        {
            return new SQLServerDb("Data Source=localhost;Initial Catalog=DeliveryPro;Integrated Security=SSPI");
        }
    }
}
