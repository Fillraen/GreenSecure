

CREATE TABLE User (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    Username VARCHAR(255) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    EncryptedPassword TEXT NOT NULL,
    CreatedDate DATETIME,
    LastLoginDate DATETIME,
    IsLocked BOOLEAN DEFAULT FALSE,
    FailedLoginAttempts INT DEFAULT 0,
    EncryptionKey TEXT NOT NULL,
    EncryptionIV TEXT NOT NULL
) ENGINE=InnoDB;

CREATE TABLE Credentials (
    Id INT PRIMARY KEY AUTO_INCREMENT,
    IdUser INT,
    Username VARCHAR(255) NOT NULL,
    EmailAddress VARCHAR(255),
    Url TEXT,
    Name VARCHAR(255),
    EncryptedPassword TEXT NOT NULL,
    Domain VARCHAR(255),
    Category VARCHAR(255),
    DateCreated DATETIME,
    LastModified DATETIME,
    Complexity INT,
    FOREIGN KEY (IdUser) REFERENCES User(UserId) ON DELETE CASCADE
) ENGINE=InnoDB;

INSERT INTO User (Username, Email, EncryptedPassword, CreatedDate, LastLoginDate, EncryptionKey, EncryptionIV)
VALUES 
    ('user1', 'user1@example.com', 'encryptedpassword1', NOW(), NOW(), 'EncryptionKey1', 'EncryptionAngle1'),
    ('user2', 'user2@example.com', 'encryptedpassword2', NOW(), NOW(), 'EncryptionKey2', 'EncryptionAngle2'),
    ('user3', 'user3@example.com', 'encryptedpassword3', NOW(), NOW(), 'EncryptionKey3', 'EncryptionAngle3'),
    ('user4', 'user4@example.com', 'encryptedpassword4', NOW(), NOW(), 'EncryptionKey4', 'EncryptionAngle4');


INSERT INTO Credentials (IdUser, Username, EmailAddress, Url, Name, EncryptedPassword, Domain, Category, DateCreated, LastModified, Complexity)
VALUES 
    (1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (1, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100),
    (1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (2, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100),
    (2, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (2, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100),
    (3, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (3, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100),
    (3, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (4, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100),
    (4, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (4, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100),
    (1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (1, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100),
    (1, 'user1_username1', 'email1_1@example.com', 'http://example1.com', 'name1', 'encryptedpassword_for_cred1', 'Website', 'Finance', NOW(), NOW(), 80),
    (1, 'user1_username2', 'email1_2@example.com', 'http://example2.com', 'name2', 'encryptedpassword_for_cred2', 'Software', 'Game', NOW(), NOW(), 100);
  
SELECT * FROM User;
Select * FROM Credentials;
  
-- Sélectionner un utilisateur par e-mail
SELECT * FROM User WHERE Email = 'user1@example.com';

-- Sélectionner tous les mots de passe d'un utilisateur spécifique (par ID)
SELECT * FROM Credentials WHERE IdUser = 1;

-- Sélectionner un mot de passe spécifique par son ID
SELECT * FROM Credentials WHERE Id = 1;


-- Insertion d'un nouvel utilisateur
INSERT INTO User (Username, Email, EncryptedPassword, CreatedDate, LastLoginDate, EncryptionKey, EncryptionIV) 
VALUES 
('testuser', 'test@example.com', 'encryptedpasswordtest', NOW(), NOW(), 'EncryptionKey1', 'EncryptionAngle1');

-- Vérifier l'insertion
SELECT * FROM User WHERE Email = 'test@example.com';

-- Mise à jour du nom d'utilisateur
UPDATE User SET Username = 'modifieduser' WHERE Email = 'test@example.com';

-- Vérifier la mise à jour
SELECT * FROM User WHERE Email = 'test@example.com';

-- Suppression de l'utilisateur (ce qui entraînera également la suppression de ses mots de passe grâce à ON DELETE CASCADE)
DELETE FROM User WHERE Email = 'test@example.com';

-- Vérifier la suppression
SELECT * FROM User WHERE Email = 'test@example.com'; -- Devrait retourner aucune ligne
SELECT * FROM Credentials WHERE IdUser = 5;
