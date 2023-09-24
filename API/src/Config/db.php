<?php

namespace App\Config;
use pdo;

class db
{
    private $dbhost = 'mysql-nicolas-trehou.alwaysdata.net';
    private $dbuser = '325970_greensecu';
    private $dbpass = 'greensecure_API';
    private $dbname = 'nicolas-trehou_greensecure';

    public function connect(){

        $prepare_conn_str = "mysql:host=$this->dbhost;dbname=$this->dbname";

        try {
            $dbConn = new PDO($prepare_conn_str, $this->dbuser, $this->dbpass);
            $dbConn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
        } catch (\PDOException $e) {
            die("Erreur de connexion : " . $e->getMessage());
        }

        //return the database connection back
        return $dbConn;
    }
}