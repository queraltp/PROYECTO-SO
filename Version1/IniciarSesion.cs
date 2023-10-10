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
        string usuario, contraseña;
        public IniciarSesion()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e) //Iniciamos la conexión
        {
            //Creamos un IDEndPoint con el IP y puerto del servidor
            IPAddress direc = IPAddress.Parse("192.168.56.102");
            IPEndPoint ipep = new IPEndPoint(direc, 9030);

            //Creamos el socket
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                server.Connect(ipep); //Intentamos conectar el socket
                MessageBox.Show("Conectado");
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

            MessageBox.Show("bienvenid@ ");

            if (trozos[1] == "INCORRECTO")
            {
                MessageBox.Show("Nombre de usuario o contraseña incorrecta.");
            }
            Consultas_BD f2 = new Consultas_BD();
            f2.ShowDialog();
        }

        private void registro_Click(object sender, EventArgs e)
        {
            Registro f1 = new Registro();
            f1.ShowDialog();
        }

    }
}
