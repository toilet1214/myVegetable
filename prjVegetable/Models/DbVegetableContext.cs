using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prjVegetable.Models;

public partial class DbVegetableContext : DbContext
{
    public DbVegetableContext()
    {
    }

    public DbVegetableContext(DbContextOptions<DbVegetableContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TImg> TImgs { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbVegetable;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TImg>(entity =>
        {
            entity.HasKey(e => e.FImgId).HasName("PK__tImg__BCED033D43C84011");

            entity.ToTable("tImg");

            entity.Property(e => e.FImgId).HasColumnName("fImg_Id");
            entity.Property(e => e.FImgCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fImg_Created_at");
            entity.Property(e => e.FImgEditer)
                .HasMaxLength(255)
                .HasDefaultValue("")
                .HasColumnName("fImg_Editer");
            entity.Property(e => e.FImgName)
                .HasMaxLength(255)
                .HasDefaultValue("")
                .HasColumnName("fImg_Name");
            entity.Property(e => e.FImgOrder).HasColumnName("fImg_Order");
            entity.Property(e => e.FProductId).HasColumnName("fProduct_Id");

            entity.HasOne(d => d.FProduct).WithMany(p => p.TImgs)
                .HasForeignKey(d => d.FProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TImg_Product");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.FProductId).HasName("PK__tProduct__D6C82E19DD7C3F34");

            entity.ToTable("tProduct");

            entity.Property(e => e.FProductId).HasColumnName("fProduct_Id");
            entity.Property(e => e.FProductClassification)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fProduct_Classification");
            entity.Property(e => e.FProductCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fProduct_Created_at");
            entity.Property(e => e.FProductDescription)
                .HasDefaultValue("")
                .HasColumnType("text")
                .HasColumnName("fProduct_Description");
            entity.Property(e => e.FProductEditer)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fProduct_Editer");
            entity.Property(e => e.FProductName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fProduct_Name");
            entity.Property(e => e.FProductOrigin)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fProduct_Origin");
            entity.Property(e => e.FProductPrice).HasColumnName("fProduct_Price");
            entity.Property(e => e.FProductQuantity).HasColumnName("fProduct_Quantity");
            entity.Property(e => e.FProductStorage)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fProduct_Storage");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
