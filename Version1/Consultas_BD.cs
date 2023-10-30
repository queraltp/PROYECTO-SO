using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;

namespace Version1
{
    public partial class Consultas_BD : Form
    {
        Socket server;
        public Consultas_BD(Socket server)
        {
            InitializeComponent();
            this.server = server;
        }

        private void Consultas_BD_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Bienvenid@ al juego");
        }

        private void conectar_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //Boton aceptar
        {
            if (radioButton2.Checked)
            {
                // Quiere saber la longitud
                string mensaje = "3/" + textBox1.Text;
                // Enviamos al servidor el nombre tecleado
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
                server.Send(msg);

                //Recibimos la respuesta del servidor
                byte[] msg2 = new byte[80];
                server.Receive(msg2);
                mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
                MessageBox.Show("Ha ganado: " + mensaje +"partidas");
            }
            
        }

        private void button2_Click(object sender, EventArgs e) //Desconectarme
        {
            string mensaje = "0/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            MessageBox.Show("Te has desconectado");
            //Cerramos la conexión
            server.Shutdown(SocketShutdown.Both);
            server.Close();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e) //Cuantos servicios
        {
            //Pedir numero de servicios realizados
            string mensaje = "4/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            label3.Text = mensaje;
        }

        private void button4_Click(object sender, EventArgs e) //Boton para la lista de conectados
        {
            string mensaje = "5/";
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);
            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

            //DataTable dt = miBase.GetUsuario(usuario_inicial);
            //dataGridView1.DataSource = dt;
            //dataGridView1.Refresh();
        }
    }
}
