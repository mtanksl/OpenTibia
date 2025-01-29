START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Accounts` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Password` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `PremiumUntil` datetime(6) NULL,
    CONSTRAINT `PK_Accounts` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Motd` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Message` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_Motd` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `ServerStorages` (
    `Key` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Value` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_ServerStorages` PRIMARY KEY (`Key`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Worlds` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Ip` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Port` int NOT NULL,
    CONSTRAINT `PK_Worlds` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `Players` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `AccountId` int NOT NULL,
    `WorldId` int NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Health` int NOT NULL,
    `MaxHealth` int NOT NULL,
    `Direction` int NOT NULL,
    `BaseOutfitItemId` int NOT NULL,
    `BaseOutfitId` int NOT NULL,
    `BaseOutfitHead` int NOT NULL,
    `BaseOutfitBody` int NOT NULL,
    `BaseOutfitLegs` int NOT NULL,
    `BaseOutfitFeet` int NOT NULL,
    `BaseOutfitAddon` int NOT NULL,
    `OutfitItemId` int NOT NULL,
    `OutfitId` int NOT NULL,
    `OutfitHead` int NOT NULL,
    `OutfitBody` int NOT NULL,
    `OutfitLegs` int NOT NULL,
    `OutfitFeet` int NOT NULL,
    `OutfitAddon` int NOT NULL,
    `BaseSpeed` int NOT NULL,
    `Speed` int NOT NULL,
    `Invisible` tinyint(1) NOT NULL,
    `SkillMagicLevel` int NOT NULL,
    `SkillMagicLevelPoints` bigint NOT NULL,
    `SkillFist` int NOT NULL,
    `SkillFistPoints` bigint NOT NULL,
    `SkillClub` int NOT NULL,
    `SkillClubPoints` bigint NOT NULL,
    `SkillSword` int NOT NULL,
    `SkillSwordPoints` bigint NOT NULL,
    `SkillAxe` int NOT NULL,
    `SkillAxePoints` bigint NOT NULL,
    `SkillDistance` int NOT NULL,
    `SkillDistancePoints` bigint NOT NULL,
    `SkillShield` int NOT NULL,
    `SkillShieldPoints` bigint NOT NULL,
    `SkillFish` int NOT NULL,
    `SkillFishPoints` bigint NOT NULL,
    `Experience` bigint NOT NULL,
    `Level` int NOT NULL,
    `Mana` int NOT NULL,
    `MaxMana` int NOT NULL,
    `Soul` int NOT NULL,
    `Capacity` int NOT NULL,
    `Stamina` int NOT NULL,
    `Gender` int NOT NULL,
    `Vocation` int NOT NULL,
    `Rank` int NOT NULL,
    `SpawnX` int NOT NULL,
    `SpawnY` int NOT NULL,
    `SpawnZ` int NOT NULL,
    `TownX` int NOT NULL,
    `TownY` int NOT NULL,
    `TownZ` int NOT NULL,
    `BankAccount` bigint NOT NULL,
    CONSTRAINT `PK_Players` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Players_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_Players_Worlds_WorldId` FOREIGN KEY (`WorldId`) REFERENCES `Worlds` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Bans` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Type` int NOT NULL,
    `AccountId` int NULL,
    `PlayerId` int NULL,
    `IpAddress` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Message` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreationDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_Bans` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Bans_Accounts_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`),
    CONSTRAINT `FK_Bans_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `BugReports` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PlayerId` int NOT NULL,
    `PositionX` int NOT NULL,
    `PositionY` int NOT NULL,
    `PositionZ` int NOT NULL,
    `Message` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `CreationDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_BugReports` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_BugReports_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `DebugAsserts` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PlayerId` int NOT NULL,
    `AssertLine` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `ReportDate` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Description` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Comment` varchar(255) CHARACTER SET utf8mb4 NULL,
    `CreationDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_DebugAsserts` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_DebugAsserts_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `Houses` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `OwnerId` int NULL,
    CONSTRAINT `PK_Houses` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_Houses_Players_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `Players` (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerAchievements` (
    `PlayerId` int NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_PlayerAchievements` PRIMARY KEY (`PlayerId`, `Name`),
    CONSTRAINT `FK_PlayerAchievements_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerBlesses` (
    `PlayerId` int NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_PlayerBlesses` PRIMARY KEY (`PlayerId`, `Name`),
    CONSTRAINT `FK_PlayerBlesses_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerDeaths` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PlayerId` int NOT NULL,
    `AttackerId` int NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Level` int NOT NULL,
    `Unjustified` tinyint(1) NOT NULL,
    `CreationDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_PlayerDeaths` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PlayerDeaths_Players_AttackerId` FOREIGN KEY (`AttackerId`) REFERENCES `Players` (`Id`) ON DELETE NO ACTION,
    CONSTRAINT `FK_PlayerDeaths_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerDepotItems` (
    `PlayerId` int NOT NULL,
    `SequenceId` int NOT NULL,
    `ParentId` int NOT NULL,
    `OpenTibiaId` int NOT NULL,
    `Count` int NOT NULL,
    CONSTRAINT `PK_PlayerDepotItems` PRIMARY KEY (`PlayerId`, `SequenceId`),
    CONSTRAINT `FK_PlayerDepotItems_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerItems` (
    `PlayerId` int NOT NULL,
    `SequenceId` int NOT NULL,
    `ParentId` int NOT NULL,
    `OpenTibiaId` int NOT NULL,
    `Count` int NOT NULL,
    CONSTRAINT `PK_PlayerItems` PRIMARY KEY (`PlayerId`, `SequenceId`),
    CONSTRAINT `FK_PlayerItems_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerKills` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PlayerId` int NOT NULL,
    `TargetId` int NOT NULL,
    `Unjustified` tinyint(1) NOT NULL,
    `CreationDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_PlayerKills` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_PlayerKills_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_PlayerKills_Players_TargetId` FOREIGN KEY (`TargetId`) REFERENCES `Players` (`Id`) ON DELETE NO ACTION
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerOutfits` (
    `PlayerId` int NOT NULL,
    `OutfitId` int NOT NULL,
    `OutfitAddon` int NOT NULL,
    CONSTRAINT `PK_PlayerOutfits` PRIMARY KEY (`PlayerId`, `OutfitId`),
    CONSTRAINT `FK_PlayerOutfits_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerSpells` (
    `PlayerId` int NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_PlayerSpells` PRIMARY KEY (`PlayerId`, `Name`),
    CONSTRAINT `FK_PlayerSpells_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerStorages` (
    `PlayerId` int NOT NULL,
    `Key` int NOT NULL,
    `Value` int NOT NULL,
    CONSTRAINT `PK_PlayerStorages` PRIMARY KEY (`PlayerId`, `Key`),
    CONSTRAINT `FK_PlayerStorages_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `PlayerVips` (
    `PlayerId` int NOT NULL,
    `VipId` int NOT NULL,
    CONSTRAINT `PK_PlayerVips` PRIMARY KEY (`PlayerId`, `VipId`),
    CONSTRAINT `FK_PlayerVips_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_PlayerVips_Players_VipId` FOREIGN KEY (`VipId`) REFERENCES `Players` (`Id`) ON DELETE NO ACTION
) CHARACTER SET=utf8mb4;

CREATE TABLE `RuleViolationReports` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `PlayerId` int NOT NULL,
    `Type` int NOT NULL,
    `RuleViolation` int NOT NULL,
    `Name` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Comment` varchar(255) CHARACTER SET utf8mb4 NOT NULL,
    `Translation` varchar(255) CHARACTER SET utf8mb4 NULL,
    `Statment` varchar(255) CHARACTER SET utf8mb4 NULL,
    `CreationDate` datetime(6) NOT NULL,
    CONSTRAINT `PK_RuleViolationReports` PRIMARY KEY (`Id`),
    CONSTRAINT `FK_RuleViolationReports_Players_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `HouseAccessLists` (
    `HouseId` int NOT NULL,
    `ListId` int NOT NULL,
    `Text` longtext CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK_HouseAccessLists` PRIMARY KEY (`HouseId`, `ListId`),
    CONSTRAINT `FK_HouseAccessLists_Houses_HouseId` FOREIGN KEY (`HouseId`) REFERENCES `Houses` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `HouseItems` (
    `HouseId` int NOT NULL,
    `SequenceId` bigint NOT NULL,
    `ParentId` bigint NOT NULL,
    `OpenTibiaId` int NOT NULL,
    `Count` int NOT NULL,
    CONSTRAINT `PK_HouseItems` PRIMARY KEY (`HouseId`, `SequenceId`),
    CONSTRAINT `FK_HouseItems_Houses_HouseId` FOREIGN KEY (`HouseId`) REFERENCES `Houses` (`Id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

INSERT INTO `Accounts` (`Id`, `Name`, `Password`, `PremiumUntil`)
VALUES (1, '1', '1', NULL);

INSERT INTO `Motd` (`Id`, `Message`)
VALUES (1, 'MTOTS - An open Tibia server developed by mtanksl');

INSERT INTO `ServerStorages` (`Key`, `Value`)
VALUES ('PlayersPeek', '0');

INSERT INTO `Worlds` (`Id`, `Ip`, `Name`, `Port`)
VALUES (1, '127.0.0.1', 'Cormaya', 7172);

INSERT INTO `Players` (`Id`, `AccountId`, `BankAccount`, `BaseOutfitAddon`, `BaseOutfitBody`, `BaseOutfitFeet`, `BaseOutfitHead`, `BaseOutfitId`, `BaseOutfitItemId`, `BaseOutfitLegs`, `BaseSpeed`, `Capacity`, `Direction`, `Experience`, `Gender`, `Health`, `Invisible`, `Level`, `Mana`, `MaxHealth`, `MaxMana`, `Name`, `OutfitAddon`, `OutfitBody`, `OutfitFeet`, `OutfitHead`, `OutfitId`, `OutfitItemId`, `OutfitLegs`, `Rank`, `SkillAxe`, `SkillAxePoints`, `SkillClub`, `SkillClubPoints`, `SkillDistance`, `SkillDistancePoints`, `SkillFish`, `SkillFishPoints`, `SkillFist`, `SkillFistPoints`, `SkillMagicLevel`, `SkillMagicLevelPoints`, `SkillShield`, `SkillShieldPoints`, `SkillSword`, `SkillSwordPoints`, `Soul`, `SpawnX`, `SpawnY`, `SpawnZ`, `Speed`, `Stamina`, `TownX`, `TownY`, `TownZ`, `Vocation`, `WorldId`)
VALUES (1, 1, 0, 0, 0, 0, 0, 266, 0, 0, 2218, 139000, 2, 15694800, 0, 645, FALSE, 100, 550, 645, 550, 'Gamemaster', 0, 0, 0, 0, 266, 0, 0, 2, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 0, 0, 10, 0, 10, 0, 100, 921, 771, 6, 2218, 2520, 921, 771, 6, 0, 1),
(2, 1, 0, 0, 0, 0, 0, 131, 0, 0, 418, 139000, 2, 15694800, 0, 1565, FALSE, 100, 550, 1565, 550, 'Knight', 0, 0, 0, 0, 131, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 4, 0, 80, 0, 90, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 1, 1),
(3, 1, 0, 0, 0, 0, 0, 129, 0, 0, 418, 139000, 2, 15694800, 0, 1105, FALSE, 100, 1470, 1105, 1470, 'Paladin', 0, 0, 0, 0, 129, 0, 0, 0, 10, 0, 10, 0, 70, 0, 10, 0, 10, 0, 15, 0, 40, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 2, 1),
(4, 1, 0, 0, 0, 0, 0, 130, 0, 0, 418, 139000, 2, 15694800, 0, 645, FALSE, 100, 2850, 645, 2850, 'Sorcerer', 0, 0, 0, 0, 130, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 60, 0, 10, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 4, 1),
(5, 1, 0, 0, 0, 0, 0, 130, 0, 0, 418, 139000, 2, 15694800, 0, 645, FALSE, 100, 2850, 645, 2850, 'Druid', 0, 0, 0, 0, 130, 0, 0, 0, 10, 0, 10, 0, 10, 0, 10, 0, 10, 0, 60, 0, 10, 0, 10, 0, 100, 921, 771, 6, 418, 2520, 921, 771, 6, 3, 1);

INSERT INTO `PlayerItems` (`PlayerId`, `SequenceId`, `Count`, `OpenTibiaId`, `ParentId`)
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

INSERT INTO `PlayerOutfits` (`OutfitId`, `PlayerId`, `OutfitAddon`)
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

CREATE INDEX `IX_Bans_AccountId` ON `Bans` (`AccountId`);

CREATE INDEX `IX_Bans_PlayerId` ON `Bans` (`PlayerId`);

CREATE INDEX `IX_BugReports_PlayerId` ON `BugReports` (`PlayerId`);

CREATE INDEX `IX_DebugAsserts_PlayerId` ON `DebugAsserts` (`PlayerId`);

CREATE INDEX `IX_Houses_OwnerId` ON `Houses` (`OwnerId`);

CREATE INDEX `IX_PlayerDeaths_AttackerId` ON `PlayerDeaths` (`AttackerId`);

CREATE INDEX `IX_PlayerDeaths_PlayerId` ON `PlayerDeaths` (`PlayerId`);

CREATE INDEX `IX_PlayerKills_PlayerId` ON `PlayerKills` (`PlayerId`);

CREATE INDEX `IX_PlayerKills_TargetId` ON `PlayerKills` (`TargetId`);

CREATE INDEX `IX_Players_AccountId` ON `Players` (`AccountId`);

CREATE INDEX `IX_Players_WorldId` ON `Players` (`WorldId`);

CREATE INDEX `IX_PlayerVips_VipId` ON `PlayerVips` (`VipId`);

CREATE INDEX `IX_RuleViolationReports_PlayerId` ON `RuleViolationReports` (`PlayerId`);

COMMIT;