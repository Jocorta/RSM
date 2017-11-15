using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace RecursosServiciosMedicos
{
    public partial class Form1 : Form
    {
        
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-48PLDOP;initial catalog=RSM;integrated security=true");

        Boolean UserEnter = true, PasswordEnter = true;
        public Form1()
        {
            InitializeComponent();
            tbContraseña._TextBox.PasswordChar = '*';

        }

        private void tbUsuario_Enter(object sender, EventArgs e)
        {
            if (UserEnter)
            {
                tbUsuario.text = "";
                UserEnter = false;
            }
        }

        private void tbContraseña_Enter(object sender, EventArgs e)
        {
            if (PasswordEnter)
            {
                tbContraseña.text = "";
                PasswordEnter = false;
            }
        }

        private void tbUsuario_Leave(object sender, EventArgs e)
        {
            if (tbUsuario.text == "")
            {
                tbUsuario.text = "Usuario";
                UserEnter = true;
            }
        }

        private void tbContraseña_Leave(object sender, EventArgs e)
        {
            if (tbContraseña.text == "")
            {
                tbContraseña.text = "Contraseña";
                PasswordEnter = true;
            }
        }

        private void tbUsuario_OnTextChange(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbUsuario.text, "^[a-zA-Z]+$") || tbUsuario.text.Length < 1)
            {
            }
            else
            {
                tbUsuario.text = tbUsuario.text.Remove(tbUsuario.text.Length - 1);
            }
        }

        private void tbUsuario_KeyPress(object sender, EventArgs e)
        {
            //Codigo validacion:
            
        }

        private void tbUsuario_KeyDown(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {            
        }

        private void tbContraseña_OnTextChange(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbContraseña.text, "^[a-zA-Z]+$") || tbContraseña.text.Length < 1)
            {
            }
            else
            {
                tbContraseña.text = tbContraseña.text.Remove(tbContraseña.text.Length - 1);
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Cerrar aplicación Registro Servicios Médicos?", "Salir", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            login();

        }

        private void login()
        {
            bool login = false;
            con.Open();

            //Login Query
            string sql = "select* from usuario where usuario = '" + tbUsuario.text + "' and contraseña = '" + tbContraseña.text + "'";

            //Database connection
            SqlCommand cmdDatabase = new SqlCommand(sql, con);

            SqlDataReader myReader = cmdDatabase.ExecuteReader();
            while (myReader.Read())
            {
                login = true;
            }

            myReader.Close();
            con.Close();

            //Login Validation

            if (login)
            {
                MessageBox.Show("Inicio de sesion correcto", "Bienvenido", MessageBoxButtons.OK);
                this.Hide();
                Principal pr = new Principal(tbUsuario.text);
                pr.Show();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                login();
            }
        }

        private void tbContraseña_KeyPress(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
    }
}
