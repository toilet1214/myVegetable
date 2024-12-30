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

    public virtual DbSet<TAboutU> TAboutUs { get; set; }

    public virtual DbSet<TCart> TCarts { get; set; }

    public virtual DbSet<TFaq> TFaqs { get; set; }

    public virtual DbSet<TImg> TImgs { get; set; }

    public virtual DbSet<TInvoice> TInvoices { get; set; }

    public virtual DbSet<TInvoiceDetail> TInvoiceDetails { get; set; }

    public virtual DbSet<TOrder> TOrders { get; set; }

    public virtual DbSet<TOrderList> TOrderLists { get; set; }

    public virtual DbSet<TPerson> TPeople { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    public virtual DbSet<TProvider> TProviders { get; set; }

    public virtual DbSet<TPurchase> TPurchases { get; set; }

    public virtual DbSet<TPurchaseDetail> TPurchaseDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbVegetable;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAboutU>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tAboutUs__D9F8227CE8C661DD");

            entity.ToTable("tAboutUs");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FContent).HasColumnName("fContent");
            entity.Property(e => e.FCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fCreatedAt");
            entity.Property(e => e.FTitle)
                .HasMaxLength(200)
                .HasColumnName("fTitle");
            entity.Property(e => e.FUpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fUpdatedAt");
        });

        modelBuilder.Entity<TCart>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tCart__D9F8227C6A79293B");

            entity.ToTable("tCart");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FBuyerId).HasColumnName("fBuyerId");
            entity.Property(e => e.FCount).HasColumnName("fCount");
            entity.Property(e => e.FImgId).HasColumnName("fImgId");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FProductName)
                .HasMaxLength(500)
                .HasColumnName("fProductName");
        });

        modelBuilder.Entity<TFaq>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tFAQ__D9F8227C029A6DEB");

            entity.ToTable("tFAQ");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAnswer).HasColumnName("fAnswer");
            entity.Property(e => e.FCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fCreatedAt");
            entity.Property(e => e.FQuestion)
                .HasMaxLength(500)
                .HasColumnName("fQuestion");
            entity.Property(e => e.FUpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fUpdatedAt");
        });

        modelBuilder.Entity<TImg>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tImg__D9F8227CCBB27846");

            entity.ToTable("tImg");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FEditer)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEditer");
            entity.Property(e => e.FName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FOrder).HasColumnName("fOrder");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FUploadAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fUploadAt");
        });

        modelBuilder.Entity<TInvoice>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227C61CA268F");

            entity.ToTable("tInvoice");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCustomerId)
                .HasMaxLength(50)
                .HasColumnName("fCustomerId");
            entity.Property(e => e.FCustomerNumber)
                .HasMaxLength(50)
                .HasColumnName("fCustomerNumber");
            entity.Property(e => e.FDate).HasColumnName("fDate");
            entity.Property(e => e.FForm)
                .HasMaxLength(50)
                .HasColumnName("fForm");
            entity.Property(e => e.FInOut)
                .HasMaxLength(50)
                .HasColumnName("fInOut");
            entity.Property(e => e.FInvoiceNumber)
                .HasMaxLength(50)
                .HasColumnName("fInvoiceNumber");
            entity.Property(e => e.FStatus)
                .HasMaxLength(50)
                .HasColumnName("fStatus");
            entity.Property(e => e.FSupplierId)
                .HasMaxLength(50)
                .HasColumnName("fSupplierId");
            entity.Property(e => e.FSupplierNumber)
                .HasMaxLength(50)
                .HasColumnName("fSupplierNumber");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227CC42E42FE");

            entity.ToTable("tInvoiceDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FConut).HasColumnName("fConut");
            entity.Property(e => e.FNumber)
                .HasMaxLength(50)
                .HasColumnName("fNumber");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductName)
                .HasMaxLength(50)
                .HasColumnName("fProductName");
            entity.Property(e => e.FSum).HasColumnName("fSum");
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tOrder__D9F8227C5D379971");

            entity.ToTable("tOrder");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAddress)
                .HasMaxLength(255)
                .HasColumnName("fAddress");
            entity.Property(e => e.FBuyerId).HasColumnName("fBuyerId");
            entity.Property(e => e.FOrderAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fOrderAt");
            entity.Property(e => e.FPhone)
                .HasMaxLength(15)
                .HasColumnName("fPhone");
            entity.Property(e => e.FReceiverName)
                .HasMaxLength(100)
                .HasColumnName("fReceiverName");
            entity.Property(e => e.FRemark)
                .HasDefaultValue("")
                .HasColumnName("fRemark");
            entity.Property(e => e.FStatus)
                .HasMaxLength(50)
                .HasColumnName("fStatus");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TOrderList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tOrderLi__D9F8227CAD53E719");

            entity.ToTable("tOrderList");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount).HasColumnName("fCount");
            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FProductName)
                .HasMaxLength(255)
                .HasColumnName("fProductName");
            entity.Property(e => e.FSum).HasColumnName("fSum");
        });

        modelBuilder.Entity<TPerson>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPerson__D9F8227C2C197292");

            entity.ToTable("tPerson");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAccount)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fAccount");
            entity.Property(e => e.FAddress)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fAddress");
            entity.Property(e => e.FBirth).HasColumnName("fBirth");
            entity.Property(e => e.FCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fCreatedAt");
            entity.Property(e => e.FEditor)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEditor");
            entity.Property(e => e.FEmail)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEmail");
            entity.Property(e => e.FEmp)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEmp");
            entity.Property(e => e.FGender)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fGender");
            entity.Property(e => e.FName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FPassword)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fPassword");
            entity.Property(e => e.FPermissiion)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fPermissiion");
            entity.Property(e => e.FPhone)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fPhone");
            entity.Property(e => e.FTel)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fTel");
            entity.Property(e => e.FTelEmptel)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fTelEmptel");
            entity.Property(e => e.FUbn)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fUBN");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tProduct__D9F8227CA7601E61");

            entity.ToTable("tProduct");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FClassification)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fClassification");
            entity.Property(e => e.FDescription)
                .HasDefaultValue("")
                .HasColumnType("text")
                .HasColumnName("fDescription");
            entity.Property(e => e.FEditer)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEditer");
            entity.Property(e => e.FLaunchAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fLaunchAt");
            entity.Property(e => e.FName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FOrigin)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fOrigin");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FQuantity).HasColumnName("fQuantity");
            entity.Property(e => e.FStorage)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fStorage");
        });

        modelBuilder.Entity<TProvider>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tProvide__D9F8227C6755C321");

            entity.ToTable("tProvider");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAccountant)
                .HasMaxLength(50)
                .HasColumnName("fAccountant");
            entity.Property(e => e.FAddress)
                .HasMaxLength(100)
                .HasColumnName("fAddress");
            entity.Property(e => e.FConnect)
                .HasMaxLength(50)
                .HasColumnName("fConnect");
            entity.Property(e => e.FDelivery)
                .HasMaxLength(100)
                .HasColumnName("fDelivery");
            entity.Property(e => e.FEditer).HasColumnName("fEditer");
            entity.Property(e => e.FInvoiceadd)
                .HasMaxLength(100)
                .HasColumnName("fInvoiceadd");
            entity.Property(e => e.FName)
                .HasMaxLength(100)
                .HasColumnName("fName");
            entity.Property(e => e.FTel)
                .HasMaxLength(30)
                .HasColumnName("fTel");
            entity.Property(e => e.FUbn)
                .HasMaxLength(10)
                .HasColumnName("fUBN");
        });

        modelBuilder.Entity<TPurchase>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227CA1FAC4BC");

            entity.ToTable("tPurchase");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FBuyDate)
                .HasColumnType("datetime")
                .HasColumnName("fBuyDate");
            entity.Property(e => e.FBuyer)
                .HasMaxLength(50)
                .HasColumnName("fBuyer");
            entity.Property(e => e.FCreater)
                .HasMaxLength(20)
                .HasColumnName("fCreater");
            entity.Property(e => e.FEditor)
                .HasMaxLength(20)
                .HasColumnName("fEditor");
            entity.Property(e => e.FExpirationDate)
                .HasColumnType("datetime")
                .HasColumnName("fExpirationDate");
            entity.Property(e => e.FInvoiceForm)
                .HasMaxLength(20)
                .HasColumnName("fInvoiceForm");
            entity.Property(e => e.FIsTax).HasColumnName("fIsTax");
            entity.Property(e => e.FNote)
                .HasMaxLength(100)
                .HasColumnName("fNote");
            entity.Property(e => e.FPayment)
                .HasMaxLength(20)
                .HasColumnName("fPayment");
            entity.Property(e => e.FPreTax).HasColumnName("fPreTax");
            entity.Property(e => e.FSupplierId)
                .HasMaxLength(50)
                .HasColumnName("fSupplierId");
            entity.Property(e => e.FSupplierName)
                .HasMaxLength(50)
                .HasColumnName("fSupplierName");
            entity.Property(e => e.FTax).HasColumnName("fTax");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TPurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227C008EA01C");

            entity.ToTable("tPurchaseDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FConut).HasColumnName("fConut");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductName)
                .HasMaxLength(50)
                .HasColumnName("fProductName");
            entity.Property(e => e.FPurchaseId).HasColumnName("fPurchaseId");
            entity.Property(e => e.FSum).HasColumnName("fSum");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
