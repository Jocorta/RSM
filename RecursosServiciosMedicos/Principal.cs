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
    public partial class Principal : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private SqlDataAdapter dataAdapter = new SqlDataAdapter();
        private string input = "";
        DiagnosticoForm diagnosticoFormObjeto = new DiagnosticoForm();
        MedicamentoForm medicamentoFormObjeto = new MedicamentoForm();

        public string nombre="",num_id = "", num_control = "", num_docente = "", seguimiento = "", fecha = "", medicamento = "", diagnostico = "", num_otro = "", edad = "", sexo = "";
        public bool RegistroSeleccionado = false,bandera1,banderaalumno;
        public int tipo= 0;

        public Principal()
        {
            InitializeComponent();
        }
        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-48PLDOP;initial catalog=RSM;integrated security=true");//conexion base de datos

        public Principal(string LoggedUser)
        {
            InitializeComponent();
            lblFecha.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lblUsuario.Text = "Usuario: " + LoggedUser;
            pnlCertificado.Hide();
 
            
        }

        private void Principal_Load(object sender, EventArgs e)
        {

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
            pnlConsulta.Show();
            pnlCertificado.Hide();
            LimpiaAlumno();
            LimpiaDocente();
            LimpiaOtro();

        }

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

        private void lblFecha_Click(object sender, EventArgs e)
        {

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
                pnlAlumno.Show();
                pnlDocente.Hide();
                pnlOtro.Hide();
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
                pnlAlumno.Hide();
                pnlDocente.Show();
                pnlOtro.Hide();
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
                pnlAlumno.Hide();
                pnlDocente.Hide();
                pnlOtro.Show();
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

        private void pnlConsulta_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDocenteBuscar_Click(object sender, EventArgs e)
        {
            //Lectura de base de datos
            input = tbDocenteNoDocente.Text;
            string cadQuery = "Select * from docente where num_docente ='" + tbDocenteNoDocente.Text + "' ";
            
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
                
                tbDocenteNombre.Text = leer["nombre"].ToString();
                tbDocenteArea.Text = leer["area"].ToString();
                // Convierte fecha de nacimiento a edad:
                fechNac = leer["fecha_nacimiento"].ToString();
                fecNac = Convert.ToDateTime(fechNac);
                var age = today.Year - fecNac.Year;
                if (fecNac > today.AddYears(-age)) age--;
                tbDocenteEdad.Text = Convert.ToString(age);
                // --------------------------------------------
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
                tbDocenteNoDocente.Text = input;
            }
            else
            {
                tbDocenteNombre.Text = "";
                tbDocenteArea.Text = "";
                tbAlumnoEdad.Text = "";
                tbAlumnoSexo.Text = "";
            }
            conn.Close();
            tbDocenteNoDocente.Text = "";
        }

        private void btnAlumnoBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }


        private string DeterminaSemestre(int numControl)
        {
            string Añostring;
            int Añoescolar;
            //                                                                                                                                                                                                                                            Perdon a la persona que tenga que arreglar esto, pero lo mas seguro es que ya estemos muertos :)
            Añostring = "20"+numControl.ToString().Substring(0, 2);
            Añoescolar = Convert.ToInt32(DateTime.Now.Year.ToString()) - Convert.ToInt32(Añostring);
            Añoescolar = Añoescolar * 2;
            if (DateTime.Now.Month >= 8)
            {
                //Semestre impar
                Añoescolar++;
            }
            return (Añoescolar.ToString()); 

        }

        private void btnAlumnoBuscar_Click(object sender, EventArgs e)
        {
            // Lectura base de datos
            input = tbDocenteNoDocente.Text;
            string cadQuery = "Select * from alumno where num_control ='" + tbAlumnoNoControl.Text + "' ";
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
            }
            conn.Close();
            tbAlumnoNoControl.Text = input;
        }

        private void tbAlumnoNoControl_KeyPress(object sender, KeyPressEventArgs e)
        {

            
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

        private void bunifuCustomLabel29_Click(object sender, EventArgs e)
        {

        }

        private void btnReceta_Click(object sender, EventArgs e)
        {

        }

        private void btnCertificadoMedic_Click(object sender, EventArgs e)
        {
          

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
        
        }

        private void btnReceta_Click_1(object sender, EventArgs e)
        {
       
        }

        private void btnBack1_Click(object sender, EventArgs e)
        {
           

        }

        private void pnlCertificado_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlImpCerti_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvListaCerti_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void chbReceta_CheckedChanged(object sender, EventArgs e)
        {
       


            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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

        private void cbTipoDct_SelectedValueChanged(object sender, EventArgs e)
        {

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
            if (tipo == 1)
            {
                //tipo certificado
                if (chbOtro.Checked == false)
                {
                    //si es de dentro del plantel
                    var application = new Microsoft.Office.Interop.Word.Application();
                    var document = new Microsoft.Office.Interop.Word.Document();
                    string cadQuery;
                    string path;
                    if (banderaalumno == true)
                    {
                        //si es alumno
                        path = @"C:\Users\jc-mt\Desktop\Documentos\Certificados\Alumnos\Certificado Medico";
                        document = application.Documents.Add(Template:path+".docx");
                        cadQuery = "Select * from alumno where num_control ='" + num_id + "' ";
                    }
                    else
                    {
                        //si es docente
                        path = @"C:\Users\jc-mt\Desktop\Documentos\Certificados\Docentes\Certificado Medico";
                        document = application.Documents.Add(Template: path+ ".docx");
                        cadQuery = "Select * from docente where num_docente ='" + num_id + "' ";
                    }

                    SqlCommand comando = new SqlCommand(cadQuery, conn);
                    conn.Open();

                    SqlDataReader leer3 = comando.ExecuteReader();
                    if (leer3.Read() == true)
                    {
                        foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
                        {
                            if (field.Code.Text.Contains("Nombre"))
                            {
                                // if (leer.Read() == true)
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

                                application.Selection.TypeText(nombre);
                            }
                            else if (field.Code.Text.Contains("Edad"))
                            {
                                field.Select();
                                application.Selection.TypeText(edad);

                            }
                            else if (field.Code.Text.Contains("Fecha"))
                            {
                                field.Select();
                                application.Selection.TypeText(fecha);
                            }

                            

                        }
                    }
                    conn.Close();
                    document.SaveAs(path+"-"+ nombre + ".docx");

                    PrintDialog pDialog = new PrintDialog();
                    if (pDialog.ShowDialog() == DialogResult.OK)
                    {
                        document = application.Documents.Add(path + "-" + nombre + ".docx");
                        application.ActivePrinter = pDialog.PrinterSettings.PrinterName;
                        application.ActiveDocument.PrintOut(); //this will also work: doc.PrintOut();
                        document.Close(SaveChanges: false);
                        document = null;
                        application.Quit();
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
                        application.Quit();
                        tbCodigoCerti.Text = "";
                        chbOtro.Checked = false;
                        cbTipoDct.Enabled = false;
                        cbTipoDct.SelectedIndex = -1;
                        tbCodigoCerti.Enabled = true;
                        chbOtro.Enabled = true;
                        btnImprimir.Enabled = false;
                    }
                }
                else
                {
                    //fuera del plantel
                    var application = new Microsoft.Office.Interop.Word.Application();
                    var document = new Microsoft.Office.Interop.Word.Document();
                    string path = @"C:\Users\jc-mt\Desktop\Documentos\Certificados\Otros\Certificado Medico";//cambiar a una carpeta de otros
                    document = application.Documents.Add(Template: path+".docx");
                    foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
                    {
                        if (field.Code.Text.Contains("Nombre"))
                        {
                            field.Select();
                            application.Selection.TypeText(nombre);

                        }
                        else if (field.Code.Text.Contains("Edad"))
                        {
                            field.Select();
                            application.Selection.TypeText(edad);

                        }
                        else if (field.Code.Text.Contains("Fecha"))
                        {
                            field.Select();
                            application.Selection.TypeText(fecha);
                        }
                    }

                    conn.Close();
                    document.SaveAs(path + "-" + nombre + ".docx");

                    PrintDialog pDialog = new PrintDialog();
                    if (pDialog.ShowDialog() == DialogResult.OK)
                    {
                        document = application.Documents.Add(path + "-" + nombre + ".docx");
                        application.ActivePrinter = pDialog.PrinterSettings.PrinterName;
                        application.ActiveDocument.PrintOut(); //this will also work: doc.PrintOut();
                        document.Close(SaveChanges: false);
                        document = null;
                        application.Quit();
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
                        application.Quit();
                        tbCodigoCerti.Text = "";
                        chbOtro.Checked = false;
                        cbTipoDct.Enabled = false;
                        cbTipoDct.SelectedIndex = -1;
                        tbCodigoCerti.Enabled = true;
                        chbOtro.Enabled = true;
                        btnImprimir.Enabled = false;
                    }
                }
            } //tipo certificado
            else
            {
                //tipo receta
                //tipo certificado
                if (chbOtro.Checked == false)
                {
                    //si es de dentro del plantel
                    var application = new Microsoft.Office.Interop.Word.Application();
                    var document = new Microsoft.Office.Interop.Word.Document();

                    string cadQuery;
                    string path;
                    if (banderaalumno == true)
                    {
                        //si es alumno
                        path = @"C:\Users\jc-mt\Desktop\Documentos\Recetas\Alumnos\Receta";
                        document = application.Documents.Add(Template: path + ".docx");//cambiar a una carpeta de alumnos
                        cadQuery = "Select * from alumno where num_control ='" + num_id + "' ";
                    }
                    else
                    {
                        //si es docente
                        path = @"C:\Users\jc-mt\Desktop\Documentos\Recetas\Docentes\Receta";
                        document = application.Documents.Add(Template: path + ".docx");//cambiar a una carpeta docente
                        cadQuery = "Select * from docente where num_docente ='" + num_id + "' ";
                    }

                    SqlCommand comando = new SqlCommand(cadQuery, conn);
                    conn.Open();

                    SqlDataReader leer3 = comando.ExecuteReader();
                    if (leer3.Read() == true)
                    {
                        foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
                        {
                            if (field.Code.Text.Contains("Nombre"))
                            {
                                // if (leer.Read() == true)
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

                                application.Selection.TypeText(nombre);
                            }
                            else if (field.Code.Text.Contains("Edad"))
                            {
                                field.Select();
                                application.Selection.TypeText(edad);

                            }
                            else if (field.Code.Text.Contains("Fecha"))
                            {
                                field.Select();
                                application.Selection.TypeText(fecha);
                            }
                            else if (field.Code.Text.Contains("Diagnostico"))
                            {
                                field.Select();
                                application.Selection.TypeText(diagnostico);
                            }
                            else if (field.Code.Text.Contains("Medicamento"))
                            {
                                field.Select();
                                application.Selection.TypeText(medicamento);
                            }



                        }
                    }
                    conn.Close();
                    document.SaveAs(path + "-" + nombre + ".docx");

                    PrintDialog pDialog = new PrintDialog();
                    if (pDialog.ShowDialog() == DialogResult.OK)
                    {
                        document = application.Documents.Add(path + "-" + nombre + ".docx");
                        application.ActivePrinter = pDialog.PrinterSettings.PrinterName;
                        application.ActiveDocument.PrintOut(); //this will also work: doc.PrintOut();
                        document.Close(SaveChanges: false);
                        document = null;
                        application.Quit();
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
                        application.Quit();
                        tbCodigoCerti.Text = "";
                        chbOtro.Checked = false;
                        cbTipoDct.Enabled = false;
                        cbTipoDct.SelectedIndex = -1;
                        tbCodigoCerti.Enabled = true;
                        chbOtro.Enabled = true;
                        btnImprimir.Enabled = false;
                    }
                }
                else
                {
                    //fuera del plantel
                    var application = new Microsoft.Office.Interop.Word.Application();
                    var document = new Microsoft.Office.Interop.Word.Document();
                    string path = @"C:\Users\jc-mt\Desktop\Documentos\Recetas\Otros\Receta";//cambiar a una carpeta de otros
                    document = application.Documents.Add(Template: path + ".docx");
                    foreach (Microsoft.Office.Interop.Word.Field field in document.Fields)
                    {
                        if (field.Code.Text.Contains("Nombre"))
                        {
                            field.Select();
                            application.Selection.TypeText(nombre);

                        }
                        else if (field.Code.Text.Contains("Edad"))
                        {
                            field.Select();
                            application.Selection.TypeText(edad);

                        }
                        else if (field.Code.Text.Contains("Fecha"))
                        {
                            field.Select();
                            application.Selection.TypeText(fecha);
                        }
                        else if (field.Code.Text.Contains("Diagnostico"))
                        {
                            field.Select();
                            application.Selection.TypeText(diagnostico);
                        }
                        else if (field.Code.Text.Contains("Medicamento"))
                        {
                            field.Select();
                            application.Selection.TypeText(medicamento);
                        }
                    }

                    conn.Close();
                    document.SaveAs(path + "-" + nombre + ".docx");

                    PrintDialog pDialog = new PrintDialog();
                    if (pDialog.ShowDialog() == DialogResult.OK)
                    {
                        document = application.Documents.Add(path + "-" + nombre + ".docx");
                        application.ActivePrinter = pDialog.PrinterSettings.PrinterName;
                        application.ActiveDocument.PrintOut(); //this will also work: doc.PrintOut();
                        document.Close(SaveChanges: false);
                        document = null;
                        application.Quit();
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
                        application.Quit();
                        tbCodigoCerti.Text = "";
                        chbOtro.Checked = false;
                        cbTipoDct.Enabled = false;
                        cbTipoDct.SelectedIndex = -1;
                        tbCodigoCerti.Enabled = true;
                        chbOtro.Enabled = true;
                        btnImprimir.Enabled = false;
                    }                    
                    //print code
                    
                }
            } //tipo receta
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            //Determinar si es dentro o fuera del plantel
            if (chbOtro.Checked == true)
            {
                //Es de Fuera
                lblTituloIngreso.Text = "Ingrese el nombre del Paciente";
                bandera1 = false;
                tbCodigoCerti.MaxLength = 60;
                //Textbox solo admite letras
            }
            else
            {
                //Es de dentro
                lblTituloIngreso.Text = "Ingrese el numero identificador del Paciente";
                bandera1 = true;
                tbCodigoCerti.MaxLength = 10;
                //textbox solo admite numeros
            }

            tbCodigoCerti.Text = "";
        }

        private void btnAtras_Click(object sender, EventArgs e)
        {
            //Panel Del Listado Ocultar
            pnlListaCerti.Hide();
        }

        private void chbCertificado_CheckedChanged(object sender, EventArgs e)
        {
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
                    string cadQuery1 = "Select num_control,num_docente,fecha,diagnostico,medicamento,seguimiento,edad,sexo from consultas where num_control ='" + tbCodigoCerti.Text + "' or num_docente= '" + tbCodigoCerti.Text + "'";
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

                    }
                    if (num_docente == "")
                    {
                        num_id = num_control;
                        banderaalumno = true;
                    }
                    else
                    {
                        num_id = num_docente;
                        banderaalumno = false;
                    }

                }
                else
                {
                    //Busqueda por nombre
                    input = tbCodigoCerti.Text;
                    string cadQuery1 = "select o.nombre,c.edad,c.sexo,c.fecha,c.diagnostico,c.medicamento,c.seguimiento from consultas as c inner join otro as o on c.num_otro=o.num_otro where nombre like '%" + tbCodigoCerti.Text + "%'";
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

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void bunifuCustomLabel30_Click(object sender, EventArgs e)
        {

        }

        private void dgvListaCerti_SelectionChanged(object sender, EventArgs e)
        {
            //cambio de seleccion de row
            if (bandera1 == true)
            {
                foreach (DataGridViewRow row in dgvListaCerti.SelectedRows)
                {
                    num_control = row.Cells[0].Value.ToString();
                    num_docente = row.Cells[1].Value.ToString();
                    seguimiento = row.Cells[2].Value.ToString();
                    fecha = row.Cells[3].Value.ToString();
                    medicamento = row.Cells[4].Value.ToString();
                    diagnostico = row.Cells[5].Value.ToString();
                    num_otro = row.Cells[6].Value.ToString();
                    edad = row.Cells[7].Value.ToString();
                    sexo = row.Cells[1].Value.ToString();

                }
                if (num_docente == "")
                {
                    //alumno
                    num_id = num_control;
                    banderaalumno = true;
                }
                else 
                {
                    //docente
                    num_id = num_docente;
                    banderaalumno = false;
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

                }

            }
        }


        private void pnlListaCerti_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnListaContinuar_Click(object sender, EventArgs e)
        {
            //Verifica que se haya seleccionado uno no en blanco
            if (num_control == "" && num_docente == "" && nombre == "" )
            {
                MessageBox.Show("Seleccione un registro no en blanco", "Error de Registro",MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
