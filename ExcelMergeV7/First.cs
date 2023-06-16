using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class Start : Form
    {
        private Functions ft = new Functions();
        private int idf1;
        private string idF1;
        private string fileName = @"C:\Users\Public\Documents\usr.json";

        private void Timer1_Tick(object sender, EventArgs e)
        {
            AutoUpdater.Start(@"\\192.168.15.134\Public\Ejecutables\versions.xml");
            AutoUpdater.ShowSkipButton = false;
        }

        public Start()
        {
            try
            {
                //Timer y Verificador de versiones de AutoUpdater
                AutoUpdater.Start(@"\\192.168.15.134\Public\Ejecutables\versions.xml");

                AutoUpdater.ShowSkipButton = false;
                InitializeComponent();
                Timer timer = new Timer();
                timer.Interval = (1 * 1000) * 60 * 5; // 5 Min
                timer.Tick += new EventHandler(Timer1_Tick);
                timer.Start();
                label1.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();

                /* Codigo de Simulacion de sesiòn persistente
                 * if (File.Exists(fileName))
                 {
                 usr.Visible = false;
                 pass.Visible = false;
                 label2.Text = "Bienvenido(a). Selecciona el tipo de monitoreo para continuar.";
                 label3.Text = "Cerrar Sesión";
                 label3.ForeColor = System.Drawing.Color.Blue;
                 iniciar.Visible = false;

                     if (ft.Log(usrV.K, usrV.Joi))
                     {
                         if (usrV.Joi == 4 || usrV.Joi == 5)
                         {
                             Consultar adm = new Consultar(usrV.K, usrV.Rick, usrV.Joi, usrV.Nombre);
                             adm.Show();
                             this.Hide();
                         }
                         else
                         {
                             Form1 CopiasControladas = new Form1(usrV.K, usrV.Rick, usrV.Joi, usrV.Nombre);
                             CopiasControladas.Show();
                             this.Hide();
                         }
                     }
                 }*/
            }
            catch (SqlException sqle)
            {
                if (sqle.Number == 53)
                {
                    MessageBox.Show("Error De Conexion Con el Servidor.");
                }
                else
                {
                    MessageBox.Show(sqle.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public Start(int idu, int rolu, int areau, string name)
        {
            id = idu;
            rol = rolu;
            area = areau;

            try
            {
                InitializeComponent();
                label1.Text = Assembly.GetEntryAssembly().GetName().Version.ToString();

                usr.Visible = false;
                pass.Visible = false;
                label2.Text = name + " Selecciona el tipo de monitoreo para continuar.";
                label3.Visible = false;
                iniciar.Visible = false;
            }
            catch (SqlException sqle)
            {
                if (sqle.Number == 53)
                {
                    MessageBox.Show("Error De Conexion Con el Servidor.");
                }
                else
                {
                    MessageBox.Show(sqle.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public List<string[,]> un = new List<string[,]>();
        private List<int> Sensors = new List<int>();

        private bool CheckDate(List<string[,]> files)
        {
            //Verificación de lecturas a un minuto de archivos Elitech y RTD
            int flag = 0, datepos = 0;

            //Busqueda de la columna de fechas en la matriz
            for (int i = 0; i < files[0].GetLength(1); i++)
            {
                if (!String.IsNullOrEmpty(files[0][0, i])
                    && !String.IsNullOrWhiteSpace(files[0][0, i]))
                {
                    if (ft.MatchDate(files[0][0, i]) || ft.MatchDate1(files[0][0, i]))
                    {
                        datepos = i;
                        Usr.DatePos = datepos;
                        break;
                    }
                }
            }

            int ii = 0;
            foreach (string[,] file in files)
            {
                //calculo de la diferencia en minutos entre lecturas
                for (int ind = 0; ind < file.GetLength(0) - 2; ind++)
                {
                    if (ind <= file.GetLength(0) - 2)
                    {
                        double diff = (ft.ToDate(file[ind + 1, datepos]) - ft.ToDate(file[ind, datepos])).TotalMinutes;
                        Usr.DateDif = diff;
                        if (diff > 1)
                        {
                            MessageBox.Show("La diferencia en tiempo entre " + ft.ToDate(file[ind + 1, datepos]) + " y " + ft.ToDate(file[ind, datepos]) + " es " + diff + " min.");
                            ft.DateDifer = int.Parse(diff.ToString());
                            flag++;
                            Sensors.Add(ii);
                            break;
                        }
                    }
                }
                ii++;
            }
            if (flag == 0)
            {
                return true;
            }
            else
            {
                //Alerta de sesnsores que no cuenten con las lecturas a 1 minuto
                for (int i = 0; i < files.Count; i++)
                {
                    if (ft.IsInDates(i, Sensors))
                    {
                        MessageBox.Show("El Sensor (" + ft.Sensors[i] + ") " + i + "\n NO contiene intervalos a un minuto.");
                    }
                }
                return false;
            }
        }

        private bool CheckDateMacro(List<string[,]> files)
        {
            //Verificación de lecturas a un minuto de los archivos excel
            int flag = 0;
            int ii = 0; int ff = 1;

            foreach (string[,] file in files)
            {
                int datepos = ft.GetDatePos(file);//Obtención de la columna de fechas de cada matriz de lectura

                for (int ind = 0; ind < file.GetLength(0) - 2; ind++)
                {
                    if (ind <= file.GetLength(0) - 2)
                    {
                        double diff = (ft.ToDate(file[ind + 1, datepos]) - ft.ToDate(file[ind, datepos])).TotalMinutes;
                        Usr.DateDif = diff;
                        if (diff > 1)
                        {
                            MessageBox.Show("La diferencia en tiempo entre " + file[ind + 1, datepos] + " y " + file[ind, datepos] + " es " + diff + " min.");
                            ft.DateDifer = int.Parse(diff.ToString());
                            flag++;
                            Sensors.Add(ii);
                            break;
                        }
                    }
                }
                ii++;
                ff++;
            }
            if (flag == 0)
            {
                return true;
            }
            else
            {
                //Alerta de que sensores son los que no tienen las lecturas a 1 minuto
                for (int i = 0; i < files.Count; i++)
                {
                    if (ft.IsInDates(i, Sensors))
                    {
                        MessageBox.Show("El Sensor (" + ft.Sensors[i] + ") " + (i + 1) + "\n NO contiene intervalos a un minuto.");
                    }
                }
                if (ft.DateDifer > 2)
                {
                    un.Clear();
                    ft.Sensors.Clear();
                }
                return false;
            }
        }

        private bool CheckDate(List<List<string[,]>> files)
        {
            //Rutina de verificación de lecturas de archivos CSV
            int datepos = 0;

            List<int> Sensors = new List<int>();
            int flg = 0, ii = 0;
            //Selección de la posición de la columna de fechas en la lista
            for (int i = 0; i < files[0][10].GetLength(1); i++)
            {
                if (!String.IsNullOrEmpty(files[0][10][0, i])
                    && !String.IsNullOrWhiteSpace(files[0][10][0, i]))
                {
                    if (ft.MatchDate(files[0][10][0, i]) || ft.MatchDate1(files[0][10][0, i]))
                    {
                        datepos = i;
                        Usr.DatePos = datepos;
                        break;
                    }
                }
            }

            foreach (List<string[,]> file in files)
            {
                for (int i = 0; i < file.Count - 2; i++)
                {
                    if (i <= file.Count - 2)
                    {
                        //Recorrido y calculo de la diferencia

                        double diff = (ft.ToDate(file[i + 1][0, 0].Substring(0, 16)) - ft.ToDate(file[i][0, 0].Substring(0, 16))).TotalMinutes;
                        Usr.DateDif = diff;
                        if (diff > 1)
                        {
                            MessageBox.Show("La diferencia en tiempo entre " + ft.ToDate(file[i + 1][0, 0].Substring(0, 16)) + " y " + ft.ToDate(file[i][0, 0].Substring(0, 16)) + " es " + diff + " min.");
                            Sensors.Add(ii);
                            ft.DateDifer = int.Parse(diff.ToString());
                            flg++;
                            break;
                        }
                    }
                }
                ii++;
            }
            if (flg == 0)
            {
                return true;
            }
            else
            {
                //Alerta de sensores que no tengan lecturas a 1 minuto
                for (int i = 0; i < files.Count; i++)
                {
                    if (ft.IsInDates(i, Sensors))
                    {
                        MessageBox.Show("El Sensor (" + ft.Sensors[i] + ") " + i + "\n NO contiene intervalos a un minuto.");
                    }
                }
                return false;
            }
        }

        private List<string> val1 = new List<string>();
        private string[,] NewArray;

        public void start(OpenFileDialog openFileDialog1, int id, int rol, int area)
        {
            //Inicio del programa - SE PUEDE SELECCIONAR MAS DE UN ARCHIVO
            openFileDialog1.Multiselect = true;
            if (rol != 1 && rol != 5)
            {
                //Delimitación del tipo de archivos dependiendo del nivel de acceso
                openFileDialog1.Filter = ".elt files (*.elt)|*.elt|.xlsx files (*.xlsx)|*.xlsx";
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //selección de rutina humedad o temperatura
                if (temperatura.Checked && !humedad.Checked)
                {
                    try
                    {
                        idF1 = id.ToString(); //

                        //ft.alert(ft.name);
                        idf1 = id;
                        ft.index = id;
                        ft.alert("Leyendo Sensores...");
                        string ext = Path.GetExtension(openFileDialog1.FileName);
                        string[,] united = null;
                        if (ext == ".xlsx")
                        {
                            //Lectura de archivos Excel
                            //El programa no lee archivos de excel 97-2003
                            ft.ReadXlsxTemp(openFileDialog1, carga1);//

                            //Eliminación del primer headder de las lecturas
                            if (ft.flag != 0)
                            {
                                for (int i = 1; i < ft.Uni.Count; i++)
                                {
                                    un.Add(ft.Uni[i]);
                                }
                            }
                            else
                            {
                            }

                            ft.alert(ft.Sensors.Count + " Sensores Cargados", "caption");
                            //Bloqueo de proceso dependiendo de los permisos y si es un archivo de macro con datos Elitech
                            if (ft.flag != 0 && (rol == 5 || rol == 1))
                            {
                                //Chequeo de la diferencia en minutos entre registros
                                if (CheckDateMacro(un))
                                {
                                    united = ft.Unite22(un);//Creación de la vista Horizontal de los sensores
                                }
                                else

                                {
                                    //Rutina para diferencia de 2 minutos
                                    //Se calcula el promedio entre posiciones
                                    //para generar una matriz con fechas a 1 minuto
                                    ft.FixedSensors.Add(ft.Uni[0]);//Temporal de las nuevas matrices
                                    if (ft.DateDifer == 2)
                                    {
                                        for (int ii = 0; ii < un.Count; ii++)

                                        {
                                            //IsInDates verifica si un indice se encuentra en una lista
                                            //Si el indice del sensor se encuentra en la lista, se hace el cálculo
                                            //De lo contrario se agrega a la colección
                                            if (ft.IsInDates(ii, Sensors))
                                            {
                                                //Cálculo de la cantidad de filas de la nueva matriz
                                                int Size = ft.DateDifer * un[ii].GetLength(0);
                                                NewArray = new string[Size, 2];
                                                //Copiar los valores a la nueva matriz en las nuevas posiciones
                                                for (int i = 0; i < Size; i = i + ft.DateDifer)
                                                {
                                                    if (i == 0)
                                                    {
                                                        NewArray[0, 0] = un[ii][0, 1];
                                                        NewArray[0, 1] = un[ii][0, 2];
                                                    }
                                                    else if (i > 0 && i < Size - 1)
                                                    {
                                                        int ind = i / ft.DateDifer;
                                                        var var1 = un[ii][ind, 1];
                                                        var var2 = un[ii][ind, 2];

                                                        NewArray[i, 0] = var1;
                                                        NewArray[i, 1] = var2;
                                                    }
                                                }
                                                //Recorrer la nueva matriz para el cálculo
                                                //y asentamiento de los valores promedio
                                                for (int i = 0; i < Size; i++)
                                                {
                                                    if (i == 0) { }
                                                    else if (i > 0 && i < Size - 1)
                                                    {
                                                        if (ft.IsValid(NewArray[i - 1, 0]) && ft.IsValid(NewArray[i + 1, 0]))
                                                        {
                                                            if (!ft.IsValid(NewArray[i + 1, 1]))
                                                            {
                                                                val1.Add(i + " " + ii + " " + NewArray[i - 1, 1]);
                                                                //Console.WriteLine(i + " " + ii + " " + NewArray[i - 1, 0]);
                                                                //Console.WriteLine(i + " " + NewArray[i + 1, 1]);
                                                            }
                                                            NewArray[i, 0] = ft.DateFormat(ft.ToDate(NewArray[i - 1, 0]).AddMinutes(1));
                                                            NewArray[i, 1] = ((float.Parse(NewArray[i - 1, 1]) + float.Parse(NewArray[i + 1, 1])) / 2).ToString();
                                                        }
                                                    }
                                                }

                                                ft.FixedSensors.Add(NewArray);//Agrear a la colección
                                            }
                                            else
                                            {
                                                ft.FixedSensors.Add(un[ii]);
                                            }
                                        }
                                        united = ft.Unite22(ft.FixedSensors);//Creación de la vista Horizontal de los sensores

                                        ft.Uni = ft.FixedSensors;
                                    }
                                    else
                                    {
                                        Close();
                                    }
                                }
                                //Console.WriteLine("ftflag!=0");
                                // united = ft.Unite22(un);
                            }
                            else
                            {
                                //Rutina para Los archivos excel RTD
                                if (CheckDate(ft.Uni))
                                //if (1 == 1)
                                {
                                    united = ft.Unite22(ft.Uni);//Creación de la vista Horizontal de los sensores
                                }

                                //Rutina para diferencia de 2 minutos
                                //Se calcula el promedio entre posiciones
                                //para generar una matriz con fechas a 1 minuto
                                if (ft.DateDifer == 2)
                                {
                                    for (int ii = 0; ii < ft.Uni.Count; ii++)
                                    {
                                        if (ft.IsInDates(ii, Sensors))
                                        {
                                            int Size = ft.DateDifer * ft.Uni[ii].GetLength(0);
                                            string[,] NewArray = new string[Size, 2];
                                            for (int i = 0; i < Size; i = i + ft.DateDifer)
                                            {
                                                if (i == 0)
                                                {
                                                    NewArray[0, 0] = ft.Uni[ii][0, 0];
                                                    NewArray[0, 1] = ft.Uni[ii][0, 1];
                                                }
                                                else if (i > 0 && i < Size - 1)
                                                {
                                                    int ind = i / ft.DateDifer;
                                                    NewArray[i, 0] = ft.Uni[ii][ind, 0];
                                                    NewArray[i, 1] = ft.Uni[ii][ind, 1];
                                                }
                                            }
                                            for (int i = 0; i < Size; i++)
                                            {
                                                if (i == 0) { }
                                                else if (i > 0 && i < Size - 1)
                                                {
                                                    if (ft.IsValid(NewArray[i - 1, 0]) && ft.IsValid(NewArray[i + 1, 0]))
                                                    {
                                                        NewArray[i, 0] = ft.Date2(ft.ToDate(NewArray[i - 1, 0]).AddMinutes(1));
                                                        NewArray[i, 1] = ((float.Parse(NewArray[i - 1, 1]) + float.Parse(NewArray[i + 1, 1])) / 2).ToString();
                                                    }
                                                }
                                            }
                                            ft.FixedSensors.Add(NewArray);
                                        }
                                        else
                                        {
                                            ft.FixedSensors.Add(ft.Uni[ii]);
                                        }
                                    }
                                    united = ft.Unite22(ft.FixedSensors);//Creación de la vista Horizontal de los sensores
                                    ft.Uni = ft.FixedSensors;
                                }
                                else
                                {
                                    united = ft.Unite22(ft.Uni);
                                }
                            }
                        }
                        else if (ext == ".csv")
                        {
                            //Rutina para la lectura de los archivos CSV de los sensores OMEGA
                            ft.NewReadOm(openFileDialog1, carga1);
                            ft.alert(ft.Uni.Count + " Sensores Cargados OPV");
                            //united = ft.Unite22(ft.Uni);
                            //Verificación de lecturas a 1 minuto
                            if (CheckDate(ft.TEMPS))
                            {
                                united = ft.MatrizDeVisualizacion(ft.TEMPS);//Creación de la vista Horizontal de los sensores
                            }
                            else
                            {
                                MessageBox.Show("Los sensores no cuentan con lecturas de un minuto");
                                this.Close();
                            }
                        }
                        else if (ext == ".elt")
                        {
                            //Lectura de Achivos Elitech
                            ft.NewReadSens(openFileDialog1, carga1);
                            ft.alert(ft.Uni.Count + " Sensores Cargados", "ELT");
                            if (CheckDate(ft.Uni))
                            {
                                united = ft.Unite22(ft.Uni);//Creación de la vista Horizontal de los sensores
                            }
                            else
                            {
                                //Cálculo de promedios para sensores con lecturas de dos minutos
                                if (ft.DateDifer == 2)
                                {
                                    for (int ii = 0; ii < ft.Uni.Count; ii++)
                                    {
                                        if (ft.IsInDates(ii, Sensors))
                                        {
                                            int Size = ft.DateDifer * ft.Uni[ii].GetLength(0);
                                            string[,] NewArray = new string[Size, 2];
                                            for (int i = 0; i < Size; i = i + ft.DateDifer)
                                            {
                                                if (i == 0)
                                                {
                                                    NewArray[0, 0] = ft.Uni[ii][0, 0];
                                                    NewArray[0, 1] = ft.Uni[ii][0, 1];
                                                }
                                                else if (i > 0 && i < Size - 1)
                                                {
                                                    int ind = i / ft.DateDifer;
                                                    NewArray[i, 0] = ft.Uni[ii][ind, 0];
                                                    NewArray[i, 1] = ft.Uni[ii][ind, 1];
                                                }
                                            }
                                            for (int i = 0; i < Size; i++)
                                            {
                                                if (i == 0) { }
                                                else if (i > 0 && i < Size - 1)
                                                {
                                                    if (ft.IsValid(NewArray[i - 1, 0]) && ft.IsValid(NewArray[i + 1, 0]))
                                                    {
                                                        NewArray[i, 0] = ft.Date2(ft.ToDate(NewArray[i - 1, 0]).AddMinutes(1));
                                                        NewArray[i, 1] = ((float.Parse(NewArray[i - 1, 1]) + float.Parse(NewArray[i + 1, 1])) / 2).ToString();
                                                    }
                                                }
                                            }
                                            ft.FixedSensors.Add(NewArray);
                                        }
                                        else
                                        {
                                            ft.FixedSensors.Add(ft.Uni[ii]);
                                        }
                                    }
                                    united = ft.Unite22(ft.FixedSensors);
                                    ft.Uni = ft.FixedSensors;
                                }
                                else
                                {
                                    Close();
                                }
                            }
                        }

                        Usr.Uni = ft.Uni;//Almacenamiento en Memoria estatica por si se requiere cambiar las fechas
                        ft.alert("Construyendo Vista...");
                        if (Usr.IsValidator)
                        {
                            //Si las lecturas de xlsx es de Validator a las fechas se les cambia el formato
                            //Al formato de los sensores RTD
                            foreach (string[,] file in Usr.Uni)
                            {
                                int dt = ft.GetDatePos(file);//Definición de la columna de fechas
                                string temp = file[1, dt];
                                DateTime dd = DateTime.Parse(file[1, dt]);
                                file[1, dt] = dd.ToString("yyyy-MM-dd HH:mm:ss");
                                for (int i = 2; i < file.GetLength(0); i++)
                                {
                                    dd = DateTime.Parse(file[i, Usr.DatePos]);

                                    file[i, dt] = dd.ToString("yyyy-MM-dd HH:mm:ss");
                                }
                            }
                            ft.Uni = Usr.Uni;
                        }

                        //El 0 por "temperatura"
                        //Creación y paso de parametros para la vista de los sensores en formato Horizontal
                        BigDGV big = new BigDGV(idf1, idF1, ft.flagG, 0, ft.Sensors, ft.isMPF, ext, rol);
                        big.BigDGV1.DataSource = ft.BuildDatatable(united);//Parseo de matriz a datatable y asignación a datagridview
                        //big.united2 = united;
                        big.label1.Visible = true;
                        //big.charge2.Value = 1;
                        //ft.Saving(united, carga1);
                        big.charge2.Visible = false;
                        big.Show();
                        big.label1.Visible = false;
                        openFileDialog1.Reset();
                        this.Hide();

                        //this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        MessageBox.Show(val1[0]);
                        MessageBox.Show(val1 + " val2:");
                        //errors.Text = ex.ToString();
                    }
                }
                else if (!temperatura.Checked && !humedad.Checked)
                {
                    MessageBox.Show("Seleccione al menos una opción para continuar");
                }
                else
                {
                    try
                    {
                        idF1 = id.ToString();

                        //ft.alert(ft.name);
                        idf1 = id;
                        ft.index = id;
                        ft.alert("Leyendo Sensores...");
                        string ext = Path.GetExtension(openFileDialog1.FileName);

                        if (ext == ".xlsx")
                        {
                            ft.NewReadV2_2HUM(openFileDialog1, carga1);

                            if (ft.flag != 0)
                            {
                                for (int i = 1; i < ft.Uni.Count; i++)
                                {
                                    un.Add(ft.Uni[i]);
                                }
                            }
                            else
                            {
                            }
                        }
                        else if (ext == ".csv")
                        {
                            ft.NewReadOm(openFileDialog1, carga1);
                        }
                        else if (ext == ".elt")
                        {
                            ft.NewReadSensHum(openFileDialog1, carga1);
                        }

                        string[,] united = null;
                        if (ext == ".xlsx")
                        {
                            ft.alert(ft.Sensors.Count + " Sensores Cargados", "MACRO");

                            if (ft.flag != 0 && (rol == 5 || rol == 1))
                            {
                                //ENTRADA DE LA MACRO

                                if (CheckDateMacro(un))
                                {
                                    united = ft.Unite22(un);
                                }
                                else
                                {
                                    MessageBox.Show("Los sensores no cuentan con lecturas de un minuto");
                                    this.Close();
                                }
                            }
                            else
                            {
                                if (CheckDate(ft.Uni))
                                {
                                    united = ft.Unite22(ft.Uni);
                                }
                                else
                                {
                                    MessageBox.Show("Los sensores no cuentan con lecturas de un minuto");
                                    this.Close();
                                }
                            }
                        }
                        else if (ext == ".csv")
                        {
                            ft.alert(ft.Uni.Count + " Sensores Cargados OPV");

                            if (CheckDate(ft.TEMPS))
                            {
                                united = ft.MatrizDeVisualizacion(ft.TEMPS);
                            }
                            else
                            {
                                MessageBox.Show("Los sensores no cuentan con lecturas de un minuto");
                                this.Close();
                            }
                        }
                        else if (ext == ".elt")
                        {
                            ft.alert(ft.Uni.Count + " Sensores Cargados", "ELT");//
                            if (CheckDate(ft.Uni))
                            {
                                united = ft.MatrizDeVisualizacionHum(ft.Uni);
                            }
                            else
                            {
                                this.Close();
                            }
                        }

                        ft.alert("Construyendo Vista...");
                        openFileDialog1.Reset();

                        ft.FExt = ext;

                        //El 1 por "Humedad"
                        BigDGV big = new BigDGV(idf1, idF1, ft.flagG, 1, ft.Sensors, ft.isMPF, ext, rol);
                        big.BigDGV1.DataSource = ft.BuildDatatable(united);

                        big.label1.Visible = true;

                        big.charge2.Visible = false;
                        big.Show();
                        big.label1.Visible = false;

                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                        //errors.Text = ex.ToString();
                    }
                }
            }
        }

        private void Start_FormClosing(object sender, FormClosingEventArgs e)

        {
        }

        public bool verifica(int id, string tabla)
        {
            string sql = "Select count(*) from " + tabla + " where Id='" + id + "'";
            SqlConnection conn = new SqlConnection(ft.connection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(sql, conn);
            int temp = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            if (temp == 0)
            {
                conn.Close();
                return true;
            }
            else
            {
                conn.Close();
                return false;
            }
        }

        public void Acceso(int id, int rol, int area)
        {//DEPENDIENDO DEL ROL Y EL CHECK DE AREA SE ELIMINAN LOS REGISTROS EN LAS TABLAS
            if (DirectToServer.Checked && (rol == 5 || rol == 4))
            {
                List<string> Sensors1 = new List<string>();
                //ToBgWorker return true or false to bgwCompleted
                ft.DeleteTable(id, "IncertidumbreHumedad");
                ft.DeleteTable(id, "Incertidumbre");
                ft.DeleteTable(id, "Datos");
                ft.DeleteTable(id, "parametros");
                if (temperatura.Checked)
                {
                    Selected selected = new Selected(id, Sensors1, rol);

                    selected.Show();
                    Hide();
                }
                else if (humedad.Checked)
                {
                    int hum = 1;
                    Selected selected = new Selected(id, Sensors1, "humedad", hum, rol);
                    //HumedadDGV humedad = new HumedadDGV(idSi, hum, Sensors1);

                    selected.Show();
                    Hide();
                }
            }
            else
            if (temperatura.Checked) //al seleccionar temperatura
            {
                if (verifica(id, "IncertidumbreHumedad") && verifica(id, "Incertidumbre") &&
                    verifica(id, "Datos"))
                {
                    //ToBgWorker return Sesnsors to bgwcompleted then write dgv
                    start(openFileDialog1, id, rol, area); //se abre el dialog
                }
                else
                {//se eliminan los datos de las tablas 
                    ft.DeleteTable(id, "IncertidumbreHumedad");
                    ft.DeleteTable(id, "Incertidumbre");
                    ft.DeleteTable(id, "Datos");
                    ft.DeleteTable(id, "parametros");
                    //ToBgWorker return Sesnsors to bgwcompleted then write dgv
                    start(openFileDialog1, id, rol, area);
                }
            }
            else if (humedad.Checked)//al seleccionar humedad
            {
                if (verifica(id, "IncertidumbreHumedad") && verifica(id, "Incertidumbre") && verifica(id, "Datos"))
                {
                    //ToBgWorker return Sesnsors to bgwcompleted then write dgv
                    start(openFileDialog1, id, rol, area);
                }
                else
                {
                    ft.DeleteTable(id, "IncertidumbreHumedad");
                    ft.DeleteTable(id, "Incertidumbre");
                    ft.DeleteTable(id, "Datos");
                    ft.DeleteTable(id, "parametros");
                    //ToBgWorker return Sesnsors to bgwcompleted then write dgv
                    start(openFileDialog1, id, rol, area);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un tipo para continuar");
            }
        }

        private int ID;

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void humedad_CheckedChanged(object sender, EventArgs e)
        {
            if (humedad.Checked)
            {
                temperatura.Checked = false;
            }
        }

        private void temperatura_CheckedChanged(object sender, EventArgs e)
        {
            if (temperatura.Checked)
            {
                humedad.Checked = false;
            }
        }

        private int id, rol, area;

        private void pass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                try
                {
                    SqlDataReader dr = ft.GetLog(usr.Text, pass.Text);
                    if (dr != null)
                    {
                        Usr.K = (int)dr.GetValue(dr.GetOrdinal("IdUsuario"));
                        Usr.Joi = (int)dr.GetValue(dr.GetOrdinal("IdRol"));
                        Usr.Rick = (int)dr.GetValue(dr.GetOrdinal("IdArea"));
                        ID = Usr.K;
                        //String name = ft.getNombre(usr.Text, pass.Text);
                        //DialogResult dialogResult = MessageBox.Show("¿" + name + "?", "", MessageBoxButtons.YesNo);

                        // if (dialogResult == DialogResult.Yes)
                        //{
                        //Acceso(id, rol, area);
                        Ingresar();
                    }

                    //}
                }
                catch (SqlException sqle)
                {
                    MessageBox.Show(sqle.Message);
                    MessageBox.Show(sqle.ToString());
                    if (sqle.Number == 53)
                    {
                        MessageBox.Show("Error De Conexión. \n Código de error:  " + sqle.ErrorCode.ToString());
                        start(openFileDialog1, id, rol, area);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void iniciar_Click(object sender, EventArgs e)
        {
            try
            {
                SqlDataReader dr = ft.GetLog(usr.Text, pass.Text);
                if (dr != null)
                {//ASIGNA LOS DATOS DEL USUARIO A LA CLASE 
                    Usr.K = (int)dr.GetValue(dr.GetOrdinal("IdUsuario"));
                    Usr.Joi = (int)dr.GetValue(dr.GetOrdinal("IdRol"));
                    Usr.Rick = (int)dr.GetValue(dr.GetOrdinal("IdArea"));
                    ID = Usr.K;
                    //String name = ft.getNombre(usr.Text, pass.Text);
                    //DialogResult dialogResult = MessageBox.Show("¿" + name + "?", "", MessageBoxButtons.YesNo);

                    // if (dialogResult == DialogResult.Yes)
                    //{
                    //Acceso(id, rol, area);
                    Ingresar();
                }
            }
            catch (SqlException sqle)
            {
                MessageBox.Show(sqle.Message);
                MessageBox.Show(sqle.ToString());
                if (sqle.Number == 53)
                {
                    MessageBox.Show("Error De Conexión. \n Código de error:  " + sqle.ErrorCode.ToString());
                    start(openFileDialog1, id, rol, area);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Start_Activated(object sender, EventArgs e)
        {
        }

        private void label3_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("¿Desea cerrar sesión?", "", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    File.Delete(fileName);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //e.Result = result;
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            carga1.Value = e.ProgressPercentage;
            if (e.UserState != null)
            {
            }
            //label1.Items.Add(e.UserState);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Numbers between 0 and 10000 divisible by 7: " + e.Result);
        }

        private void Start_Load(object sender, EventArgs e)
        {
            //Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void BGW()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(10000);
        }

        private void Ingresar()
        {
            if (Usr.Rick == 3 || Usr.Rick == 5 || Usr.Rick == 4)
            {
                Acceso(Usr.K, Usr.Joi, Usr.Rick);

                //MessageBox.Show(file);
                //string jsonString = JsonSerializer.Serialize(usr);
                //File.WriteAllText(fileName, jsonString);
                /*if (usr.Joi == 4 || usr.Joi == 5)
                {
                    Consultar adm = new Consultar(usr.K, usr.Rick, usr.Joi, usr.Nombre);
                    adm.Show();
                    string file = @"C:\Users\Public\Documents\usr.json";
                    //MessageBox.Show(file);
                    string jsonString = JsonSerializer.Serialize(usr);
                    File.WriteAllText(file, jsonString);
                }
                else
                {
                    Form1 CopiasControladas = new Form1(usr.K, usr.Rick, usr.Joi, usr.Nombre);
                    CopiasControladas.Show();
                    //Save to file

                    string file = @"C:\Users\Public\Documents\usr.json";
                    //MessageBox.Show(file);
                    string jsonString = JsonSerializer.Serialize(usr);
                    File.WriteAllText(file, jsonString);
                }*/
            }
        }
    }
}