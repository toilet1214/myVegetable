USE master
--drop table [dbo].[tPurchase]
--drop table[dbo].[tPurchaseDetail]
--drop table[dbo].[tInvoice]
--drop table[dbo].[tInvoiceDetail]
--drop DATABASE [dbVegetable]
--drop table[dbo].[tPerson]




CREATE DATABASE [dbVegetable]
GO
USE dbVegetable

GO
   CREATE TABLE tPerson(
fId INT NOT NULL Primary Key identity (1,1) ,
fName Nvarchar(500) NOT NULL DEFAULT '',
fAccount Nvarchar(500) NOT NULL UNIQUE,--用Email
fPassword Nvarchar(500) ,
fGender Nvarchar(500) NOT NULL DEFAULT '',
fBirth Date NOT NULL DEFAULT GETDATE(),
fPhone Nvarchar(500) NOT NULL DEFAULT '',
fTel Nvarchar(500) NOT NULL DEFAULT '',
fAddress Nvarchar(MAX) NOT NULL DEFAULT '',
fUBN Nvarchar(500) DEFAULT '',
fPermission int NOT NULL DEFAULT 0,
fEmp Nvarchar(500) DEFAULT '',
fEmpTel Nvarchar(500)  DEFAULT '',
fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
fEditor INT NOT NULL DEFAULT 0,
fIsVerified BIT NOT NULL DEFAULT 0,
fLoginType INT NOT NULL DEFAULT 0,
fGoogleId NVARCHAR(500),
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
fEditor INT NOT NULL );      -- 修改人，與 tPerson 表格的 fid 相關


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

fEditor INT NOT NULL );     -- 修改人，使用INT表示編輯人員的ID

GO

CREATE TABLE tFavorite (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),   -- 自動遞增的主鍵
    fProductId INT NOT NULL,              -- 商品Id
    fPersonId INT NOT NULL,     );               -- 人員Id

GO



CREATE TABLE tInventoryMain (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fBaselineDate  DATE NOT NULL, -- 盤點基準日
    fCreatedAt DATE NOT NULL, -- 盤點單建立日期 
    fEditor INT NOT NULL, -- 編輯者
    fNote NVARCHAR(500) );-- 備註

GO
CREATE TABLE tInventoryDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fInventoryMainId INT NOT NULL, -- 連接到 tInventoryMain
    fProductId INT NOT NULL, -- 連接到 tProduct 的商品 ID
    fSystemQuantity INT, -- 系統庫存
    fActualQuantity INT ); -- 實際庫存

GO
CREATE TABLE tInventoryAdjustment (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fadjustmentDate DATE NOT NULL, -- 庫存調整日
    fCreatedAt DATE NOT NULL, -- 盤點單建立日期 
    fEditor INT NOT NULL, -- 編輯者
    fNote NVARCHAR(500) NOT NULL DEFAULT '', -- 備註
    fCheckerId INT); -- 盤點人員的 ID

GO
CREATE TABLE tInventoryAdjustmentDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- 主鍵
    fInventoryAdjustmentId INT NOT NULL, -- 連接到 tInventoryAdjustment
    fProductId INT NOT NULL , -- 連接到 tProduct 的商品 ID
    fQuantity INT NOT NULL DEFAULT 0, -- 數量
    fCost DECIMAL(18,2) NOT NULL DEFAULT 0.00); -- 記錄盤點時的成本


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
    fEditor INT NOT NULL DEFAULT 0       );      -- 修改人，預設為 0

GO

CREATE TABLE tInvoiceDetail (
    fId INT NOT NULL  PRIMARY KEY IDENTITY(1,1),         -- 發票明細 ID，主鍵，自增
    fNumber NVARCHAR(50) NOT NULL,             -- 發票號碼
    fProductName NVARCHAR(50) NOT NULL ,        -- 商品名稱
    fCount INT NOT NULL DEFAULT 1,             -- 數量，預設為 1
    fPrice INT NOT NULL DEFAULT 0,             -- 單價，預設為 0
    fSum INT NOT NULL DEFAULT 0        );        -- 小計，預設為 0

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
    fNote NVARCHAR(500) NOT NULL DEFAULT ''       );      -- 備註，預設為空字串

GO
CREATE TABLE tPurchaseDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),         -- 明細 ID，主鍵，自增
    fPurchaseId INT NOT NULL  ,                  -- 採購單號，對應到 `tPurchase` 表的 `fId`
    fProductId INT NOT NULL  ,                   -- 產品 ID
    fCount INT NOT NULL DEFAULT 1,             -- 數量，預設為 1
    fPrice INT NOT NULL DEFAULT 0,             -- 單價，預設為 0
    fSum INT NOT NULL DEFAULT 0           );     -- 小計，預設為 0

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
    fNote NVARCHAR(500) NOT NULL DEFAULT ''   );     -- 備註，最大長度500字符
  
GO
 CREATE TABLE tGoodsInAndOutDetail ( 
fId INT NOT NULL PRIMARY KEY IDENTITY(1, 1), -- 自動增長的主鍵 
fGoodsInandOutId INT NOT NULL, -- 不允許為空的欄位 
fProductId INT NOT NULL , -- 不允許為空的欄位 
fCount INT NOT NULL DEFAULT 0, -- 不允許為空的欄位
fPrice INT NOT NULL DEFAULT 0, -- 不允許為空的欄位
fSum INT NOT NULL DEFAULT 0 );  -- 不允許為空的 fSum 欄位


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
fNote Nvarchar(500) NOT NULL default '',
fMerchantTradeNo Nvarchar(50) not null default  ''
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
CREATE TABLE tReport(
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      
    fPersonId INT NOT NULL,        
    fClass NVARCHAR(10) NOT NULL DEFAULT '其他',   
    fTitle NVARCHAR(100) NOT NULL DEFAULT '',
    fDescription NVARCHAR(MAX) NOT NULL DEFAULT '',      
    fTime DATETIME NOT NULL DEFAULT GETDATE() );

GO
CREATE TABLE tVerification (
    fId INT IDENTITY(1,1) PRIMARY KEY,   -- 驗證記錄 ID（自動遞增）
    fPersonId INT NOT NULL,                            -- 關聯的用戶 ID（對應 tUsers.FUserID）
    fToken NVARCHAR(255) UNIQUE NOT NULL,           -- 驗證 Token（唯一，用於驗證）
    fTokenType NVARCHAR(20) NOT NULL,               -- Token 類型驗證/忘記密碼
    fTokenSentTime DATETIME DEFAULT GETDATE(),      -- Token 發送時間
    fExpirationTime DATETIME NOT NULL,              -- Token 過期時間（10分鐘）
    fIsUsed BIT NOT NULL DEFAULT 0,                    -- 是否已使用（0=未使用，1=已使用）
    fUsedTime DATETIME,             );                -- Token 使用時間（若已使用則記錄）

GO
CREATE TABLE tComment (
    fId INT NOT NULL PRIMARY Key identity (1,1) ,-- 評論ID，自動增長
    fPersonId INT NOT NULL,						 -- 會員ID
    fProductId INT NOT NULL,					 -- 商品ID
    fOrderListId INT NOT NULL,				--訂單細項ID
    fComment NVARCHAR(MAX),		 -- 評論內容
    fStar INT NOT NULL,							 -- 評分
    fCreatedAt DATE NOT NULL DEFAULT GETDATE(), ); -- 創建日期，默認為當前時間

