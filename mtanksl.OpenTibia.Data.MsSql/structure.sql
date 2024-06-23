USE [mtots]
GO

-- Accounts

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Accounts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[PremiumDays] [int] NOT NULL,
 CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Bans

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bans](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[AccountId] [int] NULL,
	[PlayerId] [int] NULL,
	[IpAddress] [nvarchar](255) NULL,
	[Message] [nvarchar](255) NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Bans] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- BugReports

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BugReports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Message] [nvarchar](255) NOT NULL,
	[CreationDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_BugReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- DebugAsserts

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DebugAsserts](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[AssertLine] [nvarchar](255) NOT NULL,
	[ReportDate] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Comment] [nvarchar](255) NULL,
	[CreationDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DebugAsserts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Motd

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Motd](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Motd] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerDepotItems

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerDepotItems](
	[PlayerId] [int] NOT NULL,
	[SequenceId] [int] NOT NULL,
	[ParentId] [int] NOT NULL,
	[OpenTibiaId] [int] NOT NULL,
	[Count] [int] NOT NULL,
 CONSTRAINT [PK_PlayerDepotItems] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[SequenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerItems

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerItems](
	[PlayerId] [int] NOT NULL,
	[SequenceId] [int] NOT NULL,
	[ParentId] [int] NOT NULL,
	[OpenTibiaId] [int] NOT NULL,
	[Count] [int] NOT NULL,
 CONSTRAINT [PK_PlayerItems] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[SequenceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Players

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Players](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AccountId] [int] NOT NULL,
	[WorldId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Health] [int] NOT NULL,
	[MaxHealth] [int] NOT NULL,
	[Direction] [int] NOT NULL,
	[BaseOutfitItemId] [int] NOT NULL,
	[BaseOutfitId] [int] NOT NULL,
	[BaseOutfitHead] [int] NOT NULL,
	[BaseOutfitBody] [int] NOT NULL,
	[BaseOutfitLegs] [int] NOT NULL,
	[BaseOutfitFeet] [int] NOT NULL,
	[BaseOutfitAddon] [int] NOT NULL,
	[OutfitItemId] [int] NOT NULL,
	[OutfitId] [int] NOT NULL,
	[OutfitHead] [int] NOT NULL,
	[OutfitBody] [int] NOT NULL,
	[OutfitLegs] [int] NOT NULL,
	[OutfitFeet] [int] NOT NULL,
	[OutfitAddon] [int] NOT NULL,
	[BaseSpeed] [int] NOT NULL,
	[Speed] [int] NOT NULL,
	[Invisible] [bit] NOT NULL,
	[SkillMagicLevel] [int] NOT NULL,
	[SkillMagicLevelPercent] [int] NOT NULL,
	[SkillFist] [int] NOT NULL,
	[SkillFistPercent] [int] NOT NULL,
	[SkillClub] [int] NOT NULL,
	[SkillClubPercent] [int] NOT NULL,
	[SkillSword] [int] NOT NULL,
	[SkillSwordPercent] [int] NOT NULL,
	[SkillAxe] [int] NOT NULL,
	[SkillAxePercent] [int] NOT NULL,
	[SkillDistance] [int] NOT NULL,
	[SkillDistancePercent] [int] NOT NULL,
	[SkillShield] [int] NOT NULL,
	[SkillShieldPercent] [int] NOT NULL,
	[SkillFish] [int] NOT NULL,
	[SkillFishPercent] [int] NOT NULL,
	[Experience] [bigint] NOT NULL,
	[Level] [int] NOT NULL,
	[LevelPercent] [int] NOT NULL,
	[Mana] [int] NOT NULL,
	[MaxMana] [int] NOT NULL,
	[Soul] [int] NOT NULL,
	[Capacity] [int] NOT NULL,
	[Stamina] [int] NOT NULL,
	[Gender] [int] NOT NULL,
	[Vocation] [int] NOT NULL,
	[Rank] [int] NOT NULL,
	[SpawnX] [int] NOT NULL,
	[SpawnY] [int] NOT NULL,
	[SpawnZ] [int] NOT NULL,
	[TownX] [int] NOT NULL,
	[TownY] [int] NOT NULL,
	[TownZ] [int] NOT NULL,
 CONSTRAINT [PK_Players] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerStorages

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerStorages](
	[PlayerId] [int] NOT NULL,
	[Key] [int] NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_PlayerStorages] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[Key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerOutfits

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerOutfits](
	[PlayerId] [int] NOT NULL,
	[OutfitId] [int] NOT NULL,
	[OutfitAddon] [int] NOT NULL,
 CONSTRAINT [PK_PlayerOutfits] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[OutfitId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerSpells

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerSpells](
	[PlayerId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PlayerSpells] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerBlesses

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerBlesses](
	[PlayerId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PlayerBlesses] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerAchievements

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerAchievements](
	[PlayerId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PlayerAchievements] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- PlayerVips

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlayerVips](
	[PlayerId] [int] NOT NULL,
	[VipId] [int] NOT NULL,
 CONSTRAINT [PK_PlayerVips] PRIMARY KEY CLUSTERED 
(
	[PlayerId] ASC,
	[VipId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- RuleViolationReports

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RuleViolationReports](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlayerId] [int] NOT NULL,
	[Type] [int] NOT NULL,
	[RuleViolation] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Comment] [nvarchar](255) NOT NULL,
	[Translation] [nvarchar](255) NULL,
	[Statment] [nvarchar](255) NULL,
	[CreationDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_RuleViolationReports] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Worlds

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Worlds](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Ip] [nvarchar](255) NOT NULL,
	[Port] [int] NOT NULL,
 CONSTRAINT [PK_Worlds] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Houses

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Houses](
	[Id] [int] NOT NULL,
	[OwnerId] [int] NULL,
 CONSTRAINT [PK_Houses] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Houses]  WITH CHECK ADD  CONSTRAINT [FK_Houses_OwnerId] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Players] ([Id])
