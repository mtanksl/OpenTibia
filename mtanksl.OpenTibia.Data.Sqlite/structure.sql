BEGIN TRANSACTION;

CREATE TABLE "Accounts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Accounts" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Password" TEXT NOT NULL,
    "PremiumUntil" TEXT NULL
);

CREATE TABLE "Motd" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Motd" PRIMARY KEY AUTOINCREMENT,
    "Message" TEXT NOT NULL
);

CREATE TABLE "ServerStorages" (
    "Key" TEXT NOT NULL CONSTRAINT "PK_ServerStorages" PRIMARY KEY,
    "Value" TEXT NOT NULL
);

CREATE TABLE "Worlds" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Worlds" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Ip" TEXT NOT NULL,
    "Port" INTEGER NOT NULL
);

CREATE TABLE "Players" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Players" PRIMARY KEY AUTOINCREMENT,
    "AccountId" INTEGER NOT NULL,
    "WorldId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Health" INTEGER NOT NULL,
    "MaxHealth" INTEGER NOT NULL,
    "Direction" INTEGER NOT NULL,
    "BaseOutfitItemId" INTEGER NOT NULL,
    "BaseOutfitId" INTEGER NOT NULL,
    "BaseOutfitHead" INTEGER NOT NULL,
    "BaseOutfitBody" INTEGER NOT NULL,
    "BaseOutfitLegs" INTEGER NOT NULL,
    "BaseOutfitFeet" INTEGER NOT NULL,
    "BaseOutfitAddon" INTEGER NOT NULL,
    "OutfitItemId" INTEGER NOT NULL,
    "OutfitId" INTEGER NOT NULL,
    "OutfitHead" INTEGER NOT NULL,
    "OutfitBody" INTEGER NOT NULL,
    "OutfitLegs" INTEGER NOT NULL,
    "OutfitFeet" INTEGER NOT NULL,
    "OutfitAddon" INTEGER NOT NULL,
    "BaseSpeed" INTEGER NOT NULL,
    "Speed" INTEGER NOT NULL,
    "Invisible" INTEGER NOT NULL,
    "SkillMagicLevel" INTEGER NOT NULL,
    "SkillMagicLevelPoints" INTEGER NOT NULL,
    "SkillFist" INTEGER NOT NULL,
    "SkillFistPoints" INTEGER NOT NULL,
    "SkillClub" INTEGER NOT NULL,
    "SkillClubPoints" INTEGER NOT NULL,
    "SkillSword" INTEGER NOT NULL,
    "SkillSwordPoints" INTEGER NOT NULL,
    "SkillAxe" INTEGER NOT NULL,
    "SkillAxePoints" INTEGER NOT NULL,
    "SkillDistance" INTEGER NOT NULL,
    "SkillDistancePoints" INTEGER NOT NULL,
    "SkillShield" INTEGER NOT NULL,
    "SkillShieldPoints" INTEGER NOT NULL,
    "SkillFish" INTEGER NOT NULL,
    "SkillFishPoints" INTEGER NOT NULL,
    "Experience" INTEGER NOT NULL,
    "Level" INTEGER NOT NULL,
    "Mana" INTEGER NOT NULL,
    "MaxMana" INTEGER NOT NULL,
    "Soul" INTEGER NOT NULL,
    "Capacity" INTEGER NOT NULL,
    "Stamina" INTEGER NOT NULL,
    "Gender" INTEGER NOT NULL,
    "Vocation" INTEGER NOT NULL,
    "Rank" INTEGER NOT NULL,
    "SpawnX" INTEGER NOT NULL,
    "SpawnY" INTEGER NOT NULL,
    "SpawnZ" INTEGER NOT NULL,
    "TownX" INTEGER NOT NULL,
    "TownY" INTEGER NOT NULL,
    "TownZ" INTEGER NOT NULL,
    "BankAccount" INTEGER NOT NULL,
    CONSTRAINT "FK_Players_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Players_Worlds_WorldId" FOREIGN KEY ("WorldId") REFERENCES "Worlds" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Bans" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Bans" PRIMARY KEY AUTOINCREMENT,
    "Type" INTEGER NOT NULL,
    "AccountId" INTEGER NULL,
    "PlayerId" INTEGER NULL,
    "IpAddress" TEXT NULL,
    "Message" TEXT NOT NULL,
    "CreationDate" TEXT NOT NULL,
    CONSTRAINT "FK_Bans_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id"),
    CONSTRAINT "FK_Bans_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id")
);

