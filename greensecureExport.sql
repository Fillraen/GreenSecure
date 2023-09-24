-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : sam. 23 sep. 2023 à 10:54
-- Version du serveur : 8.0.31
-- Version de PHP : 8.0.26

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `greensecure`
--

-- --------------------------------------------------------

--
-- Structure de la table `credentials`
--

DROP TABLE IF EXISTS `credentials`;
CREATE TABLE IF NOT EXISTS `credentials` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `IdUser` int DEFAULT NULL,
  `Username` varchar(255) NOT NULL,
  `EmailAddress` varchar(255) DEFAULT NULL,
  `Url` text,
  `Name` varchar(255) DEFAULT NULL,
  `EncryptedPassword` text NOT NULL,
  `Domain` varchar(255) DEFAULT NULL,
  `Category` varchar(255) DEFAULT NULL,
  `DateCreated` datetime DEFAULT NULL,
  `LastModified` datetime DEFAULT NULL,
  `Complexity` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IdUser` (`IdUser`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `credentials`
--

INSERT INTO `credentials` (`Id`, `IdUser`, `Username`, `EmailAddress`, `Url`, `Name`, `EncryptedPassword`, `Domain`, `Category`, `DateCreated`, `LastModified`, `Complexity`) VALUES
(1, 1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(2, 1, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100),
(3, 1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(4, 2, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100),
(5, 2, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(6, 2, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100),
(7, 3, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(8, 3, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100),
(9, 3, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(10, 4, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100),
(11, 4, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(12, 4, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100),
(13, 1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(14, 1, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100),
(15, 1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 80),
(16, 1, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', '2023-09-23 12:48:07', '2023-09-23 12:48:07', 100);

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `EncryptedPassword` text NOT NULL,
  `CreatedDate` datetime DEFAULT NULL,
  `LastLoginDate` datetime DEFAULT NULL,
  `IsLocked` tinyint(1) DEFAULT '0',
  `FailedLoginAttempts` int DEFAULT '0',
  `EncryptionKey` text NOT NULL,
  `EncryptionIV` text NOT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Email` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `user`
--

INSERT INTO `user` (`UserId`, `Username`, `Email`, `EncryptedPassword`, `CreatedDate`, `LastLoginDate`, `IsLocked`, `FailedLoginAttempts`, `EncryptionKey`, `EncryptionIV`) VALUES
(1, 'user1', 'user1@example.com', 'fx3ylSPGumcWahz18NisMQ==', '2023-09-23 12:48:04', '2023-09-23 12:48:04', 0, 0, '6Gesa/QQhXMaXRq45dHnjHX5XajFqhivLkDab96Cbjw=', 'u+yWc2xkiRB65mU2xpBAIQ=='),
(2, 'user2', 'user2@example.com', 'fx3ylSPGumcWahz18NisMQ==', '2023-09-23 12:48:04', '2023-09-23 12:48:04', 0, 0, '6Gesa/QQhXMaXRq45dHnjHX5XajFqhivLkDab96Cbjw=', 'u+yWc2xkiRB65mU2xpBAIQ=='),
(3, 'user3', 'user3@example.com', 'fx3ylSPGumcWahz18NisMQ==', '2023-09-23 12:48:04', '2023-09-23 12:48:04', 0, 0, '6Gesa/QQhXMaXRq45dHnjHX5XajFqhivLkDab96Cbjw=', 'u+yWc2xkiRB65mU2xpBAIQ=='),
(4, 'user4', 'user4@example.com', 'fx3ylSPGumcWahz18NisMQ==', '2023-09-23 12:48:04', '2023-09-23 12:48:04', 0, 0, '6Gesa/QQhXMaXRq45dHnjHX5XajFqhivLkDab96Cbjw=', 'u+yWc2xkiRB65mU2xpBAIQ==');

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `credentials`
--
ALTER TABLE `credentials`
  ADD CONSTRAINT `credentials_ibfk_1` FOREIGN KEY (`IdUser`) REFERENCES `user` (`UserId`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
