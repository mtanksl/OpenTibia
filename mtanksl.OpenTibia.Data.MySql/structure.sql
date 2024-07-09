/*!40101 SET NAMES utf8 */;
/*!40101 SET SQL_MODE=''*/;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
CREATE DATABASE /*!32312 IF NOT EXISTS*/`mtots` /*!40100 DEFAULT CHARACTER SET latin1 */;

USE `mtots`;

-- Accounts

DROP TABLE IF EXISTS `Accounts`;

CREATE TABLE `Accounts` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NOT NULL,
  `Password` VARCHAR(255) NOT NULL,
  `PremiumUntil` DATETIME DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UQ_Accounts_Name` (`Name`)
) ENGINE=INNODB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

INSERT INTO `Accounts`(`Id`,`Name`,`Password`,`PremiumUntil`) 
VALUES (1,'1','1',NULL);

-- Bans

DROP TABLE IF EXISTS `Bans`;

CREATE TABLE `Bans` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Type` INT(11) NOT NULL,
  `AccountId` INT(11) DEFAULT NULL,
  `PlayerId` INT(11) DEFAULT NULL,
  `IpAddress` VARCHAR(255) DEFAULT NULL,
  `Message` VARCHAR(255) NOT NULL,
  `CreationDate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Bans_AccountId` (`AccountId`),
  KEY `IX_Bans_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_Bans_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Bans_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- BugReports

DROP TABLE IF EXISTS `BugReports`;

CREATE TABLE `BugReports` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` INT(11) NOT NULL,
  `Message` VARCHAR(255) NOT NULL,
  `CreationDate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_BugReports_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_BugReports_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- DebugAsserts

DROP TABLE IF EXISTS `DebugAsserts`;

CREATE TABLE `DebugAsserts` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` INT(11) NOT NULL,
  `AssertLine` VARCHAR(255) NOT NULL,
  `ReportDate` VARCHAR(255) NOT NULL,
  `Description` VARCHAR(255) NOT NULL,
  `Comment` VARCHAR(255) DEFAULT NULL,
  `CreationDate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DebugAsserts_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_DebugAsserts_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- HouseAccessLists

DROP TABLE IF EXISTS `HouseAccessLists`;

CREATE TABLE `HouseAccessLists` (
  `HouseId` INT(11) NOT NULL,
  `ListId` INT(11) NOT NULL,
  `Text` TEXT NOT NULL,
  PRIMARY KEY (`HouseId`,`ListId`),
  KEY `IX_HouseAccessLists_HouseId` (`HouseId`),
  KEY `IX_HouseAccessLists_ListId` (`ListId`),
  CONSTRAINT `FK_HouseAccessLists_HouseId` FOREIGN KEY (`HouseId`) REFERENCES `Houses` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=latin1;

-- HouseItems

DROP TABLE IF EXISTS `HouseItems`;

CREATE TABLE `HouseItems` (
  `HouseId` INT(11) NOT NULL,
  `SequenceId` BIGINT(20) NOT NULL,
  `ParentId` BIGINT(20) NOT NULL,
  `OpenTibiaId` INT(11) NOT NULL,
  `Count` INT(11) NOT NULL,
  PRIMARY KEY (`HouseId`,`SequenceId`),
  KEY `IX_HouseItems_HouseId` (`HouseId`),
  CONSTRAINT `FK_HouseItems_HouseId` FOREIGN KEY (`HouseId`) REFERENCES `Houses` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=latin1;

-- Houses

DROP TABLE IF EXISTS `Houses`;

CREATE TABLE `Houses` (
  `Id` INT(11) NOT NULL,
  `OwnerId` INT(11) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Houses_OwnerId` (`OwnerId`),
  CONSTRAINT `FK_Houses_OwnerId` FOREIGN KEY (`OwnerId`) REFERENCES `Players` (`Id`) ON DELETE SET NULL
) ENGINE=INNODB DEFAULT CHARSET=latin1;

INSERT INTO `Houses`(`Id`,`OwnerId`) 
VALUES (3,NULL),
(4,NULL);

-- Motd

DROP TABLE IF EXISTS `Motd`;

CREATE TABLE `Motd` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Message` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=INNODB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

INSERT INTO `Motd`(`Id`,`Message`) 
VALUES (0,'MTOTS - An open Tibia server developed by mtanksl');

-- PlayerAchievements

DROP TABLE IF EXISTS `PlayerAchievements`;

CREATE TABLE `PlayerAchievements` (
  `PlayerId` INT(11) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`PlayerId`,`Name`),
  KEY `IX_PlayerAchievements_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerAchievements_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- PlayerBlesses

DROP TABLE IF EXISTS `PlayerBlesses`;

