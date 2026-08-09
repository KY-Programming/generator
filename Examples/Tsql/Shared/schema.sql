-- The demo database both T-SQL examples read, applied by their prepare.js into a container created for
-- that run. The server is always empty, so the guards below are not needed for the build - they only keep
-- the script safe to run by hand against an existing database.
--
-- The columns are picked to cover the interesting parts of the type mapping: a primary key, an identity,
-- nullable and non-nullable columns, dates, decimals and a uniqueidentifier.

IF DB_ID('KyGeneratorExample') IS NULL
BEGIN
    CREATE DATABASE KyGeneratorExample;
END
GO

USE KyGeneratorExample;
GO

IF OBJECT_ID('dbo.Person', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Person
    (
        Id INT IDENTITY(1,1) NOT NULL,
        FirstName NVARCHAR(100) NOT NULL,
        LastName NVARCHAR(100) NOT NULL,
        Birthday DATE NULL,
        Height DECIMAL(5,2) NULL,
        IsActive BIT NOT NULL,
        PublicId UNIQUEIDENTIFIER NOT NULL,
        CONSTRAINT PK_Person PRIMARY KEY (Id)
    );
END
GO

IF OBJECT_ID('dbo.Address', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Address
    (
        Id INT IDENTITY(1,1) NOT NULL,
        PersonId INT NOT NULL,
        Street NVARCHAR(200) NOT NULL,
        ZipCode NVARCHAR(10) NULL,
        City NVARCHAR(100) NOT NULL,
        CONSTRAINT PK_Address PRIMARY KEY (Id),
        CONSTRAINT FK_Address_Person FOREIGN KEY (PersonId) REFERENCES dbo.Person (Id)
    );
END
GO
