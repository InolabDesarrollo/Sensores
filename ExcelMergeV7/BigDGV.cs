using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class BigDGV : Form
    {
        private Functions ft = new Functions();
        public List<string[,]> un;
        public string[,] united2;
        private string idS;
        private int idSi, rep;
        private int hum, Rol;
        private int flag;
        private bool Ismpf;
        private List<string> Sensors1 = new List<string>();
        private string FExt;

        public BigDGV()
        {
            InitializeComponent();
        }

        public int retRep()
        {
            return rep;
        }

        public BigDGV(int idfs1, string idF1, int countSensores, int hume, List<string> sensors, bool ismpf, string ext, int rol)
        {
            Console.WriteLine("BigDGV " + hume);

            Sensors1 = sensors;
            InitializeComponent();
            CheckModDate.Visible = false;
            NewDate.Visible = false;
            Functions ft = new Functions();
            loged.Text = "Usuario: " + idF1;
            loged.Visible = false;
            idS = idF1;
            hum = hume;
            Ismpf = ismpf;
            Console.WriteLine("Counted: " + countSensores);
            Console.WriteLine("Sensors: " + Sensors1.Count);
            FExt = ext;
            if (ext == ".xlsx" && !ismpf && !Usr.IsValidator && hume == 0)
            {
                Usr.Uni.RemoveAt(0);//Eliminación de headder de lecturas de temperatura
            }
            idSi = idfs1;
            Rol = rol;
            Flag = 0;
            DoubleBuffered(BigDGV1, true);
        }

        public new static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void BigDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private List<string> dates = new List<string>();

        public int Flag { get => flag; set => flag = value; }

        private void BigDGV1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void BigDGV_FormClosing(object sender, FormClosingEventArgs e)
        {
            /*ft.Start();
            ft.Uni.Clear();
            ft.Uni2.Clear();
            Console.WriteLine("SENSORS BigDGV: " + Sensors1.Count + " FT: " + ft.Sensors.Count);
            Sensors1.Clear();
            ft.Sensors.Clear();
            Console.WriteLine("SENSORS: " + Sensors1.Count + " FT: " + ft.Sensors.Count);
            GC.Collect();*/
        }

        private void BigDGV1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(BigDGV1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void BigDGV_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == false)
            {
                Console.WriteLine("BigDGV_HIDDEN");
                GC.Collect();
            }
        }

        private void BigDGV1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!reporte.Enabled)
            {
                //Activar botones
                reporte.Enabled = true;
                this.Refresh();
                warning.Visible = false;

                ft.Uni2.Clear();
                united2 = null;
            }

            try
            {
                label1.Visible = false;
                string input = BigDGV1[e.ColumnIndex, e.RowIndex].Value.ToString();
                //a través del formato de fecha se recorre la rutina de corte de sensores
                if (ft.MatchDate(input) && e.RowIndex != -1)
                {
                    //Sensores de Temperatura, humedad Elitech,RTD, Validator
                    try
                    {
                        charge2.Visible = true;
                        ft.alert("Homologando Datos...");

                        if (Ismpf && !ft.LogDate(input, dates))
                        {
                            //Sensores RTD
                            Console.WriteLine("IsMpf: " + Sensors1.Count);
                            ft.NewRead3(input.Substring(0, 16), ft.Uni, charge2, hum, Ismpf, Sensors1);
                            dates.Add(input);
                        }
                        else if (!Ismpf && !ft.LogDate(input, dates))
                        {
                            //Datos de Macro Elitech,.elt

                            int track = hum;
                            un = new List<string[,]>();
                            if (FExt == ".xlsx")
                            {
                                for (int i = 0; i < ft.Uni.Count; i++)
                                {
                                    //Console.WriteLine(i);
                                    un.Add(ft.Uni[i]);
                                }
                            }
                            else
                            {
                                un = ft.Uni;
                            }
                            ft.NewRead33(input.Substring(0, 16), un, charge2, track, Ismpf, Sensors1);
                            dates.Add(input);
                        }

                        un = ft.Uni;

                        united2 = ft.MatrizDeVisualizacion(ft.Uni2); //matriz que se obtiene o llena al cargar los datos
                        ft.alert("Construyendo Vista...");
                        BigDGV1.DataSource = null;
                        BigDGV1.Columns.Clear();
                        BigDGV1.DataSource = ft.BuildDatatable(united2); //se llena la tabla con la matriz
                        BigDGV1.Show();
                        Flag = 1;
                        charge2.Visible = false;

                        reporte.Visible = true;
                        ft.Uni2.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        //errors.Text = ex.ToString();
                    }
                }
                else if (ft.MatchDate1(input) && e.RowIndex != -1)
                {
                    //Datos Omega CSV
                    try
                    {
                        charge2.Visible = true;
                        ft.alert("Homologando Datos..." + input.Substring(0, 14) + ft.Uni.Count);
                        hum = 3;
                        ft.NewRead3(input.Substring(0, 14), ft.Uni, charge2, hum);

                        int co = 0;

                        for (int x = 0; x <= ft.Uni.LongCount() - 1; x++) //uni es lista 
                        {
                            if (ft.Uni[x] != null)
                            {
                                co++;
                            }
                        }

                        un = ft.Uni;

                        united2 = ft.MatrizDeVisualizacion(ft.Uni2);
                        ft.alert("Construyendo Vista...");
                        BigDGV1.DataSource = null;
                        BigDGV1.Columns.Clear();
                        BigDGV1.DataSource = ft.BuildDatatable(united2);
                        BigDGV1.Show();
                        Flag = 1;
                        charge2.Visible = false;

                        reporte.Visible = true;
                        ft.Uni2.Clear();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
                else
                {
                    ft.alert("Seleccione una fecha para continuar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BigDGV_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
            /*try
            {
                GC.Collect();
                Application.Restart();
            }
            catch (Exception ex)
            {
                ft.alert(ex.ToString());
            }*/
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Bulk insert de Datos
                SqlConnection con1 = new SqlConnection(ft.connection);

                con1.Open();
                DataTable dt = new DataTable();

                SqlBulkCopy objbulk = new SqlBulkCopy(con1);

                if (Ismpf || Usr.IsValidator)
                {
                    //Creación de datatable para Omega RTD y para Validator
                    List<int> DatePos = ft.GetPos2(united2);
                    List<int> NumbersPos = ft.GetPos3(united2);
                    dt = ft.SendToServerTempOmega(objbulk, DatePos, NumbersPos, united2);
                }
                else if (hum != 1)
                {
                    //Creación de datatable para Elitech Temperatura

                    List<int> DatePos = ft.GetPosDates(united2);
                    dt = ft.SendToServerTempElitech(objbulk, DatePos, united2);
                }
                else
                {
                    //Creación de datatable para Elitech Humedad

                    List<int> DatePos = ft.GetPosDates(united2);
                    dt = ft.SendToServerTempElitechHum(objbulk, DatePos, united2);
                }

                objbulk.WriteToServer(dt);//Envio al servidor
                con1.Close();
            }
            catch (SqlException sqle)
            {
                Console.WriteLine(sqle.ToString());
                if (sqle.Number == 201)
                {
                    if (hum == 0)
                    {
                        Selected selected = new Selected(idSi, Sensors1, Rol);

                        GC.Collect();
                        selected.Show();
                    }
                    else
                    {
                        Selected selected = new Selected(idSi, Sensors1, "humedad", hum, Rol);
                        //HumedadDGV humedad = new HumedadDGV(idSi, hum, Sensors1);

                        GC.Collect();
                        selected.Show();
                        //humedad.Show();
                    }
                    Hide();
                }
                else
                {
                    MessageBox.Show("Error Número: " + sqle.Number.ToString() + " SQL/TCP Exception");
                    MessageBox.Show("Error de conexión. \n Verifique su conexion e intente de nuevo.");
                    MessageBox.Show(sqle.ToString());
                    if (!reporte.Enabled)
                    {
                        reporte.Enabled = true;
                        Refresh();
                        warning.Visible = false;

                        ft.Uni2.Clear();
                        united2 = null;
                    }
                }
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Close();
            }
            //e.Result = result;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Visualización de datos en el servidor
            if (hum == 0)
            {
                Selected selected = new Selected(idSi, Sensors1, Rol);
                ft.Uni.Clear();
                ft.Uni2.Clear();
                GC.Collect();
                selected.Show();
            }
            else
            {
                Selected selected = new Selected(idSi, Sensors1, "humedad", hum, Rol);

                ft.Uni.Clear();
                ft.Uni2.Clear();
                GC.Collect();
                selected.Show();
            }
            Hide();
        }

        private void BigDGV_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;//Forzar multihilo
        }

        private void CheckModDate_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckModDate.Checked)
            {
                NewDate.Enabled = true;
            }
            else
            {
                NewDate.Enabled = false;
            }
        }

        private void NewDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Cambio de Fechas
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    if (ft.MatchDate(NewDate.Text))//lo manda a expresiones regulares para comparar los caracteres correctos
                    {
                        foreach (string[,] file in Usr.Uni)
                        {
                            int dt = ft.GetDatePos(file);

                            file[0, dt] = NewDate.Text;//Asignar Fecha escrita a la primera posición

                            for (int i = 1; i < file.GetLength(0); i++)
                            {
                                DateTime dd = DateTime.Parse(file[i - 1, Usr.DatePos]);
                                //Cálculo de siguiente fecha a 1 minuto
                                file[i, Usr.DatePos] = dd.AddMinutes(1).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                        united2 = ft.Unite22(Usr.Uni);
                        BigDGV1.DataSource = ft.BuildDatatable(united2);
                        //BigDGV1.DataSource = ft.BuildDatatable()
                        charge2.Visible = false;
                        ft.alert("Fechas cambiadas");
                    }
                    else
                    {
                        MessageBox.Show("Formato de fecha no válido (yyyy-MM-dd HH:mm:ss)");
                        charge2.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CheckModDate.Visible)
            {
                CheckModDate.Visible = false;
                NewDate.Visible = false;
            }
            else
            {
                CheckModDate.Visible = true;
                NewDate.Visible = true;
            }
        }

        private void BigDGV1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //Etiquetas en el headder de los sensores
            List<int> DatePos;
            if (united2 != null)
            {
                DatePos = ft.GetPos2(united2);
                int i = 0;

                foreach (int pos in DatePos)
                {
                    BigDGV1.Columns[0].Width = 149;
                    BigDGV1.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                    if (Sensors1.Count > 0 && i < Sensors1.Count)
                        BigDGV1.Columns[pos].HeaderText = Sensors1[i];
                    i++;
                }
            }
        }

        private void NewDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void BGW()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;//Trabajo a relizar
            worker.ProgressChanged += worker_ProgressChanged;//Cambios de estado
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;//Termino de ejecuci{on
            worker.RunWorkerAsync(10000);
        }

        private void reporte_Click(object sender, EventArgs e)
        {
            charge2.Value = 1;
            //Envío de datos al Servidor
            if (ft.flagG == 1)
            {
                //Si hay sensores con lecturas malas se presentan en una lista
                warning.Text = "A Algunos Sensores Les Faltan Datos.";
                warning.Visible = true;

                Flagged flagged = new Flagged(ft.Flags);
                flagged.Show();

                reporte.Enabled = false;
                ft.Flags.Clear();
                ft.flagG = 0;
                dates.Clear();
            }
            else if (Usr.Flag)
            {
                MessageBox.Show("Algunos Sensores no cuentan con las fechas selecionadas.");

                warning.Visible = true;
                ft.flagG2 = 0;
                reporte.Enabled = false;
                this.Close();
            }
            else
            {
                reporte.Visible = false;

                loged.Visible = false;
                rep = 1;
                try
                {
                    Console.WriteLine("SENSORS: " + Sensors1.Count);
                    BigDGV1.Enabled = false;
                    charge2.Visible = true;
                    BGW();//Envio Multi hilo

                    //this.Close();
                    //this.Hide();
                }
                catch (SqlException sqle)
                {
                    Console.WriteLine(sqle.ToString());
                    if (sqle.Number == 201)
                    {
                        if (hum == 0)
                        {
                            Selected selected = new Selected(idSi, Sensors1, Rol);
                            ft.Uni.Clear();
                            ft.Uni2.Clear();
                            GC.Collect();
                            selected.Show();
                        }
                        else
                        {
                            Selected selected = new Selected(idSi, Sensors1, "humedad", hum, Rol);
                            //HumedadDGV humedad = new HumedadDGV(idSi, hum, Sensors1);
                            ft.Uni.Clear();
                            ft.Uni2.Clear();
                            GC.Collect();
                            selected.Show();
                            //humedad.Show();
                        }
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Error Número: " + sqle.Number.ToString() + " SQL/TCP Exception");
                        MessageBox.Show("Error de conexión. \n Verifique su conexion e intente de nuevo.");
                        MessageBox.Show(sqle.ToString());
                        if (!reporte.Enabled)
                        {
                            reporte.Enabled = true;
                            this.Refresh();
                            warning.Visible = false;

                            ft.Uni2.Clear();
                            united2 = null;
                        }
                    }
                    //this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    this.Close();
                }
            }
        }
    }
}