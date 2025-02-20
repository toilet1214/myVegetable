--drop database [dbVegetable]
CREATE DATABASE [dbVegetable]
GO
USE dbVegetable
CREATE TABLE tInvoiceDetail (
	fId  int NOT NULL PRIMARY KEY identity(1,1),
	fNumber Nvarchar(50) NOT NULL,
	fProductName Nvarchar(50) NOT NULL,
	fConut int NOT NULL,
	fPrice int NOT NULL,
	fSum int NOT NULL 
);
GO
CREATE TABLE tInvoice (
	fId int NOT NULL PRIMARY KEY identity(1,1),
    	fNumber nvarchar(50) NOT NULL,
	fDate datetime NOT NULL,
	fForm nvarchar(20) NOT NULL,
	fCustomerId int NOT NULL,
	fCustomerUBN nvarchar(50) NOT NULL,
	fSupplierId int NOT NULL,
	fSupplierUBN nvarchar(50) NOT NULL,
	fInOut int NOT NULL,
	fStatus int NOT NULL,
	fTotal int NOT NULL,
	fEditer nvarchar(50)
);
GO
CREATE TABLE tPurchase (
	fId int NOT NULL PRIMARY KEY identity(1,1),
    	fBuyDate datetime NOT NULL,
	fSupplierId nvarchar(50) NOT NULL,
	fSupplierName nvarchar(50) NOT NULL,
	fBuyer nvarchar(50) NOT NULL,
	fExpirationDate datetime NOT NULL,
	fIsTax bit NOT NULL,
	fInvoiceForm nvarchar(20) NOT NULL,
	fPayment nvarchar(20) NOT NULL,
	fCreater nvarchar(20) NOT NULL,
	fEditor nvarchar(20) NOT NULL,
	fPreTax int NOT NULL,
	fTax int NOT NULL,
	fTotal int NOT NULL,
    	fNote  nvarchar(100) NOT NULL
);
GO
CREATE TABLE tPurchaseDetail (
	fId int NOT NULL PRIMARY KEY identity(1,1),
   	fPurchaseId nvarchar(50) NOT NULL,
	fProductName nvarchar(50) NOT NULL,
	fConut int NOT NULL,
	fPrice int NOT NULL,
	fSum  int NOT NULL
);
GO
create table tProvider(
fId int not null identity(1,1) primary key,
fName nvarchar(100) NOT NULL,
fUBN nvarchar(10) NOT NULL,
fTel nvarchar(30) NOT NULL,
fConnect nvarchar(50) NOT NULL,
fAccountant nvarchar(50) NOT NULL,
fAddress nvarchar(100) NOT NULL,
fDelivery nvarchar(100) NOT NULL,
fInvoiceadd nvarchar(100) NOT NULL,
fEditer int NOT NULL
);
GO
CREATE TABLE tProduct (
    fId INT IDENTITY(1,1) PRIMARY KEY ,  -- 產品ID，自動增長
    fName NVARCHAR(500) NOT NULL DEFAULT '', -- 產品名稱，不允許為 NULL，默認為空字串
    fClassification NVARCHAR(500) NOT NULL DEFAULT '', -- 產品分類，不允許為 NULL，默認為空字串
    fPrice INT NOT NULL DEFAULT 0,         -- 產品價格，整數，不允許為 NULL，默認為 0
    fDescription TEXT NOT NULL DEFAULT '',         -- 產品描述，不允許為 NULL，默認為空字串
    fQuantity INT NOT NULL DEFAULT 0,              -- 產品庫存數量，默認為 0
    fLaunchAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,  -- 上架日期，使用 DATETIME 類型，默認為當前時間戳
    fStorage NVARCHAR(500) NOT NULL DEFAULT '',     -- 藏溫方式，默認為空字串
    fOrigin NVARCHAR(500) NOT NULL DEFAULT '',      -- 產品產地，默認為空字串
    fEditer NVARCHAR(500) NOT NULL DEFAULT '',      -- 修改人，默認為空字串
);
GO
CREATE TABLE tImg
(
    fId INT IDENTITY(1,1) PRIMARY KEY,        -- 照片ID，設定為自動遞增
    fProductId INT NOT NULL,                      -- 商品ID，必須填寫
    fName NVARCHAR(500) NOT NULL DEFAULT '',    -- 照片名稱，預設為空字串
    fOrder INT NOT NULL,                       -- 照片排序，必須填寫
    fUploadAt DATETIME DEFAULT GETDATE(),   -- 上傳日期，預設為當前時間
    fEditer NVARCHAR(500) NOT NULL DEFAULT '',  -- 修改人，預設為空字串
   
);
GO
CREATE TABLE tOrder (
    fId INT IDENTITY(1,1) PRIMARY KEY,  -- 訂單 ID (自動遞增，從 1 開始，遞增步長為 1)
    fBuyerId INT NOT NULL,  -- 購買人 (不允許為空)
    fTotal INT NOT NULL,  -- 總金額 (不允許為空)
    fStatus NVARCHAR(50) NOT NULL,  -- 訂單狀態 (不允許為空)
    fOrderAt DATETIME NOT NULL DEFAULT GETDATE(),  -- 訂單創建時間 (不允許為空，默認為當前時間)
    fAddress NVARCHAR(255) NOT NULL,  -- 收件人地址 (不允許為空)
    fReceiverName NVARCHAR(100) NOT NULL,  -- 收件人姓名 (不允許為空)
    fPhone NVARCHAR(15) NOT NULL,  -- 收件人電話 (不允許為空)
    fRemark NVARCHAR(MAX) NOT NULL DEFAULT '',  -- 備註 (不允許為空，默認為空字串)
   
);
GO
CREATE TABLE tPerson (
    fId INT IDENTITY(1,1) PRIMARY KEY,            -- 自增主鍵
    fName NVARCHAR(500) NOT NULL DEFAULT '',       -- 會員姓名，預設為空字串
    fAccount NVARCHAR(500) NOT NULL DEFAULT '',    -- 會員帳號，預設為空字串
    fPassword NVARCHAR(500) NOT NULL DEFAULT '',   -- 會員密碼，預設為空字串
    fGender NVARCHAR(100) NOT NULL DEFAULT '',      -- 會員姓別，預設為空字串
    fBirth DATE NOT NULL,                          -- 會員生日
    fPhone NVARCHAR(20)  NOT NULL DEFAULT '',                -- 會員手機，預設為空字串
    fTel NVARCHAR(20)  NOT NULL DEFAULT '',                  -- 會員家電，預設為空字串
    fAddress NVARCHAR(500)  NOT NULL DEFAULT '',             -- 會員地址，預設為空字串
    fEmail NVARCHAR(500)  NOT NULL DEFAULT '',               -- 會員Email，預設為空字串
    fUBN NVARCHAR(50)  NOT NULL DEFAULT '',             -- 會員統編，預設為空字串
    fPermissiion NVARCHAR(500)  NOT NULL DEFAULT '',              -- 權限，預設為空字串
    fEmp NVARCHAR(500)  NOT NULL DEFAULT '',                 -- 緊急聯絡人姓名，預設為空字串
    fEmpTel NVARCHAR(20)  NOT NULL DEFAULT '',           -- 緊急聯絡人電話，預設為空字串
    fCreatedAt DATETIME DEFAULT GETDATE(),       -- 創建時間，預設為當前時間
    fEditor NVARCHAR(500)  NOT NULL DEFAULT ''               -- 編輯者，預設為空字串
);
GO
CREATE TABLE tCart (
fId INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
fProductId INT NOT NULL, 
fProductName NVARCHAR(500) NOT NULL, 
fCount INT NOT NULL, 
fPrice INT NOT NULL, 
fImgId INT NOT NULL, 
fBuyerId INT NOT NULL
);
GO
CREATE TABLE tOrderList (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,       -- 訂單明細編號，自動遞增
    fOrderId INT NOT NULL,                          -- 訂單編號 (外鍵參照訂單表 fO_Id)
    fProductId INT NOT NULL,                    -- 商品ID
    fProductName NVARCHAR(255) NOT NULL,        -- 商品名稱
    fPrice INT NOT NULL,                 -- 商品單價 (改為 INT)
    fCount INT NOT NULL,              -- 數量
    fSum INT NOT NULL,                   -- 小計 (改為 INT)
   );
GO
CREATE TABLE tFAQ (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- 自動遞增的主鍵
    fQuestion NVARCHAR(500) NOT NULL,        -- 問題欄位
    fAnswer NVARCHAR(MAX) NOT NULL,          -- 答案欄位
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- 記錄創建時間
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- 記錄更新時間
);
GO
CREATE TABLE tAboutUs (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- 自動遞增的主鍵
    fTitle NVARCHAR(200) NOT NULL,           -- 標題欄位
    fContent NVARCHAR(MAX) NOT NULL,         -- 內容欄位
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- 記錄創建時間
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- 記錄更新時間
);
