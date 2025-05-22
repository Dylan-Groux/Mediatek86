-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : jeu. 22 mai 2025 à 08:43
-- Version du serveur : 9.1.0
-- Version de PHP : 8.3.14

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `mediatek86`
--

DELIMITER $$
--
-- Procédures
--
DROP PROCEDURE IF EXISTS `AjouterLivre`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `AjouterLivre` (IN `p_titre` VARCHAR(255), IN `p_auteur` VARCHAR(255), IN `p_editeur` VARCHAR(255), IN `p_annee_publication` INT, IN `p_genre` VARCHAR(255))   BEGIN
    DECLARE last_doc_id INT;
    
    -- Début de la transaction
    START TRANSACTION;
    
    -- Ajout dans la table document
    INSERT INTO document (titre, type) VALUES (p_titre, 'livre');
    SET last_doc_id = LAST_INSERT_ID();
    
    -- Ajout dans la table livre
    INSERT INTO livre (id, auteur, editeur, annee_publication, genre) 
    VALUES (last_doc_id, p_auteur, p_editeur, p_annee_publication, p_genre);
    
    -- Validation de la transaction
    COMMIT;
END$$

DROP PROCEDURE IF EXISTS `RemplirAuteurEtAuteurlivre`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `RemplirAuteurEtAuteurlivre` ()   BEGIN
    -- Insertion des auteurs distincts dans la table auteur
    INSERT INTO auteur (nom)
    SELECT DISTINCT auteur FROM livre;
    
    -- Insertion des relations auteur-livre
    INSERT INTO auteurlivre (id_auteur, id_livre)
    SELECT a.id, l.id 
    FROM livre l
    JOIN auteur a ON l.auteur = a.nom;
END$$

DROP PROCEDURE IF EXISTS `SupprimerExemplairesInutilisables`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `SupprimerExemplairesInutilisables` ()   BEGIN
    -- Affichage des exemplaires inutilisables
    SELECT e.id AS exemplaire_id, e.date_achat, d.id AS document_id, d.titre 
    FROM exemplaire e
    JOIN document d ON e.document_id = d.id
    WHERE e.etat = 'inutilisable';
    
    -- Suppression des exemplaires inutilisables
    DELETE FROM exemplaire WHERE etat = 'inutilisable';
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `commande`
--

