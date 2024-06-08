using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Win32;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;

namespace EPG_translate
{
    /// <summary>
    /// Lógica de interacción para DualXlsToXml.xaml
    /// </summary>
    public partial class DualXlsToXml : Window
    {
        //DEFINIENDO VARIABLES POR DEFECTO
        string GeneralInfo = "cherryEpg - sample data http://demo.cherryhill.eu";

        //DEFINO VARIABLES LOCALES
        public string Id;
        public string DisplayName = "";
        public string Channel = "";
        //-------------------------------------------------
        public string Start = "";
        public string Stop = "";
        public string Lang = "en";
        public string GMT = " ";
        //public string Path = @"C:\EPG\schedule0"+ Id +"";

        //Variables para SpreadSheetLight
        public string RutaExcel = "";
        public string HojaExcel = "";
        public string Hoja2Excel = "";

        //Variables para generar el nombre del archivo
        string nombreArchivo = "";

        //variable que sirve para corroborar que el proceso de conversion estubo bien, si se mantiene en 0 todo marcha bien si es 1 hubo errores
        int corroborador = 0;
        int corroborador2 = 0;

        //variable para repetir el ciclo de creacion pero la segunda para ETTV
        public int aux = 0;

        //variable que tomará la ultima vuelta en la lectura del archivo y se usara para tomar la fecha final y escribirla en el archivo correspondiente
        public int indicadorVueltaFinal;


        DispatcherTimer timer = new DispatcherTimer();
        public int count = 0;

        public DualXlsToXml(string id, string canal, string rutaExcel, string hojaExcel, string hoja2Excel)
        {
            InitializeComponent();

            Id = id;
            DisplayName = canal;
            Channel = id;

            RutaExcel = rutaExcel;
            HojaExcel = hojaExcel;
            Hoja2Excel = hoja2Excel;

            lblresult1.Visibility = Visibility.Hidden;
            lblresult2.Visibility = Visibility.Hidden;
            imgOk.Visibility = Visibility.Hidden;
            imgErr.Visibility = Visibility.Hidden;
            btnOk.Visibility = Visibility.Hidden;
            btnDescargar.Visibility = Visibility.Hidden;

            //inicializo el timer
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(inicioConversion);
            timer.Start();
        }

