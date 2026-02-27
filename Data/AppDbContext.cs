using System;
using System.Collections.Generic;
using Crypto_Store.Models;
using Microsoft.EntityFrameworkCore;

namespace Crypto_Store.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<user> users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<user>(entity =>
        {
            entity.HasKey(e => e.id).HasName("users_pkey");

            entity.Property(e => e.id).HasDefaultValueSql("uuid_generate_v4()");
            entity.Property(e => e.created)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone");
            entity.Property(e => e.regresh_token_expiry).HasColumnType("timestamp without time zone");
            entity.Property(e => e.role).HasDefaultValueSql("'user'::text");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