GO

ALTER TABLE [dbo].[Houses] CHECK CONSTRAINT [FK_Houses_OwnerId]
GO

-- HouseAccessLists

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HouseAccessLists](
	[HouseId] [int] NOT NULL,
	[ListId] [int] NOT NULL,
	[Text] [text] NOT NULL,
 CONSTRAINT [PK_HouseAccessLists] PRIMARY KEY CLUSTERED 
(
	[ListId] ASC,
	[HouseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[HouseAccessLists]  WITH CHECK ADD  CONSTRAINT [FK_HouseAccessLists_HouseId] FOREIGN KEY([HouseId])
REFERENCES [dbo].[Houses] ([Id])
GO

ALTER TABLE [dbo].[HouseAccessLists] CHECK CONSTRAINT [FK_HouseAccessLists_HouseId]
GO

SET IDENTITY_INSERT [dbo].[Accounts] ON 
GO
INSERT [dbo].[Accounts] ([Id], [Name], [Password], [PremiumDays]) VALUES (1, N'1', N'1', 0)
GO
SET IDENTITY_INSERT [dbo].[Accounts] OFF
GO

INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (1, 101, 3, 1987, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (1, 102, 101, 2120, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (1, 103, 101, 2554, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (2, 101, 3, 1987, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (2, 102, 101, 2120, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (2, 103, 101, 2554, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (3, 101, 3, 1987, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (3, 102, 101, 2120, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (3, 103, 101, 2554, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (4, 101, 3, 1987, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (4, 102, 101, 2120, 1)
GO
INSERT [dbo].[PlayerItems] ([PlayerId], [SequenceId], [ParentId], [OpenTibiaId], [Count]) VALUES (4, 103, 101, 2554, 1)
GO

SET IDENTITY_INSERT [dbo].[Players] ON 
GO
INSERT [dbo].[Players] ([Id], [AccountId], [WorldId], [Name], [Health], [MaxHealth], [Direction], [BaseOutfitItemId], [BaseOutfitId], [BaseOutfitHead], [BaseOutfitBody], [BaseOutfitLegs], [BaseOutfitFeet], [BaseOutfitAddon], [OutfitItemId], [OutfitId], [OutfitHead], [OutfitBody], [OutfitLegs], [OutfitFeet], [OutfitAddon], [BaseSpeed], [Speed], [Invisible], [SkillMagicLevel], [SkillMagicLevelPercent], [SkillFist], [SkillFistPercent], [SkillClub], [SkillClubPercent], [SkillSword], [SkillSwordPercent], [SkillAxe], [SkillAxePercent], [SkillDistance], [SkillDistancePercent], [SkillShield], [SkillShieldPercent], [SkillFish], [SkillFishPercent], [Experience], [Level], [LevelPercent], [Mana], [MaxMana], [Soul], [Capacity], [Stamina], [Gender], [Vocation], [Rank], [SpawnX], [SpawnY], [SpawnZ], [TownX], [TownY], [TownZ]) VALUES (1, 1, 1, N'Gamemaster', 645, 645, 2, 0, 266, 0, 0, 0, 0, 0, 0, 266, 0, 0, 0, 0, 0, 2218, 2218, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15694800, 100, 0, 550, 550, 100, 139000, 2520, 0, 0, 2, 921, 771, 6, 921, 771, 6)
GO
INSERT [dbo].[Players] ([Id], [AccountId], [WorldId], [Name], [Health], [MaxHealth], [Direction], [BaseOutfitItemId], [BaseOutfitId], [BaseOutfitHead], [BaseOutfitBody], [BaseOutfitLegs], [BaseOutfitFeet], [BaseOutfitAddon], [OutfitItemId], [OutfitId], [OutfitHead], [OutfitBody], [OutfitLegs], [OutfitFeet], [OutfitAddon], [BaseSpeed], [Speed], [Invisible], [SkillMagicLevel], [SkillMagicLevelPercent], [SkillFist], [SkillFistPercent], [SkillClub], [SkillClubPercent], [SkillSword], [SkillSwordPercent], [SkillAxe], [SkillAxePercent], [SkillDistance], [SkillDistancePercent], [SkillShield], [SkillShieldPercent], [SkillFish], [SkillFishPercent], [Experience], [Level], [LevelPercent], [Mana], [MaxMana], [Soul], [Capacity], [Stamina], [Gender], [Vocation], [Rank],[SpawnX], [SpawnY], [SpawnZ], [TownX], [TownY], [TownZ]) VALUES (2, 1, 1, N'Knight', 1565, 1565, 2, 0, 131, 78, 69, 58, 76, 0, 0, 131, 78, 69, 58, 76, 0, 418, 418, 0, 4, 0, 0, 0, 0, 0, 90, 0, 0, 0, 0, 0, 80, 0, 0, 0, 15694800, 100, 0, 550, 550, 100, 277000, 2520, 0, 1, 0, 921, 771, 6, 921, 771, 6)
GO
INSERT [dbo].[Players] ([Id], [AccountId], [WorldId], [Name], [Health], [MaxHealth], [Direction], [BaseOutfitItemId], [BaseOutfitId], [BaseOutfitHead], [BaseOutfitBody], [BaseOutfitLegs], [BaseOutfitFeet], [BaseOutfitAddon], [OutfitItemId], [OutfitId], [OutfitHead], [OutfitBody], [OutfitLegs], [OutfitFeet], [OutfitAddon], [BaseSpeed], [Speed], [Invisible], [SkillMagicLevel], [SkillMagicLevelPercent], [SkillFist], [SkillFistPercent], [SkillClub], [SkillClubPercent], [SkillSword], [SkillSwordPercent], [SkillAxe], [SkillAxePercent], [SkillDistance], [SkillDistancePercent], [SkillShield], [SkillShieldPercent], [SkillFish], [SkillFishPercent], [Experience], [Level], [LevelPercent], [Mana], [MaxMana], [Soul], [Capacity], [Stamina], [Gender], [Vocation], [Rank],[SpawnX], [SpawnY], [SpawnZ], [TownX], [TownY], [TownZ]) VALUES (3, 1, 1, N'Paladin', 1105, 1105, 2, 0, 129, 78, 69, 58, 76, 0, 0, 129, 78, 69, 58, 76, 0, 418, 418, 0, 20, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 0, 40, 0, 0, 0, 15694800, 100, 0, 1470, 1470, 100, 231000, 2520, 0, 2, 0, 921, 771, 6, 921, 771, 6)
GO
INSERT [dbo].[Players] ([Id], [AccountId], [WorldId], [Name], [Health], [MaxHealth], [Direction], [BaseOutfitItemId], [BaseOutfitId], [BaseOutfitHead], [BaseOutfitBody], [BaseOutfitLegs], [BaseOutfitFeet], [BaseOutfitAddon], [OutfitItemId], [OutfitId], [OutfitHead], [OutfitBody], [OutfitLegs], [OutfitFeet], [OutfitAddon], [BaseSpeed], [Speed], [Invisible], [SkillMagicLevel], [SkillMagicLevelPercent], [SkillFist], [SkillFistPercent], [SkillClub], [SkillClubPercent], [SkillSword], [SkillSwordPercent], [SkillAxe], [SkillAxePercent], [SkillDistance], [SkillDistancePercent], [SkillShield], [SkillShieldPercent], [SkillFish], [SkillFishPercent], [Experience], [Level], [LevelPercent], [Mana], [MaxMana], [Soul], [Capacity], [Stamina], [Gender], [Vocation], [Rank],[SpawnX], [SpawnY], [SpawnZ], [TownX], [TownY], [TownZ]) VALUES (4, 1, 1, N'Sorcerer', 645, 645, 2, 0, 130, 78, 69, 58, 76, 0, 0, 130, 78, 69, 58, 76, 0, 418, 418, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15694800, 100, 0, 2850, 2850, 100, 139000, 2520, 0, 4, 0, 921, 771, 6, 921, 771, 6)
GO
INSERT [dbo].[Players] ([Id], [AccountId], [WorldId], [Name], [Health], [MaxHealth], [Direction], [BaseOutfitItemId], [BaseOutfitId], [BaseOutfitHead], [BaseOutfitBody], [BaseOutfitLegs], [BaseOutfitFeet], [BaseOutfitAddon], [OutfitItemId], [OutfitId], [OutfitHead], [OutfitBody], [OutfitLegs], [OutfitFeet], [OutfitAddon], [BaseSpeed], [Speed], [Invisible], [SkillMagicLevel], [SkillMagicLevelPercent], [SkillFist], [SkillFistPercent], [SkillClub], [SkillClubPercent], [SkillSword], [SkillSwordPercent], [SkillAxe], [SkillAxePercent], [SkillDistance], [SkillDistancePercent], [SkillShield], [SkillShieldPercent], [SkillFish], [SkillFishPercent], [Experience], [Level], [LevelPercent], [Mana], [MaxMana], [Soul], [Capacity], [Stamina], [Gender], [Vocation], [Rank],[SpawnX], [SpawnY], [SpawnZ], [TownX], [TownY], [TownZ]) VALUES (5, 1, 1, N'Druid', 645, 645, 2, 0, 130, 78, 69, 58, 76, 0, 0, 130, 78, 69, 58, 76, 0, 418, 418, 0, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 15694800, 100, 0, 2850, 2850, 100, 139000, 2520, 0, 3, 0, 921, 771, 6, 921, 771, 6)
GO
SET IDENTITY_INSERT [dbo].[Players] OFF
GO

SET IDENTITY_INSERT [dbo].[Worlds] ON 
GO
INSERT [dbo].[Worlds] ([Id], [Name], [Ip], [Port]) VALUES (1, N'Cormaya', N'127.0.0.1', 7172)
GO
SET IDENTITY_INSERT [dbo].[Worlds] OFF
GO

ALTER TABLE [dbo].[Bans]  WITH CHECK ADD  CONSTRAINT [FK_Bans_Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
GO
ALTER TABLE [dbo].[Bans] CHECK CONSTRAINT [FK_Bans_Accounts_AccountId]
GO
ALTER TABLE [dbo].[Bans]  WITH CHECK ADD  CONSTRAINT [FK_Bans_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
GO
ALTER TABLE [dbo].[Bans] CHECK CONSTRAINT [FK_Bans_Players_PlayerId]
GO

ALTER TABLE [dbo].[BugReports]  WITH CHECK ADD  CONSTRAINT [FK_BugReports_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[BugReports] CHECK CONSTRAINT [FK_BugReports_Players_PlayerId]
GO

ALTER TABLE [dbo].[DebugAsserts]  WITH CHECK ADD  CONSTRAINT [FK_DebugAsserts_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DebugAsserts] CHECK CONSTRAINT [FK_DebugAsserts_Players_PlayerId]
GO

ALTER TABLE [dbo].[PlayerDepotItems]  WITH CHECK ADD  CONSTRAINT [FK_PlayerDepotItems_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlayerDepotItems] CHECK CONSTRAINT [FK_PlayerDepotItems_Players_PlayerId]
GO

ALTER TABLE [dbo].[PlayerItems]  WITH CHECK ADD  CONSTRAINT [FK_PlayerItems_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlayerItems] CHECK CONSTRAINT [FK_PlayerItems_Players_PlayerId]
GO

ALTER TABLE [dbo].[Players]  WITH CHECK ADD  CONSTRAINT [FK_Players_Accounts_AccountId] FOREIGN KEY([AccountId])
REFERENCES [dbo].[Accounts] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Players] CHECK CONSTRAINT [FK_Players_Accounts_AccountId]
GO
ALTER TABLE [dbo].[Players]  WITH CHECK ADD  CONSTRAINT [FK_Players_Worlds_WorldId] FOREIGN KEY([WorldId])
REFERENCES [dbo].[Worlds] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Players] CHECK CONSTRAINT [FK_Players_Worlds_WorldId]
GO

ALTER TABLE [dbo].[PlayerStorages]  WITH CHECK ADD  CONSTRAINT [FK_PlayerStorages_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlayerStorages] CHECK CONSTRAINT [FK_PlayerStorages_Players_PlayerId]
GO

ALTER TABLE [dbo].[PlayerVips]  WITH CHECK ADD  CONSTRAINT [FK_PlayerVips_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PlayerVips] CHECK CONSTRAINT [FK_PlayerVips_Players_PlayerId]
GO
ALTER TABLE [dbo].[PlayerVips]  WITH CHECK ADD  CONSTRAINT [FK_PlayerVips_Players_VipId] FOREIGN KEY([VipId])
REFERENCES [dbo].[Players] ([Id])
GO
ALTER TABLE [dbo].[PlayerVips] CHECK CONSTRAINT [FK_PlayerVips_Players_VipId]
GO

ALTER TABLE [dbo].[RuleViolationReports]  WITH CHECK ADD  CONSTRAINT [FK_RuleViolationReports_Players_PlayerId] FOREIGN KEY([PlayerId])
REFERENCES [dbo].[Players] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[RuleViolationReports] CHECK CONSTRAINT [FK_RuleViolationReports_Players_PlayerId]
GO
