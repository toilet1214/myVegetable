USE master
--drop table [dbo].[tPurchase]
--drop table[dbo].[tPurchaseDetail]
--drop table[dbo].[tInvoice]
--drop table[dbo].[tInvoiceDetail]

CREATE DATABASE [dbVegetable]
GO
USE dbVegetable

GO
    CREATE TABLE tPerson(
fId INT NOT NULL Primary Key identity (1,1) ,
fName Nvarchar(500) NOT NULL DEFAULT '',
fAccount Nvarchar(500) NOT NULL DEFAULT '' UNIQUE,
fPassword Nvarchar(500) NOT NULL DEFAULT '',
fGender Nvarchar(500) NOT NULL DEFAULT '',
fBirth Date NOT NULL DEFAULT GETDATE(),
fPhone Nvarchar(500) NOT NULL DEFAULT '',
fTel Nvarchar(500) NOT NULL DEFAULT '',
fAddress Nvarchar(MAX) NOT NULL DEFAULT '',
fEmail Nvarchar(500) NOT NULL DEFAULT '',
fUBN Nvarchar(500) NOT NULL DEFAULT '',
fPermission int NOT NULL DEFAULT 0,
fEmp Nvarchar(500) NOT NULL DEFAULT '',
fEmpTel Nvarchar(500) NOT NULL DEFAULT '',
fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
fEditor INT NOT NULL
    );
GO
CREATE TABLE tProvider ( 
fId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, -- 廠商編號，自動遞增 
fName NVARCHAR(100) NOT NULL DEFAULT'', -- 廠商名稱 
fUbn NVARCHAR(10) NOT NULL DEFAULT'', -- 廠商統編 
fTel NVARCHAR(30) NOT NULL DEFAULT'', -- 廠商電話，預設為空字串 
fConnect NVARCHAR(50) NOT NULL DEFAULT'', -- 廠商聯絡人，預設為空字串 
fAccountant NVARCHAR(50) NOT NULL DEFAULT'', -- 廠商會計，預設為空字串 
fAddress NVARCHAR(100) NOT NULL DEFAULT'', -- 廠商公司地址，預設為空字串 
fDelivery NVARCHAR(100) NOT NULL DEFAULT'', -- 廠商送貨地址，預設為空字串 
fInvoiceAdd NVARCHAR(100) NOT NULL DEFAULT'', -- 廠商發票地址，預設為空字串 
fEditor INT NOT NULL      -- 修改人，與 tPerson 表格的 fid 相關
); 

GO
    CREATE TABLE tProduct(
fId INT NOT NULL PRIMARY Key identity (1,1) ,
fName Nvarchar(500) NOT NULL DEFAULT'',
fClassification Nvarchar(100) NOT NULL DEFAULT'',
fPrice INT NOT NULL DEFAULT 1,
fDescription Nvarchar(MAX) NOT NULL DEFAULT'',
fIntroduction NVARCHAR(MAX) NOT NULL DEFAULT '',--0205加新欄位
fQuantity INT NOT NULL DEFAULT 0,
fLaunchAt DATE NOT NULL DEFAULT GETDATE(),  -- 0117toilet要改
fStorage int not null DEFAULT 0,
fOrigin Nvarchar(100) NOT NULL DEFAULT'',
fLaunch INT NOT NULL DEFAULT 0,
fEditor INT NOT NULL
    );
GO

CREATE TABLE tImg ( 
fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 商品Id，主鍵，不能為空 
fProductId INT NOT NULL, -- 商品Id，不能為空 
fName NVARCHAR(500) NOT NULL DEFAULT'', -- 照片名稱，不能為空，預設值為空字串 
fOrderBy INT NOT NULL DEFAULT 1, -- 照片排序，不能為空，預設值為空字串 
fUploadAt DATE NOT NULL DEFAULT GETDATE(), -- 上傳日期，預設為當前日期時間  

fEditor INT NOT NULL     -- 修改人，使用INT表示編輯人員的ID
); 
GO

CREATE TABLE tFavorite (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),   -- 自動遞增的主鍵
    fProductId INT NOT NULL,              -- 商品Id
    fPersonId INT NOT NULL,                 -- 人員Id
   );
GO