GO
CREATE TABLE EcpayOrders (
    [MerchantTradeNo]      NVARCHAR (50) NOT NULL,
    [MemberID]             NVARCHAR (50) NULL,
    [RtnCode]              INT           NULL,
    [RtnMsg]               NVARCHAR (50) NULL,
    [TradeNo]              NVARCHAR (50) NULL,
    [TradeAmt]             INT           NULL,
    [PaymentDate]          DATETIME      NULL,
    [PaymentType]          NVARCHAR (50) NULL,
    [PaymentTypeChargeFee] NVARCHAR (50) NULL,
    [TradeDate]            NVARCHAR (50) NULL,
    [SimulatePaid]         INT           NULL,
    CONSTRAINT [PK_EcpayOrders] PRIMARY KEY CLUSTERED ([MerchantTradeNo] ASC)
);

-------------(插入資料)-------------------------------------------
--把資料放這邊

INSERT INTO [dbo].[tCart] ([fProductId],[fCount],[fPersonId]) VALUES(1,20,1),(2,20,1),(1,10,2),(2,10,2) 
GO

-- 插入 tInventoryMain
INSERT INTO tInventoryMain (fBaselineDate, fCreatedAt, fEditor, fNote)
VALUES
('2025-02-01', '2025-02-01', 1, ''),
('2025-02-02', '2025-02-02', 2, ''),
('2025-02-03', '2025-02-03', 3, ''),
('2025-02-04', '2025-02-04', 1, ''),
('2025-02-05', '2025-02-05', 2, '');

-- 插入 tInventoryDetail
INSERT INTO tInventoryDetail (fInventoryMainId, fProductId, fSystemQuantity, fActualQuantity)
VALUES
(1, 1, 10, 8),
(1, 2, 5, 5),
(2, 1, 15, 15),
(2, 2, 20, 18),
(2, 3, 25, 24),
(3, 1, 12, 12),
(3, 2, 14, 13),
(4, 1, 10, 9),
(4, 3, 20, 19),
(5, 2, 30, 29),
(3, 8, 5, 75.00);



-- 插入 tInvoice 資料（10 筆）
INSERT INTO tInvoice (fNumber, fDate, fForm, fCustomerId, fCustomerUbn, fProviderId, fProviderUbn, fInOut, fStatus, fTotal, fEditor)
VALUES
('AA-13965913', '2024-09-27', 0, 101, '12345678', 1, '87654321', 0, 0, 1000, 1),
('BB-25147836', '2025-02-02', 1, 102, '22345678', 2, '97654321', 1, 0, 1500, 2),
('CC-36258749', '2025-05-03', 0, 103, '32345678', 3, '87654322', 0, 1, 2000, 3),
('DD-47859620', '2025-02-04', 1, 104, '42345678', 4, '97654322', 1, 0, 500, 4),
('EE-58962371', '2025-10-05', 0, 105, '52345678', 5, '87654323', 0, 0, 3000, 5),
('FF-69783215', '2025-02-06', 1, 106, '62345678', 6, '97654323', 1, 1, 1200, 6),
('GG-78936542', '2025-08-07', 0, 107, '72345678', 7, '87654324', 0, 0, 900, 7),
('HH-89641253', '2025-02-08', 1, 108, '82345678', 8, '97654324', 1, 0, 1800, 8),
('II-96587412', '2025-04-09', 0, 109, '92345678', 9, '87654325', 0, 1, 2500, 9),
('JJ-10236547', '2025-03-10', 1, 110, '02345678', 10, '97654325', 1, 0, 1600, 10);

-- 插入 tInvoiceDetail 資料（20 筆）
INSERT INTO tInvoiceDetail (fNumber, fProductName, fCount, fPrice, fSum)
VALUES
('AA-13965913', '有機甜菜根', 2, 100, 200),
('BB-25147836', '有機白蘿蔔', 1, 150, 150),
('CC-36258749', '有機紅蘿蔔', 3, 200, 600),
('DD-47859620', '台灣馬鈴薯', 2, 50, 100),
('EE-58962371', '台灣地瓜', 4, 100, 400),
('FF-69783215', '有機洋蔥', 3, 90, 270),
('GG-78936542', '有機豆芽菜', 2, 80, 160),
('HH-89641253', '有機青江菜', 1, 60, 60),
('II-96587412', 'BoPie菠菜', 2, 120, 240),
('JJ-10236547', '台灣有機萵苣', 3, 75, 225),
('AA-13965913', '台灣香蕉', 2, 150, 300),
('BB-25147836', '美國富士進口蘋果', 3, 90, 270),
('CC-36258749', '有機牛番茄', 1, 200, 200),
('DD-47859620', '台灣在地柳丁', 2, 110, 220),
('EE-58962371', '酸酸台灣檸檬', 3, 140, 420),
('FF-69783215', '有機甜菜根', 1, 100, 100),
('GG-78936542', '有機白蘿蔔', 2, 150, 300),
('HH-89641253', '有機紅蘿蔔', 3, 200, 600),
('II-96587412', '台灣馬鈴薯', 1, 50, 50),
('JJ-10236547', '台灣地瓜', 2, 100, 200);


-- 插入 tPurchase 資料（10 筆）
INSERT INTO tPurchase (fBuyDate, fProviderId, fInvoiceForm, fPayment, fEditor, fPreTax, fTax, fTotal, fNote)
VALUES
('2025-03-01', 1, 0, 0, 1, 900, 90, 990, '無'),
('2025-02-02', 2, 1, 1, 2, 1500, 150, 1650, '無'),
('2025-04-03', 3, 0, 0, 3, 2000, 200, 2200, '無'),
('2025-05-04', 4, 1, 1, 4, 500, 50, 550, '無'),
('2025-02-05', 5, 0, 0, 5, 3000, 300, 3300, '無'),
('2025-06-06', 6, 1, 1, 6, 1200, 120, 1320, '無'),
('2025-07-07', 7, 0, 0, 7, 900, 90, 990, '無'),
('2025-12-08', 8, 1, 1, 8, 1800, 180, 1980, '無'),
('2025-10-09', 9, 0, 0, 9, 2500, 250, 2750, '無'),
('2025-08-10', 10, 1, 1, 10, 1600, 160, 1760, '無');

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



