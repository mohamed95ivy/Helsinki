using Helsinki.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Helsinki.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ConversionHistory> Conversions { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var b = modelBuilder.Entity<ConversionHistory>();
            b.HasKey(x => x.ConversionId);
            b.Property(x => x.FromCurrency).IsRequired().HasMaxLength(3);
            b.Property(x => x.ToCurrency).IsRequired().HasMaxLength(3);
            b.Property(x => x.FromAmount).HasPrecision(18, 6);
            b.Property(x => x.ToAmount).HasPrecision(18, 6);
            b.Property(x => x.ExchangeRate).HasPrecision(18, 9);
            b.Property(x => x.UserId).IsRequired();
            b.HasIndex(x => x.UserId);
            b.HasIndex(x => x.ConversionDate);
        }
    }
}
