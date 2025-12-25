Use Jannara
DECLARE @schemaName NVARCHAR(128);
DECLARE @tableName NVARCHAR(128);
DECLARE @sql NVARCHAR(MAX);

DECLARE table_cursor CURSOR FOR
SELECT t.name, s.name
FROM sys.tables t
JOIN sys.schemas s ON t.schema_id = s.schema_id
JOIN sys.columns c ON c.object_id = t.object_id
WHERE c.name = 'updated_at';

OPEN table_cursor;
FETCH NEXT FROM table_cursor INTO @tableName, @schemaName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @sql = '
    CREATE OR ALTER TRIGGER TR_' + @tableName + '_UpdateUpdatedAt
    ON ' + QUOTENAME(@schemaName) + '.' + QUOTENAME(@tableName) + '
    AFTER UPDATE
    AS
    BEGIN
        SET NOCOUNT ON;
        IF NOT UPDATE(updated_at)
        BEGIN
            UPDATE t
            SET updated_at = SYSDATETIME()
            FROM ' + QUOTENAME(@schemaName) + '.' + QUOTENAME(@tableName) + ' t
            INNER JOIN inserted i ON t.Id = i.Id;
        END
    END;
    ';

  
    EXEC sp_executesql @sql;

    FETCH NEXT FROM table_cursor INTO @tableName, @schemaName;
END

CLOSE table_cursor;
DEALLOCATE table_cursor;
