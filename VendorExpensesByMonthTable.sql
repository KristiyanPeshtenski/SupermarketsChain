USE SupermarketsChain
GO

CREATE TABLE VendorExpensesByMonth(
	Id INT PRIMARY KEY NOT NULL,
	VendorId INT FOREIGN KEY REFERENCES Vendors(Id) NOT NULL,
	[Month] NVARCHAR(50) NOT NULL,
	Expenses money
)
GO