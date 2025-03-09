START TRANSACTION;

CREATE TABLE "Accounts" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(255) NOT NULL,
    "Password" character varying(255) NOT NULL,
    "PremiumUntil" timestamp with time zone,
    CONSTRAINT "PK_Accounts" PRIMARY KEY ("Id")
);

CREATE TABLE "Motd" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Message" character varying(255) NOT NULL,
    CONSTRAINT "PK_Motd" PRIMARY KEY ("Id")
);

CREATE TABLE "ServerStorages" (
    "Key" character varying(255) NOT NULL,
    "Value" character varying(255) NOT NULL,
    CONSTRAINT "PK_ServerStorages" PRIMARY KEY ("Key")
);

CREATE TABLE "Worlds" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Name" character varying(255) NOT NULL,
    "Ip" character varying(255) NOT NULL,
    "Port" integer NOT NULL,
    CONSTRAINT "PK_Worlds" PRIMARY KEY ("Id")
);

CREATE TABLE "Players" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "AccountId" integer NOT NULL,
    "WorldId" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Health" integer NOT NULL,
    "MaxHealth" integer NOT NULL,
    "Direction" integer NOT NULL,
    "BaseOutfitItemId" integer NOT NULL,
    "BaseOutfitId" integer NOT NULL,
    "BaseOutfitHead" integer NOT NULL,
    "BaseOutfitBody" integer NOT NULL,
    "BaseOutfitLegs" integer NOT NULL,
    "BaseOutfitFeet" integer NOT NULL,
    "BaseOutfitAddon" integer NOT NULL,
    "OutfitItemId" integer NOT NULL,
    "OutfitId" integer NOT NULL,
    "OutfitHead" integer NOT NULL,
    "OutfitBody" integer NOT NULL,
    "OutfitLegs" integer NOT NULL,
    "OutfitFeet" integer NOT NULL,
    "OutfitAddon" integer NOT NULL,
    "BaseSpeed" integer NOT NULL,
    "Speed" integer NOT NULL,
    "Invisible" boolean NOT NULL,
    "SkillMagicLevel" integer NOT NULL,
    "SkillMagicLevelPoints" bigint NOT NULL,
    "SkillFist" integer NOT NULL,
    "SkillFistPoints" bigint NOT NULL,
    "SkillClub" integer NOT NULL,
    "SkillClubPoints" bigint NOT NULL,
    "SkillSword" integer NOT NULL,
    "SkillSwordPoints" bigint NOT NULL,
    "SkillAxe" integer NOT NULL,
    "SkillAxePoints" bigint NOT NULL,
    "SkillDistance" integer NOT NULL,
    "SkillDistancePoints" bigint NOT NULL,
    "SkillShield" integer NOT NULL,
    "SkillShieldPoints" bigint NOT NULL,
    "SkillFish" integer NOT NULL,
    "SkillFishPoints" bigint NOT NULL,
    "Experience" bigint NOT NULL,
    "Level" integer NOT NULL,
    "Mana" integer NOT NULL,
    "MaxMana" integer NOT NULL,
    "Soul" integer NOT NULL,
    "Capacity" integer NOT NULL,
    "Stamina" integer NOT NULL,
    "Gender" integer NOT NULL,
    "Vocation" integer NOT NULL,
    "Rank" integer NOT NULL,
    "SpawnX" integer NOT NULL,
    "SpawnY" integer NOT NULL,
    "SpawnZ" integer NOT NULL,
    "TownX" integer NOT NULL,
    "TownY" integer NOT NULL,
    "TownZ" integer NOT NULL,
    "BankAccount" bigint NOT NULL,
    CONSTRAINT "PK_Players" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Players_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Players_Worlds_WorldId" FOREIGN KEY ("WorldId") REFERENCES "Worlds" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Bans" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "Type" integer NOT NULL,
    "AccountId" integer,
    "PlayerId" integer,
    "IpAddress" character varying(255),
    "Message" character varying(255) NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Bans" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Bans_Accounts_AccountId" FOREIGN KEY ("AccountId") REFERENCES "Accounts" ("Id"),
    CONSTRAINT "FK_Bans_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id")
);

CREATE TABLE "BugReports" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "PlayerId" integer NOT NULL,
    "PositionX" integer NOT NULL,
    "PositionY" integer NOT NULL,
    "PositionZ" integer NOT NULL,
    "Message" character varying(255) NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_BugReports" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_BugReports_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "DebugAsserts" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "PlayerId" integer NOT NULL,
    "AssertLine" character varying(255) NOT NULL,
    "ReportDate" character varying(255) NOT NULL,
    "Description" character varying(255) NOT NULL,
    "Comment" character varying(255),
    "CreationDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_DebugAsserts" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_DebugAsserts_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Houses" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "OwnerId" integer,
    CONSTRAINT "PK_Houses" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Houses_Players_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Players" ("Id")
);

