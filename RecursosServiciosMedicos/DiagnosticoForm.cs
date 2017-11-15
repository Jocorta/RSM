using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecursosServiciosMedicos
{
    public partial class DiagnosticoForm : Form
    {
        public DiagnosticoForm()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            //Insertar a la base de datos la informacion
        }

        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void tbOtroNombre_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroNombre.Text, "^[a-zA-Z0-9]+$") || tbOtroNombre.Text.Length < 1)
            {
            }
            else
            {
                tbOtroNombre.Text = tbOtroNombre.Text.Remove(tbOtroNombre.Text.Length - 1);
            }
        }
    }
}
