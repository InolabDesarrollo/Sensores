using ExcelMergeV7.DataSetAperturaTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class Apertura : Form
    {
        private string maxA, minA, date1A, date2A, folioA, idA, NoSerieA, Fservicio, Freporte;
        private int ind = 0, index, minutenA;

        public new static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (ind >= 4)
            {
                ft.alert("Fechas ya seleccionadas");
            }
            else
            {
                //MessageBox.Show(e.ColumnIndex.ToString()+" "+e.RowIndex.ToString());
                if (ind == 0)
                {
                    ft.alert("Fecha " + dataGridView1[0, e.RowIndex].Value.ToString() + " seleccionada");
                    inicio.Text = ft.DateFormat(ft.ToDate(dataGridView1[0, e.RowIndex].Value.ToString()));
                    dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.FromArgb(102, 255, 102);
                    ind++;
                }
                else if (ind == 1)

                {
                    ft.alert("Fecha " + dataGridView1[0, e.RowIndex].Value.ToString() + " seleccionada");
                    fin.Text = ft.DateFormat(ft.ToDate(dataGridView1[0, e.RowIndex].Value.ToString()));
                    dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.FromArgb(102, 255, 102);
                    ind++;
                }
                else if (ind == 2)

                {
                    ft.alert("Fecha " + dataGridView1[0, e.RowIndex].Value.ToString() + " seleccionada");
                    primero.Text = ft.DateFormat(ft.ToDate(dataGridView1[0, e.RowIndex].Value.ToString()));
                    dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.FromArgb(255, 153, 102);
                    ind++;
                }
                else if (ind == 3)

                {
                    ft.alert("Fecha " + dataGridView1[0, e.RowIndex].Value.ToString() + " seleccionada");
                    ultimo.Text = ft.DateFormat(ft.ToDate(dataGridView1[0, e.RowIndex].Value.ToString()));
                    dataGridView1.Rows[e.RowIndex].Cells[0].Style.BackColor = Color.FromArgb(255, 153, 102);
                    ind++;
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                rows = dataGridView1.Rows.Count;
                cols = dataGridView1.Columns.Count;
                dataGridView1.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                ft.PaintDGV(dataGridView1, float.Parse(maxA), float.Parse(minA));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ultimo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(fin.Text)
                    && !String.IsNullOrWhiteSpace(fin.Text)
                    )
                {
                    DateTime d1, d2;
                    if (DateTime.TryParse(fin.Text, out d1) && DateTime.TryParse(ultimo.Text, out d2))
                    {
                        EndLst.Text = "Tiempo entre el final de la prueba y el ultimo sensor: "
                            + Math.Abs((d2 - d1).TotalMinutes).ToString() + " minutos.";
                        EndLst.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void enc_TextChanged(object sender, EventArgs e)
        {
            encG.Text = enc.Text;
        }

        private void anexo_TextChanged(object sender, EventArgs e)
        {
        }

        private void Apertura_Load(object sender, EventArgs e)
        {

        }

        private void primero_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(inicio.Text)
                    && !String.IsNullOrWhiteSpace(inicio.Text)
                    )
                {
                    DateTime d1, d2;
                    if (DateTime.TryParse(inicio.Text, out d1) && DateTime.TryParse(primero.Text, out d2))
                    {
                        StFs.Text = "Tiempo entre el Inicio de la prueba y el primer sensor: " +
                            Math.Abs((d2 - d1).TotalMinutes).ToString() + " minutos.";
                        StFs.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private int cols, rows;
        private string hum;

        private Functions ft = new Functions();

        private void graph_Click(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(inicio.Text) && !String.IsNullOrWhiteSpace(inicio.Text) || (!String.IsNullOrEmpty(fin.Text) && !String.IsNullOrWhiteSpace(fin.Text))
                    //|| (!String.IsNullOrEmpty(primero.Text) && !String.IsNullOrWhiteSpace(primero.Text))
                    //|| (!String.IsNullOrEmpty(ultimo.Text) && !String.IsNullOrWhiteSpace(ultimo.Text))
                    )
                {
                    string ult = "", prim = "";
                    if (!String.IsNullOrEmpty(ultimo.Text) && !String.IsNullOrWhiteSpace(ultimo.Text))
                    {
                        ult = ft.Date2(ft.ToDate(ultimo.Text));
                    }
                    if (!String.IsNullOrEmpty(primero.Text) && !String.IsNullOrWhiteSpace(primero.Text))
                    {
                        prim = ft.Date2(ft.ToDate(primero.Text));
                    }
                    string sql = "Insert into Parametros(Id,f1,f2,max,min,EncabezadoGCorregidos,Folio,Id_t,DateServicio," +
                        "DateEmision,NoSerie,Anexo,Primero,Ultimo)" +
                                 " values("
                              + index + ",'" + ft.Date2(ft.ToDate(date1A)) + "','" + ft.Date2(ft.ToDate(date2A)) + "'," +
                              maxA + "," + minA + ",'" + encG.Text + "'," +
                              "'" + folioA + "','" + idA + "','" + Fservicio + "','" + Freporte +
                              "','" + NoSerieA + "','" + anexoG.Text + "','" + prim + "','" +
                              ult + "')";
                    //Console.WriteLine(sql);
                    if (ft.DeleteTable(index, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            if (hum == "Temperatura")
                            {
                                Reporte tabla = new Reporte(index, "GApertura", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "Humedad")
                            {
                                Reporte tabla = new Reporte(index, "GAperturaHumedad", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "Incertidumbre")
                            {
                                Reporte tabla = new Reporte(index, "GAperturaIncertidumbre", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "IncertidumbreHumedad")
                            {
                                Reporte tabla = new Reporte(index, "GAperturaIncertidumbreHumedad", minutenA, maxA, minA);
                                tabla.Show();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parametros no guardados");
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            if (hum == "Temperatura")
                            {
                                Reporte tabla = new Reporte(index, "GApertura", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "Humedad")
                            {
                                Reporte tabla = new Reporte(index, "GAperturaHumedad", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "Incertidumbre")
                            {
                                Reporte tabla = new Reporte(index, "GAperturaIncertidumbre", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "IncertidumbreHumedad")
                            {
                                Reporte tabla = new Reporte(index, "GAperturaIncertidumbreHumedad", minutenA, maxA, minA);
                                tabla.Show();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parametros no guardados");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione todas las fechas para continuar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ver_Click(object sender, EventArgs e)
        {
            try
            {
                if (
                    !String.IsNullOrEmpty(inicio.Text) && !String.IsNullOrWhiteSpace(inicio.Text)
                    || (!String.IsNullOrEmpty(fin.Text) && !String.IsNullOrWhiteSpace(fin.Text))
                    //|| (!String.IsNullOrEmpty(primero.Text) && !String.IsNullOrWhiteSpace(primero.Text))
                    //|| (!String.IsNullOrEmpty(ultimo.Text) && !String.IsNullOrWhiteSpace(ultimo.Text))
                    )
                {
                    string ult = ""; string prim = "";
                    if (
                        !String.IsNullOrEmpty(ultimo.Text)
                        && !String.IsNullOrWhiteSpace(ultimo.Text)
                        )
                    {
                        ult = ft.Date2(ft.ToDate(ultimo.Text));
                    }
                    if (
                        !String.IsNullOrEmpty(primero.Text)
                        && !String.IsNullOrWhiteSpace(primero.Text)
                        )
                    {
                        prim = ft.Date2(ft.ToDate(primero.Text));
                    }
                    string sql = "Insert into Parametros(Id,f1,f2,max,min,EncabezadoCorregidos,Folio,Id_t,DateServicio," +
                        "DateEmision,NoSerie,Anexo,Primero,Ultimo,Inicio,Fin)" +
                                 " values("
                              + index + ",'" + ft.Date2(ft.ToDate(date1A)) + "','" + ft.Date2(ft.ToDate(date2A)) + "'," +
                              maxA + "," + minA + ",'" + enc.Text + "'," +
                              "'" + folioA + "','" + idA + "','" + Fservicio + "','" + Freporte +
                              "','" + NoSerieA + "','" + anexo.Text + "','" + prim + "','" +
                              ult + "','" + ft.Date2(ft.ToDate(inicio.Text)) + "','" + ft.Date2(ft.ToDate(fin.Text)) + "')";

                    if (ft.DeleteTable(index, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            if (hum == "Temperatura")
                            {
                                Reporte tabla = new Reporte(index, "Apertura", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "Humedad")
                            {
                                Reporte tabla = new Reporte(index, "AperturaHumedad", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "Incertidumbre")
                            {
                                Reporte tabla = new Reporte(index, "AperturaIncertidumbre", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else if (hum == "IncertidumbreHumedad")
                            {
                                Reporte tabla = new Reporte(index, "AperturaIncertidumbreHumedad", minutenA, maxA, minA);
                                tabla.Show();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parametros no guardados");
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            if (hum == "Temperatura")
                            {
                                Reporte tabla = new Reporte(index, "Apertura", minutenA, maxA, minA);
                                tabla.Show();
                            }
                            else
                            {
                                Reporte tabla = new Reporte(index, "AperturaHumedad", minutenA, maxA, minA);
                                tabla.Show();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parametros no guardados");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Seleccione todas las fechas para continuar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void clear_Click(object sender, EventArgs e)
        {
            inicio.ResetText();
            fin.ResetText();
            primero.ResetText();
            ultimo.ResetText();
            StFs.Visible = false;
            EndLst.Visible = false;
            ind = 0;
            for (int i = 0; i < rows; i++)
            {
                dataGridView1.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(255, 255, 255);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
        }

        public Apertura(int id, string max, string min, string date1, string date2, string Id,
            string Folio, string NoSerie,
            string fservicio, string freporte,
            int minuten, string Hum, List<string> Sens)
        {
            InitializeComponent();

            DoubleBuffered(dataGridView1, true);
            maxA = max;
            minA = min;
            date1A = date1;
            date2A = date2;
            index = id;
            idA = Id;
            folioA = Folio;
            NoSerieA = NoSerie;
            Fservicio = fservicio;
            Freporte = freporte;
            minutenA = minuten;
            hum = Hum;
            if (hum == "Temperatura")
            {
                Console.WriteLine("DS_TABLES");
                //AperturaTableAdapter Apertura = new AperturaTableAdapter();
                DataSet ds = ft.SetDataSet(id, minuten, "AperturaDatos");
                dataGridView1.DataSource = ds.Tables[0];
                this.Text = "Apertura Temperatura";
                //dataGridView1.DataSource = Apertura.GetData(id, ft.ToDate(date1), ft.ToDate(date2));
            }
            else if (hum == "Humedad")
            {
                Console.WriteLine("DS_A_Humedad");
                //AperturaTableAdapter Apertura = new AperturaTableAdapter();
                DataSet ds = ft.SetDataSet(id, minuten, "AperturaHumedad");
                dataGridView1.DataSource = ds.Tables[0];
                this.Text = "Apertura Humedad";
                this.BackColor = Color.FromArgb(241, 241, 241);
                //dataGridView1.DataSource = Apertura.GetData(id, ft.ToDate(date1), ft.ToDate(date2));
            }
            else if (hum == "Incertidumbre")
            {
                Console.WriteLine("DS_TABLES");
                //AperturaTableAdapter Apertura = new AperturaTableAdapter();
                DataSet ds = ft.SetDataSet(id, minuten, "AperturaIncertidumbre");
                dataGridView1.DataSource = ds.Tables[0];
                this.Text = "Apertura Incertidumbre";
                //dataGridView1.DataSource = Apertura.GetData(id, ft.ToDate(date1), ft.ToDate(date2));
            }
            else if (hum == "IncertidumbreHumedad")
            {
                Console.WriteLine("DS_TABLES");
                AperturaTableAdapter Apertura = new AperturaTableAdapter();
                DataSet ds = ft.SetDataSet(id, minuten, "AperturaIncertidumbreHumedad");
                dataGridView1.DataSource = ds.Tables[0];
                this.Text = "Apertura Incertidumbre Humedad";
                this.BackColor = Color.FromArgb(241, 241, 241);
                //dataGridView1.DataSource = Apertura.GetData(id, ft.ToDate(date1), ft.ToDate(date2));
            }
            dataGridView1.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
            dataGridView1.Columns[0].Width = 149;
            int count = Sens.Count;
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.Format = "0.0###";

                //IncDGV.Columns[i].ValueType = GetType(Double);
            }
            for (int i = 0; i < count; i++)
            {
                //Console.WriteLine("Sensors: " + sensors[i]);
                dataGridView1.Columns[i + 1].HeaderText = Sens[i];
            }
        }
    }
}