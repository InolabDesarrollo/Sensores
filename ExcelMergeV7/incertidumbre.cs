using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    //LOOOK INTO FR PARAMETER GETTING FROM SELECTED SENDED TO REPORT
    public partial class incertidumbre : Form
    {
        private int index, numSensores, minuten, hum, fom1;
        private string maxi, mini, date1i, date2i, idRi;
        private string folioR, anexoR, noSerie, FServicio, FReporte;
        private string inicio, final;

        private List<string> sensors1;

        private void incertidumbre_FormClosing(object sender, FormClosingEventArgs e)
        {
            GC.Collect();
            //ft.incer = 0;
            //ft.alert(ft.incer.ToString());
        }

        private Functions ft = new Functions();

        public incertidumbre()
        {
            InitializeComponent();
        }

        public incertidumbre(int ind, string max, string min, string date1, string date2, int minutes, int hume, string Fservicio, string Freporte, int fom, string folio, string idR, string anexo, List<string> sensors, string NoSerie, string Inicio, string Fin)
        {
            Console.WriteLine("Incertidumbre?");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            InitializeComponent();
            inicio = Inicio;
            final = Fin;
            maxi = max;
            mini = min;
            date1i = date1;
            date2i = date2;
            index = ind;
            minuten = minutes;
            FServicio = Fservicio;
            FReporte = Freporte;
            idRi = idR;
            folioR = folio;
            anexoR = anexo;
            fom1 = fom;
            sensors1 = sensors;
            noSerie = NoSerie;
            string L = "S";
            hum = hume;
            if (ft.IsValid(Usr.proporcion))
            {
                prop.Text = Usr.proporcion;
            }
            if (ft.IsValid(Usr.offset))
            {
                off.Text = Usr.offset;
            }
            Console.WriteLine("DETAILS Date1: " + date1i + " Date2 " + date2i);
            Console.WriteLine("Hum " + hum);
            Console.WriteLine("fom " + fom1);
            if (fom1 == 2)
            {
                this.Text = "Incertidumbre Humedad";
                this.BackColor = Color.FromArgb(241, 241, 241);
            }
            if (hum != 0)
            {
                L = "H";
            }
            try
            {
                numSensores = ft.GetSensors(index);

                ft.GetSensorsW(this, index, numSensores, L, sensors1);
                int i = 0;
                if (Usr.Incertidumbre.Count > 0)
                {
                    foreach (Control control in this.Controls)
                    {
                        if (control is TextBox && control.Name != "prop" && control.Name != "off" && i < Usr.Incertidumbre.Count)
                        {
                            control.Text = Usr.Incertidumbre[i];
                            i++;
                        }
                    }
                    Usr.Incertidumbre.Clear();
                }
                //this.Refresh();
                this.Show();

                this.Invalidate();
                this.Update();
                label1.Text = "Ejemplos de Formato de Incertidumbre: " +
                    "0.1 | -0.1 | +0.5 ";
            }
            catch (SqlException sqle)
            {
                MessageBox.Show("Error Número: " + sqle.Number.ToString() + " SQL/TCP Exception");
                MessageBox.Show(sqle.ToString());
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Aplicar_Click(object sender, EventArgs e)
        {
            if (ft.MatchFloat(prop.Text) && ft.MatchFloat(off.Text))
            {
                Usr.proporcion = prop.Text;
                Usr.offset = off.Text;

                string sql = "";
                if (hum == 0)
                {
                    sql = ft.GetQueryIncertidumbre(this, index, numSensores);//Genera el sql para acentar los cambios en la tabla incertidumbre
                }
                else
                {
                    ft.GetQueryIncertidumbreHumedad(this, index, numSensores);//Genera el sql para acentar los cambios en la tabla incertidumbreHumedad
                }

                try
                {
                    if (sql != "0")
                    {
                        if (ft.SetSql(sql) && !String.IsNullOrEmpty(maxi) && !String.IsNullOrEmpty(mini) && !String.IsNullOrEmpty(date1i) && !String.IsNullOrEmpty(date2i))
                        {
                            ft.alert("Construyendo Vista...");
                            ShowIncertidumbre inc =
                                new ShowIncertidumbre(index, maxi, mini, date1i, date2i,
                                minuten, FServicio, FReporte, idRi, folioR, anexoR, hum,
                                fom1, sensors1, noSerie, inicio, final);
                            inc.Show();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Ha Ocurrido Un Problema Con La Base De Datos.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se ha podido Realizar La Operacion Deseada");
                    }
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show("Error Número: " + sqle.Number.ToString() + " SQL/TCP Exception \n o error de formato en algún sensor ");
                    //MessageBox.Show(sqle.ToString());
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }
    }
}