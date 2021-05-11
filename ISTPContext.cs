using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace ISTPLR_one
{
    public partial class ISTPContext : DbContext
    {
        public ISTPContext()
        {
        }

        public ISTPContext(DbContextOptions<ISTPContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cashier> Cashiers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<CoffeeShop> CoffeeShops { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Position> Positions { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-OJTBMVP; Database=ISTP; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<Cashier>(entity =>
            {
                entity.Property(e => e.CashierId).HasColumnName("CashierID");

                entity.Property(e => e.CoffeeShopId).HasColumnName("CoffeeShopID");

                entity.Property(e => e.Fathername)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CoffeeShop)
                    .WithMany(p => p.Cashiers)
                    .HasForeignKey(d => d.CoffeeShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cashiers_CoffeeShops");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.ClientId).HasColumnName("ClientID");

                entity.Property(e => e.FavoriteProductId).HasColumnName("FavoriteProductID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsFixedLength(true);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(18)
                    .IsFixedLength(true);

                entity.HasOne(d => d.FavoriteProduct)
                    .WithMany(p => p.Clients)
                    .HasForeignKey(d => d.FavoriteProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Clients_Products");
            });

            modelBuilder.Entity<CoffeeShop>(entity =>
            {
                entity.Property(e => e.CoffeeShopId).HasColumnName("CoffeeShopID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.CoffeeShops)
                    .HasForeignKey(d => d.OwnerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CoffeeShops_Owners");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.BonusId).HasColumnName("BonusID");

                entity.Property(e => e.CashierId).HasColumnName("CashierID");

                entity.Property(e => e.CoffeeShopId).HasColumnName("CoffeeShopID");

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.Bonus)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.BonusId)
                    .HasConstraintName("FK_Orders_Clients");

                entity.HasOne(d => d.Cashier)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CashierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Cashiers");

                entity.HasOne(d => d.CoffeeShop)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CoffeeShopId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_CoffeeShops");
            });

            modelBuilder.Entity<Owner>(entity =>
            {
                entity.Property(e => e.OwnerId).HasColumnName("OwnerID");

                entity.Property(e => e.Fathername)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(19);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.Property(e => e.PositionId).HasColumnName("PositionID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Positions_Orders");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Positions)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Positions_Products");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description).HasColumnType("ntext");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Categories");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
