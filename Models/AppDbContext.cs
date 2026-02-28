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

    public virtual DbSet<order> orders { get; set; }

    public virtual DbSet<order_item> order_items { get; set; }

    public virtual DbSet<payment> payments { get; set; }

    public virtual DbSet<product> products { get; set; }

    public virtual DbSet<user> users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=crypto_store_db;Username=postgres;Password=Kiskis7878");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("payment_status", new[] { "created", "pending", "confirmed", "failed" })
            .HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<order>(entity =>
        {
            entity.HasKey(e => e.id).HasName("orders_pkey");

            entity.HasIndex(e => e.user_id, "idx_orders_user_id");

            entity.Property(e => e.id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.status).HasDefaultValueSql("'created'::text");
            entity.Property(e => e.total_price).HasPrecision(18, 8);

            entity.HasOne(d => d.user).WithMany(p => p.orders)
                .HasForeignKey(d => d.user_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_user_id_fkey");
        });

        modelBuilder.Entity<order_item>(entity =>
        {
            entity.HasKey(e => e.id).HasName("order_items_pkey");

            entity.HasIndex(e => e.order_id, "idx_order_items_order_id");

            entity.HasIndex(e => e.product_id, "idx_order_items_product_id");

            entity.HasIndex(e => new { e.order_id, e.product_id }, "order_items_order_id_product_id_key").IsUnique();

            entity.Property(e => e.id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.price).HasPrecision(18, 8);

            entity.HasOne(d => d.order).WithMany(p => p.order_items)
                .HasForeignKey(d => d.order_id)
                .HasConstraintName("order_items_order_id_fkey");

            entity.HasOne(d => d.product).WithMany(p => p.order_items)
                .HasForeignKey(d => d.product_id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("order_items_product_id_fkey");
        });

        modelBuilder.Entity<payment>(entity =>
        {
            entity.HasKey(e => e.id).HasName("payments_pkey");

            entity.HasIndex(e => e.order_id, "idx_payments_order_id");

            entity.Property(e => e.id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.status).HasDefaultValueSql("'created'::payment_status");

            entity.HasOne(d => d.order).WithMany(p => p.payments)
                .HasForeignKey(d => d.order_id)
                .HasConstraintName("payments_order_id_fkey");
        });

        modelBuilder.Entity<product>(entity =>
        {
            entity.HasKey(e => e.id).HasName("products_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.price).HasPrecision(18, 8);
            entity.Property(e => e.title).HasMaxLength(50);
        });

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pkey");

            entity.HasIndex(e => e.email, "users_email_key").IsUnique();

            entity.Property(e => e.id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.refresh_token_time).HasColumnType("timestamp without time zone");
            entity.Property(e => e.role).HasDefaultValueSql("'user'::text");

            entity.HasMany(d => d.products).WithMany(p => p.users)
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
