<?php

//DAO Users
declare(strict_types=1);

use Psr\Http\Message\ResponseInterface as Response;
use Psr\Http\Message\ServerRequestInterface as Request;
use Slim\Factory\AppFactory;
use Slim\App;
use Slim\Interfaces\RouteCollectorProxyInterface as Group;

return function (App $app) {


    // Routes pour les utilisateurs
    $app->group('/users', function (Group $group) {


        // Sélectionner tous les utilisateurs
        $group->get('', function (Request $request, Response $response) {

            $sql = "SELECT * FROM `user`";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->query($sql);
                $users = $stmt->fetchAll(PDO::FETCH_OBJ);
                $db = null;
                $response->getBody()->write(json_encode($users));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = ["message" => $e->getMessage()];
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });

        $group->get('/user/by-email', function (Request $request, Response $response) {
            try {
                $email = $request->getHeaderLine('Email');

                // Log pour déboguer
                error_log("Email: $email");

                $db = new \App\config\db();
                $conn = $db->connect();

                $sql = "SELECT * FROM `user` WHERE email = :email";
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':email', $email);
                $stmt->execute();

                $user = $stmt->fetch(PDO::FETCH_OBJ);

                // Log pour déboguer
                error_log(print_r($user, true));

                $response->getBody()->write(json_encode($user));

                // Ajouter le header 'Email' à la réponse
                return $response
                    ->withHeader('content-type', 'application/json')
                    ->withHeader('Email', $email)
                    ->withStatus(200);
            } catch (Exception $e) {
                // Log pour déboguer
                error_log($e->getMessage());

                $error = array("message" => $e->getMessage());
                $response->getBody()->write(json_encode($error));

                // Ajouter le header 'Email' à la réponse même en cas d'erreur
                return $response
                    ->withHeader('content-type', 'application/json')
                    ->withHeader('Email', $email) // ajouter ce header à la réponse
                    ->withStatus(500);
            }
        });


// Sélectionner un utilisateur par ID
        $group->get('/{id}', function (Request $request, Response $response, array $args) {

            $id = $args['id'];
            $sql = "SELECT * FROM `user` WHERE UserId = :id";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':id', $id);
                $stmt->execute();
                $user = $stmt->fetch(PDO::FETCH_OBJ);
                $db = null;
                $response->getBody()->write(json_encode($user));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = ["message" => $e->getMessage()];
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });

        // Ajouter un nouvel utilisateur
        $group->post('', function (Request $request, Response $response) {

            $input = $request->getParsedBody();
            $username = $input['Username'];
            $email = $input['Email'];
            $encryptedPassword = $input['EncryptedPassword'];
            $encryptionKey = $input['EncryptionKey'];
            $encryptionIV = $input['EncryptionIV'];
            $sql = "INSERT INTO `user` (`Username`, `Email`, `EncryptedPassword`, `CreatedDate`, `LastLoginDate`, `EncryptionKey`, `EncryptionIV`, `IsLocked`, `FailedLoginAttempts`) VALUES (:username, :email, :encryptedPassword, NOW(), NOW(), :encryptionKey, :encryptionIV, False, 0)";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':username', $username);
                $stmt->bindParam(':email', $email);
                $stmt->bindParam(':encryptedPassword', $encryptedPassword);
                $stmt->bindParam(':encryptionKey', $encryptionKey);
                $stmt->bindParam(':encryptionIV', $encryptionIV);
                $result = $stmt->execute();
                $db = null;
                $response->getBody()->write(json_encode($result));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = ["message" => $e->getMessage()];
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });
// Mettre à jour un utilisateur par ID
        $group->put('/{id}', function (Request $request, Response $response, array $args) {

            $input = $request->getParsedBody();
            $id = $args['id'];
            $username = $input['Username'];
            $email = $input['Email'];
            $encryptedPassword = $input['EncryptedPassword'];
            $lastLoginDate = $input['LastLoginDate'];
            $isLocked = $input['IsLocked'];
            $failedLoginAttempts = $input['FailedLoginAttempts'];
            $sql = "UPDATE `user` SET `Username` = :username, `Email` = :email, `EncryptedPassword` = :encryptedPassword, `LastLoginDate` = :lastLoginDate, `IsLocked` = :isLocked, `FailedLoginAttempts` = :failedLoginAttempts WHERE `UserId` = :id";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':id', $id);
                $stmt->bindParam(':username', $username);
                $stmt->bindParam(':email', $email);
                $stmt->bindParam(':encryptedPassword', $encryptedPassword);
                $stmt->bindParam(':lastLoginDate', $lastLoginDate);
                $stmt->bindParam(':isLocked', $isLocked, \PDO::PARAM_BOOL);
            // Assuming IsLocked is boolean
                $stmt->bindParam(':failedLoginAttempts', $failedLoginAttempts, \PDO::PARAM_INT);
            // Assuming FailedLoginAttempts is int
                $result = $stmt->execute();
                $db = null;
                $response->getBody()->write(json_encode($result));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = ["message" => $e->getMessage()];
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });
// Supprimer un utilisateur par ID
        $group->delete('/{id}', function (Request $request, Response $response, array $args) {

            $id = $args['id'];
            $sql = "DELETE FROM `user` WHERE `UserId` = :id";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':id', $id);
                $result = $stmt->execute();
                $db = null;
                $response->getBody()->write(json_encode($result));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = ["message" => $e->getMessage()];
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });
    });
};
