using System;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class uniformidad : Form
    {
        public uniformidad()
        {
            InitializeComponent();
        }

        private Functions ft = new Functions();

        public uniformidad(int id, string date1, string date2, string minuten, string max, string min, string hum, int NumeroDeSensores, string TemperaturaReferencia)
        {
            Console.WriteLine("Uniformidad \nHume: " + hum);
            InitializeComponent();

            if (hum == "datos")
            {
                ft.verUniformidad(reportViewer1, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadDatos_1");
            }
            else if (hum == "incertidumbreHumedad")
            {
                ft.verUniformidad(reportViewer1, id, date1, date2, minuten, max, min, Usr.IncertidumbreH.Count, Decimal.Parse(TemperaturaReferencia), "UniformidadIncertidumbreHumedad_1");
                //ft.verUniformidad(reportViewer1, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadIncertidumbreV1_1");
            }
            else if (hum == "datosV1")
            {
                //ft.verUniformidad(reportViewer1, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadDatosV1");
            }
            else if (hum == "datosHumedad")
            {
                this.Text = "Uniformidad Humedad";
                ft.verUniformidad(reportViewer1, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadHumedad_1");
            }
            else
            {
                this.Text = "Uniformidad Incertidumbre";
                ft.verUniformidad(reportViewer1, id, date1, date2, minuten, max, min, Usr.Incertidumbre.Count, Decimal.Parse(TemperaturaReferencia), "UniformidadIncertidumbre_1");
            }
        }

        private void uniformidad_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }
    }
}