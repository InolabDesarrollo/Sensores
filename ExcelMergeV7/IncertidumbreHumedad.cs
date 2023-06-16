using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class IncertidumbreHumedad : Form
    {
        //private const int CP_NOCLOSE_BUTTON = 0x200;
        /* protected override CreateParams CreateParams
         {
             get
             {
                 CreateParams myCp = base.CreateParams;
                 myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                 return myCp;
             }
         }*/
        private int index, numSensores, minuten, hum, fom1;

        private string maxi, mini, date1i, date2i,
            idRi, folioR, anexoR, NoSerieR, FServicio,
            Fechaserv, FechaRep, FReporte, inicio, final;

        private List<string> sensors1;

        private void IncertidumbreHumedad_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                ft.incer = 0;
                IncertidumbreHumedad inc = new IncertidumbreHumedad(index, maxi, mini, date1i, date2i,
                    minuten, hum, Fechaserv, FReporte, 2,
                    folioR, idRi, anexoR, sensors1, NoSerieR, inicio, final);

                DialogResult dialogResult = MessageBox.Show("¿Seguro que desea salir? Todo proceso no guardado será eliminado", "", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    //Clean DB
                    if (ft.DeleteTable(index, "IncertidumbreHumedad"))
                    {
                        //start.Show();
                        inc.Show();
                    }
                    else
                    {
                        MessageBox.Show("Ha ocurrido un problema al intentar vaciar los datos. ");
                    }

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

        public IncertidumbreHumedad()
        {
            InitializeComponent();
        }

        private Functions ft = new Functions();

        public IncertidumbreHumedad(int ind, string max, string min, string date1, string date2, int minutes, int hume, string Fservicio, string Freporte, int fom, string folio, string idR, string anexo, List<string> sensors, string NoSerie, string Inicio, string Final)
        {
            Console.WriteLine("Incertidumbre?");
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            InitializeComponent();
            inicio = Inicio;
            final = Final;
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
            FechaRep = Freporte;
            Fechaserv = Fservicio;
            sensors1 = sensors;
            NoSerieR = NoSerie;
            string L = "S";
            hum = hume;
            if (ft.IsValid(Usr.proporcionH))
            {
                prop.Text = Usr.proporcionH;
            }
            if (ft.IsValid(Usr.offsetH))
            {
                off.Text = Usr.offsetH;
            }
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
                //Add Sensors
                ft.GetSensorsW(this, index, numSensores, L, sensors1);
                int i = 0;
                if (Usr.IncertidumbreH.Count > 0)
                {
                    foreach (Control control in this.Controls)
                    {
                        if (control is TextBox && control.Name != "prop" && control.Name != "off" && i < Usr.IncertidumbreH.Count)
                        {
                            control.Text = Usr.IncertidumbreH[i];
                            i++;
                        }
                    }
                    Usr.IncertidumbreH.Clear();
                }
                //this.Refresh();
                this.Show();

                this.Invalidate();
                this.Update();
                label1.Text = "Ejemplos de Formato de Incertidumbre: " +
                    "0.1 | -0.1 | +0.5 ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void aplicar_Click(object sender, EventArgs e)
        {
            Usr.proporcionH = prop.Text;
            Usr.offsetH = off.Text;
            string sql = "";
            if (hum == 0)
            {
                sql = ft.GetQueryIncertidumbre(this, index, numSensores);
                //MessageBox.Show(sql+" "+hum+" GETSQL");
            }
            else
            {
                //sql=ft.GetSql1(this, index, numSensores,ft.Date2(ft.ToDate(date1i)), ft.Date2(ft.ToDate(date2i)));
                sql = ft.GetQueryIncertidumbreHumedad(this, index, numSensores);

                //MessageBox.Show(sql + " " + hum + " GETSQL1");
            }
            //MessageBox.Show(sql);
            Console.WriteLine(sql);
            Console.WriteLine("MAX: " + maxi);
            Console.WriteLine("MIN: " + mini);
            Console.WriteLine("Date1: " + date1i);
            Console.WriteLine("Date2: " + date2i);
            Console.WriteLine("FOM: " + fom1);
            Console.WriteLine("HUM " + hum);

            try
            {
                if (sql != "0")
                {
                    if (ft.SetSql(sql) && !String.IsNullOrEmpty(maxi) && !String.IsNullOrEmpty(mini) && !String.IsNullOrEmpty(date1i) && !String.IsNullOrEmpty(date2i))
                    {
                        //ToNextForm

                        ft.alert("Construyendo Vista...");
                        //Add Sensors to this show incertidumbre and incertidumbre humedad
                        Console.WriteLine("HUM: " + hum);
                        ShowIncertidumbre inc = new ShowIncertidumbre(index, maxi, mini, date1i, date2i,
                            minuten, FServicio, FReporte, idRi, folioR,
                            anexoR, hum, fom1, sensors1, NoSerieR, inicio, final);
                        inc.Show();

                        this.Close();
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}