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
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Password` varchar(255) NOT NULL,
  `PremiumDays` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UQ_Accounts_Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

insert  into `Accounts`(`Id`,`Name`,`Password`,`PremiumDays`) values 
(1,'1','1',0);

-- Bans

DROP TABLE IF EXISTS `Bans`;

CREATE TABLE `Bans` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Type` int(11) NOT NULL,
  `AccountId` int(11) NULL,
  `PlayerId` int(11) NULL,
  `IpAddress` varchar(255) NULL,
  `Message` varchar(255) NOT NULL,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Bans_AccountId` (`AccountId`),
  KEY `IX_Bans_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_Bans_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Bans_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- BugReports

DROP TABLE IF EXISTS `BugReports`;

CREATE TABLE `BugReports` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` int(11) NOT NULL,
  `Message` varchar(255) NOT NULL,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_BugReports_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_BugReports_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- DebugAsserts

DROP TABLE IF EXISTS `DebugAsserts`;

CREATE TABLE `DebugAsserts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` int(11) NOT NULL,
  `AssertLine` varchar(255) NOT NULL,
  `ReportDate` varchar(255) NOT NULL,
  `Description` varchar(255) NOT NULL,
  `Comment` varchar(255) NULL,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DebugAsserts_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_DebugAsserts_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Motd

DROP TABLE IF EXISTS `Motd`;

CREATE TABLE `Motd` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Message` varchar(255) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=utf8mb4;

insert  into `Motd`(`Id`,`Message`) values 
(0,'An open Tibia server developed by mtanksl');

-- PlayerDepotItems

DROP TABLE IF EXISTS `PlayerDepotItems`;