        private void inicioConversion(object sender, EventArgs e)
        {
            if (count == 3)
            {
                //Codigo de conversion de archivo
                //CREACION DEL DOCUMENTO XML

                while (aux <=1)
                {
                    if (aux == 0)
                    {
                        try
                        {

                            try
                            {
                                TextReader leer1 = new StreamReader("Conf-data6.deivid");
                                string read1 = leer1.ReadLine();

                                TextReader leer2 = new StreamReader("Conf-data7.deivid");
                                string read2 = leer2.ReadLine();

                                TextReader leer3 = new StreamReader("Conf-data5.deivid");
                                string read3 = leer3.ReadLine();

                                GMT = GMT + read3;
                                leer3.Close();


                                if (Id == "1")
                                {
                                    nombreArchivo = read1;
                                    leer1.Close();
                                }
                                else if (Id == "2")
                                {
                                    nombreArchivo = read2;
                                    leer2.Close();
                                }
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show("Error intentando leer los archivos de configuración, el sistema dice..." + ex.Message, "Error de lectura de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                            var sts = new XmlWriterSettings()
                            {
                                Indent = true,
                            };

                            XmlWriter Xml = XmlWriter.Create(@"C:\EPG\" + nombreArchivo + ".xml",sts);
                            Xml.WriteStartDocument();

                            Xml.WriteStartElement("tv");
                            Xml.WriteAttributeString("generator-info-name", GeneralInfo);
                            Xml.WriteStartElement("channel");
                            Xml.WriteAttributeString("id", Id);
                            Xml.WriteStartElement("display-name");
                            Xml.WriteString(DisplayName);
                            Xml.WriteEndElement();
                            Xml.WriteEndElement();

                            //desde aqui generar ciclo
                            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


                            SLDocument sl = new SLDocument(RutaExcel, HojaExcel);
                            int row = 2;
                            DataTable dt = new DataTable();


                            while (!string.IsNullOrEmpty(sl.GetCellValueAsString(row, 1)))
                            {
                                ArchivoViewModel archivovm = new ArchivoViewModel();



                                //PROCESO PARA CREAR CADENA DE FECHA PARA CHERRY

                                //convierto el campo Date de Excel a string en date, lo formateo y lo vuelvo string nuevamente
                                try
                                {
                                    DateTime dateTimeFecha = DateTime.Parse(archivovm.fecha = (sl.GetCellValueAsDateTime(row, 1)).ToString());
                                    string fechaAux = dateTimeFecha.ToString("yyyyMMdd");

                                    Start = fechaAux;
                                    Stop = fechaAux;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Date " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }

                                //convierto el campo Time de Excel a string en date, lo formateo y lo vuelvo string nuevamente
                                try
                                {
                                    DateTime dateTimeHora = DateTime.Parse(archivovm.hora = (sl.GetCellValueAsDateTime(row, 2)).ToString());
                                    string TimeAux = dateTimeHora.ToString("HHmmss");

                                    Start = Start + TimeAux + GMT;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Time " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }

                                //convierto el campo Duracion de Excel a string en date, lo formateo y lo vuelvo string nuevamente
                                try
                                {
                                    DateTime dateTimeDuracion = DateTime.Parse(archivovm.duracion = (sl.GetCellValueAsDateTime(row, 3)).ToString());
                                    string TimeAux = dateTimeDuracion.ToString("HHmmss");

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Duración " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }

                                //Por ultimo sumo el dato de hora de inicio y duracion para obtener el dato de salida del programa
                                try
                                {
                                    DateTime dateTimeHora = DateTime.Parse(archivovm.hora = (sl.GetCellValueAsDateTime(row, 2)).ToString());
                                    DateTime dateTimeDuracion = DateTime.Parse(archivovm.duracion = (sl.GetCellValueAsDateTime(row, 3)).ToString());

                                    string horas = dateTimeDuracion.ToString("HH");
                                    string minutos = dateTimeDuracion.ToString("mm");
                                    string segundos = dateTimeDuracion.ToString("ss");


                                    //sumamos los nuevos valores
                                    DateTime d1 = dateTimeHora.AddHours(int.Parse(horas));
                                    d1 = d1.AddMinutes(int.Parse(minutos));
                                    d1 = d1.AddSeconds(int.Parse(segundos));

                                    string stop = d1.ToString("HHmmss");
                                    Stop = Stop + stop + GMT;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Time " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }


                                Xml.WriteStartElement("programme");
                                Xml.WriteAttributeString("channel", Channel);
                                Xml.WriteAttributeString("start", Start);
                                Xml.WriteAttributeString("stop", Stop);

                                Xml.WriteStartElement("title");
                                Xml.WriteAttributeString("lang", Lang);
                                Xml.WriteString(archivovm.titulo = sl.GetCellValueAsString(row, 4));
                                Xml.WriteEndElement();

                                Xml.WriteStartElement("sub-title");
                                Xml.WriteAttributeString("lang", Lang);
                                Xml.WriteString(archivovm.shor = sl.GetCellValueAsString(row, 5));
                                Xml.WriteEndElement();

                                Xml.WriteStartElement("desc");
                                Xml.WriteAttributeString("lang", Lang);
                                Xml.WriteString(archivovm.synopsis = sl.GetCellValueAsString(row, 6));
                                Xml.WriteEndElement();
                                Xml.WriteEndElement();

                                row++;
                                indicadorVueltaFinal = row;
                            }

                            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                            Xml.WriteEndElement();
                            Xml.WriteEndDocument();
                            Xml.Close();

                            //ESCRIBO LAS FECHAS EN EL ARCHIVO CORRESPONDIENTE

                            try
                            {
                                if (Id == "1")
                                {
                                    //Escribiendo en archivo Fecha de inicio City
                                    ArchivoViewModel referencia = new ArchivoViewModel();
                                    DateTime dateTimeFechaInicio = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(2, 1)).ToString());
                                    string fechaInicio = dateTimeFechaInicio.ToString("MMMM dd yyyy");

                                    TextWriter archivo1 = new StreamWriter("Conf-InitCity.deivid");
                                    archivo1.WriteLine(fechaInicio);
                                    archivo1.Close();

                                    //Escribiendo en archivo Fecha final City
                                    DateTime dateTimeFechaFinal = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(indicadorVueltaFinal - 1, 1)).ToString());
                                    string fechaFinal = dateTimeFechaFinal.ToString("MMMM dd yyyy");

                                    TextWriter archivo2 = new StreamWriter("Conf-FinalCity.deivid");
                                    archivo2.WriteLine(fechaFinal);
                                    archivo2.Close();
                                }

                                else if (Id == "2")
                                {
                                    //Escribiendo en archivo Fecha de inicio ET
                                    ArchivoViewModel referencia = new ArchivoViewModel();
                                    DateTime dateTimeFechaInicio = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(2, 1)).ToString());
                                    string fechaInicio = dateTimeFechaInicio.ToString("MMMM dd yyyy");

                                    TextWriter archivo1 = new StreamWriter("Conf-InitET.deivid");
                                    archivo1.WriteLine(fechaInicio);
                                    archivo1.Close();

                                    //Escribiendo en archivo Fecha final ET
                                    DateTime dateTimeFechaFinal = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(indicadorVueltaFinal - 1, 1)).ToString());
                                    string fechaFinal = dateTimeFechaFinal.ToString("MMMM dd yyyy");

                                    TextWriter archivo2 = new StreamWriter("Conf-FinalET.deivid");
                                    archivo2.WriteLine(fechaFinal);
                                    archivo2.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error intentando escribir información en los archivos de configuración, el sistema dice ...\n" + ex.Message, "ERROR DE ESCRITURA", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }

                        catch (Exception k)
                        {
                            MessageBox.Show("Error intentando crear el archivo, compruebe que el folder /EPG/ este creado en el disco local C: y que tenga los permisos necesarios \nEl systema dice:" + "\n" + k.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            corroborador = 1;
                        }

                    }

                    else if (aux == 1)
                    {
                        Id = "2";
                        DisplayName = "El Tiempo Televisión";
                        Channel = "2";

                        try
                        {


                            try
                            {
                                TextReader leer1 = new StreamReader("Conf-data6.deivid");
                                string read1 = leer1.ReadLine();

                                TextReader leer2 = new StreamReader("Conf-data7.deivid");
                                string read2 = leer2.ReadLine();

                                TextReader leer3 = new StreamReader("Conf-data5.deivid");
                                string read3 = leer3.ReadLine();

                                GMT = " ";
                                GMT = GMT + read3;
                                leer3.Close();


                                if (Id == "1")
                                {
                                    nombreArchivo = read1;
                                    leer1.Close();
                                }
                                else if (Id == "2")
                                {
                                    nombreArchivo = read2;
                                    leer2.Close();
                                }
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show("Error intentando leer los archivos de configuración, el sistema dice..." + ex.Message, "Error de lectura de datos", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                            var sts = new XmlWriterSettings()
                            {
                                Indent = true,
                            };

                            XmlWriter Xml = XmlWriter.Create(@"C:\EPG\" + nombreArchivo + ".xml",sts);
                            Xml.WriteStartDocument();

                            Xml.WriteStartElement("tv");
                            Xml.WriteAttributeString("generator-info-name", GeneralInfo);
                            Xml.WriteStartElement("channel");
                            Xml.WriteAttributeString("id", Id);
                            Xml.WriteStartElement("display-name");
                            Xml.WriteString(DisplayName);
                            Xml.WriteEndElement();
                            Xml.WriteEndElement();

                            //desde aqui generar ciclo
                            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


                            SLDocument sl = new SLDocument(RutaExcel, Hoja2Excel);
                            int row = 2;
                            DataTable dt = new DataTable();


                            while (!string.IsNullOrEmpty(sl.GetCellValueAsString(row, 1)))
                            {
                                ArchivoViewModel archivovm = new ArchivoViewModel();



                                //PROCESO PARA CREAR CADENA DE FECHA PARA CHERRY

                                //convierto el campo Date de Excel a string en date, lo formateo y lo vuelvo string nuevamente
                                try
                                {
                                    DateTime dateTimeFecha = DateTime.Parse(archivovm.fecha = (sl.GetCellValueAsDateTime(row, 1)).ToString());
                                    string fechaAux = dateTimeFecha.ToString("yyyyMMdd");

                                    Start = fechaAux;
                                    Stop = fechaAux;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Date " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }

                                //convierto el campo Time de Excel a string en date, lo formateo y lo vuelvo string nuevamente
                                try
                                {
                                    DateTime dateTimeHora = DateTime.Parse(archivovm.hora = (sl.GetCellValueAsDateTime(row, 2)).ToString());
                                    string TimeAux = dateTimeHora.ToString("HHmmss");

                                    Start = Start + TimeAux + GMT;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Time " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }

                                //convierto el campo Duracion de Excel a string en date, lo formateo y lo vuelvo string nuevamente
                                try
                                {
                                    DateTime dateTimeDuracion = DateTime.Parse(archivovm.duracion = (sl.GetCellValueAsDateTime(row, 3)).ToString());
                                    string TimeAux = dateTimeDuracion.ToString("HHmmss");

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Duración " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }

                                //Por ultimo sumo el dato de hora de inicio y duracion para obtener el dato de salida del programa
                                try
                                {
                                    DateTime dateTimeHora = DateTime.Parse(archivovm.hora = (sl.GetCellValueAsDateTime(row, 2)).ToString());
                                    DateTime dateTimeDuracion = DateTime.Parse(archivovm.duracion = (sl.GetCellValueAsDateTime(row, 3)).ToString());

                                    string horas = dateTimeDuracion.ToString("HH");
                                    string minutos = dateTimeDuracion.ToString("mm");
                                    string segundos = dateTimeDuracion.ToString("ss");


                                    //sumamos los nuevos valores
                                    DateTime d1 = dateTimeHora.AddHours(int.Parse(horas));
                                    d1 = d1.AddMinutes(int.Parse(minutos));
                                    d1 = d1.AddSeconds(int.Parse(segundos));

                                    string stop = d1.ToString("HHmmss");
                                    Stop = Stop + stop + GMT;

                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error intentando convertir la columna Time " + ex.Message, "Error de formato", MessageBoxButton.OK, MessageBoxImage.Error);
                                    corroborador = 1;
                                }


                                Xml.WriteStartElement("programme");
                                Xml.WriteAttributeString("channel", Channel);
                                Xml.WriteAttributeString("start", Start);
                                Xml.WriteAttributeString("stop", Stop);

                                Xml.WriteStartElement("title");
                                Xml.WriteAttributeString("lang", Lang);
                                Xml.WriteString(archivovm.titulo = sl.GetCellValueAsString(row, 4));
                                Xml.WriteEndElement();

                                Xml.WriteStartElement("sub-title");
                                Xml.WriteAttributeString("lang", Lang);
                                Xml.WriteString(archivovm.shor = sl.GetCellValueAsString(row, 5));
                                Xml.WriteEndElement();

                                Xml.WriteStartElement("desc");
                                Xml.WriteAttributeString("lang", Lang);
                                Xml.WriteString(archivovm.synopsis = sl.GetCellValueAsString(row, 6));
                                Xml.WriteEndElement();
                                Xml.WriteEndElement();

                                row++;
                                indicadorVueltaFinal = row;
                            }

                            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                            Xml.WriteEndElement();
                            Xml.WriteEndDocument();
                            Xml.Close();

                            //ESCRIBO LAS FECHAS EN EL ARCHIVO CORRESPONDIENTE

                            try
                            {
                                if (Id == "1")
                                {
                                    //Escribiendo en archivo Fecha de inicio City
                                    ArchivoViewModel referencia = new ArchivoViewModel();
                                    DateTime dateTimeFechaInicio = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(2, 1)).ToString());
                                    string fechaInicio = dateTimeFechaInicio.ToString("MMMM dd yyyy");

                                    TextWriter archivo1 = new StreamWriter("Conf-InitCity.deivid");
                                    archivo1.WriteLine(fechaInicio);
                                    archivo1.Close();

                                    //Escribiendo en archivo Fecha final City
                                    DateTime dateTimeFechaFinal = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(indicadorVueltaFinal - 1, 1)).ToString());
                                    string fechaFinal = dateTimeFechaFinal.ToString("MMMM dd yyyy");

                                    TextWriter archivo2 = new StreamWriter("Conf-FinalCity.deivid");
                                    archivo2.WriteLine(fechaFinal);
                                    archivo2.Close();
                                }

                                else if (Id == "2")
                                {
                                    //Escribiendo en archivo Fecha de inicio ET
                                    ArchivoViewModel referencia = new ArchivoViewModel();
                                    DateTime dateTimeFechaInicio = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(2, 1)).ToString());
                                    string fechaInicio = dateTimeFechaInicio.ToString("MMMM dd yyyy");

                                    TextWriter archivo1 = new StreamWriter("Conf-InitET.deivid");
                                    archivo1.WriteLine(fechaInicio);
                                    archivo1.Close();

                                    //Escribiendo en archivo Fecha final ET
                                    DateTime dateTimeFechaFinal = DateTime.Parse(referencia.fecha = (sl.GetCellValueAsDateTime(indicadorVueltaFinal - 1, 1)).ToString());
                                    string fechaFinal = dateTimeFechaFinal.ToString("MMMM dd yyyy");

                                    TextWriter archivo2 = new StreamWriter("Conf-FinalET.deivid");
                                    archivo2.WriteLine(fechaFinal);
                                    archivo2.Close();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Error intentando escribir información en los archivos de configuración, el sistema dice ...\n" + ex.Message, "ERROR DE ESCRITURA", MessageBoxButton.OK, MessageBoxImage.Error);
                            }

                        }

                        catch (Exception k)
                        {
                            MessageBox.Show("Error intentando crear el archivo, compruebe que el folder /EPG/ este creado en el disco local C: y que tenga los permisos necesarios \nEl systema dice:" + "\n" + k.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                            corroborador = 1;
                        }
                    }
                    aux++;
                }

               
                count++;
                
                
            }
            else if (count == 4)
            {
                //codigo para mostrar el label y el boton segun el resultado de la conversion

                if (corroborador == 0 && corroborador2 == 0)
                {
                    //como no hubo errores muestro el mesaje de exito y muestro el voton de continuar
                    lblresult1.Visibility = Visibility.Visible;
                    lblresult1.Content = "Conversión completada";
                    lblresult1.Visibility = Visibility.Visible;
                    imgOk.Visibility = Visibility.Visible;
                    progress1.Visibility = Visibility.Hidden;

                    lblresult2.Visibility = Visibility.Visible;
                    lblresult2.Content = "Conversión completada";
                    lblresult2.Visibility = Visibility.Visible;
                    imgOk2.Visibility = Visibility.Visible;
                    progress11.Visibility = Visibility.Hidden;

                    btnDescargar.Visibility = Visibility.Visible;
                    btnOk.Visibility = Visibility.Visible;
                    lblEncabezado.Content = "Archivo Convertido";
                }
                else
                {
                    if (corroborador != 0)
                    {
                        //Como hubo errores al convertir el archivo muestro el mesaje en la pantalla
                        lblresult1.Visibility = Visibility.Visible;
                        imgErr.Visibility = Visibility.Visible;
                        lblresult1.Content = "Hubo errores en la conversion del archivo\nImposible continuar con el proceso";
                        progress1.Visibility = Visibility.Hidden;

                    }

                    else if (corroborador2 !=0)
                    {
                        //Como hubo errores al convertir el archivo muestro el mesaje en la pantalla
                        lblresult2.Visibility = Visibility.Visible;
                        imgErr2.Visibility = Visibility.Visible;
                        lblresult2.Content = "Hubo errores en la conversion del archivo\nImposible continuar con el proceso";
                        progress11.Visibility= Visibility.Hidden;
                    }

                }


                //Apago timer y reincio variables
                timer.Stop();
                count = 0;
            }

            else
            {

                count++;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Principal principal = new Principal();
            this.Hide();
            principal.Show();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            IngestaD ingestaD = new IngestaD(3);
            this.Hide();
            ingestaD.Show();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {

            Configuracion configuracion = new Configuracion();
            configuracion.Show();
        }

        private void btnDescargar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string directorio = @"C:\EPG";
                //Codigo para cargar el archivo excel
                OpenFileDialog openfiledialog = new OpenFileDialog();
                openfiledialog.Filter = "Archivos Xml cherryEPG (*.xml)|*.xml";
                openfiledialog.Title = "seleccione el archivo de Excel";
                openfiledialog.InitialDirectory = directorio;


                if (openfiledialog.ShowDialog() == true)
                {

                }

            }
            catch (Exception k)
            {

                MessageBox.Show("Imposible abrir el folder que aloja los archivos convertidos.\nCorrobore que en su disco C: esté creada la carpeta EPG" + "El systema dice:\n" + k.Message, "Error de folder", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void verArch_Click(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