CREATE TABLE "PlayerAchievements" (
    "PlayerId" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    CONSTRAINT "PK_PlayerAchievements" PRIMARY KEY ("PlayerId", "Name"),
    CONSTRAINT "FK_PlayerAchievements_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerBlesses" (
    "PlayerId" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    CONSTRAINT "PK_PlayerBlesses" PRIMARY KEY ("PlayerId", "Name"),
    CONSTRAINT "FK_PlayerBlesses_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerDeaths" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "PlayerId" integer NOT NULL,
    "AttackerId" integer,
    "Name" character varying(255),
    "Level" integer NOT NULL,
    "Unjustified" boolean NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_PlayerDeaths" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_PlayerDeaths_Players_AttackerId" FOREIGN KEY ("AttackerId") REFERENCES "Players" ("Id") ON DELETE NO ACTION,
    CONSTRAINT "FK_PlayerDeaths_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerDepotItems" (
    "PlayerId" integer NOT NULL,
    "SequenceId" integer NOT NULL,
    "ParentId" integer NOT NULL,
    "OpenTibiaId" integer NOT NULL,
    "Count" integer NOT NULL,
    CONSTRAINT "PK_PlayerDepotItems" PRIMARY KEY ("PlayerId", "SequenceId"),
    CONSTRAINT "FK_PlayerDepotItems_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerItems" (
    "PlayerId" integer NOT NULL,
    "SequenceId" integer NOT NULL,
    "ParentId" integer NOT NULL,
    "OpenTibiaId" integer NOT NULL,
    "Count" integer NOT NULL,
    CONSTRAINT "PK_PlayerItems" PRIMARY KEY ("PlayerId", "SequenceId"),
    CONSTRAINT "FK_PlayerItems_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerKills" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "PlayerId" integer NOT NULL,
    "TargetId" integer NOT NULL,
    "Unjustified" boolean NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_PlayerKills" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_PlayerKills_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PlayerKills_Players_TargetId" FOREIGN KEY ("TargetId") REFERENCES "Players" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "PlayerOutfits" (
    "PlayerId" integer NOT NULL,
    "OutfitId" integer NOT NULL,
    "OutfitAddon" integer NOT NULL,
    CONSTRAINT "PK_PlayerOutfits" PRIMARY KEY ("PlayerId", "OutfitId"),
    CONSTRAINT "FK_PlayerOutfits_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerSpells" (
    "PlayerId" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    CONSTRAINT "PK_PlayerSpells" PRIMARY KEY ("PlayerId", "Name"),
    CONSTRAINT "FK_PlayerSpells_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerStorages" (
    "PlayerId" integer NOT NULL,
    "Key" integer NOT NULL,
    "Value" integer NOT NULL,
    CONSTRAINT "PK_PlayerStorages" PRIMARY KEY ("PlayerId", "Key"),
    CONSTRAINT "FK_PlayerStorages_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "PlayerVips" (
    "PlayerId" integer NOT NULL,
    "VipId" integer NOT NULL,
    CONSTRAINT "PK_PlayerVips" PRIMARY KEY ("PlayerId", "VipId"),
    CONSTRAINT "FK_PlayerVips_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_PlayerVips_Players_VipId" FOREIGN KEY ("VipId") REFERENCES "Players" ("Id") ON DELETE NO ACTION
);

CREATE TABLE "RuleViolationReports" (
    "Id" integer GENERATED BY DEFAULT AS IDENTITY,
    "PlayerId" integer NOT NULL,
    "Type" integer NOT NULL,
    "RuleViolation" integer NOT NULL,
    "Name" character varying(255) NOT NULL,
    "Comment" character varying(255) NOT NULL,
    "Translation" character varying(255),
    "Statment" character varying(255),
    "CreationDate" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_RuleViolationReports" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_RuleViolationReports_Players_PlayerId" FOREIGN KEY ("PlayerId") REFERENCES "Players" ("Id") ON DELETE CASCADE
);

CREATE TABLE "HouseAccessLists" (
    "HouseId" integer NOT NULL,
    "ListId" integer NOT NULL,
    "Text" text NOT NULL,
    CONSTRAINT "PK_HouseAccessLists" PRIMARY KEY ("HouseId", "ListId"),
    CONSTRAINT "FK_HouseAccessLists_Houses_HouseId" FOREIGN KEY ("HouseId") REFERENCES "Houses" ("Id") ON DELETE CASCADE
);

CREATE TABLE "HouseItems" (
    "HouseId" integer NOT NULL,
    "SequenceId" bigint NOT NULL,
    "ParentId" bigint NOT NULL,
    "OpenTibiaId" integer NOT NULL,
    "Count" integer NOT NULL,
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
VALUES (1, 1, 0, 0, 0, 0, 0, 75, 0, 0, 2218, 139000, 2, 15694800, 0, 645, FALSE, 100, 550, 645, 550, 'Gamemaster', 0, 0, 0, 0, 75, 0, 0, 2, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 0, 0, 10, 0, 10, 0, 100, 921, 771, 6, 2218, 2520, 921, 771, 6, 0, 1);
INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (2, 1, 0, 0, 0, 0, 0, 131, 0, 0, 418, 132900, 2, 15694800, 0, 1565, FALSE, 100, 550, 1565, 550, 'Knight', 0, 0, 0, 0, 131, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 4, 0, 80, 0, 90, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 1, 1);
INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (3, 1, 0, 0, 0, 0, 0, 129, 0, 0, 418, 132900, 2, 15694800, 0, 1105, FALSE, 100, 1470, 1105, 1470, 'Paladin', 0, 0, 0, 0, 129, 0, 0, 0, 10, 0, 10, 0, 70, 0, 10, 0, 10, 0, 15, 0, 40, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 2, 1);
INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (4, 1, 0, 0, 0, 0, 0, 130, 0, 0, 418, 132900, 2, 15694800, 0, 645, FALSE, 100, 2850, 645, 2850, 'Sorcerer', 0, 0, 0, 0, 130, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 60, 0, 10, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 4, 1);
INSERT INTO "Players" ("Id", "AccountId", "BankAccount", "BaseOutfitAddon", "BaseOutfitBody", "BaseOutfitFeet", "BaseOutfitHead", "BaseOutfitId", "BaseOutfitItemId", "BaseOutfitLegs", "BaseSpeed", "Capacity", "Direction", "Experience", "Gender", "Health", "Invisible", "Level", "Mana", "MaxHealth", "MaxMana", "Name", "OutfitAddon", "OutfitBody", "OutfitFeet", "OutfitHead", "OutfitId", "OutfitItemId", "OutfitLegs", "Rank", "SkillAxe", "SkillAxePoints", "SkillClub", "SkillClubPoints", "SkillDistance", "SkillDistancePoints", "SkillFish", "SkillFishPoints", "SkillFist", "SkillFistPoints", "SkillMagicLevel", "SkillMagicLevelPoints", "SkillShield", "SkillShieldPoints", "SkillSword", "SkillSwordPoints", "Soul", "SpawnX", "SpawnY", "SpawnZ", "Speed", "Stamina", "TownX", "TownY", "TownZ", "Vocation", "WorldId")
VALUES (5, 1, 0, 0, 0, 0, 0, 130, 0, 0, 418, 132900, 2, 15694800, 0, 645, FALSE, 100, 2850, 645, 2850, 'Druid', 0, 0, 0, 0, 130, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 0, 60, 10, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 3, 1);

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

SELECT setval(
    pg_get_serial_sequence('"Accounts"', 'Id'),
    GREATEST(
        (SELECT MAX("Id") FROM "Accounts") + 1,
        nextval(pg_get_serial_sequence('"Accounts"', 'Id'))),
    false);
SELECT setval(
    pg_get_serial_sequence('"Motd"', 'Id'),
    GREATEST(
        (SELECT MAX("Id") FROM "Motd") + 1,
        nextval(pg_get_serial_sequence('"Motd"', 'Id'))),
    false);
SELECT setval(
    pg_get_serial_sequence('"Worlds"', 'Id'),
    GREATEST(
        (SELECT MAX("Id") FROM "Worlds") + 1,
        nextval(pg_get_serial_sequence('"Worlds"', 'Id'))),
    false);
SELECT setval(
    pg_get_serial_sequence('"Players"', 'Id'),
    GREATEST(
        (SELECT MAX("Id") FROM "Players") + 1,
        nextval(pg_get_serial_sequence('"Players"', 'Id'))),
    false);

COMMIT;

--

START TRANSACTION;

ALTER TABLE "RuleViolationReports" ADD "StatmentDate" timestamp with time zone;

ALTER TABLE "RuleViolationReports" ADD "StatmentPlayerId" integer;

CREATE INDEX "IX_RuleViolationReports_StatmentPlayerId" ON "RuleViolationReports" ("StatmentPlayerId");

ALTER TABLE "RuleViolationReports" ADD CONSTRAINT "FK_RuleViolationReports_Players_StatmentPlayerId" FOREIGN KEY ("StatmentPlayerId") REFERENCES "Players" ("Id");

COMMIT;

--

START TRANSACTION;

ALTER TABLE "Players" ADD "MaxCapacity" integer NOT NULL DEFAULT 0;

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

START TRANSACTION;

ALTER TABLE "Players" DROP COLUMN "OutfitAddon";

ALTER TABLE "Players" DROP COLUMN "OutfitBody";

ALTER TABLE "Players" DROP COLUMN "OutfitFeet";

ALTER TABLE "Players" DROP COLUMN "OutfitHead";

ALTER TABLE "Players" DROP COLUMN "OutfitId";

ALTER TABLE "Players" DROP COLUMN "OutfitItemId";

ALTER TABLE "Players" DROP COLUMN "OutfitLegs";

ALTER TABLE "Players" DROP COLUMN "Speed";

COMMIT;

--

START TRANSACTION;

ALTER TABLE "PlayerItems" ADD "Attributes" bytea;

ALTER TABLE "PlayerDepotItems" ADD "Attributes" bytea;

ALTER TABLE "HouseItems" ADD "Attributes" bytea;

COMMIT;