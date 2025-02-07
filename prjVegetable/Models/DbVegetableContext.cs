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

    public virtual DbSet<TInventoryAdjustment> TInventoryAdjustments { get; set; }

    public virtual DbSet<TInventoryAdjustmentDetail> TInventoryAdjustmentDetails { get; set; }

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

    public virtual DbSet<TReport> TReports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbVegetable;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TAboutU>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tAboutUs__D9F8227CF4D0513B");

            entity.ToTable("tAboutUs");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FContent)
                .HasDefaultValue("")
                .HasColumnName("fContent");
            entity.Property(e => e.FCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fCreatedAt");
            entity.Property(e => e.FTitle)
                .HasMaxLength(200)
                .HasDefaultValue("")
                .HasColumnName("fTitle");
            entity.Property(e => e.FUpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fUpdatedAt");
        });

        modelBuilder.Entity<TCart>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tCart__D9F8227C07B59E35");

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
            entity.HasKey(e => e.FId).HasName("PK__tFAQ__D9F8227C261847DC");

            entity.ToTable("tFAQ");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAnswer)
                .HasDefaultValue("")
                .HasColumnName("fAnswer");
            entity.Property(e => e.FCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fCreatedAt");
            entity.Property(e => e.FQuestion)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fQuestion");
            entity.Property(e => e.FUpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fUpdatedAt");
        });

        modelBuilder.Entity<TGoodsInAndOut>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tGoodsIn__D9F8227C8D861661");

            entity.ToTable("tGoodsInAndOut");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount).HasColumnName("fCount");
            entity.Property(e => e.FDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fDate");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FInOut).HasColumnName("fInOut");
            entity.Property(e => e.FInvoiceId).HasColumnName("fInvoiceId");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TGoodsInAndOutDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tGoodsIn__D9F8227C00143D13");

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
            entity.HasKey(e => e.FId).HasName("PK__tImg__D9F8227CDEE5A8C3");

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
                .HasColumnName("fUploadAt");
        });

        modelBuilder.Entity<TInventoryAdjustment>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227C80C6BEA9");

            entity.ToTable("tInventoryAdjustment");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCheckerId).HasColumnName("fCheckerId");
            entity.Property(e => e.FCreatedAt).HasColumnName("fCreatedAt");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasColumnName("fNote");
            entity.Property(e => e.FadjustmentDate).HasColumnName("fadjustmentDate");
        });

        modelBuilder.Entity<TInventoryAdjustmentDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227CFB9AC3ED");

            entity.ToTable("tInventoryAdjustmentDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCost)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("fCost");
            entity.Property(e => e.FInventoryAdjustmentId).HasColumnName("fInventoryAdjustmentId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FQuantity).HasColumnName("fQuantity");
        });

        modelBuilder.Entity<TInventoryDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227CA75814B5");

            entity.ToTable("tInventoryDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FActualQuantity).HasColumnName("fActualQuantity");
            entity.Property(e => e.FInventoryMainId).HasColumnName("fInventoryMainId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FSystemQuantity).HasColumnName("fSystemQuantity");
        });

        modelBuilder.Entity<TInventoryMain>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227CD65A3CB4");

            entity.ToTable("tInventoryMain");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FBaselineDate).HasColumnName("fBaselineDate");
            entity.Property(e => e.FCreatedAt).HasColumnName("fCreatedAt");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasColumnName("fNote");
        });

        modelBuilder.Entity<TInvoice>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227C7A3B0DCF");

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
            entity.Property(e => e.FEditor)
                .HasDefaultValue(0)
                .HasColumnName("fEditor");
            entity.Property(e => e.FForm)
                .HasMaxLength(20)
                .HasDefaultValue("")
                .HasColumnName("fForm");
            entity.Property(e => e.FInOut)
                .HasDefaultValue(0)
                .HasColumnName("fInOut");
            entity.Property(e => e.FNumber)
                .HasMaxLength(50)
                .HasColumnName("fNumber");
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FProviderUbn)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fProviderUbn");
            entity.Property(e => e.FStatus)
                .HasDefaultValue(0)
                .HasColumnName("fStatus");
            entity.Property(e => e.FTotal)
                .HasDefaultValue(0)
                .HasColumnName("fTotal");
        });

        modelBuilder.Entity<TInvoiceDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227C0C877FE0");

            entity.ToTable("tInvoiceDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount)
                .HasDefaultValue(1)
                .HasColumnName("fCount");
            entity.Property(e => e.FNumber)
                .HasMaxLength(50)
                .HasColumnName("fNumber");
            entity.Property(e => e.FPrice)
                .HasDefaultValue(0)
                .HasColumnName("fPrice");
            entity.Property(e => e.FProductName)
                .HasMaxLength(50)
                .HasColumnName("fProductName");
            entity.Property(e => e.FSum)
                .HasDefaultValue(0)
                .HasColumnName("fSum");
        });

        modelBuilder.Entity<TOrder>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tOrder__D9F8227C50DD68BE");

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
            entity.HasKey(e => e.FId).HasName("PK__tOrderLi__D9F8227C04451ECD");

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
            entity.HasKey(e => e.FId).HasName("PK__tPayment__D9F8227C731360BB");

            entity.ToTable("tPaymentReversal");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fDate");
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
            entity.HasKey(e => e.FId).HasName("PK__tPayment__D9F8227CC6D03E3B");

            entity.ToTable("tPaymentReversalDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FGoodsInAndOutId).HasColumnName("fGoodsInAndOutId");
            entity.Property(e => e.FPaymentReversalId).HasColumnName("fPaymentReversalId");
        });

        modelBuilder.Entity<TPerson>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPerson__D9F8227C3CDDE986");

            entity.ToTable("tPerson");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAccount)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fAccount");
            entity.Property(e => e.FAddress)
                .HasDefaultValue("")
                .HasColumnName("fAddress");
            entity.Property(e => e.FBirth)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fBirth");
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
            entity.HasKey(e => e.FId).HasName("PK__tProduct__D9F8227C3A03EA2D");

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
                .HasColumnName("fLaunchAt");
            entity.Property(e => e.FName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FOrigin)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fOrigin");
            entity.Property(e => e.FPrice)
                .HasDefaultValue(1)
                .HasColumnName("fPrice");
            entity.Property(e => e.FQuantity).HasColumnName("fQuantity");
            entity.Property(e => e.FStorage).HasColumnName("fStorage");
        });

        modelBuilder.Entity<TProvider>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tProvide__D9F8227CE411A2BB");

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
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227CF7F8B35A");

            entity.ToTable("tPurchase");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FBuyDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fBuyDate");
            entity.Property(e => e.FEditor)
                .HasDefaultValue(0)
                .HasColumnName("fEditor");
            entity.Property(e => e.FInvoiceForm)
                .HasDefaultValue(0)
                .HasColumnName("fInvoiceForm");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FPayment)
                .HasDefaultValue(0)
                .HasColumnName("fPayment");
            entity.Property(e => e.FPreTax)
                .HasDefaultValue(0)
                .HasColumnName("fPreTax");
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FTax)
                .HasDefaultValue(0)
                .HasColumnName("fTax");
            entity.Property(e => e.FTotal)
                .HasDefaultValue(0)
                .HasColumnName("fTotal");
        });

        modelBuilder.Entity<TPurchaseDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227C420766F7");

            entity.ToTable("tPurchaseDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount)
                .HasDefaultValue(1)
                .HasColumnName("fCount");
            entity.Property(e => e.FPrice)
                .HasDefaultValue(0)
                .HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FPurchaseId).HasColumnName("fPurchaseId");
            entity.Property(e => e.FSum)
                .HasDefaultValue(0)
                .HasColumnName("fSum");
        });

        modelBuilder.Entity<TReceiptReversal>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tReceipt__D9F8227C4ECFAE7F");

            entity.ToTable("tReceiptReversal");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fDate");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FReceiptMethod)
                .HasDefaultValue(1)
                .HasColumnName("fReceiptMethod");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TReceiptReversalDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tReceipt__D9F8227CED41AB57");

            entity.ToTable("tReceiptReversalDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FReceiptReversalId).HasColumnName("fReceiptReversalId");
        });

        modelBuilder.Entity<TReport>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tReport__D9F8227CCDC55250");

            entity.ToTable("tReport");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FClass)
                .HasMaxLength(10)
                .HasDefaultValue("其他")
                .HasColumnName("fClass");
            entity.Property(e => e.FDescription)
                .HasDefaultValue("")
                .HasColumnName("fDescription");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fTime");
            entity.Property(e => e.FTitle)
                .HasMaxLength(100)
                .HasDefaultValue("")
                .HasColumnName("fTitle");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
