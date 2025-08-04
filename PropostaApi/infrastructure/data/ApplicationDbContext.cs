using infrastructure.data.models;
using Microsoft.EntityFrameworkCore;

namespace infrastructure.data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<PropostaModel> Propostas { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

     
    }
}