USE master;
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Jannara')
BEGIN
    ALTER DATABASE [Jannara] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [Jannara];
END
GO

CREATE DATABASE [Jannara];
GO

USE [Jannara];
GO

:r .\schema.sql
:r .\seed_data.sql
:r .\updated_at_triggers.sql
