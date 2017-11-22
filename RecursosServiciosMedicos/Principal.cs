﻿using System;
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
    public partial class Principal : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private string input = "";
        public string Usuario;
        DiagnosticoForm diagnosticoFormObjeto = new DiagnosticoForm();
        MedicamentoForm medicamentoFormObjeto = new MedicamentoForm();
        DataTable dsDiagnostico = new DataTable();
        DataTable dsMedicamento = new DataTable();
        DataTable dsMedicamento2 = new DataTable();
        DataTable dsMedicamento3 = new DataTable();
        bool Med2 = false;
        bool Med3 = false;




        public string nombre = "", num_id = "", num_control = "", num_docente = "", seguimiento = "", fecha = "", medicamento = "", diagnostico = "", num_otro = "", edad = "", sexo = "",doctor="";
        public bool RegistroSeleccionado = false, banderaalumno;
        public int tipo = 0;

        public Principal()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=(LocalDb)\LocalDBDemo;initial catalog=RSM;integrated security=true");//conexion base de datos

        public Principal(string LoggedUser)
        {
            InitializeComponent();
            lblFecha.Text = DateTime.Now.ToString("MM/dd/yyyy");
            btnCerrar.Text = LoggedUser;
            Usuario = LoggedUser;
            
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            LlenaCbDiagnostico();
            LlenaCbMedicamento();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("¿Cerrar sesión?", "Salir", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Hide();
                Form1 fm1 = new Form1();
                fm1.Show();
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            Separator.Location = new Point(15, 177);
            Separator.Show();
            LimpiaAlumno();
            LimpiaDocente();
            LimpiaOtro();
            pnlConsulta.Show();
            pnlCertificado.Hide();

        }

        private void cbAlumno_OnChange(object sender, EventArgs e)
        {
            if (cbAlumno.Checked)
            {
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
                cbDocente.Checked = false;
                cbOtro.Checked = false;
                cbSeguimiento.Checked = false;
                pnlAlumno.Show();
                pnlDocente.Hide();
                pnlOtro.Hide();
                Med2 = false;
                Med3 = false;
                ddbAlumnoMedicamento2.Hide();
                ddbAlumnoMedicamento3.Hide();
                lblAlumnoMed2.Hide();
                lblAlumnoMed3.Hide();
            }
        }

        private void cbDocente_OnChange(object sender, EventArgs e)
        {
            if (cbDocente.Checked)
            {
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
                cbAlumno.Checked = false;
                cbOtro.Checked = false;
                cbSeguimiento.Checked = false;
                pnlAlumno.Hide();
                pnlDocente.Show();
                pnlOtro.Hide();
                Med2 = false;
                Med3 = false;
                ddbDocenteMedicamento2.Hide();
                lblDocMed2.Hide();
                ddbDocenteMedicamento3.Hide();
                lblDocMed3.Hide();
            }
        }

        private void cbOtro_OnChange(object sender, EventArgs e)
        {
            if (cbOtro.Checked)
            {
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
                cbDocente.Checked = false;
                cbAlumno.Checked = false;
                cbSeguimiento.Checked = false;
                pnlAlumno.Hide();
                pnlDocente.Hide();
                pnlOtro.Show();
                Med2 = false;
                Med3 = false;
                cbOtroMedicamento2.Hide();
                lblMed2.Hide();
                cbOtroMedicamento3.Hide();
                lblMed3.Hide();
            }
        }

        private void btnCertificadoMed_Click(object sender, EventArgs e)
        {

            Separator.Location = new Point(15, 272);
            Separator.Show();
            pnlConsulta.Hide();
            pnlCertificado.Show();

            tbCodigoCerti.Text = "";
            tbCodigoCerti.Enabled = true;
            cbTipoDct.Enabled = false;
            cbTipoDct.SelectedIndex = -1;
            btnImprimir.Enabled = false;
            chbOtro.Enabled = true;
            btnCancelar.Hide();
            pnlListaCerti.Hide();
        }

        private void btnConsultoria_Click(object sender, EventArgs e)
        {
            Separator.Location = new Point(15, 364);
            Separator.Show();
            //Aqui debe de venir tambien la opcion de agregar nuevo excel de alumnos
        }

        private void btnEvento_Click(object sender, EventArgs e)
        {
            Separator.Location = new Point(15, 454);
            Separator.Show();
        }

        
        private void btnDocenteBuscar_Click(object sender, EventArgs e)
        {
            BuscaPaciente();
        }

        private void btnAlumnoBuscar_Click(object sender, EventArgs e)
        {
            BuscaPaciente();
        }
        
        private void tbAlumnoNoControl_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAlumnoNoControl.Text, "^[a-zA-Z0-9]+$") || tbAlumnoNoControl.Text.Length < 1)
            {
            }
            else
            {
                tbAlumnoNoControl.Text = tbAlumnoNoControl.Text.Remove(tbAlumnoNoControl.Text.Length - 1);
            }
        }

        private void tbDocenteNoDocente_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbDocenteNoDocente.Text, "^[a-zA-Z0-9]+$") || tbDocenteNoDocente.Text.Length < 1)
            {
            }
            else
            {
                tbDocenteNoDocente.Text = tbDocenteNoDocente.Text.Remove(tbDocenteNoDocente.Text.Length - 1);
            }
        }

        private void btnOtro_OtroDiagnostico_Click(object sender, EventArgs e)
        {
            AbreDiagnostico();
        }

        private void btnDocente_OtroDiagnostico_Click(object sender, EventArgs e)
        {
            AbreDiagnostico();
        }

        private void btnAlumno_OtroDiagnostico_Click(object sender, EventArgs e)
        {
            AbreDiagnostico();
        }

      

        private void btnOtro_OtroMedicamento_Click(object sender, EventArgs e)
        {
            AbreMedicamento();
        }

        private void btnDocente_OtroMedicamento_Click(object sender, EventArgs e)
        {
            AbreMedicamento();
        }

        private void btnAlumno_OtroMedicamento_Click(object sender, EventArgs e)
        {
            AbreMedicamento();
        }

        private void btnAlumnoRealizarConsulta_Click(object sender, EventArgs e)
        {
            if (ListoParaAgregar())
            {
                InsertarConsulta();
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
            }       
        }

        //******||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        #region Funciones Limpiadoras
        private void LimpiaAlumno()
        {
            tbAlumnoNoControl.Text = "";
            tbAlumnoNombre.Text = "";
            tbAlumnoCarrera.Text = "";
            tbAlumnoSemestre.Text = "";
            tbAlumnoEdad.Text = "";
            tbAlumnoSexo.Text = "";
            tbAlumnoMotivo.Text = "";
        }
        private void LimpiaDocente()
        {
            tbDocenteNoDocente.Text = "";
            tbDocenteNombre.Text = "";
            tbDocenteArea.Text = "";
            tbDocenteEdad.Text = "";
            tbDocenteSexo.Text = "";
            tbDocenteMotivo.Text = "";
        }
        private void LimpiaOtro()
        {
            tbOtroNombre.Text = "";
            tbOtroRelacion.Text = "";
            tbOtroEdad.Text = "";
            tbOtroMotivo.Text = "";
        }
        #endregion

        private void AbreDiagnostico()
        {
            if (!diagnosticoFormObjeto.Visible)
            {
                diagnosticoFormObjeto.Show();
            }
        }

        private void AbreMedicamento()
        {
            if (!medicamentoFormObjeto.Visible)
            {
                medicamentoFormObjeto.Show();
            }
        }

        private void BuscaPaciente()
        {
            if (cbAlumno.Checked)
            {
                // Lectura base de datos
                if (tbAlumnoNoControl.Text == "creador")
                {
                    tbAlumnoNombre.Text = "Software creado por:";
                    tbAlumnoSemestre.Text = "Juan Carlos Martinez Tapia";
                    tbAlumnoCarrera.Text = "Jorge Antonio Cordoba Tapia";
                    tbAlumnoMotivo.Text = "Creado en el 2017.";
                    tbAlumnoEdad.Text = "Daniel Gamez Navarro";
                    tbAlumnoSexo.Text = "Enrique Macias Murrieta";
                    bunifuGradientPanel1.GradientBottomLeft = Color.DarkMagenta;
                    bunifuGradientPanel1.GradientBottomRight = Color.Black;
                    bunifuGradientPanel1.GradientTopLeft = Color.DarkSlateBlue;
                    bunifuGradientPanel1.GradientTopRight = Color.DarkViolet;
                    pnlAlumno.BackColor = Color.Black;
                    pnlConsulta.BackColor = Color.Black;
                    pnlDocente.BackColor = Color.Black;
                    pnlOtro.BackColor = Color.Black;
                    btnConsulta.ForeColor = Color.Black;                    
                    btnConsultoria.ForeColor = Color.Black;                    
                    btnCerrar.ForeColor = Color.Black;                    
                    btnCertificadoMed.ForeColor = Color.Black;
                    btnEvento.ForeColor = Color.Black;                    
                    lblFecha.ForeColor = Color.Black;
                    Separator.LineColor = Color.Black;
                    bunifuSeparator2.LineColor = Color.Black;
                    tbAlumnoNoControl.BackColor = Color.DarkMagenta;
                    tbAlumnoNombre.BackColor = Color.DarkMagenta;
                    tbAlumnoCarrera.BackColor = Color.DarkMagenta;
                    tbAlumnoSemestre.BackColor = Color.DarkMagenta;
                    tbAlumnoEdad.BackColor = Color.DarkMagenta;
                    tbAlumnoSexo.BackColor = Color.DarkMagenta;
                    tbAlumnoMotivo.BackColor = Color.DarkMagenta;
                    tbDocenteNoDocente.BackColor = Color.DarkMagenta;
                    tbDocenteNombre.BackColor = Color.DarkMagenta;
                    tbDocenteArea.BackColor = Color.DarkMagenta;
                    tbDocenteEdad.BackColor = Color.DarkMagenta;
                    tbDocenteSexo.BackColor = Color.DarkMagenta;
                    tbDocenteMotivo.BackColor = Color.DarkMagenta;
                    tbOtroNombre.BackColor = Color.DarkMagenta;
                    tbOtroRelacion.BackColor = Color.DarkMagenta;
                    tbOtroEdad.BackColor = Color.DarkMagenta;
                    tbOtroMotivo.BackColor = Color.DarkMagenta;
                    btnAlumnoBuscar.ActiveFillColor = Color.Black;
                    btnAlumnoBuscar.ActiveForecolor = Color.DarkMagenta;
                    btnAlumnoBuscar.ActiveLineColor = Color.DarkMagenta;
                    btnAlumnoBuscar.IdleFillColor = Color.Black;
                    btnAlumnoBuscar.IdleForecolor = Color.DarkMagenta;
                    btnAlumnoBuscar.IdleLineColor = Color.DarkMagenta;
                    btnAlumnoRealizarConsulta.ActiveFillColor = Color.Black;
                    btnAlumnoRealizarConsulta.ActiveForecolor = Color.DarkMagenta;
                    btnAlumnoRealizarConsulta.ActiveLineColor = Color.DarkMagenta;
                    btnAlumnoRealizarConsulta.IdleFillColor = Color.Black;
                    btnAlumnoRealizarConsulta.IdleForecolor = Color.DarkMagenta;
                    btnAlumnoRealizarConsulta.IdleLineColor = Color.DarkMagenta;
                    btnAlumno_OtroDiagnostico.ActiveFillColor = Color.Black;
                    btnAlumno_OtroDiagnostico.ActiveForecolor = Color.DarkMagenta;
                    btnAlumno_OtroDiagnostico.ActiveLineColor = Color.DarkMagenta;
                    btnAlumno_OtroDiagnostico.IdleFillColor = Color.Black;
                    btnAlumno_OtroDiagnostico.IdleForecolor = Color.DarkMagenta;
                    btnAlumno_OtroDiagnostico.IdleLineColor = Color.DarkMagenta;
                    btnAlumno_OtroMedicamento.ActiveFillColor = Color.Black;
                    btnAlumno_OtroMedicamento.ActiveForecolor = Color.DarkMagenta;
                    btnAlumno_OtroMedicamento.ActiveLineColor = Color.DarkMagenta;
                    btnAlumno_OtroMedicamento.IdleFillColor = Color.Black;
                    btnAlumno_OtroMedicamento.IdleForecolor = Color.DarkMagenta;
                    btnAlumno_OtroMedicamento.IdleLineColor = Color.DarkMagenta;
                    //Docente
                    btnDocenteBuscar.ActiveFillColor = Color.Black;
                    btnDocenteBuscar.ActiveForecolor = Color.DarkMagenta;
                    btnDocenteBuscar.ActiveLineColor = Color.DarkMagenta;
                    btnDocenteBuscar.IdleFillColor = Color.Black;
                    btnDocenteBuscar.IdleForecolor = Color.DarkMagenta;
                    btnDocenteBuscar.IdleLineColor = Color.DarkMagenta;
                    btnDocenteRealizarConsulta.ActiveFillColor = Color.Black;
                    btnDocenteRealizarConsulta.ActiveForecolor = Color.DarkMagenta;
                    btnDocenteRealizarConsulta.ActiveLineColor = Color.DarkMagenta;
                    btnDocenteRealizarConsulta.IdleFillColor = Color.Black;
                    btnDocenteRealizarConsulta.IdleForecolor = Color.DarkMagenta;
                    btnDocenteRealizarConsulta.IdleLineColor = Color.DarkMagenta;
                    btnDocente_OtroDiagnostico.ActiveFillColor = Color.Black;
                    btnDocente_OtroDiagnostico.ActiveForecolor = Color.DarkMagenta;
                    btnDocente_OtroDiagnostico.ActiveLineColor = Color.DarkMagenta;
                    btnDocente_OtroDiagnostico.IdleFillColor = Color.Black;
                    btnDocente_OtroDiagnostico.IdleForecolor = Color.DarkMagenta;
                    btnDocente_OtroDiagnostico.IdleLineColor = Color.DarkMagenta;
                    btnDocente_OtroMedicamento.ActiveFillColor = Color.Black;
                    btnDocente_OtroMedicamento.ActiveForecolor = Color.DarkMagenta;
                    btnDocente_OtroMedicamento.ActiveLineColor = Color.DarkMagenta;
                    btnDocente_OtroMedicamento.IdleFillColor = Color.Black;
                    btnDocente_OtroMedicamento.IdleForecolor = Color.DarkMagenta;
                    btnDocente_OtroMedicamento.IdleLineColor = Color.DarkMagenta;
                    //Otro
                    btnOtro_OtroDiagnostico.ActiveFillColor = Color.Black;
                    btnOtro_OtroDiagnostico.ActiveForecolor = Color.DarkMagenta;
                    btnOtro_OtroDiagnostico.ActiveLineColor = Color.DarkMagenta;
                    btnOtro_OtroDiagnostico.IdleFillColor = Color.Black;
                    btnOtro_OtroDiagnostico.IdleForecolor = Color.DarkMagenta;
                    btnOtro_OtroDiagnostico.IdleLineColor = Color.DarkMagenta;
                    btnOtro_OtroMedicamento.ActiveFillColor = Color.Black;
                    btnOtro_OtroMedicamento.ActiveForecolor = Color.DarkMagenta;
                    btnOtro_OtroMedicamento.ActiveLineColor = Color.DarkMagenta;
                    btnOtro_OtroMedicamento.IdleFillColor = Color.Black;
                    btnOtro_OtroMedicamento.IdleForecolor = Color.DarkMagenta;
                    btnOtro_OtroMedicamento.IdleLineColor = Color.DarkMagenta;
                    cbOtroDiagnostico.BackColor = Color.Indigo;
                    cbOtroMedicamento.BackColor = Color.Indigo;
                    ddbOtroSexo.BackColor = Color.Indigo;
                    ddbAlumnoDiagnostico.BackColor = Color.Indigo;
                    ddbAlumnoMedicamento.BackColor = Color.Indigo;
                    ddbDocenteDiagnostico.BackColor = Color.Indigo;
                    ddbDocenteMedicamento.BackColor = Color.Indigo;
                   
                }
                else
                {
                    string cadQuery = "Select * from alumno where num_control ='" + tbAlumnoNoControl.Text + "' ;";
                    input = tbAlumnoNoControl.Text;
                    SqlCommand comando = new SqlCommand(cadQuery, conn);
                    conn.Open();

                    DateTime today = DateTime.Today;
                    string fechNac;
                    DateTime fecNac;
                    string sexo;
                    string curp;

                    SqlDataReader leer = comando.ExecuteReader();

                    if (leer.Read() == true)
                    {
                        tbAlumnoNoControl.Text = input;
                        tbAlumnoNombre.Text = leer["nombre"].ToString() + " " + leer["nombre_paterno"].ToString() + " " + leer["nombre_materno"].ToString();
                        tbAlumnoCarrera.Text = leer["carrera"].ToString();
                        tbAlumnoSemestre.Text = DeterminaSemestre(Convert.ToInt32(leer["num_control"]));


                        fechNac = leer["fecha_nacimiento"].ToString();
                        fecNac = Convert.ToDateTime(fechNac);
                        var age = today.Year - fecNac.Year;
                        if (fecNac > today.AddYears(-age)) age--;
                        tbAlumnoEdad.Text = Convert.ToString(age);

                        curp = leer["CURP"].ToString();
                        sexo = curp.Substring(10, 1);
                        if (sexo == "M")
                        {
                            sexo = "Mujer";
                        }
                        else if (sexo == "H")
                        {
                            sexo = "Hombre";
                        }
                        else
                        {
                            sexo = "error de seleccion.";
                        }
                        tbAlumnoSexo.Text = sexo;
                    }
                    else
                    {
                        tbAlumnoNombre.Text = "";
                        tbAlumnoCarrera.Text = "";
                        tbAlumnoSemestre.Text = "";
                        tbAlumnoEdad.Text = "";
                        tbAlumnoSexo.Text = "";
                        MessageBox.Show("Alumno no encontrado. Es probable que el alumno no este inscrito este semestre.", "Alumno no encontrado!", MessageBoxButtons.OK);
                    }
                    conn.Close();
                    tbAlumnoNoControl.Text = input;
                }
                
            }
            else if (cbDocente.Checked)
            {

                input = tbDocenteNoDocente.Text;
                string QuerryBuscaDocente = "Select * from docente where num_docente ='" + tbDocenteNoDocente.Text + "' ";

                SqlCommand comando = new SqlCommand(QuerryBuscaDocente, conn);
                conn.Open();

                DateTime today = DateTime.Today;
                string fechNacD;
                DateTime fecNacD;
                string sexoD;
                string curpD;

                SqlDataReader leerdoc = comando.ExecuteReader();

                if (leerdoc.Read() == true)
                {

                    tbDocenteNombre.Text = leerdoc["nombre"].ToString();
                    tbDocenteArea.Text = leerdoc["departamento"].ToString();
                    // Convierte fecha de nacimiento a edad:
                    fechNacD = leerdoc["fecha_nac"].ToString();
                    fecNacD = Convert.ToDateTime(fechNacD);
                    var ageD = today.Year - fecNacD.Year;
                    if (fecNacD > today.AddYears(-ageD)) ageD--;
                    tbDocenteEdad.Text = Convert.ToString(ageD);
                    // --------------------------------------------
                    curpD = leerdoc["CURP"].ToString();
                    sexoD = curpD.Substring(10, 1);
                    if (sexoD == "M")
                    {
                        sexoD = "Mujer";
                    }
                    else if (sexoD == "H")
                    {
                        sexoD = "Hombre";
                    }
                    else
                    {
                        sexoD = "error de seleccion.";
                    }
                    tbDocenteSexo.Text = sexoD;
                }
                else
                {
                    tbDocenteNombre.Text = "";
                    tbDocenteArea.Text = "";
                    tbAlumnoEdad.Text = "";
                    tbAlumnoSexo.Text = "";
                }
                conn.Close();
                tbDocenteNoDocente.Text = input;

            }

        }
        
        private string Seguimiento()
        {
            if (cbSeguimiento.Checked)
            {
                return "Si";
            }
            else
            {
                return "No";
            }
        }

        private void InsertarConsulta()
        {
            if (cbAlumno.Checked)
            {
                try
                {
                    conn.Open();
                    if (!ddbAlumnoMedicamento2.Visible && !ddbAlumnoMedicamento3.Visible)
                    {
                        SqlCommand comandoAlumno = new SqlCommand("insert into consultas (num_control, seguimiento, fecha, medicamento, diagnostico, edad, sexo, motivo, doctor) values('" + tbAlumnoNoControl.Text + "', '" + Seguimiento() + "', '" + DateTime.Now + "', '" + ddbAlumnoMedicamento.GetItemText(ddbAlumnoMedicamento.SelectedItem) + "', '" + ddbAlumnoDiagnostico.GetItemText(ddbAlumnoDiagnostico.SelectedItem) + "', " + tbAlumnoEdad.Text + ", '" + tbAlumnoSexo.Text + "', '" + tbAlumnoMotivo.Text + "', '" + Usuario + "');", conn);
                        comandoAlumno.ExecuteNonQuery();
                        MessageBox.Show("La consulta fue agregada a la base de datos exitosamente.", "Agregado", MessageBoxButtons.OK);
                    }
                    else if (ddbAlumnoMedicamento2.Visible && !ddbAlumnoMedicamento3.Visible)
                    {
                        SqlCommand comandoAlumno = new SqlCommand("insert into consultas (num_control, seguimiento, fecha, medicamento, medicamento2, diagnostico, edad, sexo, motivo, doctor) values('" + tbAlumnoNoControl.Text + "', '" + Seguimiento() + "', '" + DateTime.Now + "', '" + ddbAlumnoMedicamento.GetItemText(ddbAlumnoMedicamento.SelectedItem) + "', '" + ddbAlumnoMedicamento2.GetItemText(ddbAlumnoMedicamento2.SelectedItem) + "', '" + ddbAlumnoDiagnostico.GetItemText(ddbAlumnoDiagnostico.SelectedItem) + "', " + tbAlumnoEdad.Text + ", '" + tbAlumnoSexo.Text + "', '" + tbAlumnoMotivo.Text + "', '" + Usuario + "');", conn);
                        comandoAlumno.ExecuteNonQuery();
                        MessageBox.Show("La consulta fue agregada a la base de datos exitosamente.", "Agregado", MessageBoxButtons.OK);
                    }
                    else if (ddbAlumnoMedicamento2.Visible && ddbAlumnoMedicamento3.Visible)
                    {
                        SqlCommand comandoAlumno = new SqlCommand("insert into consultas (num_control, seguimiento, fecha, medicamento, medicamento2, medicamento3, diagnostico, edad, sexo, motivo, doctor) values('" + tbAlumnoNoControl.Text + "', '" + Seguimiento() + "', '" + DateTime.Now + "', '" + ddbAlumnoMedicamento.GetItemText(ddbAlumnoMedicamento.SelectedItem) + "', '" + ddbAlumnoMedicamento2.GetItemText(ddbAlumnoMedicamento2.SelectedItem) + "', '" + ddbAlumnoMedicamento3.GetItemText(ddbAlumnoMedicamento3.SelectedItem) + "', '" + ddbAlumnoDiagnostico.GetItemText(ddbAlumnoDiagnostico.SelectedItem) + "', " + tbAlumnoEdad.Text + ", '" + tbAlumnoSexo.Text + "', '" + tbAlumnoMotivo.Text + "', '" + Usuario + "');", conn);
                        comandoAlumno.ExecuteNonQuery();
                        MessageBox.Show("La consulta fue agregada a la base de datos exitosamente.", "Agregado", MessageBoxButtons.OK);
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrio el siguiente problema: " + ex.Message + ". Contecte al administrador." + "\t" + ex.GetType());
                }
            }
            else if (cbDocente.Checked)
            {
                try
                {
                    conn.Open();
                    if (!ddbDocenteMedicamento2.Visible && !ddbDocenteMedicamento3.Visible)
                    {
                        SqlCommand cmd = new SqlCommand("insert into consultas (num_docente, seguimiento, fecha, medicamento, diagnostico, edad, sexo, motivo, doctor) values(" + tbDocenteNoDocente.Text + ", '" + Seguimiento() + "', '" + DateTime.Now + "', '" + ddbDocenteMedicamento.GetItemText(ddbDocenteMedicamento.SelectedItem) + "', '" + ddbDocenteDiagnostico.GetItemText(ddbDocenteDiagnostico.SelectedItem) + "', " + tbDocenteEdad.Text + ", '" + tbDocenteSexo.Text + "', '" + tbDocenteMotivo.Text + "', '" + Usuario + "');", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta Agregada.");
                    }
                    else if (ddbDocenteMedicamento2.Visible && !ddbDocenteMedicamento3.Visible)
                    {
                        SqlCommand cmd = new SqlCommand("insert into consultas (num_docente, seguimiento, fecha, medicamento, medicamento2, diagnostico, edad, sexo, motivo, doctor) values(" + tbDocenteNoDocente.Text + ", '" + Seguimiento() + "', '" + DateTime.Now + "', '" + ddbDocenteMedicamento.GetItemText(ddbDocenteMedicamento.SelectedItem) + "', '" + ddbDocenteMedicamento2.GetItemText(ddbDocenteMedicamento2.SelectedItem) + "', '" + ddbDocenteDiagnostico.GetItemText(ddbDocenteDiagnostico.SelectedItem) + "', " + tbDocenteEdad.Text + ", '" + tbDocenteSexo.Text + "', '" + tbDocenteMotivo.Text + "', '" + Usuario + "');", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta Agregada.");
                    }
                    else if (ddbDocenteMedicamento2.Visible && ddbDocenteMedicamento3.Visible)
                    {
                        SqlCommand cmd = new SqlCommand("insert into consultas (num_docente, seguimiento, fecha, medicamento, medicamento2, medicamento3, diagnostico, edad, sexo, motivo, doctor) values(" + tbDocenteNoDocente.Text + ", '" + Seguimiento() + "', '" + DateTime.Now + "', '" + ddbDocenteMedicamento.GetItemText(ddbDocenteMedicamento.SelectedItem) + "', '" + ddbDocenteMedicamento2.GetItemText(ddbDocenteMedicamento2.SelectedItem) + "', '" + ddbDocenteMedicamento3.GetItemText(ddbDocenteMedicamento3.SelectedItem) + "', '" + ddbDocenteDiagnostico.GetItemText(ddbDocenteDiagnostico.SelectedItem) + "', " + tbDocenteEdad.Text + ", '" + tbDocenteSexo.Text + "', '" + tbDocenteMotivo.Text + "', '" + Usuario + "');", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta Agregada.");
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrio el siguiente problema:" + ex.Message + "Contecte al administrador." + "\t" + ex.GetType());
                }
            }
            else if (cbOtro.Checked)
            {
                try
                {
                    string idOtro = "";
                    conn.Open();
                    //Se tiene que insertar primero en la tabla Otro, despues leer de Otro el valor del ultimo index y despues insertar en consultas la informacion

                    SqlCommand cmdd = new SqlCommand("insert into otro (nombre, relacion, edad, sexo) values('" + tbOtroNombre.Text + "', '" + tbOtroRelacion.Text + "', " + tbOtroEdad.Text + ", '" + ddbOtroSexo.SelectedItem + "');", conn);
                    cmdd.ExecuteNonQuery();
                    conn.Close();
                    //Lectura del ultimo valor:

                    string jorgeJuanKikeGamez = "Select max(num_otro) as num_otro from otro;";
                    SqlCommand comando = new SqlCommand(jorgeJuanKikeGamez, conn);
                    conn.Open();
                    
                    SqlDataReader leer2 = comando.ExecuteReader();
                    if (leer2.Read())
                    {
                        idOtro = leer2["num_otro"].ToString();
                    }
                    conn.Close();                   
                    //Insertar en Consultas

                    conn.Open();

                    if (!cbOtroMedicamento2.Visible && !cbOtroMedicamento3.Visible)
                    {
                        SqlCommand cmd = new SqlCommand("insert into consultas (num_otro, seguimiento, fecha, medicamento, diagnostico, edad, sexo, motivo, doctor) values(" + idOtro + ", '" + Seguimiento() + "', '" + DateTime.Now + "', '" + cbOtroMedicamento.GetItemText(cbOtroMedicamento.SelectedItem) + "', '" + cbOtroDiagnostico.GetItemText(cbOtroDiagnostico.SelectedItem) + "', " + tbOtroEdad.Text + ", '" + ddbOtroSexo.SelectedItem + "', '" + tbOtroMotivo.Text + "', '" + Usuario + "');", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta Agregada a consultas.");
                    }
                    else if (cbOtroMedicamento2.Visible && !cbOtroMedicamento3.Visible)
                    {
                        SqlCommand cmd = new SqlCommand("insert into consultas (num_otro, seguimiento, fecha, medicamento, medicamento2, diagnostico, edad, sexo, motivo, doctor) values(" + idOtro + ", '" + Seguimiento() + "', '" + DateTime.Now + "', '" + cbOtroMedicamento.GetItemText(cbOtroMedicamento.SelectedItem) + "', '" + cbOtroMedicamento2.GetItemText(cbOtroMedicamento2.SelectedItem) + "', '" + cbOtroDiagnostico.GetItemText(cbOtroDiagnostico.SelectedItem) + "', " + tbOtroEdad.Text + ", '" + ddbOtroSexo.SelectedItem + "', '" + tbOtroMotivo.Text + "', '" + Usuario + "');", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta Agregada a consultas.");
                    }
                    else if (cbOtroMedicamento2.Visible && cbOtroMedicamento3.Visible)
                    {
                        SqlCommand cmd = new SqlCommand("insert into consultas (num_otro, seguimiento, fecha, medicamento, medicamento2, medicamento3, diagnostico, edad, sexo, motivo, doctor) values(" + idOtro + ", '" + Seguimiento() + "', '" + DateTime.Now + "', '" + cbOtroMedicamento.GetItemText(cbOtroMedicamento.SelectedItem) + "', '" + cbOtroMedicamento2.GetItemText(cbOtroMedicamento2.SelectedItem) + "', '" + cbOtroMedicamento3.GetItemText(cbOtroMedicamento3.SelectedItem) + "', '" + cbOtroDiagnostico.GetItemText(cbOtroDiagnostico.SelectedItem) + "', " + tbOtroEdad.Text + ", '" + ddbOtroSexo.SelectedItem + "', '" + tbOtroMotivo.Text + "', '" + Usuario + "');", conn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Consulta Agregada a consultas.");
                    }
                    conn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrio el siguiente problema: " + ex.Message + "Contecte al administrador." + "\t" + ex.GetType());
                }
            }
        }

        private bool ListoParaAgregar()
        {
            if (cbAlumno.Checked)
            {
                if (tbAlumnoNoControl.Text != "" && tbAlumnoNombre.Text != "" && tbAlumnoCarrera.Text != "" && tbAlumnoSemestre.Text != "" && tbAlumnoEdad.Text != "" && tbAlumnoSexo.Text != "" && tbAlumnoMotivo.Text != "" && ddbAlumnoDiagnostico.SelectedItem.ToString() != "" && ddbAlumnoMedicamento.SelectedItem.ToString() != "")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Llene todos los campos.", "Campo/s faltantes", MessageBoxButtons.OK);
                    return false;
                }
            }
            else if (cbDocente.Checked)
            {
                if (tbDocenteNoDocente.Text != "" && tbDocenteNombre.Text != "" && tbDocenteArea.Text != "" && tbDocenteEdad.Text != "" && tbDocenteSexo.Text != "" && tbDocenteMotivo.Text != "" && ddbDocenteDiagnostico.SelectedItem.ToString() != "" && ddbDocenteMedicamento.SelectedItem.ToString() != "")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Llene todos los campos.", "Campo/s faltantes", MessageBoxButtons.OK);
                    return false;
                }
            }
            else if (cbOtro.Checked)
            {
                if (tbOtroNombre.Text != "" && tbOtroRelacion.Text != "" && tbOtroEdad.Text != "" && ddbOtroSexo.SelectedItem.ToString() != "" && tbOtroMotivo.Text != "" && cbOtroDiagnostico.SelectedItem.ToString() != "" && cbOtroMedicamento.SelectedItem.ToString() != "")
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Llene todos los campos.", "Campo/s faltantes", MessageBoxButtons.OK);
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public void LlenaCbDiagnostico()
        {
            // Llenar ComboBox de Diagnostico:
            dsDiagnostico.Clear();
            //cbOtroDiagnostico.Items.Clear();
            //ddbAlumnoDiagnostico.Items.Clear();
            //ddbDocenteDiagnostico.Items.Clear();
            conn.Open();
            string strCmdDiagnostico = "select nombre from diagnostico";
            SqlCommand cmdCbDiagnostico = new SqlCommand(strCmdDiagnostico, conn);
            SqlDataAdapter daDiagnostico = new SqlDataAdapter(strCmdDiagnostico, conn);
            
            daDiagnostico.Fill(dsDiagnostico);
            cmdCbDiagnostico.ExecuteNonQuery();
            conn.Close();

            ddbAlumnoDiagnostico.DisplayMember = "nombre";
            ddbAlumnoDiagnostico.ValueMember = "nombre";
            ddbAlumnoDiagnostico.DataSource = dsDiagnostico;
            ddbAlumnoDiagnostico.Enabled = true;
            ddbDocenteDiagnostico.DisplayMember = "nombre";
            ddbDocenteDiagnostico.ValueMember = "nombre";
            ddbDocenteDiagnostico.DataSource = dsDiagnostico;
            ddbDocenteDiagnostico.Enabled = true;
            cbOtroDiagnostico.DisplayMember = "nombre";
            cbOtroDiagnostico.ValueMember = "nombre";
            cbOtroDiagnostico.DataSource = dsDiagnostico;
            cbOtroDiagnostico.Enabled = true;


            


        }

        public void LlenaCbMedicamento()
        {
            //Llenar ComboBox de Medicamento:
            dsMedicamento.Clear();
            //cbOtroMedicamento.Items.Clear();
            //ddbAlumnoMedicamento.Items.Clear();
            //ddbDocenteMedicamento.Items.Clear();
            conn.Open();
            string strCmdMedicamento = "select nombre from medicamento";
            SqlCommand cmdCbMedicamento = new SqlCommand(strCmdMedicamento, conn);
            SqlDataAdapter daMedicamento = new SqlDataAdapter(strCmdMedicamento, conn);
            
            daMedicamento.Fill(dsMedicamento);
            cmdCbMedicamento.ExecuteNonQuery();
            conn.Close();

            ddbAlumnoMedicamento.DisplayMember = "nombre";
            ddbAlumnoMedicamento.ValueMember = "nombre";
            ddbAlumnoMedicamento.DataSource = dsMedicamento;
            ddbAlumnoMedicamento.Enabled = true;
            ddbDocenteMedicamento.DisplayMember = "nombre";
            ddbDocenteMedicamento.ValueMember = "nombre";
            ddbDocenteMedicamento.DataSource = dsMedicamento;
            ddbDocenteMedicamento.Enabled = true;
            cbOtroMedicamento.DisplayMember = "nombre";
            cbOtroMedicamento.ValueMember = "nombre";
            cbOtroMedicamento.DataSource = dsMedicamento;
            cbOtroMedicamento.Enabled = true;
            
        }

        public void LlenaCbMedicamento2()
        {
            //Llenar ComboBox de Medicamento:
            dsMedicamento2.Clear();
            //cbOtroMedicamento.Items.Clear();
            //ddbAlumnoMedicamento.Items.Clear();
            //ddbDocenteMedicamento.Items.Clear();
            conn.Open();
            string strCmdMedicamento2 = "select nombre from medicamento";
            SqlCommand cmdCbMedicamento2 = new SqlCommand(strCmdMedicamento2, conn);
            SqlDataAdapter daMedicamento2 = new SqlDataAdapter(strCmdMedicamento2, conn);

            daMedicamento2.Fill(dsMedicamento2);
            cmdCbMedicamento2.ExecuteNonQuery();
            conn.Close();

            ddbAlumnoMedicamento2.DisplayMember = "nombre";
            ddbAlumnoMedicamento2.ValueMember = "nombre";
            ddbAlumnoMedicamento2.DataSource = dsMedicamento2;
            ddbAlumnoMedicamento2.Enabled = true;
            ddbDocenteMedicamento2.DisplayMember = "nombre";
            ddbDocenteMedicamento2.ValueMember = "nombre";
            ddbDocenteMedicamento2.DataSource = dsMedicamento2;
            ddbDocenteMedicamento2.Enabled = true;
            cbOtroMedicamento2.DisplayMember = "nombre";
            cbOtroMedicamento2.ValueMember = "nombre";
            cbOtroMedicamento2.DataSource = dsMedicamento2;
            cbOtroMedicamento2.Enabled = true;

        }

        public void LlenaCbMedicamento3()
        {
            //Llenar ComboBox de Medicamento:
            dsMedicamento3.Clear();
            //cbOtroMedicamento.Items.Clear();
            //ddbAlumnoMedicamento.Items.Clear();
            //ddbDocenteMedicamento.Items.Clear();
            conn.Open();
            string strCmdMedicamento3 = "select nombre from medicamento";
            SqlCommand cmdCbMedicamento3 = new SqlCommand(strCmdMedicamento3, conn);
            SqlDataAdapter daMedicamento3 = new SqlDataAdapter(strCmdMedicamento3, conn);

            daMedicamento3.Fill(dsMedicamento3);
            cmdCbMedicamento3.ExecuteNonQuery();
            conn.Close();

            ddbAlumnoMedicamento3.DisplayMember = "nombre";
            ddbAlumnoMedicamento3.ValueMember = "nombre";
            ddbAlumnoMedicamento3.DataSource = dsMedicamento3;
            ddbAlumnoMedicamento3.Enabled = true;
            ddbDocenteMedicamento3.DisplayMember = "nombre";
            ddbDocenteMedicamento3.ValueMember = "nombre";
            ddbDocenteMedicamento3.DataSource = dsMedicamento3;
            ddbDocenteMedicamento3.Enabled = true;
            cbOtroMedicamento3.DisplayMember = "nombre";
            cbOtroMedicamento3.ValueMember = "nombre";
            cbOtroMedicamento3.DataSource = dsMedicamento3;
            cbOtroMedicamento3.Enabled = true;

        }

        private string DeterminaSemestre(int numControl)
        {
            string Añostring;
            int Añoescolar;
            //                                                                                                                                                                                                                                            Perdon a la persona que tenga que arreglar esto, pero lo mas seguro es que ya estemos muertos :)
            Añostring = "20" + numControl.ToString().Substring(0, 2);
            Añoescolar = Convert.ToInt32(DateTime.Now.Year.ToString()) - Convert.ToInt32(Añostring);
            Añoescolar = Añoescolar * 2;
            if (DateTime.Now.Month >= 8)
            {
                //Semestre impar
                Añoescolar++;
            }
            return (Añoescolar.ToString());

        }

        //Metodo de Imprimir
        private void Imprimir(Microsoft.Office.Interop.Word.Document doc, Microsoft.Office.Interop.Word.Application app, string path)
        {
            PrintDialog pDialog = new PrintDialog();
            if (pDialog.ShowDialog() == DialogResult.OK)
            {
                doc = app.Documents.Add(path);
                app.ActivePrinter = pDialog.PrinterSettings.PrinterName;
                app.ActiveDocument.PrintOut();

                doc.Close();
                doc = null;
                app.Quit();
                MessageBox.Show("Documento Creado E Impreso Con Exito", "Documento de Certidicado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                tbCodigoCerti.Text = "";
                chbOtro.Checked = false;
                cbTipoDct.Enabled = false;
                cbTipoDct.SelectedIndex = -1;
                tbCodigoCerti.Enabled = true;
                chbOtro.Enabled = true;
                btnImprimir.Enabled = false;
            }
            else
            {
                doc.Close();
                app.Quit();
                tbCodigoCerti.Text = "";
                chbOtro.Checked = false;
                cbTipoDct.Enabled = false;
                cbTipoDct.SelectedIndex = -1;
                tbCodigoCerti.Enabled = true;
                chbOtro.Enabled = true;
                btnImprimir.Enabled = false;
            }
        }
        //Metodo Llenar Documento de Alguien del Plantel
        private void LlenarDocPlantel(Microsoft.Office.Interop.Word.Document doc, Microsoft.Office.Interop.Word.Application app, string tipodoc)
        {

            string cadQuery;
            string path;
            if (banderaalumno == true)
            {
                //si es alumno
                path = @"C:\RSM\DocumentosMedicos\" + tipodoc + @"s\Alumnos\" + tipodoc;
                doc = app.Documents.Add(Template: path + ".docx");
                cadQuery = "Select * from alumno  where num_control ='" + num_id + "' ";
            }
            else
            {
                //si es docente
                path = @"C:\RSM\DocumentosMedicos\" + tipodoc + @"s\Docentes\" + tipodoc;
                doc = app.Documents.Add(Template: path + ".docx");
                cadQuery = "Select * from docente where num_docente ='" + num_id + "' ";
            }


            SqlCommand comando = new SqlCommand(cadQuery, conn);
            conn.Open();

            SqlDataReader leer3 = comando.ExecuteReader();
            if (leer3.Read() == true)
            {
                foreach (Microsoft.Office.Interop.Word.Field field in doc.Fields)
                {
                    if (field.Code.Text.Contains("Nombre"))
                    {

                        field.Select();
                        string nombrecerti;

                        if (banderaalumno == true)
                        {
                            //nombre de alumno
                            nombrecerti = leer3["nombre"].ToString() + " " + leer3["nombre_paterno"].ToString() + " " + leer3["nombre_materno"].ToString();
                            nombre = nombrecerti;
                        }
                        else
                        {
                            //nombre de docente
                            nombrecerti = leer3["nombre"].ToString();
                            nombre = nombrecerti;
                        }

                        app.Selection.TypeText(nombre);
                    }
                    else if (field.Code.Text.Contains("Edad"))
                    {
                        field.Select();
                        app.Selection.TypeText(edad);

                    }
                    else if (field.Code.Text.Contains("Fecha"))
                    {
                        field.Select();
                        app.Selection.TypeText(fecha);
                    }
                    else if (field.Code.Text.Contains("Doctor"))
                    {
                        field.Select();
                        app.Selection.TypeText(doctor);
                    }



                }
            }
            conn.Close();
            doc.SaveAs(path + "-" + nombre + ".docx");
            string finalpath = path + "-" + nombre + ".docx";

            Imprimir(doc, app, finalpath);
        }
        //Metodo Llenar Documento de ALguien Fuera del Plantel
        private void LlenarDocFueraPlantel(Microsoft.Office.Interop.Word.Document doc, Microsoft.Office.Interop.Word.Application app, string tipodoc)
        {
            string path = @"C:\RSM\DocumentosMedicos\" + tipodoc + @"s\Otros\" + tipodoc;
            doc = app.Documents.Add(Template: path + ".docx");
            String cadQuery = "Select * from consulta nombre ='" + nombre + "' ";
            SqlCommand comando = new SqlCommand(cadQuery, conn);
            conn.Open();

            foreach (Microsoft.Office.Interop.Word.Field field in doc.Fields)
            {
                if (field.Code.Text.Contains("Nombre"))
                {
                    field.Select();
                    app.Selection.TypeText(nombre);

                }
                else if (field.Code.Text.Contains("Edad"))
                {
                    field.Select();
                    app.Selection.TypeText(edad);

                }
                else if (field.Code.Text.Contains("Fecha"))
                {
                    field.Select();
                    app.Selection.TypeText(fecha);
                }
                else if (field.Code.Text.Contains("Diagnostico"))
                {
                    field.Select();
                    app.Selection.TypeText(diagnostico);
                }
                else if (field.Code.Text.Contains("Medicamento"))
                {
                    field.Select();
                    app.Selection.TypeText(medicamento);
                }
                else if (field.Code.Text.Contains("Doctor"))
                {
                    field.Select();
                    app.Selection.TypeText(doctor);
                }
            }

            conn.Close();
            doc.SaveAs(path + "-" + nombre + ".docx");
            string finalpath = path + "-" + nombre + ".docx";

            Imprimir(doc, app, finalpath);
        }


        //******||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||||

        private void btnOtroRealizarConsulta_Click(object sender, EventArgs e)
        {
            if (ListoParaAgregar())
            {
                InsertarConsulta();
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
            }          
        }

        private void btnDocenteRealizarConsulta_Click(object sender, EventArgs e)
        {
            
            if (ListoParaAgregar())
            {
                InsertarConsulta();
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
            }
        }

        private void chbOtro_CheckedChanged(object sender, EventArgs e)
        {
            //Determinar si es dentro o fuera del plantel
            if (chbOtro.Checked == true)
            {
                //Es de Fuera
                lblTituloIngreso.Text = "Ingrese el nombre del Paciente";       
                tbCodigoCerti.MaxLength = 60;
                //Textbox solo admite letras
            }
            else
            {
                //Es de dentro
                lblTituloIngreso.Text = "Ingrese el numero identificador del Paciente";
                tbCodigoCerti.MaxLength = 10;
                //textbox solo admite numeros
            }

            tbCodigoCerti.Text = "";

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tbCodigoCerti.Text = "";
            cbTipoDct.Enabled = false;
            cbTipoDct.SelectedIndex = -1;
            btnImprimir.Enabled = false;
            tbCodigoCerti.Enabled = true;
            chbOtro.Enabled = true;
            chbOtro.Checked = false;
            btnCancelar.Hide();
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            var application = new Microsoft.Office.Interop.Word.Application();
            var document = new Microsoft.Office.Interop.Word.Document();

            if (tipo == 1)
            {
                string tipodedoc = "CertificadoMedico";

                if (chbOtro.Checked == false)
                {
                    LlenarDocPlantel(document, application, tipodedoc);
                }
                else
                {
                    LlenarDocFueraPlantel(document, application, tipodedoc);
                }
            } //tipo certificado
            else
            {
                //tipo receta
                string tipodedoc = "Receta";
                if (chbOtro.Checked == false)
                {
                    LlenarDocPlantel(document, application, tipodedoc);
                }
                else
                {
                    LlenarDocFueraPlantel(document, application, tipodedoc);

                }
            } //tipo receta
        }

        private void cbTipoDct_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determinacion de Tipo de Documento
            if (cbTipoDct.SelectedIndex == 0)
            {
                //Tipo 1 Es Certificado Medico
                tipo = 1;
                if (RegistroSeleccionado == true)
                {
                    btnImprimir.Enabled = true;
                }
            }
            else
            {
                //Tipo 2 es Receta
                tipo = 2;
                if (RegistroSeleccionado == true)
                {
                    btnImprimir.Enabled = true;
                }
            }
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            //Panel Del Listado Ocultar
            pnlListaCerti.Hide();
        }

        private void tbCodigoCerti_TextChanged(object sender, EventArgs e)
        {
            if (chbOtro.Checked == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(tbCodigoCerti.Text, @"^[0-9M]+$") || tbCodigoCerti.Text.Length < 1)
                {
                }
                else
                {
                    tbCodigoCerti.Text = tbCodigoCerti.Text.Remove(tbCodigoCerti.Text.Length - 1);
                }

                if (tbCodigoCerti.TextLength < 10)
                {
                }
                else
                {

                    tbCodigoCerti.Text = tbCodigoCerti.Text.Remove(tbCodigoCerti.Text.Length - 1);//Si se puede encontrar que no detecte lo escrito mejor
                }
            }
            else
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(tbCodigoCerti.Text, @"^[a-zA-Z\s]+$") || tbCodigoCerti.Text.Length < 1)
                {
                }
                else
                {
                    tbCodigoCerti.Text = tbCodigoCerti.Text.Remove(tbCodigoCerti.Text.Length - 1);
                }
                if (tbCodigoCerti.TextLength < 60)
                {
                }
                else
                {
                    tbCodigoCerti.Text = tbCodigoCerti.Text.Remove(tbCodigoCerti.Text.Length - 1);//Si se puede encontrar que no detecte lo escrito mejor
                }
            }
        }

        private void btnBuscarCerti_Click(object sender, EventArgs e)
        {
            if (tbCodigoCerti.Text != "")
            {

                if (chbOtro.Checked == false) //buscar por numero de control de alumno y docente ya que es dentro del plantel
                {

                    input = tbCodigoCerti.Text;
                    string cadQuery1 = "Select num_control,num_docente,fecha,diagnostico,medicamento,seguimiento,edad,sexo,doctor from consultas where num_control ='" + tbCodigoCerti.Text + "' or num_docente= '" + tbCodigoCerti.Text + "'";
                    pnlListaCerti.Show();

                    //llenado del Data Grid View
                    var dataAdapter = new SqlDataAdapter(cadQuery1, conn);
                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                    var ds = new DataSet();
                    dataAdapter.Fill(ds);
                    dgvListaCerti.ReadOnly = true;
                    dgvListaCerti.DataSource = ds.Tables[0];

                    conn.Close();
                    tbCodigoCerti.Text = input;

                    foreach (DataGridViewRow row in dgvListaCerti.SelectedRows)
                    {
                        num_control = row.Cells[0].Value.ToString();
                        num_docente = row.Cells[1].Value.ToString();
                        fecha = row.Cells[2].Value.ToString();
                        diagnostico = row.Cells[3].Value.ToString();
                        medicamento = row.Cells[4].Value.ToString();
                        seguimiento = row.Cells[5].Value.ToString();
                        edad = row.Cells[6].Value.ToString();
                        sexo = row.Cells[7].Value.ToString();
                        doctor = row.Cells[8].Value.ToString();


                    }
                    if (num_docente == "")
                    {
                        //alumno
                        num_id = num_control;
                        banderaalumno = true;
                        dgvListaCerti.Columns[1].Visible = false;
                        dgvListaCerti.Columns[0].Visible = true;

                    }
                    else
                    {
                        //docente
                        num_id = num_docente;
                        banderaalumno = false;
                        dgvListaCerti.Columns[0].Visible = false;
                        dgvListaCerti.Columns[1].Visible = true;
                    }
                }
                else
                {
                    //Busqueda por nombre
                    input = tbCodigoCerti.Text;
                    string cadQuery1 = "select o.nombre,c.edad,c.sexo,c.fecha,c.diagnostico,c.medicamento,c.seguimiento,c.doctor from consultas as c inner join otro as o on c.num_otro=o.num_otro where nombre like '%" + tbCodigoCerti.Text + "%'";
                    pnlListaCerti.Show();
                    //dgvListaCerti.Rows.Clear();

                    var dataAdapter = new SqlDataAdapter(cadQuery1, conn);
                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                    var ds = new DataSet();
                    dataAdapter.Fill(ds);
                    dgvListaCerti.ReadOnly = true;
                    dgvListaCerti.DataSource = ds.Tables[0];

                    conn.Close();
                    tbCodigoCerti.Text = input;
                }
            }
            else
            {
                MessageBox.Show("Porfavor Ingrese un dato en el Buscador", "Error de Busqueda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvListaCerti_SelectionChanged(object sender, EventArgs e)
        {
            //cambio de seleccion de row
            if (chbOtro.Checked == false)
            {
                foreach (DataGridViewRow row in dgvListaCerti.SelectedRows)
                {
                    num_control = row.Cells[0].Value.ToString();
                    num_docente = row.Cells[1].Value.ToString();
                    fecha = row.Cells[2].Value.ToString();
                    diagnostico = row.Cells[3].Value.ToString();
                    medicamento = row.Cells[4].Value.ToString();
                    seguimiento = row.Cells[5].Value.ToString();
                    edad = row.Cells[6].Value.ToString();
                    sexo = row.Cells[7].Value.ToString();
                    doctor = row.Cells[8].Value.ToString();


                }
                if (num_docente == "")
                {
                    //alumno
                    num_id = num_control;
                    banderaalumno = true;
                    dgvListaCerti.Columns[1].Visible = false;
                    dgvListaCerti.Columns[0].Visible = true;

                }
                else
                {
                    //docente
                    num_id = num_docente;
                    banderaalumno = false;
                    dgvListaCerti.Columns[0].Visible = false;
                    dgvListaCerti.Columns[1].Visible = true;
                }


            }
            else
            {
                foreach (DataGridViewRow row in dgvListaCerti.SelectedRows)
                {
                    //otro
                    nombre = row.Cells[0].Value.ToString();
                    edad = row.Cells[1].Value.ToString();
                    sexo = row.Cells[2].Value.ToString();
                    fecha = row.Cells[3].Value.ToString();
                    diagnostico = row.Cells[4].Value.ToString();
                    medicamento = row.Cells[5].Value.ToString();
                    seguimiento = row.Cells[6].Value.ToString();
                    doctor = row.Cells[7].Value.ToString();

                }

            }
        }

        private void btnListaContinuar_Click(object sender, EventArgs e)
        {
            //Verifica que se haya seleccionado uno no en blanco
            if (num_control == "" && num_docente == "" && nombre == "")
            {
                MessageBox.Show("Seleccione un registro no en blanco", "Error de Registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                pnlListaCerti.Hide();
                tbCodigoCerti.Enabled = false;
                chbOtro.Enabled = false;
                RegistroSeleccionado = true;
                cbTipoDct.Enabled = true;
                btnCancelar.Show();

            }
        }

        private void cbOtroMedicamento2_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento2();
        }

        private void cbOtroMedicamento3_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento3();
        }

        private void btnOtroMasMed_Click(object sender, EventArgs e)
        {
            if (!Med2 && !Med3)
            {
                Med2 = true;
                lblMed2.Visible = true;
                cbOtroMedicamento2.Visible = true;
            }
            else if (Med2 && !Med3)
            {
                Med3 = true;
                lblMed3.Visible = true;
                cbOtroMedicamento3.Visible = true;
            }
            else if (Med2 && Med3)
            {
                MessageBox.Show("Tres medicamentos es la cantidad maxima de medicamentos por consulta. En caso de requerir insertar mas medicamentos, ingrese otra consulta", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDocenteMasMed_Click(object sender, EventArgs e)
        {
            
            if (!Med2 && !Med3)
            {
                Med2 = true;
                lblDocMed2.Visible = true;
                ddbDocenteMedicamento2.Visible = true;
            }
            else if (Med2 && !Med3)
            {
                Med3 = true;
                lblDocMed3.Visible = true;
                ddbDocenteMedicamento3.Visible = true;
            }
            else if (Med2 && Med3)
            {
                MessageBox.Show("Tres medicamentos es la cantidad maxima de medicamentos por consulta. En caso de requerir insertar mas medicamentos, ingrese otra consulta", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ddbDocenteMedicamento2_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento2();
        }

        private void ddbDocenteMedicamento3_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento3();
        }

        private void btnAlumnoMasMed_Click(object sender, EventArgs e)
        {
            if (!Med2 && !Med3)
            {
                Med2 = true;
                lblAlumnoMed2.Visible = true;
                ddbAlumnoMedicamento2.Visible = true;
            }
            else if (Med2 && !Med3)
            {
                Med3 = true;
                lblAlumnoMed3.Visible = true;
                ddbAlumnoMedicamento3.Visible = true;
            }
            else if (Med2 && Med3)
            {
                MessageBox.Show("Tres medicamentos es la cantidad maxima de medicamentos por consulta. En caso de requerir insertar mas medicamentos, ingrese otra consulta", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ddbAlumnoMedicamento2_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento2();
        }

        private void ddbAlumnoMedicamento3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ddbAlumnoMedicamento3_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento3();
        }

        private void tbOtroNombre_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroNombre.Text, @"^[a-zA-Z0-9\s]+$") || tbOtroNombre.Text.Length < 1)
            {
            }
            else
            {
                tbOtroNombre.Text = tbOtroNombre.Text.Remove(tbOtroNombre.Text.Length - 1);
            }
        }

        private void tbOtroRelacion_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroRelacion.Text, @"^[a-zA-Z0-9\s]+$") || tbOtroRelacion.Text.Length < 1)
            {
            }
            else
            {
                tbOtroRelacion.Text = tbOtroRelacion.Text.Remove(tbOtroRelacion.Text.Length - 1);
            }
        }

        private void tbOtroEdad_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroEdad.Text, "^[0-9]+$") || tbOtroEdad.Text.Length < 1)
            {
            }
            else
            {
                tbOtroEdad.Text = tbOtroEdad.Text.Remove(tbOtroEdad.Text.Length - 1);
            }
        }

        private void tbOtroMotivo_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroMotivo.Text, @"^[a-zA-Z0-9\s]+$") || tbOtroMotivo.Text.Length < 1)
            {
            }
            else
            {
                tbOtroMotivo.Text = tbOtroMotivo.Text.Remove(tbOtroMotivo.Text.Length - 1);
            }
        }

        private void tbAlumnoMotivo_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAlumnoMotivo.Text, @"^[a-zA-Z0-9\s]+$") || tbAlumnoMotivo.Text.Length < 1)
            {
            }
            else
            {
                tbAlumnoMotivo.Text = tbAlumnoMotivo.Text.Remove(tbAlumnoMotivo.Text.Length - 1);
            }
        }

        private void tbDocenteMotivo_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbDocenteMotivo.Text, @"^[a-zA-Z0-9\s]+$") || tbDocenteMotivo.Text.Length < 1)
            {
            }
            else
            {
                tbDocenteMotivo.Text = tbDocenteMotivo.Text.Remove(tbDocenteMotivo.Text.Length - 1);
            }
        }

        private void cbOtroDiagnostico_Click(object sender, EventArgs e)
        {
            LlenaCbDiagnostico();
        }

        private void cbOtroMedicamento_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento();
        }

        private void ddbAlumnoDiagnostico_Click(object sender, EventArgs e)
        {
            LlenaCbDiagnostico();
        }

        private void ddbAlumnoMedicamento_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento();
        }

        private void ddbDocenteDiagnostico_Click(object sender, EventArgs e)
        {
            LlenaCbDiagnostico();
        }

        private void ddbDocenteMedicamento_Click(object sender, EventArgs e)
        {
            LlenaCbMedicamento();
        }
    }
}
