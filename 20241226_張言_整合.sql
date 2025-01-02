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
    fId INT IDENTITY(1,1) PRIMARY KEY ,  -- ���~ID�A�۰ʼW��
    fName NVARCHAR(500) NOT NULL DEFAULT '', -- ���~�W�١A�����\�� NULL�A�q�{���Ŧr��
    fClassification NVARCHAR(500) NOT NULL DEFAULT '', -- ���~�����A�����\�� NULL�A�q�{���Ŧr��
    fPrice INT NOT NULL DEFAULT 0,         -- ���~����A��ơA�����\�� NULL�A�q�{�� 0
    fDescription TEXT NOT NULL DEFAULT '',         -- ���~�y�z�A�����\�� NULL�A�q�{���Ŧr��
    fQuantity INT NOT NULL DEFAULT 0,              -- ���~�w�s�ƶq�A�q�{�� 0
    fLaunchAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,  -- �W�[����A�ϥ� DATETIME �����A�q�{����e�ɶ��W
    fStorage NVARCHAR(500) NOT NULL DEFAULT '',     -- �÷Ť覡�A�q�{���Ŧr��
    fOrigin NVARCHAR(500) NOT NULL DEFAULT '',      -- ���~���a�A�q�{���Ŧr��
    fEditer NVARCHAR(500) NOT NULL DEFAULT '',      -- �ק�H�A�q�{���Ŧr��
);
GO
CREATE TABLE tImg
(
    fId INT IDENTITY(1,1) PRIMARY KEY,        -- �Ӥ�ID�A�]�w���۰ʻ��W
    fProductId INT NOT NULL,                      -- �ӫ~ID�A������g
    fName NVARCHAR(500) NOT NULL DEFAULT '',    -- �Ӥ��W�١A�w�]���Ŧr��
    fOrder INT NOT NULL,                       -- �Ӥ��ƧǡA������g
    fUploadAt DATETIME DEFAULT GETDATE(),   -- �W�Ǥ���A�w�]����e�ɶ�
    fEditer NVARCHAR(500) NOT NULL DEFAULT '',  -- �ק�H�A�w�]���Ŧr��
   
);
GO
CREATE TABLE tOrder (
    fId INT IDENTITY(1,1) PRIMARY KEY,  -- �q�� ID (�۰ʻ��W�A�q 1 �}�l�A���W�B���� 1)
    fBuyerId INT NOT NULL,  -- �ʶR�H (�����\����)
    fTotal INT NOT NULL,  -- �`���B (�����\����)
    fStatus NVARCHAR(50) NOT NULL,  -- �q�檬�A (�����\����)
    fOrderAt DATETIME NOT NULL DEFAULT GETDATE(),  -- �q��Ыخɶ� (�����\���šA�q�{����e�ɶ�)
    fAddress NVARCHAR(255) NOT NULL,  -- ����H�a�} (�����\����)
    fReceiverName NVARCHAR(100) NOT NULL,  -- ����H�m�W (�����\����)
    fPhone NVARCHAR(15) NOT NULL,  -- ����H�q�� (�����\����)
    fRemark NVARCHAR(MAX) NOT NULL DEFAULT '',  -- �Ƶ� (�����\���šA�q�{���Ŧr��)
   
);
GO
CREATE TABLE tPerson (
    fId INT IDENTITY(1,1) PRIMARY KEY,            -- �ۼW�D��
    fName NVARCHAR(500) NOT NULL DEFAULT '',       -- �|���m�W�A�w�]���Ŧr��
    fAccount NVARCHAR(500) NOT NULL DEFAULT '',    -- �|���b���A�w�]���Ŧr��
    fPassword NVARCHAR(500) NOT NULL DEFAULT '',   -- �|���K�X�A�w�]���Ŧr��
    fGender NVARCHAR(100) NOT NULL DEFAULT '',      -- �|���m�O�A�w�]���Ŧr��
    fBirth DATE NOT NULL,                          -- �|���ͤ�
    fPhone NVARCHAR(20)  NOT NULL DEFAULT '',                -- �|������A�w�]���Ŧr��
    fTel NVARCHAR(20)  NOT NULL DEFAULT '',                  -- �|���a�q�A�w�]���Ŧr��
    fAddress NVARCHAR(500)  NOT NULL DEFAULT '',             -- �|���a�}�A�w�]���Ŧr��
    fEmail NVARCHAR(500)  NOT NULL DEFAULT '',               -- �|��Email�A�w�]���Ŧr��
    fUBN NVARCHAR(50)  NOT NULL DEFAULT '',             -- �|���νs�A�w�]���Ŧr��
    fPermissiion NVARCHAR(500)  NOT NULL DEFAULT '',              -- �v���A�w�]���Ŧr��
    fEmp NVARCHAR(500)  NOT NULL DEFAULT '',                 -- ����p���H�m�W�A�w�]���Ŧr��
    fEmpTel NVARCHAR(20)  NOT NULL DEFAULT '',           -- ����p���H�q�ܡA�w�]���Ŧr��
    fCreatedAt DATETIME DEFAULT GETDATE(),       -- �Ыخɶ��A�w�]����e�ɶ�
    fEditor NVARCHAR(500)  NOT NULL DEFAULT ''               -- �s��̡A�w�]���Ŧr��
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
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,       -- �q����ӽs���A�۰ʻ��W
    fOrderId INT NOT NULL,                          -- �q��s�� (�~��ѷӭq��� fO_Id)
    fProductId INT NOT NULL,                    -- �ӫ~ID
    fProductName NVARCHAR(255) NOT NULL,        -- �ӫ~�W��
    fPrice INT NOT NULL,                 -- �ӫ~��� (�אּ INT)
    fCount INT NOT NULL,              -- �ƶq
    fSum INT NOT NULL,                   -- �p�p (�אּ INT)
   );
GO
CREATE TABLE tFAQ (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- �۰ʻ��W���D��
    fQuestion NVARCHAR(500) NOT NULL,        -- ���D���
    fAnswer NVARCHAR(MAX) NOT NULL,          -- �������
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- �O���Ыخɶ�
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- �O����s�ɶ�
);
GO
CREATE TABLE tAboutUs (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,      -- �۰ʻ��W���D��
    fTitle NVARCHAR(200) NOT NULL,           -- ���D���
    fContent NVARCHAR(MAX) NOT NULL,         -- ���e���
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),   -- �O���Ыخɶ�
    fUpdatedAt DATETIME NOT NULL DEFAULT GETDATE()    -- �O����s�ɶ�
);