CREATE TABLE tInventoryMain (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fBaselineDate  DATE NOT NULL, -- 盤點基準日
    fCreatedAt DATE NOT NULL, -- 盤點單建立日期 
    fEditor INT NOT NULL, -- 編輯者
    fNote NVARCHAR(500) -- 備註
);
GO
CREATE TABLE tInventoryDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fInventoryMainId INT NOT NULL, -- 連接到 tInventoryMain
    fProductId INT NOT NULL, -- 連接到 tProduct 的商品 ID
    fSystemQuantity INT, -- 系統庫存
    fActualQuantity INT -- 實際庫存
);
GO
CREATE TABLE tInventoryAdjustment (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fadjustmentDate DATE NOT NULL, -- 庫存調整日
    fCreatedAt DATE NOT NULL, -- 盤點單建立日期 
    fEditor INT NOT NULL, -- 編輯者
    fNote NVARCHAR(500) NOT NULL DEFAULT '', -- 備註
    fCheckerId INT -- 盤點人員的 ID
);
GO
CREATE TABLE tInventoryAdjustmentDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fInventoryAdjustmentId INT NOT NULL, -- 連接到 tInventoryAdjustment
    fProductId INT NOT NULL , -- 連接到 tProduct 的商品 ID
    fQuantity INT NOT NULL DEFAULT 0, -- 數量
    fCost DECIMAL(18,2) NOT NULL DEFAULT 0.00 -- 記錄盤點時的成本
);

GO
   CREATE TABLE tInvoice (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),         -- 發票 ID，主鍵，自增
    fNumber NVARCHAR(50) NOT NULL,             -- 發票號碼
    fDate datetime NOT NULL,                       -- 發票日期
    fForm NVARCHAR(20) NOT NULL DEFAULT '',    -- 發票格式，預設為空字串
    fCustomerId INT NOT NULL,                  -- 客戶 ID
    fCustomerUbn NVARCHAR(50)  DEFAULT '', -- 客戶統編，預設為空字串
    fProviderId INT NOT NULL,                  -- 供應商 ID
    fProviderUbn NVARCHAR(50) NOT NULL DEFAULT '', -- 供應商統編，預設為空字串
    fInOut INT NOT NULL DEFAULT 0,             -- 收支類型，預設為 0
    fStatus INT NOT NULL DEFAULT 0,            -- 狀態(作廢與否)，預設為 0
    fTotal INT NOT NULL DEFAULT 0,             -- 總金額，預設為 0
    fEditor INT NOT NULL DEFAULT 0             -- 修改人，預設為 0
);
GO

CREATE TABLE tInvoiceDetail (
    fId INT NOT NULL  PRIMARY KEY IDENTITY(1,1),         -- 發票明細 ID，主鍵，自增
    fNumber NVARCHAR(50) NOT NULL,             -- 發票號碼
    fProductName NVARCHAR(50) NOT NULL ,        -- 商品名稱
    fCount INT NOT NULL DEFAULT 1,             -- 數量，預設為 1
    fPrice INT NOT NULL DEFAULT 0,             -- 單價，預設為 0
    fSum INT NOT NULL DEFAULT 0                -- 小計，預設為 0
);
GO
CREATE TABLE tPurchase ( 
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),         -- 採購單 ID，主鍵，自增
    fBuyDate DATETIME NOT NULL DEFAULT GETDATE(),       -- 採購日期，預設為當前日期
    fProviderId INT NOT NULL ,                           -- 供應商 ID
    fInvoiceForm INT NOT NULL DEFAULT 0,                -- 發票格式，預設為 0
    fPayment INT NOT NULL DEFAULT 0,                    -- 支付方式，預設為 0
    fEditor INT NOT NULL DEFAULT 0,            -- 修改人，預設為 0
    fPreTax INT NOT NULL DEFAULT 0,            -- 稅前金額，預設為 0
    fTax INT NOT NULL DEFAULT 0,               -- 稅額，預設為 0
    fTotal INT NOT NULL DEFAULT 0,             -- 總金額，預設為 0
    fNote NVARCHAR(500) NOT NULL DEFAULT ''             -- 備註，預設為空字串
);
GO
CREATE TABLE tPurchaseDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),         -- 明細 ID，主鍵，自增
    fPurchaseId INT NOT NULL  ,                  -- 採購單號，對應到 `tPurchase` 表的 `fId`
    fProductId INT NOT NULL  ,                   -- 產品 ID
    fCount INT NOT NULL DEFAULT 1,             -- 數量，預設為 1
    fPrice INT NOT NULL DEFAULT 0,             -- 單價，預設為 0
    fSum INT NOT NULL DEFAULT 0                -- 小計，預設為 0
);
GO



