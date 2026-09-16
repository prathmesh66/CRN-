CREATE TABLE [dbo].[Product](
 [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
 [ProductName] NVARCHAR(255) NOT NULL,
 [CreatedBy] NVARCHAR(100) NOT NULL,
 [CreatedOn] DATETIME2 NOT NULL,
 [ModifiedBy] NVARCHAR(100) NULL,
 [ModifiedOn] DATETIME2 NULL
);
CREATE INDEX IX_Product_ProductName ON [dbo].[Product]([ProductName]);
CREATE TABLE [dbo].[Item](
 [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
 [ProductId] INT NOT NULL,
 [Quantity] INT NOT NULL,
 CONSTRAINT FK_Item_Product FOREIGN KEY([ProductId]) REFERENCES [dbo].[Product]([Id]) ON DELETE CASCADE
);
CREATE INDEX IX_Item_ProductId ON [dbo].[Item]([ProductId]);
CREATE TABLE [dbo].[UserAccount](
 [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
 [Username] NVARCHAR(100) NOT NULL UNIQUE,
 [PasswordHash] NVARCHAR(MAX) NOT NULL,
 [Role] NVARCHAR(50) NOT NULL
);
CREATE TABLE [dbo].[RefreshToken](
 [Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
 [UserAccountId] INT NOT NULL,
 [TokenHash] NVARCHAR(128) NOT NULL UNIQUE,
 [CreatedOnUtc] DATETIME2 NOT NULL,
 [ExpiresOnUtc] DATETIME2 NOT NULL,
 [RevokedOnUtc] DATETIME2 NULL,
 CONSTRAINT FK_RefreshToken_UserAccount FOREIGN KEY([UserAccountId]) REFERENCES [dbo].[UserAccount]([Id]) ON DELETE CASCADE
);
