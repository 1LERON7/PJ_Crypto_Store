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

    public virtual DbSet<Favorite> Favorites { get; set; }

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

        modelBuilder.Entity<Favorite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("favorites_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Product).WithMany(p => p.Favorites).HasConstraintName("favorites_product_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Favorites).HasConstraintName("favorites_user_id_fkey");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created).HasDefaultValueSql("now()");
            entity.Property(e => e.Status).HasDefaultValueSql("'created'::text");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_items_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems).HasConstraintName("order_items_order_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_items_product_id_fkey");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payments_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Confirmations).HasDefaultValue(0);
            entity.Property(e => e.Created).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments).HasConstraintName("payments_order_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("products_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created).HasDefaultValueSql("now()");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.Property(e => e.Id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.Created).HasDefaultValueSql("now()");
            entity.Property(e => e.Role).HasDefaultValueSql("'user'::text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
