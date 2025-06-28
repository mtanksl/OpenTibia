BEGIN TRANSACTION;
GO

CREATE TABLE [Accounts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(255) NOT NULL,
    [Password] nvarchar(255) NOT NULL,
    [PremiumUntil] datetime2 NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Motd] (
    [Id] int NOT NULL IDENTITY,
    [Message] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_Motd] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ServerStorages] (
    [Key] nvarchar(255) NOT NULL,
    [Value] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_ServerStorages] PRIMARY KEY ([Key])
);
GO

CREATE TABLE [Worlds] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(255) NOT NULL,
    [Ip] nvarchar(255) NOT NULL,
    [Port] int NOT NULL,
    CONSTRAINT [PK_Worlds] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Players] (
    [Id] int NOT NULL IDENTITY,
    [AccountId] int NOT NULL,
    [WorldId] int NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    [Health] int NOT NULL,
    [MaxHealth] int NOT NULL,
    [Direction] int NOT NULL,
    [BaseOutfitItemId] int NOT NULL,
    [BaseOutfitId] int NOT NULL,
    [BaseOutfitHead] int NOT NULL,
    [BaseOutfitBody] int NOT NULL,
    [BaseOutfitLegs] int NOT NULL,
    [BaseOutfitFeet] int NOT NULL,
    [BaseOutfitAddon] int NOT NULL,
    [OutfitItemId] int NOT NULL,
    [OutfitId] int NOT NULL,
    [OutfitHead] int NOT NULL,
    [OutfitBody] int NOT NULL,
    [OutfitLegs] int NOT NULL,
    [OutfitFeet] int NOT NULL,
    [OutfitAddon] int NOT NULL,
    [BaseSpeed] int NOT NULL,
    [Speed] int NOT NULL,
    [Invisible] bit NOT NULL,
    [SkillMagicLevel] int NOT NULL,
    [SkillMagicLevelPoints] bigint NOT NULL,
    [SkillFist] int NOT NULL,
    [SkillFistPoints] bigint NOT NULL,
    [SkillClub] int NOT NULL,
    [SkillClubPoints] bigint NOT NULL,
    [SkillSword] int NOT NULL,
    [SkillSwordPoints] bigint NOT NULL,
    [SkillAxe] int NOT NULL,
    [SkillAxePoints] bigint NOT NULL,
    [SkillDistance] int NOT NULL,
    [SkillDistancePoints] bigint NOT NULL,
    [SkillShield] int NOT NULL,
    [SkillShieldPoints] bigint NOT NULL,
    [SkillFish] int NOT NULL,
    [SkillFishPoints] bigint NOT NULL,
    [Experience] bigint NOT NULL,
    [Level] int NOT NULL,
    [Mana] int NOT NULL,
    [MaxMana] int NOT NULL,
    [Soul] int NOT NULL,
    [Capacity] int NOT NULL,
    [Stamina] int NOT NULL,
    [Gender] int NOT NULL,
    [Vocation] int NOT NULL,
    [Rank] int NOT NULL,
    [SpawnX] int NOT NULL,
    [SpawnY] int NOT NULL,
    [SpawnZ] int NOT NULL,
    [TownX] int NOT NULL,
    [TownY] int NOT NULL,
    [TownZ] int NOT NULL,
    [BankAccount] bigint NOT NULL,
    CONSTRAINT [PK_Players] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Players_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Players_Worlds_WorldId] FOREIGN KEY ([WorldId]) REFERENCES [Worlds] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Bans] (
    [Id] int NOT NULL IDENTITY,
    [Type] int NOT NULL,
    [AccountId] int NULL,
    [PlayerId] int NULL,
    [IpAddress] nvarchar(255) NULL,
    [Message] nvarchar(255) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Bans] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bans_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
    CONSTRAINT [FK_Bans_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id])
);
GO

