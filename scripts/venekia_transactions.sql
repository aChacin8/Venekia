USE VenekiaDb
GO

IF OBJECT_ID ('dbo.Transactions', 'U') IS NOT NULL
    DROP TABLE dbo.Transactions;
GO

CREATE TABLE dbo.Transactions (
    Id UNIQUEIDENTIFIER NOT NULL,
    WalletId UNIQUEIDENTIFIER NOT NULL,
    Type INT NOT NULL, -- credit (1) / debit (2)
    Amount DECIMAL(14,2) NOT NULL,
    BalanceBefore DECIMAL(14,2) NOT NULL,
    BalanceAfter DECIMAL(14,2) NOT NULL,
    Reference VARCHAR(100),
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),

    CONSTRAINT PK_Transactions PRIMARY KEY (Id),

    CONSTRAINT FK_Transactions_Wallets
        FOREIGN KEY (WalletId)
        REFERENCES dbo.Wallets (Id)
);
GO