-- --------------------------------------------------------
-- Hoszt:                        127.0.0.1
-- Szerver verzió:               10.4.28-MariaDB - mariadb.org binary distribution
-- Szerver OS:                   Win64
-- HeidiSQL Verzió:              12.2.0.6576
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Adatbázis struktúra mentése a diploma.
CREATE DATABASE IF NOT EXISTS `diploma` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `diploma`;

-- Struktúra mentése tábla diploma. factory
CREATE TABLE IF NOT EXISTS `factory` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  `width` float NOT NULL,
  `length` float NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tábla adatainak mentése diploma.factory: ~1 rows (hozzávetőleg)
REPLACE INTO `factory` (`id`, `name`, `width`, `length`) VALUES
	(1, 'Gyár_1', 5, 5);

-- Struktúra mentése tábla diploma. io_port
CREATE TABLE IF NOT EXISTS `io_port` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  `plc_id` int(11) NOT NULL,
  `direction` tinyint(4) NOT NULL COMMENT '0 - In,   1-Out',
  `offset` int(11) NOT NULL,
  `bit` int(11) NOT NULL,
  `value` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK1_plc_id` (`plc_id`),
  CONSTRAINT `FK1_plc_id` FOREIGN KEY (`plc_id`) REFERENCES `plc` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tábla adatainak mentése diploma.io_port: ~3 rows (hozzávetőleg)
REPLACE INTO `io_port` (`id`, `name`, `plc_id`, `direction`, `offset`, `bit`, `value`) VALUES
	(2, 'lámpa relé', 2, 1, 0, 0, 0),
	(3, 'lámpa gomb', 2, 0, 1, 0, 0),
	(4, 'kettes kimenet', 2, 1, 0, 1, 0);

-- Struktúra mentése tábla diploma. plc
CREATE TABLE IF NOT EXISTS `plc` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  `address` text NOT NULL,
  `type_id` int(11) NOT NULL,
  `db_number` int(11) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `FK1_plc_type_id` (`type_id`),
  CONSTRAINT `FK1_plc_type_id` FOREIGN KEY (`type_id`) REFERENCES `plc_type` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tábla adatainak mentése diploma.plc: ~1 rows (hozzávetőleg)
REPLACE INTO `plc` (`id`, `name`, `address`, `type_id`, `db_number`) VALUES
	(2, 'PLC_1', '192.168.100.100', 6, 1);

-- Struktúra mentése tábla diploma. plc_type
CREATE TABLE IF NOT EXISTS `plc_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  PRIMARY KEY (`id`) USING BTREE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci ROW_FORMAT=DYNAMIC;

-- Tábla adatainak mentése diploma.plc_type: ~2 rows (hozzávetőleg)
REPLACE INTO `plc_type` (`id`, `name`) VALUES
	(6, 'S7-1200'),
	(9, 'S7-300');

-- Struktúra mentése tábla diploma. robot
CREATE TABLE IF NOT EXISTS `robot` (
  `guid` varchar(36) NOT NULL DEFAULT '0',
  `alias` text NOT NULL,
  `address` text DEFAULT NULL,
  `name` text DEFAULT NULL,
  `system_name` text DEFAULT NULL,
  `host_name` text DEFAULT NULL,
  `version` text DEFAULT NULL,
  `virtual` tinyint(4) DEFAULT 0 COMMENT '0 virtual, 1 real',
  `xpos` float NOT NULL DEFAULT 0,
  `ypos` float NOT NULL DEFAULT 0,
  `z_orinetation` float NOT NULL DEFAULT 0,
  `type_id` int(11) NOT NULL,
  `factory_id` int(11) NOT NULL,
  PRIMARY KEY (`guid`),
  KEY `FK2_factor_id` (`factory_id`),
  KEY `FK1_robot_type_id` (`type_id`),
  CONSTRAINT `FK1_robot_type_id` FOREIGN KEY (`type_id`) REFERENCES `robot_type` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK2_factor_id` FOREIGN KEY (`factory_id`) REFERENCES `factory` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci ROW_FORMAT=DYNAMIC;

-- Tábla adatainak mentése diploma.robot: ~4 rows (hozzávetőleg)
REPLACE INTO `robot` (`guid`, `alias`, `address`, `name`, `system_name`, `host_name`, `version`, `virtual`, `xpos`, `ypos`, `z_orinetation`, `type_id`, `factory_id`) VALUES
	('42a6dfe8-2839-4f2b-9b21-2568b46f2fba', 'Virtuális IRB120 1', '127.0.0.1', 'IRB120_3_58', 'IRB120_3_58', '', '6.13.0.1', 1, 0.5, 0.3, -90, 3, 1),
	('44a91dfd-0aa5-432a-8da7-825a9b8fd9bc', 'Labor Yumi', '10.61.10.202', '14000-501288', '14000-501288', '', '6.6.0.1', 0, 0, 0.3, -90, 5, 1),
	('4df28949-1daf-49a4-8905-2baac30feb3b', 'Virtuális Yumi 2', '127.0.0.1', 'IRB14000', 'IRB14000', '', '6.13.0.1', 1, -0.5, -0.035, 0, 5, 1),
	('734130cb-8a4b-48f5-8793-92615d4f80f1', 'Virtuális Yumi 1', '127.0.0.1', 'Yumi', 'Yumi', '', '6.13.0.1', 1, 0.4, -0.5, 90, 5, 1);

-- Struktúra mentése tábla diploma. robot_type
CREATE TABLE IF NOT EXISTS `robot_type` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` text NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Tábla adatainak mentése diploma.robot_type: ~2 rows (hozzávetőleg)
REPLACE INTO `robot_type` (`id`, `name`) VALUES
	(3, 'IRB120'),
	(5, 'IRB14000');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