CREATE TABLE [BugReports] (
    [Id] int NOT NULL IDENTITY,
    [PlayerId] int NOT NULL,
    [PositionX] int NOT NULL,
    [PositionY] int NOT NULL,
    [PositionZ] int NOT NULL,
    [Message] nvarchar(255) NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_BugReports] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BugReports_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [DebugAsserts] (
    [Id] int NOT NULL IDENTITY,
    [PlayerId] int NOT NULL,
    [AssertLine] nvarchar(255) NOT NULL,
    [ReportDate] nvarchar(255) NOT NULL,
    [Description] nvarchar(255) NOT NULL,
    [Comment] nvarchar(255) NULL,
    [CreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_DebugAsserts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_DebugAsserts_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Houses] (
    [Id] int NOT NULL IDENTITY,
    [OwnerId] int NULL,
    CONSTRAINT [PK_Houses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Houses_Players_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Players] ([Id])
);
GO

CREATE TABLE [PlayerAchievements] (
    [PlayerId] int NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_PlayerAchievements] PRIMARY KEY ([PlayerId], [Name]),
    CONSTRAINT [FK_PlayerAchievements_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerBlesses] (
    [PlayerId] int NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_PlayerBlesses] PRIMARY KEY ([PlayerId], [Name]),
    CONSTRAINT [FK_PlayerBlesses_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerDeaths] (
    [Id] int NOT NULL IDENTITY,
    [PlayerId] int NOT NULL,
    [AttackerId] int NULL,
    [Name] nvarchar(255) NULL,
    [Level] int NOT NULL,
    [Unjustified] bit NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_PlayerDeaths] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PlayerDeaths_Players_AttackerId] FOREIGN KEY ([AttackerId]) REFERENCES [Players] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_PlayerDeaths_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerDepotItems] (
    [PlayerId] int NOT NULL,
    [SequenceId] int NOT NULL,
    [ParentId] int NOT NULL,
    [OpenTibiaId] int NOT NULL,
    [Count] int NOT NULL,
    CONSTRAINT [PK_PlayerDepotItems] PRIMARY KEY ([PlayerId], [SequenceId]),
    CONSTRAINT [FK_PlayerDepotItems_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerItems] (
    [PlayerId] int NOT NULL,
    [SequenceId] int NOT NULL,
    [ParentId] int NOT NULL,
    [OpenTibiaId] int NOT NULL,
    [Count] int NOT NULL,
    CONSTRAINT [PK_PlayerItems] PRIMARY KEY ([PlayerId], [SequenceId]),
    CONSTRAINT [FK_PlayerItems_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerKills] (
    [Id] int NOT NULL IDENTITY,
    [PlayerId] int NOT NULL,
    [TargetId] int NOT NULL,
    [Unjustified] bit NOT NULL,
    [CreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_PlayerKills] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PlayerKills_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlayerKills_Players_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Players] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [PlayerOutfits] (
    [PlayerId] int NOT NULL,
    [OutfitId] int NOT NULL,
    [OutfitAddon] int NOT NULL,
    CONSTRAINT [PK_PlayerOutfits] PRIMARY KEY ([PlayerId], [OutfitId]),
    CONSTRAINT [FK_PlayerOutfits_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerSpells] (
    [PlayerId] int NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_PlayerSpells] PRIMARY KEY ([PlayerId], [Name]),
    CONSTRAINT [FK_PlayerSpells_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerStorages] (
    [PlayerId] int NOT NULL,
    [Key] int NOT NULL,
    [Value] int NOT NULL,
    CONSTRAINT [PK_PlayerStorages] PRIMARY KEY ([PlayerId], [Key]),
    CONSTRAINT [FK_PlayerStorages_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PlayerVips] (
    [PlayerId] int NOT NULL,
    [VipId] int NOT NULL,
    CONSTRAINT [PK_PlayerVips] PRIMARY KEY ([PlayerId], [VipId]),
    CONSTRAINT [FK_PlayerVips_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_PlayerVips_Players_VipId] FOREIGN KEY ([VipId]) REFERENCES [Players] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [RuleViolationReports] (
    [Id] int NOT NULL IDENTITY,
    [PlayerId] int NOT NULL,
    [Type] int NOT NULL,
    [RuleViolation] int NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    [Comment] nvarchar(255) NOT NULL,
    [Translation] nvarchar(255) NULL,
    [Statment] nvarchar(255) NULL,
    [CreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_RuleViolationReports] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RuleViolationReports_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Players] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [HouseAccessLists] (
    [HouseId] int NOT NULL,
    [ListId] int NOT NULL,
    [Text] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_HouseAccessLists] PRIMARY KEY ([HouseId], [ListId]),
    CONSTRAINT [FK_HouseAccessLists_Houses_HouseId] FOREIGN KEY ([HouseId]) REFERENCES [Houses] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [HouseItems] (
    [HouseId] int NOT NULL,
    [SequenceId] bigint NOT NULL,
    [ParentId] bigint NOT NULL,
    [OpenTibiaId] int NOT NULL,
    [Count] int NOT NULL,
    CONSTRAINT [PK_HouseItems] PRIMARY KEY ([HouseId], [SequenceId]),
    CONSTRAINT [FK_HouseItems_Houses_HouseId] FOREIGN KEY ([HouseId]) REFERENCES [Houses] ([Id]) ON DELETE CASCADE
);
GO

SET IDENTITY_INSERT [Accounts] ON;
INSERT INTO [Accounts] ([Id], [Name], [Password], [PremiumUntil]) VALUES (1, N'1', N'1', NULL);
SET IDENTITY_INSERT [Accounts] OFF;
GO

SET IDENTITY_INSERT [Motd] ON;
INSERT INTO [Motd] ([Id], [Message])
VALUES (1, N'MTOTS - An open Tibia server developed by mtanksl');
SET IDENTITY_INSERT [Motd] OFF;
GO

INSERT INTO [ServerStorages] ([Key], [Value])
VALUES (N'PlayersPeek', N'0');
GO

SET IDENTITY_INSERT [Worlds] ON;
INSERT INTO [Worlds] ([Id], [Ip], [Name], [Port])
VALUES (1, N'127.0.0.1', N'Cormaya', 7172);
SET IDENTITY_INSERT [Worlds] OFF;
GO

SET IDENTITY_INSERT [Players] ON;
INSERT INTO [Players] ([Id], [AccountId], [BankAccount], [BaseOutfitAddon], [BaseOutfitBody], [BaseOutfitFeet], [BaseOutfitHead], [BaseOutfitId], [BaseOutfitItemId], [BaseOutfitLegs], [BaseSpeed], [Capacity], [Direction], [Experience], [Gender], [Health], [Invisible], [Level], [Mana], [MaxHealth], [MaxMana], [Name], [OutfitAddon], [OutfitBody], [OutfitFeet], [OutfitHead], [OutfitId], [OutfitItemId], [OutfitLegs], [Rank], [SkillAxe], [SkillAxePoints], [SkillClub], [SkillClubPoints], [SkillDistance], [SkillDistancePoints], [SkillFish], [SkillFishPoints], [SkillFist], [SkillFistPoints], [SkillMagicLevel], [SkillMagicLevelPoints], [SkillShield], [SkillShieldPoints], [SkillSword], [SkillSwordPoints], [Soul], [SpawnX], [SpawnY], [SpawnZ], [Speed], [Stamina], [TownX], [TownY], [TownZ], [Vocation], [WorldId])
VALUES (1, 1, CAST(0 AS bigint), 0, 0, 0, 0, 75, 0, 0, 2218, 139000, 2, CAST(15694800 AS bigint), 0, 645, CAST(0 AS bit), 100, 550, 645, 550, N'Gamemaster', 0, 0, 0, 0, 75, 0, 0, 2, 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 0, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 100, 921, 771, 6, 2218, 2520, 921, 771, 6, 0, 1),
(2, 1, CAST(0 AS bigint), 0, 0, 0, 0, 131, 0, 0, 418, 132900, 2, CAST(15694800 AS bigint), 0, 1565, CAST(0 AS bit), 100, 550, 1565, 550, N'Knight', 0, 0, 0, 0, 131, 0, 0, 0, 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 4, CAST(0 AS bigint), 80, CAST(0 AS bigint), 90, CAST(0 AS bigint), 100, 921, 771, 6, 418, 2520, 921, 771, 6, 1, 1),
(3, 1, CAST(0 AS bigint), 0, 0, 0, 0, 129, 0, 0, 418, 132900, 2, CAST(15694800 AS bigint), 0, 1105, CAST(0 AS bit), 100, 1470, 1105, 1470, N'Paladin', 0, 0, 0, 0, 129, 0, 0, 0, 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 70, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 15, CAST(0 AS bigint), 40, CAST(0 AS bigint), 10, CAST(0 AS bigint), 100, 921, 771, 6, 418, 2520, 921, 771, 6, 2, 1),
(4, 1, CAST(0 AS bigint), 0, 0, 0, 0, 130, 0, 0, 418, 132900, 2, CAST(15694800 AS bigint), 0, 645, CAST(0 AS bit), 100, 2850, 645, 2850, N'Sorcerer', 0, 0, 0, 0, 130, 0, 0, 0, 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 60, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 100, 921, 771, 6, 418, 2520, 921, 771, 6, 4, 1),
(5, 1, CAST(0 AS bigint), 0, 0, 0, 0, 130, 0, 0, 418, 132900, 2, CAST(15694800 AS bigint), 0, 645, CAST(0 AS bit), 100, 2850, 645, 2850, N'Druid', 0, 0, 0, 0, 130, 0, 0, 0, 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 60, CAST(0 AS bigint), 10, CAST(0 AS bigint), 10, CAST(0 AS bigint), 100, 921, 771, 6, 418, 2520, 921, 771, 6, 3, 1);
SET IDENTITY_INSERT [Players] OFF;
GO

INSERT INTO [PlayerItems] ([PlayerId], [SequenceId], [Count], [OpenTibiaId], [ParentId])
VALUES (1, 101, 1, 1987, 3),
(1, 102, 1, 2120, 101),
(1, 103, 1, 2554, 101),
(2, 101, 1, 1987, 3),
(2, 102, 1, 2120, 101),
(2, 103, 1, 2554, 101),
(3, 101, 1, 1987, 3),
(3, 102, 1, 2120, 101),
(3, 103, 1, 2554, 101),
(4, 101, 1, 1987, 3),
(4, 102, 1, 2120, 101),
(4, 103, 1, 2554, 101),
(5, 101, 1, 1987, 3),
(5, 102, 1, 2120, 101),
(5, 103, 1, 2554, 101);
GO

INSERT INTO [PlayerOutfits] ([OutfitId], [PlayerId], [OutfitAddon])
VALUES (128, 1, 0),
(129, 1, 0),
(130, 1, 0),
(131, 1, 0),
(128, 2, 0),
(129, 2, 0),
(130, 2, 0),
(131, 2, 0),
(128, 3, 0),
(129, 3, 0),
(130, 3, 0),
(131, 3, 0),
(128, 4, 0),
(129, 4, 0),
(130, 4, 0),
(131, 4, 0),
(128, 5, 0),
(129, 5, 0),
(130, 5, 0),
(131, 5, 0);
GO

CREATE INDEX [IX_Bans_AccountId] ON [Bans] ([AccountId]);
GO

CREATE INDEX [IX_Bans_PlayerId] ON [Bans] ([PlayerId]);
GO

CREATE INDEX [IX_BugReports_PlayerId] ON [BugReports] ([PlayerId]);
GO

CREATE INDEX [IX_DebugAsserts_PlayerId] ON [DebugAsserts] ([PlayerId]);
GO

CREATE INDEX [IX_Houses_OwnerId] ON [Houses] ([OwnerId]);
GO

CREATE INDEX [IX_PlayerDeaths_AttackerId] ON [PlayerDeaths] ([AttackerId]);
GO

CREATE INDEX [IX_PlayerDeaths_PlayerId] ON [PlayerDeaths] ([PlayerId]);
GO

CREATE INDEX [IX_PlayerKills_PlayerId] ON [PlayerKills] ([PlayerId]);
GO

CREATE INDEX [IX_PlayerKills_TargetId] ON [PlayerKills] ([TargetId]);
GO

CREATE INDEX [IX_Players_AccountId] ON [Players] ([AccountId]);
GO

CREATE INDEX [IX_Players_WorldId] ON [Players] ([WorldId]);
GO

CREATE INDEX [IX_PlayerVips_VipId] ON [PlayerVips] ([VipId]);
GO

CREATE INDEX [IX_RuleViolationReports_PlayerId] ON [RuleViolationReports] ([PlayerId]);
GO

COMMIT;
GO

--

BEGIN TRANSACTION;
GO

ALTER TABLE [RuleViolationReports] ADD [StatmentDate] datetime2 NULL;
GO

ALTER TABLE [RuleViolationReports] ADD [StatmentPlayerId] int NULL;
GO

CREATE INDEX [IX_RuleViolationReports_StatmentPlayerId] ON [RuleViolationReports] ([StatmentPlayerId]);
GO

ALTER TABLE [RuleViolationReports] ADD CONSTRAINT [FK_RuleViolationReports_Players_StatmentPlayerId] FOREIGN KEY ([StatmentPlayerId]) REFERENCES [Players] ([Id]);
GO

COMMIT;
GO

--

BEGIN TRANSACTION;
GO

ALTER TABLE [Players] ADD [MaxCapacity] int NOT NULL DEFAULT 0;
GO

UPDATE [Players] SET [MaxCapacity] = 139000
WHERE [Id] = 1;
GO

UPDATE [Players] SET [MaxCapacity] = 139000
WHERE [Id] = 2;
GO

UPDATE [Players] SET [MaxCapacity] = 139000
WHERE [Id] = 3;
GO

UPDATE [Players] SET [MaxCapacity] = 139000
WHERE [Id] = 4;
GO

UPDATE [Players] SET [MaxCapacity] = 139000
WHERE [Id] = 5;
GO

COMMIT;
GO

--

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]

WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'OutfitAddon');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Players] DROP COLUMN [OutfitAddon];
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'OutfitBody');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Players] DROP COLUMN [OutfitBody];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'OutfitFeet');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Players] DROP COLUMN [OutfitFeet];
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'OutfitHead');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [Players] DROP COLUMN [OutfitHead];
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'OutfitId');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [Players] DROP COLUMN [OutfitId];
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'OutfitItemId');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [Players] DROP COLUMN [OutfitItemId];
GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'OutfitLegs');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [Players] DROP COLUMN [OutfitLegs];
GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Players]') AND [c].[name] = N'Speed');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [Players] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [Players] DROP COLUMN [Speed];
GO

