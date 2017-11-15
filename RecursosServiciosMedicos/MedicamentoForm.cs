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
    public partial class MedicamentoForm : Form
    {
        public MedicamentoForm()
        {
            InitializeComponent();
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

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Insertar a la base de datos la informacion
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
