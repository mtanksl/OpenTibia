using Microsoft.EntityFrameworkCore;
using OpenTibia.Data.Contexts;

namespace OpenTibia.Data.InMemory.Contexts
{
    public class InMemoryContext : DatabaseContext
    {
        public InMemoryContext() : base()
        {
           
        }

        public InMemoryContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("mtots");

            base.OnConfiguring(optionsBuilder);
        }
    }
}