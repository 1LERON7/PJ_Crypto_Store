using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<order> Orders { get; set; }

    public virtual DbSet<order_item> OrderItems { get; set; }

    public virtual DbSet<payment> Payments { get; set; }

    public virtual DbSet<product> Products { get; set; }

    public virtual DbSet<user> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=crypto_store;Username=postgres;Password=Kiskis7878");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("payment_status", new[] { "created", "pending", "confirmed", "failed" })
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.HasIndex(e => e.UserId, "idx_orders_user_id");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Status).HasDefaultValueSql("'created'::text");
            entity.Property(e => e.TotalPrice).HasPrecision(18, 8);

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<order_item>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_items_pkey");

            entity.HasIndex(e => e.OrderId, "idx_order_items_order_id");

            entity.HasIndex(e => e.ProductId, "idx_order_items_product_id");

            entity.HasIndex(e => new { e.OrderId, e.ProductId }, "order_items_order_id_product_id_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Price).HasPrecision(18, 8);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("order_items_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_items_product_id_fkey");
        });

        modelBuilder.Entity<payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.HasIndex(e => e.OrderId, "idx_payments_order_id");

            entity.HasIndex(e => e.TxHash, "payments_tx_hash_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("payments_order_id_fkey");
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.Price).HasPrecision(18, 8);
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.RefreshTokenExpiry).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Role).HasDefaultValueSql("'user'::text");

            entity.HasMany(d => d.Products).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "favorite",
                    r => r.HasOne<product>().WithMany()
                        .HasForeignKey("product_id")
                        .HasConstraintName("favorites_product_id_fkey"),
                    l => l.HasOne<user>().WithMany()
                        .HasForeignKey("user_id")
                        .HasConstraintName("favorites_user_id_fkey"),
                    j =>
                    {
                        j.HasKey("user_id", "product_id").HasName("favorites_pkey");
                        j.ToTable("favorites");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
