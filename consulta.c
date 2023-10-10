//programa en C para consultar los datos de la base de datos
//Incluir esta libreria para poder hacer las llamadas en shiva2.upc.es
//#include <my_global.h>
#include <mysql.h>
#include <string.h>
#include <stdlib.h>
#include <unistd.h>
#include <stdlib.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <stdio.h>
#include <ctype.h>
#include<stdbool.h>  
#include <pthread.h>
int main(int argc, char **argv)
{
	//Conexion mediante sockets con el cliente
	int sock_conn, sock_listen, ret;
	struct sockaddr_in serv_adr;
	char peticion[512];
	char respuesta[512];
	char usuario[80];
	char contrasena[80];
	if((sock_listen=socket(AF_INET, SOCK_STREAM, 0))<0)
		printf("Error creando el socket\n");
	
	memset(&serv_adr,0, sizeof(serv_adr));
	serv_adr.sin_family = AF_INET;
	
	serv_adr.sin_addr.s_addr = htonl(INADDR_ANY);
	serv_adr.sin_port = htons(9030);
	if(bind(sock_listen, (struct sockaddr *)&serv_adr, sizeof(serv_adr))<0)
		printf("Error al bind \n");
	
	if(listen(sock_listen, 3)<0)
		printf("Error en el listen \n");

	// Estructura especial para almacenar resultados de consultas 
	MYSQL_RES *resultado;
	MYSQL_ROW row;
	//Creamos una conexion al servidor MYSQL 
	MYSQL *conn;
	conn = mysql_init(NULL);
	if (conn==NULL) {
		printf ("Error al crear la conexion: %u %s\n", 
				mysql_errno(conn), mysql_error(conn));
		exit (1);
	}
	int Registro (char usuario[20],char contrasena[20])
	{
		char consulta[80];
		char respuesta[512];
		printf ("Nombre: %s, Contrase\uffc3\uffb1a: %s\n", usuario, contrasena);
		sprintf(consulta, "SELECT jugadores.username FROM jugadores WHERE jugadores.username='%s' ",usuario);
		// hacemos la consulta
		int err =mysql_query (conn, consulta);
		if (err!=0)
		{printf ("Error al consultar datos de la base %u %s\n",
				 mysql_errno(conn), mysql_error(conn));
		exit (1);
		}
		//Recogemos el resultado
		resultado = mysql_store_result (conn);
		row=mysql_fetch_row(resultado);
		if (row==NULL)
		{//Consultamos el numero de usuarios registrados para obtener el ultimo id
/*			strcpy(consulta_id4,"SELECT MAX(jugadores.id) FROM (jugador)");*/
/*			err=mysql_query (conn, consulta_id4);*/
/*			if (err!=0)*/
/*			{*/
/*				printf ("Error al consultar datos de la base %u %s\n",*/
/*						mysql_errno(conn), mysql_error(conn));*/
/*				exit (1);*/
/*			}*///Recogemos el resultado de la consulta de id
/*			resultado = mysql_store_result (conn);*/
/*			row = mysql_fetch_row (resultado);*/
			
			int idJugador=atoi(row[0])+1;
			char insert[150];
			sprintf(insert,"INSERT INTO jugadores(id_jugador,username,pass) VALUES (%d,'%s','%s')",idJugador,usuario,contrasena);
			
			err=mysql_query (conn, insert);
			if (err!=0){
				printf ("Error al insertar datos de la base %u %s\n",
						mysql_errno(conn), mysql_error(conn));
				exit (1);
				sprintf(respuesta,"4/NO INSERTADO");          		 
			}
			else
			{
				sprintf(respuesta,"4/SI");
			}
		}
		else
		{
			sprintf(respuesta,"4/NO REGISTRADO");
		}  		 
	}
	int terminar = 0;
	while(terminar == 0) //Bucle de atencion al cliente
	{
		printf("Escuchando \n");
		
		sock_conn = accept(sock_listen, NULL, NULL);
		printf("He recibido conexion \n");
		
		ret = read(sock_conn, peticion, sizeof(peticion));
		printf("Recibido \n");
		
		peticion[ret]='\0';
		
		printf("Peticion %s \n", peticion);
		
		char *p = strtok(peticion, "/");
		int codigo = atoi(p);
		int err;
		int cont;
		char jugador1 [10];
		char jugador2 [10];
		char consulta [80];
		//inicializar la conexion
		conn = mysql_real_connect (conn, "localhost","root", "mysql", "juego",0, NULL, 0);
		if (conn==NULL) {
			printf ("Error al inicializar la conexion: %u %s\n", 
					mysql_errno(conn), mysql_error(conn));
			exit (1);
		}
		
		if (codigo == 1) //Consulta 1
		{
			p=strtok(NULL,"/");
			strcpy(usuario,p);
			p=strtok(NULL,"/");
			strcpy(contrasena,p);
			printf ("Codigo: %d, Nombre: %s, Contrase\ufff1a: %s\n", codigo, usuario, contrasena);
			sprintf(consulta, "SELECT jugadores.username, jugadores.pass FROM jugadores WHERE (jugadores.username='%s' AND jugadores.pass='%s')",usuario,contrasena);
			err=mysql_query(conn, consulta);
			if (err!=0){
				printf ("Error al consultar datos de la base %u %s\n", mysql_errno(conn),mysql_error(conn));
				exit(1);
			}
			//Recogemos el resultado de la consulta
			resultado = mysql_store_result(conn);
			row = mysql_fetch_row(resultado);
			if (row == NULL)
			{
				printf("No se ha obtenido la consulta \n");
				sprintf(respuesta,"1/INCORRECTO");
			}
			else
			{
				printf("Inicio de sesion completado \n");
				sprintf(respuesta,"1/CORRECTO");
				
			}
			write(sock_conn, respuesta, strlen(respuesta));
		}
		else if(codigo==2) //Registro de nuevo usuario
		{
				p = strtok( NULL, "/");
				strcpy (usuario, p);
				p = strtok( NULL, "/");
				strcpy(contrasena,p);
				Registro(usuario,contrasena);
				// Enviamos la respuesta
				printf("Respuesta: %s \n", respuesta);
				write (sock_conn,respuesta, strlen(respuesta));
		}
		else if(codigo ==3)
		{
			//construimos la consulta SQL
			printf ("Dime cuantas partidas ha jugado el usuario:\n"); 
			scanf ("%s", jugador1);
			sprintf(consulta,"SELECT COUNT(partidas.id_partidas) FROM jugadores, participaciones, partidas WHERE jugadores.username = '%s'  AND jugadores.id_jugador = participaciones.id_jugador AND participaciones.id_partida = partidas.id_partidas", jugador1); 
			//strcpy(jugador1, p);
			/*scanf("%s", jugador2);*/
			printf ("consulta: %s\n", consulta);
			err=mysql_query (conn, consulta); 
			//recogemos el resultado de la consulta 
			resultado = mysql_store_result (conn); 
			row = mysql_fetch_row (resultado);
			if (row == NULL)
				printf ("No se han obtenido datos en la consulta\n");
			else{
				//El resultado debe ser una matriz con una sola fila
				//y una columna que contiene el numero de partidas
				printf ("Numero de partidas: %s\n", row[0] );
			}
			if (err!=0) 
			{
				printf ("Error al consultar datos de la base %u %s\n",mysql_errno(conn), mysql_error(conn));
				exit (1);
			}
		}
		
		
	}
	
	// cerrar la conexion con el servidor 
	close(sock_conn);
}