COMMIT;
GO

--

BEGIN TRANSACTION;
GO

ALTER TABLE [PlayerItems] ADD [Attributes] varbinary(max) NULL;
GO

ALTER TABLE [PlayerDepotItems] ADD [Attributes] varbinary(max) NULL;
GO

ALTER TABLE [HouseItems] ADD [Attributes] varbinary(max) NULL;
GO

COMMIT;
GO

--

BEGIN TRANSACTION;
GO

ALTER TABLE [Players] ADD [BaseOutfitMount] int NOT NULL DEFAULT 0;
GO

UPDATE [Players] SET [BaseOutfitMount] = 0
WHERE [Id] = 1;
GO

UPDATE [Players] SET [BaseOutfitMount] = 0
WHERE [Id] = 2;
GO

UPDATE [Players] SET [BaseOutfitMount] = 0
WHERE [Id] = 3;
GO

UPDATE [Players] SET [BaseOutfitMount] = 0
WHERE [Id] = 4;
GO

UPDATE [Players] SET [BaseOutfitMount] = 0
WHERE [Id] = 5;
GO

COMMIT;
GO

--

BEGIN TRANSACTION;
GO

ALTER TABLE [PlayerVips] ADD [Description] nvarchar(max) NULL;
GO

ALTER TABLE [PlayerVips] ADD [IconId] int NOT NULL DEFAULT 0;
GO

ALTER TABLE [PlayerVips] ADD [NotifyLogin] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

COMMIT;
GO