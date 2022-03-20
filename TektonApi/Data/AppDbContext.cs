using Microsoft.EntityFrameworkCore;
using TektonApi.Data.Entities;

namespace TektonApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Product>(x =>
            {
                x.HasKey(e => e.Id);

                x.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128);

                x.Property(e => e.Price)
                    .IsRequired()
                    .HasPrecision(18, 2);
            });
        }
    }
}
