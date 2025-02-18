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
fId INT NOT NULL IDENTITY(1, 1) PRIMARY KEY, -- �t�ӽs���A�۰ʻ��W 
fName NVARCHAR(100) NOT NULL DEFAULT'', -- �t�ӦW�� 
fUbn NVARCHAR(10) NOT NULL DEFAULT'', -- �t�Ӳνs 
fTel NVARCHAR(30) NOT NULL DEFAULT'', -- �t�ӹq�ܡA�w�]���Ŧr�� 
fConnect NVARCHAR(50) NOT NULL DEFAULT'', -- �t���p���H�A�w�]���Ŧr�� 
fAccountant NVARCHAR(50) NOT NULL DEFAULT'', -- �t�ӷ|�p�A�w�]���Ŧr�� 
fAddress NVARCHAR(100) NOT NULL DEFAULT'', -- �t�Ӥ��q�a�}�A�w�]���Ŧr�� 
fDelivery NVARCHAR(100) NOT NULL DEFAULT'', -- �t�Ӱe�f�a�}�A�w�]���Ŧr�� 
fInvoiceAdd NVARCHAR(100) NOT NULL DEFAULT'', -- �t�ӵo���a�}�A�w�]���Ŧr�� 
fEditor INT NOT NULL      -- �ק�H�A�P tPerson ��檺 fid ����
); 

GO
    CREATE TABLE tProduct(
fId INT NOT NULL PRIMARY Key identity (1,1) ,
fName Nvarchar(500) NOT NULL DEFAULT'',
fClassification Nvarchar(100) NOT NULL DEFAULT'',
fPrice INT NOT NULL DEFAULT 1,
fDescription Nvarchar(MAX) NOT NULL DEFAULT'',
fIntroduction NVARCHAR(MAX) NOT NULL DEFAULT '',--0205�[�s���
fQuantity INT NOT NULL DEFAULT 0,
fLaunchAt DATE NOT NULL DEFAULT GETDATE(),  -- 0117toilet�n��
fStorage int not null DEFAULT 0,
fOrigin Nvarchar(100) NOT NULL DEFAULT'',
fLaunch INT NOT NULL DEFAULT 0,
fEditor INT NOT NULL
    );
GO

CREATE TABLE tImg ( 
fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �ӫ~Id�A�D��A���ର�� 
fProductId INT NOT NULL, -- �ӫ~Id�A���ର�� 
fName NVARCHAR(500) NOT NULL DEFAULT'', -- �Ӥ��W�١A���ର�šA�w�]�Ȭ��Ŧr�� 
fOrderBy INT NOT NULL DEFAULT 1, -- �Ӥ��ƧǡA���ର�šA�w�]�Ȭ��Ŧr�� 
fUploadAt DATE NOT NULL DEFAULT GETDATE(), -- �W�Ǥ���A�w�]����e����ɶ�  

fEditor INT NOT NULL     -- �ק�H�A�ϥ�INT��ܽs��H����ID
); 
GO

CREATE TABLE tFavorite (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),   -- �۰ʻ��W���D��
    fProductId INT NOT NULL,              -- �ӫ~Id
    fPersonId INT NOT NULL,                 -- �H��Id
   );
GO



CREATE TABLE tInventoryMain (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fBaselineDate  DATE NOT NULL, -- �L�I��Ǥ�
    fCreatedAt DATE NOT NULL, -- �L�I��إߤ�� 
    fEditor INT NOT NULL, -- �s���
    fNote NVARCHAR(500) -- �Ƶ�
);
GO
CREATE TABLE tInventoryDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fInventoryMainId INT NOT NULL, -- �s���� tInventoryMain
    fProductId INT NOT NULL, -- �s���� tProduct ���ӫ~ ID
    fSystemQuantity INT, -- �t�ήw�s
    fActualQuantity INT -- ��ڮw�s
);
GO
CREATE TABLE tInventoryAdjustment (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fadjustmentDate DATE NOT NULL, -- �w�s�վ��
    fCreatedAt DATE NOT NULL, -- �L�I��إߤ�� 
    fEditor INT NOT NULL, -- �s���
    fNote NVARCHAR(500) NOT NULL DEFAULT '', -- �Ƶ�
    fCheckerId INT -- �L�I�H���� ID
);
GO
CREATE TABLE tInventoryAdjustmentDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fInventoryAdjustmentId INT NOT NULL, -- �s���� tInventoryAdjustment
    fProductId INT NOT NULL , -- �s���� tProduct ���ӫ~ ID
    fQuantity INT NOT NULL DEFAULT 0, -- �ƶq
    fCost DECIMAL(18,2) NOT NULL DEFAULT 0.00 -- �O���L�I�ɪ�����
);

