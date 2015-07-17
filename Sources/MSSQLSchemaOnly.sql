USE master
GO

CREATE DATABASE SupermarketsChain
GO

USE SupermarketsChain
GO

CREATE TABLE Vendors (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL
)

CREATE TABLE Measures (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL
)

CREATE TABLE Products (
	Id INT PRIMARY KEY IDENTITY,
	VendorId INT NOT NULL FOREIGN KEY REFERENCES Vendors(Id),
	Name NVARCHAR(50) NOT NULL,
	MeasureId INT NOT NULL FOREIGN KEY REFERENCES Measures(Id),
	Price MONEY NOT NULL
)

CREATE TABLE Supermarkets (
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL
)

CREATE TABLE SupermarketsProducts (
	SupermarketId INT FOREIGN KEY REFERENCES Supermarkets(Id),
	ProductId INT FOREIGN KEY REFERENCES Products(Id)
	CONSTRAINT [PK_SupermarketsProducts] PRIMARY KEY CLUSTERED 
	(
		SupermarketId ASC,
		ProductId ASC
	)
)

CREATE TABLE VendorExpensesByMonth(
	Id INT PRIMARY KEY NOT NULL,
	VendorId INT FOREIGN KEY REFERENCES Vendors(Id) NOT NULL,
	[Month] NVARCHAR(50) NOT NULL,
	Expenses money
)
GO

/* Delete data and reset identity

DELETE SupermarketsProducts
DELETE Supermarkets
DBCC CHECKIDENT ('Supermarkets', RESEED,  0);

ALTER TABLE SupermarketsProducts NOCHECK CONSTRAINT ALL
DELETE Products
DBCC CHECKIDENT ('Products', RESEED,  0);
ALTER TABLE SupermarketsProducts CHECK CONSTRAINT ALL

DELETE Measures
DBCC CHECKIDENT ('Measures', RESEED,  0);

DELETE Vendors
DBCC CHECKIDENT ('Vendors', RESEED,  0);

*/
