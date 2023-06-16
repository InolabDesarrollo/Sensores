using ExcelMergeV7.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class ShowIncertidumbre : Form
    {
        private Functions ft = new Functions();
        private int IdS, hume, fom;
        private DataSet ds;

        private string maxi, mini, date1i, date2i,
            minuten, idRi, folioR, anexoR, noSerie,
            fechaServicio, FechaReporte, Aper, inicio, final, Table = "Incertidumbre";

        private void ModDateCB_CheckedChanged(object sender, EventArgs e)
        {
            if (ModDateCB.Checked)
            {
                NewDate.Enabled = true;
            }
            else
            {
                NewDate.Enabled = false;
            }
        }

        private void configuraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ModDateCB.Visible)
            {
                ModDateCB.Visible = false;
                NewDate.Visible = false;
            }
            else
            {
                ModDateCB.Visible = true;
                NewDate.Visible = true;
            }
        }

        private void NewDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    //Cambio de Fechas en datos de la tabla incertidumbre
                    if (ft.MatchDate(NewDate.Text.Trim()) || ft.MatchDate1(NewDate.Text.Trim()))
                    {
                        //se obtienen los datos del servidor
                        DataTable data = ft.SetDataSet(IdS, int.Parse(mins.Text), "IncertidumbreTempUpd").Tables[0];
                        int count = 0, jump = int.Parse(mins.Text);
                        if (Usr.DateDif > 1 && Usr.DateDif != 2)
                        {
                            jump = (int)Usr.DateDif;
                        }
                        if (jump > 1)
                        {
                            count = 1;
                        }
                        else
                        {
                            count = jump;
                        }
                        data.Rows[0].SetField(1, NewDate.Text);

                        for (int i = 1; i < data.Rows.Count; i++)
                        {
                            DateTime dd = DateTime.Parse(data.Rows[i - 1].ItemArray[1].ToString());

                            data.Rows[i].SetField(1, ft.DateFormat(dd.AddMinutes(jump)));
                        }

                        string query = "Update parametros set f1='" + ft.Date2(ft.ToDate(data.Rows[0].ItemArray[1].ToString()))
                            + "',f2='" + ft.Date2(ft.ToDate(data.Rows[data.Rows.Count - 1].ItemArray[1].ToString())) + "'" +
                            "where id=" + IdS + "; Delete from " + Table + " where id=" + IdS + ";";
                        if (ft.SetSql(query))
                        {
                            Date1TB.Text = data.Rows[0].ItemArray[1].ToString();
                            Date2TB.Text = data.Rows[data.Rows.Count - 1].ItemArray[1].ToString();
                            mins.Text = count.ToString();
                            mins.Enabled = false;
                            SqlConnection con1 = new SqlConnection(ft.connection);
                            con1.Open();
                            SqlBulkCopy objbulk = ft.blkObjTemp(data.Columns.Count, con1);

                            //string sql = "insert into datos(id,Tiempo,";
                            objbulk.WriteToServer(data);
                            ds = ft.SetDataSet(IdS, count, "IncertidumbreTemp");
                            IncDGV.DataSource = null;
                            IncDGV.Columns.Clear();
                            IncDGV.DataSource = ds.Tables[0];
                        }
                        else
                        {
                            MessageBox.Show("Ha ocurrido un error por favor vuelva a intentarlo.");
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Formato de fecha no válido (yyyy-MM-dd HH:mm:ss)");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void anexoTB_TextChanged(object sender, EventArgs e)
        {
            anexoR = anexoTB.Text;
        }

        private void titulo_TextChanged(object sender, EventArgs e)
        {
            EncRep.Text = titulo.Text;
        }

        public new static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        private void IncDGV_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (ft.IsValid(maxi) && ft.IsValid(mini))
            {
                ft.PaintDGV(IncDGV, float.Parse(maxi), float.Parse(mini));
            }
        }

        private void apertura_Click(object sender, EventArgs e)
        {
            try
            {
                string sql = "Insert into Parametros(Id,f1,f2,max,min,Folio,Id_t," + "DateServicio,DateEmision,NoSerie,Anexo)" +
                          " values("
                       + IdS + ",'" + ft.Date2(ft.ToDate(Date1TB.Text)) + "','" + ft.Date2(ft.ToDate(Date2TB.Text)) + "'," +
                       maxi + "," + mini + "," +
                       "'" + folioR + "','" + idRi + "','" + fechaServicio + "','" + FechaReporte +
                       "','" + noSerie + "','" + anexoR + "')";
                if (ft.isDate(Date1TB.Text) && ft.isDate(Date2TB.Text))
                {
                    if (ft.DeleteTable(IdS, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            string Tipo = "";
                            if (Aper == "Temperatura")
                            {
                                Tipo = "Incertidumbre";
                            }
                            else
                            {
                                Tipo = "IncertidumbreHumedad";
                            }

                            Apertura app = new Apertura(IdS, maxi,
                                mini, ft.DateFormat(ft.ToDate(date1i)),
                                ft.DateFormat(ft.ToDate(date2i)),
                                idRi, folioR, noSerie,
                                fechaServicio, FechaReporte,
                                int.Parse(minuten),
                                Tipo, sensors1

                                );
                            app.Show();
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            string Tipo = "";
                            if (Aper == "Temperatura")
                            {
                                Tipo = "Incertidumbre";
                            }
                            else
                            {
                                Tipo = "IncertidumbreHumedad";
                            }

                            Apertura app = new Apertura(IdS, maxi,
                                mini, ft.DateFormat(ft.ToDate(date1i)),
                                ft.DateFormat(ft.ToDate(date2i)),
                                idRi, folioR, noSerie,
                                fechaServicio, FechaReporte,
                                int.Parse(minuten),
                                Tipo, sensors1

                                );
                            app.Show();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Verifique el formato d las fechas.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private List<string> sensors1;

        private void grafica_Click(object sender, EventArgs e)
        {
            try
            {
                if (ft.DeleteTable(IdS, "Parametros"))
                {
                    if (ft.IsValid(titulo.Text) && ft.isDate(Date1TB.Text) && ft.isDate(Date2TB.Text))
                    {
                        string sql = "Insert into Parametros(Id,f1,f2,max,min,EncabezadoGCorregidos,Folio,Id_t,DateServicio,DateEmision,NoSerie,anexo)" +
                              " values("
                           + IdS + ",'" + ft.Date2(ft.ToDate(Date1TB.Text)) + "','" + ft.Date2(ft.ToDate(Date2TB.Text)) + "'," +
                           maxi + "," + mini + ",'" + titulo.Text + "'," +
                           "'" + folioR + "','" + idRi + "','" +
                           fechaServicio + "','" +
                           FechaReporte + "','" + noSerie
                           + "','" + anexoG.Text + "')";

                        if (ft.SetSql(sql))
                        {
                            ft.alert("Creando Gráfica");

                            if (hume == 0)
                            {
                                Reporte reporte = new Reporte(IdS, 2, maxi, mini, date1i, date2i, mins.Text, titulo.Text, "DatosCorregidos");
                                reporte.Show();
                                Reporte reporte1 = new Reporte(IdS, 3, titulo.Text);
                                reporte1.Show();
                            }
                            else
                            {
                                Reporte reporte = new Reporte(IdS, 2, maxi, mini, date1i, date2i, mins.Text, titulo.Text, "DatosCorregidosHumedad");
                                reporte.Show();
                                Reporte reporte1 = new Reporte(IdS, 4, titulo.Text);
                                reporte1.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Por favor vuelva a intentarlo.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingrese un encabezado para poder continuar");
                        titulo.Focus();
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(titulo.Text) && !string.IsNullOrWhiteSpace(titulo.Text))
                    {
                        string sql = "Insert into Parametros(Id,f1,f2,max,min,EncabezadoGCorregidos,Folio,Id_t,DateServicio,DateEmision,NoSerie,anexo)" +
                              " values("
                           + IdS + ",'" + ft.Date2(ft.ToDate(Date1TB.Text)) + "','" + ft.Date2(ft.ToDate(Date2TB.Text)) + "'," +
                           maxi + "," + mini + ",'" + EncRep.Text + "'," +
                           "'" + folioR + "','" + idRi + "','" +
                           fechaServicio + "','" +
                           FechaReporte + "','" + noSerie
                           + "','" + anexoG.Text + "')";

                        if (ft.SetSql(sql))
                        {
                            ft.alert("Creando Gráfica");

                            if (hume == 0)
                            {
                                Reporte reporte = new Reporte(IdS, 2, maxi, mini, date1i, date2i, mins.Text, titulo.Text, "DatosCorregidos");
                                reporte.Show();
                                Reporte reporte1 = new Reporte(IdS, 3, titulo.Text);
                                reporte1.Show();
                            }
                            else
                            {
                                Reporte reporte = new Reporte(IdS, 2, maxi, mini, date1i, date2i, mins.Text, titulo.Text, "DatosCorregidosHumedad");
                                reporte.Show();
                                Reporte reporte1 = new Reporte(IdS, 4, titulo.Text);
                                reporte1.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Por favor vuelva a intentarlo.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ingrese un encabezado para poder continuar");
                        titulo.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ShowIncertidumbre_FormClosed(object sender, FormClosedEventArgs e)
        {
            string table = "Incertidumbre";
            if (hume != 0)
            {
                table = "IncertidumbreHumedad";
            }
            if (ft.DeleteTable(IdS, table))
            {
                FormCollection frm = Application.OpenForms;
                for (int i = 0; i < frm.Count; i++)
                {
                    if (frm[i].Name == "IncertidumbreHumedad" && frm[i].Visible == false)
                    {
                        frm[i].Show();
                        Thread.Sleep(2000);
                        //frm[i].Dispose();
                        frm[i].Close();
                        break;
                    }
                }
                //start.Show();
                if (hume != 0)
                {
                    FormCollection frm1 = Application.OpenForms;
                    for (int i = 0; i < frm1.Count; i++)
                    {
                        if (frm1[i].Name == "IncertidumbreHumedad" && frm1[i].Visible == false)
                        {
                            frm1[i].Show();
                            frm1[i].Close();
                            break;
                        }
                    }
                }
                else
                {
                    FormCollection frm1 = Application.OpenForms;
                    for (int i = 0; i < frm1.Count; i++)
                    {
                        if ((frm1[i].Name == "IncertidumbreHumedad" && frm1[i].Visible == false) || (frm[i].Name == "incertidumbre" && frm[i].Visible == false))
                        {
                            frm1[i].Close();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Ha ocurrido un problema al intentar vaciar los datos. ");
                this.Close();
            }
        }

        private void report_Click(object sender, EventArgs e)
        {
            try
            {
                if (ft.MatchFloat(maxi)
                    && ft.MatchFloat(mini)
                    && ft.isDate(date1i)
                    && ft.isDate(date2i)
                    //&& ft.isDate(inicio)
                    //&& ft.isDate(final)
                    )
                {
                    ft.alert("Creando Reporte");

                    string r = "incertidumbre", u = "incertidumbre";
                    if (hume == 3 || hume == 1)
                    {
                        r = "incertidumbreHumedad";
                        u = "incertidumbreHumedad";
                    }
                    else
                    {
                        r = "incertidumbre";
                        r = "incertidumbre";
                    }
                    string st, end;
                    if (ft.isDate(inicio))
                    {
                        st = "'" + ft.Date2(ft.ToDate(inicio)) + "'";
                    }
                    else
                    {
                        st = "null";
                    }
                    if (ft.isDate(final))
                    {
                        end = "'" + ft.Date2(ft.ToDate(final)) + "'";
                    }
                    else
                    {
                        end = "null";
                    }
                    string sql = "Insert into Parametros(Id,f1,f2,max,min,EncabezadoCorregidos," +
                                    "Folio,Id_t,DateServicio,DateEmision,NoSerie,Anexo,inicio,fin)" +
                           " values("
                        + IdS + ",'" + ft.Date2(ft.ToDate(Date1TB.Text)) + "','" +
                        ft.Date2(ft.ToDate(Date2TB.Text)) + "'," +
                        maxi + "," + mini + ",'" + EncRep.Text + "'," +
                        "'" + folioR + "','" + idRi + "','" + fechaServicio
                        + "','" + FechaReporte + "','" + noSerie + "','" + anexoR +
                        "'," + st + "," + end + ")";

                    if (ft.DeleteTable(IdS, "Parametros"))
                    {
                        if (ft.MatchFloat(referencia.Text) && ft.isDate(Date1TB.Text) && ft.isDate(Date2TB.Text))
                        {
                            if (!String.IsNullOrEmpty(EncRep.Text) && !String.IsNullOrWhiteSpace(EncRep.Text))
                            {
                                if (ft.SetSql(sql))
                                {
                                    Reporte2 rep = new Reporte2(IdS, maxi, mini, date1i, date2i, r, mins.Text, FechaReporte, fechaServicio, folioR, anexoR, idRi);
                                    rep.Show();

                                    uniformidad uni = new uniformidad(IdS, date1i, date2i, mins.Text, maxi, mini, u, Usr.Incertidumbre.Count, referencia.Text);
                                    uni.Show();
                                    uniformidadV1 uniV1 = new uniformidadV1(IdS, date1i, date2i, mins.Text, maxi, mini, u, Usr.Incertidumbre.Count, referencia.Text);
                                    uniV1.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Por favor vuelva a intentarlo.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Ingrese un encabezado para continuar.");
                                EncRep.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Verifique la temperatura de referencia");
                            referencia.Focus();
                        }
                    }
                    else
                    {
                        if (ft.MatchFloat(referencia.Text) && ft.isDate(Date1TB.Text) && ft.isDate(Date2TB.Text))
                        {
                            if (!String.IsNullOrEmpty(EncRep.Text) && !String.IsNullOrWhiteSpace(EncRep.Text))
                            {
                                if (ft.SetSql(sql))
                                {
                                    Reporte2 rep = new Reporte2(IdS, maxi, mini, date1i, date2i, r, mins.Text, FechaReporte, fechaServicio, folioR, anexoR, idRi);
                                    rep.Show();

                                    uniformidad uni = new uniformidad(IdS, date1i, date2i, mins.Text, maxi, mini, u, sensors1.Count, referencia.Text);
                                    uni.Show();
                                    uniformidadV1 uniV1 = new uniformidadV1(IdS, date1i, date2i, mins.Text, maxi, mini, u, sensors1.Count, referencia.Text);
                                    uniV1.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Por favor vuelva a intentarlo.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Ingrese un encabezado para continuar.");
                                EncRep.Focus();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Verifique la temperatura de referencia");
                            referencia.Focus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Verifique que los parámetros cuenten con el formato correcto.");
                    //ft.Uni.Clear();
                    //ft.Uni2.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void ShowIncertidumbre_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //Message on closing on closed execute
                ft.incer = 0;

                DialogResult dialogResult = MessageBox.Show("¿Seguro que desea salir? Todo proceso no guardado será eliminado", "", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    //Clean DB

                    //if code here....
                }
                else
                {
                    //else code here....
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private string val = "";

        private void IncDGV_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                using (SolidBrush b = new SolidBrush(IncDGV.RowHeadersDefaultCellStyle.ForeColor))
                {
                    e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString() + "\n" + val);
            }
        }

        public ShowIncertidumbre(int id, string max, string min, string date1, string date2, int minutes, string fechaServ, string fecharep, string idR, string folio, string anexo, int hum, int fom1, List<string> sensors, string NoSerie, string Inicio, string Final)
        {
            try
            {
                InitializeComponent();
                maxi = max;
                mini = min;
                date1i = date1;
                Date1TB.Text = date1i;
                date2i = date2;
                Date2TB.Text = date2i;
                IdS = id;
                inicio = Inicio;
                final = Final;
                minuten = minutes.ToString();
                mins.Text = minuten.ToString();
                FechaReporte = fecharep;
                fechaServicio = fechaServ;
                idRi = idR;
                folioR = folio;
                anexoR = anexo;
                anexoTB.Text = anexoR;
                hume = hum;
                noSerie = NoSerie;
                sensors1 = sensors;
                fom = fom1;
                DoubleBuffered(IncDGV, true);
                data.Text = "Límite Max: " + max + " Límite Min: " + min //+ " \nFecha 1: " + date1
                                                                         //+ " Fecha 2: " + date2
                                                                         //+ " \nIntervalo: " + minuten + " minutos."
                    +
                    " No.De Serie: " + noSerie;
                if (ft.IsValid(inicio) && ft.IsValid(final))
                {
                    data.Text = data.Text + " \n Inicio de la prueba: " + inicio + "\n Fin de la prueba: " + final;
                }
                referencia.Text = (float.Parse(maxi) - ((float.Parse(maxi) - float.Parse(mini)) / 2)).ToString();
                if (Usr.DateDif > 1 && Usr.DateDif != 2)
                {
                    mins.Text = "1";
                    mins.Enabled = false;
                }

                if (hum == 1 || hum == 3)
                {
                    Aper = "Humedad";
                    Table = "IncertidumbreHumedad";
                    configuraciónToolStripMenuItem.Visible = false;

                    ds = ft.SetDataSet(id, int.Parse(minuten), "IncertidumbreHum");
                    IncDGV.DataSource = ds.Tables[0];

                    this.BackColor = Color.FromArgb(241, 241, 241);
                    this.Text = "Incertidumbre Humedad";
                    IncDGV.BackgroundColor = Color.FromArgb(241, 241, 241);
                    SD.Text = "D. esta= " + ft.getSDdates(IdS.ToString(), "IncertidumbreHumedad", "h", "SDP") + " CV= " + ft.getSDdates(IdS.ToString(), "Incertidumbrehumedad", "H", "CV") + "%";
                }
                else
                {
                    Aper = "Temperatura";
                    this.Text = "Incertidumbre Temperatura";

                    ds = ft.SetDataSet(id, int.Parse(minuten), "IncertidumbreTemp");
                    IncDGV.DataSource = ds.Tables[0];
                    SD.Text = "D. esta= " + ft.getSDdates(IdS.ToString(), "incertidumbre", "s", "SDP") + " CV= " + ft.getSDdates(IdS.ToString(), "incertidumbre", "s", "CV") + "%";
                }

                IncDGV.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss ";
                IncDGV.Columns[0].Width = 149;
                for (int i = 1; i < IncDGV.ColumnCount; i++)
                {
                    IncDGV.Columns[i].DefaultCellStyle.Format = "N1";
                }
                if (sensors.Count > 0)
                {
                    ft.SetSens(sensors, folioR);
                    for (int i = 0; i < sensors.Count; i++)
                    {
                        IncDGV.Columns[i + 1].HeaderText = sensors1[i];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}