CREATE TABLE tGoodsInAndOut (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 進出貨單ID，主鍵，自增，從1開始
    fInOut INT NOT NULL DEFAULT 0,                                  -- 進貨或出貨
    fDate DATE NOT NULL DEFAULT GETDATE(),                              -- 進出貨日期
    fInvoiceId INT NOT NULL,                              -- 發票ID
    fProviderId INT NOT NULL DEFAULT 0,                 -- 供應商ID
    fPersonId INT NOT NULL DEFAULT 0,                   -- 客戶ID
    fTotal INT NOT NULL DEFAULT 0,                                  -- 進出貨總額（含稅）
    fEditor INT NOT NULL,                                 -- 修改人
    fNote NVARCHAR(500) NOT NULL DEFAULT ''      -- 備註，最大長度500字符
 );   
GO
 CREATE TABLE tGoodsInAndOutDetail ( 
fId INT NOT NULL PRIMARY KEY IDENTITY(1, 1), -- 自動增長的主鍵 
fGoodsInandOutId INT NOT NULL, -- 不允許為空的欄位 
fProductId INT NOT NULL , -- 不允許為空的欄位 
fCount INT NOT NULL DEFAULT 0, -- 不允許為空的欄位
fPrice INT NOT NULL DEFAULT 0, -- 不允許為空的欄位
fSum INT NOT NULL DEFAULT 0  -- 不允許為空的 fSum 欄位

); 
    GO
CREATE TABLE tOrder(
fId INT NOT NULL PRIMARY Key identity (1,1) ,
fPersonId INT NOT NULL,
fStatus INT NOT NULL default 0,
fPay INT NOT NULL default 0,
fOrderAt DATETIME not null default GETDATE(),
fTotal int NOT NULL DEFAULT 0,
fAddress Nvarchar(MAX) NOT NULL DEFAULT'',
fReceiverName Nvarchar(500) NOT NULL DEFAULT'',
fPhone Nvarchar(50) NOT NULL DEFAULT'',
fNote Nvarchar(500) NOT NULL default ''
    );

    GO
    CREATE TABLE tOrderList(
fId INT NOT NULL PRIMARY Key identity (1,1) ,
fOrderId int not null,
fProductId int not null,
fPrice int not null DEFAULT 0,
fCount int not null DEFAULT 0,
fSum int not null DEFAULT 0
    );
GO
CREATE TABLE tCart(
fId INT NOT NULL PRIMARY Key identity (1,1) ,
fProductId INT NOT NULL,
fCount int not null DEFAULT 1,
fPersonId int not null
    );
GO
CREATE TABLE tReceiptReversal (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),     -- 收款單號，主鍵，自增，從1開始
    fDate DATE NOT NULL DEFAULT GETDATE(),   -- 收款日期
    fPersonId INT NOT NULL,                      -- 客戶ID，不能為空
    fReceiptMethod INT NOT NULL DEFAULT 1,                 -- 收款方式，預設為1
    fTotal INT NOT NULL DEFAULT 0,                                  -- 總金額
    fEditor INT NOT NULL,                         -- 建檔暨修改人員，不能為空
    fNote NVARCHAR(500) NOT NULL DEFAULT ''     -- 備註，不能為空，預設為空字串
);      
GO
CREATE TABLE tReceiptReversalDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),          -- 明細ID，主鍵，自增，從1開始
    fReceiptReversalId INT NOT NULL,             -- 收款單號ID，不能為空
    fOrderId INT NOT NULL      -- 訂單ID，不能為空
);                        
GO
CREATE TABLE tPaymentReversal (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),    -- 付款單號，主鍵，自增，從1開始
    fDate DATE NOT NULL DEFAULT GETDATE(),                         -- 付款日期，不能為空
    fProviderId INT,                             -- 廠商ID，可以為空
    fPaymentMethod INT NOT NULL DEFAULT 0,       -- 付款方式，預設為0，不能為空
    fTotal INT NOT NULL DEFAULT 0,                                  -- 總金額
    fEditor INT NOT NULL,                                 -- 建檔暨修改人員
    fNote NVARCHAR(500) NOT NULL DEFAULT '' -- 備註，預設為空字串，最大長度500字符
 );            
