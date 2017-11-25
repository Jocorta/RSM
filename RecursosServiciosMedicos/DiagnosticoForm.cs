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
namespace RecursosServiciosMedicos
{
    public partial class DiagnosticoForm : Form
    {
        public DiagnosticoForm()
        {
            InitializeComponent();
        }
        SqlConnection ConnDia = new SqlConnection("Data Source=(LocalDb)\\LocalDBDemo;initial catalog=RSM;integrated security=true");

        private void btnCancelar_Click_1(object sender, EventArgs e) { this.Hide(); }
        private void btnCancelar_Click(object sender, EventArgs e){ this.Hide(); }

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
            if (tbDiagnostico.Text != "")
            {
                try
                {
                    ConnDia.Open();
                    SqlCommand cmdMed = new SqlCommand("insert into diagnostico (nombre) values('" + tbDiagnostico.Text + "');", ConnDia);
                    cmdMed.ExecuteNonQuery();
                    MessageBox.Show("Diagnostico agregado a la base de datos.");
                    ConnDia.Close();
                    //Vuelve a cargar en panel principal los ComboBoxes:
                    Principal priDia = new Principal();
                    priDia.LlenaCbDiagnostico();
                    priDia.LlenaCbMedicamento();
                    // El form se cierra una vez se agrego el nuevo valor
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrio el siguiente problema: " + ex.Message + ". Contecte al administrador." + "\t" + ex.GetType());
                }
            }
        }

        private void tbOtroNombre_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbDiagnostico.Text, @"^[a-zA-Z0-9\s]+$") || tbDiagnostico.Text.Length < 1)
            {
            }
            else
            {
                tbDiagnostico.Text = tbDiagnostico.Text.Remove(tbDiagnostico.Text.Length - 1);
            }
        }
    }
}