CREATE TABLE `PlayerBlesses` (
  `PlayerId` INT(11) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`PlayerId`,`Name`),
  KEY `IX_PlayerBlesses_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerBlesses_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- PlayerDepotItems

DROP TABLE IF EXISTS `PlayerDepotItems`;

CREATE TABLE `PlayerDepotItems` (
  `PlayerId` INT(11) NOT NULL,
  `SequenceId` INT(11) NOT NULL,
  `ParentId` INT(11) NOT NULL,
  `OpenTibiaId` INT(11) NOT NULL,
  `Count` INT(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`SequenceId`),
  KEY `IX_PlayerDepotItems_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerDepotItems_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- PlayerItems

DROP TABLE IF EXISTS `PlayerItems`;

CREATE TABLE `PlayerItems` (
  `PlayerId` INT(11) NOT NULL,
  `SequenceId` INT(11) NOT NULL,
  `ParentId` INT(11) NOT NULL,
  `OpenTibiaId` INT(11) NOT NULL,
  `Count` INT(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`SequenceId`),
  KEY `IX_PlayerItems_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerItems_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

INSERT INTO `PlayerItems`(`PlayerId`,`SequenceId`,`ParentId`,`OpenTibiaId`,`Count`) VALUES 
(1,101,3,1987,1),
(1,102,101,2120,1),
(1,103,101,2554,1),
(2,101,3,1987,1),
(2,102,101,2120,1),
(2,103,101,2554,1),
(3,101,3,1987,1),
(3,102,101,2120,1),
(3,103,101,2554,1),
(4,101,3,1987,1),
(4,102,101,2120,1),
(4,103,101,2554,1),
(5,101,3,1987,1),
(5,102,101,2120,1),
(5,103,101,2554,1);

-- PlayerOutfits

DROP TABLE IF EXISTS `PlayerOutfits`;

CREATE TABLE `PlayerOutfits` (
  `PlayerId` INT(11) NOT NULL,
  `OutfitId` INT(11) NOT NULL,
  `OutfitAddon` INT(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`OutfitId`),
  KEY `IX_PlayerOutfits_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerOutfits_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- PlayerSpells

DROP TABLE IF EXISTS `PlayerSpells`;

CREATE TABLE `PlayerSpells` (
  `PlayerId` INT(11) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  PRIMARY KEY (`PlayerId`,`Name`),
  KEY `IX_PlayerSpells_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerSpells_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- PlayerStorages

DROP TABLE IF EXISTS `PlayerStorages`;

CREATE TABLE `PlayerStorages` (
  `PlayerId` INT(11) NOT NULL,
  `Key` INT(11) NOT NULL,
  `Value` INT(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`Key`),
  KEY `IX_PlayerStorages_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerStorages_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- PlayerVips

DROP TABLE IF EXISTS `PlayerVips`;

CREATE TABLE `PlayerVips` (
  `PlayerId` INT(11) NOT NULL,
  `VipId` INT(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`VipId`),
  KEY `IX_PlayerVips_PlayerId` (`PlayerId`),
  KEY `IX_PlayerVips_VipId` (`VipId`),
  CONSTRAINT `FK_PlayerVips_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PlayerVips_VipId` FOREIGN KEY (`VipId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- Players

DROP TABLE IF EXISTS `Players`;

CREATE TABLE `Players` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `AccountId` INT(11) NOT NULL,
  `WorldId` INT(11) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  `Health` INT(11) NOT NULL,
  `MaxHealth` INT(11) NOT NULL,
  `Direction` INT(11) NOT NULL,
  `BaseOutfitItemId` INT(11) NOT NULL,
  `BaseOutfitId` INT(11) NOT NULL,
  `BaseOutfitHead` INT(11) NOT NULL,
  `BaseOutfitBody` INT(11) NOT NULL,
  `BaseOutfitLegs` INT(11) NOT NULL,
  `BaseOutfitFeet` INT(11) NOT NULL,
  `BaseOutfitAddon` INT(11) NOT NULL,
  `OutfitItemId` INT(11) NOT NULL,
  `OutfitId` INT(11) NOT NULL,
  `OutfitHead` INT(11) NOT NULL,
  `OutfitBody` INT(11) NOT NULL,
  `OutfitLegs` INT(11) NOT NULL,
  `OutfitFeet` INT(11) NOT NULL,
  `OutfitAddon` INT(11) NOT NULL,
  `BaseSpeed` INT(11) NOT NULL,
  `Speed` INT(11) NOT NULL,
  `Invisible` INT(11) NOT NULL,
  `SkillMagicLevel` INT(11) NOT NULL,
  `SkillMagicLevelPercent` INT(11) NOT NULL,
  `SkillFist` INT(11) NOT NULL,
  `SkillFistPercent` INT(11) NOT NULL,
  `SkillClub` INT(11) NOT NULL,
  `SkillClubPercent` INT(11) NOT NULL,
  `SkillSword` INT(11) NOT NULL,
  `SkillSwordPercent` INT(11) NOT NULL,
  `SkillAxe` INT(11) NOT NULL,
  `SkillAxePercent` INT(11) NOT NULL,
  `SkillDistance` INT(11) NOT NULL,
  `SkillDistancePercent` INT(11) NOT NULL,
  `SkillShield` INT(11) NOT NULL,
  `SkillShieldPercent` INT(11) NOT NULL,
  `SkillFish` INT(11) NOT NULL,
  `SkillFishPercent` INT(11) NOT NULL,
  `Experience` BIGINT(11) NOT NULL,
  `Level` INT(11) NOT NULL,
  `LevelPercent` INT(11) NOT NULL,
  `Mana` INT(11) NOT NULL,
  `MaxMana` INT(11) NOT NULL,
  `Soul` INT(11) NOT NULL,
  `Capacity` INT(11) NOT NULL,
  `Stamina` INT(11) NOT NULL,
  `Gender` INT(11) NOT NULL,
  `Vocation` INT(11) NOT NULL,
  `Rank` INT(11) NOT NULL,
  `SpawnX` INT(11) NOT NULL,
  `SpawnY` INT(11) NOT NULL,
  `SpawnZ` INT(11) NOT NULL,
  `TownX` INT(11) NOT NULL,
  `TownY` INT(11) NOT NULL,
  `TownZ` INT(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Players_AccountId` (`AccountId`),
  KEY `IX_Players_WorldId` (`WorldId`),
  CONSTRAINT `FK_Players_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `IX_Players_WorldId` FOREIGN KEY (`WorldId`) REFERENCES `Worlds` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4;

INSERT INTO `Players`(`Id`,`AccountId`,`WorldId`,`Name`,`Health`,`MaxHealth`,`Direction`,`BaseOutfitItemId`,`BaseOutfitId`,`BaseOutfitHead`,`BaseOutfitBody`,`BaseOutfitLegs`,`BaseOutfitFeet`,`BaseOutfitAddon`,`OutfitItemId`,`OutfitId`,`OutfitHead`,`OutfitBody`,`OutfitLegs`,`OutfitFeet`,`OutfitAddon`,`BaseSpeed`,`Speed`,`Invisible`,`SkillMagicLevel`,`SkillMagicLevelPercent`,`SkillFist`,`SkillFistPercent`,`SkillClub`,`SkillClubPercent`,`SkillSword`,`SkillSwordPercent`,`SkillAxe`,`SkillAxePercent`,`SkillDistance`,`SkillDistancePercent`,`SkillShield`,`SkillShieldPercent`,`SkillFish`,`SkillFishPercent`,`Experience`,`Level`,`LevelPercent`,`Mana`,`MaxMana`,`Soul`,`Capacity`,`Stamina`,`Gender`,`Vocation`,`Rank`,`SpawnX`,`SpawnY`,`SpawnZ`,`TownX`,`TownY`,`TownZ`) VALUES 
(1,1,1,'Gamemaster',645,645,2,0,266,0,0,0,0,0,0,266,0,0,0,0,0,2218,2218,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15694800,100,0,550,550,100,139000,2520,0,0,2,921,771,6,921,771,6),
(2,1,1,'Knight',1565,1565,2,0,131,78,69,58,76,0,0,131,78,69,58,76,0,418,418,0,4,0,0,0,0,0,90,0,0,0,0,0,80,0,0,0,15694800,100,0,550,550,100,277000,2520,0,1,0,921,771,6,921,771,6),
(3,1,1,'Paladin',1105,1105,2,0,129,78,69,58,76,0,0,129,78,69,58,76,0,418,418,0,20,0,0,0,0,0,0,0,0,0,70,0,40,0,0,0,15694800,100,0,1470,1470,100,231000,2520,0,2,0,921,771,6,921,771,6),
(4,1,1,'Sorcerer',645,645,2,0,130,78,69,58,76,0,0,130,78,69,58,76,0,418,418,0,70,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15694800,100,0,2850,2850,100,139000,2520,0,4,0,921,771,6,921,771,6),
(5,1,1,'Druid',645,645,2,0,130,78,69,58,76,0,0,130,78,69,58,76,0,418,418,0,70,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15694800,100,0,2850,2850,100,139000,2520,0,3,0,921,771,6,921,771,6);

-- RuleViolationReports

DROP TABLE IF EXISTS `RuleViolationReports`;

CREATE TABLE `RuleViolationReports` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` INT(11) NOT NULL,
  `Type` INT(11) NOT NULL,
  `RuleViolation` VARCHAR(255) NOT NULL,
  `Name` VARCHAR(255) NOT NULL,
  `Comment` VARCHAR(255) NOT NULL,
  `Translation` VARCHAR(255) DEFAULT NULL,
  `Statment` VARCHAR(255) DEFAULT NULL,
  `CreationDate` DATETIME NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RuleViolationReports_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_RuleViolationReports_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

-- Worlds

DROP TABLE IF EXISTS `Worlds`;

CREATE TABLE `Worlds` (
  `Id` INT(11) NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(255) NOT NULL,
  `Ip` VARCHAR(255) NOT NULL,
  `Port` INT(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=INNODB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

INSERT INTO `Worlds`(`Id`,`Name`,`Ip`,`Port`) VALUES 
(1,'Cormaya','127.0.0.1',7172);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;