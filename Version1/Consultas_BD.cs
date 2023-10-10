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
        public Consultas_BD()
        {
            InitializeComponent();
        }

        private void Consultas_BD_Load(object sender, EventArgs e)
        {

        }

        private void conectar_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) //Boton aceptar
        {
            if (radioButton2.Checked)
            {
                // Quiere saber la longitud
                string mensaje = "4/" + textBox1.Text;
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
    }
}
