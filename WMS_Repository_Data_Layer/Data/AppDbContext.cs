using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WMS_Repository_Data_Layer.Data.Entities.Models;

namespace WMS_Repository_Data_Layer.Data;

public partial class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Issuing> Issuings { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Receiving> Receivings { get; set; }

    public virtual DbSet<StockMovementReport> StockMovementReports { get; set; }

    public virtual DbSet<StockOverviewReport> StockOverviewReports { get; set; }


    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A2B96D4C008");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(50);
        });

        modelBuilder.Entity<Issuing>(entity =>
        {
            entity.HasKey(e => e.IssueId).HasName("PK__Issuing__6C861624BD157912");

            entity.ToTable("Issuing");

            entity.Property(e => e.IssueId).HasColumnName("IssueID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Issuings)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Issuing__Product__412EB0B6");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Logs__5E5499A8B9080010");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.TimeStamp).HasPrecision(0);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Logs__UserID__440B1D61");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6EDCCBC3A6C");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.UnitPrice).HasColumnType("smallmoney");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__3B75D760");
        });

        modelBuilder.Entity<Receiving>(entity =>
        {
            entity.HasKey(e => e.ReceiveId).HasName("PK__Receivin__3034B3A91C224A38");

            entity.ToTable("Receiving");

            entity.Property(e => e.ReceiveId).HasColumnName("ReceiveID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Product).WithMany(p => p.Receivings)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Receiving__Produ__3E52440B");
        });

        modelBuilder.Entity<StockMovementReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("StockMovementReport");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.TotalValue).HasColumnType("smallmoney");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(9)
                .IsUnicode(false);
            entity.Property(e => e.UnitPrice).HasColumnType("smallmoney");
        });

        modelBuilder.Entity<StockOverviewReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("StockOverviewReport");

            entity.Property(e => e.CategoryName).HasMaxLength(50);
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.TotalPrice).HasColumnType("smallmoney");
            entity.Property(e => e.UnitPrice).HasColumnType("smallmoney");
        });


       
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