DROP TABLE IF EXISTS `commande`;
CREATE TABLE IF NOT EXISTS `commande` (
  `id` varchar(5) NOT NULL,
  `dateCommande` date DEFAULT NULL,
  `montant` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `commande`
--

INSERT INTO `commande` (`id`, `dateCommande`, `montant`) VALUES
('C001', '2025-04-12', 100),
('C002', '2025-04-12', 150),
('C003', '2025-04-12', 200),
('C004', '2025-04-13', 250),
('C005', '2025-04-13', 300),
('C006', '2025-04-14', 350),
('C007', '2025-04-14', 400),
('C008', '2025-04-14', 450),
('C009', '2025-04-15', 500),
('C011', '2025-04-15', 600),
('C012', '2025-04-16', 650),
('C014', '2025-04-16', 750),
('C015', '2025-04-17', 800),
('C017', '2025-04-20', 475),
('C018', '2025-05-06', 451),
('C019', '2025-05-06', 54),
('C020', '2025-05-06', 5418),
('C021', '2025-05-06', 458),
('C022', '2025-05-06', 541),
('C023', '2025-05-19', 145),
('C024', '2025-05-19', 45),
('C025', '2025-05-19', 454),
('C026', '2025-05-19', 45),
('C027', '2025-05-19', 452);

-- --------------------------------------------------------

--
-- Structure de la table `commandedocument`
--

DROP TABLE IF EXISTS `commandedocument`;
CREATE TABLE IF NOT EXISTS `commandedocument` (
  `id` varchar(5) NOT NULL,
  `id_document` varchar(10) NOT NULL,
  `nbExemplaire` int DEFAULT NULL,
  `idLivreDvd` varchar(10) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `id_commande` varchar(5) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idLivreDvd` (`idLivreDvd`),
  KEY `fk_id_commande` (`id_commande`),
  KEY `fk_commande_document` (`id_document`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `commandedocument`
--

INSERT INTO `commandedocument` (`id`, `id_document`, `nbExemplaire`, `idLivreDvd`, `id_commande`) VALUES
('CD001', '00001', 3, '00003', 'C001'),
('CD002', '00002', 2, '00001', 'C002'),
('CD003', '00003', 4, '00002', 'C003'),
('CD004', '00004', 1, '00003', 'C004'),
('CD005', '00005', 2, '00001', 'C005'),
('CD006', '00006', 5, '00001', 'C006'),
('CD007', '00007', 3, '00001', 'C007'),
('CD008', '00008', 4, '00002', 'C008'),
('CD009', '00009', 2, '00003', 'C009'),
('CD010', '00002', 1, '00001', 'C001'),
('CD011', '00003', 2, '00002', 'C002'),
('CD012', '00004', 3, '00003', 'C003'),
('CD013', '00005', 4, '00001', 'C004'),
('CD014', '00006', 2, '00001', 'C005'),
('CD015', '00007', 5, '00001', 'C006'),
('CD017', '00007', 4, NULL, 'C017'),
('CD018', '00019', 6, NULL, 'C018'),
('CD019', '00008', 4, NULL, 'C019'),
('CD020', '00003', 4, NULL, 'C020'),
('CD021', '00010', 4, NULL, 'C021'),
('CD022', '00002', 4, NULL, 'C022'),
('CD023', '00020', 3, NULL, 'C023'),
('CD024', '00020', 4, NULL, 'C024'),
('CD025', '00020', 5, NULL, 'C025'),
('CD026', '00007', 2, NULL, 'C026'),
('CD027', '00003', 4, NULL, 'C027');

-- --------------------------------------------------------

--
-- Structure de la table `document`
--

DROP TABLE IF EXISTS `document`;
CREATE TABLE IF NOT EXISTS `document` (
  `id` varchar(10) NOT NULL,
  `titre` varchar(60) DEFAULT NULL,
  `image` varchar(500) DEFAULT NULL,
  `idRayon` varchar(5) NOT NULL,
  `idPublic` varchar(5) NOT NULL,
  `idGenre` varchar(5) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idRayon` (`idRayon`),
  KEY `idPublic` (`idPublic`),
  KEY `idGenre` (`idGenre`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `document`
--

INSERT INTO `document` (`id`, `titre`, `image`, `idRayon`, `idPublic`, `idGenre`) VALUES
('00001', 'Quand sort la recluse', '', 'LV003', '00002', '10014'),
('00002', 'Un pays à l\'aube', '', 'LV001', '00002', '10004'),
('00003', 'Et je danse aussi', '', 'LV002', '00003', '10013'),
('00004', 'L\'armée furieuse', '', 'LV003', '00002', '10014'),
('00005', 'Les anonymes', '', 'LV001', '00002', '10014'),
('00006', 'La marque jaune', '', 'BD001', '00003', '10001'),
('00007', 'Dans les coulisses du musée', '', 'LV001', '00003', '10006'),
('00008', 'Histoire du juif errant', '', 'LV002', '00002', '10006'),
('00009', 'Pars vite et reviens tard', '', 'LV003', '00002', '10014'),
('00010', 'Le vestibule des causes perdues', '', 'LV001', '00002', '10006'),
('00011', 'L\'île des oubliés', '', 'LV002', '00003', '10006'),
('00012', 'La souris bleue', '', 'LV002', '00003', '10006'),
('00013', 'Sacré Pêre Noël', '', 'JN001', '00001', '10001'),
('00014', 'Mauvaise étoile', '', 'LV003', '00003', '10014'),
('00015', 'La confrérie des téméraires', '', 'JN002', '00004', '10014'),
('00016', 'Le butin du requin', '', 'JN002', '00004', '10014'),
('00017', 'Catastrophes au Brésil', '', 'JN002', '00004', '10014'),
('00018', 'Le Routard - Maroc', '', 'DV005', '00003', '10011'),
('00019', 'Guide Vert - Iles Canaries', '', 'DV005', '00003', '10011'),
('00020', 'Guide Vert - Irlande', '', 'DV005', '00003', '10011'),
('00021', 'Les déferlantes', '', 'LV002', '00002', '10006'),
('00022', 'Une part de Ciel', '', 'LV002', '00002', '10006'),
('00023', 'Le secret du janissaire', '', 'BD001', '00002', '10001'),
('00024', 'Pavillon noir', '', 'BD001', '00002', '10001'),
('00025', 'L\'archipel du danger', '', 'BD001', '00002', '10001'),
('00026', 'La planète des singes', '', 'LV002', '00003', '10002'),
('10001', 'Arts Magazine', '', 'PR002', '00002', '10016'),
('10002', 'Alternatives Economiques', '', 'PR002', '00002', '10015'),
('10003', 'Challenges', '', 'PR002', '00002', '10015'),
('10004', 'Rock and Folk', '', 'PR002', '00002', '10016'),
('10005', 'Les Echos', '', 'PR001', '00002', '10015'),
('10006', 'Le Monde', '', 'PR001', '00002', '10018'),
('10007', 'Telerama', '', 'PR002', '00002', '10016'),
('10008', 'L\'Obs', '', 'PR002', '00002', '10018'),
('10009', 'L\'Equipe', '', 'PR001', '00002', '10017'),
('10010', 'L\'Equipe Magazine', '', 'PR002', '00002', '10017'),
('10011', 'Geo', '', 'PR002', '00003', '10016'),
('20001', 'Star Wars 5 L\'empire contre attaque', '', 'DF001', '00003', '10002'),
('20002', 'Le seigneur des anneaux : la communauté de l\'anneau', '', 'DF001', '00003', '10019'),
('20003', 'Jurassic Park', '', 'DF001', '00003', '10002'),
('20004', 'Matrix', '', 'DF001', '00003', '10002');

-- --------------------------------------------------------

--
-- Structure de la table `documentunitaire`
--

DROP TABLE IF EXISTS `documentunitaire`;
CREATE TABLE IF NOT EXISTS `documentunitaire` (
  `id` varchar(7) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `id_document` varchar(5) DEFAULT NULL,
  `id_commande` varchar(5) NOT NULL,
  `etat` smallint DEFAULT NULL,
  `dateAchat` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `id_document` (`id_document`),
  KEY `fk_commande_documentunitaire` (`id_commande`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `documentunitaire`
--

INSERT INTO `documentunitaire` (`id`, `id_document`, `id_commande`, `etat`, `dateAchat`) VALUES
('DU00001', '00020', '', 5, '2022-09-06 00:00:00'),
('DU00002', '00014', '', 2, '2023-09-12 00:00:00'),
('DU00003', '00012', '', 5, '2020-02-27 00:00:00'),
('DU00004', '00010', '', 3, '2023-06-08 00:00:00'),
('DU00006', '00021', '', 4, '2023-10-30 00:00:00'),
('DU00007', '00018', '', 4, '2020-04-10 00:00:00'),
('DU00008', '00009', '', 4, '2022-07-06 00:00:00'),
('DU00122', '00019', 'C018', 1, '2025-05-06 00:00:00'),
('DU00010', '00023', '', 2, '2021-05-26 00:00:00'),
('DU00011', '00023', '', 3, '2020-12-29 00:00:00'),
('DU00012', '00010', '', 5, '2021-09-25 00:00:00'),
('DU00013', '00018', '', 2, '2020-12-13 00:00:00'),
('DU00014', '00023', '', 1, '2022-12-15 00:00:00'),
('DU00015', '00023', '', 2, '2023-06-24 00:00:00'),
('DU00017', '00018', '', 1, '2022-06-08 00:00:00'),
('DU00124', '00019', 'C018', 1, '2025-05-06 00:00:00'),
('DU00019', '00021', '', 1, '2022-09-16 00:00:00'),
('DU00020', '00020', '', 5, '2023-10-31 00:00:00'),
('DU00021', '00005', '', 1, '2023-10-25 00:00:00'),
('DU00022', '00003', '', 5, '2021-05-12 00:00:00'),
('DU00023', '00025', '', 3, '2022-12-26 00:00:00'),
('DU00024', '00016', '', 2, '2021-03-10 00:00:00'),
('DU00025', '00002', '', 1, '2021-12-04 00:00:00'),
('DU00026', '00013', '', 1, '2020-02-27 00:00:00'),
('DU00027', '00008', '', 5, '2021-05-16 00:00:00'),
('DU00028', '00005', '', 1, '2020-07-10 00:00:00'),
('DU00029', '00014', '', 2, '2020-05-06 00:00:00'),
('DU00030', '00001', '', 3, '2023-04-13 00:00:00'),
('DU00031', '00022', '', 4, '2020-04-29 00:00:00'),
('DU00032', '00011', '', 2, '2022-05-05 00:00:00'),
('DU00033', '00004', '', 1, '2022-08-19 00:00:00'),
('DU00034', '00011', '', 3, '2020-12-02 00:00:00'),
('DU00035', '00016', '', 2, '2020-06-13 00:00:00'),
('DU00036', '00018', '', 2, '2021-03-20 00:00:00'),
('DU00037', '00026', '', 3, '2020-11-21 00:00:00'),
('DU00038', '00015', '', 5, '2020-10-11 00:00:00'),
('DU00039', '00001', '', 1, '2021-05-20 00:00:00'),
('DU00040', '00018', '', 3, '2023-08-09 00:00:00'),
('DU00041', '00006', '', 4, '2022-10-02 00:00:00'),
('DU00042', '00025', '', 4, '2021-12-01 00:00:00'),
('DU00043', '00007', '', 5, '2021-12-20 00:00:00'),
('DU00044', '00022', '', 1, '2022-04-13 00:00:00'),
('DU00045', '00017', '', 3, '2020-01-24 00:00:00'),
('DU00046', '00008', '', 3, '2021-12-25 00:00:00'),
('DU00047', '00010', '', 4, '2023-09-16 00:00:00'),
('DU00123', '00019', 'C018', 1, '2025-05-06 00:00:00'),
('DU00049', '00017', '', 5, '2021-10-29 00:00:00'),
('DU00050', '00004', '', 2, '2020-09-24 00:00:00'),
('DU00051', '00009', '', 1, '2022-02-25 00:00:00'),
('DU00052', '00015', '', 2, '2022-02-04 00:00:00'),
('DU00053', '00020', '', 4, '2021-12-03 00:00:00'),
('DU00054', '00008', '', 5, '2021-03-24 00:00:00'),
('DU00055', '00004', '', 1, '2023-01-21 00:00:00'),
('DU00056', '00020', '', 5, '2022-12-07 00:00:00'),
('DU00057', '00009', '', 5, '2020-08-11 00:00:00'),
('DU00058', '00022', '', 5, '2023-07-17 00:00:00'),
('DU00059', '00008', '', 2, '2022-10-13 00:00:00'),
('DU00060', '00017', '', 5, '2023-06-18 00:00:00'),
('DU00061', '00019', '', 1, '2022-03-03 00:00:00'),
('DU00062', '00011', '', 4, '2022-02-10 00:00:00'),
('DU00063', '00009', '', 2, '2020-01-01 00:00:00'),
('DU00064', '00011', '', 2, '2023-10-13 00:00:00'),
('DU00065', '00016', '', 4, '2023-02-17 00:00:00'),
('DU00066', '00009', '', 2, '2021-02-20 00:00:00'),
('DU00067', '00013', '', 2, '2021-09-24 00:00:00'),
('DU00068', '00020', '', 5, '2020-07-27 00:00:00'),
('DU00069', '00014', '', 1, '2020-12-23 00:00:00'),
('DU00070', '00002', '', 1, '2022-06-05 00:00:00'),
('DU00071', '00025', '', 1, '2022-10-25 00:00:00'),
('DU00072', '00011', '', 1, '2021-03-19 00:00:00'),
('DU00073', '00023', '', 1, '2021-07-10 00:00:00'),
('DU00074', '00011', '', 1, '2023-10-28 00:00:00'),
('DU00075', '00003', '', 1, '2023-08-14 00:00:00'),
('DU00076', '00010', '', 1, '2021-07-21 00:00:00'),
('DU00077', '00009', '', 1, '2023-04-17 00:00:00'),
('DU00078', '00025', '', 2, '2020-06-27 00:00:00'),
('DU00079', '00018', '', 3, '2022-04-19 00:00:00'),
('DU00080', '00013', '', 5, '2023-10-23 00:00:00'),
('DU00081', '00018', '', 2, '2024-01-19 00:00:00'),
('DU00082', '00023', '', 5, '2022-12-16 00:00:00'),
('DU00083', '00009', '', 3, '2022-10-25 00:00:00'),
('DU00084', '00012', '', 2, '2020-04-27 00:00:00'),
('DU00085', '00003', '', 3, '2020-11-03 00:00:00'),
('DU00086', '00020', '', 5, '2022-01-06 00:00:00'),
('DU00087', '00024', '', 3, '2021-02-03 00:00:00'),
('DU00088', '00017', '', 4, '2022-12-23 00:00:00'),
('DU00089', '00012', '', 2, '2022-10-01 00:00:00'),
('DU00090', '00023', '', 2, '2023-05-07 00:00:00'),
('DU00091', '00021', '', 1, '2020-07-28 00:00:00'),
('DU00092', '00011', '', 4, '2023-03-27 00:00:00'),
('DU00093', '00025', '', 2, '2024-01-19 00:00:00'),
('DU00094', '00006', '', 1, '2022-01-16 00:00:00'),
('DU00095', '00015', '', 3, '2020-03-09 00:00:00'),
('DU00096', '00022', '', 4, '2022-09-22 00:00:00'),
('DU00097', '00006', '', 2, '2022-07-15 00:00:00'),
('DU00098', '00014', '', 5, '2023-12-01 00:00:00'),
('DU00099', '00001', '', 2, '2021-10-08 00:00:00'),
('DU00100', '00013', '', 5, '2020-05-10 00:00:00'),
('DU00101', '00007', '', 1, '2025-04-20 00:00:00'),
('DU00102', '00007', '', 1, '2025-04-20 00:00:00'),
('DU00103', '00007', '', 1, '2025-04-20 00:00:00'),
('DU00104', '00007', '', 1, '2025-04-20 00:00:00'),
('DU00105', '00011', '', 1, '2025-04-21 00:00:00'),
('DU00106', '00011', '', 1, '2025-04-21 00:00:00'),
('DU00107', '00011', '', 1, '2025-04-21 00:00:00'),
('DU00108', '00011', '', 1, '2025-04-21 00:00:00'),
('DU00109', '00011', '', 1, '2025-04-21 00:00:00'),
('DU00110', '00011', '', 1, '2025-04-21 00:00:00'),
('DU00111', '00025', '', 1, '2025-04-28 00:00:00'),
('DU00112', '00003', 'C022', 1, '2025-04-29 00:00:00'),
('DU00113', '00003', 'C022', 1, '2025-04-29 00:00:00'),
('DU00114', '00003', 'C022', 1, '2025-04-29 00:00:00'),
('DU00116', '00008', 'C023', 1, '2025-04-29 00:00:00'),
('DU00117', '00008', 'C023', 1, '2025-04-29 00:00:00'),
('DU00118', '00008', 'C023', 1, '2025-04-29 00:00:00'),
('DU00119', '00008', 'C023', 1, '2025-04-29 00:00:00'),
('DU00120', '00008', 'C023', 1, '2025-04-29 00:00:00'),
('DU00121', '00008', 'C023', 1, '2025-04-29 00:00:00'),
('DU00125', '00019', 'C018', 1, '2025-05-06 00:00:00'),
('DU00126', '00019', 'C018', 1, '2025-05-06 00:00:00'),
('DU00127', '00019', 'C018', 1, '2025-05-06 00:00:00'),
('DU00128', '00008', 'C019', 1, '2025-05-06 00:00:00'),
('DU00129', '00008', 'C019', 1, '2025-05-06 00:00:00'),
('DU00130', '00008', 'C019', 1, '2025-05-06 00:00:00'),
('DU00131', '00008', 'C019', 1, '2025-05-06 00:00:00'),
('DU00132', '00003', 'C020', 1, '2025-05-06 00:00:00'),
('DU00133', '00003', 'C020', 1, '2025-05-06 00:00:00'),
('DU00134', '00003', 'C020', 1, '2025-05-06 00:00:00'),
('DU00135', '00003', 'C020', 1, '2025-05-06 00:00:00'),
('DU00136', '00010', 'C021', 1, '2025-05-06 00:00:00'),
('DU00137', '00010', 'C021', 1, '2025-05-06 00:00:00'),
('DU00138', '00010', 'C021', 1, '2025-05-06 00:00:00'),
('DU00139', '00010', 'C021', 1, '2025-05-06 00:00:00'),
('DU00140', '00002', 'C022', 1, '2025-05-06 00:00:00'),
('DU00141', '00002', 'C022', 1, '2025-05-06 00:00:00'),
('DU00142', '00002', 'C022', 1, '2025-05-06 00:00:00'),
('DU00143', '00002', 'C022', 1, '2025-05-06 00:00:00'),
('DU00144', '00020', 'C023', 1, '2025-05-19 00:00:00'),
('DU00145', '00020', 'C023', 1, '2025-05-19 00:00:00'),
('DU00146', '00020', 'C023', 1, '2025-05-19 00:00:00'),
('DU00147', '00020', 'C024', 1, '2025-05-19 00:00:00'),
('DU00148', '00020', 'C024', 1, '2025-05-19 00:00:00'),
('DU00149', '00020', 'C024', 1, '2025-05-19 00:00:00'),
('DU00150', '00020', 'C024', 1, '2025-05-19 00:00:00'),
('DU00151', '00020', 'C025', 1, '2025-05-19 00:00:00'),
('DU00152', '00020', 'C025', 1, '2025-05-19 00:00:00'),
('DU00153', '00020', 'C025', 1, '2025-05-19 00:00:00'),
('DU00154', '00020', 'C025', 1, '2025-05-19 00:00:00'),
('DU00155', '00020', 'C025', 1, '2025-05-19 00:00:00'),
('DU00156', '00007', 'C026', 1, '2025-05-19 00:00:00'),
('DU00157', '00007', 'C026', 1, '2025-05-19 00:00:00'),
('DU00158', '00007', 'C027', 1, '2025-05-19 00:00:00'),
('DU00159', '00007', 'C027', 1, '2025-05-19 00:00:00'),
('DU00160', '00007', 'C027', 1, '2025-05-19 00:00:00'),
('DU00161', '00007', 'C027', 1, '2025-05-19 00:00:00'),
('DU00162', '00003', 'C027', 4, '2025-05-19 00:00:00'),
('DU00163', '00003', 'C027', 1, '2025-05-19 00:00:00'),
('DU00164', '00003', 'C027', 1, '2025-05-19 00:00:00'),
('DU00165', '00003', 'C027', 1, '2025-05-19 00:00:00');

-- --------------------------------------------------------

--
-- Structure de la table `dvd`
--

DROP TABLE IF EXISTS `dvd`;
CREATE TABLE IF NOT EXISTS `dvd` (
  `id` varchar(10) NOT NULL,
  `synopsis` text,
  `realisateur` varchar(20) DEFAULT NULL,
  `duree` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `dvd`
--

INSERT INTO `dvd` (`id`, `synopsis`, `realisateur`, `duree`) VALUES
('20001', 'Luc est entraîné par Yoda pendant que Han et Leia tentent de se cacher dans la cité des nuages.', 'George Lucas', 124),
('20002', 'L\'anneau unique, forgé par Sauron, est porté par Fraudon qui l\'amène à Foncombe. De là, des représentants de peuples différents vont s\'unir pour aider Fraudon à amener l\'anneau à la montagne du Destin.', 'Peter Jackson', 228),
('20003', 'Un milliardaire et des généticiens créent des dinosaures à partir de clonage.', 'Steven Spielberg', 128),
('20004', 'Un informaticien réalise que le monde dans lequel il vit est une simulation gérée par des machines.', 'Les Wachowski', 136);

-- --------------------------------------------------------

--
-- Structure de la table `etat`
--

DROP TABLE IF EXISTS `etat`;
CREATE TABLE IF NOT EXISTS `etat` (
  `id` char(5) NOT NULL,
  `libelle` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `etat`
--

INSERT INTO `etat` (`id`, `libelle`) VALUES
('00001', 'neuf'),
('00002', 'usagé'),
('00003', 'détérioré'),
('00004', 'inutilisable');

-- --------------------------------------------------------

--
-- Structure de la table `exemplaire`
--

DROP TABLE IF EXISTS `exemplaire`;
CREATE TABLE IF NOT EXISTS `exemplaire` (
  `id` varchar(10) NOT NULL,
  `numero` int NOT NULL,
  `dateAchat` date DEFAULT NULL,
  `photo` varchar(500) NOT NULL,
  `idEtat` char(5) NOT NULL,
  PRIMARY KEY (`id`,`numero`),
  KEY `idEtat` (`idEtat`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `exemplaire`
--

INSERT INTO `exemplaire` (`id`, `numero`, `dateAchat`, `photo`, `idEtat`) VALUES
('10002', 418, '2021-12-01', '', '00001'),
('10007', 3237, '2021-11-23', '', '00001'),
('10007', 3238, '2021-11-30', '', '00001'),
('10007', 3239, '2021-12-07', '', '00001'),
('10007', 3240, '2021-12-21', '', '00001'),
('10011', 505, '2022-10-16', '', '00001'),
('10011', 506, '2021-04-01', '', '00001'),
('10011', 507, '2021-05-03', '', '00001'),
('10011', 508, '2021-06-05', '', '00001'),
('10011', 509, '2021-07-01', '', '00001'),
('10011', 510, '2021-08-04', '', '00001'),
('10011', 511, '2021-09-01', '', '00001'),
('10011', 512, '2021-10-06', '', '00001'),
('10011', 513, '2021-11-01', '', '00001'),
('10011', 514, '2021-12-01', '', '00001');

-- --------------------------------------------------------

--
-- Structure de la table `genre`
--

DROP TABLE IF EXISTS `genre`;
CREATE TABLE IF NOT EXISTS `genre` (
  `id` varchar(5) NOT NULL,
  `libelle` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `genre`
--

INSERT INTO `genre` (`id`, `libelle`) VALUES
('10000', 'Humour'),
('10001', 'Bande dessinée'),
('10002', 'Science Fiction'),
('10003', 'Biographie'),
('10004', 'Historique'),
('10006', 'Roman'),
('10007', 'Aventures'),
('10008', 'Essai'),
('10009', 'Documentaire'),
('10010', 'Technique'),
('10011', 'Voyages'),
('10012', 'Drame'),
('10013', 'Comédie'),
('10014', 'Policier'),
('10015', 'Presse Economique'),
('10016', 'Presse Culturelle'),
('10017', 'Presse sportive'),
('10018', 'Actualités'),
('10019', 'Fantazy');

-- --------------------------------------------------------

--
-- Structure de la table `livre`
--

DROP TABLE IF EXISTS `livre`;
CREATE TABLE IF NOT EXISTS `livre` (
  `id` varchar(10) NOT NULL,
  `ISBN` varchar(13) DEFAULT NULL,
  `auteur` varchar(20) DEFAULT NULL,
  `collection` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `livre`
--

INSERT INTO `livre` (`id`, `ISBN`, `auteur`, `collection`) VALUES
('00001', '1234569877896', 'Fred Vargas', 'Commissaire Adamsberg'),
('00002', '1236547896541', 'Dennis Lehanne', ''),
('00003', '6541236987410', 'Anne-Laure Bondoux', ''),
('00004', '3214569874123', 'Fred Vargas', 'Commissaire Adamsberg'),
('00005', '3214563214563', 'RJ Ellory', ''),
('00006', '3213213211232', 'Edgar P. Jacobs', 'Blake et Mortimer'),
('00007', '6541236987541', 'Kate Atkinson', ''),
('00008', '1236987456321', 'Jean d\'Ormesson', ''),
('00009', '', 'Fred Vargas', 'Commissaire Adamsberg'),
('00010', '', 'Manon Moreau', ''),
('00011', '', 'Victoria Hislop', ''),
('00012', '', 'Kate Atkinson', ''),
('00013', '', 'Raymond Briggs', ''),
('00014', '', 'RJ Ellory', ''),
('00015', '', 'Floriane Turmeau', ''),
('00016', '', 'Julian Press', ''),
('00017', '', 'Philippe Masson', ''),
('00018', '', '', 'Guide du Routard'),
('00019', '', '', 'Guide Vert'),
('00020', '', '', 'Guide Vert'),
('00021', '', 'Claudie Gallay', ''),
('00022', '', 'Claudie Gallay', ''),
('00023', '', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00024', '', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00025', '', 'Ayrolles - Masbou', 'De cape et de crocs'),
('00026', '', 'Pierre Boulle', 'Julliard');

-- --------------------------------------------------------

--
-- Structure de la table `livres_dvd`
--

DROP TABLE IF EXISTS `livres_dvd`;
CREATE TABLE IF NOT EXISTS `livres_dvd` (
  `id` varchar(10) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `livres_dvd`
--

INSERT INTO `livres_dvd` (`id`) VALUES
('00001'),
('00002'),
('00003'),
('00004'),
('00005'),
('00006'),
('00007'),
('00008'),
('00009'),
('00010'),
('00011'),
('00012'),
('00013'),
('00014'),
('00015'),
('00016'),
('00017'),
('00018'),
('00019'),
('00020'),
('00021'),
('00022'),
('00023'),
('00024'),
('00025'),
('00026'),
('20001'),
('20002'),
('20003'),
('20004');

-- --------------------------------------------------------

--
-- Structure de la table `public`
--

DROP TABLE IF EXISTS `public`;
CREATE TABLE IF NOT EXISTS `public` (
  `id` varchar(5) NOT NULL,
  `libelle` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `public`
--

INSERT INTO `public` (`id`, `libelle`) VALUES
('00001', 'Jeunesse'),
('00002', 'Adultes'),
('00003', 'Tous publics'),
('00004', 'Ados');

-- --------------------------------------------------------

--
-- Structure de la table `rayon`
--

DROP TABLE IF EXISTS `rayon`;
CREATE TABLE IF NOT EXISTS `rayon` (
  `id` char(5) NOT NULL,
  `libelle` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `rayon`
--

INSERT INTO `rayon` (`id`, `libelle`) VALUES
('BD001', 'BD Adultes'),
('BL001', 'Beaux Livres'),
('DF001', 'DVD films'),
('DV001', 'Sciences'),
('DV002', 'Maison'),
('DV003', 'Santé'),
('DV004', 'Littérature classique'),
('DV005', 'Voyages'),
('JN001', 'Jeunesse BD'),
('JN002', 'Jeunesse romans'),
('LV001', 'Littérature étrangère'),
('LV002', 'Littérature française'),
('LV003', 'Policiers français étrangers'),
('PR001', 'Presse quotidienne'),
('PR002', 'Magazines');

-- --------------------------------------------------------

--
-- Structure de la table `revue`
--

DROP TABLE IF EXISTS `revue`;
CREATE TABLE IF NOT EXISTS `revue` (
  `id` varchar(10) NOT NULL,
  `periodicite` varchar(2) DEFAULT NULL,
  `delaiMiseADispo` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `revue`
--

INSERT INTO `revue` (`id`, `periodicite`, `delaiMiseADispo`) VALUES
('10001', 'MS', 52),
('10002', 'MS', 52),
('10003', 'HB', 15),
('10004', 'HB', 15),
('10005', 'QT', 5),
('10006', 'QT', 5),
('10007', 'HB', 26),
('10008', 'HB', 26),
('10009', 'QT', 5),
('10010', 'HB', 12),
('10011', 'MS', 52);

-- --------------------------------------------------------

--
-- Structure de la table `suivi`
--

DROP TABLE IF EXISTS `suivi`;
CREATE TABLE IF NOT EXISTS `suivi` (
  `id` varchar(5) NOT NULL,
  `id_commande` varchar(5) NOT NULL,
  `status` smallint NOT NULL COMMENT 'Statuts de la commande',
  `date_suivi` date NOT NULL,
  PRIMARY KEY (`id`),
  KEY `id_commande` (`id_commande`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `suivi`
--

INSERT INTO `suivi` (`id`, `id_commande`, `status`, `date_suivi`) VALUES
('S001', 'C001', 2, '2025-05-19'),
('S002', 'C002', 2, '2025-05-06'),
('S003', 'C003', 3, '2025-04-12'),
('S004', 'C004', 1, '2025-05-06'),
('S005', 'C005', 2, '2025-04-13'),
('S006', 'C006', 3, '2025-04-14'),
('S007', 'C007', 2, '2025-04-19'),
('S008', 'C008', 2, '2025-04-14'),
('S009', 'C009', 3, '2025-04-15'),
('S020', 'C020', 1, '2025-05-06'),
('S011', 'C011', 2, '2025-04-15'),
('S012', 'C012', 3, '2025-04-16'),
('S023', 'C023', 1, '2025-05-19'),
('S014', 'C014', 2, '2025-04-16'),
('S015', 'C015', 3, '2025-04-17'),
('S026', 'C026', 2, '2025-05-19'),
('S018', 'C018', 1, '2025-05-06'),
('S017', 'C017', 1, '2025-04-20'),
('S025', 'C025', 1, '2025-05-19'),
('S024', 'C024', 1, '2025-05-19'),
('S022', 'C022', 1, '2025-05-06'),
('S021', 'C021', 1, '2025-05-06'),
('S019', 'C019', 1, '2025-05-06'),
('S027', 'C027', 1, '2025-05-19');

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

DROP TABLE IF EXISTS `user`;
CREATE TABLE IF NOT EXISTS `user` (
  `id` varchar(5) NOT NULL,
  `username` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `role` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Déchargement des données de la table `user`
--

INSERT INTO `user` (`id`, `username`, `password`, `role`) VALUES
('1', 'Admin', 'Admin', 'ADMIN'),
('2', 'Jorge', 'mdp', 'SALARIE');

-- --------------------------------------------------------

--
-- Structure de la table `userlog`
--

DROP TABLE IF EXISTS `userlog`;
CREATE TABLE IF NOT EXISTS `userlog` (
  `username` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `role` varchar(255) NOT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `commandedocument`
--
ALTER TABLE `commandedocument`
  ADD CONSTRAINT `commandedocument_ibfk_2` FOREIGN KEY (`idLivreDvd`) REFERENCES `livres_dvd` (`id`),
  ADD CONSTRAINT `fk_commande_document` FOREIGN KEY (`id_document`) REFERENCES `livre` (`id`),
  ADD CONSTRAINT `fk_id_commande` FOREIGN KEY (`id_commande`) REFERENCES `commande` (`id`);

--
-- Contraintes pour la table `document`
--
ALTER TABLE `document`
  ADD CONSTRAINT `document_ibfk_1` FOREIGN KEY (`idRayon`) REFERENCES `rayon` (`id`),
  ADD CONSTRAINT `document_ibfk_2` FOREIGN KEY (`idPublic`) REFERENCES `public` (`id`),
  ADD CONSTRAINT `document_ibfk_3` FOREIGN KEY (`idGenre`) REFERENCES `genre` (`id`);

--
-- Contraintes pour la table `dvd`
--
ALTER TABLE `dvd`
  ADD CONSTRAINT `dvd_ibfk_1` FOREIGN KEY (`id`) REFERENCES `livres_dvd` (`id`);

--
-- Contraintes pour la table `exemplaire`
--
ALTER TABLE `exemplaire`
  ADD CONSTRAINT `exemplaire_ibfk_1` FOREIGN KEY (`id`) REFERENCES `document` (`id`),
  ADD CONSTRAINT `exemplaire_ibfk_2` FOREIGN KEY (`idEtat`) REFERENCES `etat` (`id`);

--
-- Contraintes pour la table `livre`
--
ALTER TABLE `livre`
  ADD CONSTRAINT `livre_ibfk_1` FOREIGN KEY (`id`) REFERENCES `livres_dvd` (`id`);

--
-- Contraintes pour la table `livres_dvd`
--
ALTER TABLE `livres_dvd`
  ADD CONSTRAINT `livres_dvd_ibfk_1` FOREIGN KEY (`id`) REFERENCES `document` (`id`);

--
-- Contraintes pour la table `revue`
--
ALTER TABLE `revue`
  ADD CONSTRAINT `revue_ibfk_1` FOREIGN KEY (`id`) REFERENCES `document` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
