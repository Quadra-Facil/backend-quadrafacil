IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [ArenaHours] (
    [Id] int NOT NULL IDENTITY,
    [ArenaId] int NOT NULL,
    [WeekDays] nvarchar(max) NOT NULL,
    [StartTime] time NOT NULL,
    [EndTime] time NOT NULL,
    CONSTRAINT [PK_ArenaHours] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250128140033_arenahours', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [ArenaHours] ADD [Open] bit NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250130155008_collun-open', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Promotions]') AND [c].[name] = N'StartDate');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Promotions] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Promotions] ALTER COLUMN [StartDate] datetime2 NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Promotions]') AND [c].[name] = N'EndDate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Promotions] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Promotions] ALTER COLUMN [EndDate] datetime2 NULL;
GO

ALTER TABLE [Promotions] ADD [QtdPeople] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [Promotions] ADD [WeekDays] nvarchar(max) NOT NULL DEFAULT N'[]';
GO

ALTER TABLE [Promotions] ADD [When] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250203131159_coluns-important-promotions', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Reserve] ADD [Value] int NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250208151258_collumValueReserve', N'8.0.10');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250422010558_uptablesdbhostcloud', N'8.0.10');
GO

COMMIT;
GO

