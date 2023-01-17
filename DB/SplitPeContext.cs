using Microsoft.EntityFrameworkCore;
using SplitPeWebAPI.Models;
namespace SplitPeWebAPI.DB
{
    public class SplitPeContext: DbContext
    {
        private readonly IConfiguration configuration;

        public SplitPeContext(IConfiguration configuration)
        {
            this.configuration = configuration;
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("db"));
            //optionsBuilder.UseMySQL("server=127.0.0.1;port=3306;user=root;password=hypervenomctr360;database=splitpe");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OweTo>().HasNoKey().ToView("Dummy");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<PersonalExpense> PersonalExpenses { get; set; }
        public DbSet<SplitExpense> SplitExpenses { get; set; }

        public DbSet<Due> Dues { get; set; }

        public DbSet<OweTo> OweTos { get; set; }
        public DbSet<OweBy> OweBys { get; set; }

    }
}