CREATE TABLE "BugReports" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_BugReports" PRIMARY KEY AUTOINCREMENT,
    "PlayerId" INTEGER NOT NULL,
    "PositionX" INTEGER NOT NULL,
    "PositionY" INTEGER NOT NULL,
    "PositionZ" INTEGER NOT NULL,
    "Message" TEXT NOT NULL,
    "CreationDate" TEXT NOT NULL,
    CONSTRAINT "FK_BugReports_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "DebugAsserts" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_DebugAsserts" PRIMARY KEY AUTOINCREMENT,
    "PlayerId" INTEGER NOT NULL,
    "AssertLine" TEXT NOT NULL,
    "ReportDate" TEXT NOT NULL,
    "Description" TEXT NOT NULL,
    "Comment" TEXT NULL,
    "CreationDate" TEXT NOT NULL,
    CONSTRAINT "FK_DebugAsserts_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Houses" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Houses" PRIMARY KEY AUTOINCREMENT,
    "OwnerId" INTEGER NULL,
    CONSTRAINT "FK_Houses_Players_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Players" ("Id")
);

CREATE TABLE "PlayerAchievements" (
    "PlayerId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    CONSTRAINT "PK_PlayerAchievements" PRIMARY KEY ("PlayerId", "Name"),
    CONSTRAINT "FK_PlayerAchievements_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerBlesses" (
    "PlayerId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    CONSTRAINT "PK_PlayerBlesses" PRIMARY KEY ("PlayerId", "Name"),
    CONSTRAINT "FK_PlayerBlesses_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerDeaths" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PlayerDeaths" PRIMARY KEY AUTOINCREMENT,
    "PlayerId" INTEGER NOT NULL,
    "AttackerId" INTEGER NULL,
    "Name" TEXT NULL,
    "Level" INTEGER NOT NULL,
    "Unjustified" INTEGER NOT NULL,
    "CreationDate" TEXT NOT NULL,
    CONSTRAINT "FK_PlayerDeaths_Players_AttackerId" FOREIGN KEY ("AttackerId") REFERENCES "Players" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_PlayerDeaths_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerDepotItems" (
    "PlayerId" INTEGER NOT NULL,
    "SequenceId" INTEGER NOT NULL,
    "ParentId" INTEGER NOT NULL,
    "OpenTibiaId" INTEGER NOT NULL,
    "Count" INTEGER NOT NULL,
    CONSTRAINT "PK_PlayerDepotItems" PRIMARY KEY ("PlayerId", "SequenceId"),
    CONSTRAINT "FK_PlayerDepotItems_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerItems" (
    "PlayerId" INTEGER NOT NULL,
    "SequenceId" INTEGER NOT NULL,
    "ParentId" INTEGER NOT NULL,
    "OpenTibiaId" INTEGER NOT NULL,
    "Count" INTEGER NOT NULL,
    CONSTRAINT "PK_PlayerItems" PRIMARY KEY ("PlayerId", "SequenceId"),
    CONSTRAINT "FK_PlayerItems_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerKills" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_PlayerKills" PRIMARY KEY AUTOINCREMENT,
    "PlayerId" INTEGER NOT NULL,
    "TargetId" INTEGER NOT NULL,
    "Unjustified" INTEGER NOT NULL,
    "CreationDate" TEXT NOT NULL,
    CONSTRAINT "FK_PlayerKills_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PlayerKills_Players_TargetId" FOREIGN KEY ("TargetId") REFERENCES "Players" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "PlayerOutfits" (
    "PlayerId" INTEGER NOT NULL,
    "OutfitId" INTEGER NOT NULL,
    "OutfitAddon" INTEGER NOT NULL,
    CONSTRAINT "PK_PlayerOutfits" PRIMARY KEY ("PlayerId", "OutfitId"),
    CONSTRAINT "FK_PlayerOutfits_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerSpells" (
    "PlayerId" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    CONSTRAINT "PK_PlayerSpells" PRIMARY KEY ("PlayerId", "Name"),
    CONSTRAINT "FK_PlayerSpells_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerStorages" (
    "PlayerId" INTEGER NOT NULL,
    "Key" INTEGER NOT NULL,
    "Value" INTEGER NOT NULL,
    CONSTRAINT "PK_PlayerStorages" PRIMARY KEY ("PlayerId", "Key"),
    CONSTRAINT "FK_PlayerStorages_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerVips" (
    "PlayerId" INTEGER NOT NULL,
    "VipId" INTEGER NOT NULL,
    CONSTRAINT "PK_PlayerVips" PRIMARY KEY ("PlayerId", "VipId"),
    CONSTRAINT "FK_PlayerVips_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PlayerVips_Players_VipId" FOREIGN KEY ("VipId") REFERENCES "Players" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "RuleViolationReports" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_RuleViolationReports" PRIMARY KEY AUTOINCREMENT,
    "PlayerId" INTEGER NOT NULL,
    "Type" INTEGER NOT NULL,
    "RuleViolation" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Comment" TEXT NOT NULL,
    "Translation" TEXT NULL,
    "Statment" TEXT NULL,
    "CreationDate" TEXT NOT NULL,
    CONSTRAINT "FK_RuleViolationReports_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "HouseAccessLists" (
    "HouseId" INTEGER NOT NULL,
    "ListId" INTEGER NOT NULL,
    "Text" TEXT NOT NULL,
    CONSTRAINT "PK_HouseAccessLists" PRIMARY KEY ("HouseId", "ListId"),
    CONSTRAINT "FK_HouseAccessLists_Houses_HouseId" FOREIGN KEY ("HouseId") REFERENCES "Houses" ("Id") ON DELETE CASCADE
);

CREATE TABLE "HouseItems" (
    "HouseId" INTEGER NOT NULL,
    "SequenceId" INTEGER NOT NULL,
    "ParentId" INTEGER NOT NULL,
    "OpenTibiaId" INTEGER NOT NULL,
    "Count" INTEGER NOT NULL,
    CONSTRAINT "PK_HouseItems" PRIMARY KEY ("HouseId", "SequenceId"),
    CONSTRAINT "FK_HouseItems_Houses_HouseId" FOREIGN KEY ("HouseId") REFERENCES "Houses" ("Id") ON DELETE CASCADE
);

INSERT INTO "Accounts" ("Id", "Name", "Password", "PremiumUntil")
VALUES (1, '1', '1', NULL);

INSERT INTO "Motd" ("Id", "Message")
VALUES (1, 'MTOTS - An open Tibia server developed by mtanksl');

INSERT INTO "ServerStorages" ("Key", "Value")
VALUES ('PlayersPeek', '0');

INSERT INTO "Worlds" ("Id", "Ip", "Name", "Port")
VALUES (1, '127.0.0.1', 'Cormaya', 7172);

INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (1, 1, 0, 0, 0, 0, 0, 266, 0, 0, 2218, 139000, 2, 15694800, 0, 645, 0, 100, 550, 645, 550, 'Gamemaster', 0, 0, 0, 0, 266, 0, 0, 2, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 0, 0, 10, 0, 10, 0, 100, 921, 771, 6, 2218, 2520, 921, 771, 6, 0, 1);

INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (2, 1, 0, 0, 0, 0, 0, 131, 0, 0, 418, 132900, 2, 15694800, 0, 1565, 0, 100, 550, 1565, 550, 'Knight', 0, 0, 0, 0, 131, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 4, 0, 80, 0, 90, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 1, 1);

INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (3, 1, 0, 0, 0, 0, 0, 129, 0, 0, 418, 132900, 2, 15694800, 0, 1105, 0, 100, 1470, 1105, 1470, 'Paladin', 0, 0, 0, 0, 129, 0, 0, 0, 10, 0, 10, 0, 70, 0, 10, 0, 10, 0, 15, 0, 40, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 2, 1);

INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (4, 1, 0, 0, 0, 0, 0, 130, 0, 0, 418, 132900, 2, 15694800, 0, 645, 0, 100, 2850, 645, 2850, 'Sorcerer', 0, 0, 0, 0, 130, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 60, 0, 10, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 4, 1);

INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (5, 1, 0, 0, 0, 0, 0, 130, 0, 0, 418, 132900, 2, 15694800, 0, 645, 0, 100, 2850, 645, 2850, 'Druid', 0, 0, 0, 0, 130, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 60, 0, 10, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 3, 1);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (1, 101, 1, 1987, 3);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (1, 102, 1, 2120, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (1, 103, 1, 2554, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (2, 101, 1, 1987, 3);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (2, 102, 1, 2120, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (2, 103, 1, 2554, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (3, 101, 1, 1987, 3);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (3, 102, 1, 2120, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (3, 103, 1, 2554, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (4, 101, 1, 1987, 3);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (4, 102, 1, 2120, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (4, 103, 1, 2554, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (5, 101, 1, 1987, 3);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (5, 102, 1, 2120, 101);

INSERT INTO "PlayerItems" ("PlayerId", "SequenceId", "Count", "OpenTibiaId", "ParentId")
VALUES (5, 103, 1, 2554, 101);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (128, 1, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (129, 1, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (130, 1, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (131, 1, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (128, 2, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (129, 2, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (130, 2, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (131, 2, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (128, 3, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (129, 3, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (130, 3, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (131, 3, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (128, 4, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (129, 4, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (130, 4, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (131, 4, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (128, 5, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (129, 5, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (130, 5, 0);

INSERT INTO "PlayerOutfits" ("OutfitId", "PlayerId", "OutfitAddon")
VALUES (131, 5, 0);

CREATE INDEX "IX_Bans_AccountId" ON "Bans" ("AccountId");

CREATE INDEX "IX_Bans_PlayerId" ON "Bans" ("PlayerId");

CREATE INDEX "IX_BugReports_PlayerId" ON "BugReports" ("PlayerId");

CREATE INDEX "IX_DebugAsserts_PlayerId" ON "DebugAsserts" ("PlayerId");

CREATE INDEX "IX_Houses_OwnerId" ON "Houses" ("OwnerId");

CREATE INDEX "IX_PlayerDeaths_AttackerId" ON "PlayerDeaths" ("AttackerId");

CREATE INDEX "IX_PlayerDeaths_PlayerId" ON "PlayerDeaths" ("PlayerId");

CREATE INDEX "IX_PlayerKills_PlayerId" ON "PlayerKills" ("PlayerId");

CREATE INDEX "IX_PlayerKills_TargetId" ON "PlayerKills" ("TargetId");

CREATE INDEX "IX_Players_AccountId" ON "Players" ("AccountId");

CREATE INDEX "IX_Players_WorldId" ON "Players" ("WorldId");

CREATE INDEX "IX_PlayerVips_VipId" ON "PlayerVips" ("VipId");

CREATE INDEX "IX_RuleViolationReports_PlayerId" ON "RuleViolationReports" ("PlayerId");

COMMIT;

--

BEGIN TRANSACTION;

ALTER TABLE "RuleViolationReports" ADD "StatmentDate" TEXT NULL;

ALTER TABLE "RuleViolationReports" ADD "StatmentPlayerId" INTEGER NULL;

CREATE INDEX "IX_RuleViolationReports_StatmentPlayerId" ON "RuleViolationReports" ("StatmentPlayerId");

CREATE TABLE "ef_temp_RuleViolationReports" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_RuleViolationReports" PRIMARY KEY AUTOINCREMENT,
    "Comment" TEXT NOT NULL,
    "CreationDate" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "PlayerId" INTEGER NOT NULL,
    "RuleViolation" INTEGER NOT NULL,
    "Statment" TEXT NULL,
    "StatmentDate" TEXT NULL,
    "StatmentPlayerId" INTEGER NULL,
    "Translation" TEXT NULL,
    "Type" INTEGER NOT NULL,
    CONSTRAINT "FK_RuleViolationReports_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_RuleViolationReports_Players_StatmentPlayerId" FOREIGN KEY ("StatmentPlayerId") REFERENCES "Players" ("Id")
);

INSERT INTO "ef_temp_RuleViolationReports" ("Id", "Comment", "CreationDate", "Name", "PlayerId", "RuleViolation", "Statment", "StatmentDate", "StatmentPlayerId", "Translation", "Type")
SELECT "Id", "Comment", "CreationDate", "Name", "PlayerId", "RuleViolation", "Statment", "StatmentDate", "StatmentPlayerId", "Translation", "Type"
FROM "RuleViolationReports";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "RuleViolationReports";

ALTER TABLE "ef_temp_RuleViolationReports" RENAME TO "RuleViolationReports";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_RuleViolationReports_PlayerId" ON "RuleViolationReports" ("PlayerId");

CREATE INDEX "IX_RuleViolationReports_StatmentPlayerId" ON "RuleViolationReports" ("StatmentPlayerId");

COMMIT;

--

BEGIN TRANSACTION;

ALTER TABLE "Players" ADD "MaxCapacity" INTEGER NOT NULL DEFAULT 0;

UPDATE "Players" SET "MaxCapacity" = 139000
WHERE "Id" = 1;

UPDATE "Players" SET "MaxCapacity" = 139000
WHERE "Id" = 2;

UPDATE "Players" SET "MaxCapacity" = 139000
WHERE "Id" = 3;

UPDATE "Players" SET "MaxCapacity" = 139000
WHERE "Id" = 4;

UPDATE "Players" SET "MaxCapacity" = 139000
WHERE "Id" = 5;

COMMIT;

--

BEGIN TRANSACTION;

CREATE TABLE "ef_temp_Players" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Players" PRIMARY KEY AUTOINCREMENT,
    "AccountId" INTEGER NOT NULL,
    "BankAccount" INTEGER NOT NULL,
    "BaseOutfitAddon" INTEGER NOT NULL,
    "BaseOutfitBody" INTEGER NOT NULL,
    "BaseOutfitFeet" INTEGER NOT NULL,
    "BaseOutfitHead" INTEGER NOT NULL,
    "BaseOutfitId" INTEGER NOT NULL,
    "BaseOutfitItemId" INTEGER NOT NULL,
    "BaseOutfitLegs" INTEGER NOT NULL,
    "BaseSpeed" INTEGER NOT NULL,
    "Capacity" INTEGER NOT NULL,
    "Direction" INTEGER NOT NULL,
    "Experience" INTEGER NOT NULL,
    "Gender" INTEGER NOT NULL,
    "Health" INTEGER NOT NULL,
    "Invisible" INTEGER NOT NULL,
    "Level" INTEGER NOT NULL,
    "Mana" INTEGER NOT NULL,
    "MaxCapacity" INTEGER NOT NULL,
    "MaxHealth" INTEGER NOT NULL,
    "MaxMana" INTEGER NOT NULL,
    "Name" TEXT NOT NULL,
    "Rank" INTEGER NOT NULL,
    "SkillAxe" INTEGER NOT NULL,
    "SkillAxePoints" INTEGER NOT NULL,
    "SkillClub" INTEGER NOT NULL,
    "SkillClubPoints" INTEGER NOT NULL,
    "SkillDistance" INTEGER NOT NULL,
    "SkillDistancePoints" INTEGER NOT NULL,
    "SkillFish" INTEGER NOT NULL,
    "SkillFishPoints" INTEGER NOT NULL,
    "SkillFist" INTEGER NOT NULL,
    "SkillFistPoints" INTEGER NOT NULL,
    "SkillMagicLevel" INTEGER NOT NULL,
    "SkillMagicLevelPoints" INTEGER NOT NULL,
    "SkillShield" INTEGER NOT NULL,
    "SkillShieldPoints" INTEGER NOT NULL,
    "SkillSword" INTEGER NOT NULL,
    "SkillSwordPoints" INTEGER NOT NULL,
    "Soul" INTEGER NOT NULL,
    "SpawnX" INTEGER NOT NULL,
    "SpawnY" INTEGER NOT NULL,
    "SpawnZ" INTEGER NOT NULL,
    "Stamina" INTEGER NOT NULL,
    "TownX" INTEGER NOT NULL,
    "TownY" INTEGER NOT NULL,
    "TownZ" INTEGER NOT NULL,
    "Vocation" INTEGER NOT NULL,
    "WorldId" INTEGER NOT NULL,
    CONSTRAINT "FK_Players_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Players_Worlds_WorldId" FOREIGN KEY ("WorldId") REFERENCES "Worlds" ("Id") ON DELETE CASCADE
);

INSERT INTO "ef_temp_Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxCapacity", "MaxHealth", "MaxMana", "Name", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
SELECT "Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxCapacity", "MaxHealth", "MaxMana", "Name", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId"
FROM "Players";

COMMIT;

PRAGMA foreign_keys = 0;

BEGIN TRANSACTION;

DROP TABLE "Players";

ALTER TABLE "ef_temp_Players" RENAME TO "Players";

COMMIT;

PRAGMA foreign_keys = 1;

BEGIN TRANSACTION;

CREATE INDEX "IX_Players_AccountId" ON "Players" ("AccountId");

CREATE INDEX "IX_Players_WorldId" ON "Players" ("WorldId");

COMMIT;