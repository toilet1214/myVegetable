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

    public virtual DbSet<TGoodsInAndOut> TGoodsInAndOuts { get; set; }

    public virtual DbSet<TGoodsInAndOutDetail> TGoodsInAndOutDetails { get; set; }

    public virtual DbSet<TImg> TImgs { get; set; }

    public virtual DbSet<TInventoryDetail> TInventoryDetails { get; set; }

    public virtual DbSet<TInventoryMain> TInventoryMains { get; set; }

    public virtual DbSet<TInvoice> TInvoices { get; set; }

    public virtual DbSet<TInvoiceDetail> TInvoiceDetails { get; set; }

    public virtual DbSet<TOrder> TOrders { get; set; }

    public virtual DbSet<TOrderList> TOrderLists { get; set; }

    public virtual DbSet<TPaymentReversal> TPaymentReversals { get; set; }

    public virtual DbSet<TPaymentReversalDetail> TPaymentReversalDetails { get; set; }

    public virtual DbSet<TPerson> TPeople { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    public virtual DbSet<TProvider> TProviders { get; set; }

    public virtual DbSet<TPurchase> TPurchases { get; set; }

    public virtual DbSet<TPurchaseDetail> TPurchaseDetails { get; set; }

    public virtual DbSet<TReceiptReversal> TReceiptReversals { get; set; }

    public virtual DbSet<TReceiptReversalDetail> TReceiptReversalDetails { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbVegetable;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAboutU>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tAboutUs__D9F8227C7BA8034C");

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
            entity.HasKey(e => e.FId).HasName("PK__tCart__D9F8227C52577084");

            entity.ToTable("tCart");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount)
                .HasDefaultValue(1)
                .HasColumnName("fCount");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
        });

        modelBuilder.Entity<TFaq>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tFAQ__D9F8227CCAA21BFA");

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

        modelBuilder.Entity<TGoodsInAndOut>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tGoodsIn__D9F8227C6E6D1F09");

            entity.ToTable("tGoodsInAndOut");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount).HasColumnName("fCount");
            entity.Property(e => e.FDate).HasColumnName("fDate");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FInOut).HasColumnName("fInOut");
            entity.Property(e => e.FInvoiceId).HasColumnName("fInvoiceId");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasColumnName("fNote");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TGoodsInAndOutDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tGoodsIn__D9F8227C0D75D901");

            entity.ToTable("tGoodsInAndOutDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount).HasColumnName("fCount");
            entity.Property(e => e.FGoodsInandOutId).HasColumnName("fGoodsInandOutId");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FSum).HasColumnName("fSum");
        });

        modelBuilder.Entity<TImg>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tImg__D9F8227C0E108997");

            entity.ToTable("tImg");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FOrderBy)
                .HasDefaultValue(1)
                .HasColumnName("fOrderBy");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FUploadAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fUploadAt");
        });

        modelBuilder.Entity<TInventoryDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227CFDE970EB");

            entity.ToTable("tInventoryDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FActualQuantity).HasColumnName("fActualQuantity");
            entity.Property(e => e.FInventoryDetailId).HasColumnName("fInventoryDetailId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FSystemQuantity).HasColumnName("fSystemQuantity");
        });

        modelBuilder.Entity<TInventoryMain>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227CC62C0886");

            entity.ToTable("tInventoryMain");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FBaselineDate).HasColumnName("fBaselineDate");
            entity.Property(e => e.FCreatedAt).HasColumnName("fCreatedAt");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
        });

        modelBuilder.Entity<TInvoice>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227C2B69F24F");

            entity.ToTable("tInvoice");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCustomerId).HasColumnName("fCustomerId");
            entity.Property(e => e.FCustomerUbn)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fCustomerUbn");
            entity.Property(e => e.FDate)
                .HasColumnType("datetime")
                .HasColumnName("fDate");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FForm)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fForm");
            entity.Property(e => e.FInOut).HasColumnName("fInOut");
            entity.Property(e => e.FNumber)
                .HasMaxLength(50)
                .HasColumnName("fNumber");
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FProviderUbn)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fProviderUbn");
            entity.Property(e => e.FStatus).HasColumnName("fStatus");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227CB4613B10");

            entity.ToTable("tInvoiceDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount)
                .HasDefaultValue(1)
                .HasColumnName("fCount");
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
            entity.HasKey(e => e.FId).HasName("PK__tOrder__D9F8227C27814018");

            entity.ToTable("tOrder");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAddress)
                .HasDefaultValue("")
                .HasColumnName("fAddress");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FOrderAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fOrderAt");
            entity.Property(e => e.FPay).HasColumnName("fPay");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FPhone)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fPhone");
            entity.Property(e => e.FReceiverName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fReceiverName");
            entity.Property(e => e.FStatus).HasColumnName("fStatus");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TOrderList>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tOrderLi__D9F8227C087E9748");

            entity.ToTable("tOrderList");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount).HasColumnName("fCount");
            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FSum).HasColumnName("fSum");
        });

        modelBuilder.Entity<TPaymentReversal>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPayment__D9F8227C2F30F56C");

            entity.ToTable("tPaymentReversal");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FDate).HasColumnName("fDate");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FPaymentMethod).HasColumnName("fPaymentMethod");
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TPaymentReversalDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPayment__D9F8227C3336C4B4");

            entity.ToTable("tPaymentReversalDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FGoodsInAndOutId).HasColumnName("fGoodsInAndOutId");
            entity.Property(e => e.FPaymentReversalId).HasColumnName("fPaymentReversalId");
        });

        modelBuilder.Entity<TPerson>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPerson__D9F8227C92602A5B");

            entity.ToTable("tPerson");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAccount)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fAccount");
            entity.Property(e => e.FAddress)
                .HasDefaultValue("")
                .HasColumnName("fAddress");
            entity.Property(e => e.FBirth).HasColumnName("fBirth");
            entity.Property(e => e.FCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fCreatedAt");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FEmail)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEmail");
            entity.Property(e => e.FEmp)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEmp");
            entity.Property(e => e.FEmpTel)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fEmpTel");
            entity.Property(e => e.FGender)
                .HasMaxLength(500)
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
            entity.Property(e => e.FPermission).HasColumnName("fPermission");
            entity.Property(e => e.FPhone)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fPhone");
            entity.Property(e => e.FTel)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fTel");
            entity.Property(e => e.FUbn)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fUBN");
        });

        modelBuilder.Entity<TProduct>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tProduct__D9F8227C892771F4");

            entity.ToTable("tProduct");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FClassification)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fClassification");
            entity.Property(e => e.FDescription)
                .HasDefaultValue("")
                .HasColumnName("fDescription");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FLaunch).HasColumnName("fLaunch");
            entity.Property(e => e.FLaunchAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fLaunchAt");
            entity.Property(e => e.FName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FOrigin)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fOrigin");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FQuantity).HasColumnName("fQuantity");
            entity.Property(e => e.FStorage).HasColumnName("fStorage");
        });

        modelBuilder.Entity<TProvider>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tProvide__D9F8227C3CFFF66B");

            entity.ToTable("tProvider");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAccountant)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fAccountant");
            entity.Property(e => e.FAddress)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fAddress");
            entity.Property(e => e.FConnect)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fConnect");
            entity.Property(e => e.FDelivery)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fDelivery");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FInvoiceAdd)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fInvoiceAdd");
            entity.Property(e => e.FName)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FTel)
                .HasMaxLength(30)
                .HasDefaultValue("")
                .HasColumnName("fTel");
            entity.Property(e => e.FUbn)
                .HasMaxLength(10)
                .HasDefaultValue("")
                .HasColumnName("fUbn");
        });

        modelBuilder.Entity<TPurchase>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227C91E7D472");

            entity.ToTable("tPurchase");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FBuyDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fBuyDate");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FInvoiceForm).HasColumnName("fInvoiceForm");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FPayment).HasColumnName("fPayment");
            entity.Property(e => e.FPreTax).HasColumnName("fPreTax");
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FTax).HasColumnName("fTax");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TPurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227C414ABB1F");

            entity.ToTable("tPurchaseDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount)
                .HasDefaultValue(1)
                .HasColumnName("fCount");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FPurchaseId).HasColumnName("fPurchaseId");
            entity.Property(e => e.FSum).HasColumnName("fSum");
        });

        modelBuilder.Entity<TReceiptReversal>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tReceipt__D9F8227C58D4F892");

            entity.ToTable("tReceiptReversal");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FDate).HasColumnName("fDate");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FReceiptMethod).HasColumnName("fReceiptMethod");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TReceiptReversalDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tReceipt__D9F8227C94ABD693");

            entity.ToTable("tReceiptReversalDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FReceiptReversalId).HasColumnName("fReceiptReversalId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
