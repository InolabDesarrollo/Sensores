using System;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class Reporte2 : Form
    {
        private Functions ft = new Functions();
        private Selected selected = new Selected();

        public Reporte2()
        {
            InitializeComponent();
        }

        public Reporte2(int id, string max, string min, string date1, string date2, string r, string minuten, string fechareporte, string fechaservicio, string folio, string anexo, string idR)
        {
            InitializeComponent();

            try
            {
                Console.WriteLine("REPORTING R " + r);
                //MessageBox.Show(ft.DateFormat(ft.ToDate(date1))+ ft.DateFormat(ft.ToDate(date2))+" max: "+max+" min: "+min);
                if (r == "incertidumbre")
                {
                    this.Text = "Incertidumbre";
                    ft.verDatosReales(reportViewer1, id, max, min, date1, date2, minuten, fechareporte, fechaservicio, folio, anexo, idR, "DatosReales");
                }
                else if (r == "datos")
                {
                    this.Text = "Datos";
                    ft.verDatos(reportViewer1, id, max, min, date1, date2, minuten, fechareporte, fechaservicio, folio, anexo, idR, "TablaSensor_1");
                    //ft.verDatos(reportViewer1, id, max, min, date1, date2, minuten, fechareporte, fechaservicio, folio, anexo, idR, "TablaSensor");
                }
                else if (r == "incertidumbreHumedad")
                {
                    this.Text = "Incertidumbre Humedad";

                    ft.verDatosReales(reportViewer1, id, max, min, date1, date2, minuten, fechareporte, fechaservicio, folio, anexo, idR, "DatosRealesHumedad");
                }
                else
                {
                    Console.WriteLine("DatosHumedad: " + r);
                    this.Text = "Datos Humedad";
                    ft.verDatos(reportViewer1, id, max, min, date1, date2, minuten, fechareporte, fechaservicio, folio, anexo, idR, "TablaSensorHumedad_1");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Reporte2_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void Reporte2_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}