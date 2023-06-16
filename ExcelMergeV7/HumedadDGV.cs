using ExcelMergeV7.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class HumedadDGV : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public new static void DoubleBuffered(DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered",
                  BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }

        public string Data { get => data; set => data = value; }

        private string data;
        public int index; private int hum;
        private List<string> sensors1;

        public HumedadDGV()
        {
            InitializeComponent();
        }

        public HumedadDGV(int index1, int humed, List<string> sensors)
        {
            try
            {
                Console.WriteLine("Humedad Started");
                InitializeComponent();

                index = index1;
                Console.WriteLine(index);
                hum = humed;
                sensors1 = sensors;
                //dataGridView1.DataBindings.Add();
                /*Datos2TableAdapter datos = new Datos2TableAdapter();
                dataGridView1.DataSource = datos.GetDataBy(index);
                dataGridView1.Columns[0].Width = 149;
                dataGridView1.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                for (int i = 0; i < sensors.Count; i++)
                {
                    //Console.WriteLine("Sensors: " + sensors[i]);
                    dataGridView1.Columns[i + 1].HeaderText = sensors1[i];
                }
                */

                //SD.Text = "D.esta= " + ft.getSD(index1.ToString(), "Datos", "H", "SDP") + "CV: " + ft.getSD(index1.ToString(), "Datos", "H", "CV") + "%";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private Functions ft = new Functions();

        private void cargar_Click(object sender, EventArgs e)
        {
        }

        private void Selected_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("¿Seguro que desea salir? Todo proceso no guardado será eliminado", "", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    //Clean DB

                    if (ft.DeleteTable(index, "Datos"))
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
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void criterio_TextChanged_1(object sender, EventArgs e)
        {
            ft.CoulorW(criterio);
            referencia.Text = criterio.Text;
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

        private void margin_TextChanged_1(object sender, EventArgs e)
        {
            ft.CoulorW(margin);
        }

        private void hours_ValueChanged_1(object sender, EventArgs e)
        {
            date1.Text = Data;
            if (hours.Value != 0 && !String.IsNullOrEmpty(date1.Text))
            {
                date2.Text = ft.DateFormat(ft.ToDate(date1.Text).AddHours(double.Parse(hours.Value.ToString())));
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dataGridView1.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void HumedadDGV_FormClosing(object sender, FormClosingEventArgs e)
        {
            //e.Cancel = true;
        }

        private IncertidumbreHumedad options;

        private void graph_Click_1(object sender, EventArgs e)
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
                   )
                {
                    string sql = "Insert into Parametros(Id,EncabezadoGCrudos," +
                           "Folio,Id_t,DateServicio,DateEmision,NoSerie,Anexo,max,min)" +
                                 " values("
                              + index + ",'" + encabezado.Text + "'," +
                              "'" + folio.Text + "','" + id.Text + "','"
                              + FechaServicio.Text + "','" +
                              FechaReporte.Text + "','" +
                              NoSerie.Text + "','" + anexoG.Text + "'," + max.Text + "," + min.Text + ")";
                    if (ft.DeleteTable(index, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            ft.alert("Creando Gráfica");

                            Reporte reporte = new Reporte(index, 1, encabezado.Text);
                            reporte.Show();
                            Reporte reporte1 = new Reporte(index, 5, encabezado.Text);
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

                            Reporte reporte = new Reporte(index, 1, encabezado.Text);
                            reporte.Show();
                            Reporte reporte1 = new Reporte(index, 5, encabezado.Text);
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
        }

        private void reporte_Click_1(object sender, EventArgs e)
        {
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
                            SD_dts.Text = "D.esta= " + ft.getSDdates(index.ToString(), "Datos", "h", "SDP") + "   CV= " + ft.getSDdates(index.ToString(), "Datos", "H", "CV") + "%";
                            //uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text);
                            if (ft.MatchFloat(referencia.Text))
                            {
                                Reporte2 rep = new Reporte2(index, max.Text, min.Text, date1.Text, date2.Text, "DatosHumedad", minutes.Value.ToString(), FechaReporte.Text, FechaServicio.Text, folio.Text, anexo.Text, id.Text);
                                rep.Show();

                                //uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, 1);
                                //uni.Show();
                                uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datosHumedad", sensors1.Count, referencia.Text);
                                uni.Show();
                                uniformidadV1 uniV1 = new uniformidadV1(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datosV1Humedad", sensors1.Count, referencia.Text);
                                uniV1.Show();
                            }
                            else
                            {
                                ft.alert("Verifique el formato  de la temperatura de referenca.");
                                referencia.Focus();
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        if (ft.SetSql(sql))
                        {
                            SD_dts.Text = "D. esta= " + ft.getSDdates(index.ToString(), "Datos", "h", "SDP") + "   CV= " + ft.getSDdates(index.ToString(), "Datos", "H", "CV") + "%";
                            //uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text);
                            if (ft.MatchFloat(referencia.Text))
                            {
                                Reporte2 rep = new Reporte2(index, max.Text, min.Text, date1.Text, date2.Text, "DatosHumedad", minutes.Value.ToString(), FechaReporte.Text, FechaServicio.Text, folio.Text, anexo.Text, id.Text);
                                rep.Show();
                                Console.WriteLine("HUME SEND TO uniformidad.cs " + hum);
                                //uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, 1);
                                //uni.Show();
                                uniformidad uni = new uniformidad(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datosHumedad", sensors1.Count, referencia.Text);
                                uni.Show();
                                uniformidadV1 uniV1 = new uniformidadV1(index, date1.Text, date2.Text, minutes.Value.ToString(), max.Text, min.Text, "datosV1Humedad", sensors1.Count, referencia.Text);
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

                    SqlDataReader rr = ft.getSD(index.ToString(), "Datos", "H");
                    if (rr.GetValue(0).ToString() != "" && rr.GetValue(1).ToString() != "")
                    {
                        Decimal sdp = Decimal.Parse(rr.GetValue(0).ToString());
                        Decimal.Round(sdp, 2).ToString();
                        Decimal cv = Decimal.Parse(rr.GetValue(1).ToString());
                        Decimal.Round(cv, 2).ToString();
                        SD.Text = "D. esta(datos crudos)= " + Decimal.Round(sdp, 2).ToString()
                            + " CV= " + Decimal.Round(cv, 2).ToString() + "%";
                    };
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

        private void anexoG_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(anexoG);
        }

        private void label18_Click(object sender, EventArgs e)
        {
        }

        private void apertura_Click(object sender, EventArgs e)
        {
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
                         + index + ",'" + ft.Date2(ft.ToDate(date1.Text)) + "','" + ft.Date2(ft.ToDate(date2.Text)) + "'," +
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
                                "Humedad", sensors1

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
                                "Humedad", sensors1

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

        private void margin_KeyPress(object sender, KeyPressEventArgs e) //valida formato de datos
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Tab))
            {
                try
                {
                    if (!String.IsNullOrEmpty(criterio.Text) && !String.IsNullOrEmpty(margin.Text))
                    {
                        if (ft.MatchFloat(margin.Text))
                        {
                            Decimal cC = Decimal.Parse(criterio.Text);
                            Decimal margen = Decimal.Parse(margin.Text);

                            max.Text = (cC + margen).ToString();
                            min.Text = (cC - margen).ToString();

                            dataGridView1.DataSource = null;
                            Datos2TableAdapter datos = new Datos2TableAdapter();
                            dataGridView1.DataSource = datos.GetDataBy(index);
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (ft.IsValid(max.Text) && ft.IsValid(min.Text))
            {
                ft.PaintDGV(dataGridView1, float.Parse(max.Text), float.Parse(min.Text));
            }
        }

        private void date1_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(date1);
        }

        private void date2_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(date2);
        }

        private void FechaServicio_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(FechaServicio);
        }

        private void FechaReporte_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(FechaReporte);
        }

        private void InicioPrueba_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(InicioPrueba);
        }

        private void FinPrueba_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorWDate(FinPrueba);
        }

        private void min_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(min);
        }

        private void max_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(max);
        }

        private void anexo_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(anexo);
        }

        private void encabezado_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(encabezado);
        }

        private void DatosEnc_TextChanged(object sender, EventArgs e)
        {
            ft.CoulorW(DatosEnc);
        }

        private void incertidumbre_Click(object sender, EventArgs e)
        {
            try
            {
                if (ft.MatchFloat(max.Text) && ft.MatchFloat(min.Text)
                   && ft.isDate(date1.Text) && ft.isDate(date2.Text)
                   && hours.Value != 0 && minutes.Value != 0
                   && ft.IsValid(folio.Text) && ft.IsValid(id.Text) && ft.IsValid(anexo.Text)
                   && ft.isDate(FechaReporte.Text)
                   && ft.isDate(FechaServicio.Text)
                   //&& ft.isDate(InicioPrueba.Text)
                   //&& ft.isDate(FinPrueba.Text)

                   )
                {
                    string st = ft.Format(InicioPrueba.Text), end = ft.Format(FinPrueba.Text);
                    string sql = "delete from incertidumbreHumedad where id=" + index + ";" +
                        "insert into Parametros(id,f1,f2,inicio,fin)" +
                        " values('" + index + "','" + ft.Date2(ft.ToDate(date1.Text)) +
                        "','" + ft.Date2(ft.ToDate(date2.Text)) +
                        "'," + st + "," + end + ")";
                    if (ft.DeleteTable(index, "Parametros"))
                    {
                        if (ft.SetSql(sql))
                        {
                            if (options == null && ft.GetIdIncertidumbre(index.ToString(), "IncertidumbreHumedad"))
                            {
                                options = new IncertidumbreHumedad(index, max.Text, min.Text, date1.Text, date2.Text,
                                    int.Parse(minutes.Value.ToString()), hum, FechaServicio.Text, FechaReporte.Text, 1,
                                    folio.Text, id.Text, anexo.Text, sensors1, NoSerie.Text, InicioPrueba.Text, FinPrueba.Text);

                                options.FormClosed += (o, ea) => options = null;
                                options.Show();
                            }
                            else
                            {
                                Console.WriteLine("FirstELse");
                                if (ft.GetIdIncertidumbre(index.ToString(), "IncertidumbreHumedad"))
                                {
                                    options = new IncertidumbreHumedad(index, max.Text, min.Text, date1.Text, date2.Text,
                                    int.Parse(minutes.Value.ToString()), hum, FechaServicio.Text, FechaReporte.Text, 1,
                                    folio.Text, id.Text, anexo.Text, sensors1, NoSerie.Text, InicioPrueba.Text, FinPrueba.Text);

                                    options.FormClosed += (o, ea) => options = null;
                                    options.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Cierre primero la ventana de incertidumbre(Humedad).");
                                }
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
                            if (options == null && ft.GetIdIncertidumbre(index.ToString(), "Incertidumbrehumedad"))
                            {
                                options = new IncertidumbreHumedad(index, max.Text, min.Text, date1.Text, date2.Text,
                                    int.Parse(minutes.Value.ToString()), hum, FechaServicio.Text, FechaReporte.Text, 1,
                                    folio.Text, id.Text, anexo.Text, sensors1, NoSerie.Text, InicioPrueba.Text, FinPrueba.Text);

                                options.FormClosed += (o, ea) => options = null;
                                options.Show();
                            }
                            else
                            {
                                Console.WriteLine("SecondElse");
                                if (ft.GetIdIncertidumbre(index.ToString(), "IncertidumbreHumedad"))
                                {
                                    options = new IncertidumbreHumedad(index, max.Text, min.Text, date1.Text, date2.Text,
                                    int.Parse(minutes.Value.ToString()), hum, FechaServicio.Text, FechaReporte.Text, 1,
                                    folio.Text, id.Text, anexo.Text, sensors1, NoSerie.Text, InicioPrueba.Text, FinPrueba.Text);

                                    options.FormClosed += (o, ea) => options = null;
                                    options.Show();
                                }
                                else
                                {
                                    MessageBox.Show("Cierre primero la ventana de incertidumbre(Temperatura).");
                                }
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

        private void HumedadDGV_Shown(object sender, EventArgs e)
        {
            DoubleBuffered(dataGridView1, true);
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

        private void Switch_Click(object sender, EventArgs e)
        {
        }

        private void DupB_Click(object sender, EventArgs e)
        {
        }

        private void SetNl_Click(object sender, EventArgs e)
        {
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
                        SX.Text = SX.Text.Substring(1);
                        SX.Text = "H" + SX.Text;

                        SY.Text = SY.Text.Substring(1);
                        SY.Text = "H" + SY.Text;
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
                                Selected wf = (Selected)this.Owner;
                                wf.Go();
                                //DoubleBuffered(dataGridView1, true);
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
                        SXD.Text = SXD.Text.Substring(1);
                        SXD.Text = "H" + SXD.Text;

                        SYD.Text = SYD.Text.Substring(1);
                        SYD.Text = "H" + SYD.Text;
                        string sql = "update datos set " + SYD.Text + "= " + SXD.Text + " where id=" + index;
                        if (ft.SetSql(sql))
                        {
                            try
                            {
                                //StBW();
                                Selected wf = (Selected)this.Owner;
                                wf.Go();
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
                        Drop.Text = Drop.Text.Substring(1);
                        Drop.Text = "H" + Drop.Text;

                        string sql = "update datos set " + Drop.Text + "= null where id=" + index;
                        if (ft.SetSql(sql))
                        {
                            try
                            {
                                //StBW();
                                Selected wf = (Selected)this.Owner;
                                wf.Go();
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

        private void HumedadDGV_Load(object sender, EventArgs e)
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
    }
}