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
    public partial class Registro : Form
    {
        Socket server;
        
        public Registro(Socket server)
        {
            InitializeComponent();
            this.server = server;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Aceptar_Click(object sender, EventArgs e)
        {
            string mensaje = "2/" + usuarioIn.Text + "/" + contraseñaIn.Text;
            // Enviamos al servidor el nombre tecleado
            byte[] msg = System.Text.Encoding.ASCII.GetBytes(mensaje);
            this.server.Send(msg);

            //Recibimos la respuesta del servidor
            byte[] msg2 = new byte[80];
            this.server.Receive(msg2);
            mensaje = Encoding.ASCII.GetString(msg2).Split('\0')[0];
            if(mensaje == "SI")
            {
                MessageBox.Show("Usuario registrado correctamente");
            }
        }
    }
}
