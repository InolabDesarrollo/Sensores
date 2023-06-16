using Microsoft.Reporting.WinForms;
using System;
using System.Net;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class Reporte : Form
    {
        private Selected selected = new Selected();

        public Reporte()
        {
            InitializeComponent();
        }

        public Reporte(int id, int hum, string encabezado)
        {
            InitializeComponent();
            try
            {
                if (hum == 1)
                {
                    this.Text = "Gráfica Humedad";
                    verHumCrudos(id, encabezado);
                }
                else if (hum == 2)
                {
                    this.Text = "Gráfica promedios";//Crudos
                    verPromedios(id, encabezado, "GraficaPromedioCrudos");
                }
                else if (hum == 3)
                {
                    this.Text = "Gráfica promedios";//corregidos
                    verPromedios(id, encabezado, "GraficaPromediosCorregidos");
                }
                else if (hum == 4)
                {
                    this.Text = "Gráfica promedios";//corregidosHumedad
                    verPromedios(id, encabezado, "GraficaPromediosCorrgidosHumedad");
                }
                else if (hum == 5)
                {
                    this.Text = "Gráfica promedios";//corregidosHumedad
                    verPromedios(id, encabezado, "GraficaPromediosHumedad");
                }
                else
                {
                    this.Text = "Gráfica Temperatura";
                    verCrudos(id, encabezado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public Reporte(int id, string reporte, int minutes, string max, string min)
        {
            InitializeComponent();
            Console.WriteLine("Rep: " + reporte);
            ft.verApertura(reportViewer1, id, reporte, minutes, max, min);
            if (reporte == "Apertura")
            {
                this.Text = "Apertura Temperatura";
            }
            else if (reporte == "AperturaHumedad")
            {
                this.Text = "Apertura Humedad";
            }
            else if (reporte == "AperturaIncertidumbre")
            {
                this.Text = "Apertura Incertidumbre";
            }
            else if (reporte == "AperturaIncertidumbreHumedad")
            {
                this.Text = "Apertura Incertidumbre Humedad";
            }
            else if (reporte == "GApertura")
            {
                this.Text = "Gráfica Apertura";
            }
            else if (reporte == "GAperturaHumedad")
            {
                this.Text = "Gráfica Apertura Humedad";
            }
            else if (reporte == "GAperturaIncertidumbre")
            {
                this.Text = "Gráfica Apertura Incertidumbre";
            }
            else if (reporte == "GAperturaIncertidumbreHumedad")
            {
                this.Text = "Gráfica Apertura Incertidumbre Humedad";
            }
        }

        public Reporte(int id, int type, string max, string min, string date1, string date2, string minuten, string encabezado, string grafica)
        {
            InitializeComponent();
            try
            {
                if (type == 0)
                {
                    this.Text = "Gráfica Datos Crudos";
                    verCrudos(id);
                }
                else
                {
                    if (grafica == "DatosCorregidosHumedad")
                    {
                        this.Text = "Gráfica Humedad Datos Corregidos";
                        VerGReales(reportViewer1, id, max, min, date1, date2, minuten, encabezado, "GraficaDatosCorregidosHumedadEncabezado");
                    }
                    else
                    {
                        this.Text = "Gráfica Datos Corregidos";
                        VerGReales(reportViewer1, id, max, min, date1, date2, minuten, encabezado, "GraficaDatosCorregidosEncabezado");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private Functions ft = new Functions();

        public void verCrudos(int id, string encabezado)
        {
            Console.WriteLine("encabezado VerCrudos Guardado");
            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");

            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(ft.connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/GraficaSensorEncabezado";
            reportViewer1.ShowParameterPrompts = false;
            ReportParameter idP = new ReportParameter();
            idP.Name = "id";
            idP.Values.Add(id.ToString());
            ReportParameter encP = new ReportParameter();
            encP.Name = "encabezado";
            encP.Values.Add(encabezado);

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { idP });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { encP });
            reportViewer1.RefreshReport();
        }

        public void verPromedios(int id, string encabezado, string Reporte)
        {
            Console.WriteLine("encabezado VerCrudos Guardado");
            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");

            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(ft.connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/" + Reporte;
            reportViewer1.ShowParameterPrompts = false;
            ReportParameter idP = new ReportParameter();
            idP.Name = "id";
            idP.Values.Add(id.ToString());
            ReportParameter encP = new ReportParameter();
            encP.Name = "encabezado";
            encP.Values.Add(encabezado);

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { idP });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { encP });
            reportViewer1.RefreshReport();
        }

        public void verCrudos(int id)
        {
            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");

            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(ft.connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/GraficaSensorEncabezado";
            reportViewer1.ShowParameterPrompts = false;
            ReportParameter idP = new ReportParameter();
            idP.Name = "id";
            idP.Values.Add(id.ToString());

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { idP });

            reportViewer1.RefreshReport();
        }

        public void verHumCrudos(int id, string encabezado)
        {
            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");

            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(ft.connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/GraficasDatosHumedadEncabezado";
            reportViewer1.ShowParameterPrompts = false;
            ReportParameter idP = new ReportParameter();
            idP.Name = "id";
            idP.Values.Add(id.ToString());
            ReportParameter encP = new ReportParameter();
            encP.Name = "encabezado";
            Console.WriteLine(encabezado);
            encP.Values.Add(encabezado);

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { idP });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { encP });
            reportViewer1.RefreshReport();
        }

        public void VerGReales(ReportViewer reportViewer1, int id, string max, string min, string date1, string date2, string minuten, string encabezado, string grafica)
        {
            Functions ft = new Functions();
            Console.WriteLine("****************VerGReales*************");
            Console.WriteLine("Id: " + id);
            Console.WriteLine("Date1: " + date1);
            Console.WriteLine("Date2: " + date2);
            Console.WriteLine("minuten: " + minuten);
            Console.WriteLine("Max: " + max);
            Console.WriteLine("min: " + min);

            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");
            //MessageBox.Show("DatosReales");
            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(ft.connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/" + grafica;
            reportViewer1.ShowParameterPrompts = false;
            ReportParameter parameter = new ReportParameter();
            parameter.Name = "id";
            parameter.Values.Add(id.ToString());
            ReportParameter parameter0 = new ReportParameter();
            parameter0.Name = "max";
            parameter0.Values.Add(max);
            ReportParameter parameter1 = new ReportParameter();
            parameter1.Name = "min";
            //MessageBox.Show(min);
            parameter1.Values.Add(min);

            ReportParameter parameter4 = new ReportParameter();
            parameter4.Name = "minuto";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter4.Values.Add(minuten);
            ReportParameter parameter5 = new ReportParameter();
            parameter5.Name = "encabezado";
            //MessageBox.Show(encabezado);
            parameter5.Values.Add(encabezado);

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter0 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter1 });

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter4 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter5 });

            reportViewer1.RefreshReport();

            Console.WriteLine("****************VerGReales*************");
        }

        private void Reporte_Load(object sender, EventArgs e)
        {
            this.reportViewer1.RefreshReport();
        }

        private void Reporte_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (selected.graph.Enabled == false)
            {
                MessageBox.Show("Button Disabled");
            }
        }
    }
}