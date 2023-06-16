using System;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class uniformidadV1 : Form
    {
        public uniformidadV1()
        {
            InitializeComponent();
        }

        private void uniformidadV1_Load(object sender, EventArgs e)
        {
            this.Rep.RefreshReport();
            this.Rep.RefreshReport();
        }

        private Functions ft = new Functions();

        public uniformidadV1(int id, string date1, string date2, string minuten, string max, string min, string hum, int NumeroDeSensores, string TemperaturaReferencia)
        {
            InitializeComponent();
            Console.WriteLine("UniformidadV1 \nHume: " + hum);

            if (hum == "datos")
            {
                //ft.verUniformidad(reportViewer1, id, date1, date2, minuten, max, min);
                ft.verUniformidad(Rep, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadDatos");
            }
            else if (hum == "incertidumbreHumedad")
            {
                //ft.verUniformidadHumedad(reportViewer1,id,date1,date2,minuten,max,min);
                ft.verUniformidad(Rep, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadRealesHumedadV1");
            }
            else if (hum == "datosV1")
            {
                this.Text = "Max, Min, Estabilidad Datos";
                ft.verUniformidad(Rep, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadDatosV1_1");
                //ft.verUniformidad(Rep, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadDatosV1");
            }
            else if (hum == "datosV1Humedad")
            {
                Console.WriteLine("DatosHumedadV1");
                this.Text = "Max, Min, Estabilidad Humedad";
                ft.verUniformidad(Rep, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadHumedadV1_1");
            }
            else
            {
                this.Text = "Max, Min, Estabilidad Incertidumbre";
                ft.verUniformidad(Rep, id, date1, date2, minuten, max, min, NumeroDeSensores, Decimal.Parse(TemperaturaReferencia), "UniformidadIncertidumbreV1_1");
            }
        }
    }
}