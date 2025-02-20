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

    public virtual DbSet<EcpayOrder> EcpayOrders { get; set; }

    public virtual DbSet<TCart> TCarts { get; set; }

    public virtual DbSet<TComment> TComments { get; set; }

    public virtual DbSet<TFavorite> TFavorites { get; set; }

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

    public virtual DbSet<TPerson> TPeople { get; set; }

    public virtual DbSet<TProduct> TProducts { get; set; }

    public virtual DbSet<TProvider> TProviders { get; set; }

    public virtual DbSet<TPurchase> TPurchases { get; set; }

    public virtual DbSet<TPurchaseDetail> TPurchaseDetails { get; set; }

    public virtual DbSet<TReport> TReports { get; set; }

    public virtual DbSet<TVerification> TVerifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=dbVegetable;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EcpayOrder>(entity =>
        {
            entity.HasKey(e => e.MerchantTradeNo);

            entity.Property(e => e.MerchantTradeNo).HasMaxLength(50);
            entity.Property(e => e.MemberId)
                .HasMaxLength(50)
                .HasColumnName("MemberID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentType).HasMaxLength(50);
            entity.Property(e => e.PaymentTypeChargeFee).HasMaxLength(50);
            entity.Property(e => e.RtnMsg).HasMaxLength(50);
            entity.Property(e => e.TradeDate).HasMaxLength(50);
            entity.Property(e => e.TradeNo).HasMaxLength(50);
        });

        modelBuilder.Entity<TCart>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tCart__D9F8227C4E94F4C5");

            entity.ToTable("tCart");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount)
                .HasDefaultValue(1)
                .HasColumnName("fCount");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
        });

        modelBuilder.Entity<TComment>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tComment__D9F8227CD48FF542");

            entity.ToTable("tComment");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FComment).HasColumnName("fComment");
            entity.Property(e => e.FCreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("fCreatedAt");
            entity.Property(e => e.FOrderListId).HasColumnName("fOrderListId");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FStar).HasColumnName("fStar");
        });

        modelBuilder.Entity<TFavorite>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tFavorit__D9F8227C8B1A03F0");

            entity.ToTable("tFavorite");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
        });

        modelBuilder.Entity<TGoodsInAndOut>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tGoodsIn__D9F8227C77188623");

            entity.ToTable("tGoodsInAndOut");

            entity.Property(e => e.FId).HasColumnName("fId");
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
            entity.Property(e => e.FProviderId).HasColumnName("fProviderId");
            entity.Property(e => e.FTotal).HasColumnName("fTotal");
        });

        modelBuilder.Entity<TGoodsInAndOutDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tGoodsIn__D9F8227CA03D3E10");

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
            entity.HasKey(e => e.FId).HasName("PK__tImg__D9F8227C3D55F3BE");

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
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227C9E6A041E");

            entity.ToTable("tInventoryAdjustment");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCheckerId).HasColumnName("fCheckerId");
            entity.Property(e => e.FCreatedAt).HasColumnName("fCreatedAt");
            entity.Property(e => e.FEditor).HasColumnName("fEditor");
            entity.Property(e => e.FNote)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fNote");
            entity.Property(e => e.FadjustmentDate).HasColumnName("fadjustmentDate");
        });

        modelBuilder.Entity<TInventoryAdjustmentDetail>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227CA08E5106");

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
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227C57ED5BAB");

            entity.ToTable("tInventoryDetail");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FActualQuantity).HasColumnName("fActualQuantity");
            entity.Property(e => e.FInventoryMainId).HasColumnName("fInventoryMainId");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FSystemQuantity).HasColumnName("fSystemQuantity");
        });

        modelBuilder.Entity<TInventoryMain>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tInvento__D9F8227CACC28686");

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
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227C3B06C490");

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
            entity.HasKey(e => e.FId).HasName("PK__tInvoice__D9F8227C9E9EC5B6");

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
            entity.HasKey(e => e.FId).HasName("PK__tOrder__D9F8227C9DC06EC6");

            entity.ToTable("tOrder");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAddress)
                .HasDefaultValue("")
                .HasColumnName("fAddress");
            entity.Property(e => e.FMerchantTradeNo)
                .HasMaxLength(50)
                .HasDefaultValue("")
                .HasColumnName("fMerchantTradeNo");
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
            entity.HasKey(e => e.FId).HasName("PK__tOrderLi__D9F8227C51A1F5D4");

            entity.ToTable("tOrderList");

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FCount).HasColumnName("fCount");
            entity.Property(e => e.FOrderId).HasColumnName("fOrderId");
            entity.Property(e => e.FPrice).HasColumnName("fPrice");
            entity.Property(e => e.FProductId).HasColumnName("fProductId");
            entity.Property(e => e.FSum).HasColumnName("fSum");
        });

        modelBuilder.Entity<TPerson>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tPerson__D9F8227CEE8EF0D9");

            entity.ToTable("tPerson");

            entity.HasIndex(e => e.FAccount, "UQ__tPerson__E129946360FAE680").IsUnique();

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FAccount)
                .HasMaxLength(500)
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
            entity.Property(e => e.FGoogleId)
                .HasMaxLength(500)
                .HasColumnName("fGoogleId");
            entity.Property(e => e.FIsVerified).HasColumnName("fIsVerified");
            entity.Property(e => e.FLoginType).HasColumnName("fLoginType");
            entity.Property(e => e.FName)
                .HasMaxLength(500)
                .HasDefaultValue("")
                .HasColumnName("fName");
            entity.Property(e => e.FPassword)
                .HasMaxLength(500)
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
            entity.HasKey(e => e.FId).HasName("PK__tProduct__D9F8227C8AEFF62B");

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
            entity.Property(e => e.FIntroduction)
                .HasDefaultValue("")
                .HasColumnName("fIntroduction");
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
            entity.HasKey(e => e.FId).HasName("PK__tProvide__D9F8227CD0D8BB44");

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
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227CA3A818B3");

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
            entity.HasKey(e => e.FId).HasName("PK__tPurchas__D9F8227C1E55AA69");

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

        modelBuilder.Entity<TReport>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tReport__D9F8227C9865C3CE");

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

        modelBuilder.Entity<TVerification>(entity =>
        {
            entity.HasKey(e => e.FId).HasName("PK__tVerific__D9F8227C49721A5F");

            entity.ToTable("tVerification");

            entity.HasIndex(e => e.FToken, "UQ__tVerific__B1047326B5603CE1").IsUnique();

            entity.Property(e => e.FId).HasColumnName("fId");
            entity.Property(e => e.FExpirationTime)
                .HasColumnType("datetime")
                .HasColumnName("fExpirationTime");
            entity.Property(e => e.FIsUsed).HasColumnName("fIsUsed");
            entity.Property(e => e.FPersonId).HasColumnName("fPersonId");
            entity.Property(e => e.FToken)
                .HasMaxLength(255)
                .HasColumnName("fToken");
            entity.Property(e => e.FTokenSentTime)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fTokenSentTime");
            entity.Property(e => e.FTokenType)
                .HasMaxLength(20)
                .HasColumnName("fTokenType");
            entity.Property(e => e.FUsedTime)
                .HasColumnType("datetime")
                .HasColumnName("fUsedTime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