GO
CREATE TABLE tPaymentReversalDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),          -- 主鍵，付款沖帳細項ID，自增
    fPaymentReversalId INT NOT NULL,             -- 付款單號ID，不能為空
    fGoodsInAndOutId INT NOT NULL                -- 進貨單ID，不能為空
 );              
GO
CREATE TABLE tAboutUs (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- 自動遞增的主鍵
    fTitle NVARCHAR(200) NOT NULL DEFAULT '',           -- 標題欄位
    fContent NVARCHAR(MAX) NOT NULL DEFAULT '',         -- 內容欄位
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- 記錄創建時間
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- 記錄更新時間
);
GO
CREATE TABLE tFAQ (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- 自動遞增的主鍵
    fQuestion NVARCHAR(500) NOT NULL DEFAULT '',        -- 問題欄位
    fAnswer NVARCHAR(MAX) NOT NULL DEFAULT '',          -- 答案欄位
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- 記錄創建時間
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- 記錄更新時間
);
GO
CREATE TABLE tReport(
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      
    fPersonId INT NOT NULL,        
    fClass NVARCHAR(10) NOT NULL DEFAULT '其他',   
    fTitle NVARCHAR(100) NOT NULL DEFAULT '',
    fDescription NVARCHAR(MAX) NOT NULL DEFAULT '',      
    fTime DATETIME NOT NULL DEFAULT GETDATE() 
);

