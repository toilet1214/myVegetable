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

    public virtual DbSet<TPerson> TPeople { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=CR1-S24;Initial Catalog=dbVegetable;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TPerson>(entity =>
        {
            entity.HasKey(e => e.FPId).HasName("PK__tPerson__05F97AA995935B1A");

            entity.ToTable("tPerson");

            entity.Property(e => e.FPId).HasColumnName("fP_Id");
            entity.Property(e => e.FPAccount)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Account");
            entity.Property(e => e.FPAddress)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Address");
            entity.Property(e => e.FPBirth).HasColumnName("fP_Birth");
            entity.Property(e => e.FPCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fP_Created_at");
            entity.Property(e => e.FPEditor)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Editor");
            entity.Property(e => e.FPEmail)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Email");
            entity.Property(e => e.FPEmp)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Emp");
            entity.Property(e => e.FPGender)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fP_Gender");
            entity.Property(e => e.FPName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Name");
            entity.Property(e => e.FPPassword)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Password");
            entity.Property(e => e.FPPhone)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fP_Phone");
            entity.Property(e => e.FPStatus)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fP_Status");
            entity.Property(e => e.FPTel)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fP_Tel");
            entity.Property(e => e.FPTelEmptel)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fP_TelEmptel");
            entity.Property(e => e.FPUinvoice)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fP_UInvoice");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