INSERT INTO tProvider (fName, fUbn, fTel, fConnect, fAccountant, fAddress, fDelivery, fInvoiceAdd, fEditor) VALUES
('孟莉有限公司', '75584465', '02-79922165', '鄭力菲', '鄭力菲', '新北市新店區自由街7號', '新北市新店區自由街7號', '新北市新店區自由街7號', 1),
('蟬業有限公司', '74144039', '04-80122559', '鐘曉輝', '吳茹蓉', '台中市南區建國路123號', '台中市南區建國路123號', '台中市南區建國路123號', 1),
('鉦琛有限公司', '43919037', '07-8964031', '林凰陣', '林凰陣', '高雄市前鎮區中華路789號', '高雄市前鎮區中華路789號', '高雄市前鎮區中華路789號', 1),
('淮語有限公司', '79446462', '02-30062388', '謝塵鑾', '黃冉棠', '臺北市文山區景明街29號', '臺北市文山區景明街29號', '臺北市文山區景明街29號', 1),
('岫登有限公司', '51058813', '956789012', '黎展嶽', '顏倫彰', '新北市板橋區民生路202號', '新北市板橋區民生路202號', '新北市板橋區民生路202號', 1),
('綵翎有限公司', '21125385', '967890123', '王運道', '邱影葉', '桃園市中壢區中原路303號', '桃園市中壢區中原路303號', '桃園市中壢區中原路303號', 1),
('淋洛有限公司', '13277628', '978901234', '鄭帆洋', '鄭帆洋', '台南市東區東門路404號', '台南市東區東門路404號', '台南市東區東門路404號', 1),
('睿敏有限公司', '56445981', '989012345', '黃國藻', '黃國藻', '基隆市中正區和平路505號', '基隆市中正區和平路505號', '基隆市中正區和平路505號', 1),
('育蔓有限公司', '47755302', '990123456', '王毓琦', '韓煥羚', '新竹市東區光明路606號', '新竹市東區光明路606號', '新竹市東區光明路606號', 1),
('高宜有限公司', '44205065', '901234567', '陳又蓉', '陳又蓉', '彰化市彰化區中山路707號', '彰化市彰化區中山路707號', '彰化市彰化區中山路707號', 1),
('蓉躍有限公司', '95600594', '04-84518083', '吳靜飄', '吳靜飄', '台中市北區建國路808號', '台中市北區建國路808號', '台中市北區建國路808號', 1),
('菖曦有限公司', '14958278', '07-8689957', '白倩珈', '林珂紫', '高雄市鳳山區中山路909號', '高雄市鳳山區中山路909號', '高雄市鳳山區中山路909號', 1),
('生衣有限公司', '88166949', '02-50703374', '石舟輝', '龔子恆', '台北市中山區民生東路1010號', '台北市中山區民生東路1010號', '台北市中山區民生東路1010號', 1),
('絹倢有限公司', '60012888', '944455566', '張中宇', '鄭慈鋒', '新竹市香山區中華路1111號', '新竹市香山區中華路1111號', '新竹市香山區中華路1111號', 1),
('鉦地有限公司', '72330079', '955566677', '田彥遠', '陳瀾笑', '新北市三峽區大學路1212號', '新北市三峽區大學路1212號', '新北市三峽區大學路1212號', 1),
('灝陞有限公司', '28038288', '966677788', '許記雪', '郭力玫', '台南市南區大同路1313號', '台南市南區大同路1313號', '台南市南區大同路1313號', 1),
('岐釋有限公司', '59047641', '977788899', '陳宸雄', '鄭勵折', '基隆市七堵區和平路1414號', '基隆市七堵區和平路1414號', '基隆市七堵區和平路1414號', 1),
('徽甲有限公司', '72835402', '02-85058129', '王堇彤', '王堇彤', '台北市大安區光復北路1515號', '台北市大安區光復北路1515號', '台北市大安區光復北路1515號', 1),
('挺葉有限公司', '928676', '04-88413777', '劉俞昊', '劉俞昊', '台中市南區建國路1616號', '台中市南區建國路1616號', '台中市南區建國路1616號', 1),
('煇菀有限公司', '12345678', '02-2345678', '李小明', '王大華', '台北市信義區光復路123號', '台北市信義區光復路123號', '台北市信義區光復路123號', 1),
('恭滿有限公司', '23456789', '03-3456789', '陳志強', '李小華', '新北市板橋區中山路456號', '新北市板橋區中山路456號', '新北市板橋區中山路456號', 1),
('楠冬有限公司', '34567890', '04-4567890', '張怡君', '陳國華', '台中市南區建國路789號', '台中市南區建國路789號', '台中市南區建國路789號', 1),
('夆昌溥有限公司', '45678901', '05-5678901', '劉志豪', '王小美', '高雄市三民區建國路321號', '高雄市三民區建國路321號', '高雄市三民區建國路321號', 1),
('運通製業有限公司', '56789012', '02-6789012', '王淑貞', '陳志明', '台北市中山區建國北路654號', '台北市中山區建國北路654號', '台北市中山區建國北路654號', 1),
('沛城漁港有限公司', '12345678', '02-2345678', '陳淑臻', '陳淑臻', '台北市信義區市府路', '台北市信義區市府路', '台北市信義區市府路123號', 1),
('龍運樓有限公司', '23456789', '03-3456789', '張浩', '陳婷', '新北市板橋區中山路', '新北市板橋區中山路', '新北市板橋區中山路49號', 1),
('協興泰茶樓有限公司', '34567890', '04-1234567', '王大明', '黃晶晶', '台中市南區建國路', '台中市南區建國路', '台中市南區建國路35號', 1),
('堅毅行有限公司', '45678901', '02-3456789', '蔡宇', '蔡宇', '高雄市苓雅區建國路', '高雄市苓雅區建國路', '高雄市苓雅區建國路123號', 1),
('信基廚房有限公司', '56789012', '05-2345678', '陳凡欣', '陳凡欣', '桃園市中壢區光明路', '桃園市中壢區光明路', '桃園市中壢區光明路123號', 1);
INSERT INTO tPerson (
    fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fUBN, fPermission, 
    fEmp, fEmpTel, fCreatedAt, fEditor, fIsVerified, fLoginType, fGoogleId
) VALUES 
('吳小莉', 'lisi@email.com', 'bfzkuqufvfD6', '女', '1985-03-22', '0922345678', '03-23456782', '新北市板橋區中山路5000號', '1', 0, 'lisi@email.com', '98129965', '2025-02-04T00:00:00.000', 0, 0, 0, NULL),
('陳七', 'wangwu@email.com', 'a69Xfts7jw9h', '男', '1988-07-10', '0932345678', '04-3456789', '桃園市中壢區成功路9號', '1', 0, 'wangwu@email.com', '40026863', '2025-02-04T09:00:00.000', 0, 0, 0, NULL),
('李曉明', 'zhaoliu@email.com', 'Vquqj4zk6a9a', '女', '1992-12-15', '0942345678', '05-4567890', '基隆市仁愛區中正路', '1', 0, 'zhaoliu@email.com', NULL, '2025-02-04T09:30:00.000', 1, 0, 0, NULL),
('蔡美玲', 'chenqi@email.com', 'Pvgkwgkh2fk7', '男', '1980-04-05', '0952345678', '06-6789012', '台中市南區建國路', '1', 0, 'chenqi@email.com', NULL, '2025-02-04T10:00:00.000', 1, 0, 0, NULL),
('林八', 'linba@email.com', 'Auz9g28dfph9', '女', '1995-08-25', '0962345678', '07-7890123', '高雄市苓雅區中正路', '1', 0, 'linba@email.com', '57150921', '2025-02-04T10:30:00.000', 0, 0, 0, NULL),
('王建明', 'guojiu@email.com', 'q4A27rq72cab', '男', '1993-11-11', '0972345678', '08-8901234', '新竹市東區光復路', '1', 0, 'guojiu@email.com', NULL, '2025-02-04T11:00:00.000', 0, 0, 0, NULL),
('吳十', 'wushi@email.com', 'gejhiyztnk88', '女', '1991-09-09', '0982345678', '09-9012345', '彰化市鹿港鎮中山路', '1', 0, 'wushi@email.com', NULL, '2025-02-04T11:30:00.000', 1, 0, 0, NULL),
('張大偉', 'qiushi@email.com', 'emuctj6famq3', '男', '1986-02-18', '0913345678', '02-3456789', '台南市東區中華路', '1', 0, 'qiushi@email.com', NULL, '2025-02-04T12:00:00.000', 0, 0, 0, NULL),
('高志強', 'caishier@email.com', 'tb983bgqhjzy', '女', '1994-06-30', '0923345678', '03-4567890', '宜蘭市羅東鎮文化路', '1', 0, 'caishier@email.com', NULL, '2025-02-04T12:30:00.000', 0, 0, 0, NULL),
('劉德華', 'liudehua@email.com', 'fv7b9ktdaz6b', '男', '1985-05-23', '0923456789', '03-4567890', '新竹市北區建國路', '1', 0, 'liudehua@email.com', NULL, '2025-02-04T13:30:00.000', 0, 0, 0, NULL),
('林美玲', 'linmeiling@email.com', 'u5seunv37xje', '女', '1990-09-05', '0934567890', '04-5678901', '台中市西區自由路', '1', 0, 'linmeiling@email.com', NULL, '2025-02-04T14:00:00.000', 0, 0, 0, NULL),
('陳國華', 'chenguohe@email.com', 'fwzcgweg8n48', '男', '1982-02-15', '0945678901', '05-6789012', '高雄市三民區建國路', '1', 0, 'chenguohe@email.com', '69587054', '2025-02-04T14:30:00.000', 0, 0, 0, NULL),
('黃志強', 'huangzhiqiang@email.com', 'zk4bqqjzypiw', '男', '1989-07-08', '0956789012', '06-7890123', '台南市中西區府前路', '1', 0, 'huangzhiqiang@email.com', '90633152', '2025-02-04T15:00:00.000', 0, 0, 0, NULL),
('徐玉珍', 'xuyuzhen@email.com', 'vifd9iy6bn5p', '女', '1995-03-18', '0967890123', '07-8901234', '基隆市仁愛區和平路', '1', 0, 'xuyuzhen@email.com', '60031610', '2025-02-04T15:30:00.000', 0, 0, 0, NULL),
('張嘉玲', 'zhangjialing@email.com', 'epywfxc5gspd', '女', '1993-08-30', '0978901234', '08-9012345', '新北市新莊區建國路', '1', 0, 'zhangjialing@email.com', '21154846', '2025-02-04T16:00:00.000', 0, 0, 0, NULL),
('陳欣怡', 'chenxinyi@email.com', 'ax65j8q8y6rc', '女', '1988-11-10', '0989012345', '09-0123456', '桃園市平鎮區復興路', '1', 0, 'chenxinyi@email.com', '12345678', '2025-02-04T16:30:00.000', 0, 0, 0, NULL),
('劉思妤', 'liusiwei@email.com', 'as55vqbydhss', '女', '1994-05-22', '0912349876', '02-3456789', '台北市內湖區民權東路', '1', 0, 'liusiwei@email.com', NULL, '2025-02-04T17:00:00.000', 1, 0, 0, NULL),
('黃伯翔', 'zhangsan@example.com', 'Abcd1234', '男', '1990-05-15', '0912345678', '02-2345678', '台北市信義區信義路', '1', 0, 'zhangsan@example.com', NULL, '2025-02-19T09:00:00.000', 0, 0, 0, NULL),
('楊綺舒', 'lisi@example.com', 'Xyz7890', '女', '1992-08-20', '0987654321', '03-3456789', '新北市板橋區中山路', '1', 0, 'lisi@example.com', '12345678', '2025-02-19T09:05:00.000', 1, 0, 0, NULL),
('阮嶽陣', 'wangwu@example.com', 'Dfgh5678', '男', '1985-03-10', '0922334455', '04-1234567', '桃園市中壢區中原路', '1', 0, 'wangwu@example.com', '87654321', '2025-02-19T09:10:00.000', 1, 0, 0, NULL),
('梁葉蓉', 'zhaoliu@example.com', 'Qwerty12', '女', '1994-11-30', '0933445566', '05-2345678', '台中市南區建國路', '1', 0, 'zhaoliu@example.com', NULL, '2025-02-19T09:15:00.000', 2, 0, 0, NULL),
('吳光禧', 'zhangqi@example.com', 'Asdf9876', '男', '1988-07-25', '0911223344', '02-3456789', '高雄市三民區建國路', '1', 0, 'zhangqi@example.com', '11223344', '2025-02-19T09:20:00.000', 0, 0, 0, NULL),
('王小明', 'wangxiaoming@example.com', 'Hjkl1234', '男', '1987-01-15', '0912345678', '02-2345678', '台北市大安區和平東路', '1', 0, 'wangxiaoming@example.com', NULL, '2025-02-19T10:00:00.000', 0, 0, 0, NULL),
('林志玲', 'linzhiling@example.com', 'Qwer5678', '女', '1990-04-25', '0987654321', '03-3456789', '新竹市東區光復路', '1', 0, 'linzhiling@example.com', '23456789', '2025-02-19T10:05:00.000', 1, 0, 0, NULL),
('陳建國', 'chenjianguo@example.com', 'F2PqpiiJ', '男', '1982-09-12', '0922334455', '04-1234567', '台中市北區建國路', '1', 0, 'chenjianguo@example.com', '11223344', '2025-02-19T10:10:00.000', 2, 0, 0, NULL),
('鄭欣宜', 'zhengxinyi@example.com', 'KRBU1rYy', '女', '1995-02-18', '0933445566', '05-2345678', '高雄市左營區蓮池潭路', '1', 0, 'zhengxinyi@example.com', NULL, '2025-02-19T10:15:00.000', 0, 0, 0, NULL),
('李永康', 'liyongkang@example.com', 'hiJU4ygW', '男', '1993-06-05', '0911223344', '02-3456789', '桃園市八德區建國路', '1', 0, 'liyongkang@example.com', '55667788', '2025-02-19T10:20:00.000', 1, 0, 0, NULL),
('趙媛義', 'caiyilin@example.com', 'UytQBVy4', '女', '1989-09-15', '0912123456', '02-3456789', '台北市中山區長春路', '1', 0, 'caiyilin@example.com', NULL, '2025-02-19T11:00:00.000', 0, 0, 0, NULL),
('侯名森', 'zhoujielun@example.com', 'EK6uAghv', '男', '1978-01-12', '0922333444', '03-1234567', '新北市三重區民生路', '1', 0, 'zhoujielun@example.com', '12345678', '2025-02-19T11:05:00.000', 1, 0, 0, NULL),
('蔣博方', 'huangzichao@example.com', '87AFdBDm', '男', '1991-07-20', '0911234567', '04-2345678', '台中市西區建國路', '1', 0, 'huangzichao@example.com', NULL, '2025-02-19T11:10:00.000', 0, 0, 0, NULL),
('李柏洪', 'linjunjie@example.com', 'z43yFdRh', '男', '1985-03-30', '0922445566', '05-3456789', '高雄市苓雅區建國路', '1', 0, 'linjunjie@example.com', '23456789', '2025-02-19T11:15:00.000', 1, 0, 0, NULL),
('楊海盈', 'qiuzhe@example.com', 'UvsngdMB', '男', '1990-11-10', '0933556677', '02-1234567', '台北市內湖區成功路', '1', 0, 'qiuzhe@example.com', '34567890', '2025-02-19T11:20:00.000', 2, 0, 0, NULL);

