--drop database [dbVegetable]
CREATE DATABASE [dbVegetable]
GO
USE dbVegetable

GO
CREATE TABLE tPerson(
fId INT NOT NULL Primary Key identity (1,1) ,
fName Nvarchar(500) NOT NULL DEFAULT '',
fAccount Nvarchar(500) NOT NULL DEFAULT '',
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
fQuantity INT NOT NULL DEFAULT 0,
fLaunchAt DATE NOT NULL DEFAULT GETDATE(),  -- 0117toilet�n��
fStorage int not null DEFAULT 0,
fOrigin Nvarchar(100) NOT NULL DEFAULT'',
fLaunch INT NOT NULL DEFAULT 0,
fEditor INT NOT NULL
    );


CREATE TABLE tImg ( 
fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �ӫ~Id�A�D��A���ର�� 
fProductId INT NOT NULL, -- �ӫ~Id�A���ର�� 
fName NVARCHAR(500) NOT NULL DEFAULT'', -- �Ӥ��W�١A���ର�šA�w�]�Ȭ��Ŧr�� 
fOrderBy INT NOT NULL DEFAULT 1, -- �Ӥ��ƧǡA���ର�šA�w�]�Ȭ��Ŧr�� 
fUploadAt DATE NOT NULL DEFAULT GETDATE(), -- �W�Ǥ���A�w�]����e����ɶ�   -- 0117toilet�n��

fEditor INT NOT NULL     -- �ק�H�A�ϥ�INT��ܽs��H����ID
); 

CREATE TABLE tInventoryMain (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fBaselineDate  DATE NOT NULL, -- �L�I��Ǥ�
    fCreatedAt DATE NOT NULL, -- �L�I��إߤ�� 
    fEditor INT NOT NULL, -- �s���
    fNote NVARCHAR(500) -- �Ƶ�
);

CREATE TABLE tInventoryDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fInventoryMainId INT NOT NULL, -- �s���� tInventoryMain
    fProductId INT NOT NULL, -- �s���� tProduct ���ӫ~ ID
    fSystemQuantity INT, -- �t�ήw�s
    fActualQuantity INT -- ��ڮw�s
);

CREATE TABLE tInventoryAdjustment (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fadjustmentDate DATE NOT NULL, -- �w�s�վ��
    fCreatedAt DATE NOT NULL, -- �L�I��إߤ�� 
    fEditor INT NOT NULL, -- �s���
    fNote NVARCHAR(500) NOT NULL DEFAULT '', -- �Ƶ�
    fCheckerId INT -- �L�I�H���� ID
);

CREATE TABLE tInventoryAdjustmentDetail (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1), -- �D��
    fInventoryAdjustmentId INT NOT NULL, -- �s���� tInventoryAdjustment
    fProductId INT NOT NULL, -- �s���� tProduct ���ӫ~ ID
    fQuantity INT NOT NULL DEFAULT 0, -- �ƶq
    fCost DECIMAL(18,2) NOT NULL DEFAULT 0.00 -- �O���L�I�ɪ�����
);

GO
    CREATE TABLE tInvoice (
    fId INT  PRIMARY KEY IDENTITY(1,1),         -- �o�� ID�A�D��A�ۼW
    fNumber NVARCHAR(50) ,             -- �o�����X
    fDate datetime ,                       -- �o�����
    fForm NVARCHAR(20)  DEFAULT '',    -- �o���榡�A�w�]���Ŧr��
    fCustomerId INT ,                  -- �Ȥ� ID
    fCustomerUbn NVARCHAR(50)  DEFAULT '', -- �Ȥ�νs�A�w�]���Ŧr��
    fProviderId INT ,                  -- ������ ID
    fProviderUbn NVARCHAR(50) NOT NULL DEFAULT '', -- �����Ӳνs�A�w�]���Ŧr��
    fInOut INT  DEFAULT 0,             -- ���������A�w�]�� 0
    fStatus INT  DEFAULT 0,            -- ���A(�@�o�P�_)�A�w�]�� 0
    fTotal INT  DEFAULT 0,             -- �`���B�A�w�]�� 0
    fEditor INT  DEFAULT 0             -- �ק�H�A�w�]�� 0
);
GO

CREATE TABLE tInvoiceDetail (
    fId INT  PRIMARY KEY IDENTITY(1,1),         -- �o������ ID�A�D��A�ۼW
    fNumber NVARCHAR(50) ,             -- �o�����X
    fProductName NVARCHAR(50) ,        -- �ӫ~�W��
    fCount INT DEFAULT 1,             -- �ƶq�A�w�]�� 1
    fPrice INT  DEFAULT 0,             -- ����A�w�]�� 0
    fSum INT  DEFAULT 0                -- �p�p�A�w�]�� 0
);

CREATE TABLE tPurchase ( 
    fId INT PRIMARY KEY IDENTITY(1,1),         -- ���ʳ� ID�A�D��A�ۼW
    fBuyDate DATETIME  DEFAULT GETDATE(),       -- ���ʤ���A�w�]����e���
    fProviderId INT ,                           -- ������ ID
    fInvoiceForm INT DEFAULT 0,                -- �o���榡�A�w�]�� 0
    fPayment INT  DEFAULT 0,                    -- ��I�覡�A�w�]�� 0
    fEditor INT  DEFAULT 0,            -- �ק�H�A�w�]�� 0
    fPreTax INT  DEFAULT 0,            -- �|�e���B�A�w�]�� 0
    fTax INT  DEFAULT 0,               -- �|�B�A�w�]�� 0
    fTotal INT  DEFAULT 0,             -- �`���B�A�w�]�� 0
    fNote NVARCHAR(500)  DEFAULT ''             -- �Ƶ��A�w�]���Ŧr��
);

