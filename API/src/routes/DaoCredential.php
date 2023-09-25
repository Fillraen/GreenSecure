<?php
//DAO Credentials
declare(strict_types=1);

use Psr\Http\Message\ResponseInterface as Response;
use Psr\Http\Message\ServerRequestInterface as Request;
use Slim\Factory\AppFactory;
use Slim\App;
use Slim\Interfaces\RouteCollectorProxyInterface as Group;

return function (App $app) {
    // Routes pour les informations d'identification (credentials)
    $app->group('/credentials', function (Group $group) {

        // Sélectionner tous les mots de passe
        $group->get('', function (Request $request, Response $response) {
            $sql = "SELECT * FROM `credentials`";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->query($sql);
                $credentials = $stmt->fetchAll(PDO::FETCH_OBJ);
                $db = null;
                $response->getBody()->write(json_encode($credentials));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = array("message" => $e->getMessage());
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });

        $group->get('/user/{id}',function (Request $request, Response $response, array $args){
            $id = $args['id'];
            $sql = "SELECT * FROM `credentials` WHERE IdUser = :id";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':id', $id);
                $stmt->execute();
                $credential = $stmt->fetchAll(PDO::FETCH_OBJ);
                $db = null;
                $response->getBody()->write(json_encode($credential));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = array("message" => $e->getMessage());
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });


        // Sélectionner un mot de passe par ID
        $group->get('/{id}', function (Request $request, Response $response, array $args) {
            $id = $args['id'];
            $sql = "SELECT * FROM `credentials` WHERE Id = :id";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':id', $id);
                $stmt->execute();
                $credential = $stmt->fetchAll(PDO::FETCH_OBJ);
                $db = null;
                $response->getBody()->write(json_encode($credential));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = array("message" => $e->getMessage());
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });

        // Ajouter un nouveau mot de passe
        $group->post('', function (Request $request, Response $response) {
            $input = $request->getParsedBody();
            $idUser = $input['IdUser'];
            $username = $input['Username'];
            $emailAddress = $input['Email'];
            $url = $input['Url'];
            $name = $input['Name'];
            $encryptedPassword = $input['EncryptedPassword'];
            $domain = $input['Domain'];
            $category = $input['Category'];
            $complexity = $input['Complexity'];

            $sql = "INSERT INTO `credentials` (`IdUser`, `Username`, `EmailAddress`, `Url`, `Name`, `EncryptedPassword`, `Domain`, `Category`, `DateCreated`, `LastModified`, `Complexity`) VALUES (:idUser, :username, :emailAddress, :url, :name, :encryptedPassword, :domain, :category, NOW(), NOW(), :complexity)";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':idUser', $idUser);
                $stmt->bindParam(':username', $username);
                $stmt->bindParam(':emailAddress', $emailAddress);
                $stmt->bindParam(':url', $url);
                $stmt->bindParam(':name', $name);
                $stmt->bindParam(':encryptedPassword', $encryptedPassword);
                $stmt->bindParam(':domain', $domain);
                $stmt->bindParam(':category', $category);
                $stmt->bindParam(':complexity', $complexity, \PDO::PARAM_INT);
                $result = $stmt->execute();
                $db = null;
                $response->getBody()->write(json_encode($result));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = array("message" => $e->getMessage());
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });

        // Mettre à jour un mot de passe par ID
        $group->put('/{id}', function (Request $request, Response $response, array $args) {
            $input = $request->getParsedBody();
            $id = $args['id'];
            $idUser = $input['IdUser'];
            $username = $input['Username'];
            $emailAddress = $input['EmailAddress'];
            $url = $input['Url'];
            $name = $input['Name'];
            $encryptedPassword = $input['EncryptedPassword'];
            $domain = $input['Domain'];
            $category = $input['Category'];
            $complexity = $input['Complexity'];

            $sql = "UPDATE `credentials` SET `IdUser` = :idUser, `Username` = :username, `EmailAddress` = :emailAddress, `Url` = :url, `Name` = :name, `EncryptedPassword` = :encryptedPassword, `Domain` = :domain, `Category` = :category, `LastModified` = NOW(), `Complexity` = :complexity WHERE `Id` = :id";
            try {
                $db = new \App\config\db();
                $conn = $db->connect();
                $stmt = $conn->prepare($sql);
                $stmt->bindParam(':id', $id);
                $stmt->bindParam(':idUser', $idUser);
                $stmt->bindParam(':username', $username);
                $stmt->bindParam(':emailAddress', $emailAddress);
                $stmt->bindParam(':url', $url);
                $stmt->bindParam(':name', $name);
                $stmt->bindParam(':encryptedPassword', $encryptedPassword);
                $stmt->bindParam(':domain', $domain);
                $stmt->bindParam(':category', $category);
                $stmt->bindParam(':complexity', $complexity, \PDO::PARAM_INT);
                $result = $stmt->execute();
                $db = null;
                $response->getBody()->write(json_encode($result));
                return $response->withHeader('content-type', 'application/json')->withStatus(200);
            } catch (PDOException $e) {
                $error = array("message" => $e->getMessage());
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });

        // Supprimer un mot de passe par ID
        $group->delete('/{id}', function (Request $request, Response $response, array $args) {
            $id = $args['id'];
            $sql = "DELETE FROM `credentials` WHERE `Id` = :id";
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
                $error = array("message" => $e->getMessage());
                $response->getBody()->write(json_encode($error));
                return $response->withHeader('content-type', 'application/json')->withStatus(500);
            }
        });

    });
};

