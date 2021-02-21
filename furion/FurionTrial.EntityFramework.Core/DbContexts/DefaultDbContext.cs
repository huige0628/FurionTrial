using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;

namespace FurionTrial.EntityFramework.Core
{
    [AppDbContext("FurionTrial", DbProvider.Sqlite)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
        }
    }
}