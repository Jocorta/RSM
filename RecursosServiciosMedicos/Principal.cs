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
using System.Data.OleDb;

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
        DataTable dsEvento = new DataTable();
        DataTable dsUsuario = new DataTable();
        DataTable dsUsuario2 = new DataTable();
        DataTable dsMedicamento = new DataTable();
        DataTable dsMedicamento2 = new DataTable();
        DataTable dsMedicamento3 = new DataTable();
        bool Med2 = false;
        bool Med3 = false;
        public string nombre = "", num_id = "", num_control = "", num_docente = "", seguimiento = "", fecha = "",motivo="", medicamento = "", diagnostico = "", num_otro = "", edad = "", sexo = "", doctor = "",nombredoc="",cedula="";
        public bool RegistroSeleccionado = false, banderaalumno;
        public int tipo = 0;
        public Principal()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-48PLDOP;initial catalog=RSM;integrated security=true");//conexion base de datos
        #region Funciones
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
        private void AltaDocente()
        {
            string data_source;

            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "Archivo Excel |*.xlsx;*.xls;*.xlsm";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                data_source = choofdlog.FileName;//path de el archivo excel
                //variables de documento excel
                var xlapp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xldoc = xlapp.Workbooks.Open(data_source);
                Microsoft.Office.Interop.Excel.Worksheet hoja = xldoc.Sheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                xldoc = xlapp.Workbooks.Add(data_source);
                hoja = xldoc.Sheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                //conexion OleDb para jalar la info del excel
                string conexion = "Provider=Microsoft.Jet.OleDb.4.0; Data Source=" + data_source + ";Extended Properties=\"Excel 8.0; HDR=Yes\"";//conexion al archivo excel
                OleDbConnection origen = default(OleDbConnection);
                origen = new OleDbConnection(conexion);
                //seleccion de todo dentro de la hoja
                OleDbCommand seleccion = default(OleDbCommand);
                seleccion = new OleDbCommand("Select * From [" + hoja.Name + "$]", origen);
                //llenador
                OleDbDataAdapter adaptador = new OleDbDataAdapter();
                adaptador.SelectCommand = seleccion;
                DataSet ds = new DataSet();
                adaptador.Fill(ds);
                //cerrar cosas abiertas
                origen.Close();
                xldoc.Close(SaveChanges: false);
                xlapp.Quit();
                adaptador.Dispose();
                seleccion.Dispose();
                //Conexion Base de datos
                conn.Open();
                //Limpiar Tabla
                string commandText = "ALTER TABLE docente nocheck constraint all; " +
                    "ALTER TABLE consultas nocheck constraint all; delete from docente; " +
                    "ALTER TABLE docente check constraint all; " +
                    "ALTER TABLE consultas check constraint all;";
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                //Importar lo dentro del data set
                SqlBulkCopy importar = default(SqlBulkCopy);
                importar = new SqlBulkCopy(conn);
                importar.DestinationTableName = "docente";
                importar.WriteToServer(ds.Tables[0]);
                conn.Close();
            }
            else
            {
            }

        }
        private void AltaAlumnoIth()
        {
            string data_source;

            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "Archivo Excel |*.xlsx;*.xls;*.xlsm";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {

                data_source = choofdlog.FileName;//path de el archivo excel

                //variables de documento excel
                var xlapp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xldoc = xlapp.Workbooks.Open(data_source);
                Microsoft.Office.Interop.Excel.Worksheet hoja = xldoc.Sheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                xldoc = xlapp.Workbooks.Add(data_source);
                hoja = xldoc.Sheets[1] as Microsoft.Office.Interop.Excel.Worksheet;

                //conexion OleDb para jalar la info del excel
                string conexion = "Provider=Microsoft.Jet.OleDb.4.0; Data Source=" + data_source + ";Extended Properties=\"Excel 8.0; HDR=Yes\"";//conexion al archivo excel
                OleDbConnection origen = default(OleDbConnection);
                origen = new OleDbConnection(conexion);

                //seleccion de todo dentro de la hoja
                OleDbCommand seleccion = default(OleDbCommand);
                seleccion = new OleDbCommand("Select * From [" + hoja.Name + "$]", origen);


                //llenador
                OleDbDataAdapter adaptador = new OleDbDataAdapter();
                adaptador.SelectCommand = seleccion;
                DataSet ds = new DataSet();
                adaptador.Fill(ds);



                //cerrar cosas abiertas
                origen.Close();
                xldoc.Close(SaveChanges: false);
                xlapp.Quit();
                adaptador.Dispose();
                seleccion.Dispose();
                //Conexion Base de datos
                conn.Open();

                //Limpiar Tabla

                string commandText = "ALTER TABLE alumno nocheck constraint all; " +
                    "ALTER TABLE consultas nocheck constraint all; delete from alumno; " +
                    "ALTER TABLE alumno check constraint all; " +
                    "ALTER TABLE consultas check constraint all";
                using (SqlCommand cmd = new SqlCommand(commandText, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                //Importar lo dentro del data set
                SqlBulkCopy importar = default(SqlBulkCopy);
                importar = new SqlBulkCopy(conn);
                importar.DestinationTableName = "alumno";
                importar.WriteToServer(ds.Tables[0]);

                conn.Close();


            }
            else
            {

            }

        }
        private void AltaAlumnoEvento()
        {
            string data_source;

            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "Archivo Excel |*.xlsx;*.xls;*.xlsm";
            choofdlog.FilterIndex = 1;
            choofdlog.Multiselect = false;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {

                data_source = choofdlog.FileName;//path de el archivo excel

                //variables de documento excel
                var xlapp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbook xldoc = xlapp.Workbooks.Open(data_source);
                Microsoft.Office.Interop.Excel.Worksheet hoja = xldoc.Sheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                xldoc = xlapp.Workbooks.Add(data_source);
                hoja = xldoc.Sheets[1] as Microsoft.Office.Interop.Excel.Worksheet;
                //conexion OleDb para jalar la info del excel
                string conexion = "Provider=Microsoft.Jet.OleDb.4.0; Data Source=" + data_source + ";Extended Properties=\"Excel 8.0; HDR=Yes\"";//conexion al archivo excel
                OleDbConnection origen = default(OleDbConnection);
                origen = new OleDbConnection(conexion);
                //seleccion de todo dentro de la hoja
                OleDbCommand seleccion = default(OleDbCommand);
                seleccion = new OleDbCommand("Select * From [" + hoja.Name + "$]", origen);
                //llenador
                OleDbDataAdapter adaptador = new OleDbDataAdapter();
                adaptador.SelectCommand = seleccion;
                DataSet ds = new DataSet();
                adaptador.Fill(ds);
                //cerrar cosas abiertas
                origen.Close();
                xldoc.Close(SaveChanges: false);
                xlapp.Quit();
                adaptador.Dispose();
                seleccion.Dispose();
                //Conexion Base de datos
                conn.Open();
                //Importar lo dentro del data set
                SqlBulkCopy importar = default(SqlBulkCopy);
                importar = new SqlBulkCopy(conn);
                importar.DestinationTableName = "alumno";
                importar.WriteToServer(ds.Tables[0]);
                conn.Close();

            }
            else
            {

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
        public void LlenaCbUsuario()
        {
            dsUsuario.Clear();
            conn.Open();
            string strCmdUsuario = "select usuario from usuario where usuario != 'DSE'";
            SqlCommand cmdCbUsuario = new SqlCommand(strCmdUsuario, conn);
            SqlDataAdapter daUsuario = new SqlDataAdapter(strCmdUsuario, conn);

            daUsuario.Fill(dsUsuario);
            cmdCbUsuario.ExecuteNonQuery();
            conn.Close();

            cbAdminBajaUsr.DisplayMember = "usuario";
            cbAdminBajaUsr.ValueMember = "usuario";
            cbAdminBajaUsr.DataSource = dsUsuario;
            cbAdminBajaUsr.Enabled = true;

        }
        public void LlenaCbUsuario2()
        {
            dsUsuario2.Clear();
            conn.Open();
            string strCmdUsuario2 = "select usuario from usuario where usuario != 'DSE'";
            SqlCommand cmdCbUsuario2 = new SqlCommand(strCmdUsuario2, conn);
            SqlDataAdapter daUsuario2 = new SqlDataAdapter(strCmdUsuario2, conn);

            daUsuario2.Fill(dsUsuario2);
            cmdCbUsuario2.ExecuteNonQuery();
            conn.Close();


            cbAdminCambioPswUsr.DisplayMember = "usuario";
            cbAdminCambioPswUsr.ValueMember = "usuario";
            cbAdminCambioPswUsr.DataSource = dsUsuario2;
            cbAdminCambioPswUsr.Enabled = true;
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
            cbAdminDia.DisplayMember = "nombre";
            cbAdminDia.ValueMember = "nombre";
            cbAdminDia.DataSource = dsDiagnostico;
            cbAdminDia.Enabled = true;
        }
        public void LlenaCbMedicamento()
        {
            //Llenar ComboBox de Medicamento:
            dsMedicamento.Clear();
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
            cbAdminMed.DisplayMember = "nombre";
            cbAdminMed.ValueMember = "nombre";
            cbAdminMed.DataSource = dsMedicamento;
            cbAdminMed.Enabled = true;
        }
        public void LlenaCbMedicamento2()
        {
            //Llenar ComboBox de Medicamento:
            dsMedicamento2.Clear();
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
        public void LlenaCbEvento()
        {
            dsEvento.Clear();
            conn.Open();
            string strCmdCbEvento = "select nombre from evento";
            SqlCommand cmdCbDiagnostico = new SqlCommand(strCmdCbEvento, conn);
            SqlDataAdapter daEvento = new SqlDataAdapter(strCmdCbEvento, conn);
            daEvento.Fill(dsEvento);
            cmdCbDiagnostico.ExecuteNonQuery();
            conn.Close();
            cbAdminAlumnoEvento.DisplayMember = "nombre";
            cbAdminAlumnoEvento.ValueMember = "nombre";
            cbAdminAlumnoEvento.DataSource = dsEvento;
            cbAdminAlumnoEvento.Enabled = true;
        }
        private string DeterminaSemestre(int numControl)
        {
            string Añostring;
            int Añoescolar;
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
            conn.Open();
            SqlCommand comando = new SqlCommand(cadQuery, conn);
            SqlDataReader leer3 = comando.ExecuteReader();

            if (tipodoc == "CertificadoMedico")
            {
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
                            conn.Close();
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
                            string querydoc = "Select * from usuario where usuario='" + doctor + "'";
                            SqlCommand comando2 = new SqlCommand(querydoc, conn);
                            conn.Open();
                            SqlDataReader leer4 = comando2.ExecuteReader();

                            if (leer4.Read() == true)
                            {
                                nombredoc = leer4["nombre_usuario"].ToString();
                                cedula = leer4["cedula"].ToString();
                                field.Select();
                                app.Selection.TypeText(nombredoc);
                                leer4.Close();
                            }
                        }
                        else if (field.Code.Text.Contains("Cedula"))
                        {

                            field.Select();
                            app.Selection.TypeText(cedula);

                        }
                    }
                    
                }
                conn.Close();
            }
            else if (tipodoc == "Receta")
            {
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
                            conn.Close();
                        }
                        else if (field.Code.Text.Contains("Fecha"))
                        {
                            field.Select();
                            app.Selection.TypeText(fecha);
                        }
                        else if (field.Code.Text.Contains("Motivo"))
                        {
                            field.Select();
                            app.Selection.TypeText(motivo);
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
                            string querydoc = "Select * from usuario where usuario='" + doctor + "'";
                            SqlCommand comando2 = new SqlCommand(querydoc, conn);
                            conn.Open();
                            SqlDataReader leer4 = comando2.ExecuteReader();

                            if (leer4.Read() == true)
                            {
                                nombredoc = leer4["nombre_usuario"].ToString();
                                cedula = leer4["cedula"].ToString();
                                field.Select();
                                app.Selection.TypeText(nombredoc);
                                leer4.Close();
                            }
                        }
                        else if (field.Code.Text.Contains("Cedula"))
                        {

                            field.Select();
                            app.Selection.TypeText(cedula);

                        }



                    }
                }
                conn.Close();
            }
            

            doc.SaveAs(path + "-" + nombre + ".docx");
            string finalpath = path + "-" + nombre + ".docx";
            Imprimir(doc, app, finalpath);
        }
        //Metodo Llenar Documento de ALguien Fuera del Plantel
        private void LlenarDocFueraPlantel(Microsoft.Office.Interop.Word.Document doc, Microsoft.Office.Interop.Word.Application app, string tipodoc)
        {
            string path = @"C:\RSM\DocumentosMedicos\" + tipodoc + @"s\Otros\" + tipodoc;
            doc = app.Documents.Add(Template: path + ".docx");
            if (tipodoc=="CertificadoMedico")
            {
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
                    else if (field.Code.Text.Contains("Doctor"))
                    {
                        string querydoc = "Select * from usuario where usuario='" + doctor + "'";
                        SqlCommand comando2 = new SqlCommand(querydoc, conn);
                        conn.Open();
                        SqlDataReader leer4 = comando2.ExecuteReader();

                        if (leer4.Read() == true)
                        {
                            nombredoc = leer4["nombre_usuario"].ToString();
                            cedula = leer4["cedula"].ToString();
                            field.Select();
                            app.Selection.TypeText(nombredoc);
                            leer4.Close();
                        }
                    }
                    else if (field.Code.Text.Contains("Cedula"))
                    {

                        field.Select();
                        app.Selection.TypeText(cedula);

                    }
                }
            }
            else if (tipodoc=="Receta")
            {
                foreach (Microsoft.Office.Interop.Word.Field field in doc.Fields)
                {
                    if (field.Code.Text.Contains("Nombre"))
                    {
                        app.Selection.TypeText(nombre);
                    }
                    else if (field.Code.Text.Contains("Fecha"))
                    {
                        field.Select();
                        app.Selection.TypeText(fecha);
                    }
                    else if (field.Code.Text.Contains("Motivo"))
                    {
                        field.Select();
                        app.Selection.TypeText(motivo);
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
                        string querydoc = "Select * from usuario where usuario='" + doctor + "'";
                        SqlCommand comando2 = new SqlCommand(querydoc, conn);
                        conn.Open();
                        SqlDataReader leer4 = comando2.ExecuteReader();

                        if (leer4.Read() == true)
                        {
                            nombredoc = leer4["nombre_usuario"].ToString();
                            cedula = leer4["cedula"].ToString();
                            field.Select();
                            app.Selection.TypeText(nombredoc);
                            leer4.Close();
                        }
                    }
                    else if (field.Code.Text.Contains("Cedula"))
                    {

                        field.Select();
                        app.Selection.TypeText(cedula);

                    }

                }
            }
            
            conn.Close();
            doc.SaveAs(path + "-" + nombre + ".docx");
            string finalpath = path + "-" + nombre + ".docx";
            Imprimir(doc, app, finalpath);
        }
        
        
        //metodos CONSULTORIA
        private void HideControlesSE()
        {
            cbSEGeneracion.Hide();
            cbSECarrera.Hide();
            cbSELapso.Hide();
            cbEvento.Hide();
            comboGeneracion.Hide();
            comboCarrera.Hide();
            comboEvento.Hide();
            dtpInicio.Hide();
            dtpFinal.Hide();
            lblSECarreraArea.Hide();
            lblSECarrArMini.Hide();
            lblSEEvento.Hide();
            lblSEEventoMini.Hide();
            lblSEGene.Hide();
            lblSECarreraArea.Hide();
            lblSEGeneMini.Hide();
            lblSELapso.Hide();
            lblSEFechaInicio.Hide();
            lblSEFechaFinal.Hide();
        }
        private void ShowControlesSE()
        {
            cbSEGeneracion.Show();
            cbSECarrera.Show();
            cbSELapso.Show();
            cbEvento.Show();
            comboGeneracion.Show();
            comboCarrera.Show();
            comboEvento.Show();
            dtpInicio.Show();
            dtpFinal.Show();
            lblSECarreraArea.Show();
            lblSECarrArMini.Show();
            lblSEEvento.Show();
            lblSEEventoMini.Show();
            lblSEGene.Show();
            lblSECarreraArea.Show();
            lblSEGeneMini.Show();
            lblSELapso.Show();
            lblSEFechaInicio.Show();
            lblSEFechaFinal.Show();
            tabSEResultados.Hide();
            lblTotalRegistros.Hide();
            lblKEv.Hide();
        }
        private void Aparece(DataGridView tab)
        {
            tab.Width = 450;
            tab.Height = 218;
            tab.Location = new Point(50, 70);
            tab.Visible = true;
            tab.Show();
            HideControlesSE();

        }//Para que la tabla se ajuste al tamaño del panel y se muestre    
        private void LlenarComboBoxServEsc(ComboBox combo, string querty, string atributo)
        {
            SqlCommand cmd = new SqlCommand(querty, conn);
            cmd.CommandText = querty;
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            combo.SelectedIndex = 0;

            while (reader.Read())
            {
                combo.Items.Add(reader[atributo].ToString());
                combo.ValueMember = reader[atributo].ToString();
                combo.DisplayMember = reader[atributo].ToString();
            }

            reader.Close();
            conn.Close();
        }//Llenar ComboBox de busqueda para SE
        private bool Reader(string querty)
        {
            bool r;
            SqlCommand comando = new SqlCommand(querty, conn);
            SqlDataReader leer = comando.ExecuteReader();
            if (leer.Read())
            {
                r = true;
            }
            else
            {
                r = false;
            }
            leer.Close();
            return r;
        }//Ver si la busqueda existe en la BD 
        private void HacerConsulta(string txb, int caso, string control, int tipoUsuario) //Diferentes casos de usuario normal 
        {
            try
            {
                string buscar = "";
                if (tipoUsuario == 1) // Alumnos
                {
                    #region Usuario SE
                    switch (caso)
                    {
                        #region Alumno
                        case 1: //Carrera, lapso, gene, evento
                            #region Carrera, lapso, gene, evento
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                            " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where a." + control + " = '" + txb +
                                            "' and c.fecha >= '" + dtpInicio.Value.Year + "-" + dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                            dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and substring(a.num_control,1,2)= " + comboGeneracion.SelectedItem +
                                            " and e.nombre = '" + comboEvento.SelectedItem + "'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            break;
                        #endregion

                        case 2: //Carrera, lapso, gene
                            #region Carrera, lapso, gene
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where a." + control + " = '" + txb +
                                                "' and c.fecha >= '" + dtpInicio.Value.Year + "-" + dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                                dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and substring(a.num_control,1,2)= " + comboGeneracion.SelectedItem +
                                                " and a.evento is null";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                lblKEv.Show();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            break;
                        #endregion

                        case 3: //Carrera, lapso, evento
                            #region Carrera, lapso, evento
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                            " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where a." + control + " = '" + txb +
                                            "' and c.fecha >= '" + dtpInicio.Value.Year + "-" + dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                            dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and e.nombre = '" + comboEvento.SelectedItem + "'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            break;
                        #endregion

                        case 4: //Carrera, lapso
                            #region Carrera, lapso
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }


                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where a." + control + " = '" + txb +
                                                "' and c.fecha >= '" + dtpInicio.Value.Year + "-" + dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                                dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and a.evento is null";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                lblKEv.Show();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            break;
                            #endregion
                            break;
                        case 5: //Carrera, gene, evento
                            #region Carrera, gene, evento
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                           " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where a." + control + " = '" + txb +
                                            "' and substring(a.num_control,1,2)= " + comboGeneracion.SelectedItem + " and e.nombre = '" + comboEvento.SelectedItem + "'";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 6: //Carrera, Gene
                            #region Carrera, gene
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                           " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control where a." + control + " = '" + txb +
                                            "' and substring(a.num_control,1,2)= " + comboGeneracion.SelectedItem + " and a.evento is null";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 7:  //Carrera, evento
                            #region Carrera, evento
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                           " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where a." + control + " = '" + txb +
                                            "' and e.nombre = '" + comboEvento.SelectedItem + "'";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 8: //Carrera
                            #region Carrera
                            #region Carrera
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una carrera", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where a." + control + " = '" + txb + "' and a.evento is null";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                lblKEv.Show();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 9: //Lapso, gene, evento
                            #region Lapso, gene, evento
                            if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                            " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where c.fecha >= '" +
                                            dtpInicio.Value.Year + "-" + dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                            dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and substring(a.num_control,1,2)= " + comboGeneracion.SelectedItem +
                                            " and e.nombre = '" + comboEvento.SelectedItem + "'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 10: //Lapso, gene
                            #region Lapso, gene
                            if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where c.fecha >= '" + dtpInicio.Value.Year + "-" +
                                                dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" + dtpFinal.Value.Year + "-" +
                                                dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and substring(a.num_control,1,2)= " + comboGeneracion.SelectedItem + " and a.evento is null";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                lblKEv.Show();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 11: //Lapso, evento
                            #region Lapso, evento
                            if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                            " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where c.fecha >= '" +
                                            dtpInicio.Value.Year + "-" + dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                            dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and e.nombre = '" + comboEvento.SelectedItem + "'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 12: //Lapso
                            #region Lapso

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where c.fecha >= '" + dtpInicio.Value.Year + "-" +
                                                dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" + dtpFinal.Value.Year + "-" +
                                                dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59'";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                lblKEv.Show();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 13: //Gene, evento
                            #region Gene, evento
                            if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            else if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                            " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where substring(a.num_control,1,2)= " +
                                            comboGeneracion.SelectedItem + " and e.nombre = '" + comboEvento.SelectedItem + "'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 14: //Gene
                            #region Gene
                            if (comboGeneracion.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una generación", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }

                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where substring(a.num_control,1,2)= " +
                                                comboGeneracion.SelectedItem + " and a.evento is null";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                lblKEv.Show();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;
                        case 15: //Evento
                            #region Evento
                            if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                            " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join alumno as a on c.num_control=a.num_control inner join  evento as e on e.num_evento=a.evento where e.nombre = '" +
                                            comboEvento.SelectedItem + "'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;

                        #endregion  //No sé, pero no se mueve porque si no está, no funciona :)
                        #endregion //Alumno
                        #region Docente
                        //Lapso, area, evento
                        //case 16:
                        //    #region Lapso, area, evento
                        //    #endregion
                        //    break;

                        //Lapso, area
                        case 17:
                            #region Lapso, area
                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una departamento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select d.num_docente as 'Número de Control', d.nombre as 'Nombre', d.departamento as Departamento, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join docente as d on c.num_docente=c.num_docente where d.departamento = '" + comboCarrera.SelectedItem +
                                            "' and c.fecha >= '" + dtpInicio.Value.Year + "-" + dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                            dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;

                        //Lapso, evento
                        //case 18:
                        //    #region Lapso, evento
                        //    #endregion
                        //    break;

                        //Lapso
                        case 19:
                            #region Lapso

                            conn.Open();
                            buscar = " select d.num_docente as 'Número de Control', d.nombre as 'Nombre', d.departamento as Departamento, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join docente as d on c.num_docente=c.num_docente where c.fecha >= '" + dtpInicio.Value.Year + "-" +
                                            dtpInicio.Value.Month + "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" + dtpFinal.Value.Year + "-" + dtpFinal.Value.Month +
                                            "-" + dtpFinal.Value.Day + " 23:59'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;

                        //Area, evento
                        //case 20:
                        //    #region Area, evento
                        //    #endregion
                        //    break;

                        //Area
                        case 21:
                            #region Area

                            if (comboCarrera.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione una departamento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = " select d.num_docente as 'Número de Control', d.nombre as 'Nombre', d.departamento as Departamento, c.seguimiento as Seguimiento," +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join docente as d on c.num_docente=c.num_docente where d.departamento = '" + comboCarrera.SelectedItem + "'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                btnExportar.Visible = true;
                            }

                            conn.Close();

                            #endregion
                            break;

                        //Evento
                        //case 22:
                        //    #region Evento
                        //    #endregion
                        //    break;

                        #endregion //Docente
                        #region Otro

                        //Lapso y evento
                        case 23:
                            #region Lapso, evento
                            if (comboEvento.SelectedIndex == 0)
                            {
                                MessageBox.Show(this, "Seleccione un evento", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            }
                            conn.Open();
                            buscar = "select o.nombre as 'Nombre', c.seguimiento as Seguimiento, o.relacion as Relación, " +
                                            "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                            "consultas as c inner join otro as o on c.num_otro=o.num_otro where c.fecha >= '" + dtpInicio.Value.Year + "-" + dtpInicio.Value.Month +
                                            "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" + dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day +
                                            " 23:59' and o.relacion like '%Evento%'";
                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblKEv.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = comboEvento.SelectedItem.ToString();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;

                        //Lapso
                        case 24:
                            #region Lapso
                            conn.Open();
                            buscar = " select o.nombre as 'Nombre', c.seguimiento as Seguimiento, o.relacion as Relación, " +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join otro as o on c.num_otro=o.num_otro  where c.fecha >= '" + dtpInicio.Value.Year + "-" + dtpInicio.Value.Month +
                                                "-" + dtpInicio.Value.Day + " 00:00' and c.fecha <= '" +
                                                dtpFinal.Value.Year + "-" + dtpFinal.Value.Month + "-" + dtpFinal.Value.Day + " 23:59' and o.relacion not like '%Evento%'";

                            if (!Reader(buscar))
                            {
                                MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblControl.Show();
                                txbBusquedaClave.Show();
                                conn.Close();
                            }
                            else
                            {
                                Aparece(tabSEResultados);
                                var dataAdapter = new SqlDataAdapter(buscar, conn);
                                var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                var ds = new DataSet();
                                dataAdapter.Fill(ds);
                                tabSEResultados.ReadOnly = true;
                                tabSEResultados.DataSource = ds.Tables[0];
                                lblTotalRegistros.Show();
                                lblTotalRegistros.Text = "Total de registros = " + tabSEResultados.RowCount.ToString();
                                lblKEv.Text = "Registros ITH";
                                lblKEv.Show();
                                btnExportar.Visible = true;
                            }

                            conn.Close();
                            #endregion
                            break;

                            #endregion //Otro
                    }
                    #endregion //Usuario SE
                }
                else if (tipoUsuario == 2)
                {
                    #region Usuario Normal
                    if (cbSeguir.Checked)
                    {
                        switch (caso)
                        {
                            case 1: //Alumno fecha y seguimiento
                                conn.Open();
                                buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where c.seguimiento='Si' and c." + control + " >= '" + txb + " 00:00'" +
                                                " and " + "c." + control + " <= '" + txb + " 23:59'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 2:  //Alumno clave y seguimiento
                                conn.Open();
                                buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where c.seguimiento='Si' and c." + control + " = " + txb;
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 3: //Docente fecha y seguimiento
                                conn.Open();
                                buscar = " select c.num_docente as 'Número de Control', d.nombre as 'Nombre', d.departamento as Área, c.seguimiento as Seguimiento," +
                                                        "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                        "consultas as c inner join docente as d on c.num_docente=d.num_docente where c.seguimiento='Si' and c." + control + " >= '" + txb + " 00:00'" +
                                                        " and " + "c." + control + " <= '" + txb + " 23:59'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 4: //Docente clave y seguimiento
                                conn.Open();
                                buscar = " select c.num_docente as 'Número de Control', d.nombre as 'Nombre', d.departamento as Área, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join docente as d on c.num_docente=d.num_docente where c.seguimiento = 'Si' and c." + control + " = " + txb;
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 5: //Otro fecha y seguimiento
                                conn.Open();
                                buscar = " select o.nombre as 'Nombre', c.seguimiento as Seguimiento, o.relacion as 'Relación', " +
                                                        "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                        "consultas as c inner join otro as o on c.num_otro=o.num_otro where c.seguimiento='Si' and c." + control + " >= '" + txb + " 00:00'" +
                                                        " and " + "c." + control + " <= '" + txb + " 23:59'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 6: //Otro nombre y seguimiento
                                conn.Open();
                                buscar = " select o.nombre as 'Nombre', c.seguimiento as Seguimiento, o.relacion as 'Relación', " +
                                                        "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                        "consultas as c inner join otro as o on c.num_otro=o.num_otro where c.seguimiento='Si' and o." + control + " like '%" + txb + "%'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                        }
                    }
                    else
                    {
                        switch (caso)
                        {
                            case 1: //Alumno fecha y sin seguimiento
                                conn.Open();
                                buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where c." + control + " >= '" + txb + " 00:00'" +
                                                " and " + "c." + control + " <= '" + txb + " 23:59'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 2: //Alumno clave y sin seguimiento
                                conn.Open();
                                buscar = " select c.num_control as 'Número de Control', a.nombre as 'Nombre', a.nombre_paterno as 'Apellido Paterno', " +
                                                " a.nombre_materno as 'Apellido Materno', a.carrera as Carrera, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join alumno as a on c.num_control=a.num_control where c." + control + " = " + txb;
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 3: //Docente fecha y sin seguimento
                                conn.Open();
                                buscar = " select c.num_docente as 'Número de Control', d.nombre as 'Nombre', d.departamento as Área, c.seguimiento as Seguimiento," +
                                                        "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                        "consultas as c inner join docente as d on c.num_docente=d.num_docente where c." + control + " >= '" + txb + " 00:00'" +
                                                        " and " + "c." + control + " <= '" + txb + " 23:59'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 4: //Docente clave y sin seguimiento
                                conn.Open();
                                buscar = " select c.num_docente as 'Número de Control', d.nombre as 'Nombre', d.departamento as Área, c.seguimiento as Seguimiento," +
                                                "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                "consultas as c inner join docente as d on c.num_docente=d.num_docente where c." + control + " = " + txb;
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 5: //Otro fecha y sin seguimiento
                                conn.Open();
                                buscar = " select o.nombre as 'Nombre', c.seguimiento as Seguimiento, o.relacion as 'Relación', " +
                                                        "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                        "consultas as c inner join otro as o on c.num_otro=o.num_otro where c." + control + " >= '" + txb + " 00:00'" +
                                                        " and " + "c." + control + " <= '" + txb + " 23:59'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                            case 6: //Otro nombre y sin seguimiento
                                conn.Open();
                                buscar = " select o.nombre as 'Nombre', c.seguimiento as Seguimiento, o.relacion as 'Relación', " +
                                                        "c.fecha as 'Fecha de consulta' , c.doctor as Doctor, c.diagnostico as Diagnóstio , c.medicamento as Medicamento from " +
                                                        "consultas as c inner join otro as o on c.num_otro=o.num_otro where o." + control + " like '%" + txb + "%'";
                                if (!Reader(buscar))
                                {
                                    MessageBox.Show(this, "No se encontró consulta registrada con estos datos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    lblControl.Show();
                                    txbBusquedaClave.Show();
                                    conn.Close();
                                }
                                else
                                {
                                    Aparece(tabResultados);
                                    var dataAdapter = new SqlDataAdapter(buscar, conn);
                                    var commandBuilder = new SqlCommandBuilder(dataAdapter);
                                    var ds = new DataSet();
                                    dataAdapter.Fill(ds);
                                    tabResultados.ReadOnly = true;
                                    tabResultados.DataSource = ds.Tables[0];
                                    btnExportar.Visible = true;
                                }
                                conn.Close();
                                break;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio el siguiente problema: " + ex.Message + "Contecte al administrador." + "\t" + ex.GetType());
                conn.Close();
            }
        }
        private void BuscaConsulta(int tiposDeUsuario) //Busca en base de datos
        {
            int a = 0;
            int b = 0;
            try
            {
                switch (tiposDeUsuario) //Para ver si es normal o SE
                {
                    case 1:
                        // Para ver si es alumno, docente u otro
                        #region contador A
                        if (cbSEAlumno.Checked)
                        {
                            a = 1;
                        }
                        else if (cbSEDocente.Checked)
                        {
                            a = 2;
                        }
                        else if (cbSEOtro.Checked)
                        {
                            a = 3;
                        }
                        #endregion
                        // Para ver si es por Carrera, area, lapso, generacion o evento
                        #region Implementacion
                        switch (a)
                        {
                            case 1:
                                #region Casos B de alumno
                                if (cbSECarrera.Checked)
                                {
                                    if (cbSELapso.Checked)
                                    {
                                        if (cbSEGeneracion.Checked)
                                        {
                                            if (cbEvento.Checked)
                                            {
                                                b = 1; //Carrera, lapso, gene, evento
                                            }
                                            else
                                            {
                                                b = 2; //Carrera, lapso, gene
                                            }
                                        }
                                        else if (cbEvento.Checked)
                                        {
                                            b = 3; //Carrera, lapso, evento
                                        }
                                        else
                                        {
                                            b = 4; //Carrera, lapso
                                        }

                                    }
                                    else if (cbSEGeneracion.Checked)
                                    {
                                        if (cbEvento.Checked)
                                        {
                                            b = 5; //Carrera, gene, evento
                                        }
                                        else
                                        {
                                            b = 6; //Carrera, gene
                                        }
                                    }
                                    else if (cbEvento.Checked)
                                    {
                                        b = 7; //Carrera, evento
                                    }
                                    else
                                    {
                                        b = 8; //Carrera
                                    }
                                }
                                else if (cbSELapso.Checked)
                                {
                                    if (cbSEGeneracion.Checked)
                                    {
                                        if (cbEvento.Checked)
                                        {
                                            b = 9; //Lapso, gene, evento
                                        }
                                        else
                                        {
                                            b = 10; //Lapso, gene
                                        }
                                    }
                                    else if (cbEvento.Checked)
                                    {
                                        b = 11; //Lapso, evento
                                    }
                                    else
                                    {
                                        b = 12; //Lapso
                                    }
                                }
                                else
                                      if (cbSEGeneracion.Checked)
                                {
                                    if (cbEvento.Checked)
                                    {
                                        b = 13; //Gene, evento
                                    }
                                    else
                                    {
                                        b = 14; //Gene
                                    }
                                }
                                else if (cbEvento.Checked)
                                {
                                    b = 15; //Evento
                                }
                                else
                                {
                                    b = 0; //0
                                }
                                #endregion //Casos B alumno
                                #region Switch b alumno
                                switch (b)
                                {
                                    //Carrera, lapso, gene, evento
                                    case 1:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Carrera, lapso, gene
                                    case 2:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Carrera, lapso, evento
                                    case 3:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Carrera, lapso
                                    case 4:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Carrera, gene, evento
                                    case 5:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Carrera, gene
                                    case 6:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Carrera, evento
                                    case 7:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Carrera
                                    case 8:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "carrera", tiposDeUsuario);
                                        break;
                                    //Lapso, gene, evento
                                    case 9:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Lapso, gene
                                    case 10:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Lapso, evento
                                    case 11:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Lapso
                                    case 12:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Gene, evento
                                    case 13:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Gene
                                    case 14:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Evento
                                    case 15:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                }
                                #endregion //Switch B alumno
                                break;
                            case 2:
                                #region Casos B de Docente
                                if (cbSELapso.Checked)
                                {
                                    if (cbSECarrera.Checked)
                                    {
                                        if (cbEvento.Checked)
                                        {
                                            b = 16; //Area, lapso, evento
                                        }
                                        else
                                        {
                                            b = 17; //Area, lapso,
                                        }
                                    }
                                    else if (cbEvento.Checked)
                                    {
                                        b = 18; //Lapso, evento
                                    }
                                    else
                                    {
                                        b = 19; //Lapso
                                    }

                                }
                                else if (cbSECarrera.Checked)
                                {
                                    if (cbEvento.Checked)
                                    {
                                        b = 20; //Area, evento
                                    }
                                    else
                                    {
                                        b = 21; //Area
                                    }
                                }
                                else if (cbEvento.Checked)
                                {
                                    b = 22; //Evento
                                }
                                else
                                {
                                    b = 0; //0
                                }
                                #endregion //Casos B Docente
                                #region Switch B Docente
                                switch (b)
                                {
                                    //Lapso, area, evento
                                    //case 16: HacerConsulta(null, b, "", tiposDeUsuario);
                                    //    break;
                                    //Lapso, area
                                    case 17:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Lapso, evento
                                    //case 18: HacerConsulta(null, b, "", tiposDeUsuario);
                                    //    break;
                                    //Lapso
                                    case 19:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                    //Area, evento
                                    //case 20: HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "departamento", tiposDeUsuario);
                                    //    break;
                                    //Area
                                    case 21:
                                        HacerConsulta(comboCarrera.SelectedItem.ToString(), b, "departamento", tiposDeUsuario);
                                        break;
                                        //Evento
                                        //case 22: HacerConsulta(comboEvento.SelectedItem.ToString(), b, "evento", tiposDeUsuario);
                                        //    break;
                                }
                                #endregion //Switch B docente
                                break;
                            case 3:
                                #region Casos B Otros
                                if (cbEvento.Checked)
                                {
                                    b = 23; //Lapso y evento, incluidos docentes
                                }
                                else
                                {
                                    b = 24; //Lapso ITH
                                }
                                #endregion //B Otro
                                #region Switch Otro
                                switch (b)
                                {
                                    //Lapso y evento
                                    case 23:
                                        HacerConsulta(comboEvento.SelectedItem.ToString(), b, "evento", tiposDeUsuario);
                                        break;
                                    //Lapso
                                    case 24:
                                        HacerConsulta(null, b, "", tiposDeUsuario);
                                        break;
                                }
                                #endregion //Switch Otro
                                break;
                        }
                        #endregion //Implementación
                        break;
                    case 2:
                        // Para ver si es alumno, docente u otro
                        #region Contador A
                        if (cbAlumnoBusqueda.Checked)
                        {
                            a = 1;
                        }
                        else if (cbDocenteBusqueda.Checked)
                        {
                            a = 2;
                        }
                        else if (cbOtroBusqueda.Checked)
                        {
                            a = 3;
                        }
                        #endregion
                        // Para ver si es por fecha o clave, y si es de seguimiento o no
                        #region Contador B
                        if (cbFecha.Checked)
                        {
                            b = 1;
                        }
                        else if (cbNoControl.Checked)
                        {
                            b = 2;
                        }
                        #endregion
                        // Para que ejecute los diferentes casos
                        #region Implementacion Normal
                        switch (a)
                        {
                            case 1:
                                switch (b)
                                {
                                    case 1:

                                        HacerConsulta(calCalendario.SelectionRange.Start.Year.ToString() + "-" + calCalendario.SelectionRange.Start.Month.ToString() + "-" + calCalendario.SelectionRange.Start.Day.ToString(), 1, "fecha", tiposDeUsuario);
                                        break;
                                    case 2:
                                        HacerConsulta(txbBusquedaClave.Text, 2, "num_control", tiposDeUsuario);
                                        break;
                                }
                                break;
                            case 2:
                                switch (b)
                                {
                                    case 1:
                                        HacerConsulta(calCalendario.SelectionRange.Start.Year.ToString() + "-" + calCalendario.SelectionRange.Start.Month.ToString() + "-" + calCalendario.SelectionRange.Start.Day.ToString(), 3, "fecha", tiposDeUsuario);
                                        break;
                                    case 2:
                                        HacerConsulta(txbBusquedaClave.Text, 4, "num_docente", tiposDeUsuario);
                                        break;
                                }
                                break;
                            case 3:
                                switch (b)
                                {
                                    case 1:
                                        HacerConsulta(calCalendario.SelectionRange.Start.Year.ToString() + "-" + calCalendario.SelectionRange.Start.Month.ToString() + "-" + calCalendario.SelectionRange.Start.Day.ToString(), 5, "fecha", tiposDeUsuario);
                                        break;
                                    case 2:
                                        HacerConsulta(txbBusquedaClave.Text, 6, "nombre", tiposDeUsuario);
                                        break;
                                }
                                break;
                        }
                        #endregion
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio el siguiente problema: " + ex.Message + "Contecte al administrador." + "\t" + ex.GetType());
                conn.Close();
            }
        }
        private void cbFalse()
        {
            cbSECarrera.Checked = false;
            comboCarrera.SelectedIndex = 0;
            comboCarrera.Enabled = false;
            cbSELapso.Checked = false;
            dtpInicio.Value = DateTime.Now;
            dtpInicio.Enabled = false;
            dtpFinal.Value = DateTime.Now;
            dtpFinal.Enabled = false;
            cbSEGeneracion.Checked = false;
            comboGeneracion.SelectedIndex = 0;
            comboGeneracion.Enabled = false;
            cbEvento.Checked = false;
            comboEvento.SelectedIndex = 0;
            comboEvento.Enabled = false;
        }//Funcion regresa todo FALSE en CONSULTA AVANZADA
        private void copyAlltoClipboard(DataGridView tabla)
        {
            tabla.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            tabla.MultiSelect = true;
            tabla.SelectAll();
            DataObject dataObj = tabla.GetClipboardContent();
            if (dataObj != null)

                Clipboard.SetDataObject(dataObj);
        }
        private void ImportarExcel(DataGridView grid)
        {
            copyAlltoClipboard(grid);
            Microsoft.Office.Interop.Excel.Application xlexcel;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlexcel = new Microsoft.Office.Interop.Excel.Application();
            xlexcel.Visible = true;
            xlWorkBook = xlexcel.Workbooks.Add(misValue);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
            CR.Select();
            xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        }
        public void Checar()
        {
            conn.Open();
            SqlCommand comandoEvento = new SqlCommand("delete alumno where evento in (select num_evento from evento where fecha_fin < ' " + DateTime.Now.ToString("yyyy-MM-dd h:mm tt") + "');", conn);
            comandoEvento.ExecuteNonQuery();
            MessageBox.Show("Se ha actualizado la base de datos", "Actualizacion", MessageBoxButtons.OK);
            conn.Close();
        }
        #endregion

        #region Controles Complejos
        public Principal(string LoggedUser)
        {
            Usuario = LoggedUser.ToLower();
            InitializeComponent();
            lblFecha.Text = DateTime.Now.ToString("MM/dd/yyyy");
            btnCerrar.Text = Usuario;


        } //CONSTRUCTOR CON USER
        private void Principal_Load(object sender, EventArgs e)
        {
            LlenaCbDiagnostico();
            LlenaCbMedicamento();
            LlenaCbEvento();
            LlenaCbUsuario();
            LlenaCbUsuario2();
            cbAlumno.Checked = true;
            pnlAlumno.Show();
            Checar();

            if (Usuario == "dse")
            {
                btnCertificadoMed.Text = "Administracion";
                ttSeguimiento.Active = false;
                pnlBusqueda.Hide();
                pnlConsulta.Hide();
                pnlCertificado.Hide();

                pnlConsultoria.Hide();
                pnlServiciosEscolares.Show();
                pnlAdministracion.Hide();
                btnConsulta.Text = "Consultoría Específica";
                btnConsultoria.Text = "Consultoría General";
                bunifuCustomLabel30.Text = "Servicios Médicos";
            }

            LlenaCbDiagnostico();
            LlenaCbMedicamento();
            LlenarComboBoxServEsc(comboEvento, "Select nombre from evento", "nombre"); //Llena los ComboBox de evento
            LlenarComboBoxServEsc(comboGeneracion, "select distinct substring(num_control,1,2) as 'Generación' from alumno order by Generación asc", "Generación"); //Llena ComboBox de generación
                                                                                                                                                                    // LlenarComboBoxServEsc(comboIT,"",""); //Llena el ComboBox de institutos para eventos
            pnlConsultoria.Hide();

        } // FUNCTION LOAD
        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Cerrar sesión?", "Salir", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                this.Hide();
                Form1 fm1 = new Form1();
                fm1.Show();
            }
        }//Boton de CERRAR SESION
        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if (Usuario == "dse")
            {
                Separator.Location = new Point(15, 177);
                Separator.Show();
                cbSEOtro.Checked = false;
                cbSEAlumno.Checked = false;
                cbSEDocente.Checked = false;
                pnlServiciosEscolares.Hide();
                pnlAdministracion.Hide();
                pnlConsulta.Hide();
                pnlServiciosEscolares.Show();
                btnExportar.Hide();
                pnlEvento.Hide();
                pnlAgregarEvento.Hide();

            }
            else
            {
                Separator.Location = new Point(15, 177);
                Separator.Show();
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
                pnlConsulta.Show();
                pnlCertificado.Hide();
                pnlConsultoria.Hide();
                pnlAdministracion.Hide();
                pnlServiciosEscolares.Hide();
                btnExportar.Hide();
                pnlEvento.Hide();
                pnlAgregarEvento.Hide();
            }

        }//Limpia y muestra PNL CONSULTA
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
        }//Limpia y muestra PNL CONSULTA ALUMNO
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
        }//Limpia y muestra PNL CONSULTA DOCENTE
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
        }//Limpia y muestra PNL CONSULTA OTRO
        private void btnCertificadoMed_Click(object sender, EventArgs e)
        {
            if (Usuario == "dse")
            {
                cbSEOtro.Checked = false;
                cbSEAlumno.Checked = false;
                cbSEDocente.Checked = false;
                Separator.Location = new Point(15, 272);
                Separator.Show();
                pnlConsulta.Hide();
                pnlConsultoria.Hide();
                pnlAdministracion.Show();
                pnlServiciosEscolares.Hide();
                pnlEvento.Hide();
                pnlAgregarEvento.Hide();
            }
            else
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
                pnlConsultoria.Hide();
                pnlServiciosEscolares.Hide();
                pnlEvento.Hide();
                pnlAgregarEvento.Hide();
            }
        }//Muestra paneles dependiendo de USR
        private void tbAlumnoNoControl_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAlumnoNoControl.Text, "^[a-zA-Z0-9]+$") || tbAlumnoNoControl.Text.Length < 1)
            {
            }
            else
            {
                tbAlumnoNoControl.Text = tbAlumnoNoControl.Text.Remove(tbAlumnoNoControl.Text.Length - 1);
            }
        } // Validacion SQL-INJECTION
        private void tbDocenteNoDocente_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbDocenteNoDocente.Text, "^[a-zA-Z0-9]+$") || tbDocenteNoDocente.Text.Length < 1)
            {
            }
            else
            {
                tbDocenteNoDocente.Text = tbDocenteNoDocente.Text.Remove(tbDocenteNoDocente.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void btnAlumnoRealizarConsulta_Click(object sender, EventArgs e)
        {
            if (ListoParaAgregar())
            {
                InsertarConsulta();
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
            }
        }// Verificacion Pre-Consulta
        private void btnOtroRealizarConsulta_Click(object sender, EventArgs e)
        {
            if (ListoParaAgregar())
            {
                InsertarConsulta();
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
            }
        }// Verificacion Pre-Consulta
        private void btnDocenteRealizarConsulta_Click(object sender, EventArgs e)
        {

            if (ListoParaAgregar())
            {
                InsertarConsulta();
                LimpiaAlumno();
                LimpiaDocente();
                LimpiaOtro();
            }
        }// Verificacion Pre-Consulta
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

        }// Verificacion Pre-Imprimir Tipo: OTRO
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
        }// Boton regresa todo como antes
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
        }//Ejecuta funciones para IMPRIMIR
        private void cbTipoDct_SelectedIndexChanged(object sender, EventArgs e)
        {

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
        }//Determinacion de Tipo de Documento
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

                    tbCodigoCerti.Text = tbCodigoCerti.Text.Remove(tbCodigoCerti.Text.Length - 1);
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
        }//Validaciones en Textbox
        private void btnBuscarCerti_Click(object sender, EventArgs e)
        {
            if (tbCodigoCerti.Text != "")
            {

                if (chbOtro.Checked == false) //buscar por numero de control de alumno y docente ya que es dentro del plantel
                {

                    input = tbCodigoCerti.Text;
                    string cadQuery1 = "Select c.num_control,c.num_docente,c.fecha,c.motivo,c.diagnostico,c.medicamento,c.medicamento2,c.medicamento3,c.seguimiento,c.edad,c.sexo,c.doctor from consultas as c inner join usuario as u on c.doctor = u.usuario where c.num_control ='" + tbCodigoCerti.Text + "' or c.num_docente= '" + tbCodigoCerti.Text + "'";
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
                        motivo= row.Cells[3].Value.ToString();
                        diagnostico = row.Cells[4].Value.ToString();
                        medicamento = row.Cells[5].Value.ToString()+""+ row.Cells[6].Value.ToString()+""+ row.Cells[7].Value.ToString();
                        seguimiento = row.Cells[8].Value.ToString();
                        edad = row.Cells[9].Value.ToString();
                        sexo = row.Cells[10].Value.ToString();
                        doctor = row.Cells[11].Value.ToString();


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
                    string cadQuery1 = "select o.nombre,c.edad,c.sexo,c.fecha,c.motivo,c.diagnostico,c.medicamento,c.medicamento2,c.medicamento3,c.seguimiento,c.doctor from consultas as c inner join otro as o on c.num_otro=o.num_otro where nombre like '%" + tbCodigoCerti.Text + "%'";
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

                    foreach (DataGridViewRow row in dgvListaCerti.SelectedRows)
                    {
                        nombre = row.Cells[0].Value.ToString();
                        edad = row.Cells[1].Value.ToString();
                        sexo = row.Cells[2].Value.ToString();
                        fecha = row.Cells[3].Value.ToString();
                        motivo = row.Cells[4].Value.ToString();
                        diagnostico = row.Cells[5].Value.ToString();
                        medicamento = row.Cells[6].Value.ToString() + "" + row.Cells[7].Value.ToString() + "" + row.Cells[8].Value.ToString();
                        seguimiento = row.Cells[9].Value.ToString();
                        doctor = row.Cells[10].Value.ToString();


                    }
                }
            }
            else
            {
                MessageBox.Show("Porfavor Ingrese un dato en el Buscador", "Error de Busqueda", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//A Juan le gusta hacer funciones en botones :)
        private void dgvListaCerti_SelectionChanged(object sender, EventArgs e)
        {
            if (chbOtro.Checked == false)
            {
                foreach (DataGridViewRow row in dgvListaCerti.SelectedRows)
                {
                    num_control = row.Cells[0].Value.ToString();
                    num_docente = row.Cells[1].Value.ToString();
                    fecha = row.Cells[2].Value.ToString();
                    motivo = row.Cells[3].Value.ToString();
                    diagnostico = row.Cells[4].Value.ToString();
                    medicamento = row.Cells[5].Value.ToString() + "" + row.Cells[6].Value.ToString() + "" + row.Cells[7].Value.ToString();
                    seguimiento = row.Cells[8].Value.ToString();
                    edad = row.Cells[9].Value.ToString();
                    sexo = row.Cells[10].Value.ToString();
                    doctor = row.Cells[11].Value.ToString();


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
                    motivo = row.Cells[4].Value.ToString();
                    diagnostico = row.Cells[5].Value.ToString();
                    medicamento = row.Cells[6].Value.ToString() + "" + row.Cells[7].Value.ToString() + "" + row.Cells[8].Value.ToString();
                    seguimiento = row.Cells[9].Value.ToString();
                    doctor = row.Cells[10].Value.ToString();

                }

            }
        }//Cambio de seleccion de row
        private void btnListaContinuar_Click(object sender, EventArgs e)
        {
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
        }//Verifica que se haya seleccionado uno no en blanco
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
                MessageBox.Show("Tres medicamentos es la cantidad maxima de medicamentos por consulta. En caso de requerir insertar mas medicamentos, ingrese otra consulta como seguimiento", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }//Validacion para mostrar CB de MEDICAMENTOS
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
                MessageBox.Show("Tres medicamentos es la cantidad maxima de medicamentos por consulta. En caso de requerir insertar mas medicamentos, ingrese otra consulta como seguimiento", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }//Validacion para mostrar CB de MEDICAMENTOS
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
                MessageBox.Show("Tres medicamentos es la cantidad maxima de medicamentos por consulta. En caso de requerir insertar mas medicamentos, ingrese otra consulta como seguimiento", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }//Validacion para mostrar CB de MEDICAMENTOS
        private void tbOtroNombre_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroNombre.Text, @"^[a-zA-Z0-9\s]+$") || tbOtroNombre.Text.Length < 1)
            {
            }
            else
            {
                tbOtroNombre.Text = tbOtroNombre.Text.Remove(tbOtroNombre.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void tbOtroRelacion_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroRelacion.Text, @"^[a-zA-Z0-9\s]+$") || tbOtroRelacion.Text.Length < 1)
            {
            }
            else
            {
                tbOtroRelacion.Text = tbOtroRelacion.Text.Remove(tbOtroRelacion.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void tbOtroEdad_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroEdad.Text, "^[0-9]+$") || tbOtroEdad.Text.Length < 1)
            {
            }
            else
            {
                tbOtroEdad.Text = tbOtroEdad.Text.Remove(tbOtroEdad.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void tbOtroMotivo_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbOtroMotivo.Text, @"^[a-zA-Z0-9\s]+$") || tbOtroMotivo.Text.Length < 1)
            {
            }
            else
            {
                tbOtroMotivo.Text = tbOtroMotivo.Text.Remove(tbOtroMotivo.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void tbAlumnoMotivo_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAlumnoMotivo.Text, @"^[a-zA-Z0-9\s]+$") || tbAlumnoMotivo.Text.Length < 1)
            {
            }
            else
            {
                tbAlumnoMotivo.Text = tbAlumnoMotivo.Text.Remove(tbAlumnoMotivo.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void tbDocenteMotivo_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbDocenteMotivo.Text, @"^[a-zA-Z0-9\s]+$") || tbDocenteMotivo.Text.Length < 1)
            {
            }
            else
            {
                tbDocenteMotivo.Text = tbDocenteMotivo.Text.Remove(tbDocenteMotivo.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void btnAdminMedDia_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tenga en cuenta que esta sección es únicamente y solamente para dar de baja medicamentos y diagnósticos en la base de datos.", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            pnlAdminMedDia.Show();
            pnlAltaAlumno.Hide();
            pnlAdminUsr.Hide();
            pnlAdminAltaDocente.Hide();
        }//Muestra PNL de ALTA-BAJA MEDICAMENTO
        private void btnAdminBorrarDia_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Seguro que desea eliminar el diagnostico " + cbAdminDia.GetItemText(cbAdminDia.SelectedItem) + " de manera permanente?", "Eliminar diagnostico?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string queryBorrarDia = "alter table consultas nocheck constraint all; delete diagnostico where nombre = '" + cbAdminDia.GetItemText(cbAdminDia.SelectedItem) + "'; alter table consultas check constraint all;";
                    SqlCommand comandoBorrarDia = new SqlCommand(queryBorrarDia, conn);
                    conn.Open();
                    comandoBorrarDia.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Baja realizada con exito.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LlenaCbDiagnostico();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se presento el siguiente error al realizar la baja en la base de datos: " + ex.Message + ". Asegurese de que el diagnostico no haya sido eliminado anteriormente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



        }//BTN ELIMINA DIAGNOSTICO seleccionado
        private void btnAdminBorrarMed_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Seguro que desea eliminar el medicamento " + cbAdminMed.GetItemText(cbAdminMed.SelectedItem) + " de manera permanente?", "Eliminar medicamento?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    string queryBorrarMed = "alter table consultas nocheck constraint all; delete medicamento where nombre = '" + cbAdminMed.GetItemText(cbAdminMed.SelectedItem) + "'; alter table consultas check constraint all;";
                    SqlCommand comandoBorrarMed = new SqlCommand(queryBorrarMed, conn);
                    conn.Open();
                    comandoBorrarMed.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Baja realizada con exito.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LlenaCbMedicamento();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se presento el siguiente error al realizar la baja en la base de datos: " + ex.Message + ". Asegurese de que el medicamento no haya sido eliminado anteriormente.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }



        }//BTN ELIMINA MEDICAMENTO seleccionado
        private void btnAdminAlumno_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tenga en cuenta que en esta sección solamente se agregan alumnos del ITH y de evento. Lea con cuidado las alertas a la hora de realizar cambios", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LlenaCbEvento();
            pnlAdminMedDia.Hide();
            pnlAltaAlumno.Show();
            pnlAdminUsr.Hide();
            pnlAdminAltaDocente.Hide();

        }//Muestra PNL de ALTA de ALUMNOS
        private void btnAltaAlumnosITH_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("¡Atención! Tenga en cuenta que solo es posible agregar todos los alumnos inscritos y no es posible agregar en partes. Asegúrese de que el Excel contenga todos los alumnos inscritos en el semestre ya que los alumnos anteriores serán SOBREESCRITOS con los que va a agregar a continuación. ¿Desea continuar?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    AltaAlumnoIth();
                    MessageBox.Show("Alumnos agregados exitosamente.", "Operación realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió el siguiente problema a la hora de agregar los alumnos: " + ex.Message + ". Asegúrese de que el archivo Excel tenga el formato especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//Exe funcion dar de ALTA ALUMNOS ITH
        private void btnAdminAltaAlumnoEvento_Click(object sender, EventArgs e)
        {
            string eventoSelected = "";
            try
            {
                DialogResult dialogResult = MessageBox.Show("¡Atención! A continuación, se agregarán los alumnos seleccionados a la base de datos y cuando termine el evento seleccionado serán removidos de la base de datos. Se le recomienda cerciorarse de que se haya seleccionado el evento correcto. ¿Desea continuar?", "Alerta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    #region MEGA-MEXICANADA


                    SqlCommand cmdLeeEvento = new SqlCommand("select num_evento from evento where nombre = '" + cbAdminAlumnoEvento.GetItemText(cbAdminAlumnoEvento.SelectedItem) + "';", conn);
                    conn.Open();
                    SqlDataReader leerEvento = cmdLeeEvento.ExecuteReader();
                    if (leerEvento.Read())
                    {
                        eventoSelected = leerEvento["num_evento"].ToString();
                    }

                    conn.Close();


                    SqlCommand cmdAddMexicanada = new SqlCommand("ALTER TABLE alumno ADD CONSTRAINT DF_Alumno_Evento DEFAULT " + eventoSelected + " FOR evento;", conn);
                    conn.Open();
                    cmdAddMexicanada.ExecuteNonQuery();
                    conn.Close();
                    #endregion
                    AltaAlumnoEvento();
                    #region FIN MEGA-MEXICANADA


                    SqlCommand cmdEliminaMexicanada = new SqlCommand("alter table alumno drop constraint df_Alumno_Evento;", conn);
                    conn.Open();
                    cmdEliminaMexicanada.ExecuteNonQuery();
                    conn.Close();
                    #endregion
                    MessageBox.Show("Alumnos de evento agregados exitosamente", "Operación realizada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió el siguiente problema a la hora de agregar los alumnos: " + ex.Message + ". Asegúrese de que el archivo Excel tenga el formato especificado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }//Exe funcion dar de ALTA ALUMNOS EVENTO
        private void btnAdminUsuarios_Click(object sender, EventArgs e)
        {
            MessageBox.Show("En esta sección solamente se puede administrar los usuarios utilizados para usar el software RSM", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LlenaCbEvento();
            pnlAdminMedDia.Hide();
            pnlAltaAlumno.Hide();
            pnlAdminAltaDocente.Hide();
            pnlAdminUsr.Show();
            LlenaCbUsuario();
            LlenaCbUsuario2();
        }//Muestra panel de ADMINISTRACION
        private void tbAdminCambioPsw_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAdminCambioPsw.Text, "^[a-zA-Z0-9]+$") || tbAdminCambioPsw.Text.Length < 1)
            {
            }
            else
            {
                tbAdminCambioPsw.Text = tbAdminCambioPsw.Text.Remove(tbAdminCambioPsw.Text.Length - 1);
            }
            if (tbAdminCambioPsw.Text.Length > 0)
            {
                tbAdminCambioConfirmaPsw.Enabled = true;
            }
            else if (tbAdminCambioPsw.Text.Length == 0)
            {
                tbAdminCambioConfirmaPsw.Text = "";
                tbAdminCambioConfirmaPsw.Enabled = false;
            }
        }//Multiple VALIDACION TB CAMBIO PSW
        private void tbAdminCambioConfirmaPsw_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAdminCambioConfirmaPsw.Text, "^[a-zA-Z0-9]+$") || tbAdminCambioConfirmaPsw.Text.Length < 1)
            {
            }
            else
            {
                tbAdminCambioConfirmaPsw.Text = tbAdminCambioConfirmaPsw.Text.Remove(tbAdminCambioConfirmaPsw.Text.Length - 1);
            }
            if (tbAdminCambioConfirmaPsw.Text.Length > 0)
            {
                btnAdminCambiaPsw.Show();
            }
            else if (tbAdminCambioConfirmaPsw.Text.Length == 0)
            {
                btnAdminCambiaPsw.Hide();
            }
        }//Multiple VALIDACION TB CAMBIO PSW
        private void tbAdminConfirmBaja_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAdminConfirmBaja.Text, "^[a-zA-Z]+$") || tbAdminConfirmBaja.Text.Length < 1)
            {
                tbAdminConfirmBaja.Text.ToUpper();
            }
            else
            {
                tbAdminConfirmBaja.Text = tbAdminConfirmBaja.Text.Remove(tbAdminConfirmBaja.Text.Length - 1);
            }
            if (tbAdminConfirmBaja.Text == "BAJA")
            {
                btnAdminUsrBaja.Show();
            }
            else
            {
                btnAdminUsrBaja.Hide();
            }
        }//Multiple VALIDACION TB BAJA USR
        private void tbAdminAltaUsr_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAdminAltaUsr.Text, "^[a-zA-Z0-9]+$") || tbAdminAltaUsr.Text.Length < 1)
            {
            }
            else
            {
                tbAdminAltaUsr.Text = tbAdminAltaUsr.Text.Remove(tbAdminAltaUsr.Text.Length - 1);
            }
            if (tbAdminAltaUsr.Text.Length > 0)
            {
                tbAdminAltaPsw.Enabled = true;
            }
            else
            {
                tbAdminAltaPsw.Enabled = false;
            }
        }//Multiple VALIDACION TB ALTA USR
        private void tbAdminAltaPsw_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAdminAltaPsw.Text, "^[a-zA-Z0-9]+$") || tbAdminAltaPsw.Text.Length < 1)
            {
            }
            else
            {
                tbAdminAltaPsw.Text = tbAdminAltaPsw.Text.Remove(tbAdminAltaPsw.Text.Length - 1);
            }
            if (tbAdminAltaPsw.Text.Length > 0)
            {
                tbAdminAltaConfirmPsw.Enabled = true;
            }
            else
            {
                tbAdminAltaConfirmPsw.Text = "";
                tbAdminAltaConfirmPsw.Enabled = false;

            }
        }//Multiple VALIDACION TB ALTA USR
        private void tbAdminAltaConfirmPsw_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(tbAdminAltaConfirmPsw.Text, "^[a-zA-Z0-9]+$") || tbAdminAltaConfirmPsw.Text.Length < 1)
            {
            }
            else
            {
                tbAdminAltaConfirmPsw.Text = tbAdminAltaConfirmPsw.Text.Remove(tbAdminAltaConfirmPsw.Text.Length - 1);
            }
        }// Validacion SQL-INJECTION
        private void btnConsultoria_Click(object sender, EventArgs e)
        {
            Separator.Location = new Point(15, 364);
            Separator.Show();
            pnlBusqueda.Hide();
            if (Usuario == "dse")
            {

                cbAlumnoBusqueda.Checked = false;
                cbDocenteBusqueda.Checked = false;
                cbOtroBusqueda.Checked = false;
                Separator.Location = new Point(15, 364);
                Separator.Show();
                pnlBusqueda.Hide();
                pnlCertificado.Hide();
                pnlConsultoria.Show();
                pnlAdministracion.Hide();
                pnlServiciosEscolares.Hide();
                btnConsultoria.Text = "Consultoría General";
                bunifuDragControl1.TargetControl = pnlServiciosEscolares;
                pnlEvento.Hide();
                pnlAgregarEvento.Hide();
            }
            else
            {
                cbOtroBusqueda.Checked = false;
                cbAlumnoBusqueda.Checked = false;
                cbDocenteBusqueda.Checked = false;
                btnConsultoria.Text = "Consultoría";
                pnlConsultoria.Show();
                pnlServiciosEscolares.Hide();
                pnlCertificado.Hide();
                bunifuDragControl1.TargetControl = pnlConsultoria;
                pnlEvento.Hide();
                pnlAgregarEvento.Hide();
            }
        }//Muestra PNL CONSULTAS AVANZADAS
        private void btnEvento_Click(object sender, EventArgs e)
        {
            Separator.Location = new Point(15, 454);
            Separator.Show();
            pnlConsulta.Hide();
            pnlCertificado.Hide();
            pnlAdministracion.Hide();
            pnlConsultoria.Hide();
            pnlServiciosEscolares.Hide();
            pnlEvento.Show();
            pnlAgregarEvento.Show();
        }        //Muestra PNL EVENTO
        private void txbBusquedaClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && !char.IsSeparator(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }//Validacion SQL-INJECTION
        private void cbAlumnoBusqueda_OnChange(object sender, EventArgs e)
        {
            calCalendario.Hide();
            cbAlumnoBusqueda.Checked = true;
            cbDocenteBusqueda.Checked = false;
            cbOtroBusqueda.Checked = false;
            cbFecha.Checked = false;
            cbNoControl.Checked = false;
            cbSeguir.Checked = false;
            cbNoControl.Show();
            pnlBusqueda.Show();
            lblControl.Text = "No Control";
            lblControl.Hide();
            lblNoControl.Text = "No Control";
            lblNoControl.Show();
            lblAlumno.Show();
            tabResultados.Hide();
            txbBusquedaClave.Clear();
            txbBusquedaClave.Hide();
        }//Cambios en PNL EVENTO al cambiar ALUMNO
        private void cbDocenteBusqueda_OnChange(object sender, EventArgs e)
        {
            calCalendario.Hide();
            cbDocenteBusqueda.Checked = true;
            cbAlumnoBusqueda.Checked = false;
            cbOtroBusqueda.Checked = false;
            cbFecha.Checked = false;
            cbNoControl.Checked = false;
            cbSeguir.Checked = false;
            cbNoControl.Show();
            lblControl.Text = "No Docente";
            lblControl.Hide();
            lblNoControl.Text = "No Docente";
            lblAlumno.Show();
            lblNoControl.Show();
            pnlBusqueda.Show();
            tabResultados.Hide();
            txbBusquedaClave.Clear();
            txbBusquedaClave.Hide();

        }//Cambios en PNL EVENTO al cambiar DOCENTE
        private void cbOtroBusqueda_OnChange(object sender, EventArgs e)
        {
            calCalendario.Hide();
            cbOtroBusqueda.Checked = true;
            cbAlumnoBusqueda.Checked = false;
            cbDocenteBusqueda.Checked = false;
            cbFecha.Checked = false;
            cbNoControl.Checked = false;
            cbSeguir.Checked = false;
            //cbNoControl.Hide();
            lblNoControl.Show();
            lblNoControl.Text = "Nombre";
            lblControl.Hide();
            lblControl.Text = "Nombre";
            pnlBusqueda.Show();
            tabResultados.Hide();
            txbBusquedaClave.Clear();
            txbBusquedaClave.Hide();

        }//Cambios en PNL EVENTO al cambiar OTRO
        private void cbFecha_OnChange(object sender, EventArgs e)
        {
            calCalendario.Show();
            calCalendario.SelectionStart = DateTime.Now;
            calCalendario.BackColor = Color.DarkGray;
            cbFecha.Checked = true;
            cbNoControl.Checked = false;
            lblControl.Show();
            lblControl.Location = new Point(143, 83);
            lblControl.Text = "Seleccione fecha de consulta";
            tabResultados.Hide();
            txbBusquedaClave.Hide();
        }//Cambios en PNL EVENTO al cambiar FECHA
        private void cbNoControl_OnChange(object sender, EventArgs e)
        {
            calCalendario.Hide();
            cbFecha.Checked = false;
            cbNoControl.Checked = true;
            lblControl.Show();
            lblControl.Location = new Point(117, 137);
            if (cbAlumnoBusqueda.Checked)
            {
                lblControl.Text = "No Control";
            }
            else if (cbDocenteBusqueda.Checked)
            {
                lblControl.Text = "No Docente";
            }
            else if (cbOtroBusqueda.Checked)
            {
                lblControl.Text = "Nombre";
            }
            tabResultados.Hide();
            txbBusquedaClave.Show();
        }//Cambios en PNL EVENTO al cambiar el NO.CONTROL
        private void cbSeguir_OnChange(object sender, EventArgs e)
        {
            tabResultados.Hide();
            if (cbFecha.Checked)
            {
                calCalendario.Show();
            }
            if (cbNoControl.Checked)
            {
                lblControl.Show();
                txbBusquedaClave.Show();
            }

        }//Cambios en PNL EVENTO
        private void cbSEAlumno_OnChange(object sender, EventArgs e)
        {
            ShowControlesSE();
            comboCarrera.Items.Clear();
            comboCarrera.Items.Insert(0, "Seleccione");
            LlenarComboBoxServEsc(comboCarrera, "select distinct carrera from alumno", "carrera");//Llena ComboBox Carrera
            cbFalse();
            cbSEAlumno.Checked = true;
            cbSEDocente.Checked = false;
            cbSEOtro.Checked = false;
            cbSELapso.Location = new Point(35, 77);
            dtpInicio.Location = new Point(43, 223);
            dtpFinal.Location = new Point(43, 270);
            lblSECarreraArea.Text = "Carrera";
            lblSECarrArMini.Text = "Carrera";
            lblSELapso.Location = new Point(66, 77);
            lblSEFechaInicio.Location = new Point(81, 207);
            lblSEFechaFinal.Location = new Point(81, 254);


            pnlSE.Show();
        }//Movimientos PNL Servicios Escolares por ALUMNO
        private void cbSEDocente_OnChange(object sender, EventArgs e)
        {
            ShowControlesSE();
            comboCarrera.Items.Clear();
            comboCarrera.Items.Insert(0, "Seleccione");
            LlenarComboBoxServEsc(comboCarrera, "select distinct departamento from docente", "departamento");//Llena ComboBox Carrera
            cbFalse();
            cbEvento.Hide();
            cbSEAlumno.Checked = false;
            cbSEDocente.Checked = true;
            cbSEOtro.Checked = false;
            cbSEGeneracion.Hide();
            cbSELapso.Location = cbSEGeneracion.Location;
            comboGeneracion.Hide();
            comboEvento.Hide();
            dtpInicio.Location = new Point(43, 170);
            dtpFinal.Location = new Point(43, 217);
            lblSEEvento.Hide();
            lblSEEventoMini.Hide();
            lblSECarreraArea.Text = "Área";
            lblSECarrArMini.Text = "Área";
            lblSEGene.Hide();
            lblSEGeneMini.Hide();
            lblSELapso.Location = lblSEGene.Location;
            lblSEFechaInicio.Location = new Point(81, 154);
            lblSEFechaFinal.Location = new Point(81, 201);
            pnlSE.Show();
        }//Movimientos PNL Servicios Escolares por DOCENTE
        private void cbSEOtro_OnChange(object sender, EventArgs e)
        {
            ShowControlesSE();
            cbFalse();
            cbSEAlumno.Checked = false;
            cbSEDocente.Checked = false;
            cbSEOtro.Checked = true;
            cbSECarrera.Hide();
            cbSEGeneracion.Hide();
            cbSELapso.Show();
            comboCarrera.Hide();
            comboGeneracion.Hide();
            dtpInicio.Location = new Point(43, 126);
            dtpInicio.Enabled = false;
            dtpFinal.Location = new Point(43, 173);
            dtpFinal.Enabled = false;
            lblSECarreraArea.Hide();
            lblSEGene.Hide();
            cbSELapso.Location = new Point(60, 23);
            lblSELapso.Location = new Point(81, 23);
            lblSECarrArMini.Hide();
            lblSEGeneMini.Hide();
            lblSEFechaInicio.Location = new Point(81, 110);
            lblSEFechaFinal.Location = new Point(81, 157);
            pnlSE.Show();
        }//Movimientos PNL Servicios Escolares por OTRO
        private void cbEvento_OnChange(object sender, EventArgs e)
        {
            if (cbEvento.Checked)
            {
                if (cbSEDocente.Checked)
                {
                    MessageBox.Show(this, "Para buscar a un docente de evento, seleccione el apartado de 'Otro'", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cbEvento.Checked = false;
                }
                else
                {
                    comboEvento.Enabled = true;
                }
            }
            else
            {
                comboEvento.Enabled = false;
            }
        }//Movimientos PNL Servicios Escolares por ALUMNO
        private void btnAdminUsrBaja_Click(object sender, EventArgs e)
        {
            if (tbAdminConfirmBaja.Text == "BAJA")
            {
                DialogResult dialogResult = MessageBox.Show("Seguro que desea cambiar la contraseña del usuario " + cbAdminCambioPswUsr.GetItemText(cbAdminCambioPswUsr.SelectedItem) + "?", "Cambiar contraseña?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        string queryBorraUsr = "delete usuario where usuario = '" + cbAdminBajaUsr.GetItemText(cbAdminBajaUsr.SelectedItem) + "';";
                        SqlCommand cmdBorraUsr = new SqlCommand(queryBorraUsr, conn);
                        conn.Open();
                        cmdBorraUsr.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Cambio realizado con exito.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LlenaCbUsuario();
                        LlenaCbUsuario2();
                        tbAdminConfirmBaja.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Se presento el siguiente error al realizar el cambio en la base de datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
                else
                {
                    tbAdminConfirmBaja.Text = "";
                }

            }
        } //BORRA USUARIO DE LA BD
        private void btnAdminCambiaPsw_Click(object sender, EventArgs e)
        {
            if (tbAdminCambioPsw.Text == tbAdminCambioConfirmaPsw.Text)
            {
                DialogResult dialogResult = MessageBox.Show("Seguro que desea cambiar la contraseña del usuario " + cbAdminCambioPswUsr.GetItemText(cbAdminCambioPswUsr.SelectedItem) + "?", "Cambiar contraseña?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        string queryCambiaPsw = "update usuario set contraseña = '" + tbAdminCambioPsw.Text + "' where usuario = '" + cbAdminCambioPswUsr.GetItemText(cbAdminCambioPswUsr.SelectedItem) + "';";
                        SqlCommand cmdCambiaPsw = new SqlCommand(queryCambiaPsw, conn);
                        conn.Open();
                        cmdCambiaPsw.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Cambio realizado con exito.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LlenaCbUsuario();
                        LlenaCbUsuario2();
                        tbAdminCambioConfirmaPsw.Text = "";
                        tbAdminCambioPsw.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Se presento el siguiente error al realizar el cambio en la base de datos: " + ex.Message + ". Asegurese de que los datos sean correctos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    tbAdminCambioConfirmaPsw.Text = "";
                    tbAdminCambioPsw.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Contraseñas no coinciden", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //CAMBIA PSW DEL USUARIO
        private void btnAdminAltaUsr_Click(object sender, EventArgs e)
        {
            if (tbAdminAltaPsw.Text == tbAdminAltaConfirmPsw.Text)
            {
                DialogResult dialogResult = MessageBox.Show("Seguro que desea agregar a la base de datos el usuario " + tbAdminAltaUsr.Text + "?", "Agregar usuario?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try
                    {
                        string queryAltaUsr = "insert into usuario (usuario, contraseña, nivel) values ('" + tbAdminAltaUsr.Text + "', '" + tbAdminAltaPsw.Text + "', 0);";
                        SqlCommand cmdAltaUsr = new SqlCommand(queryAltaUsr, conn);
                        conn.Open();
                        cmdAltaUsr.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("Cambio realizado con exito.", "Listo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LlenaCbUsuario();
                        LlenaCbUsuario2();
                        tbAdminAltaConfirmPsw.Text = "";
                        tbAdminAltaPsw.Text = "";
                        tbAdminAltaUsr.Text = "";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Se presento el siguiente error al realizar el cambio en la base de datos: " + ex.Message + ". Asegurese de que los datos sean correctos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    tbAdminAltaConfirmPsw.Text = "";
                    tbAdminAltaPsw.Text = "";
                    tbAdminAltaUsr.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Contraseñas no coinciden", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } //AGREGA USUARIO A LA BD
        private void cbSECarrera_OnChange(object sender, EventArgs e)
        {
            if (cbSECarrera.Checked)
            {
                comboCarrera.Enabled = true;
            }
            else
            {
                comboCarrera.Enabled = false;

            }
        }//Cambio de Bandera
        private void cbSEGeneracion_OnChange(object sender, EventArgs e)
        {
            if (cbSEGeneracion.Checked)
            {
                comboGeneracion.Enabled = true;
            }
            else
            {
                comboGeneracion.Enabled = false;

            }
        }//Cambio de Bandera
        private void cbSELapso_OnChange(object sender, EventArgs e)
        {
            if (cbSELapso.Checked)
            {
                dtpInicio.Enabled = true;
                dtpFinal.Enabled = true;
            }
            else
            {
                dtpInicio.Enabled = false;
                dtpFinal.Enabled = false;
            }
        }//Cambio de Banderas
        private void botSEBuscar_Click(object sender, EventArgs e)
        {
            if (!cbSECarrera.Checked && !cbSELapso.Checked && !cbSEGeneracion.Checked && !cbEvento.Checked && !cbSEOtro.Checked)
            {

                MessageBox.Show(this, "Seleccione un parámetro de búsqueda", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                BuscaConsulta(1);
            }
        }//Validacion de busqueda vacia
        private void botBack_Click(object sender, EventArgs e)
        {

            if (!tabSEResultados.Visible)
            {
                MessageBox.Show(this, "Debe realizar una búsqueda primero", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                tabSEResultados.Hide();
                btnExportar.Hide();
                ShowControlesSE();
                if (cbSEDocente.Checked)
                {
                    ShowControlesSE();
                    cbSEGeneracion.Hide();
                    cbEvento.Hide();
                    comboEvento.Hide();
                    comboGeneracion.Hide();
                    lblSEEvento.Hide();
                    lblSEEventoMini.Hide();
                    lblSEGene.Hide();
                    lblSEGeneMini.Hide();
                }
                else if (cbSEOtro.Checked)
                {
                    cbSECarrera.Hide();
                    cbSEGeneracion.Hide();
                    cbSELapso.Hide();
                    comboCarrera.Hide();
                    comboGeneracion.Hide();
                    lblSECarreraArea.Hide();
                    lblSEGene.Hide();
                    lblSECarrArMini.Hide();
                    lblSEGeneMini.Hide();
                }
            }
        }//Regresa usuario al estado anterior
        private void txbBusquedaClave_OnTextChange(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txbBusquedaClave.Text, "^[a-zA-Z]+$") || txbBusquedaClave.Text.Length < 1)
            {
            }
            else if (txbBusquedaClave.Text == " ")
            {
                txbBusquedaClave.Text = txbBusquedaClave.Text.Remove(txbBusquedaClave.Text.Length - 1);
            }
        }//Validacion SQL-INJECTION
        private void botBusquedaAlumno_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!cbOtroBusqueda.Checked && !cbAlumnoBusqueda.Checked && !cbDocenteBusqueda.Checked) || (!cbNoControl.Checked && !cbFecha.Checked))
                {
                    MessageBox.Show(this, "Ingrese los datos correctamente", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    lblControl.Hide();
                    txbBusquedaClave.Hide();
                    calCalendario.Hide();
                    BuscaConsulta(2);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrio el siguiente problema: " + ex.Message + "Contecte al administrador." + "\t" + ex.GetType());
                conn.Close();
            }
        }//Boton de búsqueda 
        #endregion

        #region Controles Simples
        private void btnAtras_Click(object sender, EventArgs e) { pnlListaCerti.Hide(); }
        private void ddbDocenteMedicamento2_Click(object sender, EventArgs e) { LlenaCbMedicamento2(); }
        private void btnDocenteBuscar_Click(object sender, EventArgs e) { BuscaPaciente(); }
        private void btnAlumnoBuscar_Click(object sender, EventArgs e) { BuscaPaciente(); }
        private void btnOtro_OtroDiagnostico_Click(object sender, EventArgs e) { AbreDiagnostico(); }
        private void btnDocente_OtroDiagnostico_Click(object sender, EventArgs e) { AbreDiagnostico(); }
        private void btnAlumno_OtroDiagnostico_Click(object sender, EventArgs e) { AbreDiagnostico(); }
        private void btnOtro_OtroMedicamento_Click(object sender, EventArgs e) { AbreMedicamento(); }
        private void btnDocente_OtroMedicamento_Click(object sender, EventArgs e) { AbreMedicamento(); }
        private void btnAlumno_OtroMedicamento_Click(object sender, EventArgs e) { AbreMedicamento(); }
        private void cbOtroDiagnostico_Click(object sender, EventArgs e) { LlenaCbDiagnostico(); }
        private void cbOtroMedicamento_Click(object sender, EventArgs e) { LlenaCbMedicamento(); }
        private void ddbAlumnoDiagnostico_Click(object sender, EventArgs e) { LlenaCbDiagnostico(); }
        private void ddbAlumnoMedicamento_Click(object sender, EventArgs e) { LlenaCbMedicamento(); }
        private void ddbDocenteDiagnostico_Click(object sender, EventArgs e) { LlenaCbDiagnostico(); }
        private void ddbDocenteMedicamento_Click(object sender, EventArgs e) { LlenaCbMedicamento(); }
        private void ddbAlumnoMedicamento2_Click(object sender, EventArgs e) { LlenaCbMedicamento2(); }
        private void ddbAlumnoMedicamento3_Click(object sender, EventArgs e) { LlenaCbMedicamento3(); }
        private void ddbDocenteMedicamento3_Click(object sender, EventArgs e) { LlenaCbMedicamento3(); }
        private void cbOtroMedicamento2_Click(object sender, EventArgs e) { LlenaCbMedicamento2(); }
        private void cbOtroMedicamento3_Click(object sender, EventArgs e) { LlenaCbMedicamento3(); }
        private void comboBox1_Click(object sender, EventArgs e) { LlenaCbMedicamento(); }
        private void btnImportar_Click(object sender, EventArgs e)
        {
            if (Usuario == "dse")
            {
                ImportarExcel(tabSEResultados);
            }
            else
            {
                ImportarExcel(tabResultados);
            }
        }
        private void btnAgregarEvento_Click(object sender, EventArgs e)
        {
            string fechi = calEventoIni.SelectionStart.ToString("yyyy-MM-dd h:mm tt");
            string fechf = calEventoFin.SelectionStart.ToString("yyyy-MM-dd h:mm tt");
            try
            {
                conn.Open();
                SqlCommand comandoEvento = new SqlCommand("insert into evento (nombre, fecha_inicio, fecha_fin) values( '" + tbEvento.Text + "' ,  '" + fechi + "', '" + fechf + "');", conn);
                comandoEvento.ExecuteNonQuery();
                MessageBox.Show("El evento ha sido creado exitosamente.", "Creado", MessageBoxButtons.OK);
                conn.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Favor de ingresar todos los campos", "Alerta", MessageBoxButtons.OK);
            }
        }
        private void pnlEvento_Click(object sender, EventArgs e) { calEventoFin.Hide(); calEventoIni.Hide(); }
        private void btnAdminExeAltaDocente_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Seguro que desea cambiar los docentes en la base de datos? (Debe tener el archivo excel listo) ", "Agregar docente", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dialogResult == DialogResult.Yes)
            {
                try
                {
                    AltaDocente();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Se presento el siguiente error al realizar el cambio en la base de datos: " + ex.Message + ". Asegurese de que el formato del archivo excel sea el especificado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdminAltaDocente_Click(object sender, EventArgs e)
        {

        }

        private void btnAdminAltaDocente_Click_1(object sender, EventArgs e)
        {
            pnlAdminMedDia.Hide();
            pnlAltaAlumno.Hide();
            pnlAdminUsr.Hide();
            pnlAdminAltaDocente.Show();
        }

        private void cbAdminDia_Click(object sender, EventArgs e) { LlenaCbDiagnostico(); }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { btnAdminAltaAlumnoEvento.Enabled = true; }
        private void cbAdminAlumnoEvento_Click(object sender, EventArgs e) { LlenaCbEvento(); }
        private void cbAdminCambioPswUsr_Click(object sender, EventArgs e) { LlenaCbUsuario(); }
        private void cbAdminBajaUsr_Click(object sender, EventArgs e) { LlenaCbUsuario2(); }
        private void tbFechaIniEvento_MouseClick(object sender, MouseEventArgs e) { calEventoIni.Show(); }
        private void calEventoIni_DateChanged(object sender, DateRangeEventArgs e) { tbFechaIniEvento.Text = calEventoIni.SelectionStart.ToString(); }
        private void calEventoIni_Leave(object sender, EventArgs e) { calEventoIni.Hide(); }
        private void tbFechaFinEvento_MouseClick(object sender, MouseEventArgs e) { calEventoFin.Show(); }
        private void calEventoFin_Leave(object sender, EventArgs e) { calEventoFin.Hide(); }
        private void calEventoFin_DateChanged(object sender, DateRangeEventArgs e) { tbFechaFinEvento.Text = calEventoFin.SelectionStart.ToString(); }
        private void pnlAgregarEvento_Click(object sender, EventArgs e) { calEventoFin.Hide(); calEventoIni.Hide(); }
        #endregion
    }
}
