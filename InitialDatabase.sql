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

INSERT INTO Vendors (Name) 
Values 
('Unipek Bulgaria LTD'),
('Kaliakra Bulgaria LTD'),
('Deroni Bulgaria LTD'),
('Tandem Bulgaria LTD'),
('Sachi Bulgaria LTD'),
('Leki Bulgaria LTD'),
('Gradus Bulgaria LTD'),
('Olimpus International LTD'),
('Madzharov Bulgaria LTD'),
('Milka International LTD'),
('Sushard International LTD'),
('Rois Bulgaria LTD'),
('Zagorka Bulgaria LTD'),
('Kamenitza Bulgaria LTD'),
('Pirinsko Bulgaria LTD')

INSERT INTO Measures(Name) 
Values 
('kg'),
('g'),
('l'),
('ml'),
('pieces')

INSERT INTO Products(VendorId, Name, MeasureId, Price) 
Values 
(1, 'White bread', 5, 1.20),
(1, 'Wheet bread', 5, 1.00),
(1, 'Holegrain bread', 5, 1.00),
(2, 'Cooking oil', 3, 2.00),
(2, 'Margarine', 5, 4.00),
(3, 'Chutney', 5, 3.00),
(3, 'Ketchup', 5, 2.00),
(4, 'Ham', 1, 15.00),
(4, 'Tandem Sausages', 1, 9.00),
(5, 'Sachi Sausages', 1, 7.00),
(6, 'Leki Sausages', 1, 6.00),
(7, 'Chiken', 1, 5.50),
(7, 'Chicken drumsticks', 1, 7.00),
(7, 'Chicken wings', 1, 6.00),
(8, 'Olimpus white cheese', 1, 12.00),
(8, 'Olimpus yellow cheese', 1, 18.00),
(9, 'Madzharov white cheese', 1, 10.00),
(9, 'Madzharov yellow cheese', 1, 16.00),
(10, 'Milka kids chockolate', 1, 2.00),
(11, 'Shushard dark chocolate', 1, 1.00),
(12, 'Аlmonds', 1, 5.50),
(12, 'Hazelnuts', 1, 4.50),
(12, 'Cashew', 1, 5.00),
(13, 'Zagorka beer', 1, 1.00),
(14, 'Kamenitza beer', 1, 1.00),
(15, 'Pirinsko beer', 1, 1.00)

INSERT INTO Supermarkets(Name) 
Values 
('Dar Banishora'),
('Dar Ovcha Kupel'),
('Dar Beli Brezi'),
('Dar Mladost'),
('Dar Lyulin')

INSERT INTO SupermarketsProducts(SupermarketId, ProductId) 
Values 
(1, 1),
(2, 1),
(3, 1),
(4, 1),
(5, 1),
(1, 2),
(2, 2),
(3, 2),
(4, 2),
(5, 2),
(1, 3),
(2, 3),
(3, 3),
(4, 3),
(5, 3),
(1, 4),
(2, 4),
(3, 4),
(4, 4),
(5, 4),
(1, 5),
(2, 5),
(3, 5),
(4, 5),
(5, 5),
(1, 6),
(2, 6),
(3, 6),
(4, 6),
(5, 6),
(1, 7),
(2, 7),
(3, 7),
(4, 7),
(5, 7),
(1, 8),
(2, 8),
(3, 8),
(4, 8),
(5, 8),
(1, 9),
(2, 9),
(3, 9),
(4, 9),
(5, 9),
(1, 10),
(2, 10),
(3, 10),
(4, 10),
(5, 10),
(1, 11),
(2, 11),
(3, 11),
(4, 11),
(5, 11),
(1, 12),
(2, 12),
(3, 12),
(4, 12),
(5, 12),
(1, 13),
(2, 13),
(3, 13),
(4, 13),
(5, 13),
(1, 14),
(2, 14),
(3, 14),
(4, 14),
(5, 14),
(1, 15),
(2, 15),
(3, 15),
(4, 15),
(5, 15),
(1, 16),
(2, 16),
(3, 16),
(4, 16),
(5, 16),
(1, 17),
(2, 17),
(3, 17),
(4, 17),
(5, 17),
(1, 18),
(2, 18),
(3, 18),
(4, 18),
(5, 18),
(1, 19),
(2, 19),
(3, 19),
(4, 19),
(5, 19),
(1, 20),
(2, 20),
(3, 20),
(4, 20),
(5, 20),
(1, 21),
(2, 21),
(3, 21),
(4, 21),
(5, 21),
(1, 22),
(2, 22),
(3, 22),
(4, 22),
(5, 22),
(1, 23),
(2, 23),
(3, 23),
(4, 23),
(5, 23),
(1, 24),
(2, 24),
(3, 24),
(4, 24),
(5, 24),
(1, 25),
(2, 25),
(3, 25),
(4, 25),
(5, 25),
(1, 26),
(2, 26),
(3, 26),
(4, 26),
(5, 26)