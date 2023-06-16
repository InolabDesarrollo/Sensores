using ExcelMergeV7.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class Selected : Form
    {
        private int index; // es IdUsuario
        private int Rol;
        private List<string> sensors1 = new List<string>();
        private incertidumbre options;

        public new static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
            dgv.Refresh();
        }

        private void ClearDGV(Control ctr)
        {
            this.ActiveControl = ctr;
            dataGridView1.Refresh();
            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = null;
        }

        public Selected(int index1, List<string> sensors, int rol)
        {
            try
            {
                InitializeComponent();
                index = index1;
                Rol = rol;

                sensors1 = sensors;

                //dataGridView1.DataBindings.Add();
                Go();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void incertidumbre_Click(object sender, EventArgs e)
        {
            try
            {
                if (ft.MatchFloat(max.Text) && ft.MatchFloat(min.Text)
                   && ft.isDate(date1.Text)
                   && ft.isDate(date2.Text)
                   && hours.Value != 0 && minutes.Value != 0
                   && ft.IsValid(folio.Text) && ft.IsValid(id.Text) && ft.IsValid(anexo.Text)
                   && ft.isDate(FechaReporte.Text)
                   && ft.isDate(FechaServicio.Text)

                   )
                {
                    string st = ft.Format(InicioPrueba.Text), end = ft.Format(FinPrueba.Text); // da formato a la fecha
                    string sql = "delete from incertidumbre where id=" + index + ";" +
                        "insert into Parametros(id,f1,f2,inicio,fin)" +
                        " values('" + index + "','" + ft.Date2(ft.ToDate(date1.Text)) +
                        "','" + ft.Date2(ft.ToDate(date2.Text)) +
                        "'," + st + "," + end + ");";
                    if (ft.DeleteTable(index, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            //OPTIONS es una instancia de Incertidumbre para que sea posible iniciarla
                            //la tabla incertidumbre debe estar vacía
                            if (options == null && ft.GetIdIncertidumbre(index.ToString(), "Incertidumbre"))
                            {
                                //Incertidumbre crea los campos dependiendo de la cantidad de columnas en
                                //la tabla datos
                                options = new incertidumbre(index, max.Text, min.Text, date1.Text, date2.Text,
                                    int.Parse(minutes.Value.ToString()), 0, FechaServicio.Text,
                                    FechaReporte.Text, 1, folio.Text, id.Text, anexo.Text,
                                    sensors1, NoSerie.Text, InicioPrueba.Text, FinPrueba.Text);

                                options.FormClosed += (o, ea) => options = null;
                                options.Show();
                            }
                            else
                            {
                                /*if (ft.GetIdIncertidumbre(index.ToString(), "Incertidumbre"))
                                {
                                    options.WindowState = FormWindowState.Normal;
                                    options.Show();
                                }
                                else
                                {*/
                                options = null;
                                ft.DeleteTable(index, "incertidumbre");
                                MessageBox.Show("Cierre primero la ventana de incertidumbre(Temperatura).");
                                //}
                            }
                        }
                        else
                        {
                            MessageBox.Show("Se ha producido un error.");
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            if (options == null && ft.GetIdIncertidumbre(index.ToString(), "Incertidumbre"))
                            {
                                options = new incertidumbre(index, max.Text, min.Text, date1.Text, date2.Text,
                                    int.Parse(minutes.Value.ToString()), 0, FechaServicio.Text,
                                    FechaReporte.Text, 1, folio.Text, id.Text, anexo.Text,
                                    sensors1, NoSerie.Text, InicioPrueba.Text, FinPrueba.Text);

                                options.FormClosed += (o, ea) => options = null;
                                options.Show();
                            }
                            else
                            {
                                /*if (ft.GetIdIncertidumbre(index.ToString(), "Incertidumbre"))
                                {
                                    options.WindowState = FormWindowState.Normal;
                                    options.Show();
                                }
                                else
                                {*/
                                MessageBox.Show("Cierre primero la ventana de incertidumbre(Temperatura).");
                                //}
                            }
                        }
                        else
                        {
                            MessageBox.Show("Se ha producido un error.");
                        }
                    }
                }
                else
                {
                    ft.CheckWrongs(this, hours, minutes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private HumedadDGV humedadS;

        public Selected(int index1, List<string> sensors, string humedad, int hum, int rol)
        {
            InitializeComponent();

            if (humedad == "humedad")
            {
                humedadS = new HumedadDGV(index1, hum, sensors);
            }
            index = index1;
            Rol = rol;

            sensors1 = sensors;
            //Console.WriteLine("SD: " + ft.getSD(index1.ToString(), "Datos","S","SDP")+" CV: "+ ft.getSD(index1.ToString(), "Datos", "S", "CV")+"datos+s");
            Go();

            //dataGridView1.DataBindings.Add();
        }

        public Selected()
        {
            InitializeComponent();
        }

        private Functions ft = new Functions();

        private void reporte_Click(object sender, EventArgs e)
        {
            try
            {
                if (ft.MatchFloat(max.Text) && ft.MatchFloat(min.Text)
                    && ft.isDate(date1.Text)
                    && ft.isDate(date2.Text) && hours.Value != 0 && minutes.Value != 0
                   && ft.IsValid(folio.Text) && ft.IsValid(anexo.Text)
                   && ft.IsValid(id.Text)
                   && ft.IsValid(referencia.Text)
                   && ft.IsValid(DatosEnc.Text)
                   && ft.isDate(FechaReporte.Text)
                   && ft.isDate(FechaServicio.Text)
               //&& ft.isDate(InicioPrueba.Text)
               //&& ft.isDate(FinPrueba.Text)
               )
                {
                    ft.SetSens(sensors1, folio.Text);
                    string st = ft.Format(InicioPrueba.Text), end = ft.Format(FinPrueba.Text);

                    string sql = "Insert into Parametros(Id,f1,f2,max,min,EncabezadoCrudos,Folio,Id_t,DateServicio,DateEmision,NoSerie,Anexo,inicio,fin)" +
                            " values("
                         + index + ",'" + ft.Date2(ft.ToDate(date1.Text)) + "','" + ft.Date2(ft.ToDate(date2.Text)) + "'," +
                         max.Text + "," + min.Text + ",'" + DatosEnc.Text + "'," +
                         "'" + folio.Text + "','" + id.Text + "','" + FechaServicio.Text + "','" + FechaReporte.Text +
                         "','" + NoSerie.Text + "','" + anexo.Text + "'," + st + "," + end + ")";
                    ft.alert("Creando Reporte");
                    //ft.alert("max:"+max.Text+" min: "+min.Text+" date1: "+date1.Text+" date2: "+date2.Text);

                    //Reporte2 rep= new Reporte2(index,max.Text,min.Text,date1.Text,date2.Text,int.Parse(hours.Value.ToString()), int.Parse(minutes.Value.ToString()));
                    if (ft.DeleteTable(index, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            SD_dts.Text = "D. esta= " + ft.getSDdates(index.ToString(), "Datos", "s", "SDP") + "   CV= " + ft.getSDdates(index.ToString(), "Datos", "s", "CV") + "%";
                            Reporte2 rep = new Reporte2(index, max.Text, min.Text, date1.Text, date2.Text, "datos", minutes.Value.ToString(), FechaReporte.Text, FechaServicio.Text, folio.Text, anexo.Text, id.Text);
                            rep.Show();
                            //uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text);
                            if (ft.MatchFloat(referencia.Text))
                            {
                                uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datos", sensors1.Count, referencia.Text);
                                uni.Show();
                                uniformidadV1 uniV1 = new uniformidadV1(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datosV1", sensors1.Count, referencia.Text);
                                uniV1.Show();
                            }
                            else
                            {
                                ft.alert("Verifique el formato \n de la temperatura de referenca.");
                                referencia.Focus();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parametros NO Guardados");
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            SD_dts.Text = "D. esta= " + ft.getSDdates(index.ToString(), "Datos", "s", "SDP") + "   CV= " + ft.getSDdates(index.ToString(), "Datos", "s", "CV") + "%";
                            Reporte2 rep = new Reporte2(index, max.Text, min.Text, date1.Text, date2.Text, "datos", minutes.Value.ToString(), FechaReporte.Text, FechaServicio.Text, folio.Text, anexo.Text, id.Text);
                            rep.Show();
                            //uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text);
                            if (ft.MatchFloat(referencia.Text))
                            {
                                uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datos", sensors1.Count, referencia.Text);
                                uni.Show();
                                uniformidadV1 uniV1 = new uniformidadV1(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datosV1", sensors1.Count, referencia.Text);
                                uniV1.Show();
                            }
                            else
                            {
                                ft.alert("Verifique el formato \n de la temperatura de referenca.");
                                referencia.Focus();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Parametros NO Guardados");
                        }
                    }
                    SqlDataReader re = ft.getSD(index.ToString(), "Datos", "s");
                    if (re.GetValue(0).ToString() != "" && re.GetValue(1).ToString() != "")
                    {
                        Decimal sdp = Decimal.Parse(re.GetValue(0).ToString());
                        Decimal.Round(sdp, 2).ToString();
                        Decimal cv = Decimal.Parse(re.GetValue(1).ToString());
                        Decimal.Round(cv, 2).ToString();
                        SD.Text = "D. esta(datos crudos)= " + Decimal.Round(sdp, 2).ToString()
                            + " CV= " + Decimal.Round(cv, 2).ToString() + "%";
                    }
                }
                else
                {
                    ft.CheckWrongs(this, hours, minutes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                Console.WriteLine(ex.ToString());
            }

            //this.Close();
        }

        private void graph_Click(object sender, EventArgs e)
        {
            try
            {
                if (ft.IsValid(folio.Text)
                   && ft.IsValid(id.Text)
                   && ft.IsValid(referencia.Text)
                   && ft.IsValid(encabezado.Text)
                   && ft.IsValid(margin.Text)
                   && ft.isDate(FechaReporte.Text)
                   && ft.isDate(FechaServicio.Text)
                   && ft.IsValid(minutes.Value.ToString())
                   && minutes.Value > 0
                   )
                {
                    string sql = "Insert into Parametros(Id,EncabezadoGCrudos," +
                           "Folio,Id_t,DateServicio,DateEmision,NoSerie,Anexo,max,min,Minuto)" +
                                 " values("
                              + index + ",'" + encabezado.Text + "'," +
                              "'" + folio.Text + "','" + id.Text + "','"
                              + FechaServicio.Text + "','" +
                              FechaReporte.Text + "','" +
                              NoSerie.Text + "','" + anexoG.Text + "'," + max.Text + "," + min.Text + "," + minutes.Value + ")";
                    if (ft.DeleteTable(index, "Parametros")) //se elimina el registro anterior para que cada usuario tenga su registro, cuando el index = a un registro
                    {
                        if (ft.SetSql(sql))
                        {
                            ft.alert("Creando Gráfica");
                            //El entero define la Gráfica
                            Reporte reporte = new Reporte(index, 0, encabezado.Text);
                            reporte.Show();
                            Reporte reporte1 = new Reporte(index, 2, encabezado.Text);
                            reporte1.Show();
                        }
                        else
                        {
                            Console.WriteLine("encabezado verCrudos NO Guardado");
                            ft.alert("Por favor vuelva a intentarlo.");
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            ft.alert("Creando Gráfica");

                            Reporte reporte = new Reporte(index, 0, encabezado.Text);
                            reporte.Show();
                            Reporte reporte1 = new Reporte(index, 2, encabezado.Text);
                            reporte1.Show();
                        }
                        else
                        {
                            Console.WriteLine("encabezado verCrudos NO Guardado");
                            ft.alert("Por favor vuelva a intentarlo.");
                        }
                    }
                }
                else
                {
                    ft.CheckWrongs(this, hours, minutes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            //this.Hide();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void criterio_TextChanged(object sender, EventArgs e)
        {
            referencia.Text = criterio.Text;
            ft.CoulorW(criterio);
            if (ft.IsValid(criterio.Text) && ft.IsValid(margin.Text))
            {
                if (ft.MatchFloat(criterio.Text))
                {
                    Decimal cC = Decimal.Parse(criterio.Text);
                    Decimal margen = Decimal.Parse(margin.Text);

                    max.Text = (cC + margen).ToString();
                    min.Text = (cC - margen).ToString();
                }
            }
        }

        private void margin_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(margin);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void hours_ValueChanged(object sender, EventArgs e)
        {
            if (hours.Value > 0)
            {
                hours.BackColor = SystemColors.Window;
                if (humedadS != null)
                {
                    humedadS.hours.Value = hours.Value;
                }
                if (ft.isDate(date1.Text))
                {
                    date2.Text = ft.DateFormat(ft.ToDate(date1.Text).AddHours(double.Parse(hours.Value.ToString())));
                }
                else
                {
                    MessageBox.Show("Primero seleccione una fecha.");
                    hours.Value = 0;
                }
            }
            else
            {
                hours.BackColor = System.Drawing.Color.LightCoral;
            }
        }

        private void Selected_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Clean DB
            //On Selected First hide all then close it

            try
            {
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Selected_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (Rol == 4 || Rol == 5)
                {
                    DialogResult dialogResult1 = MessageBox.Show("¿Salir sin guardar?", "", MessageBoxButtons.YesNo);
                    if (dialogResult1 == DialogResult.No)
                    {
                        DialogResult dialogResult = MessageBox.Show("¿Seguro que desea salir? Todo proceso no guardado será eliminado", "", MessageBoxButtons.YesNo);

                        if (dialogResult == DialogResult.Yes && !String.IsNullOrEmpty(folio.Text) && !String.IsNullOrWhiteSpace(folio.Text))
                        {
                            string sql = "Update Datos set Id='" + folio.Text + "', IdUsuario='" + index + "', FechaRegistro='" + ft.Date2(DateTime.Now) + "' where Id='" + index + "';";
                            string q = "delete from incertidumbre where id=" + index + ";";
                            string q1 = "delete from incertidumbreHumedad where id=" + index + ";";
                            string q2 = "delete from parametros where id=" + index + ";";

                            //Console.WriteLine(sql);
                            if (ft.GetFolioId(folio.Text))
                            {
                                if (ft.SetSql(sql + q + q1 + q2))
                                {
                                    //ft.incer = 0;
                                    //ft.Uni.Clear();
                                    //ft.Uni2.Clear();
                                    //GC.Collect();
                                    //ft.Start();
                                }
                                else
                                {
                                    MessageBox.Show("Ha ocurrido un problema al intentar vaciar los datos. ");
                                    e.Cancel = true;
                                }
                            }
                            else
                            {
                                MessageBox.Show("El No. de Folio ya se encuentra en el servidor");
                                e.Cancel = true;
                            }
                        }
                        else if (dialogResult == DialogResult.No)
                        {
                            e.Cancel = true;
                        }
                        else
                        {
                            MessageBox.Show("Escriba el No. de Folio para continuar.");
                            e.Cancel = true;
                        }
                    }
                    else
                    {
                        if (index == Usr.K)
                        {
                            string sql = "delete from datos where id='" + index + "';";
                            string q = "delete from incertidumbre where id=" + index + ";";
                            string q1 = "delete from incertidumbreHumedad where id=" + index + ";";
                            string q2 = "delete from parametros where id=" + index + ";";
                            ft.SetSql(sql + q + q1 + q2);
                        }
                        else
                        {
                            string q = "delete from incertidumbre where id=" + index + ";";
                            string q1 = "delete from incertidumbreHumedad where id=" + index + ";";
                            string q2 = "delete from parametros where id=" + index + ";";
                            ft.SetSql(q + q1 + q2);
                        }
                    }
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("¿Seguro que desea salir? Todo proceso no guardado será eliminado", "", MessageBoxButtons.YesNo);

                    if (dialogResult == DialogResult.Yes && !String.IsNullOrEmpty(folio.Text) && !String.IsNullOrWhiteSpace(folio.Text))
                    {
                        string sql = "Update Datos set Id='" + folio.Text + "', IdUsuario='" + index + "', FechaRegistro='" + ft.Date2(DateTime.Now) + "' where Id='" + index + "'";
                        if (ft.GetFolioId(folio.Text))
                        {
                            if (ft.SetSql(sql))
                            {
                                /*ft.incer = 0;
                                ft.Uni.Clear();
                                ft.Uni2.Clear();
                                GC.Collect();
                                ft.Start();*/
                            }
                            else
                            {
                                MessageBox.Show("Ha ocurrido un problema al intentar vaciar los datos. ");
                                e.Cancel = true;
                            }
                        }
                        else
                        {
                            MessageBox.Show("El No. de Folio ya se encuentra en el servidor");
                            e.Cancel = true;
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        MessageBox.Show("Escriba el No. de Folio para continuar.");
                        e.Cancel = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                e.Cancel = true;
            }
        }

        private void date1_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(date1);
            if (date1.Text != null && humedadS != null)
            {
                humedadS.Data = date1.Text;
                humedadS.date1.Text = humedadS.Data;
            }
        }

        private void apertura_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("Apertura");
            try
            {
                if (ft.MatchFloat(max.Text) && ft.MatchFloat(min.Text) && ft.isDate(date1.Text)
                    && ft.isDate(date2.Text) && hours.Value != 0 && minutes.Value != 0
               && ft.IsValid(folio.Text) && ft.IsValid(anexo.Text)
               && ft.IsValid(id.Text)
               && ft.IsValid(referencia.Text)
               && ft.IsValid(DatosEnc.Text)
               && ft.isDate(FechaReporte.Text)
               && ft.isDate(FechaServicio.Text)
               )
                {
                    string sql = "Insert into Parametros(Id,f1,f2,max,min,EncabezadoCrudos,Folio,Id_t,DateServicio,DateEmision,NoSerie,Anexo)" +
                            " values("
                         + index + ",'" + ft.Date2(ft.ToDate(date1.Text)) + "','" + ft.Date2(ft.ToDate(date2.Text)) + "'," + //INDEX ES EL ID DE USUARIO
                         max.Text + "," + min.Text + ",'" + DatosEnc.Text + "'," +
                         "'" + folio.Text + "','" + id.Text + "','" + FechaServicio.Text + "','" + FechaReporte.Text +
                         "','" + NoSerie.Text + "','" + anexo.Text + "')";
                    if (ft.DeleteTable(index, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            Console.WriteLine("1");
                            Apertura app = new Apertura(index, max.Text,
                                min.Text, ft.DateFormat(ft.ToDate(date1.Text)),
                                ft.DateFormat(ft.ToDate(date2.Text)),
                                id.Text, folio.Text, NoSerie.Text,
                                FechaServicio.Text, FechaReporte.Text,
                                int.Parse(minutes.Value.ToString()),
                                "Temperatura", sensors1

                                );
                            app.Show();
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            Console.WriteLine("1");
                            Apertura app = new Apertura(index, max.Text,
                                min.Text, ft.DateFormat(ft.ToDate(date1.Text)),
                                ft.DateFormat(ft.ToDate(date2.Text)),
                                id.Text, folio.Text, NoSerie.Text,
                                FechaServicio.Text, FechaReporte.Text,
                                int.Parse(minutes.Value.ToString()),
                                "Temperatura", sensors1

                                );
                            app.Show();
                        }
                    }
                }
                else
                {
                    ft.CheckWrongs(this, hours, minutes);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void id_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(id);
            if (ft.IsValid(id.Text)
                && humedadS != null

                )
            {
                humedadS.id.Text = id.Text;
            }
        }

        private void folio_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(folio);
            if (ft.IsValid(folio.Text)
                && humedadS != null

                )
            {
                humedadS.folio.Text = folio.Text;
            }
        }

        private void NoSerie_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(NoSerie);
            if (ft.IsValid(NoSerie.Text)
                && humedadS != null

                )
            {
                humedadS.NoSerie.Text = NoSerie.Text;
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)
                {
                    if (ft.isDate(FinPrueba.Text))
                    {
                    }
                    else

                    if (ft.isDate(InicioPrueba.Text) && !ft.isDate(FinPrueba.Text))
                    {
                        dataGridView1[0, e.RowIndex].Style.BackColor = System.Drawing.Color.Red;
                        dataGridView1[0, e.RowIndex].Style.ForeColor = System.Drawing.Color.White;
                        FinPrueba.Text = dataGridView1[0, e.RowIndex].Value.ToString();
                        ClearDGV(FinPrueba);
                    }
                    else
                    if (!ft.isDate(date1.Text))
                    {
                        ft.alert("Fecha " + dataGridView1[0, e.RowIndex].Value.ToString() + " seleccionada");
                        date1.Text = ft.DateFormat(ft.ToDate(dataGridView1[0, e.RowIndex].Value.ToString()));
                    }
                    else
                    if (ft.isDate(date1.Text) && ft.isDate(date2.Text))
                    {
                        dataGridView1[0, e.RowIndex].Style.BackColor = System.Drawing.Color.Green;
                        dataGridView1[0, e.RowIndex].Style.ForeColor = System.Drawing.Color.White;
                        InicioPrueba.Text = dataGridView1[0, e.RowIndex].Value.ToString();
                        ClearDGV(InicioPrueba);
                    }
                    else
                    if (ft.isDate(date1.Text) && hours.Value != 0)
                    {
                        ft.alert("Fecha " + dataGridView1[0, e.RowIndex].Value.ToString() + " seleccionada");
                        date1.Text = ft.DateFormat(ft.ToDate(dataGridView1[0, e.RowIndex].Value.ToString()));
                        date2.Text = ft.DateFormat(ft.ToDate(date1.Text).AddHours(double.Parse(hours.Value.ToString())));
                    }
                    else
                    if (ft.isDate(date1.Text))
                    {
                        ft.alert("Fecha " + dataGridView1[0, e.RowIndex].Value.ToString() + " seleccionada");
                        date1.Text = ft.DateFormat(ft.ToDate(dataGridView1[0, e.RowIndex].Value.ToString()));
                        //date2.Text = ft.DateFormat(ft.ToDate(date1.Text).AddHours(double.Parse(hours.Value.ToString())));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (ft.MatchFloat(max.Text) && ft.MatchFloat(min.Text))
            {
                ft.PaintDGV(dataGridView1, float.Parse(max.Text), float.Parse(min.Text));
            }
        }

        private void margin_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    if (ft.IsValid(criterio.Text) && ft.IsValid(margin.Text))
                    {
                        if (ft.MatchFloat(margin.Text))
                        {
                            Decimal cC = Decimal.Parse(criterio.Text);
                            Decimal margen = Decimal.Parse(margin.Text);

                            max.Text = (cC + margen).ToString();
                            min.Text = (cC - margen).ToString();

                            dataGridView1.DataSource = null;
                            Datos1TableAdapter datos = new Datos1TableAdapter();
                            dataGridView1.DataSource = datos.GetByID(index);
                            dataGridView1.Columns[0].Width = 149;
                            dataGridView1.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                            for (int i = 0; i < sensors1.Count; i++)
                            {
                                //Console.WriteLine("Sensors: " + sensors[i]);
                                dataGridView1.Columns[i + 1].HeaderText = sensors1[i];
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void margin_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void folio_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private Datos1TableAdapter datos;

        public int Go()
        {
            //datos = new Datos1TableAdapter();
            //dataGridView1.DataSource = datos.GetByID(index);
            DataSet ds = ft.SetDataSet(index, "GetDatos");
            dataGridView1.DataSource = ds.Tables[0];

            dataGridView1.Columns[0].Width = 149;
            dataGridView1.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

            if (sensors1.Count > 0)
            {
                for (int i = 0; i < sensors1.Count; i++)
                {
                    dataGridView1.Columns[i + 1].HeaderText = sensors1[i];
                }
            }

            if (humedadS != null)
            {
                //humedadS.SD.Text = "D.esta= " + ft.getSD(index.ToString(), "Datos", "H", "SDP") + "CV: " + ft.getSD(index.ToString(), "Datos", "H", "CV") + "%";
                humedadS.index = index;
                Datos2TableAdapter datos2 = new Datos2TableAdapter();
                humedadS.dataGridView1.DataSource = datos2.GetDataBy(index);
                humedadS.dataGridView1.Columns[0].Width = 149;
                humedadS.dataGridView1.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

                if (sensors1.Count > 0)
                {
                    for (int i = 0; i < sensors1.Count; i++)
                    {
                        //Console.WriteLine("Sensors: " + sensors[i]);
                        humedadS.dataGridView1.Columns[i + 1].HeaderText = sensors1[i];
                    }
                }
            }
            return 0;
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = Go();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /*pbCalculationProgress.Value = e.ProgressPercentage;
            if (e.UserState != null)
                lbResults.Items.Add(e.UserState);*/
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result is SqlDataReader dr)
            {
                if (dr.GetValue(0).ToString() != "" && dr.GetValue(1).ToString() != "")
                {
                    Decimal sdp = Decimal.Parse(dr.GetValue(0).ToString());
                    Decimal.Round(sdp, 2).ToString();
                    Decimal cv = Decimal.Parse(dr.GetValue(1).ToString());
                    Decimal.Round(cv, 2).ToString();
                    SD.Text = "D. esta(datos crudos)= " + Decimal.Round(sdp, 2).ToString()
                        + " CV= " + Decimal.Round(cv, 2).ToString() + "%";

                    if (sensors1.Count > 0)
                    {
                        for (int i = 0; i < sensors1.Count; i++)
                        {
                            dataGridView1.Columns[i + 1].HeaderText = sensors1[i];
                        }
                    }
                }
            }
            ft.alert("Datos Recuperados");
            //dataGridView1.Refresh();
            //MessageBox.Show("Numbers between 0 and 10000 divisible by 7: " + e.Result);
            //dataGridView1.Refresh();
        }

        private void StBW()
        {
            try
            {
                BackgroundWorker worker = new BackgroundWorker();
                worker.WorkerReportsProgress = true;
                worker.DoWork += worker_DoWork;
                worker.ProgressChanged += worker_ProgressChanged;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                worker.RunWorkerAsync(10000);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void folio_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    index = int.Parse(folio.Text);
                    if (ft.IsValid(folio.Text) && (Rol == 5 || Rol == 4) && index != Usr.K)
                    {
                        //StBW();
                        Go();
                        //DoubleBuffered(dataGridView1, true);
                        if (humedadS != null)
                        {
                            humedadS.dataGridView1.Refresh();
                            //DoubleBuffered(humedadS.dataGridView1, true);
                        }
                        Usr.DateDif = 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void date2_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(date2);
        }

        private void FechaServicio_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(FechaServicio);
            if (ft.IsValid(FechaServicio.Text)
               && humedadS != null

               )
            {
                humedadS.FechaServicio.Text = FechaServicio.Text;
            }
        }

        private void FechaReporte_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(FechaReporte);
            if (ft.IsValid(FechaReporte.Text)
               && humedadS != null

               )
            {
                humedadS.FechaReporte.Text = FechaReporte.Text;
            }
        }

        private void InicioPrueba_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(InicioPrueba);
            if (humedadS != null)
            {
                humedadS.InicioPrueba.Text = InicioPrueba.Text;
            }
        }

        private void FinPrueba_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(FinPrueba);
            if (humedadS != null)
            {
                humedadS.FinPrueba.Text = FinPrueba.Text;
            }
        }

        private void Selected_Shown(object sender, EventArgs e)
        {
            try
            {
                if (humedadS != null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                //this.Hide();

                                humedadS.Show(this);
                                //frm.FormClosing += (s, o) => this.Show();
                            }));
                            //return;
                        }
                    });
                }
                if (sensors1.Count > 0)
                {
                    //SD.Text = "D. esta(datos crudos)= " + ft.getSD(index.ToString(), "Datos", "s", "SDP") + " CV= " + ft.getSD(index.ToString(), "Datos", "s", "CV") + "%";
                }
                //StBW();
                //Go();

                DoubleBuffered(dataGridView1, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void max_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(max); //cambia color de controles en caso de que esten vacios
        }

        private void min_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(min);
        }

        private void anexoG_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(anexoG);
        }

        private void encabezado_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(encabezado);
            DatosEnc.Text = encabezado.Text;
        }

        private void DatosEnc_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(DatosEnc);
        }

        private void anexo_TextChanged_1(object sender, EventArgs e)
        {
            ft.CoulorW(anexo);
        }

        private void minutes_ValueChanged(object sender, EventArgs e)
        {
            if (minutes.Value > 0)
            {
                ft.NumCheck(minutes);
                if (humedadS != null)
                {
                    humedadS.minutes.Value = minutes.Value;
                }
            }
            else
            {
                ft.NumCheck(minutes);
            }
        }

        private void Selected_Load(object sender, EventArgs e)
        {
            //Control.CheckForIllegalCrossThreadCalls = false;
            SwitchSens.Visible = false;
            DuplicateCB.Visible = false;
            DropSense.Visible = false;
            SX.Visible = false;
            SXD.Visible = false;
            SY.Visible = false;
            SYD.Visible = false;
            Drop.Visible = false;
            label21.Visible = false;
            label22.Visible = false;

            WindowState = FormWindowState.Maximized;
            dataGridView1.Location = new Point(0, 300);
            dataGridView1.Height = 400;
            dataGridView1.Width = 1080;
        }

        private void SwitchSens_CheckedChanged(object sender, EventArgs e)
        {
            if (SwitchSens.Checked)
            {
                SX.Enabled = true;
                SY.Enabled = true;
            }
            else
            {
                SX.Enabled = false;
                SY.Enabled = false;
            }
        }

        private void Switch_Click(object sender, EventArgs e)
        {
        }

        private void DropSense_CheckedChanged(object sender, EventArgs e)
        {
            if (DropSense.Checked)
            {
                Drop.Enabled = true;
            }
            else
            {
                Drop.Enabled = false;
            }
        }

        private void SetNl_Click(object sender, EventArgs e)
        {
        }

        private void DupB_Click(object sender, EventArgs e)
        {
        }

        private void DuplicateCB_CheckedChanged(object sender, EventArgs e)
        {
            if (DuplicateCB.Checked)
            {
                SXD.Enabled = true;
                SYD.Enabled = true;
            }
            else
            {
                SXD.Enabled = false;
                SYD.Enabled = false;
            }
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            /*
            try
            {
                DataTable changes = ((DataTable)dataGridView1.DataSource).GetChanges();
                if (changes != null)
                {
                    SqlCommandBuilder mcb = new SqlCommandBuilder(datos);
                    mySqlDataAdapter.UpdateCommand = mcb.GetUpdateCommand();
                    mySqlDataAdapter.Update(changes);
                    ((DataTable)dataGridView1.DataSource).AcceptChanges();

                    MessageBox.Show("Cell Updated");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        private void Selected_Enter(object sender, EventArgs e)
        {
        }

        private void Selected_MouseEnter(object sender, EventArgs e)
        {
            /*if (humedadS != null)
            {
                humedadS.WindowState = FormWindowState.Minimized;
            }*/
        }

        private void Selected_Leave(object sender, EventArgs e)
        {
        }

        private void Selected_MouseLeave(object sender, EventArgs e)
        {
        }

        private void id_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Selected_Click(object sender, EventArgs e)
        {
            if (humedadS != null)
            {
                humedadS.WindowState = FormWindowState.Minimized;
            }
        }

        private void SY_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    if (
                (SX.Text.Contains("s") || SX.Text.Contains("S"))
                && (SY.Text.Contains("s") || SY.Text.Contains("S"))
                )
                    {
                        string sql =
                                "merge Datos as SensSwitch "
                                + "using (select " + SX.Text + ", tiempo, id from Datos where id = " + index + " ) as sx "
                                + "on SensSwitch.Tiempo = Sx.Tiempo and Sensswitch.id = Sx.id "
                                + "when Matched then "
                                + "update set Sensswitch.s40 = sx." + SX.Text + "; "

                                + "merge Datos as SensSwitch "
                                + "using (select " + SY.Text + ", tiempo, id from Datos where id = " + index + " ) as sy "
                                + "on SensSwitch.Tiempo = Sy.Tiempo and Sensswitch.id = Sy.id "
                                + "when Matched then "
                                + "update set Sensswitch." + SX.Text + " = sy." + SY.Text + "; "

                                + "merge Datos as SensSwitch "
                                + "using (select s40, tiempo, id from Datos where id = " + index + " ) as ss "
                                + "on SensSwitch.Tiempo = ss.Tiempo and Sensswitch.id = ss.id "
                                + "when Matched then "
                                + "update set Sensswitch." + SY.Text + " = ss.s40; "

                                + "update Datos set S40 = null where id = " + index;
                        if (ft.SetSql(sql))
                        {
                            try
                            {
                                //StBW();
                                Go();
                                //DoubleBuffered(dataGridView1, true);
                                if (humedadS != null)
                                {
                                    humedadS.dataGridView1.Refresh();
                                    //DoubleBuffered(humedadS.dataGridView1, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void SYD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    if (
               (SXD.Text.Contains("s") || SXD.Text.Contains("S"))
               && (SYD.Text.Contains("s") || SYD.Text.Contains("S"))
               )
                    {
                        string sql = "update datos set " + SYD.Text + "= " + SXD.Text + " where id=" + index;
                        if (ft.SetSql(sql))
                        {
                            try
                            {
                                //StBW();
                                Go();
                                //DoubleBuffered(dataGridView1, true);
                                if (humedadS != null)
                                {
                                    humedadS.dataGridView1.Refresh();
                                    //DoubleBuffered(humedadS.dataGridView1, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void Drop_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    if (
                 (Drop.Text.Contains("s") || Drop.Text.Contains("S"))

                 )
                    {
                        string sql = "update datos set " + Drop.Text + "= null where id=" + index;
                        if (ft.SetSql(sql))
                        {
                            try
                            {
                                //StBW();
                                Go();
                                //DoubleBuffered(dataGridView1, true);
                                if (humedadS != null)
                                {
                                    humedadS.dataGridView1.Refresh();
                                    //DoubleBuffered(humedadS.dataGridView1, true);
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (SwitchSens.Visible)
            {
                SwitchSens.Visible = false;
                DuplicateCB.Visible = false;
                DropSense.Visible = false;
                SX.Visible = false;
                SXD.Visible = false;
                SY.Visible = false;
                SYD.Visible = false;
                Drop.Visible = false;
                label21.Visible = false;
                label22.Visible = false;
            }
            else
            {
                SwitchSens.Visible = true;
                DuplicateCB.Visible = true;
                DropSense.Visible = true;
                SX.Visible = true;
                SXD.Visible = true;
                SY.Visible = true;
                SYD.Visible = true;
                Drop.Visible = true;
                label21.Visible = true;
                label22.Visible = true;
            }
        }

        private void validatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void distribucionToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}