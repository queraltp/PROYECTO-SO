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
    public partial class IniciarSesion : Form
    {
        Socket server;
        public IniciarSesion()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e) //Iniciamos la conexión
        {
            //Creamos un IDEndPoint con el IP y puerto del servidor
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9190);

            //Creamos el socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep); //Intentamos conectar el socket
                MessageBox.Show("Se ha establecido conexión con el servidor");
            }
            catch (SocketException exc)
            {
                MessageBox.Show("No se ha podido conectar con el servidor");
                return;
            }
        }

        private void inicioSesion_Click(object sender, EventArgs e)
        {
            string mensaje = "1/" + textBox1.Text + "/" + textBox2.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];

            string[] trozos = mensaje.Split('/');
            //int codigo = Convert.ToInt32(trozos[0]);

            if (trozos[1] == "INCORRECTO")
            {
                MessageBox.Show("Nombre de usuario o contraseña incorrecta.");
            }
            else
            {
                Consultas_BD f2 = new Consultas_BD(server);
                f2.ShowDialog();
            }
               
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void registro_Click(object sender, EventArgs e)
        {
            //Hay que mandar el socket server al otro formulario
            Registro f1 = new Registro(server);
            f1.ShowDialog();
        }

    }
}
