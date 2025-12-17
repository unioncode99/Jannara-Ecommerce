use master
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'Jannara')
BEGIN
    ALTER DATABASE [Jannara] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [Jannara];
END
GO

:r .\schema.sql