CREATE TABLE `PlayerDepotItems` (
  `PlayerId` int(11) NOT NULL,
  `SequenceId` int(11) NOT NULL,
  `ParentId` int(11) NOT NULL,
  `OpenTibiaId` int(11) NOT NULL,
  `Count` int(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`SequenceId`),
  KEY `IX_PlayerDepotItems_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerDepotItems_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- PlayerItems

DROP TABLE IF EXISTS `PlayerItems`;

CREATE TABLE `PlayerItems` (
  `PlayerId` int(11) NOT NULL,
  `SequenceId` int(11) NOT NULL,
  `ParentId` int(11) NOT NULL,
  `OpenTibiaId` int(11) NOT NULL,
  `Count` int(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`SequenceId`),
  KEY `IX_PlayerItems_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerItems_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

insert  into `PlayerItems`(`PlayerId`,`SequenceId`,`ParentId`,`OpenTibiaId`,`Count`) values 
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

-- PlayerStorages

DROP TABLE IF EXISTS `PlayerStorages`;

CREATE TABLE `PlayerStorages` (
  `PlayerId` int(11) NOT NULL,
  `Key` int(11) NOT NULL,
  `Value` int(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`Key`),
  KEY `IX_PlayerStorages_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_PlayerStorages_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- PlayerVips

DROP TABLE IF EXISTS `PlayerVips`;

CREATE TABLE `PlayerVips` (
  `PlayerId` int(11) NOT NULL,
  `VipId` int(11) NOT NULL,
  PRIMARY KEY (`PlayerId`,`VipId`),
  KEY `IX_PlayerVips_PlayerId` (`PlayerId`),
  KEY `IX_PlayerVips_VipId` (`VipId`),
  CONSTRAINT `FK_PlayerVips_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_PlayerVips_VipId` FOREIGN KEY (`VipId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Players

DROP TABLE IF EXISTS `Players`;

CREATE TABLE `Players` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `AccountId` int(11) NOT NULL,
  `WorldId` int(11) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Health` int(11) NOT NULL,
  `MaxHealth` int(11) NOT NULL,
  `Direction` int(11) NOT NULL,
  `BaseOutfitItemId` int(11) NOT NULL,
  `BaseOutfitId` int(11) NOT NULL,
  `BaseOutfitHead` int(11) NOT NULL,
  `BaseOutfitBody` int(11) NOT NULL,
  `BaseOutfitLegs` int(11) NOT NULL,
  `BaseOutfitFeet` int(11) NOT NULL,
  `BaseOutfitAddon` int(11) NOT NULL,
  `OutfitItemId` int(11) NOT NULL,
  `OutfitId` int(11) NOT NULL,
  `OutfitHead` int(11) NOT NULL,
  `OutfitBody` int(11) NOT NULL,
  `OutfitLegs` int(11) NOT NULL,
  `OutfitFeet` int(11) NOT NULL,
  `OutfitAddon` int(11) NOT NULL,
  `BaseSpeed` int(11) NOT NULL,
  `Speed` int(11) NOT NULL,
  `Invisible` int(11) NOT NULL,
  `SkillMagicLevel` int(11) NOT NULL,
  `SkillMagicLevelPercent` int(11) NOT NULL,
  `SkillFist` int(11) NOT NULL,
  `SkillFistPercent` int(11) NOT NULL,
  `SkillClub` int(11) NOT NULL,
  `SkillClubPercent` int(11) NOT NULL,
  `SkillSword` int(11) NOT NULL,
  `SkillSwordPercent` int(11) NOT NULL,
  `SkillAxe` int(11) NOT NULL,
  `SkillAxePercent` int(11) NOT NULL,
  `SkillDistance` int(11) NOT NULL,
  `SkillDistancePercent` int(11) NOT NULL,
  `SkillShield` int(11) NOT NULL,
  `SkillShieldPercent` int(11) NOT NULL,
  `SkillFish` int(11) NOT NULL,
  `SkillFishPercent` int(11) NOT NULL,
  `Experience` int(11) NOT NULL,
  `Level` int(11) NOT NULL,
  `LevelPercent` int(11) NOT NULL,
  `Mana` int(11) NOT NULL,
  `MaxMana` int(11) NOT NULL,
  `Soul` int(11) NOT NULL,
  `Capacity` int(11) NOT NULL,
  `Stamina` int(11) NOT NULL,
  `Gender` int(11) NOT NULL,
  `Vocation` int(11) NOT NULL,
  `SpawnX` int(11) NOT NULL,
  `SpawnY` int(11) NOT NULL,
  `SpawnZ` int(11) NOT NULL,
  `TownX` int(11) NOT NULL,
  `TownY` int(11) NOT NULL,
  `TownZ` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Players_AccountId` (`AccountId`),
  KEY `IX_Players_WorldId` (`WorldId`),
  CONSTRAINT `FK_Players_AccountId` FOREIGN KEY (`AccountId`) REFERENCES `Accounts` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `IX_Players_WorldId` FOREIGN KEY (`WorldId`) REFERENCES `Worlds` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4;

insert  into `Players`(`Id`,`AccountId`,`WorldId`,`Name`,`Health`,`MaxHealth`,`Direction`,`BaseOutfitItemId`,`BaseOutfitId`,`BaseOutfitHead`,`BaseOutfitBody`,`BaseOutfitLegs`,`BaseOutfitFeet`,`BaseOutfitAddon`,`OutfitItemId`,`OutfitId`,`OutfitHead`,`OutfitBody`,`OutfitLegs`,`OutfitFeet`,`OutfitAddon`,`BaseSpeed`,`Speed`,`Invisible`,`SkillMagicLevel`,`SkillMagicLevelPercent`,`SkillFist`,`SkillFistPercent`,`SkillClub`,`SkillClubPercent`,`SkillSword`,`SkillSwordPercent`,`SkillAxe`,`SkillAxePercent`,`SkillDistance`,`SkillDistancePercent`,`SkillShield`,`SkillShieldPercent`,`SkillFish`,`SkillFishPercent`,`Experience`,`Level`,`LevelPercent`,`Mana`,`MaxMana`,`Soul`,`Capacity`,`Stamina`,`Gender`,`Vocation`,`SpawnX`,`SpawnY`,`SpawnZ`,`TownX`,`TownY`,`TownZ`) values 
(1,1,1,'Gamemaster',645,645,2,0,266,0,0,0,0,0,0,266,0,0,0,0,0,2218,2218,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15694800,100,0,550,550,100,139000,2520,0,9,921,771,6,921,771,6),
(2,1,1,'Knight',1565,1565,2,0,131,78,69,58,76,0,0,131,78,69,58,76,0,418,418,0,4,0,0,0,0,0,90,0,0,0,0,0,80,0,0,0,15694800,100,0,550,550,100,277000,2520,0,1,921,771,6,921,771,6),
(3,1,1,'Paladin',1105,1105,2,0,129,78,69,58,76,0,0,129,78,69,58,76,0,418,418,0,20,0,0,0,0,0,0,0,0,0,70,0,40,0,0,0,15694800,100,0,1470,1470,100,231000,2520,0,2,921,771,6,921,771,6),
(4,1,1,'Sorcerer',645,645,2,0,130,78,69,58,76,0,0,130,78,69,58,76,0,418,418,0,70,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15694800,100,0,2850,2850,100,139000,2520,0,4,921,771,6,921,771,6),
(5,1,1,'Druid',645,645,2,0,130,78,69,58,76,0,0,130,78,69,58,76,0,418,418,0,70,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,15694800,100,0,2850,2850,100,139000,2520,0,3,921,771,6,921,771,6);

-- RuleViolationReports

DROP TABLE IF EXISTS `RuleViolationReports`;

CREATE TABLE `RuleViolationReports` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PlayerId` int(11) NOT NULL,
  `Type` int(11) NOT NULL,
  `RuleViolation` varchar(255) NOT NULL,
  `Name` varchar(255) NOT NULL,
  `Comment` varchar(255) NOT NULL,
  `Translation` varchar(255) NULL,
  `Statment` varchar(255) NULL,
  `CreationDate` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RuleViolationReports_PlayerId` (`PlayerId`),
  CONSTRAINT `FK_RuleViolationReports_PlayerId` FOREIGN KEY (`PlayerId`) REFERENCES `Players` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Worlds

DROP TABLE IF EXISTS `Worlds`;

CREATE TABLE `Worlds` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(255) NOT NULL,
  `Ip` varchar(255) NOT NULL,
  `Port` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4;

insert  into `Worlds`(`Id`,`Name`,`Ip`,`Port`) values 
(1,'Cormaya','127.0.0.1',7172);

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;