GO
   CREATE TABLE tInvoice (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),         -- �o�� ID�A�D��A�ۼW
    fNumber NVARCHAR(50) NOT NULL,             -- �o�����X
    fDate datetime NOT NULL,                       -- �o�����
    fForm NVARCHAR(20) NOT NULL DEFAULT '',    -- �o���榡�A�w�]���Ŧr��
    fCustomerId INT NOT NULL,                  -- �Ȥ� ID
    fCustomerUbn NVARCHAR(50)  DEFAULT '', -- �Ȥ�νs�A�w�]���Ŧr��
    fProviderId INT NOT NULL,                  -- ������ ID
    fProviderUbn NVARCHAR(50) NOT NULL DEFAULT '', -- �����Ӳνs�A�w�]���Ŧr��
    fInOut INT NOT NULL DEFAULT 0,             -- ���������A�w�]�� 0
    fStatus INT NOT NULL DEFAULT 0,            -- ���A(�@�o�P�_)�A�w�]�� 0
    fTotal INT NOT NULL DEFAULT 0,             -- �`���B�A�w�]�� 0
    fEditor INT NOT NULL DEFAULT 0             -- �ק�H�A�w�]�� 0
);
GO

CREATE TABLE tInvoiceDetail (
    fId INT NOT NULL  PRIMARY KEY IDENTITY(1,1),         -- �o������ ID�A�D��A�ۼW
    fNumber NVARCHAR(50) NOT NULL,             -- �o�����X
    fProductName NVARCHAR(50) NOT NULL ,        -- �ӫ~�W��
    fCount INT NOT NULL DEFAULT 1,             -- �ƶq�A�w�]�� 1
    fPrice INT NOT NULL DEFAULT 0,             -- ����A�w�]�� 0
    fSum INT NOT NULL DEFAULT 0                -- �p�p�A�w�]�� 0
);
GO
CREATE TABLE tPurchase ( 
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),         -- ���ʳ� ID�A�D��A�ۼW
    fBuyDate DATETIME NOT NULL DEFAULT GETDATE(),       -- ���ʤ���A�w�]����e���
    fProviderId INT NOT NULL ,                           -- ������ ID
    fInvoiceForm INT NOT NULL DEFAULT 0,                -- �o���榡�A�w�]�� 0
    fPayment INT NOT NULL DEFAULT 0,                    -- ��I�覡�A�w�]�� 0
    fEditor INT NOT NULL DEFAULT 0,            -- �ק�H�A�w�]�� 0
    fPreTax INT NOT NULL DEFAULT 0,            -- �|�e���B�A�w�]�� 0
    fTax INT NOT NULL DEFAULT 0,               -- �|�B�A�w�]�� 0
    fTotal INT NOT NULL DEFAULT 0,             -- �`���B�A�w�]�� 0
    fNote NVARCHAR(500) NOT NULL DEFAULT ''             -- �Ƶ��A�w�]���Ŧr��
);
GO
CREATE TABLE tPurchaseDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),         -- ���� ID�A�D��A�ۼW
    fPurchaseId INT NOT NULL  ,                  -- ���ʳ渹�A������ `tPurchase` �� `fId`
    fProductId INT NOT NULL  ,                   -- ���~ ID
    fCount INT NOT NULL DEFAULT 1,             -- �ƶq�A�w�]�� 1
    fPrice INT NOT NULL DEFAULT 0,             -- ����A�w�]�� 0
    fSum INT NOT NULL DEFAULT 0                -- �p�p�A�w�]�� 0
);
GO