CREATE TABLE tPurchaseDetail (
    fId INT  PRIMARY KEY IDENTITY(1,1),         -- ���� ID�A�D��A�ۼW
    fPurchaseId INT ,                  -- ���ʳ渹�A������ `tPurchase` �� `fId`
    fProductId INT ,                   -- ���~ ID
    fCount INT  DEFAULT 1,             -- �ƶq�A�w�]�� 1
    fPrice INT  DEFAULT 0,             -- ����A�w�]�� 0
    fSum INT  DEFAULT 0                -- �p�p�A�w�]�� 0
);




CREATE TABLE tGoodsInAndOut (
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),           -- �i�X�f��ID�A�D��A�ۼW�A�q1�}�l
    fInOut INT NOT NULL DEFAULT 0,                                  -- �i�f�ΥX�f
    fDate DATE NOT NULL DEFAULT GETDATE(),                              -- �i�X�f���
    fInvoiceId INT NOT NULL,                              -- �o��ID
    fProviderId INT NOT NULL,                 -- ������ID
    fPersonId INT NOT NULL,                   -- �Ȥ�ID
    fProductId INT NOT NULL,                  -- �ӫ~ID
    fCount INT  NOT NULL DEFAULT 0,                                  -- �ӫ~�ƶq
    fPrice INT  NOT NULL DEFAULT 0,                                  -- �i�X�f����]�t�|�^
    fTotal INT NOT NULL DEFAULT 0,                                  -- �`�B�]�t�|�^
    fEditor INT NOT NULL,                                 -- �ק�H
    fNote NVARCHAR(500) NOT NULL DEFAULT ''      -- �Ƶ��A�̤j����500�r��
 );   

 CREATE TABLE tGoodsInAndOutDetail ( 
fId INT NOT NULL PRIMARY KEY IDENTITY(1, 1), -- �۰ʼW�����D�� 
fGoodsInandOutId INT NOT NULL, -- �����\���Ū���� 
fProductId INT NOT NULL , -- �����\���Ū���� 
fCount INT NOT NULL DEFAULT 0, -- �����\���Ū����
fPrice INT NOT NULL DEFAULT 0, -- �����\���Ū����
fSum INT NOT NULL DEFAULT 0 );  -- �����\���Ū� fSum ���

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
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),          -- ���ڳ渹�A�D��A�ۼW�A�q1�}�l
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
    fId INT NOT NULL PRIMARY KEY IDENTITY(1,1),          -- �I�ڳ渹�A�D��A�ۼW�A�q1�}�l
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

CREATE TABLE tAboutUs (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- �۰ʻ��W���D��
    fTitle NVARCHAR(200) NOT NULL DEFAULT '',           -- ���D���
    fContent NVARCHAR(MAX) NOT NULL DEFAULT '',         -- ���e���
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- �O���Ыخɶ�
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- �O����s�ɶ�
);

CREATE TABLE tFAQ (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- �۰ʻ��W���D��
    fQuestion NVARCHAR(500) NOT NULL DEFAULT '',        -- ���D���
    fAnswer NVARCHAR(MAX) NOT NULL DEFAULT '',          -- �������
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- �O���Ыخɶ�
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- �O����s�ɶ�
);

CREATE TABLE tReport(
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      
    fPersonId INT NOT NULL,        
    fClass NVARCHAR(10) NOT NULL DEFAULT '��L',   
    fTitle NVARCHAR(100) NOT NULL DEFAULT '',
    fDescription NVARCHAR(MAX) NOT NULL DEFAULT '',      
    fTime DATETIME NOT NULL DEFAULT GETDATE() 
);


