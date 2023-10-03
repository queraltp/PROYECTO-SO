DROP DATABASE IF EXISTS juego;
CREATE DATABASE juego; 

USE juego;

CREATE TABLE jugadores {

username VARCHAR(20) NOT NULL,
password VARCHAR(20)NOT NULL,
PRIMARY KEY(username,password),

}ENGINE Innojuego;

/*será nuestra tabla puente para saber cuantas partidas ha hecho X jugador
*/

CREATE TABLE participaciones {

id_jugador VARCHAR(20),
num_partidas INT,
num_participaciones INT,
PRIMARY KEY(num_participaciones),
FOREIGN KEY id_jugador REFERENCES jugadores(username),
FOREIGN KEY num_partidas REFERENCES partidas(id_partidas),

}ENGINE Innojuego;

CREATE TABLE partidas {

id_partidas INT,
fecha_hora_fin DATE,
duracion FLOAT,
ganador VARCHAR(20),

}ENGINE Innojuego;

INSERT INTO jugadores VALUES ('crisbelbo', 1234);
INSERT INTO jugadores VALUES ('user1', 4321);
INSERT INTO jugadores VALUES ('user2', 1234);
INSERT INTO jugadores VALUES ('user3', 1234);
INSERT INTO partidas VALUES (1, 03/10/2023, 1.3, 'user1');


