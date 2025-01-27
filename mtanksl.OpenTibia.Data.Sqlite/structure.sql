PRAGMA foreign_keys = off;
BEGIN TRANSACTION;

-- Accounts

CREATE TABLE Accounts (
  Id INTEGER CONSTRAINT PK_Accounts_Id PRIMARY KEY AUTOINCREMENT NOT NULL, 
  Name NVARCHAR (255) NOT NULL UNIQUE, 
  Password NVARCHAR (255) NOT NULL, 
  PremiumUntil DATETIME
);

INSERT INTO Accounts (Id, Name, Password, PremiumUntil) VALUES (1, '1', '1', NULL);

-- Bans

CREATE TABLE Bans (
  Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
  Type INTEGER NOT NULL, 
  AccountId INTEGER REFERENCES Accounts (Id) ON DELETE CASCADE, 
  PlayerId INTEGER REFERENCES Players (Id) ON DELETE CASCADE, 
  IpAddress NVARCHAR (255), 
  Message NVARCHAR (255) NOT NULL, 
  CreationDate DATETIME NOT NULL
);

-- BugReports

CREATE TABLE BugReports (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    PlayerId INTEGER REFERENCES Players (Id) ON DELETE CASCADE NOT NULL,
    PositionX INTEGER NOT NULL,
    PositionY INTEGER NOT NULL,
    PositionZ INTEGER NOT NULL,
    Message NVARCHAR (255) NOT NULL,
    CreationDate DATETIME NOT NULL
);
CREATE INDEX IX_BugReports_PlayerId ON BugReports (PlayerId);

-- DebugAsserts

CREATE TABLE DebugAsserts (
  Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
  PlayerId INTEGER REFERENCES Players (Id) ON DELETE CASCADE NOT NULL, 
  AssertLine NVARCHAR (255) NOT NULL, 
  ReportDate NVARCHAR (255) NOT NULL, 
  Description NVARCHAR (255) NOT NULL, 
  Comment NVARCHAR (255), 
  CreationDate DATETIME NOT NULL
);
CREATE INDEX IX_DebugAsserts_PlayerId ON DebugAsserts (PlayerId);

-- Motd

CREATE TABLE Motd (
  Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
  Message NVARCHAR (255) NOT NULL
);

-- PlayerDepotItems

CREATE TABLE PlayerDepotItems (
  PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE, 
  SequenceId INTEGER NOT NULL, 
  ParentId INTEGER NOT NULL, 
  OpenTibiaId INTEGER NOT NULL, 
  Count INTEGER NOT NULL, 
  PRIMARY KEY (PlayerId, SequenceId)
);
CREATE INDEX IX_PlayerDepotItems_PlayerId ON PlayerDepotItems (PlayerId);

-- PlayerItems

CREATE TABLE PlayerItems (
  PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE, 
  SequenceId INTEGER NOT NULL, 
  ParentId INTEGER NOT NULL, 
  OpenTibiaId INTEGER NOT NULL, 
  Count INTEGER NOT NULL, 
  PRIMARY KEY (PlayerId, SequenceId)
);
CREATE INDEX IX_PlayerItems_PlayerId ON PlayerItems (PlayerId);

INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (1, 101, 3, 1987, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (1, 102, 101, 2120, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (1, 103, 101, 2554, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (2, 101, 3, 1987, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (2, 102, 101, 2120, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (2, 103, 101, 2554, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (3, 101, 3, 1987, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (3, 102, 101, 2120, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (3, 103, 101, 2554, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (4, 101, 3, 1987, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (4, 102, 101, 2120, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (4, 103, 101, 2554, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (5, 101, 3, 1987, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (5, 102, 101, 2120, 1);
INSERT INTO PlayerItems (PlayerId, SequenceId, ParentId, OpenTibiaId, Count) VALUES (5, 103, 101, 2554, 1);

-- Players

CREATE TABLE Players (
  Id INTEGER CONSTRAINT PK_Players_Id PRIMARY KEY AUTOINCREMENT NOT NULL, 
  AccountId INTEGER NOT NULL REFERENCES Accounts (Id) ON DELETE CASCADE, 
  WorldId INTEGER NOT NULL REFERENCES Worlds (Id), 
  Name NVARCHAR (255) NOT NULL UNIQUE, 
  Health INTEGER NOT NULL, 
  MaxHealth INTEGER NOT NULL, 
  Direction INTEGER NOT NULL, 
  BaseOutfitItemId INTEGER NOT NULL, 
  BaseOutfitId INTEGER NOT NULL, 
  BaseOutfitHead INTEGER NOT NULL, 
  BaseOutfitBody INTEGER NOT NULL, 
  BaseOutfitLegs INTEGER NOT NULL, 
  BaseOutfitFeet INTEGER NOT NULL, 
  BaseOutfitAddon INTEGER NOT NULL, 
  OutfitItemId INTEGER NOT NULL, 
  OutfitId INTEGER NOT NULL, 
  OutfitHead INTEGER NOT NULL, 
  OutfitBody INTEGER NOT NULL, 
  OutfitLegs INTEGER NOT NULL, 
  OutfitFeet INTEGER NOT NULL, 
  OutfitAddon INTEGER NOT NULL, 
  BaseSpeed INTEGER NOT NULL, 
  Speed INTEGER NOT NULL, 
  Invisible BOOLEAN NOT NULL, 
  SkillMagicLevel INTEGER NOT NULL, 
  SkillMagicLevelPoints BIGINT NOT NULL, 
  SkillFist INTEGER NOT NULL, 
  SkillFistPoints BIGINT NOT NULL, 
  SkillClub INTEGER NOT NULL, 
  SkillClubPoints BIGINT NOT NULL, 
  SkillSword INTEGER NOT NULL, 
  SkillSwordPoints BIGINT NOT NULL, 
  SkillAxe INTEGER NOT NULL, 
  SkillAxePoints BIGINT NOT NULL, 
  SkillDistance INTEGER NOT NULL, 
  SkillDistancePoints BIGINT NOT NULL, 
  SkillShield INTEGER NOT NULL, 
  SkillShieldPoints BIGINT NOT NULL, 
  SkillFish INTEGER NOT NULL, 
  SkillFishPoints BIGINT NOT NULL, 
  Experience BIGINT NOT NULL, 
  Level INTEGER NOT NULL, 
  Mana INTEGER NOT NULL, 
  MaxMana INTEGER NOT NULL, 
  Soul INTEGER NOT NULL, 
  Capacity INTEGER NOT NULL, 
  Stamina INTEGER NOT NULL, 
  Gender INTEGER NOT NULL, 
  Vocation INTEGER NOT NULL, 
  Rank INTEGER NOT NULL, 
  SpawnX INTEGER NOT NULL, 
  SpawnY INTEGER NOT NULL, 
  SpawnZ INTEGER NOT NULL, 
  TownX INTEGER NOT NULL, 
  TownY INTEGER NOT NULL, 
  TownZ INTEGER NOT NULL,
  BankAccount BIGINT NOT NULL, 
);
CREATE INDEX IX_Players_AccountId ON Players (AccountId);
CREATE INDEX IX_Players_WorldId ON Players (WorldId);

INSERT INTO Players (Id, AccountId, WorldId, NAME, Health, MaxHealth, Direction, BaseOutfitItemId, BaseOutfitId, BaseOutfitHead, BaseOutfitBody, BaseOutfitLegs, BaseOutfitFeet, BaseOutfitAddon, OutfitItemId, OutfitId, OutfitHead, OutfitBody, OutfitLegs, OutfitFeet, OutfitAddon, BaseSpeed, Speed, Invisible, SkillMagicLevel, SkillMagicLevelPoints, SkillFist, SkillFistPoints, SkillClub, SkillClubPoints, SkillSword, SkillSwordPoints, SkillAxe, SkillAxePoints, SkillDistance, SkillDistancePoints, SkillShield, SkillShieldPoints, SkillFish, SkillFishPoints, Experience, Level, Mana, MaxMana, Soul, Capacity, Stamina, Gender, Vocation, Rank, SpawnX, SpawnY, SpawnZ, TownX, TownY, TownZ, BankAccount) VALUES (1, 1, 1, 'Gamemaster', 645, 645, 2, 0, 266, 0, 0, 0, 0, 0, 0, 266, 0, 0, 0, 0, 0, 2218, 2218, 0, 0,  0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 15694800, 100, 550, 550, 100, 139000, 2520, 0, 0, 2, 921, 771, 6, 921, 771, 6, 0);
INSERT INTO Players (Id, AccountId, WorldId, NAME, Health, MaxHealth, Direction, BaseOutfitItemId, BaseOutfitId, BaseOutfitHead, BaseOutfitBody, BaseOutfitLegs, BaseOutfitFeet, BaseOutfitAddon, OutfitItemId, OutfitId, OutfitHead, OutfitBody, OutfitLegs, OutfitFeet, OutfitAddon, BaseSpeed, Speed, Invisible, SkillMagicLevel, SkillMagicLevelPoints, SkillFist, SkillFistPoints, SkillClub, SkillClubPoints, SkillSword, SkillSwordPoints, SkillAxe, SkillAxePoints, SkillDistance, SkillDistancePoints, SkillShield, SkillShieldPoints, SkillFish, SkillFishPoints, Experience, Level, Mana, MaxMana, Soul, Capacity, Stamina, Gender, Vocation, Rank, SpawnX, SpawnY, SpawnZ, TownX, TownY, TownZ, BankAccount) VALUES (2, 1, 1, 'Knight', 1565, 1565, 2, 0, 131, 78, 69, 58, 76, 0, 0, 131, 78, 69, 58, 76, 0, 418, 418, 0, 4,  0, 10, 0, 10, 0, 90, 0, 10, 0, 10, 0, 80, 0, 10, 0, 15694800, 100, 550, 550, 100, 277000, 2520, 0, 1, 0, 921, 771, 6, 921, 771, 6, 0);
INSERT INTO Players (Id, AccountId, WorldId, NAME, Health, MaxHealth, Direction, BaseOutfitItemId, BaseOutfitId, BaseOutfitHead, BaseOutfitBody, BaseOutfitLegs, BaseOutfitFeet, BaseOutfitAddon, OutfitItemId, OutfitId, OutfitHead, OutfitBody, OutfitLegs, OutfitFeet, OutfitAddon, BaseSpeed, Speed, Invisible, SkillMagicLevel, SkillMagicLevelPoints, SkillFist, SkillFistPoints, SkillClub, SkillClubPoints, SkillSword, SkillSwordPoints, SkillAxe, SkillAxePoints, SkillDistance, SkillDistancePoints, SkillShield, SkillShieldPoints, SkillFish, SkillFishPoints, Experience, Level, Mana, MaxMana, Soul, Capacity, Stamina, Gender, Vocation, Rank, SpawnX, SpawnY, SpawnZ, TownX, TownY, TownZ, BankAccount) VALUES (3, 1, 1, 'Paladin', 1105, 1105, 2, 0, 129, 78, 69, 58, 76, 0, 0, 129, 78, 69, 58, 76, 0, 418, 418, 0, 20, 0, 10, 0, 10, 0, 10, 0, 10, 0, 70, 0, 40, 0, 10, 0, 15694800, 100, 1470, 1470, 100, 231000, 2520, 0, 2, 0, 921, 771, 6, 921, 771, 6, 0);
INSERT INTO Players (Id, AccountId, WorldId, NAME, Health, MaxHealth, Direction, BaseOutfitItemId, BaseOutfitId, BaseOutfitHead, BaseOutfitBody, BaseOutfitLegs, BaseOutfitFeet, BaseOutfitAddon, OutfitItemId, OutfitId, OutfitHead, OutfitBody, OutfitLegs, OutfitFeet, OutfitAddon, BaseSpeed, Speed, Invisible, SkillMagicLevel, SkillMagicLevelPoints, SkillFist, SkillFistPoints, SkillClub, SkillClubPoints, SkillSword, SkillSwordPoints, SkillAxe, SkillAxePoints, SkillDistance, SkillDistancePoints, SkillShield, SkillShieldPoints, SkillFish, SkillFishPoints, Experience, Level, Mana, MaxMana, Soul, Capacity, Stamina, Gender, Vocation, Rank, SpawnX, SpawnY, SpawnZ, TownX, TownY, TownZ, BankAccount) VALUES (4, 1, 1, 'Sorcerer', 645, 645, 2, 0, 130, 78, 69, 58, 76, 0, 0, 130, 78, 69, 58, 76, 0, 418, 418, 0, 70, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 15694800, 100, 2850, 2850, 100, 139000, 2520, 0, 4, 0, 921, 771, 6, 921, 771, 6, 0);
INSERT INTO Players (Id, AccountId, WorldId, NAME, Health, MaxHealth, Direction, BaseOutfitItemId, BaseOutfitId, BaseOutfitHead, BaseOutfitBody, BaseOutfitLegs, BaseOutfitFeet, BaseOutfitAddon, OutfitItemId, OutfitId, OutfitHead, OutfitBody, OutfitLegs, OutfitFeet, OutfitAddon, BaseSpeed, Speed, Invisible, SkillMagicLevel, SkillMagicLevelPoints, SkillFist, SkillFistPoints, SkillClub, SkillClubPoints, SkillSword, SkillSwordPoints, SkillAxe, SkillAxePoints, SkillDistance, SkillDistancePoints, SkillShield, SkillShieldPoints, SkillFish, SkillFishPoints, Experience, Level, Mana, MaxMana, Soul, Capacity, Stamina, Gender, Vocation, Rank, SpawnX, SpawnY, SpawnZ, TownX, TownY, TownZ, BankAccount) VALUES (5, 1, 1, 'Druid', 645, 645, 2, 0, 130, 78, 69, 58, 76, 0, 0, 130, 78, 69, 58, 76, 0, 418, 418, 0, 70, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 15694800, 100, 2850, 2850, 100, 139000, 2520, 0, 3, 0, 921, 771, 6, 921, 771, 6, 0);

-- PlayerStorages

CREATE TABLE PlayerStorages (
  PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE, 
  "Key" INTEGER NOT NULL, 
  Value INTEGER NOT NULL, 
  PRIMARY KEY (PlayerId, "Key")
);
CREATE INDEX IX_PlayerStorages_PlayerId ON PlayerStorages (PlayerId);

-- PlayerOutfits

CREATE TABLE PlayerOutfits (
    PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE,
    OutfitId INTEGER NOT NULL,
    OutfitAddon INTEGER NOT NULL,
    PRIMARY KEY (PlayerId, OutfitId)
);
CREATE INDEX IX_PlayerOutfits_PlayerId ON PlayerOutfits (PlayerId);

-- PlayerSpells

CREATE TABLE PlayerSpells (
    PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE,
    Name NVARCHAR (255) NOT NULL, 
    PRIMARY KEY (PlayerId, Name)
);
CREATE INDEX IX_PlayerSpells_PlayerId ON PlayerSpells (PlayerId);

-- PlayerBlesses

CREATE TABLE PlayerBlesses (
    PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE,
    Name NVARCHAR (255) NOT NULL,
    PRIMARY KEY (PlayerId, Name)
);
CREATE INDEX IX_PlayerBlesses_PlayerId ON PlayerBlesses (PlayerId);

-- PlayerAchievements

CREATE TABLE PlayerAchievements (
    PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE,
    Name NVARCHAR (255) NOT NULL, 
    PRIMARY KEY (PlayerId, Name)
);
CREATE INDEX IX_PlayerAchievements_PlayerId ON PlayerAchievements (PlayerId);

-- PlayerVips

CREATE TABLE PlayerVips (
  PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE, 
  VipId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE, 
  PRIMARY KEY (PlayerId, VipId)
);
CREATE INDEX IX_PlayerVips_PlayerId ON PlayerVips (PlayerId);
CREATE INDEX IX_PlayerVips_VipId ON PlayerVips (VipId);

-- PlayerKills

CREATE TABLE PlayerKills (
    Id INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
    PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE,
    TargetId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE,
    CreationDate DATETIME NOT NULL
);
CREATE INDEX IX_PlayerKills_PlayerId ON PlayerKills (PlayerId);
CREATE INDEX IX_PlayerKills_TargetId ON PlayerKills (TargetId);

-- PlayerDeaths

CREATE TABLE PlayerDeaths (
    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
    PlayerId INTEGER NOT NULL REFERENCES Players (Id) ON DELETE CASCADE,
    AttackerId INTEGER REFERENCES Players (Id) ON DELETE CASCADE,
    Name NVARCHAR (255),
    Level INTEGER NOT NULL,
    Unjustified  BOOLEAN NOT NULL,
    CreationDate DATETIME NOT NULL
);
CREATE INDEX IX_PlayerDeaths_PlayerId ON PlayerKills (PlayerId);
CREATE INDEX IX_PlayerDeaths_AttackerId ON PlayerKills (AttackerId);

-- RuleViolationReports

CREATE TABLE RuleViolationReports (
  Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
  PlayerId INTEGER REFERENCES Players (Id) ON DELETE CASCADE NOT NULL, 
  Type INTEGER NOT NULL, 
  RuleViolation INTEGER NOT NULL, 
  Name NVARCHAR (255) NOT NULL, 
  Comment NVARCHAR (255) NOT NULL, 
  Translation NVARCHAR (255), 
  Statment NVARCHAR (255), 
  CreationDate DATETIME NOT NULL
);
CREATE INDEX IX_RuleViolationReports_PlayerId ON RuleViolationReports (PlayerId);

-- Worlds

CREATE TABLE Worlds (
  Id INTEGER CONSTRAINT PK_Worlds_Id PRIMARY KEY AUTOINCREMENT NOT NULL, 
  Name NVARCHAR (255) NOT NULL, 
  Ip NVARCHAR (255) NOT NULL, 
  Port INTEGER NOT NULL
);

INSERT INTO Worlds (Id, NAME, Ip, Port) VALUES (1, 'Cormaya', '127.0.0.1', 7172);

-- Houses

CREATE TABLE Houses (
    Id INTEGER PRIMARY KEY NOT NULL,
    OwnerId INTEGER REFERENCES Players (Id) ON DELETE SET NULL
);
CREATE INDEX IX_Houses_OwnerId ON Houses (OwnerId);

-- HouseAccessLists

CREATE TABLE HouseAccessLists (
    HouseId INTEGER REFERENCES Houses (Id) ON DELETE CASCADE NOT NULL,
    ListId  INTEGER NOT NULL,
    Text TEXT NOT NULL,
    PRIMARY KEY ( HouseId, ListId)
);
CREATE INDEX IX_HouseAccessLists_HouseId ON HouseAccessLists (HouseId);

-- HouseItems

CREATE TABLE HouseItems (
    HouseId INTEGER NOT NULL REFERENCES Houses (Id) ON DELETE CASCADE,
    SequenceId BIGINT  NOT NULL,
    ParentId BIGINT  NOT NULL,
    OpenTibiaId INTEGER NOT NULL,
    Count INTEGER NOT NULL,
    PRIMARY KEY (HouseId, SequenceId)
);
CREATE INDEX IX_HouseItems_HouseId ON HouseItems (HouseId);

-- ServerStorages

CREATE TABLE ServerStorages (
    [Key] NVARCHAR (255) NOT NULL PRIMARY KEY,
    [Value] NVARCHAR (255) NOT NULL
);

INSERT INTO ServerStorages([Key],[Value]) VALUES ('PlayersPeek','0');

COMMIT TRANSACTION;
PRAGMA foreign_keys = ON;