CREATE TABLE tGoodsInAndOut (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �i�X�f��ID�A�D��A�ۼW�A�q1�}�l
    fInOut INT NOT NULL DEFAULT 0,                                  -- �i�f�ΥX�f
    fDate DATE NOT NULL DEFAULT GETDATE(),                              -- �i�X�f���
    fInvoiceId INT NOT NULL,                              -- �o��ID
    fProviderId INT NOT NULL DEFAULT 0,                 -- ������ID
    fPersonId INT NOT NULL DEFAULT 0,                   -- �Ȥ�ID
    fTotal INT NOT NULL DEFAULT 0,                                  -- �i�X�f�`�B�]�t�|�^
    fEditor INT NOT NULL,                                 -- �ק�H
    fNote NVARCHAR(500) NOT NULL DEFAULT ''      -- �Ƶ��A�̤j����500�r��
 );   
GO
 CREATE TABLE tGoodsInAndOutDetail ( 
fId INT NOT NULL PRIMARY KEY IDENTITY(1, 1), -- �۰ʼW�����D�� 
fGoodsInandOutId INT NOT NULL, -- �����\���Ū���� 
fProductId INT NOT NULL , -- �����\���Ū���� 
fCount INT NOT NULL DEFAULT 0, -- �����\���Ū����
fPrice INT NOT NULL DEFAULT 0, -- �����\���Ū����
fSum INT NOT NULL DEFAULT 0  -- �����\���Ū� fSum ���

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
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),     -- ���ڳ渹�A�D��A�ۼW�A�q1�}�l
    fDate DATE NOT NULL DEFAULT GETDATE(),   -- ���ڤ��
    fPersonId INT NOT NULL,                      -- �Ȥ�ID�A���ର��
    fReceiptMethod INT NOT NULL DEFAULT 1,                 -- ���ڤ覡�A�w�]��1
    fTotal INT NOT NULL DEFAULT 0,                                  -- �`���B
    fEditor INT NOT NULL,                         -- ���ɺ[�ק�H���A���ର��
    fNote NVARCHAR(500) NOT NULL DEFAULT ''     -- �Ƶ��A���ର�šA�w�]���Ŧr��
);      
GO
CREATE TABLE tReceiptReversalDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),          -- ����ID�A�D��A�ۼW�A�q1�}�l
    fReceiptReversalId INT NOT NULL,             -- ���ڳ渹ID�A���ର��
    fOrderId INT NOT NULL      -- �q��ID�A���ର��
);                        
GO
CREATE TABLE tPaymentReversal (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),    -- �I�ڳ渹�A�D��A�ۼW�A�q1�}�l
    fDate DATE NOT NULL DEFAULT GETDATE(),                         -- �I�ڤ���A���ର��
    fProviderId INT,                             -- �t��ID�A�i�H����
    fPaymentMethod INT NOT NULL DEFAULT 0,       -- �I�ڤ覡�A�w�]��0�A���ର��
    fTotal INT NOT NULL DEFAULT 0,                                  -- �`���B
    fEditor INT NOT NULL,                                 -- ���ɺ[�ק�H��
    fNote NVARCHAR(500) NOT NULL DEFAULT '' -- �Ƶ��A�w�]���Ŧr��A�̤j����500�r��
 );            
GO
CREATE TABLE tPaymentReversalDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),          -- �D��A�I�ڨR�b�Ӷ�ID�A�ۼW
    fPaymentReversalId INT NOT NULL,             -- �I�ڳ渹ID�A���ର��
    fGoodsInAndOutId INT NOT NULL                -- �i�f��ID�A���ର��
 );              
GO
CREATE TABLE tAboutUs (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- �۰ʻ��W���D��
    fTitle NVARCHAR(200) NOT NULL DEFAULT '',           -- ���D���
    fContent NVARCHAR(MAX) NOT NULL DEFAULT '',         -- ���e���
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- �O���Ыخɶ�
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- �O����s�ɶ�
);
GO
CREATE TABLE tFAQ (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- �۰ʻ��W���D��
    fQuestion NVARCHAR(500) NOT NULL DEFAULT '',        -- ���D���
    fAnswer NVARCHAR(MAX) NOT NULL DEFAULT '',          -- �������
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- �O���Ыخɶ�
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- �O����s�ɶ�
);
GO
CREATE TABLE tReport(
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      
    fPersonId INT NOT NULL,        
    fClass NVARCHAR(10) NOT NULL DEFAULT '��L',   
    fTitle NVARCHAR(100) NOT NULL DEFAULT '',
    fDescription NVARCHAR(MAX) NOT NULL DEFAULT '',      
    fTime DATETIME NOT NULL DEFAULT GETDATE() 
);

USE [dbVegetable]
GO
SET IDENTITY_INSERT [dbo].[tProduct] ON 
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (1, N'�j�յ�', N'����', 100, N'�p�յ�', N'
��椺�e�p�U�G
�i4�خڲ���+2�ؿ���+6�ظ���(���e�H��u����ո˥X�f)�j
a.�ڲ��ʪG��³��:�p���ʡB�ɦ̵��B�Ө��B���ߵf�X�B�ͥʡB���ɦ̡B�z�̴ԡB����ڡB���R��B����B���ʡB���ڽ��B�j���ʡB�n�ʡB�X�l�B�H�x�s�ġB���ڽ��B���a���B�a�ʡC
b.����:�¤�աB����ۣ�B�E��ۣ�B�q��ۣ�B���jۣ�B�պ��Fۣ�B�ȦաB����ۣ�C
c.�u��������:�j��A��B�ڽ�A��B�p�յ�B�p�Q��B�¸��յ�B�C����B�d�_��B��ۣ��B�sӦ�U�B�Ťߵ�B�a�ʸ��B���A��C', 1, CAST(N'2024-12-23' AS Date), 2, N'�̪F', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (2, N'�Ե�', N'����', 150, N'���s�A�Ե�', N'
��椺�e�p�U�G
�i4�خڲ���+2�ؿ���+6�ظ���(���e�H��u����ո˥X�f)�j
a.�ڲ��ʪG��³��:�p���ʡB�ɦ̵��B�Ө��B���ߵf�X�B�ͥʡB���ɦ̡B�z�̴ԡB����ڡB���R��B����B���ʡB���ڽ��B�j���ʡB�n�ʡB�X�l�B�H�x�s�ġB���ڽ��B���a���B�a�ʡC
b.����:�¤�աB����ۣ�B�E��ۣ�B�q��ۣ�B���jۣ�B�պ��Fۣ�B�ȦաB����ۣ�C
c.�u��������:�j��A��B�ڽ�A��B�p�յ�B�p�Q��B�¸��յ�B�C����B�d�_��B��ۣ��B�sӦ�U�B�Ťߵ�B�a�ʸ��B���A��C', 1, CAST(N'2024-12-23' AS Date), 1, N'�x�n', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (3, N'�ᷦ��', N'����', 200, N'�@���@�������ᷦ��', N'
��椺�e�p�U�G
�i4�خڲ���+2�ؿ���+6�ظ���(���e�H��u����ո˥X�f)�j
a.�ڲ��ʪG��³��:�p���ʡB�ɦ̵��B�Ө��B���ߵf�X�B�ͥʡB���ɦ̡B�z�̴ԡB����ڡB���R��B����B���ʡB���ڽ��B�j���ʡB�n�ʡB�X�l�B�H�x�s�ġB���ڽ��B���a���B�a�ʡC
b.����:�¤�աB����ۣ�B�E��ۣ�B�q��ۣ�B���jۣ�B�պ��Fۣ�B�ȦաB����ۣ�C
c.�u��������:�j��A��B�ڽ�A��B�p�յ�B�p�Q��B�¸��յ�B�C����B�d�_��B��ۣ��B�sӦ�U�B�Ťߵ�B�a�ʸ��B���A��C', 2, CAST(N'2024-12-23' AS Date), 1, N'����', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (4, N'�v��', N'�ڲ�', 50, N'�|�����v��', N'
��椺�e�p�U�G
�i4�خڲ���+2�ؿ���+6�ظ���(���e�H��u����ո˥X�f)�j
a.�ڲ��ʪG��³��:�p���ʡB�ɦ̵��B�Ө��B���ߵf�X�B�ͥʡB���ɦ̡B�z�̴ԡB����ڡB���R��B����B���ʡB���ڽ��B�j���ʡB�n�ʡB�X�l�B�H�x�s�ġB���ڽ��B���a���B�a�ʡC
b.����:�¤�աB����ۣ�B�E��ۣ�B�q��ۣ�B���jۣ�B�պ��Fۣ�B�ȦաB����ۣ�C
c.�u��������:�j��A��B�ڽ�A��B�p�յ�B�p�Q��B�¸��յ�B�C����B�d�_��B��ۣ��B�sӦ�U�B�Ťߵ�B�a�ʸ��B���A��C', 0, CAST(N'2024-12-23' AS Date), 1, N'�̪F', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (5, N'���ڽ�', N'�ڲ�', 100, N'�K��K����ڽ�', N' ', 10, CAST(N'2024-12-23' AS Date), 1, N'�̪F', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (6, N'�J�ڽ�', N'�ڲ�', 90, N'�s�A�J�ڽ��A��i�S���d', N' ', 50, CAST(N'2025-01-15' AS Date), 2, N'�x��', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (7, N'�f�X', N'�G��', 80, N'���z�z���f�X�A�����i�f', N'', 30, CAST(N'2025-01-10' AS Date), 3, N'����', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (8, N'�C��', N'�G��', 60, N'�A�઺�C�ԡA�A�X����', N'', 20, CAST(N'2025-01-12' AS Date), 1, N'�x�n', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (9, N'�ɦ�', N'�G��', 120, N'�������ɦ̡A��i����', N'', 40, CAST(N'2025-01-18' AS Date), 4, N'�̪F', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (10, N'�X�l', N'�G��', 75, N'����X�l�A�A���i�f', N'', 60, CAST(N'2025-01-20' AS Date), 2, N'�n��', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (11, N'ī�G', N'���G', 150, N'�s�Aī�G�A���ܥi�f', N'', 100, CAST(N'2025-01-14' AS Date), 1, N'�Ὤ', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (12, N'����', N'���G', 90, N'�����A�ѵM�L�K�[', N'', 80, CAST(N'2025-01-16' AS Date), 3, N'�x��', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (13, N'����', N'�G��', 200, N'�s�A����A�G���|��', N'', 150, CAST(N'2025-01-11' AS Date), 2, N'�s��', 1, 2)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (14, N'���Y', N'�ڲ�', 110, N'���@���Y�A�f�P�ӿ�', N'', 50, CAST(N'2025-01-19' AS Date), 4, N'�x�_', 1, 1)
GO
INSERT [dbo].[tProduct] ([fId], [fName], [fClassification], [fPrice], [fDescription], [fIntroduction], [fQuantity], [fLaunchAt], [fStorage], [fOrigin], [fLaunch], [fEditor]) VALUES (15, N'��ۣ', N'ۣ��', 140, N'����Q������ۣ�A�A�X�N��', N'', 70, CAST(N'2025-01-13' AS Date), 3, N'�y��', 1, 1)
GO
SET IDENTITY_INSERT [dbo].[tProduct] OFF
GO






USE [dbVegetable]
GO
SET IDENTITY_INSERT [dbo].[tImg] ON 
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (1, 1, N'�j�յ�01.png', 1, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (3, 1, N'�j�յ�02.png', 2, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (4, 1, N'�j�յ�03.png', 3, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (5, 5, N'���ڽ�01.jpg', 1, CAST(N'2025-02-03' AS Date), 1)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (6, 2, N'�i��01.png', 1, CAST(N'2025-02-03' AS Date), 2)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (7, 3, N'�ᷦ��02.jpg', 1, CAST(N'2025-02-03' AS Date), 2)
GO
INSERT [dbo].[tImg] ([fId], [fProductId], [fName], [fOrderBy], [fUploadAt], [fEditor]) VALUES (8, 4, N'�v��01.jpg', 1, CAST(N'2025-02-03' AS Date), 1)
GO
SET IDENTITY_INSERT [dbo].[tImg] OFF
GO



---------------------------------------�X---------------------------------------�X---------------------------------------


INSERT INTO [dbo].[tCart] ([fProductId],[fCount],[fPersonId]) VALUES(1,20,1),(2,20,1),(1,10,2),(2,10,2) 
GO

-- ���J tInventoryMain ���
INSERT INTO tInventoryMain (fBaselineDate, fCreatedAt, fEditor, fNote)
VALUES 
('2025-02-01', '2025-02-02', 1, '�Ĥ@���L�I'),
('2025-02-03', '2025-02-04', 2, '�ĤG���L�I'),
('2025-02-05', '2025-02-06', 3, '�ĤT���L�I');

-- ���J tInventoryDetail ���
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

-- ���J tInventoryAdjustment ���
INSERT INTO tInventoryAdjustment (fadjustmentDate, fCreatedAt, fEditor, fNote, fCheckerId)
VALUES 
('2025-02-07', '2025-02-07', 1, '�Ĥ@���վ�', 5),
('2025-02-08', '2025-02-08', 2, '�ĤG���վ�', 6),
('2025-02-09', '2025-02-09', 3, '�ĤT���վ�', 7);

-- ���J tInventoryAdjustmentDetail ���
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
VALUES ('Dwayne Strong', '123', '123', '�k', '1990-06-08', '0979874614', '079599366', 'Unit 6832 Box 4261\nDPO AE 11131', 'bradley56@roth.com', '33448366', 1, 'William Stephens', '0962975478', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Brian Cole', '456', '456', '�k', '1979-01-07', '0969038974', '071316228', '572 Dixon Cliffs\nNew Eileen, AZ 39214', 'paula82@gmail.com', '62276379', 0, 'Amy Johnson', '0995817086', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Penny Hernandez', '789', '789', '�k', '1968-09-19', '0964016893', '076332359', '8634 Alicia Radial Apt. 606\nWest Larry, IA 24761', 'derrickjohnson@hotmail.com', '67066326', 1, 'Leah Scott', '0949105231', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Tonya Harrington', '147', '147', '�k', '1964-02-27', '0993996385', '071691538', '3599 Sandra Station\nPort Tammie, HI 47783', 'kathryncarpenter@vasquez-lawrence.com', '29663392', 0, 'Maria Good', '0935070180', 1);
GO
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Deborah Randall', '258', '258', '�k', '1965-06-02', '0939263973', '076542174', '258 Gabriella Squares\nSouth Anthonyton, NM 84414', 'carmen05@potts.com', '57128249', 1, 'Trevor Ward', '0919014552', 1);
GO

-- ���J tInvoice ��ơ]10 ���^
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

-- ���J tInvoiceDetail ��ơ]20 ���^
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

-- ���J tPurchase ��ơ]10 ���^
INSERT INTO tPurchase (fBuyDate, fProviderId, fInvoiceForm, fPayment, fEditor, fPreTax, fTax, fTotal, fNote)
VALUES
('2025-03-01', 301, 0, 0, 1, 900, 90, 990, '����1'),
('2025-02-02', 302, 1, 1, 2, 1500, 150, 1650, '����2'),
('2025-04-03', 303, 0, 0, 3, 2000, 200, 2200, '����3'),
('2025-05-04', 304, 1, 1, 4, 500, 50, 550, '����4'),
('2025-02-05', 305, 0, 0, 5, 3000, 300, 3300, '����5'),
('2025-06-06', 306, 1, 1, 6, 1200, 120, 1320, '����6'),
('2025-07-07', 307, 0, 0, 7, 900, 90, 990, '����7'),
('2025-12-08', 308, 1, 1, 8, 1800, 180, 1980, '����8'),
('2025-10-09', 309, 0, 0, 9, 2500, 250, 2750, '����9'),
('2025-08-10', 310, 1, 1, 10, 1600, 160, 1760, '����10');

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