USE [dbVegetable]
GO
SET IDENTITY_INSERT [dbo].[tProduct] ON 
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (1, N'大白菜', N'葉菜', 100, N'小白菜', N'
菜單內容如下：
【4種根莖類+2種蕈類+6種葉菜(內容以當季蔬菜組裝出貨)】
a.根莖瓜果雜糧類:小黃瓜、玉米筍、敏豆、牛心番茄、櫛瓜、甜玉米、糯米椒、甜菜根、高麗菜、黃秋葵、絲瓜、白蘿蔔、大黃瓜、南瓜、茄子、人蔘山藥、紅蘿蔔、馬鈴薯、地瓜。
b.蕈類:黑木耳、金絲菇、鴻喜菇、秀珍菇、杏鮑菇、白精靈菇、銀耳、金滑菇。
c.短期葉菜類:大陸A菜、蘿蔓A菜、小白菜、小松菜、黑葉白菜、青江菜、千寶菜、塔菇菜、山茼蒿、空心菜、地瓜葉、紅莧菜。', 1, CAST(N'2024-12-23' AS Date), 2, N'屏東', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (2, N'菠菜', N'葉菜', 150, N'綠色新鮮菠菜', N'
菜單內容如下：
【4種根莖類+2種蕈類+6種葉菜(內容以當季蔬菜組裝出貨)】
a.根莖瓜果雜糧類:小黃瓜、玉米筍、敏豆、牛心番茄、櫛瓜、甜玉米、糯米椒、甜菜根、高麗菜、黃秋葵、絲瓜、白蘿蔔、大黃瓜、南瓜、茄子、人蔘山藥、紅蘿蔔、馬鈴薯、地瓜。
b.蕈類:黑木耳、金絲菇、鴻喜菇、秀珍菇、杏鮑菇、白精靈菇、銀耳、金滑菇。
c.短期葉菜類:大陸A菜、蘿蔓A菜、小白菜、小松菜、黑葉白菜、青江菜、千寶菜、塔菇菜、山茼蒿、空心菜、地瓜葉、紅莧菜。', 1, CAST(N'2024-12-23' AS Date), 1, N'台南', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (3, N'花椰菜', N'葉菜', 200, N'一顆一顆的綠色花椰菜', N'
菜單內容如下：
【4種根莖類+2種蕈類+6種葉菜(內容以當季蔬菜組裝出貨)】
a.根莖瓜果雜糧類:小黃瓜、玉米筍、敏豆、牛心番茄、櫛瓜、甜玉米、糯米椒、甜菜根、高麗菜、黃秋葵、絲瓜、白蘿蔔、大黃瓜、南瓜、茄子、人蔘山藥、紅蘿蔔、馬鈴薯、地瓜。
b.蕈類:黑木耳、金絲菇、鴻喜菇、秀珍菇、杏鮑菇、白精靈菇、銀耳、金滑菇。
c.短期葉菜類:大陸A菜、蘿蔓A菜、小白菜、小松菜、黑葉白菜、青江菜、千寶菜、塔菇菜、山茼蒿、空心菜、地瓜葉、紅莧菜。', 2, CAST(N'2024-12-23' AS Date), 1, N'高雄', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (4, N'洋蔥', N'根莖', 50, N'會哭的洋蔥', N'
菜單內容如下：
【4種根莖類+2種蕈類+6種葉菜(內容以當季蔬菜組裝出貨)】
a.根莖瓜果雜糧類:小黃瓜、玉米筍、敏豆、牛心番茄、櫛瓜、甜玉米、糯米椒、甜菜根、高麗菜、黃秋葵、絲瓜、白蘿蔔、大黃瓜、南瓜、茄子、人蔘山藥、紅蘿蔔、馬鈴薯、地瓜。
b.蕈類:黑木耳、金絲菇、鴻喜菇、秀珍菇、杏鮑菇、白精靈菇、銀耳、金滑菇。
c.短期葉菜類:大陸A菜、蘿蔓A菜、小白菜、小松菜、黑葉白菜、青江菜、千寶菜、塔菇菜、山茼蒿、空心菜、地瓜葉、紅莧菜。', 0, CAST(N'2024-12-23' AS Date), 1, N'屏東', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (5, N'白蘿蔔', N'根莖', 100, N'嘿喲嘿喲拔蘿蔔', N' ', 10, CAST(N'2024-12-23' AS Date), 1, N'屏東', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (6, N'胡蘿蔔', N'根莖', 90, N'新鮮胡蘿蔔，營養又健康', N' ', 50, CAST(N'2025-01-15' AS Date), 2, N'台中', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (7, N'番茄', N'果菜', 80, N'紅透透的番茄，香甜可口', N'', 30, CAST(N'2025-01-10' AS Date), 3, N'高雄', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (8, N'青椒', N'果菜', 60, N'鮮嫩的青椒，適合炒菜', N'', 20, CAST(N'2025-01-12' AS Date), 1, N'台南', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (9, N'玉米', N'果菜', 120, N'香甜的玉米，營養滿分', N'', 40, CAST(N'2025-01-18' AS Date), 4, N'屏東', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (10, N'茄子', N'果菜', 75, N'紫色茄子，鮮美可口', N'', 60, CAST(N'2025-01-20' AS Date), 2, N'南投', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (11, N'蘋果', N'水果', 150, N'新鮮蘋果，甜脆可口', N'', 100, CAST(N'2025-01-14' AS Date), 1, N'花蓮', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (12, N'香蕉', N'水果', 90, N'香蕉，天然無添加', N'', 80, CAST(N'2025-01-16' AS Date), 3, N'台中', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (13, N'葡萄', N'果實', 200, N'新鮮葡萄，果香四溢', N'', 150, CAST(N'2025-01-11' AS Date), 2, N'新竹', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (14, N'芋頭', N'根莖', 110, N'香濃芋頭，口感細膩', N'', 50, CAST(N'2025-01-19' AS Date), 4, N'台北', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (15, N'香菇', N'菇類', 140, N'香氣十足的香菇，適合煮湯', N'', 70, CAST(N'2025-01-13' AS Date), 3, N'宜蘭', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[tProduct] OFF
GO






USE [dbVegetable]
GO
SET IDENTITY_INSERT [dbo].[tImg] ON 
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (1, 1, N'大白菜01.png', 1, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (3, 1, N'大白菜02.png', 2, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (4, 1, N'大白菜03.png', 3, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (5, 5, N'白蘿蔔01.jpg', 1, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (6, 2, N'波菜01.png', 1, CAST(N'2025-02-03' AS Date), 2)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (7, 3, N'花椰菜02.jpg', 1, CAST(N'2025-02-03' AS Date), 2)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (8, 4, N'洋蔥01.jpg', 1, CAST(N'2025-02-03' AS Date), 1)
GO
SET IDENTITY_INSERT [dbo].[tImg] OFF
GO



---------------------------------------—---------------------------------------—---------------------------------------


INSERT INTO [dbo].[tCart] ([fProductId],[fCount],[fPersonId]) VALUES(1,20,1),(2,20,1),(1,10,2),(2,10,2) 
GO

-- 插入 tInventoryMain 資料
INSERT INTO tInventoryMain (fBaselineDate, fCreatedAt, fEditor, fNote)
VALUES 
('2025-02-01', '2025-02-02', 1, '第一次盤點'),
('2025-02-03', '2025-02-04', 2, '第二次盤點'),
('2025-02-05', '2025-02-06', 3, '第三次盤點');

-- 插入 tInventoryDetail 資料
INSERT INTO tInventoryDetail (fInventoryMainId, fProductId, fSystemQuantity, fActualQuantity)
VALUES 
(1, 101, 50, 48),
(1, 102, 30, 28),
(1, 103, 20, 18),
(2, 104, 40, 42),
(2, 105, 25, 23),
(2, 106, 15, 14),
(3, 107, 60, 58),
(3, 108, 35, 34),
(3, 109, 10, 10);

-- 插入 tInventoryAdjustment 資料
INSERT INTO tInventoryAdjustment (fadjustmentDate, fCreatedAt, fEditor, fNote, fCheckerId)
VALUES 
('2025-02-07', '2025-02-07', 1, '第一次調整', 5),
('2025-02-08', '2025-02-08', 2, '第二次調整', 6),
('2025-02-09', '2025-02-09', 3, '第三次調整', 7);

-- 插入 tInventoryAdjustmentDetail 資料
INSERT INTO tInventoryAdjustmentDetail (fInventoryAdjustmentId, fProductId, fQuantity, fCost)
VALUES 
(1, 101, -2, 100.50),
(1, 102, -2, 95.75),
(1, 103, -2, 80.25),
(2, 104, 2, 120.00),
(2, 105, -2, 110.50),
(2, 106, -1, 90.75),
(3, 107, -2, 130.25),
(3, 108, -1, 115.60),
(3, 109, 0, 75.00);

INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Dwayne Strong', '123', '123', '女', '1990-06-08', '0979874614', '079599366', 'Unit 6832 Box 4261\nDPO AE 11131', 'bradley56@roth.com', '33448366', 1, 'William Stephens', '0962975478', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Brian Cole', '456', '456', '女', '1979-01-07', '0969038974', '071316228', '572 Dixon Cliffs\nNew Eileen, AZ 39214', 'paula82@gmail.com', '62276379', 0, 'Amy Johnson', '0995817086', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Penny Hernandez', '789', '789', '男', '1968-09-19', '0964016893', '076332359', '8634 Alicia Radial Apt. 606\nWest Larry, IA 24761', 'derrickjohnson@hotmail.com', '67066326', 1, 'Leah Scott', '0949105231', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Tonya Harrington', '147', '147', '女', '1964-02-27', '0993996385', '071691538', '3599 Sandra Station\nPort Tammie, HI 47783', 'kathryncarpenter@vasquez-lawrence.com', '29663392', 0, 'Maria Good', '0935070180', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Deborah Randall', '258', '258', '男', '1965-06-02', '0939263973', '076542174', '258 Gabriella Squares\nSouth Anthonyton, NM 84414', 'carmen05@potts.com', '57128249', 1, 'Trevor Ward', '0919014552', 1);
GO

-- 插入 tInvoice 資料（10 筆）
INSERT INTO tInvoice (fNumber, fDate, fForm, fCustomerId, fCustomerUbn, fProviderId, fProviderUbn, fInOut, fStatus, fTotal, fEditor)
VALUES
('INV001', '2024-09-27', 0, 101, '12345678', 201, '87654321', 0, 0, 1000, 1),
('INV002', '2025-02-02', 1, 102, '22345678', 202, '97654321', 1, 0, 1500, 2),
('INV003', '2025-05-03', 0, 103, '32345678', 203, '87654322', 0, 1, 2000, 3),
('INV004', '2025-02-04', 1, 104, '42345678', 204, '97654322', 1, 0, 500, 4),
('INV005', '2025-10-05', 0, 105, '52345678', 205, '87654323', 0, 0, 3000, 5),
('INV006', '2025-02-06', 1, 106, '62345678', 206, '97654323', 1, 1, 1200, 6),
('INV007', '2025-08-07', 0, 107, '72345678', 207, '87654324', 0, 0, 900, 7),
('INV008', '2025-02-08', 1, 108, '82345678', 208, '97654324', 1, 0, 1800, 8),
('INV009', '2025-04-09', 0, 109, '92345678', 209, '87654325', 0, 1, 2500, 9),
('INV010', '2025-03-10', 1, 110, '02345678', 210, '97654325', 1, 0, 1600, 10);

-- 插入 tInvoiceDetail 資料（20 筆）
INSERT INTO tInvoiceDetail (fNumber, fProductName, fCount, fPrice, fSum)
VALUES
('INV001', '1', 2, 100, 200),
('INV002', '2', 1, 150, 150),
('INV003', '3', 3, 200, 600),
('INV004', '4', 2, 50, 100),
('INV005', '5', 4, 100, 400),
('INV006', '6', 3, 90, 270),
('INV007', '7', 2, 80, 160),
('INV008', '8', 1, 60, 60),
('INV009', '9', 2, 120, 240),
('INV010', '10', 3, 75, 225),
('INV001', '11', 2, 150, 300),
('INV002', '12', 3, 90, 270),
('INV003', '13', 1, 200, 200),
('INV004', '14', 2, 110, 220),
('INV005', '15', 3, 140, 420),
('INV006', '1', 1, 100, 100),
('INV007', '2', 2, 150, 300),
('INV008', '3', 3, 200, 600),
('INV009', '4', 1, 50, 50),
('INV010', '5', 2, 100, 200);

-- 插入 tPurchase 資料（10 筆）
INSERT INTO tPurchase (fBuyDate, fProviderId, fInvoiceForm, fPayment, fEditor, fPreTax, fTax, fTotal, fNote)
VALUES
('2025-03-01', 301, 0, 0, 1, 900, 90, 990, '採購1'),
('2025-02-02', 302, 1, 1, 2, 1500, 150, 1650, '採購2'),
('2025-04-03', 303, 0, 0, 3, 2000, 200, 2200, '採購3'),
('2025-05-04', 304, 1, 1, 4, 500, 50, 550, '採購4'),
('2025-02-05', 305, 0, 0, 5, 3000, 300, 3300, '採購5'),
('2025-06-06', 306, 1, 1, 6, 1200, 120, 1320, '採購6'),
('2025-07-07', 307, 0, 0, 7, 900, 90, 990, '採購7'),
('2025-12-08', 308, 1, 1, 8, 1800, 180, 1980, '採購8'),
('2025-10-09', 309, 0, 0, 9, 2500, 250, 2750, '採購9'),
('2025-08-10', 310, 1, 1, 10, 1600, 160, 1760, '採購10');

INSERT INTO tPurchaseDetail (fPurchaseId, fProductId, fCount, fPrice, fSum)
VALUES
(1, 1, 2, 100, 200),
(2, 2, 1, 150, 150),
(3, 3, 3, 200, 600),
(4, 4, 2, 50, 100),
(5, 5, 4, 100, 400),
(6, 6, 3, 90, 270),
(7, 7, 2, 80, 160),
(8, 8, 1, 60, 60),
(9, 9, 2, 120, 240),
(10, 10, 3, 75, 225),
(1, 11, 2, 150, 300),
(2, 12, 3, 90, 270),
(3, 13, 1, 200, 200),
(4, 14, 2, 110, 220),
(5, 15, 3, 140, 420),
(6, 1, 1, 100, 100),
(7, 2, 2, 150, 300),
(8, 3, 3, 200, 600),
(9, 4, 1, 50, 50),
(10, 5, 2, 100, 200);

