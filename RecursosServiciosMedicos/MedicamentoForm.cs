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
    public partial class MedicamentoForm : Form
    {
        public MedicamentoForm()
        {
            InitializeComponent();
        }
        SqlConnection ConnMed = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\RSM\Database\RSM.mdf;Integrated Security=True;Connect Timeout=30");

        private void btnCancelar_Click(object sender, EventArgs e){ this.Hide(); }
        private void bunifuFlatButton2_Click(object sender, EventArgs e) { this.Hide(); }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Insertar a la base de datos la informacion
            if (tbMedicamento.Text != "")
            {
                try
                {
                    ConnMed.Open();
                    SqlCommand cmdMed = new SqlCommand("insert into medicamento (nombre) values('" + tbMedicamento.Text + "');", ConnMed);
                    cmdMed.ExecuteNonQuery();
                    MessageBox.Show("Medicamento agregado a la base de datos.");
                    ConnMed.Close();
                    //Vuelve a cargar en panel principal los ComboBoxes:
                    Principal priMed = new Principal();
                    priMed.LlenaCbDiagnostico();
                    priMed.LlenaCbMedicamento();
                    
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
            if (System.Text.RegularExpressions.Regex.IsMatch(tbMedicamento.Text, @"^[a-zA-Z0-9\s]+$") || tbMedicamento.Text.Length < 1)
            {
            }
            else
            {
                tbMedicamento.Text = tbMedicamento.Text.Remove(tbMedicamento.Text.Length - 1);
            }
        }


    }
}
