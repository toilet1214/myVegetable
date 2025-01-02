--drop database [dbVegetable]
--drop table tInvoiceDetail
--drop table tInvoice
--drop table tPurchase
--drop table tPurchaseDetail
CREATE DATABASE [dbVegetable]
GO
USE dbVegetable
CREATE TABLE tInvoiceDetail (
	fId  int  PRIMARY KEY identity(1,1),
	fNumber Nvarchar(50),
	fProductName Nvarchar(50),
	fConut int ,
	fPrice int ,
	fSum int  
);
GO
CREATE TABLE tInvoice (
	fId int  PRIMARY KEY identity(1,1),
    	fNumber nvarchar(50),
	fDate date,
	fForm nvarchar(20),
	fCustomerId int,
	fCustomerUBN nvarchar(50),
	fSupplierId int,
	fSupplierUBN nvarchar(50),
	fInOut int,
	fStatus int,
	fTotal int,
	fEditor nvarchar(50)
);
GO
CREATE TABLE tPurchase (
	fId int  PRIMARY KEY identity(1,1),
    	fBuyDate date,
	fSupplierId nvarchar(50),
	fSupplierName nvarchar(50),
	fBuyer nvarchar(50),
	fExpirationDate date,
	fIsTax nvarchar(50) ,
	fInvoiceForm nvarchar(20),
	fPayment nvarchar(20),
	fCreater nvarchar(20),
	fEditor nvarchar(20),
	fPreTax int,
	fTax int,
	fTotal int,
    	fNote  nvarchar(100) 
);
GO
CREATE TABLE tPurchaseDetail (
	fId int PRIMARY KEY identity(1,1),
   	fPurchaseId nvarchar(50),
	fProductName nvarchar(50),
	fConut int,
	fPrice int,
	fSum  int 
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
fEditor int NOT NULL
);
GO
CREATE TABLE tProduct (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY ,  -- ���~ID�A�۰ʼW��
    fName NVARCHAR(500) NOT NULL DEFAULT '', -- ���~�W�١A�����\�� NULL�A�q�{���Ŧr��
    fClassification NVARCHAR(500) NOT NULL DEFAULT '', -- ���~�����A�����\�� NULL�A�q�{���Ŧr��
    fPrice INT NOT NULL DEFAULT 0,         -- ���~����A��ơA�����\�� NULL�A�q�{�� 0
    fDescription TEXT NOT NULL DEFAULT '',         -- ���~�y�z�A�����\�� NULL�A�q�{���Ŧr��
    fQuantity INT NOT NULL DEFAULT 0,              -- ���~�w�s�ƶq�A�q�{�� 0
    fLaunchAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,  -- �W�[����A�ϥ� DATETIME �����A�q�{����e�ɶ��W
    fStorage NVARCHAR(500) NOT NULL DEFAULT '',     -- �÷Ť覡�A�q�{���Ŧr��
    fOrigin NVARCHAR(500) NOT NULL DEFAULT '',      -- ���~���a�A�q�{���Ŧr��
    fEditor NVARCHAR(500) NOT NULL DEFAULT '',      -- �ק�H�A�q�{���Ŧr��
);
GO
CREATE TABLE tImg
(
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,        -- �Ӥ�ID�A�]�w���۰ʻ��W
    fProductId INT NOT NULL,                      -- �ӫ~ID�A������g
    fName NVARCHAR(500) NOT NULL DEFAULT '',    -- �Ӥ��W�١A�w�]���Ŧr��
    fOrder INT NOT NULL,                       -- �Ӥ��ƧǡA������g
    fUploadAt DATETIME DEFAULT GETDATE(),   -- �W�Ǥ���A�w�]����e�ɶ�
    fEditor NVARCHAR(500) NOT NULL DEFAULT '',  -- �ק�H�A�w�]���Ŧr��
   
);
GO
CREATE TABLE tOrder (
    fId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,  -- �q�� ID (�۰ʻ��W�A�q 1 �}�l�A���W�B���� 1)
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
    fPermission NVARCHAR(500)  NOT NULL DEFAULT 'member',              -- �v���A�w�]���Ŧr��
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
GO

CREATE TABLE tInventoryMain (
    fId INT IDENTITY(1,1) PRIMARY KEY,
    fInventoryNo NVARCHAR(50) NOT NULL,
    fBaselineDate DATE NOT NULL,
    fCreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    fCreatorId NVARCHAR(255) NOT NULL, 
    fEditor NVARCHAR(255) NULL 
);
GO

CREATE TABLE tInventoryDetail (
    fId INT IDENTITY(1,1) PRIMARY KEY, 
    fInventoryNo NVARCHAR(50) NOT NULL,
    fProductId INT NOT NULL, 
    fProductName NVARCHAR(500) NOT NULL,  
    fSystemQuantity INT NOT NULL,  
    fActualQuantity INT NULL, 
    fDifferenceQuantity INT NULL,  

    fAmount INT NOT NULL,                         
    fRemark NVARCHAR(MAX) NULL,
    fEditor NVARCHAR(255) NULL 	
);
GO

INSERT INTO tAboutUs (fTitle, fContent)
VALUES
('����ڭ�', '�ڭ̬O�@�a�M�`�󴣨ѷs�A�B���d�M�������G���������x�C�ڭ̪��ϩR�O���C�@���U�ȴ��ѳ̰��~�誺���~�M�̨Ϊ��ʪ�����C�q��a�A�������t�e��z���\��A�ڭ̽T�O�C�@�󲣫~���ŦX�Y�檺�~��зǡA�åB�C�@���t�e����p���e�F�C\n\n�ڭ̪��ζ��Ѥ@�s���R�����åB�P�O��ﵽ�ͬ��~�誺�M�~�H�h�զ��C���F���G�A�ڭ̤]���ѦU���u�誺�������~�A���b���C���U�Ȫ���`������[���d�P�h�ˤơC\n\n�ڭ̤������`���~�~��A�٭����U�Ȫ�����C�ڭ̪��ȪA�ζ��H�ɬ��z�ѵ�������D�A�ô��ѱM�~����ĳ�M����C�z�����N�O�ڭ̳̭��n���ؼСC\n\n�P�±z��ܧڭ̪��A�ȡA���ݬ��z���ѧ�h�s�A���������~�A���z���ͬ���[���d�C');
GO
INSERT INTO tFAQ (fQuestion, fAnswer)
VALUES
('�����Ӧp��U�q��H', '�z�i�H��ܩһݪ����G�A�N���̲K�[���ʪ����öi�浲�b�C�t�αN�޾ɱz������I�y�{�A����H�Υd�B�u�W��I���覡�C'),
('�ڥi�H�ϥΤ���I�ڤ覡�H', '�ڭ̱����h�إI�ڤ覡�A�]�A�H�Υd�B�ɰO�d�BApple Pay �M Google Pay�C���b�����N��ܥi�Ϊ��I�ڿﶵ�C'),
('�ڦp��d�ߧڪ��q�檬�A�H', '�z�i�H�b�ڭ̪������W�n�J�z���b��A�ìd�ݭq��ԲӫH���M�t�e���A�C�ڭ̤]�|�q�L�q�l�l��q���z�q��i�סC'),
('�t�e�a�Ϧ����ǡH', '�ڭ̴��ѥ���d�򪺰t�e�A�ȡC�z�i�H�b���b�ɬd�ݬO�_����z���t�e�a�}�C'),
('�A�̪����G�O�s�A���ܡH', '�O���A�ڭ̴��Ѫ��s�A���G�O�C��D�諸�A�åB�O�ҳ̰��~��C�ڭ̪����~�Ӧۥ��a�A���A�åB�g�L�Y��z��C'),
('�p�G�ڦ��쪺���G���l�a�A�ӫ���H', '�p�G�z���쪺�ӫ~�l�a�A�Цb24�p�ɤ��pô�ڭ̪��ȪA�ζ��A�ڭ̷|�w�ưh���f�A�ȡC'),
('�p����U�|���H', '�z�u�ݭn�I�������W�誺�u���U�v���s�A��g�z���򥻸�ơA�Y�i�����ڭ̪��|���A�ɨ��M���u�f�M�u�f��C'),
('�ѰO�K�X�ӫ���H', '�b�n�J�����A�I���u�ѰO�K�X�v�A�t�η|�n�D�z���ѵ��U�ɨϥΪ��q�l�l��a�}�A�õo�e���]�K�X���챵�C'),
('�ڥi�H�h�f�ܡH', '�ڭ̴��� 7 �ѵL�z�Ѱh�f�A�ȡC�z�i�H�b����ӫ~�᪺ 7 �Ѥ��pô�ڭ̿�z�h�f�C'),
('�p��ӽаh�ڡH', '�Y�z��q�榳������D�A�Цb�ڭ̪��ȪA������g�h�ڥӽСA�ڭ̱N�b 3-5 �Ӥu�@�餺�B�z�z���h�ڥӽСC');
