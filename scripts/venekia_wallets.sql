USE VenekiaDb;
GO

IF OBJECT_ID ('dbo.Wallets', 'U') IS NOT NULL
	DROP TABLE dbo.Wallets;
GO

CREATE TABLE dbo.Wallets (
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	UserId UNIQUEIDENTIFIER NOT NULL,
	Currency CHAR(3) NOT NULL,
	Balance DECIMAL (14,2) NOT NULL DEFAULT 0.00,
	Status INT NOT NULL DEFAULT 1,
	CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
	UpdatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT FK_Wallets_Users
        FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id)
        ON DELETE CASCADE,

    CONSTRAINT UQ_Wallets_User_Currency 
        UNIQUE (UserId, Currency), --Un usuario no puede tener dos wallets con la misma Currency
);

GO