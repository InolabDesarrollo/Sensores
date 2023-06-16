using Microsoft.Reporting.WinForms;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    internal class Functions
    {
        //LOS HILOS LOS OCUPAN PARA LA OBTENCION Y VISUALIZACON DE DATOS

        //**estos hilos no tienen referencia
        [ThreadStatic] private static List<string[,]> uni = new List<string[,]>();
        [ThreadStatic] private static List<string[,]> uni2 = new List<string[,]>();
        [ThreadStatic] private static List<string> flags = new List<string>();
        [ThreadStatic] private static List<string[,]> fixedSensor = new List<string[,]>();
    //**
        public List<string[,]> Uni { get => uni; set => uni = value; }

        public List<string[,]> FixedSensors { get => fixedSensor; set => fixedSensor = value; }
        private int dateDifer;
        public int DateDifer { get => dateDifer; set => dateDifer = value; }
        public List<string[,]> Uni2 { get => uni2; set => uni2 = value; }
        public List<string> Sensors { get => sensors; set => sensors = value; }
        public List<string> Flags { get => flags; set => flags = value; }
        [ThreadStatic] public bool isMPF = false;

        //**estos hilos no se ocupan o en tiempo de ejecucion
        [ThreadStatic]
        public int dataRow;

        [ThreadStatic]
        public List<string> dates = new List<string>();

        [ThreadStatic]
        public List<int> FlaggedSensors = new List<int>();

        [ThreadStatic]
        public int flagG = 0;

        [ThreadStatic]
        public int flagG2 = 0;

        private int MaxCol;

        [ThreadStatic]
        public int index;

        [ThreadStatic]
        public int incer = 0;

        [ThreadStatic]
        private List<string> sensors = new List<string>();

        public List<List<string[,]>> TEMPS = new List<List<string[,]>>();
        private List<string[,]> temps = new List<string[,]>();

        [ThreadStatic]
        public int flag = 0;

        [ThreadStatic]
        public string FExt = "";
        //**

        // diseño de la ventana del mensaje de alerta
        public void alert(string message, string caption) 
        {
            var w = new Form() { Size = new Size(0, 0) };
            Task.Delay(TimeSpan.FromSeconds(0.85))
                .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());

            MessageBox.Show(w, message, caption);
        }

        // diseño de la ventana de alerta
        public void alert(string message)
        {
            var w = new Form() { Size = new Size(0, 0) };
            Task.Delay(TimeSpan.FromSeconds(0.85))
                .ContinueWith((t) => w.Close(), TaskScheduler.FromCurrentSynchronizationContext());

            MessageBox.Show(w, message);
        }

        public void ToUni(List<List<string[,]>> temps, int colsd)
        {
            int fl = 1;
            foreach (List<string[,]> file in temps)
            {
                string[,] tt = new string[file.Count, colsd];
                int i = 0;
                foreach (string[,] row in file)
                {
                    for (int x = 0; x < row.GetLength(1); x++)
                    {
                        tt[i, x] = row[0, x];
                    }
                    i++;
                }
                Uni.Add(tt);
                fl++;
            }
        }

        //llenado de la matriz para verla en el grid
        public string[,] MatrizDeVisualizacion(List<string[,]> temps)
        {
            int colsD = 0, RowsD = 0;
            for (int i = 0; i < temps.Count; i++)
            {
                if (temps[i] != null)
                {
                    int cols = temps[i].GetLength(1);

                    int rows = temps[i].GetLength(0);

                    colsD = colsD + cols;

                    if (RowsD < rows)
                    {
                        RowsD = rows;
                    }
                }
            }

            string[,] united = new string[RowsD, colsD];

            int colS = 0;

            for (int i = 0; i < temps.Count; i++)
            {
                if (temps[i] != null)
                {
                    int cols = temps[i].GetLength(1);
                    int rows = temps[i].GetLength(0);

                    for (int x = 0; x < rows; x++)
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            united[x, colS + j] = temps[i][x, j];
                        }
                    }
                    colS = colS + cols;
                }
                else
                {
                    Console.WriteLine("No File Unite2 FILE: " + i);
                }
            }

            Console.WriteLine("Ended");
            return united;
        }

        //
        public string[,] MatrizDeVisualizacion(List<List<string[,]>> Temps)
        {
            int colsD = 0, RowsD = 0, rowsd = 0, colsd = 0, count = 0;

            foreach (List<string[,]> temp in Temps)
            {
                foreach (string[,] row in temp)
                {
                    if (colsd < row.GetLength(1))
                    {
                        colsd = row.GetLength(1);
                    }

                    rowsd++;
                }
                colsD = colsD + colsd;
                if (RowsD < rowsd)
                {
                    RowsD = rowsd;
                }
                rowsd = 0;
                count++;
            }

            // Console.WriteLine("Uni: "+uni.Count);
            string[,] united = new string[RowsD, colsD + 1];
            //Console.WriteLine("Rows: "+RowsD+" Cols "+colsD+" Count: "+count);
            int colS = 0, rowss = 0, comp = 0, ii = 1; ;
            foreach (List<string[,]> temp in Temps)
            {
                //Console.WriteLine("TEMP COUNT "+ii);
                foreach (string[,] row in temp)
                {
                    //Console.WriteLine("Cols: " + row.GetLength(1)+" COLS: "+colS);
                    for (int i = 0; i < row.GetLength(1); i++)
                    {
                        if (!String.IsNullOrEmpty(row[0, i]))
                        {
                            //Console.WriteLine("United[" + rowss + "," + (colS + i) + "]=Row[0," + i + "]= " + row[0, i]);

                            united[rowss, colS + i] = row[0, i];

                            if (colS < row.GetLength(1))
                            {
                                comp = row.GetLength(1);
                            }
                        }
                    }

                    rowss++;
                }
                colS = colS + comp;
                rowss = 0;
                ii++;
            }

            //Console.WriteLine("Ended United2List");
            return united;
        }


        //carga los datos de humedad para el grid desde la matriz
        public string[,] MatrizDeVisualizacionHum(List<string[,]> temps)
        {
            int colsD = 0, RowsD = 0;
            for (int i = 0; i < temps.Count; i++)
            {
                if (temps[i] != null)
                {
                    int cols = temps[i].GetLength(1);

                    int rows = temps[i].GetLength(0);

                    colsD = colsD + cols;

                    if (RowsD < rows)
                    {
                        RowsD = rows;
                    }
                }
                else
                {
                    Console.WriteLine("Temp: " + i + " is NULL");
                }
            }

            string[,] united = new string[RowsD, colsD];

            int colS = 0;

            for (int i = 0; i < temps.Count; i++)
            {
                if (temps[i] != null)
                {
                    int cols = temps[i].GetLength(1);
                    int rows = temps[i].GetLength(0);
                    // Console.WriteLine("cols: "+cols+" rows "+rows+" Temp: "+i);
                    for (int x = 0; x <= rows - 1; x++)
                    {
                        for (int j = 0; j <= cols - 1; j++)
                        {
                            //Console.WriteLine("i: "+i+" j: "+j+" U: "+temps[i][x,j]);
                            united[x, colS + j] = temps[i][x, j];
                            //file[i, j] = null;
                        }
                    }
                    colS = colS + cols;
                }
                else
                {
                    Console.WriteLine("No File Unite2 FILE: " + i);
                }
            }

            // Console.WriteLine("Finished unite22");
            return united;
        }

        //carga los datos para el cambio de fecha
        public string[,] Unite22(List<string[,]> temps)
        {
            int colsD = 0, RowsD = 0;
            for (int i = 0; i < temps.Count; i++)
            {
                if (temps[i] != null)
                {
                    int cols = temps[i].GetLength(1);
                    int rows = temps[i].GetLength(0);

                    colsD = colsD + cols;

                    if (RowsD < rows)
                    {
                        RowsD = rows;
                    }
                }
                else
                {
                    Console.WriteLine("Temp: " + i + " is NULL");
                }
            }

            string[,] united = new string[RowsD, colsD];

            int colS = 0;

            for (int i = 0; i < temps.Count; i++)
            {
                if (temps[i] != null)
                {
                    int cols = temps[i].GetLength(1);
                    int rows = temps[i].GetLength(0);

                    for (int x = 0; x <= rows - 1; x++)
                    {
                        for (int j = 0; j <= cols - 1; j++)
                        {
                            united[x, colS + j] = temps[i][x, j];
                        }
                    }
                    colS = colS + cols;
                }
                else
                {
                    Console.WriteLine("No File Unite2 FILE: " + i);
                }
            }

            return united;
        }

        public List<int> GetPosDates(string[,] united)
        {
            List<int> DatePos = new List<int>();
            //string input = firstdoc.GetCellValueAsString(i, j);
            for (int col = 0; col < united.GetLength(1); col++)
            {
                if (IsValid(united[0, col]))
                {
                    if (MatchDate(united[0, col]) || MatchDate1(united[0, col]))
                    {
                        DatePos.Add(col);
                    }
                }
            }

            return DatePos;
        }

        public List<int> GetPos2(string[,] united)
        {
            List<int> DatePos = new List<int>();
            //string input = firstdoc.GetCellValueAsString(i, j);
            for (int col = 0; col < united.GetLength(1); col++)
            {
                if (IsValid(united[0, col]))
                {
                    if (MatchDate(united[0, col]) || MatchDate1(united[0, col]) || MatchFloat(united[0, col]))
                    {
                        DatePos.Add(col);
                    }
                }
            }

            return DatePos;
        }

        public List<int> GetPos3(string[,] united)
        {
            List<int> DatePos = new List<int>();
            //string input = firstdoc.GetCellValueAsString(i, j);
            for (int col = 0; col < united.GetLength(1); col++)
            {
                if (IsValid(united[0, col]))
                {
                    if (MatchFloat(united[0, col]))
                    {
                        DatePos.Add(col);
                    }
                }
            }

            return DatePos;
        }

        //RECORRE LAS COLUMNAS DE LA TABLA GENERADA DE LOS ARCHIVOS
        public DataTable BuildDatatable(string[,] Temps)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < Temps.GetLength(1); i++)
            {
                dt.Columns.Add("Column" + (i + 1));
            }

            for (var i = 0; i < Temps.GetLength(0); ++i)
            {
                DataRow row = dt.NewRow();
                for (var j = 0; j < Temps.GetLength(1); ++j)
                {
                    row[j] = Temps[i, j];
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        public void WriteDGV2(List<string> flags, DataGridView dgv)
        {
            dgv.Columns.Add("Fechas", "Fechas");

            int i = 0;
            //Console.WriteLine(dgv.Columns.Count + " " + dgv.Rows.Count);
            foreach (string date in flags)
            {
                dgv.Rows.Add();
                if (!String.IsNullOrEmpty(date))
                {
                    dgv.Columns[0].Width = 149;
                    dgv.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
                    dgv.Rows[i].Cells[0].Value = date;
                    dgv.Rows[i].Cells[0].Style.BackColor = Color.FromArgb(234, 240, 248);
                }
                i++;
            }
        }

        public string[,] MarkUp2V2(string[,] file, string fecha, bool ismpf)
        {
            string date = "";
            int cols = file.GetLength(1);
            int rows = file.GetLength(0);
            string[,] marked = null;
            int flag = 0, mark = 0;
            int jj = 0;
            flagG2 = 0;

            if (ismpf)
            {
                //Omega RTD
                for (int i = 0; i <= rows - 1; i++)
                {
                    if (flag == 1)
                    {
                        //Finalizacion del ciclo si se ha encontrado la fecha
                        break;
                    }
                    else
                    {
                        for (int j = 1; j <= cols - 1; j++)
                        {
                            string text = file[i, j];//valor a analizar

                            if (isDate(text) && MatchDate(text))
                            {
                                if (text.Substring(0, 16) == fecha)
                                {
                                    //Si se encuentra la fecha
                                    //Anclas de posiciones
                                    dataRow = j + 1;//Columna de datos
                                    mark = i;//Posicion de la incidencia
                                    jj = file.GetLength(1) - j;//Cantidad de Columnas

                                    flag = 1;//Flag de incidencias

                                    string[,] marked1 = new string[rows - mark, jj + 1];//Matriz temporal

                                    //Recorrido de matriz anterior y llenado de la nueva a partir de la incidencia
                                    for (int y = 0; y < rows - mark; y++)
                                    {
                                        for (int j1 = 1; j1 <= jj + 1; j1++)
                                        {
                                            if (file[mark + y, j1] == "NC" || file[mark + y, j1] == "NA")
                                            {
                                                //En esta rutina se agregan las fechas antes o despues de donde no hay datos
                                                //en una lista para su posterior visualización
                                                for (int indice = j1; indice <= j1; indice--)
                                                {
                                                    if (MatchDate(file[mark + y, indice]) || MatchDate1(file[mark + y, indice]))
                                                    {
                                                        Flags.Add(file[mark + y, indice]);
                                                        break;
                                                    }
                                                }
                                                flagG = 1;
                                            }

                                            marked1[y, j1 - 1] = file[mark + y, j1];
                                        }
                                    }

                                    marked = marked1;

                                    break;
                                }
                            }
                        }
                    }
                    if (flag != 1 && i == rows - 1)
                    {
                        //Dates.Add(fecha);
                        flagG2 = 500;
                        Usr.Flag = true;
                        marked = file;
                    }
                }
            }
            else
            {
                for (int i = 0; i < rows; i++)
                {
                    if (flag == 1)
                    {
                        //Si se ha encontrado la fecha se termina el ciclo
                        break;
                    }
                    if (Usr.IsValidator)
                    {
                        int dd = GetDatePos(file);
                        for (int ins = 1; ins < file.GetLength(0); ins++)
                        {
                            string comp = file[ins, dd].Substring(0, 16);
                            int Dif = DateTime.Compare(ToDate(comp).AddSeconds(-1 * ToDate(comp).Second), ToDate(fecha).AddSeconds(-1 * ToDate(fecha).Second));
                            if (Dif == 0)
                            {
                                //Console.WriteLine("Sizes " + file.GetLength(0) + " " + file.GetLength(1));
                                string[,] NewFile = new string[file.GetLength(0) - ins + 1, file.GetLength(1) + 1];
                                //Console.WriteLine("Sizes " + NewFile.GetLength(0) + " " + NewFile.GetLength(1));
                                for (int k = 0; k < file.GetLength(0) - ins; k++)
                                {
                                    for (int kk = dd; kk < file.GetLength(1); kk++)
                                    {
                                        //   Console.WriteLine("ins: " + ins + " k " + k + " kk " + kk);
                                        // Console.WriteLine("ins+k " + (ins + k));
                                        NewFile[k, kk] = file[ins + k, kk];
                                    }
                                }
                                return NewFile;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < cols; j++)
                        {
                            //Console.WriteLine("Uni2: "+file[i,j]);
                            string text = file[i, j];
                            // Console.WriteLine("i: " + i + " j: " + j+" "+file[i,j]);

                            //MessageBox.Show(text+" != "+ errors.Text);
                            if (IsValid(text) && MatchDate(text))
                            {
                                //Console.WriteLine("TEXT: " +file[i, j].Substring(0, 16) + " != " + fecha);
                                //MessageBox.Show(file[i, j].Substring(0, 16)+" != "+fecha);
                                if (i == 0 && text.Substring(0, 16) == fecha)
                                {
                                    return file;
                                }
                                else
                                if (text.Substring(0, 16) == fecha)
                                {
                                    dataRow = j + 1;
                                    date = text;
                                    //Console.WriteLine("Date: " + file[i,j]+"Data: "+file[i,j+1]+" J: "+j);
                                    mark = i;
                                    jj = file.GetLength(1);
                                    //Console.WriteLine("JJ: "+(jj + 1)+" "+j);
                                    flag = 1;

                                    string[,] marked1 = new string[rows - mark, jj];
                                    //Console.WriteLine("Marked1Cols: " + marked1.GetLength(1));
                                    int SIZE = rows - mark;
                                    for (int y = 0; y < SIZE; y++)
                                    {
                                        //Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[mark+y,j1]+" "+file.GetLength(1));
                                        if (file[mark + y, jj - 2] == "NC" || file[mark + y, jj - 2] == "NA" || file[mark + y, jj - 2] == "-" || file[mark + y, jj - 2].Contains("Low Voltage"))
                                        {
                                            flagG = 1;
                                            //file[i, j + 1] = "0";
                                            //Console.WriteLine("Flagged3: " + " File[" + (mark + y) + "," + j1 + "]" + "==" + file[mark + y, j1]);

                                            //Console.WriteLine(j1);

                                            //Console.WriteLine(indice);
                                            if (MatchDate(file[mark + y, jj - 2]) || MatchDate1(file[mark + y, jj - 2]))
                                            {
                                                //Console.WriteLine(file[mark + y, indice] + "+++++++MArked date??" + indice);
                                                Flags.Add(file[mark + y, jj - 2]);
                                                break;
                                            }

                                            flagG = 1;
                                            //MessageBox.Show("Empiness");
                                        }

                                        marked1[y, jj - 2] = file[mark + y, jj - 2];
                                        marked1[y, jj - 1] = file[mark + y, jj - 1];

                                        //progressBar1.PerformStep();
                                    }
                                    //sl.SetCellValue(i, j - 1, "--");

                                    marked = marked1;

                                    break;
                                }
                            }

                            //MessageBox.Show("Indices: rows:"+i+"X cols:"+ (colS + j));
                            //united[i, colS + j] = file[i, j];
                            //file[i, j] = null;
                        }
                    }
                    if (flag != 1 && i == rows - 1)
                    {
                        /*marked = new string[rows, cols];
                        //Console.WriteLine("Rows: "+rows+" Cols: "+cols);
                        for (int y = 0; y < rows; y++)
                        {
                            for (int j1 = 0; j1 < cols; j1++)
                            {
                                Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[y,j1]);
                                marked[y, j1] = file[y, j1];
                            }
                        }*/
                        Console.WriteLine("Flaged");
                        flagG2 = 500;
                        Usr.Flag = true;
                        Flags.Add(file[0, 0]);
                        marked = file;
                    }
                }
            }
            Console.WriteLine("Markup2_2 EndED");
            return marked;
        }

        public string[,] MarkUp2V2(string[,] file, string fecha, bool ismpf, string sensor)
        {
            Console.WriteLine("Started MarkUp2V2_3 mit sensors");
            int cols = file.GetLength(1);
            int rows = file.GetLength(0) - 1;
            string[,] marked = null;
            int flag = 0, mark = 0;
            string test = "";
            int jj = 0;
            flagG2 = 0;
            Console.WriteLine("Cols: " + cols + " Rows: " + rows);
            if (ismpf)
            {
                for (int i = 0; i <= rows - 1; i++)
                {
                    if (flag == 1)
                    {
                        Console.WriteLine("Broken in: " + file[i - 1, jj].Substring(0, 16) + " Data: " + file[i - 1, jj + 1]);
                        break;
                    }
                    else
                    {
                        for (int j = 1; j <= cols - 1; j++)
                        {
                            //Console.WriteLine("Uni2: "+file[i,j]);
                            string text = file[i, j];
                            //Console.WriteLine("i: " + i + " j: " + j+" "+file[i,j]);

                            //MessageBox.Show(text+" != "+ errors.Text);
                            if (isDate(file[i, j]) && file[i, j].Substring(0, 16) == fecha)
                            {
                                //Console.WriteLine("TEXT: " +file[i, j].Substring(0, 16) + " != " + fecha);
                                //MessageBox.Show(file[i, j].Substring(0, 16)+" != "+fecha);

                                dataRow = j + 1;

                                //Console.WriteLine("Date: " + file[i,j]+"Data: "+file[i,j+1]+" J: "+j);
                                mark = i;
                                //jj = j;

                                jj = file.GetLength(1) - j;

                                //Console.WriteLine("JJ: " + (jj + 1));
                                flag = 1;

                                string[,] marked1 = new string[rows - mark, jj + 1];
                                // Console.WriteLine("Marked1Cols: " + marked1.GetLength(1));

                                //Console.WriteLine("is MPF");
                                for (int y = 0; y < rows - mark; y++)
                                {
                                    for (int j1 = 1; j1 <= jj + 1; j1++)
                                    {
                                        //Console.WriteLine("y: " + y + " j1 " + j1);
                                        //Console.WriteLine(" File: " + file[mark + y, j1] + " " + file.GetLength(1));
                                        if (file[mark + y, j1] == "NC" || file[mark + y, j1] == "NA")
                                        {
                                            //file[i, j + 1] = "0";
                                            Console.WriteLine("Flagged4: " + " File[" + (mark + y) + "," + j1 + "]" + "==" + file[mark + y, j1]);
                                            for (int indice = j1; indice <= j1; indice--)
                                            {
                                                Console.WriteLine(indice);
                                                if (MatchDate(file[mark + y, indice]) || MatchDate1(file[mark + y, indice]))
                                                {
                                                    Console.WriteLine(file[mark + y, indice] + "+++++++MArked date??" + indice);
                                                    Flags.Add(file[mark + y, indice]);
                                                    break;
                                                }
                                            }
                                            flagG = 1;
                                            //MessageBox.Show("Empiness");
                                        }

                                        marked1[y, j1 - 1] = file[mark + y, j1];
                                    }
                                    //progressBar1.PerformStep();
                                }

                                //sl.SetCellValue(i, j - 1, "--");

                                marked = marked1;
                                Console.WriteLine("BeforeBreak: " + test);
                                break;
                            }
                            else
                            {
                                flag = 250;
                            }

                            //MessageBox.Show("Indices: rows:"+i+"X cols:"+ (colS + j));
                            //united[i, colS + j] = file[i, j];
                            //file[i, j] = null;
                        }
                    }
                }
                if (flag == 250)
                {
                    Dates.Add(fecha);
                    sensorS.Add(sensor);
                    marked = new string[rows, cols];
                    flagG2 = 500;
                    Usr.Flag = true;
                    Console.WriteLine("NO DATE " + flagG2);
                    //Console.WriteLine("Rows: "+rows+" Cols: "+cols);
                    /*for (int y = 0; y < rows; y++)
                    {
                        for (int j1 = 0; j1 < cols; j1++)
                        {
                            //Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[y,j1]);
                            marked[y, j1] = file[y, j1];
                        }
                        //progressBar1.PerformStep();
                    }*/
                    marked = file;
                }
                else
                {
                    Console.WriteLine("Dated");
                    flagG2 = 0;
                }
            }
            else
            {
                for (int i = 0; i <= rows - 1; i++)
                {
                    if (flag == 1)
                    {
                        //Console.WriteLine("Broken in: " + file[i-1, jj].Substring(0, 16)+" Data: "+file[i-1,jj+1]);
                        break;
                    }
                    else
                    {
                        for (int j = 1; j <= cols - 1; j++)
                        {
                            //Console.WriteLine("Uni2: "+file[i,j]);
                            string text = file[i, j];
                            // Console.WriteLine("i: " + i + " j: " + j+" "+file[i,j]);

                            //MessageBox.Show(text+" != "+ errors.Text);
                            if (!String.IsNullOrEmpty(text) && MatchDate(text))
                            {
                                //Console.WriteLine("TEXT: " +file[i, j].Substring(0, 16) + " != " + fecha);
                                //MessageBox.Show(file[i, j].Substring(0, 16)+" != "+fecha);
                                if (text.Substring(0, 16) == fecha)
                                {
                                    dataRow = j + 1;

                                    //Console.WriteLine("Date: " + file[i,j]+"Data: "+file[i,j+1]+" J: "+j);
                                    mark = i;
                                    jj = j;
                                    //Console.WriteLine("JJ: "+(jj + 1));
                                    flag = 1;

                                    string[,] marked1 = new string[rows - mark, jj + 1];
                                    //Console.WriteLine("Marked1Cols: " + marked1.GetLength(1));
                                    for (int y = 0; y < rows - mark; y++)
                                    {
                                        for (int j1 = 1; j1 <= jj + 1; j1++)
                                        {
                                            //Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[mark+y,j1]+" "+file.GetLength(1));
                                            if (file[mark + y, j1] == "NC" || file[mark + y, j1] == "NA")
                                            {
                                                //file[i, j + 1] = "0";
                                                Console.WriteLine("Flagged5: " + " File[" + (mark + y) + "," + j1 + "]" + "==" + file[mark + y, j1]);
                                                for (int indice = j1; indice <= j1; indice--)
                                                {
                                                    Console.WriteLine(indice);
                                                    if (MatchDate(file[mark + y, indice]) || MatchDate1(file[mark + y, indice]))
                                                    {
                                                        Console.WriteLine(file[mark + y, indice] + "+++++++MArked date??" + indice);
                                                        Flags.Add(file[mark + y, indice]);
                                                        break;
                                                    }
                                                }
                                                flagG = 1;
                                                //MessageBox.Show("Empiness");
                                            }

                                            marked1[y, j1 - 1] = file[mark + y, j1];
                                            test = marked1[y, j1 - 1];
                                        }
                                        //progressBar1.PerformStep();
                                    }
                                    //sl.SetCellValue(i, j - 1, "--");

                                    marked = marked1;
                                    Console.WriteLine("BeforeBreak: " + test);

                                    break;
                                }
                            }

                            //MessageBox.Show("Indices: rows:"+i+"X cols:"+ (colS + j));
                            //united[i, colS + j] = file[i, j];
                            //file[i, j] = null;
                        }
                    }
                    if (flag != 1 && i == rows - 1)
                    {
                        Dates.Add(fecha);
                        sensorS.Add(sensor);
                        marked = new string[rows, cols];
                        flagG2 = 500;
                        Usr.Flag = true;
                        //Console.WriteLine("Rows: "+rows+" Cols: "+cols);
                        /*for (int y = 0; y < rows; y++)
                        {
                            for (int j1 = 0; j1 < cols; j1++)
                            {
                                //Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[y,j1]);
                                marked[y, j1] = file[y, j1];
                            }
                            //progressBar1.PerformStep();
                        }*/
                        marked = file;
                    }
                }
            }
            Console.WriteLine("Markup2_3 EndED");
            return marked;
        }

        public string[,] MarkUp2V3(string[,] file, string fecha)
        {
            //Console.WriteLine("Started MarkUp2V3" + fecha);
            int cols = file.GetLength(1);
            int rows = file.GetLength(0) - 1;
            string[,] marked = null;
            int flag = 0, mark = 0;
            int jj = 0;
            flagG2 = 0;
            //Console.WriteLine("Cols: " + cols + " Rows: " + rows);

            for (int i = 0; i < rows; i++)
            {
                if (flag == 1)
                {
                    Console.WriteLine("Broken in: " + file[i - 1, jj].Substring(0, 16) + " Data: " + file[i - 1, jj + 1]);
                    break;
                }
                else
                {
                    for (int j = 0; j < cols; j++)
                    {
                        //Console.WriteLine("Uni2: "+file[i,j]);
                        string text = file[i, j];
                        //Console.WriteLine("i: " + i + " j: " + j+" "+file[i,j]);

                        //MessageBox.Show(text+" != "+ errors.Text);
                        if (isDate(text) && MatchDate1(text))
                        {
                            //Console.WriteLine("TEXT: " +file[i, j].Substring(0, 14) + " != " + fecha);
                            //MessageBox.Show(file[i, j].Substring(0, 16)+" != "+fecha);
                            if (text.Substring(0, 14) == fecha)
                            {
                                dataRow = j + 1;

                                //Console.WriteLine("Date: " + file[i,j]+"Data: "+file[i,j+1]+" J: "+j);
                                mark = i;
                                jj = j;
                                //Console.WriteLine("JJ: "+(jj + 1));
                                flag = 1;

                                string[,] marked1 = new string[rows - mark, jj + 3];
                                //Console.WriteLine("Marked1Cols: " + marked1.GetLength(1));
                                for (int y = 0; y < rows - mark; y++)
                                {
                                    for (int j1 = 0; j1 <= jj + 2; j1++)
                                    {
                                        //Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[mark+y,j1]);
                                        if (file[mark + y, j1] == "NC" || file[mark + y, j1] == "NA")
                                        {
                                            //file[i, j + 1] = "0";
                                            Console.WriteLine("Flagged6: " + " File[" + (mark + y) + "," + j1 + "]" + "==" + file[mark + y, j1]);
                                            for (int indice = j1; indice <= j1; indice--)
                                            {
                                                Console.WriteLine(indice);
                                                if (MatchDate(file[mark + y, indice]) || MatchDate1(file[mark + y, indice]))
                                                {
                                                    Console.WriteLine(file[mark + y, indice] + "+++++++MArked date??" + indice);
                                                    Flags.Add(file[mark + y, indice]);
                                                    break;
                                                }
                                            }
                                            flagG = 1;
                                            //MessageBox.Show("Empiness");
                                        }

                                        marked1[y, j1] = file[mark + y, j1];
                                    }
                                    //progressBar1.PerformStep();
                                }
                                //sl.SetCellValue(i, j - 1, "--");

                                marked = marked1;

                                break;
                            }
                        }

                        //MessageBox.Show("Indices: rows:"+i+"X cols:"+ (colS + j));
                        //united[i, colS + j] = file[i, j];
                        //file[i, j] = null;
                    }
                }
                if (flag != 1 && i == rows - 1)
                {
                    Console.WriteLine("NoDate??");
                    flagG2 = 500;
                    Usr.Flag = true;
                    marked = file;
                }
            }
            //Console.WriteLine("Markup2V3 EndED");
            return marked;
        }

        public string[,] MarkUp21(string[,] file, string fecha)
        {//We're here
            //Console.WriteLine("Started MarkUp21");
            int cols = file.GetLength(1);
            int rows = file.GetLength(0) - 1;
            string[,] marked = null;
            int flag = 0, mark = 0;
            int jj = 0;
            //Console.WriteLine("Cols: " + cols + " Rows: " + rows);
            flagG2 = 0;
            for (int i = 0; i < rows; i++)
            {
                if (flag == 1)
                {
                    //Console.WriteLine("Broken in: " + file[i-1, jj].Substring(0, 16)+" Data: "+file[i-1,jj+1]);
                    break;
                }
                else
                {
                    for (int j = 0; j < cols; j++)
                    {
                        //Console.WriteLine("Uni2: "+file[i,j]);
                        string text = file[i, j];
                        if (!String.IsNullOrEmpty(text))
                        {
                            //Console.WriteLine("i: " + i + " j: " + j+" "+file[i,j]);

                            //MessageBox.Show(text+" != "+ errors.Text);
                            if (MatchDate(text))
                            {
                                //Console.WriteLine("TEXT: " +file[i, j].Substring(0, 16) + " != " + fecha);
                                //MessageBox.Show(file[i, j].Substring(0, 16)+" != "+fecha);
                                if (text.Substring(0, 16) == fecha)
                                {
                                    dataRow = j + 1;

                                    //Console.WriteLine("Date: " + file[i,j]+"Data: "+file[i,j+1]+" J: "+j);
                                    mark = i;
                                    jj = j;
                                    //Console.WriteLine("MaxCol: "+(MaxCol));
                                    flag = 1;

                                    string[,] marked1 = new string[rows - mark, jj + 3];
                                    //Console.WriteLine("Marked1Cols: " + marked1.GetLength(1));
                                    for (int y = 0; y < rows - mark; y++)
                                    {
                                        for (int j1 = 0; j1 < jj + 3; j1++)
                                        {
                                            //Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[mark+y,j1]+" "+file.GetLength(1));
                                            if (file[mark + y, j1] == "NC" || file[mark + y, j1] == "NA" || file[mark + y, j1] == "-")
                                            {
                                                flagG = 1;
                                                //file[i, j + 1] = "0";
                                                //Console.WriteLine("Flagged3: " + " File[" + (mark + y) + "," + j1 + "]" + "==" + file[mark + y, j1]);

                                                //Console.WriteLine(j1);
                                                for (int indice = j1; indice <= j1; indice--)
                                                {
                                                    //Console.WriteLine(indice);
                                                    if (MatchDate(file[mark + y, indice]) || MatchDate1(file[mark + y, indice]))
                                                    {
                                                        //Console.WriteLine(file[mark + y, indice] + "+++++++MArked date??" + indice);
                                                        Flags.Add(file[mark + y, indice]);
                                                        break;
                                                    }
                                                }
                                                flagG = 1;
                                                //MessageBox.Show("Empiness");
                                            }
                                            marked1[y, j1] = file[mark + y, j1];
                                        }
                                        //progressBar1.PerformStep();
                                    }
                                    //sl.SetCellValue(i, j - 1, "--");

                                    marked = marked1;

                                    break;
                                }
                            }
                        }

                        //MessageBox.Show("Indices: rows:"+i+"X cols:"+ (colS + j));
                        //united[i, colS + j] = file[i, j];
                        //file[i, j] = null;
                    }
                }
                if (flag != 1 && i == rows - 1)
                {
                    /*marked = new string[rows, cols];
                    //Console.WriteLine("Rows: "+rows+" Cols: "+cols);
                    for (int y = 0; y < rows; y++)
                    {
                        for (int j1 = 0; j1 < cols; j1++)
                        {
                            Console.WriteLine("y: " + y+" j1 "+j1+" File: "+file[y,j1]);
                            marked[y, j1] = file[y, j1];
                        }
                    }*/
                    Console.WriteLine("Flaged");
                    flagG2 = 500;
                    Usr.Flag = true;
                    Flags.Add(file[0, 0]);
                    marked = file;
                }
            }
            //Console.WriteLine("Markup21 EndED");
            return marked;
        }

        public void DuplicaV2(string[,] temp, int indice, int iTemp, int prevI, int col)
        {
            int cols = MaxCol;
            iTemp = prevI;
            int rows = indice;
            if (rows < 0)
            {
                rows = 0;
            }
            string[,] uni = new string[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                uni[i, col - 1] = temp[iTemp + i, col];
                uni[i, col] = temp[iTemp + i, MaxCol];
                if (uni[i, col - 1] == "NC")
                {
                    Console.WriteLine(uni[i, col - 1]);
                }
            }

            Uni.Add(uni);
        }

        public void DuplicaV2HUM(string[,] temp, int indice, int iTemp, int prevI, int col)
        {
            //int cols = temp.GetLength(1);
            int cols = MaxCol;
            iTemp = prevI;
            Console.WriteLine("Start DuplicaV2HUM " + temp.GetLength(1) + " " + col);
            Console.WriteLine("Cols:" + cols);
            int rows = indice;
            if (rows < 0)
            {
                rows = 0;
            }
            //Console.WriteLine("Rows:" + rows);
            string[,] uni = new string[rows, cols];

            //Console.WriteLine("Duplica---------------------------------------------------------------------------------------------------");
            for (int i = 0; i < rows; i++)
            {
                for (int j = col - 1; j < cols; j++)
                {
                    //Console.WriteLine("i: "+i+ " j: "+j+ "Temp: " + temp[iTemp + i, j]);

                    //Console.WriteLine("i: "+i+ " j: "+j);
                    uni[i, j] = temp[iTemp + i, j + 1];
                    if (uni[i, j] == "NC")
                    {
                        Console.WriteLine(uni[i, j]);
                    }
                }
            }
            //uni = MarkUp(uni,fecha);
            //Console.WriteLine("DataRow: " + dataRow);
            Uni.Add(uni);
            //Write(uni,"PruebaUni"+indice+".csv");
            //Console.WriteLine("Sleeping");
            //Thread.Sleep(3000000);
            //Console.WriteLine("End DuplicaV2");
            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }

        public void DuplicaIV2(string[,] temp, int indice, int iTemp, ProgressBar progressBar, int col)
        {
            Bar(progressBar, indice, 1);
            //Console.WriteLine("Start DuplicaIV2");
            //int cols = temp.GetLength(1) ;
            int cols = MaxCol;
            //iTemp = prevI;
            //Console.WriteLine("Cols:" + cols);
            int rows = indice;
            if (rows < 0)
            {
                rows = 0;
            }
            //Console.WriteLine("Rows:" + rows);
            string[,] uni = new string[rows, cols];

            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            for (int i = 0; i < rows; i++)
            {
                uni[i, col - 1] = temp[iTemp + i, col];
                uni[i, col] = temp[iTemp + i, MaxCol];
                if (uni[i, col - 1] == "NC")
                {
                    Console.WriteLine(uni[i, col - 1]);
                }
                //progressBar.PerformStep();
            }
            //uni = MarkUp(uni,fecha);
            //Console.WriteLine("DataRow: " + dataRow);
            Uni.Add(uni);
            //Write(uni,"PruebaUni"+indice+".csv");
            //Console.WriteLine("Sleeping");
            //Thread.Sleep(3000000);
            //Console.WriteLine("End DuplicaIV2");
            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }

        public void DuplicaIV2Hum(string[,] temp, int indice, int iTemp, ProgressBar progressBar, int col)
        {
            Bar(progressBar, indice, 1);
            Console.WriteLine("Start DuplicaIV2Hum");
            //int cols = temp.GetLength(1) ;
            int cols = MaxCol;
            //iTemp = prevI;
            //Console.WriteLine("Cols:" + cols);
            int rows = indice;
            if (rows < 0)
            {
                rows = 0;
            }
            //Console.WriteLine("Rows:" + rows);
            string[,] uni = new string[rows, cols];

            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            for (int i = 0; i < rows; i++)
            {
                for (int j = col - 1; j < cols; j++)
                {
                    //Console.WriteLine("i: "+i+ " j: "+j+ "Temp: " + temp[iTemp + i, j]);

                    //Console.WriteLine("i: "+i+ " j: "+j);
                    uni[i, j] = temp[iTemp + i, j + 1];
                }
                //progressBar.PerformStep();
            }
            //uni = MarkUp(uni,fecha);
            //Console.WriteLine("DataRow: " + dataRow);
            Uni.Add(uni);
            //Write(uni,"PruebaUni"+indice+".csv");
            //Console.WriteLine("Sleeping");
            //Thread.Sleep(3000000);
            //Console.WriteLine("End DuplicaIV2");
            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }

        public void DuplicaIV2_2(string[,] temp, int indice, int iTemp, ProgressBar progressBar, int col)
        {
            Bar(progressBar, indice, 1);
            //Console.WriteLine("DuplicaIV2_2");
            int cols = temp.GetLength(1);
            int cols1 = MaxCol;
            //iTemp = prevI;
            //Console.WriteLine("Cols:" + cols);
            int rows = indice;

            if (rows < 0)
            {
                rows = 0;
            }
            //Console.WriteLine("Rows:" + rows);
            string[,] uni = new string[rows, cols];

            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            for (int i = 0; i < rows; i++)
            {
                for (int j = col; j < cols; j++)
                {
                    //Console.WriteLine("i: "+i+ " j: "+j+ "Temp: " + temp[iTemp + i, j]);

                    //Console.WriteLine("i: "+i+ " j: "+j);
                    uni[i, j] = temp[iTemp + i, j];
                }
                //progressBar.PerformStep();
            }
            //uni = MarkUp(uni,fecha);
            //Console.WriteLine("DataRow: " + dataRow);
            Uni.Add(uni);
            //Write(uni,"PruebaUni"+indice+".csv");
            //Console.WriteLine("Sleeping");
            //Thread.Sleep(3000000);
            //Console.WriteLine("End DuplicaIV2");
            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }

        public void DuplicaIV2_2Hum(string[,] temp, int indice, int iTemp, ProgressBar progressBar, int col)
        {
            Bar(progressBar, indice, 1);
            Console.WriteLine("DuplicaIV2_2HUM");
            int cols = temp.GetLength(1);
            int cols1 = MaxCol;
            //iTemp = prevI;
            //Console.WriteLine("Cols:" + cols);
            int rows = indice;

            if (rows < 0)
            {
                rows = 0;
            }
            //Console.WriteLine("Rows:" + rows);
            string[,] uni = new string[rows, cols];

            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
            for (int i = 0; i < rows; i++)
            {
                for (int j = col + 1; j < cols - 1; j++)
                {
                    //Console.WriteLine("i: "+i+ " j: "+j+ "Temp: " + temp[iTemp + i, j]);

                    //Console.WriteLine("i: "+i+ " j: "+j);
                    uni[i, j] = temp[iTemp + i, j];
                }
                //progressBar.PerformStep();
            }
            //uni = MarkUp(uni,fecha);
            //Console.WriteLine("DataRow: " + dataRow);
            Uni.Add(uni);
            //Write(uni,"PruebaUni"+indice+".csv");
            //Console.WriteLine("Sleeping");
            //Thread.Sleep(3000000);
            //Console.WriteLine("End DuplicaIV2");
            //Console.WriteLine("---------------------------------------------------------------------------------------------------------");
        }

        //ABRE EL ARCHIVO SELECCIONADO QUE PROVIENE DE EXCEL
        public void ReadXlsxTemp(OpenFileDialog openFileDialog1, ProgressBar progressBar1)
        {
            Console.WriteLine("Start ReadXlsxTemp");
            //Lectura Archivos xlsx
            foreach (string file in openFileDialog1.FileNames)
            {
                int indice = 0;
                List<string> archivos = new List<string>();

                archivos.Add(Path.GetFullPath(file));

                SLDocument firstdoc = new SLDocument(archivos[indice]);
                string sheet = firstdoc.GetWorksheetNames()[0];

                /*Código para recorrer las páginas del documento
                 * foreach (var name in firstdoc.GetWorksheetNames())
                {
                    sheet = name;
                    //Console.WriteLine(name);
                    break;
                }*/
                //firstdoc.CloseWithoutSaving();
                //SLDocument firstdoc = new SLDocument(archivos[indice], "Hoja1");
                firstdoc = new SLDocument(archivos[indice], sheet);
                //firstdoc = new SLDocument(archivos[indice], sheet);
                var stats = firstdoc.GetWorksheetStatistics(); //Información del documento
                var columnsCount = stats.NumberOfColumns;//Cantidad de columnas
                var rowsCount = stats.NumberOfRows;//Cantidad de filas

                int count = 0;
                Bar(progressBar1, rowsCount, 1);//Animación de barra de progreso

                string[,] temp = new string[rowsCount + 1, columnsCount + 1];//Temporal de lecturas

                int prevI = 0;
                int ii = 0;
                int col = 0;
                string test1;
                int totalCols = columnsCount + 1;
                for (int i = 0; i < rowsCount + 1; i++)
                {
                    for (int j = 0; j < totalCols; j++)
                    {
                        var test = firstdoc.GetCellStyle(i, j);//Estilo(formato) de celda a actual
                        test1 = firstdoc.GetCellValueAsString(i, j);//Valor de la celda actual

                        if (test1 == "Serial Number" || test1 == "S/N" || test1 == "Número de serie")
                        {
                            Sensors.Add(firstdoc.GetCellValueAsString(i, j + 1));//conteo de sensores
                        }
                        else if (test1.Contains("OM-CP-"))
                        {
                            Sensors.Add(firstdoc.GetCellValueAsString(i + 2, j));//Conteo de sensores
                        }
                        else if (test1 == "Time" || test1 == "Tiempo")
                        {
                            //Ancla de lecturas Elitech
                            //De esta fila hacia atras se recorrerá el temporal para recolectar las lecturas

                            MaxCol = j + 1;//Maximo de columnas a recorrer, 0 para fecha, 1 para valor de la lectura
                            col = j;
                            indice = i - prevI;

                            DuplicaV2(temp, indice - 27, i, prevI, col);//Recorrido de valores, aqui se crea el headder que se eliminará el termino de esta rutina
                            ii = i;
                            j = 0;
                            i++;
                            count++;
                            flag++;

                            prevI = i;
                        }
                        else if (test1.Contains("Fecha"))
                        {
                            //Ancla de Lecturas RTD
                            MaxCol = columnsCount;
                            col = j;
                            indice = i - prevI;
                            isMPF = true;

                            ii = i;
                            j = 0;
                            i++;
                            count++;

                            prevI = i;
                        }
                        else if (test1.Contains("VALIDATOR"))
                        {
                            //Ancla de lecturas de Validator
                            totalCols = 37;
                            Usr.IsValidator = true;
                            temp = new string[rowsCount + 1, totalCols];//Redefinición del temporal, Validator son máximo 18 canales por análisis
                        }
                        else
                        {
                            //Añadir valores al temporal
                            //Solo se añaden valores que sean fechas o valores numéricos
                            if ((test.FormatCode == "General" || test.FormatCode == "0.0" || test.FormatCode == "#0.00") && MatchFloat(test1))
                            {
                                //Valores Numéricos
                                temp[i, j] = test1;
                            }
                            else if ((isDate(test1) || test.FormatCode.Contains("hh")) && IsValid(test1))
                            {
                                //Fechas
                                temp[i, j] = DateFormat(firstdoc.GetCellValueAsDateTime(i, j));
                            }
                        }
                        test1 = null;
                    }

                    if (i == rowsCount)
                    {
                        //Última lectura al final de las filas
                        int tt = rowsCount - ii;

                        Bar(progressBar1, indice, 1);//Animación de barra de progreso
                        //Rutina dependiendo de si es macro o no
                        if (flag == 0)
                        {
                            DuplicaIV2_2(temp, tt, ii + 1, progressBar1, col);
                        }
                        else
                        {
                            DuplicaIV2(temp, tt, ii + 1, progressBar1, col);
                        }
                    }
                }

                progressBar1.PerformStep();

                firstdoc = null;

                temp = null;
            }
        }

        public void NewReadV2_2HUM(OpenFileDialog openFileDialog1, ProgressBar progressBar1)
        {
            Console.WriteLine("Start NewReadV2_2Hum");
            foreach (string file in openFileDialog1.FileNames)
            {
                int indice = 0;
                List<string> archivos = new List<string>();
                //Console.WriteLine("Archivo: " + progressBar1.Value);
                //message +=/* Path.GetFileName(file) + " - " +*/ file + Environment.NewLine;
                archivos.Add(Path.GetFullPath(file));

                /*SLDocument firstdoc = new SLDocument(archivos[indice]);
                string sheet = "";

                foreach (var name in firstdoc.GetWorksheetNames())
                {
                    sheet = name;
                    //Console.WriteLine(name);
                    break;
                }*/
                //firstdoc.CloseWithoutSaving();
                SLDocument firstdoc = new SLDocument(archivos[indice], "Hoja1");
                //firstdoc = new SLDocument(archivos[indice], sheet);
                var stats = firstdoc.GetWorksheetStatistics(); //sheet::SLDocument
                var columnsCount = stats.NumberOfColumns;
                var rowsCount = stats.NumberOfRows;
                //Console.WriteLine("Cols: "+columnsCount);
                //Console.WriteLine("Rows: "+rowsCount);

                int count = 0;
                Bar(progressBar1, rowsCount, 1);

                string[,] temp = new string[rowsCount + 1, columnsCount + 1];

                //MessageBox.Show("rows:" + temp.Length);
                //MessageBox.Show("cols: "+temp.Rank);
                int prevI = 0;
                int ii = 0;
                int col = 0;
                string test1;

                for (int i = 0; i < rowsCount; i++)
                {
                    //dataGridView1.Rows.Add();
                    for (int j = 0; j < columnsCount - 1; j++)
                    {
                        var test = firstdoc.GetCellStyle(i, j);
                        test1 = firstdoc.GetCellValueAsString(i, j);
                        //Console.WriteLine("I: "+i+" J: "+j+"data?: "+firstdoc.GetCellValueAsString(i, j));
                        if (test1 == "Serial Number" || test1 == "S/N" || test1 == "Número de serie")
                        {
                            //Console.WriteLine("Trip Description: "+ firstdoc.GetCellValueAsString(i, j + 1));
                            Sensors.Add(firstdoc.GetCellValueAsString(i, j + 1));
                            //Console.WriteLine(firstdoc.GetCellValueAsString(i, j + 1));
                        }
                        /*else if (firstdoc.GetCellValueAsString(i, j).Contains("Número de Serie:")) {
                            Sensors.Add(firstdoc.GetCellValueAsString(i, j + 2));
                           //Console.WriteLine(firstdoc.GetCellValueAsString(i, j + 2));
                        }*/
                        else if (test1.Contains("OM-CP-"))
                        {
                            Sensors.Add(firstdoc.GetCellValueAsString(i + 2, j));
                            Console.WriteLine(firstdoc.GetCellValueAsString(i + 2, j));
                        }
                        else if (test1 == "Time" || test1 == "Tiempo")
                        {
                            //Console.WriteLine("Style Time: " + firstdoc.GetCellStyle(i + 1, j).FormatCode);
                            //Console.WriteLine("Style Temp: " + firstdoc.GetCellStyle(i + 1, j + 1).FormatCode);

                            MaxCol = j + 2;
                            col = j;
                            indice = i - prevI;

                            //Duplica(temp, indice - 27, i, prevI);
                            DuplicaV2HUM(temp, indice - 27, i, prevI, col);
                            ii = i;
                            j = 0;
                            i++;
                            count++;
                            flag++;

                            prevI = i;
                            //indice = 0;
                            //dataGridView1.Rows[i-1].Cells[j-1].Value = firstdoc.GetCellValueAsString(i, j);
                        }
                        else if (test1.Contains("Fecha"))
                        {
                            //Console.WriteLine("Fecha");
                            MaxCol = columnsCount;
                            col = j;
                            indice = i - prevI;
                            isMPF = true;
                            //Console.WriteLine("Style Fecha: "+firstdoc.GetCellStyle(i + 1, j ).FormatCode);
                            //Console.WriteLine("Style Tiempo: "+firstdoc.GetCellStyle(i + 1, j + 1).FormatCode);
                            //Console.WriteLine("Style Temp: "+firstdoc.GetCellStyle(i + 1, j + 2).FormatCode);
                            //Duplica(temp, indice - 27, i, prevI);
                            //DuplicaV2(temp, indice - 27, i, prevI, col);
                            ii = i;
                            j = 0;
                            i++;
                            count++;

                            prevI = i;
                        }
                        else
                        {
                            //Console.WriteLine("Format:"+test.FormatCode+" data: "+test1);
                            if ((test.FormatCode == "General" || test.FormatCode == "0.0") && MatchFloat(test1))
                            {
                                //Console.WriteLine("Num?? " + test1 + " Format:" + test.FormatCode);
                                temp[i, j] = test1;
                            }

                            /*else if (test.FormatCode == "dd/mm/yyyy")
                            {
                                //Console.WriteLine();
                            }*/
                            else if ((isDate(test1) || test.FormatCode.Contains("hh")) && IsValid(test1))
                            {
                                //Console.WriteLine("Format2DT:" + test.FormatCode + " data: " + test1);

                                //Console.WriteLine(DateFormat(firstdoc.GetCellValueAsDateTime(i, j)));

                                temp[i, j] = DateFormat(firstdoc.GetCellValueAsDateTime(i, j));
                            }
                            /* else
                             {
                                 Console.WriteLine("Format??:" + test.FormatCode + " data: " + test1);

                                 temp[i, j] = test1;
                             }*/
                        }
                        test1 = null;
                    }

                    if (i == rowsCount - 1)
                    {
                        //Console.WriteLine("EndFile");
                        int tt = rowsCount - ii;
                        //MessageBox.Show("total-ii: " + (rowsCount - ii) + " indice: " + indice + " i: " + ii);
                        //progressBar1.Value = progressBar1.Value - indice;
                        Bar(progressBar1, indice, 1);
                        //Console.WriteLine(i+" != "+ii);
                        //Console.WriteLine("FLAG: "+flag);
                        if (flag == 0)
                        {
                            //Console.WriteLine("DuplicaIV2_2??");
                            DuplicaIV2_2Hum(temp, tt, ii + 1, progressBar1, col);
                        }
                        else
                        {
                            //Console.WriteLine("DuplicaIV2??");
                            DuplicaIV2Hum(temp, tt, ii + 1, progressBar1, col);
                        }
                    }
                }
                //archivos[indice] = null;
                progressBar1.PerformStep();
                firstdoc.CloseWithoutSaving();

                //Console.WriteLine("FILE: " + count);
                temp = null;
            }
            Console.WriteLine("End NewReadV2_2 ");
        }

        public void NewReadOm(OpenFileDialog openFileDialog1, ProgressBar progressBar1)
        {
            //Lecturas Omega
            int indice = 0;
            List<string> archivos = new List<string>();

            foreach (string file in openFileDialog1.FileNames)
            {
                List<string[,]> temps2 = new List<string[,]>();

                archivos.Add(Path.GetFullPath(file));

                using (var reader = new StreamReader(archivos[indice]))
                {
                    int signal = 0;//Conteo de lineas para agregar datos despues del headder
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');//SEparar lecturas
                        string[,] temp = new string[1, values.Count()];
                        if (line.ToString().Contains("INO"))
                        {
                            //Console.WriteLine(line.Substring(27));
                            Sensors.Add(line.Substring(0, line.Length / 2));//Conteo de sensores
                        }
                        else if (signal >= 12)
                        {
                            for (int i = 1; i < values.Count(); i++)
                            {
                                if (i != values.Count() - 1)
                                {
                                    temp[0, i] = values[i];
                                }
                                else
                                {
                                    //Construcción de fecha en el formato correcto
                                    temp[0, 0] = values[values.Count() - 1];
                                    DateTime dd = ToDate(temp[0, 0]);
                                    var day = dd.Day;
                                    var month = dd.Month;
                                    var year = dd.Year;
                                    var hours = dd.Hour;
                                    var minutes = dd.Minute;
                                    var seconds = dd.Second;
                                    DateTime dt4 = new DateTime(year, day, month, hours, minutes, seconds, DateTimeKind.Utc);

                                    temp[0, 0] = DateFormat1(dt4);
                                }
                            }
                            temps2.Add(temp);
                        }

                        signal++;
                    }
                }

                TEMPS.Add(temps2);

                indice++;
            }

            ToUni(TEMPS, 5);//Agregar datos a Temporal

            //Console.WriteLine("End NewReadOm");
        }

        public void NewReadSensHum(OpenFileDialog openFileDialog1, ProgressBar progressBar1)
        {
            Console.WriteLine("Start NewReadSensHum");
            int indice = 0;
            List<string> archivos = new List<string>();
            foreach (string file in openFileDialog1.FileNames)
            {
                //System.IO.StreamReader opened = new System.IO.StreamReader(@Path.GetFullPath(file));

                var onlyFileName = System.IO.Path.GetFileName(file);
                string[] name = onlyFileName.Split('.');

                Console.WriteLine("FileName??:   " + name[0]);

                Sensors.Add(name[0]);
                archivos.Add(Path.GetFullPath(file));
                List<string> dates = new List<string>();
                List<string> temp = new List<string>();
                List<string> hum = new List<string>();

                //string[,] temp = new string[1, 2];

                using (var reader = new StreamReader(archivos[indice]))
                {
                    int signal = 0;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        //Dos temp y unirlos en un temps2 antes de subir a TEMPS
                        string date = "", number = "", hume = "";
                        if (line.Contains("<Time>") && line.Contains("</Time>"))
                        {
                            string[] parts = line.Substring(12, 25).Split('T');
                            string[] part2 = parts[1].Split('-');
                            date = parts[0] + " " + part2[0];

                            dates.Add(date);
                            //Console.WriteLine(temp[0, 0]);
                        }
                        else if (line.Contains("<Value1>") && line.Contains("</Value1>"))
                        {
                            string[] values = line.Substring(14).Split('<');

                            number = values[0];

                            //Console.WriteLine(values[0]+" "+values[1]);
                            temp.Add(number);
                            //Array.Clear(temp,0,temp.Length);
                        }
                        else if (line.Contains("<Value2>") && line.Contains("</Value2>"))
                        {
                            string[] values = line.Substring(14).Split('<');

                            hume = values[0];

                            //Console.WriteLine(values[0]+" "+values[1]);
                            hum.Add(hume);
                            //Array.Clear(temp,0,temp.Length);
                        }

                        signal++;
                    }
                }

                //Console.WriteLine("# of elements: " + temps.Count.ToString());
                string[,] DatesUndNumbers = new string[dates.Count, 3];
                Console.WriteLine("Dates: " + dates.Count + " Temperatura: " + temp.Count + " Humedad: " + hum.Count);
                for (int i = 0; i < dates.Count; i++)
                {
                    DatesUndNumbers[i, 0] = dates[i];
                    DatesUndNumbers[i, 1] = temp[i];
                    DatesUndNumbers[i, 2] = hum[i];
                }
                Uni.Add(DatesUndNumbers);
                //TEMPS.Add();
                indice++;
                Console.WriteLine(indice);
                //temps2.Clear();
                //Console.WriteLine(temp[0,1]);
            }
            //ToUni2(TEMPS, 3);

            Console.WriteLine("End NewReadSensHum " + MaxCol);
        }

        public void NewReadSens(OpenFileDialog openFileDialog1, ProgressBar progressBar1)
        {
            //Recolección de Datos Elitech
            int indice = 0;
            List<string> archivos = new List<string>();
            foreach (string file in openFileDialog1.FileNames)
            {
                var onlyFileName = System.IO.Path.GetFileName(file);
                string[] name = onlyFileName.Split('.');

                Sensors.Add(name[0]);//Conteo de sensores
                archivos.Add(Path.GetFullPath(file));
                //Definición de dos temporales Columna de fechas, columna de valores
                List<string> dates = new List<string>();
                List<string> numbers = new List<string>();

                using (var reader = new StreamReader(archivos[indice]))
                {
                    int signal = 0;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        string date = "", number = "";
                        //Selección de información
                        if (line.Contains("<Time>") && line.Contains("</Time>"))
                        {
                            //Selección de fechas
                            string[] parts = line.Substring(12, 25).Split('T');
                            string[] part2 = parts[1].Split('-');
                            date = parts[0] + " " + part2[0];

                            dates.Add(date);
                        }
                        else if (line.Contains("<Value1>") && line.Contains("</Value1>"))
                        {
                            //Selección de Valores
                            string[] values = line.Substring(14).Split('<');

                            number = values[0];

                            numbers.Add(number);
                        }

                        signal++;
                    }
                }

                //Unificación de columnas
                string[,] DatesUndNumbers = new string[dates.Count, 2];
                Console.WriteLine("Dates: " + dates.Count + " Numbers: " + numbers.Count);
                for (int i = 0; i < dates.Count; i++)
                {
                    DatesUndNumbers[i, 0] = dates[i];
                    DatesUndNumbers[i, 1] = numbers[i];
                }
                Uni.Add(DatesUndNumbers);

                indice++;
            }
        }

        public void Bar(ProgressBar progressBar, int max, int start)
        {
            try
            {
                progressBar.Visible = true;
                // Set Minimum to 1 to represent the first file being copied.
                progressBar.Minimum = 1;
                // Set Maximum to the total number of files to copy.
                if (max < 0)
                {
                    progressBar.Maximum = 0;
                }
                else
                {
                    progressBar.Maximum = max;
                }

                // Set the initial value of the ProgressBar.
                if (max < 0)
                {
                    progressBar.Value = 0;
                }
                else
                {
                    progressBar.Value = max;
                }
                // Set the Step property to a value of 1 to represent each file being copied.
                progressBar.Step = 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        [ThreadStatic] public List<string> Dates = new List<string>();
        [ThreadStatic] public List<string> sensorS = new List<string>();

        public bool LogDate(string date, List<string> dates)
        {
            int flag = 0;

            for (int i = 0; i < dates.Count; i++)
            {
                if (date == dates[i])
                {
                    flag++;
                }
            }
            if (flag == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool IsInDates(int date, List<int> dates)
        {
            int flag = 0;

            for (int i = 0; i < dates.Count; i++)
            {
                if (date == dates[i])
                {
                    flag++;
                }
            }
            if (flag == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void NewRead3(string fecha, List<string[,]> uni, ProgressBar progressBar1, int hum)
        {
            Console.WriteLine("Start NewRead3+4");
            flagG = 0;
            Bar(progressBar1, uni.Count, 1);
            try
            {
                foreach (string[,] sensor in uni)
                {
                    if (sensor != null)
                    {
                        Console.WriteLine("Hum: " + hum);
                        Uni2.Add(MarkUp2V3(sensor, fecha));
                        if (flagG2 == 500)
                        {
                            MessageBox.Show("Un sensor NO contiene la fecha selecionada.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Uni Empty");
                    }
                    progressBar1.PerformStep();
                }
                Console.WriteLine("End NewRead3");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        //lectura de datos para cambio de fecha?
        public void NewRead33(string fecha, List<string[,]> uni, ProgressBar progressBar1, int hum, bool ismpf, List<string> sens)
        {
            Console.WriteLine("Start NewRead33+i " + ismpf);
            flagG2 = 0;
            Bar(progressBar1, uni.Count, 1);
            try
            {
                int i = 0, inc = 0; ;

                Console.WriteLine("TOTAL: " + uni.Count);

                for (i = inc; i < uni.Count; i++)
                {
                    if (uni[i] != null)
                    {
                        if (hum == 0)
                        {
                            Console.WriteLine("I: " + i);
                            if (ismpf)
                            {
                                Console.WriteLine("IsNullOrEmptySensor: " + i);
                                Uni2.Add(MarkUp2V2(uni[i], fecha, ismpf, sensors[i]));
                            }
                            else
                            {
                                Uni2.Add(MarkUp2V2(uni[i], fecha, ismpf));
                                if (flagG2 == 500 && sens.Count > 0)
                                {
                                    MessageBox.Show("El Sensor (" + sens[i] + ") " + (i + 1) + "\n NO contiene la fecha selecionada.");
                                    //flagG2 = 0;
                                }
                                else if (flagG2 == 500)
                                {
                                    MessageBox.Show("Fecha no encontrada");
                                }
                            }
                        }
                        else if (hum == 3)
                        {
                            Console.WriteLine("Hum: " + hum);
                            Uni2.Add(MarkUp2V3(uni[i], fecha));
                            if (flagG2 == 500)
                            {
                                MessageBox.Show("Un sensor NO contiene la fecha selecionada.");
                                //flagG2 = 0;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Hum: " + hum);
                            Uni2.Add(MarkUp21(uni[i], fecha));
                            if (flagG2 == 500)
                            {
                                MessageBox.Show("El Sensor (" + sens[i] + ") " + (i + 1) + "\n NO contiene la fecha selecionada.");
                                //flagG2 = 0;
                                FlaggedSensors.Add(i);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Uni Empty");
                    }
                }

                Console.WriteLine("End NewRead33+i");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void NewRead3(string fecha, List<string[,]> uni, ProgressBar progressBar1, int hum, bool ismpf, List<string> sensores)
        {
            //Corte de sensores

            Bar(progressBar1, uni.Count, 1);
            try
            {
                for (int i = 0; i < uni.Count; i++)
                {
                    if (uni[i] != null)
                    {
                        if (hum == 0)
                        {
                            Uni2.Add(MarkUp2V2(uni[i], fecha, ismpf));//Corte

                            if (flagG2 == 500)
                            {
                                MessageBox.Show("Un sensor " + sensores[i] + " NO contiene la fecha selecionada.");
                                flagG2 = 0;
                            }
                        }
                        else if (hum == 3)
                        {
                            Console.WriteLine("Hum: " + hum + flagG2);
                            Uni2.Add(MarkUp2V3(uni[i], fecha));
                            Console.WriteLine("FG2: " + flagG2);
                            if (flagG2 == 500)
                            {
                                MessageBox.Show("Un sensor NO contiene la fecha selecionada.");
                                flagG2 = 0;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Hum: " + hum);
                            Uni2.Add(MarkUp21(uni[i], fecha));
                            if (flagG2 == 500)
                            {
                                MessageBox.Show("Un sensor NO contiene la fecha selecionada.");
                                flagG2 = 0;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Uni Empty");
                    }
                }

                Console.WriteLine("End NewRead3");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        //EXPRESIONES REGULARES - BUSCA COINCIDENCIA EN EL TEXTO
        public bool MatchFloat(string text)
        {
            string flot = @"^[+-]?\d+(\.\d+)?$";// expresiones regulares 
            //string input = firstdoc.GetCellValueAsString(i, j);
            Match m = Regex.Match(text, flot, RegexOptions.IgnoreCase);
            return m.Success; //RETORNA SI EXISTE EN LA FRASE 
        }

        /* EXPRESIONES REGULARES
         * [+-]? CERO O SIGNO POSITIVO O NEGATIVO 
         * ^ INICIO DE FRASE
         * \d{2} REPETICIONES DE CUALQUIER NUMERO POR EL INTERVALO
         * 
         *         
             */
        //VERIFICA EL FORMATO DE LA FECHA, comprara el formato este en yyyy-MM-dd hh:mm:ss para no cambiar el valor de la temperatura o humedad
        public bool MatchDate(string text)
        {
            //Console.WriteLine("MatchDate "+text);
            string date = @"^(\d{4})-(\d{2})-(\d{2}) (\d{2}):(\d{2}):(\d{2})$"; // yyyy/MM/dd
            string date2 = @"^(\d{2})\/(\d{2})\/(\d{4}) (\d{2}):(\d{2}):(\d{2})$"; // dd/MM/yyyy
            Match m = Regex.Match(text, date, RegexOptions.IgnoreCase);
            if (m.Success || Regex.Match(text, date2, RegexOptions.IgnoreCase).Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MatchDate1(string text)
        {
            string date = @"^(\d{2})\/(\d{2})\/(\d{4}) (\d{2}):(\d{2}):(\d{2})\.0$";

            Match m = Regex.Match(text, date, RegexOptions.IgnoreCase);
            return m.Success;
        }

        //CONVIERTE FECHA
        public DateTime ToDate(string date)
        {
            DateTime.TryParse(date, out DateTime datum);
            return datum;
        }

        public string DateFormat(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public string DateFormat1(DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss.f");
        }

        public string Date2(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public bool DeleteTable(int id, string table)
        {
            string sql = "Delete from " + table + " Where Id='" + id + "';";

            SqlConnection conn = new SqlConnection(connection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(sql, conn);
            int temp = Convert.ToInt32(cmd.ExecuteNonQuery().ToString());

            if (temp > 0)
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

        public string DeleteWS(string txt)
        {
            string t = txt.Trim();
            t = t.Replace(" ", "");
            return t;
        }

        public void Start()
        {
            //Funcion para mostrar la pantalla Inicial
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                //iterate through
                if (frm.Name == "Start")
                {
                    foreach (Control cctr in frm.Controls)
                    {
                        if (cctr is CheckedListBox)
                        {
                            cctr.Enabled = true;
                        }
                        else if (cctr is ProgressBar)
                        {
                            cctr.Visible = false;
                        }
                        else if (cctr is Button)
                        {
                            cctr.Enabled = false;
                        }
                    }
                    frm.Show();
                    frm.Refresh();
                }
            }
        }

        public string connectionR = "http://INOLABSERVER01/Reportes_Inolab";

        //********************************************************************************Seccion para invocar el reporteador
        public void verDatos(ReportViewer reportViewer1, int id, string max, string min, string date1, string date2, string minutes, string fechareporte, string fechaservicio, string folio, string anexo, string idR, string reporte)
        {
            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");

            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/" + reporte;
            //reportViewer1.ServerReport.ReportPath = "/Servicio/TablaSensor";
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
            parameter4.Values.Add(minutes);

            ReportParameter parameter5 = new ReportParameter();
            parameter5.Name = "folio";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter5.Values.Add(folio);
            ReportParameter parameter6 = new ReportParameter();
            parameter6.Name = "ide";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter6.Values.Add(idR);
            ReportParameter parameter7 = new ReportParameter();
            parameter7.Name = "fr";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter7.Values.Add(DateFormat(ToDate(fechareporte)));
            ReportParameter parameter8 = new ReportParameter();
            parameter8.Name = "fe";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter8.Values.Add(DateFormat(ToDate(fechaservicio)));
            ReportParameter parameter9 = new ReportParameter();
            parameter9.Name = "anexo";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter9.Values.Add(anexo);

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter0 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter1 });

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter4 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter5 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter6 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter7 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter8 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter9 });
            reportViewer1.RefreshReport();
        }

        public void verDatosReales(ReportViewer reportViewer1, int id, string max, string min, string date1, string date2, string minuten, string fechareporte, string fechaservicio, string folio, string anexo, string idR, string reporte)
        {
            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");
            //MessageBox.Show("DatosReales");
            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/" + reporte;
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
            parameter5.Name = "folio";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter5.Values.Add(folio);
            ReportParameter parameter6 = new ReportParameter();
            parameter6.Name = "ide";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter6.Values.Add(idR);
            ReportParameter parameter7 = new ReportParameter();
            parameter7.Name = "fr";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter7.Values.Add(DateFormat(ToDate(fechareporte)));
            ReportParameter parameter8 = new ReportParameter();
            parameter8.Name = "fe";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter8.Values.Add(DateFormat(ToDate(fechaservicio)));
            ReportParameter parameter9 = new ReportParameter();
            parameter9.Name = "anexo";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter9.Values.Add(anexo);

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter0 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter1 });

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter4 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter5 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter6 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter7 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter8 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter9 });
            reportViewer1.RefreshReport();
        }

        public void verApertura(ReportViewer reportViewer1, int id, string reporte, int minuten, string max, string min)
        {
            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");
            //MessageBox.Show("DatosReales");
            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/" + reporte;
            reportViewer1.ShowParameterPrompts = false;
            ReportParameter parameter = new ReportParameter();
            parameter.Name = "id";
            parameter.Values.Add(id.ToString());

            ReportParameter parameter0 = new ReportParameter();
            parameter0.Name = "minuto";
            parameter0.Values.Add(minuten.ToString());

            ReportParameter parameter1 = new ReportParameter();
            parameter1.Name = "max";
            parameter1.Values.Add(max);

            ReportParameter parameter2 = new ReportParameter();
            parameter2.Name = "min";
            parameter2.Values.Add(min);

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter0 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter1 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter2 });

            reportViewer1.RefreshReport();
        }

        public void verUniformidad(ReportViewer reportViewer1, int id, string date1, string date2, string minuten, string max, string min, int NumeroDeSensores, Decimal TemperaturaDeReferencia, string reporte)
        {
            date1 = ToDate(date1).ToString("dd/MM/yyyy HH:mm:ss");
            date2 = ToDate(date2).ToString("dd/MM/yyyy HH:mm:ss");

            NetworkCredential myCred = new NetworkCredential("cflores", "carlos_42");
            //MessageBox.Show("DatosReales");
            reportViewer1.ServerReport.ReportServerCredentials.NetworkCredentials = myCred;
            reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
            reportViewer1.ServerReport.ReportServerUrl = new Uri(connectionR);
            reportViewer1.ServerReport.ReportPath = "/Servicio/" + reporte;
            reportViewer1.ShowParameterPrompts = false;
            ReportParameter parameter = new ReportParameter();
            parameter.Name = "id";
            parameter.Values.Add(id.ToString());

            ReportParameter parameter4 = new ReportParameter();
            parameter4.Name = "minuto";
            //MessageBox.Show(ft.DateFormat(ft.ToDate(date2)));
            parameter4.Values.Add(minuten);
            ReportParameter parameter0 = new ReportParameter();
            parameter0.Name = "max";
            parameter0.Values.Add(max);
            ReportParameter parameter1 = new ReportParameter();
            parameter1.Name = "min";
            //MessageBox.Show(min);
            parameter1.Values.Add(min);
            ReportParameter sens = new ReportParameter();
            sens.Name = "NumeroSensores";
            //MessageBox.Show(min);
            sens.Values.Add(NumeroDeSensores.ToString());
            ReportParameter temp = new ReportParameter();
            temp.Name = "TemperaturaDeReferencia";
            //MessageBox.Show(min);
            temp.Values.Add(TemperaturaDeReferencia.ToString());

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter0 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter1 });

            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { parameter4 });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { sens });
            reportViewer1.ServerReport.SetParameters(new ReportParameter[] { temp });
            reportViewer1.RefreshReport();
        }

        //********************************************************************************Seccion para invocar el reporteador
        public int GetSensors(int id)
        {
            string sql = "select top 1 S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,S14,S15,S16,S17,S18,S19,S20,S21,S22,S23,S23,S24,S25,S26,S27,S28,S29,S30,S31,S32,S33,S34,S35,S36,S37,S38,S39,S40"
                + " from Datos where Id='" + id + "' order by tiempo asc;";
            //string sql = "select top 1 *"

            //MessageBox.Show(sql);
            int numCols = 0; ;
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (IsValid(reader.GetValue(i).ToString()))
                    {
                        numCols++;
                    }
                }
                return numCols;
            }
            else
            {
                return 0;
            }
        }

        public void GetSensorsW(Form form, int id, int numSensores, string L, List<string> sensors)
        {
            //Funcion para crear los campos donde se escribe el error en el windowsForm Incertidumbre
            string sql = "select top 1 S1,S2,S3,S4,S5,S6,S7,S8,S9,S10,S11,S12,S13,S14,S15,S16,S17,S18,S19,S20,S21,S22,S23,S23,S24,S25,S26,S27,S28,S29,S30,S31,S32,S33,S34,S35,S36,S37,S38,S39,S40"
                + " from Datos where Id='" + id + "' order by tiempo asc;";
            int numCols = 0; ;
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                //Coordenadas iniciales
                int y = 75;
                int x = 25;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    if (IsValid(reader.GetValue(i).ToString()))
                    {
                        Label namelabel = new Label();
                        namelabel.Location = new Point(x, y);

                        namelabel.Name = "L" + (i + 1);
                        namelabel.AutoSize = true;
                        form.Controls.Add(namelabel);
                        TextBox incert = new TextBox();
                        incert.Location = new Point(x + 30, y + 15);
                        incert.Size = new System.Drawing.Size(45, 20);
                        incert.Name = L + (i + 1);
                        form.Controls.Add(incert);

                        y = y + 45;

                        numCols++;
                    }
                    //if (i == (numSensores / 2) && numSensores != null)
                    if ((i % 7) == 0)
                    {
                        x = x + 125;
                        y = 75;
                    }
                }
            }
            else
            {
            }
            int count = 0, LBLS = 0;
            //Console.WriteLine("FT.SENSORS " + Sensors.Count);
            foreach (Control ctrl in form.Controls)
            {
                if (ctrl is Label && ctrl.Name != "label1" && ctrl.Name != "label2" && ctrl.Name != "label3")
                {
                    //Console.WriteLine("SENSOR: " + sensors[count]);
                    //ctrl.Text = sensors[count];
                    LBLS++;
                }
            }

            if (sensors.Count > 0 && sensors.Count >= LBLS)
            {
                foreach (Control ctrl in form.Controls)
                {
                    if (ctrl is Label && ctrl.Name != "label1" && ctrl.Name != "label2" && ctrl.Name != "label3")
                    {
                        //Console.WriteLine("SENSOR: " + sensors[count]);
                        ctrl.Text = sensors[count];
                        count++;
                    }
                }
            }
            else
            {
                int i = 1;
                foreach (Control ctrl in form.Controls)
                {
                    if (ctrl is Label && ctrl.Name != "label1" && ctrl.Name != "label2" && ctrl.Name != "label3")
                    {
                        //Console.WriteLine("SENSOR: " + sensors[count]);
                        ctrl.Text = "S" + i;
                        i++;
                    }
                }
            }
        }

        public string GetQueryIncertidumbre(Form form, int index, int numSensores)
        {
            //Funcion que genera el Query para incertar datos en la tabla incertidumbre con las incertidumbres escritas en
            //el windowsForms incertidumbre
            string endsql = "FROM Datos WHERE Id = '" + index + "'; ";
            string sqlStart = "INSERT INTO Incertidumbre (Id,Tiempo,";
            string sqlStartP2 = "";
            string sqlMid = "SELECT Id,Tiempo,";
            string up = " ";
            int cc = 1;
            int flag = 0;

            foreach (Control ctr in form.Controls)
            {
                if (ctr is TextBox && ctr.Name != "prop" && ctr.Name != "off")
                {
                    if (/*m.Success*/MatchFloat(ctr.Text) && !DeleteWS(ctr.Text).Contains(" ")
                        && !String.IsNullOrEmpty(DeleteWS(ctr.Text))
                        && !String.IsNullOrWhiteSpace(DeleteWS(ctr.Text)))
                    {
                        Usr.Incertidumbre.Add(DeleteWS(ctr.Text));
                        //Console.WriteLine("TEXT " + ctr.Text);
                        if (numSensores == 1)
                        {
                            sqlStartP2 = sqlStartP2 + "S" + cc + ") ";
                            //Console.WriteLine(sqlStartP2);
                            //c++;
                        }
                        else
                        if (cc == numSensores)
                        {
                            sqlStartP2 = sqlStartP2 + "S" + cc + ") ";
                            //Console.WriteLine(sqlStartP2);
                            //c++;
                        }
                        else
                        {
                            sqlStartP2 = sqlStartP2 + "S" + cc + ", ";
                            //Console.WriteLine(sqlStartP2);
                            cc++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Formato Incorrecto en " + ctr.Name);
                        flag++;
                    }
                }
            }
            if (flag == 0)
            {
                int c = 1;
                foreach (Control ctr1 in form.Controls)
                {
                    if (ctr1 is TextBox && ctr1.Name != "prop" && ctr1.Name != "off")
                    {
                        if (numSensores == 1)
                        {
                            up = up + "S" + (c) + "=" + "round(round(S" + c + "*(" + Usr.proporcion + "),1)-(" + Usr.offset + "),1)-(" + DeleteWS(ctr1.Text) + ") ";
                            //c++;
                        }
                        else
                        if (c == numSensores)
                        {
                            //up = up + "S" + (c) + "= S" + (c) + " " +ft.DeleteWS(ctr1.Text) + " ";
                            if (String.IsNullOrEmpty(DeleteWS(ctr1.Text))
                                || String.IsNullOrWhiteSpace(DeleteWS(ctr1.Text)))
                            {
                                up = up + "round(round(S" + (c) + "*" + Usr.proporcion + ",1)- (" + Usr.offset + "),1) ";
                                c++;
                            }
                            else if (DeleteWS(ctr1.Text).Contains("+")
                                || DeleteWS(ctr1.Text).Contains("-"))
                            {
                                up = up + "S" + (c) + "=" + "round(round(S" + c + "*(" + Usr.proporcion + "),1)-(" + Usr.offset + "),1)-(" + DeleteWS(ctr1.Text) + ") ";
                                c++;
                            }
                            else
                            {
                                up = up + "S" + (c) + "= " + "round(round(S" + c + "*(" + Usr.proporcion + "),1)-(" + Usr.offset + "),1)-(" + DeleteWS(ctr1.Text) + ") ";
                                c++;
                            }
                            //c++;
                        }
                        else
                        {
                            if (ctr1.Text.Trim() == "0")
                            {
                                up = up + "S" + (c) + "=round(round(S" + (c) + "*" + Usr.proporcion + ",1)-(" + Usr.offset + "),1), ";
                                c++;
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(DeleteWS(ctr1.Text))
                                    || String.IsNullOrWhiteSpace(DeleteWS(ctr1.Text)))
                                {
                                    up = up + "round(round(S" + (c) + "*" + Usr.proporcion + ",1)-(" + Usr.offset + "),1) , ";
                                    c++;
                                }
                                else if (DeleteWS(ctr1.Text).Contains("+")
                                    || DeleteWS(ctr1.Text).Contains("-"))
                                {
                                    up = up + "S" + (c) + "= " + "round(round(S" + c + "*(" + Usr.proporcion + "),1)-(" + Usr.offset + "),1)-(" + DeleteWS(ctr1.Text) + "), ";
                                    c++;
                                }
                                else
                                {
                                    up = up + "S" + (c) + "= " + "round(round(S" + c + "*(" + Usr.proporcion + "),1)-(" + Usr.offset + "),1)-(" + DeleteWS(ctr1.Text) + "), ";
                                    c++;
                                }
                            }
                        }
                    }
                }
                return sqlStart + sqlStartP2 + sqlMid + up + endsql;
            }
            else
            {
                return "0";
            }
        }

        public string GetQueryIncertidumbreHumedad(Form form, int index, int numSensores)
        {
            //Funcion que genera el Query para incertar datos en la tabla incertidumbreHumedad con las incertidumbres escritas en
            //el windowsForms incertidumbreHumedad
            string endsql = "FROM Datos WHERE Id = '" + index + "'; ";
            string sqlStart = "INSERT INTO IncertidumbreHumedad (Id,Tiempo,";
            string sqlStartP2 = "";
            string sqlMid = "SELECT Id,Tiempo,";
            string up = " ";
            string ret;
            int cc = 1;
            int flag = 0;
            foreach (Control ctr in form.Controls)
            {
                if (ctr is TextBox && ctr.Name != "prop" && ctr.Name != "off")
                {
                    if (/*m.Success*/MatchFloat(ctr.Text) && !DeleteWS(ctr.Text).Contains(" ")
                        && !String.IsNullOrEmpty(DeleteWS(ctr.Text))
                        && !String.IsNullOrWhiteSpace(DeleteWS(ctr.Text)))
                    {
                        Usr.IncertidumbreH.Add(DeleteWS(ctr.Text));
                        if (numSensores == 1)
                        {
                            sqlStartP2 = sqlStartP2 + "H" + cc + ") ";
                            //c++;
                        }
                        else
                        if (cc == numSensores)
                        {
                            sqlStartP2 = sqlStartP2 + "H" + cc + ") ";
                            //c++;
                        }
                        else
                        {
                            sqlStartP2 = sqlStartP2 + "H" + cc + ", ";
                            cc++;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Formato Incorrecto en " + ctr.Name);
                        flag++;
                    }
                }
            }
            if (flag == 0)
            {
                int c = 1;
                foreach (Control ctr1 in form.Controls)
                {
                    if (ctr1 is TextBox && ctr1.Name != "prop" && ctr1.Name != "off")
                    {
                        if (numSensores == 1)
                        {
                            up = up + "H" + (c) + "=" + "round(round(H" + c + "*(" + Usr.proporcionH + "),1)-(" + Usr.offsetH + "),1)-(" + DeleteWS(ctr1.Text) + ") ";
                            //c++;
                        }
                        else
                        if (c == numSensores)
                        {
                            //up = up + "S" + (c) + "= S" + (c) + " " +ft.DeleteWS(ctr1.Text) + " ";
                            if (String.IsNullOrEmpty(DeleteWS(ctr1.Text))
                                || String.IsNullOrWhiteSpace(DeleteWS(ctr1.Text)))
                            {
                                up = up + "round(round(H" + (c) + "*" + Usr.proporcionH + ",1)- (" + Usr.offsetH + "),1 )";
                                c++;
                            }
                            else if (DeleteWS(ctr1.Text).Contains("+")
                                || DeleteWS(ctr1.Text).Contains("-"))
                            {
                                up = up + "H" + (c) + "=" + "round(round(H" + c + "*" + Usr.proporcionH + ",1)-(" + Usr.offsetH + "),1)-(" + DeleteWS(ctr1.Text) + ") ";
                                c++;
                            }
                            else
                            {
                                up = up + "H" + (c) + "= " + "round(round(H" + c + "*" + Usr.proporcionH + ",1)-(" + Usr.offsetH + "),1)-(" + DeleteWS(ctr1.Text) + ") ";
                                c++;
                            }
                            //c++;
                        }
                        else
                        {
                            if (ctr1.Text.Trim() == "0")
                            {
                                up = up + "H" + (c) + "=Round(Round(H" + (c) + "*" + Usr.proporcionH + ",1)-(" + Usr.offsetH + "),1), ";
                                c++;
                            }
                            else
                            {
                                if (String.IsNullOrEmpty(DeleteWS(ctr1.Text))
                                    || String.IsNullOrWhiteSpace(DeleteWS(ctr1.Text)))
                                {
                                    up = up + "H" + (c) + "= " + "round(round(H" + (c) + "*" + Usr.proporcionH + ",1)-(" + Usr.offsetH + ") ,1), ";
                                    c++;
                                }
                                else if (DeleteWS(ctr1.Text).Contains("+")
                                    || DeleteWS(ctr1.Text).Contains("-"))
                                {
                                    up = up + "H" + (c) + "= " + "round(round(H" + c + "*" + Usr.proporcionH + ",1)-(" + Usr.offsetH + "),1)-(" + DeleteWS(ctr1.Text) + "), ";
                                    c++;
                                }
                                else
                                {
                                    up = up + "H" + (c) + "= " + "round(round(H" + c + "*" + Usr.proporcionH + ",1)-(" + Usr.offsetH + "),1)-(" + DeleteWS(ctr1.Text) + "), ";
                                    c++;
                                }
                            }
                        }
                    }
                }
                ret = sqlStart + sqlStartP2 + sqlMid + up + endsql;
            }
            else
            {
                ret = "0";
            }

            return ret;
        }

        public bool SetSql(string sql)
        {
            //Funcion para ejecutar un query
            SqlConnection conn = new SqlConnection(connection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(sql, conn);
            int temp = Convert.ToInt32(cmd.ExecuteNonQuery().ToString());

            if (temp > 0)
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

        public bool isDate(string date)
        {
            return DateTime.TryParse(date, out DateTime datevalue);
        }

        public DataSet SetDataSet(int Id, int minuten, string procedure)
        {
            //Funcion para obtener un DataSet con datos de un procedimiento almacenado
            //Con el detalle de cuantos minutos
            DataSet ds = new DataSet("Datos");
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand sqlComm = new SqlCommand(procedure, conn);
                sqlComm.Parameters.AddWithValue("@Id", Id.ToString());
                sqlComm.Parameters.AddWithValue("@minuto", minuten.ToString());

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            return ds;
        }

        public DataSet SetDataSet(int Id, string procedure)
        {
            //Funcion para obtener un DataSet con datos de un procedimiento almacenado
            DataSet ds = new DataSet("Datos");
            using (SqlConnection conn = new SqlConnection(connection))
            {
                SqlCommand sqlComm = new SqlCommand(procedure, conn);
                sqlComm.Parameters.AddWithValue("@Id", Id.ToString());

                sqlComm.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = sqlComm;

                da.Fill(ds);
            }

            return ds;
        }

        public bool GetFolioId(string Folio)
        {
            //Funcion para verificar si el Id/Folio ya se encuentra en la tabla Datos
            string sql = "Select Id from Datos where Id='" + Folio + "'";

            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            string re;
            if (reader.Read())
            {
                re = reader.GetValue(0).ToString();
                if (re == Folio)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        public bool GetIdIncertidumbre(string Id, string Tabla)
        {
            //Funcion para verificar si el Id/Folio ya se encuentra en la tabla de datos
            string sql = "Select Id from " + Tabla + " where Id='" + Id + "'";

            SqlConnection conn = new SqlConnection(connection);
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            string re;
            if (reader.Read())
            {
                re = reader.GetValue(0).ToString();
                Console.WriteLine(re);
                if (re == Id)
                {
                    Console.WriteLine("FALSE");
                    return false;
                }
                else
                {
                    Console.WriteLine("TRUE");
                    return true;
                }
            }
            else
            {
                Console.WriteLine("TRUE");
                return true;
            }
        }

        public string connection1 = "Data Source=INOLABSERVER01;Initial Catalog=Browser;Persist Security Info=True;User ID=ventas;Password=V3ntas_17";
        public string connection = "Data Source=INOLABSERVER01;Initial Catalog=Documentacion;Persist Security Info=True;User ID=ventas;Password=V3ntas_17";

        public SqlDataReader GetLog(string usr, string pass)
        {
            //Funcion para obtener datos del usuario de la BD
            string sql =
              "select IdUsuario,IdRol,IdArea from Usuarios where Usuario='" + usr + "' and Password_='" + pass + "' and Activo='1'";
            //MessageBox.Show(sql);
            SqlConnection conn = new SqlConnection(connection1);
            conn.Open();

            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader;
            }
            return null;
        }

        public SqlDataReader getSD(string id, string tabla, string oder)
        {
            /*Funcion para obtener la desviación estandar
             * y coeficiente de variación
             */
            string sql = "declare @id as int='" + id + "';" +
            "with consulta as (" +
            "select " + oder + "1 AS num from " + tabla + " where id = @id union all " +
            "select " + oder + "2 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "3 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "4 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "5 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "6 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "7 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "8 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "9 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "10 AS num from " + tabla + " where id = @id union all " +
            "select " + oder + "11 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "12 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "13 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "14 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "15 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "16 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "17 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "18 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "19 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "20 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "21 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "22 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "23 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "24 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "25 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "26 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "27 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "28 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "29 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "30 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "31 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "32 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "33 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "34 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "35 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "36 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "37 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "38 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "39 AS num  from " + tabla + " where id = @id union all " +
            "select " + oder + "40 AS num  from " + tabla + " where id = @id)" +

            "select STDEVP(num) as 'SDP',(STDEVP(num)/AVG(num))*100 as 'CV' from consulta ";
            SqlConnection conn = new SqlConnection(connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Read();
            /*if ()
            {
                string n = reader.GetValue(reader.GetOrdinal(hash)).ToString();
                if (MatchFloat(n))
                {
                    float re = float.Parse(reader.GetValue(reader.GetOrdinal(hash)).ToString());
                    //return re.ToString("n2");
                }
                else
                {
                    //return "--";
                }
            }
            else
            {
                //return "--";
            }*/
            return reader;
        }

        public string getSDdates(string id, string tabla, string oder, string hash)
        {
            /*Funcion para obtener la desviación estandar
             * y coeficiente de variación entre dos fechas
             */
            string sql = "declare @id as int='" + id + "'; declare @f1 as datetime;set @f1=( select f1 from Parametros where id=@id); declare @f2 as datetime set @f2 = (select f2 from Parametros where id = @id);" +
            "with consulta as (" +
            "select " + oder + "1 AS num from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "2 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "3 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "4 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "5 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "6 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "7 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "8 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "9 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "10 AS num from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "11 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "12 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "13 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "14 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "15 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "16 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "17 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "18 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "19 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "20 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "21 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "22 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "23 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "24 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "25 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "26 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "27 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "28 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "29 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "30 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "31 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "32 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "33 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "34 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "35 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "36 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "37 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "38 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "39 AS num  from " + tabla + " where id = @id and Tiempo between @f1 and @f2 union all " +
            "select " + oder + "40 AS num  from " + tabla + " where id = @id)" +

            "select STDEVP(num) as 'SDP',(STDEVP(num)/AVG(num))*100 as 'CV' from consulta ";
            //Console.WriteLine(sql);
            SqlConnection conn = new SqlConnection(connection);

            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read() && !(reader.GetValue(reader.GetOrdinal(hash)) is DBNull))
            {
                float re = float.Parse(reader.GetValue(reader.GetOrdinal(hash)).ToString());
                return re.ToString("n2");
            }
            else
            {
                return "--";
            }
        }

        public bool IsValid(string txt)
        {
            if (String.IsNullOrWhiteSpace(txt) || String.IsNullOrEmpty(txt))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void CheckWrongs(Form form, NumericUpDown nud1, NumericUpDown nud2)
        {
            //Función para colorear los elementos del Form que estén vacíos
            NumCheck(nud1);
            NumCheck(nud2);

            if (ColourWrongs(form))
            {
                MessageBox.Show("Verifique el formato de datos.");
            }
        }

        public bool ColourWrongs(Form form)
        {
            //Función para colorear los elementos del Form que estén vacíos
            bool i = false;
            foreach (Control ctr in form.Controls)
            {
                if (ctr is TextBox)
                {
                    if (!IsValid(ctr.Text))
                    {
                        i = true;
                        ctr.BackColor = System.Drawing.Color.LightCoral;
                    }
                    else
                    {
                        if (ctr.BackColor != System.Drawing.Color.LightGreen)
                        {
                            ctr.BackColor = SystemColors.Window;
                        }
                    }
                }
            }
            return i;
        }

        public void CoulorWDate(TextBox txt)
        {
            //Función para colorear los elementos del Form que no sean fechas
            if (!isDate(txt.Text))
            {
                txt.BackColor = System.Drawing.Color.LightCoral;
            }
            else
            {
                txt.BackColor = System.Drawing.Color.LightGreen;
            }
        }

        public void CoulorW(TextBox txt)
        {
            //Función para colorear los elementos del Form que estén vacíos
            if (!IsValid(txt.Text))
            {
                txt.BackColor = System.Drawing.Color.LightCoral;
            }
            else
            {
                txt.BackColor = SystemColors.Window;
            }
        }

        public void NumCheck(NumericUpDown nud)
        {
            //Función para colorear los elementos del Form que no sean números
            if (nud.Value == 0)
            {
                nud.BackColor = System.Drawing.Color.LightCoral;
                MessageBox.Show("Valor no válido");
            }
            else
            {
                nud.BackColor = SystemColors.Window;
            }
        }

        public string Format(string date)
        {
            //Funcion para cambiar el formato de una fecha
            string rt;
            if (isDate(date))
            {
                return rt = "'" + Date2(ToDate(date)) + "'";
            }
            else
            {
                return rt = "null";
            }
        }

        public SqlDataReader GetRead(string query, string conection)
        {
            SqlConnection conn = new SqlConnection(conection);
            conn.Open();
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return reader;
            }
            else
            {
                return null;
            }
        }

        public bool SetSens(List<string> Sens, string folio)
        {
            //Se guardan los titulos de los sensores usados en la Tabla RegTempSens en la BD Documentacion
            SqlDataReader read = GetRead("Select count(*) as Total from RegTempSens where folio='" + folio + "';", connection);
            int total = (int)read.GetValue(read.GetOrdinal("Total"));
            if (total <= 0)
            {
                if (Sens.Count > 0)
                {
                    for (int i = 0; i < Sens.Count; i++)
                    {
                        string query = "INSERT INTO RegTempSens(Folio,NoSerie,IdUsuario,Registro)" +
                            "VALUES('" + folio + "','" + Sens[i] + "'," + Usr.K + ",'" + Date2(DateTime.Now) + "')";

                        if (IsValid(folio))
                        {
                            SetSql(query);
                        }
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        public DataTable SendToServerTempOmega(SqlBulkCopy objbulk, List<int> DatePos, List<int> NumbersPos, string[,] united2)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Id");
            dt.Columns.Add("Tiempo");

            objbulk.ColumnMappings.Clear();
            objbulk.ColumnMappings.Add("Id", "Id");
            objbulk.ColumnMappings.Add("Tiempo", "Tiempo");
            //Mapeo de columnas y adición de columnas al datatable nuevo
            for (int i = 0; i < NumbersPos.Count - 1; i++)
            {
                dt.Columns.Add("S" + (i + 1));
                objbulk.ColumnMappings.Add("S" + (i + 1), "S" + (i + 1));
            }
            dt.Columns.Add("S" + NumbersPos.Count);
            objbulk.ColumnMappings.Add("S" + NumbersPos.Count, "S" + NumbersPos.Count);

            objbulk.DestinationTableName = "datos";//tabla destino

            //Recolecci{on de datos de la matriz de vista horizontal de datos
            for (int i = 0; i < united2.GetLength(0); i++)
            {
                DataRow _ravi = dt.NewRow();
                _ravi["id"] = Usr.K;

                if (i == 0)
                {
                    _ravi["Tiempo"] = ToDate(united2[i, DatePos[0]]);
                }
                else
                {
                    if (isDate(united2[i, DatePos[0]]))
                    {
                        _ravi["Tiempo"] = ToDate(united2[i, DatePos[0]]);
                    }
                    else
                    {
                        //Si ya no se encuentra la fecha en la primer columna se busca la siguiente
                        //columna de fechas para seguir agregando añadiendo fecha a los datos
                        int np = 0;
                        for (int x = 0; x < DatePos.Count; x++)
                        {
                            if (isDate(united2[i, DatePos[x]]))
                            {
                                np = DatePos[x];
                                break;
                            }
                        }
                        if (np == 0)
                        {
                            break;
                        }
                        DateTime dd = DateTime.Parse(united2[i, np]);
                        _ravi["Tiempo"] = dd;
                    }
                }

                //REcolecci{on de valores numéricos
                for (int j = 0; j < NumbersPos.Count - 1; j++)
                {
                    if (!IsValid(united2[i, NumbersPos[j]]))
                    {
                        _ravi["S" + (j + 1)] = Convert.DBNull;
                    }
                    else
                    {
                        _ravi["S" + (j + 1)] = float.Parse(united2[i, NumbersPos[j]]);
                    }
                }
                if (IsValid(united2[i, NumbersPos[(NumbersPos.Count - 1)]]))
                {
                    _ravi["S" + (NumbersPos.Count)] = float.Parse(united2[i, NumbersPos[(NumbersPos.Count - 1)]]);
                }
                else
                {
                    _ravi["S" + (NumbersPos.Count)] = Convert.DBNull;
                }

                dt.Rows.Add(_ravi);
            }
            return dt;
        }

        public DataTable SendToServerTempElitech(SqlBulkCopy objbulk, List<int> DatePos, string[,] united2)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Id");
            dt.Columns.Add("Tiempo");

            objbulk.ColumnMappings.Clear();
            objbulk.ColumnMappings.Add("Id", "Id");
            objbulk.ColumnMappings.Add("Tiempo", "Tiempo");

            //Mapeo de columnas en el Objektbulk y adición de columnas en el datatable
            for (int i = 0; i < DatePos.Count - 1; i++)
            {
                dt.Columns.Add("S" + (i + 1));
                objbulk.ColumnMappings.Add("S" + (i + 1), "S" + (i + 1));
            }
            dt.Columns.Add("S" + DatePos.Count);
            objbulk.ColumnMappings.Add("S" + DatePos.Count, "S" + DatePos.Count);

            objbulk.DestinationTableName = "datos";//Tabla destino

            //Recolección de datos de la matriz de vista horizontal
            for (int i = 0; i < united2.GetLength(0); i++)
            {
                DataRow _ravi = dt.NewRow();
                _ravi["id"] = Usr.K;

                if (i == 0)
                {
                    _ravi["Tiempo"] = ToDate(united2[i, DatePos[0]]);
                }
                else
                {
                    if (isDate(united2[i, DatePos[0]]))
                    {
                        _ravi["Tiempo"] = ToDate(united2[i, DatePos[0]]);
                    }
                    else
                    {
                        int np = 0;
                        //Si ya no hay mas fechas en la primer columna se busca la siguiente
                        for (int x = 0; x < DatePos.Count; x++)
                        {
                            if (isDate(united2[i, DatePos[x]]))
                            {
                                np = DatePos[x];
                                break;
                            }
                        }
                        if (np == 0)
                        {
                            break;
                        }
                        DateTime dd = DateTime.Parse(united2[i, np]);

                        _ravi["Tiempo"] = dd;
                    }
                }
                //Recolección de valores numéricos
                for (int j = 0; j < DatePos.Count - 1; j++)
                {
                    if (!IsValid(united2[i, DatePos[j] + 1]))
                    {
                        _ravi["S" + (j + 1)] = Convert.DBNull;
                    }
                    else
                    {
                        _ravi["S" + (j + 1)] = float.Parse(united2[i, DatePos[j] + 1]);
                    }
                }
                if (IsValid(united2[i, DatePos[(DatePos.Count - 1)] + 1]))
                {
                    _ravi["S" + (DatePos.Count)] = float.Parse(united2[i, DatePos[(DatePos.Count - 1)] + 1]);
                }
                else
                {
                    _ravi["S" + (DatePos.Count)] = Convert.DBNull;
                }

                dt.Rows.Add(_ravi);
            }
            return dt;
        }

        public DataTable SendToServerTempElitechHum(SqlBulkCopy objbulk, List<int> DatePos, string[,] united2)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("Id");
            dt.Columns.Add("Tiempo");

            objbulk.ColumnMappings.Clear();
            objbulk.ColumnMappings.Add("Id", "Id");
            objbulk.ColumnMappings.Add("Tiempo", "Tiempo");

            //Mapeo de columnas en el objectbulk y adición de columnas al datatable
            for (int i = 0; i < DatePos.Count - 1; i++)
            {
                dt.Columns.Add("S" + (i + 1));
                objbulk.ColumnMappings.Add("S" + (i + 1), "S" + (i + 1));

                dt.Columns.Add("H" + (i + 1));
                objbulk.ColumnMappings.Add("H" + (i + 1), "H" + (i + 1));
            }
            dt.Columns.Add("S" + DatePos.Count);
            objbulk.ColumnMappings.Add("S" + DatePos.Count, "S" + DatePos.Count);

            dt.Columns.Add("H" + DatePos.Count);
            objbulk.ColumnMappings.Add("H" + DatePos.Count, "H" + DatePos.Count);

            objbulk.DestinationTableName = "datos";//Tabla destino
            //Recolección de fechas
            for (int i = 0; i < united2.GetLength(0); i++)
            {
                DataRow _ravi = dt.NewRow();
                _ravi["id"] = Usr.K;

                if (i == 0)
                {
                    _ravi["Tiempo"] = ToDate(united2[i, DatePos[0]]);
                }
                else
                {
                    if (isDate(united2[i, DatePos[0]]))
                    {
                        _ravi["Tiempo"] = ToDate(united2[i, DatePos[0]]);
                    }
                    else
                    {
                        //si la primer columna de fechas se queda sin fechas se busca la siguiente
                        int np = 0;
                        for (int x = 0; x < DatePos.Count; x++)
                        {
                            if (isDate(united2[i, DatePos[x]]))
                            {
                                np = DatePos[x];
                                break;
                            }
                        }
                        if (np == 0)
                        {
                            break;
                        }
                        DateTime dd = DateTime.Parse(united2[i, np]);

                        _ravi["Tiempo"] = dd;
                    }
                }
                //Recolección de valores numèricos
                for (int j = 0; j < DatePos.Count - 1; j++)
                {
                    if (!IsValid(united2[i, DatePos[j] + 1]))
                    {
                        _ravi["S" + (j + 1)] = Convert.DBNull;
                    }
                    else
                    {
                        _ravi["S" + (j + 1)] = float.Parse(united2[i, DatePos[j] + 1]);
                    }

                    if (!IsValid(united2[i, DatePos[j] + 2]))
                    {
                        _ravi["H" + (j + 1)] = Convert.DBNull;
                    }
                    else
                    {
                        _ravi["H" + (j + 1)] = float.Parse(united2[i, DatePos[j] + 2]);
                    }
                }
                if (IsValid(united2[i, DatePos[(DatePos.Count - 1)] + 1]))
                {
                    _ravi["S" + (DatePos.Count)] = float.Parse(united2[i, DatePos[(DatePos.Count - 1)] + 1]);
                }
                else
                {
                    _ravi["S" + (DatePos.Count)] = Convert.DBNull;
                }
                if (IsValid(united2[i, DatePos[(DatePos.Count - 1)] + 1]))
                {
                    _ravi["H" + (DatePos.Count)] = float.Parse(united2[i, DatePos[(DatePos.Count - 1)] + 2]);
                }
                else
                {
                    _ravi["H" + (DatePos.Count)] = Convert.DBNull;
                }
                dt.Rows.Add(_ravi);
            }
            return dt;
        }

        public int GetDatePos(string[,] file)
        {
            for (int i = 0; i < file.GetLength(1); i++)
            {
                if (isDate(file[10, i]))
                {
                    Usr.DatePos = i;
                    return i;
                }
            }
            return 0;
        }

        public void PaintDGV(DataGridView dgcv, float max, float min)
        {
            //Formato de datagridview dependiendo de los valores maximo y minimo
            int rows = dgcv.Rows.Count;
            int cols = dgcv.Columns.Count;
            dgcv.Columns[0].Width = 149;
            dgcv.Columns[0].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var cell = dgcv.Rows[i].Cells[j];

                    if (MatchFloat(cell.Value.ToString()))
                    {
                        dgcv.Columns[j].DefaultCellStyle.Format = "N1";
                        float value = float.Parse(dgcv.Rows[i].Cells[j].Value.ToString());

                        if (value < min)
                        {
                            cell.Style.BackColor = Color.FromArgb(49, 183, 255);
                            cell.Style.ForeColor = Color.FromArgb(0, 0, 0);
                            if (value == -1000)
                            {
                                cell.Style.BackColor = Color.FromArgb(255, 255, 0);
                                cell.Style.ForeColor = Color.FromArgb(0, 0, 0);
                            }
                        }
                        else if (value > max)
                        {
                            cell.Style.BackColor = Color.FromArgb(255, 51, 51);
                            cell.Style.ForeColor = Color.FromArgb(255, 255, 255);
                        }
                    }
                }
            }
        }

        public SqlBulkCopy blkObjTemp(int Cols, SqlConnection con)
        {
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.ColumnMappings.Clear();
            objbulk.DestinationTableName = "incertidumbre";
            objbulk.ColumnMappings.Add("Id", "Id");
            objbulk.ColumnMappings.Add("Tiempo", "Tiempo");
            for (int i = 0; i < Cols - 2; i++)
            {
                objbulk.ColumnMappings.Add("S" + (i + 1), "S" + (i + 1));
            }
            return objbulk;
        }
    }
}