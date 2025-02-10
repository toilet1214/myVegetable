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

--person
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Dwayne Strong', '123', '123', '�k', '1990-06-08', '0979874614', '079599366', 'Unit 6832 Box 4261\nDPO AE 11131', 'bradley56@roth.com', '33448366', 1, 'William Stephens', '0962975478', 1);
INSERT INTO tPerson (fName, fAccount, fPassword, fGender, fBirth, fPhone, fTel, fAddress, fEmail, fUBN, fPermission, fEmp, fEmpTel, fEditor) 
VALUES ('Brian Cole', '456', '456', '�k', '1979-01-07', '0969038974', '071316228', '572 Dixon Cliffs\nNew Eileen, AZ 39214', 'paula82@gmail.com', '62276379', 0, 'Amy Johnson', '0995817086', 1);
