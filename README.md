#1. Install SQL Server with Docker

docker pull mcr.microsoft.com/mssql/server:2022-lts
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=pass@123" -p 1433:1433 --name sqlcache -d mcr.microsoft.com/mssql/server:2022-lts

#2. Connect to SQL Server

sqlcmd -S localhost,1433 -U sa -P pass@123

#3. Create Database and Table

CREATE DATABASE CacheDb;
GO

#4. Switch to CacheDb and create the Cache table:

USE CacheDb;
GO

CREATE TABLE dbo.Cache (
    Id NVARCHAR(449) NOT NULL PRIMARY KEY,
    Value VARBINARY(MAX) NOT NULL,
    ExpiresAtTime DATETIMEOFFSET NOT NULL,
    SlidingExpirationInSeconds BIGINT NULL,
    AbsoluteExpiration DATETIMEOFFSET NULL
);
GO

#5. dotnet run

#6. Run localhost:5000