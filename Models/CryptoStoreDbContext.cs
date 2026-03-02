using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

public partial class CryptoStoreDbContext : DbContext
{
    public CryptoStoreDbContext()
    {
    }

    public CryptoStoreDbContext(DbContextOptions<CryptoStoreDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=crypto_store_db;Username=postgres;Password=Kiskis7878");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("payment_status", new[] { "created", "pending", "confirmed", "failed" })
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.HasIndex(e => e.UserId, "idx_orders_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'created'::text")
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(18, 8)
                .HasColumnName("total_price");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_items_pkey");

            entity.ToTable("order_items");

            entity.HasIndex(e => e.OrderId, "idx_order_items_order_id");

            entity.HasIndex(e => e.ProductId, "idx_order_items_product_id");

            entity.HasIndex(e => new { e.OrderId, e.ProductId }, "order_items_order_id_product_id_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.Price)
                .HasPrecision(18, 8)
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_items_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_items_product_id_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.ToTable("payments");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Confirmations)
                .HasDefaultValue(0)
                .HasColumnName("confirmations");
            entity.Property(e => e.ConfirmedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("confirmed_at");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created");
            entity.Property(e => e.Currency).HasColumnName("currency");
            entity.Property(e => e.Network).HasColumnName("network");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.TxHash).HasColumnName("tx_hash");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("payments_order_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Created) 
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.ImageUrl).HasColumnName("image_url");
            entity.Property(e => e.Price)
                .HasPrecision(18, 8)
                .HasColumnName("price");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnName("created");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.RefreshTokenTime).HasColumnName("refresh_token_time");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'user'::text")
                .HasColumnName("role");

            //entity.HasMany(d => d.Products).WithMany(p => p.Users)
            //    .UsingEntity<Dictionary<string, object>>(
            //        "Favorite",
            //        r => r.HasOne<Product>().WithMany()
            //            .HasForeignKey("ProductId")
            //            .HasConstraintName("favorites_product_id_fkey"),
            //        l => l.HasOne<User>().WithMany()
            //            .HasForeignKey("UserId")
            //            .HasConstraintName("favorites_user_id_fkey"),
            //        j =>
            //        {
            //            j.HasKey("UserId", "ProductId").HasName("favorites_pkey");
            //            j.ToTable("favorites");
            //            j.IndexerProperty<Guid>("UserId").HasColumnName("user_id");
            //            j.IndexerProperty<Guid>("ProductId").HasColumnName("product_id");
            //        });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