USE [dbVegetable]
GO
SET IDENTITY_INSERT [dbo].[tImg] ON 
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (2, 1, N'S__2433029.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (6, 1, N'S__2433031.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (7, 1, N'S__2433032.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (8, 2, N'S__2433038.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (9, 2, N'S__2433034.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (10, 2, N'S__2433037.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (11, 3, N'S__2433042.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (12, 3, N'S__2433039.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (13, 3, N'S__2433041.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (14, 4, N'S__2433048.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (15, 4, N'S__2433044.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (16, 4, N'S__2433046.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (17, 5, N'S__2433049.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (18, 5, N'S__2433052.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (19, 5, N'S__2433053.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (20, 6, N'S__2433061.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (21, 6, N'S__2433064.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (22, 6, N'S__2433058.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (23, 7, N'S__2433058.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (24, 7, N'S__2433058.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (25, 7, N'S__2433067.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (26, 8, N'S__2449414.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (27, 8, N'S__2449412.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (28, 8, N'S__2449417.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (29, 9, N'S__2449420.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (30, 9, N'S__2449422.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (31, 9, N'S__2449420.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (32, 10, N'S__2449430.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (33, 10, N'S__2449432.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (34, 10, N'S__2449431.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (35, 11, N'S__2449442.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (36, 11, N'S__2449438.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (38, 11, N'S__2449436.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (39, 12, N'S__2449449.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (40, 12, N'S__2449444.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (41, 12, N'S__2449448.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (42, 13, N'S__2449450.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (43, 13, N'S__2449455.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (44, 13, N'S__2449456.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (46, 14, N'S__2449460.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (47, 14, N'S__2449462.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (48, 14, N'S__2449461.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (49, 15, N'S__2449466.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (50, 15, N'S__2449468.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (51, 15, N'S__2449469.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (55, 18, N'S__2449476.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (56, 18, N'S__2449478.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (57, 18, N'S__2449480.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (58, 20, N'S__2449490.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (60, 20, N'S__2449485.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (61, 20, N'S__2457605.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (62, 21, N'S__2449492.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (63, 21, N'S__2449498.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (64, 21, N'S__2449496.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (65, 22, N'S__2457606.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (67, 22, N'S__2457608.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (68, 22, N'S__2457607.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (69, 23, N'S__2457616.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (70, 23, N'S__2457618.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (71, 23, N'S__2457617.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (72, 24, N'S__2457624.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (73, 24, N'S__2457621.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (74, 24, N'S__2457627.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (76, 25, N'S__2457638.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (77, 25, N'S__2457633.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (78, 25, N'S__2457634.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (79, 26, N'S__2457644.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (80, 26, N'S__2457646.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (81, 26, N'S__2457645.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (82, 27, N'S__2457652.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (83, 27, N'S__2457653.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (84, 27, N'S__2457655.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (85, 28, N'S__2457669.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (86, 28, N'S__2457667.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (87, 28, N'S__2457666.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (89, 29, N'S__2457681.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (90, 29, N'S__2457672.jpg', 2, CAST(N'2025-02-20' AS Date), 2)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (91, 29, N'S__2457674.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (92, 30, N'S__2457686.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (93, 30, N'S__2457683.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (95, 30, N'S__2457681.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (96, 31, N'S__2457690.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (97, 31, N'S__2457690.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (98, 31, N'S__2457692.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (99, 32, N'S__2457696.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (100, 32, N'S__2457699.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (101, 32, N'S__2457700.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (102, 33, N'S__2457707.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (103, 33, N'S__2457708.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (104, 33, N'S__2457705.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (105, 34, N'S__2473993.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (106, 34, N'S__2473994.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (107, 34, N'S__2473992.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (108, 35, N'S__2473995.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (109, 35, N'S__2473997.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (110, 35, N'S__2473998.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (111, 36, N'S__2474003.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (112, 36, N'S__2474002.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (113, 36, N'S__2474000.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (114, 37, N'S__2474008.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (115, 37, N'S__2474006.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (116, 37, N'S__2474011.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (117, 38, N'S__2474015.jpg', 1, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (119, 38, N'S__2474014.jpg', 2, CAST(N'2025-02-20' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (120, 38, N'S__2474012.jpg', 3, CAST(N'2025-02-20' AS Date), 1)
GO
SET IDENTITY_INSERT [dbo].[tImg] OFF
GO




USE [dbVegetable]
GO
SET IDENTITY_INSERT [dbo].[tProduct] ON 
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (1, N'有機甜菜根', N'果菜', 120, N'( 400g±5%/包)
鮮甜的有機甜菜根，天然無添加', N'◎本產有機甜菜根
◎鮮採鮮出，保證新鮮美味
◎富含維生素、礦物質及抗氧化物，對健康有益
◎適合生食、燉湯、做沙拉或烤製
◎無農藥，天然栽培，符合有機標準
◎全程可追溯，確保品質與安全', 9, CAST(N'2024-12-23' AS Date), 2, N'宜蘭縣三星鄉行健溪一路二段161號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (2, N'有機白蘿蔔', N'根莖', 39, N'(每袋約200g)
新鮮胡蘿蔔，營養又健康', N'好山好水好鮮甜
當天採收出貨蘿蔔開採
纖細透亮的蘿蔔，一煮及嫩，香甜可口
管是醃製還是料理都是最佳選擇。', 10, CAST(N'2024-12-23' AS Date), 2, N'新北市金山區兩湖里倒照湖23號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (3, N'有機紅蘿蔔
', N'根莖', 39, N'(每袋約200g)
新鮮胡蘿蔔，營養又健康', N'好山好水好鮮甜
當天採收出貨蘿蔔開採
纖細透亮的蘿蔔，一煮及嫩，香甜可口
管是醃製還是料理都是最佳選擇。', 7, CAST(N'2024-12-23' AS Date), 2, N'桃園市楊梅區高榮里幼獅路一段439號', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (4, N'台灣馬鈴薯', N'根莖', 20, N'(250g~300g/顆) 
本產馬鈴薯
適合咖哩 沙拉 優良澱粉', N'以下是使用馬鈴薯的常見方式：
1.烹煮
2.烘烤
3.煎炸
4.蒸煮
5.泥餅或糕點', 55, CAST(N'2024-12-23' AS Date), 2, N'桃園市大溪區美華里美山路二段一巷261號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (5, N'台灣地瓜', N'根莖', 29, N'(250g~300g/顆) 
鮮吃地瓜', N'◎嚴選雲林產馬鈴薯,隨時品嚐鮮採滋味
◎製作過程無任何水、糖、鹽添加,口感綿密、薯香濃郁
◎無論是製作美味料理還是單純享受原味,方便又省時
◎ 鮮吃馬鈴薯。新鮮始於收成', 15, CAST(N'2024-12-23' AS Date), 2, N'新竹縣北埔鄉大湖村尾隘子7鄰20號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (6, N'有機洋蔥', N'根莖', 25, N'(每顆約150g)
鮮脆可口洋蔥', N'◎天然栽培，無添加
◎適合各種烹飪方式
◎味道香甜，提升料理風味
◎產地直送，鮮度無比', 1, CAST(N'2025-01-15' AS Date), 2, N'苗栗縣通霄鎮南和里166號', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (7, N'有機豆芽菜 ', N'其他', 30, N'(每份約150g)
適合炒菜水煮', N'◎鮮採鮮出，全程追溯
◎產銷履歷，品質保證
◎爽脆可口，清甜美味', 7, CAST(N'2025-01-10' AS Date), 2, N'台中市和平區武陵路3-1號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (8, N'有機青江菜', N'果菜', 30, N' (每袋約300g)
新鮮脆嫩，健康營養的青江菜', N'◎新鮮採摘，口感清脆，營養豐富
◎全程追溯，保證品質
◎適合炒食、湯品或燒菜
◎無農藥，天然有機栽培
◎富含維生素A與C，促進免疫力及皮膚健康', 5, CAST(N'2025-01-12' AS Date), 2, N'彰化縣彰化市石牌路一段428號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (9, N'BoPie菠菜 ', N'葉菜', 30, N'(每袋約250g)
綠色新鮮菠菜', N'◎鮮採鮮出，全程追溯
◎產銷履歷，品質保證
◎爽脆可口，清甜美味', 31, CAST(N'2025-01-18' AS Date), 2, N'南投縣仁愛鄉仁和路170號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (10, N'台灣有機萵苣', N'葉菜', 69, N'(每袋約200g)
清新爽脆，最佳涼拌食材', N'◎鮮採鮮出，全程追溯
◎產銷履歷，品質保證
◎爽脆可口，清甜美味', 21, CAST(N'2025-01-20' AS Date), 2, N'南投縣草屯鎮富頂路⼀段726巷99號', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (11, N'台灣香蕉', N'水果', 70, N'(600g±5% x1袋）
香蕉，天然無添加', N'◎ 每日宅鮮,天天吃到美味
◎口感豐盈北蕉,軟、香、甜、細緻
◎日本催熟技術AI科技確保品質一致
◎ 新鮮冷鏈,讓蔬果更美味', 5, CAST(N'2025-01-14' AS Date), 2, N'雲林縣崙背鄉東興路182之32號', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (12, N'美國富士進口蘋果
', N'水果', 599, N'(6粒入／1kg±5% x1袋)新鮮蘋果，甜脆可口', N'◎果色鮮豔，大小均勻，口感爽脆
◎口感爽脆，味道清甜
◎肉質鮮嫩，甜度適中
◎ 新鮮冷鏈，讓蔬果更美味', 3, CAST(N'2025-01-16' AS Date), 2, N'嘉義縣民雄鄉民新路369號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (13, N'有機牛番茄', N'根莖
', 50, N'(每顆約200g)
紅透透的番茄，香甜可口', N'◎果色鮮豔，大小均勻
◎口感爽脆，味道清甜
◎肉質鮮嫩，甜度適中
◎ 新鮮冷鏈，讓蔬果更美味', 5, CAST(N'2025-01-11' AS Date), 2, N'台南市大內區其子瓦60號', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (14, N'台灣在地柳丁 ', N'水果', 45, N'(每顆約200g)
甜美口感，輕鬆享受新鮮果香', N'◎每顆柳丁均選自優質農場，口感濃郁
◎天然風味，無農藥殘留
◎富含豐富的維生素C，營養滿分
◎適合直接食用、榨汁或加入甜點中
◎ 新鮮冷鏈，讓蔬果更美味', 7, CAST(N'2025-01-19' AS Date), 2, N'高雄市小港區明聖街135巷10弄12號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (15, N'酸酸台灣檸檬', N'水果', 35, N'(每顆約150g)
酸香濃郁，清爽檸檬', N'◎果肉多汁，酸度適中，天然香氣
◎富含維生素C，對皮膚及免疫系統有益
◎適合製作檸檬水、果汁、調味或烘焙使用
◎每顆檸檬均經過精選，保證品質
◎無農藥殘留，天然栽培，健康無憂', 9, CAST(N'2025-01-13' AS Date), 2, N'屏東縣恆春鎮恆公路1097-1號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (18, N'新鮮台灣酪梨', N'果菜', 95, N'(每顆約250g)
綿密滑順，營養滿分的酪梨', N'◎果肉細緻滑順，香氣濃郁
◎富含單元不飽和脂肪酸，有助於降低膽固醇
◎適合加入沙拉、三明治、奶昔或直接食用
◎天然有機栽培，無農藥，保證無添加
◎每顆酪梨均經過精選，確保品質與新鮮度', 15, CAST(N'2025-02-20' AS Date), 2, N'台北市大安區信義路三段129號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (20, N'新鮮台灣芒果 ', N'水果', 130, N'(每顆約350g)
甜美多汁的台灣芒果', N'◎果色鮮豔，大小均勻
◎口感爽脆，味道清甜
◎肉質鮮嫩，甜度適中
◎ 新鮮冷鏈，讓蔬果更美味', 23, CAST(N'2025-02-20' AS Date), 2, N'新北市板橋區文化路二段328巷18弄5號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (21, N'新鮮台灣櫛瓜', N'果菜', 50, N'(每顆約300g)
清新可口，健康營養的櫛瓜', N'◎果肉嫩滑，清爽可口，適合多種烹飪方式
◎富含維生素C與抗氧化物，增強免疫力
◎適合搭配炒菜、沙拉或製作蔬菜餅
◎天然無農藥栽培，保證無添加
◎每顆櫛瓜均經過精選，確保口感與品質', 5, CAST(N'2025-02-20' AS Date), 2, N'桃園市中壢區復興路87號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (22, N'履歷小黃瓜', N'果菜', 35, N'(每顆約250g)
脆嫩清爽，夏日必備的小黃瓜', N'◎果肉鮮嫩多汁，清爽可口
◎富含水分和維生素C，有助於保持肌膚水潤
◎適合生食、涼拌、製作沙拉或做泡菜
◎無農藥，天然栽培，保證無添加
◎每顆小黃瓜均經過精選，保證新鮮與口感', 14, CAST(N'2025-02-20' AS Date), 2, N'新竹市東區關新路58巷12號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (23, N'新鮮台灣茄子', N'果菜', 60, N'(每顆約350g)
綿密口感，適合多種烹調方式的茄子', N'◎果肉細緻，質地柔軟，口感綿密
◎富含纖維、維生素B群，促進消化健康
◎適合燒烤、炒菜、燉煮、做茄子煲等料理
◎無農藥栽培，天然栽培，健康無憂
◎每顆茄子均經過精選，確保最佳品質', 25, CAST(N'2025-02-20' AS Date), 2, N'台中市西屯區福科路360巷22號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (24, N'在地南瓜', N'根莖', 80, N'(每顆約1kg)
甜美可口，營養豐富的台灣南瓜', N'◎果肉紮實、甜美，口感細膩
◎富含β-胡蘿蔔素、維生素A，對眼睛與皮膚有益
◎適合製作南瓜湯、燉煮、蒸煮或烘焙
◎天然栽培，無農藥殘留，保證新鮮與健康
◎每顆南瓜均經過精選，確保最佳品質', 23, CAST(N'2025-02-20' AS Date), 2, N'彰化縣員林市大同路88巷16號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (25, N'甜口玉米', N'根莖', 40, N'(每根約300g)
甜美多汁，香氣撲鼻的台灣玉米', N'◎每根玉米粒飽滿，口感甜美
◎富含纖維、維生素B群，對消化系統有益
◎適合蒸煮、烤製、煮湯或作為小吃
◎無農藥殘留，天然栽培，保證新鮮與健康
◎每根玉米均經過精選，保證最佳品質', 6, CAST(N'2025-02-20' AS Date), 2, N'南投縣草屯鎮玉屏路150號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (26, N'脆綠花椰菜', N'葉菜', 80, N'(每顆約500g)
清脆多汁，營養豐富的台灣花椰菜', N'本產花椰菜
◎花朵細緻，色澤翠綠，口感清脆
◎富含維生素C和膳食纖維，有助於增強免疫力與促進消化
◎適合蒸煮、炒菜、做湯或拌沙拉
◎無農藥，天然栽培，保證無添加
◎每顆花椰菜均經過精選，保證新鮮與口感', 12, CAST(N'2025-02-20' AS Date), 2, N'雲林縣斗六市內環路58巷19弄8號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (27, N'有機甜椒', N'果菜', 55, N'(每袋約300g)
鮮嫩甜美，色彩繽紛的台灣甜椒', N'◎果肉厚實，口感甜美，富有天然風味
◎富含維生素C、β-胡蘿蔔素，對免疫系統有益
◎適合炒菜、燉煮、做沙拉或做醃製食物
◎無農藥，天然栽培，健康無憂
◎每顆甜椒均經過精選，保證新鮮口感', 44, CAST(N'2025-02-20' AS Date), 2, N'花蓮縣吉安鄉自強路100巷6號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (28, N'日本進口葡萄', N'水果', 300, N'(每包約500g)
甜美多汁，營養豐富的台灣葡萄', N'◎果實飽滿，口感甜美，汁液豐富
◎富含抗氧化物質，對心血管健康有益
◎可直接食用、做果汁、或用來做甜點
◎無農藥，天然栽培，保證新鮮健康
◎每包葡萄均經過精選，保證最佳品質', 5, CAST(N'2025-02-20' AS Date), 1, N'基隆市中正區義一路25號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (29, N'台灣苗栗草莓', N'水果', 120, N'(每盒約250g)
甜美多汁，營養滿點的台灣草莓', N'◎果實鮮紅，口感香甜，果肉多汁
◎富含維生素C和抗氧化劑，有助於提高免疫力
◎適合直接食用、製作果醬、甜點或加進沙拉中
◎無農藥，天然栽培，健康無憂
◎每盒草莓均經過精選，保證新鮮與口感', 7, CAST(N'2025-02-20' AS Date), 1, N'苗栗縣頭份市建國路215巷6弄3號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (30, N'新鮮進口藍莓', N'水果', 200, N'(每盒約150g)
口感酸甜，富含抗氧化劑的藍莓', N'◎果實小巧，酸甜可口，充滿天然風味
◎富含抗氧化劑和維生素C，有助於延緩衰老與提升免疫力
◎適合直接食用、加進酸奶、沙拉、或製作果醬
◎經過嚴格篩選，保證每顆藍莓新鮮美味
◎採用冷鏈運輸，保證水果鮮度與品質', 5, CAST(N'2025-02-20' AS Date), 1, N'新竹縣竹北市文興路二段168巷12弄3號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (31, N'新鮮進口西洋梨
', N'水果', 70, N'(每顆約300g)
甜美多汁，細膩口感的西洋梨', N'◎果肉細緻，甜美多汁，口感極為清爽
◎富含膳食纖維和維生素C，有助於消化與提升免疫力
◎適合直接食用、製作甜點或加入沙拉中
◎每顆西洋梨經過精選，保證新鮮與高品質
◎採用冷鏈運輸，確保水果鮮度與品質', 41, CAST(N'2025-02-20' AS Date), 1, N'台中市南屯區大墩路56巷20號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (32, N'新鮮進口櫻桃', N'水果', 350, N'(每盒約250g)
鮮美多汁，酸甜可口的進口櫻桃', N'◎果實鮮紅，酸甜可口，口感清新多汁
◎富含維生素C、鉀、抗氧化劑，有助於維持健康
◎適合直接食用、做沙拉、甜點或加入果汁
◎每顆櫻桃均經過精選，確保最佳品質
◎採用冷鏈運輸，保證水果新鮮與口感', 26, CAST(N'2025-02-20' AS Date), 1, N'嘉義市西區友愛路120巷8弄5號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (33, N'梨山超甜桃子', N'水果', 100, N'(每顆約350g)
甜美多汁，清香可口的進口桃子', N'◎果肉細膩，甜美多汁，香氣四溢
◎富含維生素C與膳食纖維，對免疫力與消化有益
◎適合直接食用、製作甜點、果汁或加入沙拉
◎每顆桃子均經過精選，保證最佳品質
◎採用冷鏈運輸，確保水果新鮮與口感', 4, CAST(N'2025-02-20' AS Date), 1, N'台南市北區開元路200巷15號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (34, N'新鮮杏胞菇', N'蕈菇', 90, N'(每包約200g)
口感鮮嫩，香氣十足的杏胞菇', N'◎菇體潔白，肉質嫩滑，口感清脆
◎富含蛋白質、維生素及膳食纖維，對健康有益
◎適合燒烤、炒菜、煮湯或做為料理配料
◎無農藥，天然栽培，保證新鮮無憂
◎每包杏胞菇均經過精選，確保最佳品質', 6, CAST(N'2025-02-20' AS Date), 1, N'高雄市鳳山區青年路三段88巷10弄6號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (35, N'新鮮洋菇', N'蕈菇', 70, N'(每包約200g)
 口感細膩，香氣誘人的洋菇', N'◎菇體白皙，口感柔嫩，風味濃郁
◎富含維生素D、膳食纖維，有助於骨骼與免疫健康
◎適合煮湯、炒菜、燉煮或用於各式料理
◎無農藥，天然栽培，保證新鮮健康
◎每包洋菇均經過精選，保證最佳品質', 10, CAST(N'2025-02-20' AS Date), 1, N'屏東縣潮州鎮新生路75號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (36, N'溫室金針菇', N'蕈菇', 50, N'(每袋約200g)
鮮嫩爽脆，天然栽培的金針菇', N'◎金黃色的菇體，質地脆嫩，口感清爽
◎富含蛋白質、膳食纖維及多種微量元素，對健康有益
◎適合涮火鍋、炒菜、燉湯或製作沙拉
◎無農藥，天然栽培，保證新鮮無憂
◎每包金針菇均經過精選，確保最佳品質', 12, CAST(N'2025-02-20' AS Date), 1, N'台東縣成功鎮中華路168巷3號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (37, N'有機黑木耳
', N'蕈菇', 50, N'(每袋約150g)
口感脆嫩，富含膳食纖維的黑木耳', N'◎口感脆嫩，富含膳食纖維
◎採收後即刻乾燥，保持最佳品質
◎適合用於涼拌、煮湯、炒菜或火鍋
◎無農藥殘留，天然栽培，品質保證
◎富含鐵質，有助於促進血液循環和增強免疫力', 15, CAST(N'2025-02-20' AS Date), 2, N'澎湖縣馬公市文澳街32號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (38, N'新鮮雪白菇
', N'蕈菇', 90, N'(每包約200g)
鮮嫩滑順，清香可口的雪白菇', N'◎雪白的菇帽，肉質細緻，口感爽脆
◎富含膳食纖維、維生素，幫助消化與維持健康
◎適合火鍋、湯品、炒菜或做涼拌
◎無農藥、無添加，純天然栽培
◎每包雪白菇均經過精選，確保新鮮與品質', 6, CAST(N'2025-02-20' AS Date), 1, N'花蓮縣吉安鄉南海六街22號', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (39, N'綜合蔬菜箱
', N'蔬菜箱', 699, N'(每箱約5kg)
健康又美味，綜合時令蔬菜箱', N'每箱包含精選多樣化的時令蔬菜，滿足各種料理需求
◎內含蔬菜如玉米、胡蘿蔔、茄子、花椰菜等，豐富您的餐桌
◎每一項蔬菜都是新鮮當季採摘，富含維生素和礦物質
◎無農藥、無添加，保證安全與品質
◎適合家中、餐廳或企業訂購，提供多樣化的蔬菜選擇', 2, CAST(N'2025-02-20' AS Date), 2, N'苗栗縣頭份市信義路36巷5弄9號', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[tProduct] OFF
GO



USE [dbVegetable]
GO
SET IDENTITY_INSERT [dbo].[tComment] ON 
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (3, 1, 3, 1, N'胡蘿蔔的顏色鮮艷，味道甜美，不僅可以生吃，也很適合做湯，健康又美味！', 2, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (5, 1, 10, 1, N'這個有機生菜真的很新鮮，口感清脆，吃起來非常爽口，搭配沙拉超好吃！', 3, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (6, 1, 13, 1, N'這顆番茄非常多汁，味道濃郁，吃一口就感覺像是直接從田裡摘下來的！', 5, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (7, 1, 9, 1, N'菠菜的葉子很大，口感綿密，煮湯或炒菜都很適合，還能提供豐富的鐵質！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (8, 1, 22, 1, N'這些小黃瓜很嫩，咬下去一口滿滿的清香，當作小零嘴或加到沙拉中都超好吃！', 3, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (9, 1, 27, 1, N'這些甜椒顏色鮮豔，吃起來非常甜美，口感也很脆，炒菜時增加了不少風味！', 2, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (10, 1, 4, 1, N'土豆煮熟後非常軟綿，做成薯泥或是炸薯條，無論哪種做法都非常美味！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (13, 2, 1, 1, N'這顆甜菜根非常甜美，煮熟後顏色鮮豔，口感滑嫩，當作沙拉配料或是煮湯都非常美味！', 5, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (14, 3, 1, 1, N'甜菜根煮熟後的味道相當獨特，甜中帶有土壤的香氣，是我最喜歡的蔬菜之一！', 3, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (15, 4, 1, 1, N'這顆甜菜根的味道非常甜，無論是做成湯還是烤製，都能保留它的天然風味，吃了之後感覺很有營養！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (16, 5, 1, 1, N'我喜歡把甜菜根切成薄片生吃，它的甜味和清脆口感非常清爽，是健康的零食選擇！', 3, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (17, 6, 1, 1, N'甜菜根真的很好吃！炒菜或者用來做醬料，它的自然甜味讓整道菜都更加豐富，還有助於排毒，真的是健康又美味！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (18, 2, 2, 1, N'這根白蘿蔔非常新鮮，口感清脆，無論是生吃還是做湯，都非常爽口，特別適合冬天食用！', 5, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (20, 3, 2, 1, N'白蘿蔔的味道清淡又帶點甜味，用來煮湯或是炖肉都能增添鮮香，非常美味！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (21, 4, 2, 1, N'這顆白蘿蔔的肉質細緻，口感不會太辛辣，做成泡菜或者燉湯，味道都非常棒！', 3, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (22, 5, 2, 1, N'白蘿蔔真的超級多用途，炒菜、做湯、還可以拌沙拉，搭配不同食材都有不一樣的美味！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (23, 6, 2, 1, N'我把白蘿蔔做成了醃製小菜，清爽的味道搭配微微的辣感，真的是一種極品開胃菜！', 5, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (24, 1, 3, 1, N'這根紅蘿蔔顏色鮮豔，甜味十足，無論是生吃還是煮湯，都能為料理增添自然的甜味，口感也非常脆嫩！', 3, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (26, 2, 3, 1, N'這顆紅蘿蔔非常鮮嫩，營養豐富，做成沙拉或是炒菜都能帶來清新口感，非常健康又美味！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (27, 3, 3, 1, N'紅蘿蔔不僅味道甘甜，還富含維生素A，無論是做成湯還是燉菜，總是讓整道菜更有層次！', 5, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (28, 4, 3, 1, N'這些紅蘿蔔的質地非常好，切開來後肉質細緻，吃起來有一種自然的甜味，非常適合做成料理或當作零食！', 5, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (29, 5, 3, 1, N'紅蘿蔔炒肉或是煮湯都非常適合，甜美的口感能平衡整道菜的味道，簡單卻很美味！', 4, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (30, 6, 1, 1, N'甜菜根煮熟後的味道相當獨特，甜中帶有土壤的香氣，是我最喜歡的蔬菜之一！', 3, CAST(N'2025-02-21' AS Date))
GO
INSERT [dbo].[tComment] ([fId], [fPersonId], [fProductId], [fOrderListId], [fComment], [fStar], [fCreatedAt]) VALUES (31, 1, 1, 1, N'好吃好吃甜菜根', 5, CAST(N'2025-02-21' AS Date))
GO
SET IDENTITY_INSERT [dbo].[tComment] OFF
GO
SET IDENTITY_INSERT [dbo].[tOrder] ON 
GO
INSERT [dbo].[tOrder] ([fId], [fPersonId], [fStatus], [fPay], [fOrderAt], [fTotal], [fAddress], [fReceiverName], [fPhone], [fNote], [fMerchantTradeNo]) VALUES (1, 1, 2, 1, CAST(N'2025-02-21T01:13:41.267' AS DateTime), 1099, N'1', N'1', N'1', N'1', N'1')
GO
INSERT [dbo].[tOrder] ([fId], [fPersonId], [fStatus], [fPay], [fOrderAt], [fTotal], [fAddress], [fReceiverName], [fPhone], [fNote], [fMerchantTradeNo]) VALUES (2, 1, 1, 1, CAST(N'2025-02-21T01:14:23.010' AS DateTime), 100, N'1', N'1', N'1', N'', N'')
GO
SET IDENTITY_INSERT [dbo].[tOrder] OFF
GO
SET IDENTITY_INSERT [dbo].[tOrderList] ON 
GO
INSERT [dbo].[tOrderList] ([fId], [fOrderId], [fProductId], [fPrice], [fCount], [fSum]) VALUES (1, 1, 1, 50, 10, 500)
GO
INSERT [dbo].[tOrderList] ([fId], [fOrderId], [fProductId], [fPrice], [fCount], [fSum]) VALUES (2, 1, 3, 10, 10, 100)
GO
INSERT [dbo].[tOrderList] ([fId], [fOrderId], [fProductId], [fPrice], [fCount], [fSum]) VALUES (3, 1, 2, 49, 10, 499)
GO
INSERT [dbo].[tOrderList] ([fId], [fOrderId], [fProductId], [fPrice], [fCount], [fSum]) VALUES (4, 2, 4, 10, 10, 100)
GO
SET IDENTITY_INSERT [dbo].[tOrderList] OFF
GO

























