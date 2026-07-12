using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class StockMarketDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");

            //Fluent API configurations can be added here if needed
            modelBuilder.Entity<BuyOrder>()
                .Property(temp => temp.StockSymbol)
                .HasColumnName("StockSymbol")
                .HasColumnType("nvarchar(12)");

            modelBuilder.Entity<BuyOrder>()
                .Property(temp => temp.StockName)
                .HasColumnName("StockName")
                .HasColumnType("nvarchar(60)");

            modelBuilder.Entity<BuyOrder>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_BuyOrders_Quantity", "[Quantity] >= 1 AND [Quantity] <= 100000");
                    t.HasCheckConstraint("CK_BuyOrders_Price", "[Price] >= 1 AND [Price] <= 100000");
                });

            modelBuilder.Entity<SellOrder>()
                .Property(temp => temp.StockSymbol)
                .HasColumnName("StockSymbol")
                .HasColumnType("nvarchar(12)");

            modelBuilder.Entity<SellOrder>()
                .Property(temp => temp.StockName)
                .HasColumnName("StockName")
                .HasColumnType("nvarchar(60)");

            modelBuilder.Entity<SellOrder>()
                .ToTable(t =>
                {
                    t.HasCheckConstraint("CK_SellOrders_Quantity", "[Quantity] >= 1 AND [Quantity] <= 100000");
                    t.HasCheckConstraint("CK_SellOrders_Price", "[Price] >= 1 AND [Price] <= 100000");
                });
        }
    }
}
