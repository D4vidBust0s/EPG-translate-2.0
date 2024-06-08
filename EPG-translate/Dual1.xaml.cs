using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace EPG_translate
{
    /// <summary>
    /// Lógica de interacción para Dual1.xaml
    /// </summary>
    public partial class Dual1 : Window
    {
        //inicializo el timer
        DispatcherTimer timer = new DispatcherTimer();
        public int count = 1;
        public int count1 = 1;
        //Configuracion de la libreria SpreadSheetLight
        string rutaExcel = @"";
        int Row = 2;

        //Validacion si la fecha fueValida o no celda por celda 
        public int validaFechaCelda = 0;
        public int validaHoraCelda = 0;
        public int validaDuracionCelda = 0;

        public int validaFechaCelda1 = 0;
        public int validaHoraCelda1 = 0;
        public int validaDuracionCelda1 = 0;


        //Variables para validar si todas las celdas son iguales en numero City
        public int aux1 = 0;
        public int aux2 = 0;
        public int aux3 = 0;
        public int aux4 = 0;
        public int aux5 = 0;
        public int aux6 = 0;
        public int aux7 = 0;

        //Variables para validar si todas las celdas son iguales en numero ET
        public int aux11 = 0;
        public int aux22 = 0;
        public int aux33 = 0;
        public int aux44 = 0;
        public int aux55 = 0;
        public int aux66 = 0;
        public int aux77 = 0;

        public int corroborador = 0;
        public int corroborador11 = 0;

        public string hoja1;
        public string hoja2;
        public int cantidadDeServicios = 2;
        //public int id = 1;
        //public int extArchivo = 0;

        List<TablaViewModel> listErrorCol = new List<TablaViewModel>();
        List<TablaViewModel> listErrorRow = new List<TablaViewModel>();

        List<TablaViewModel> listErrorCol11 = new List<TablaViewModel>();
        List<TablaViewModel> listErrorRow11 = new List<TablaViewModel>();

        List<HojaViewModel> hojaViewModels1 = new List<HojaViewModel>();
        List<HojaViewModel> hojaViewModels2 = new List<HojaViewModel>();

        public Dual1(string RutaExcel,string Hoja1, string Hoja2)
        {
            InitializeComponent();
            timer.Tick += new EventHandler(validacion);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            cantidadDeServicios = 2;
            rutaExcel = RutaExcel;
            hoja1 = "Hoja"+Hoja1;
            hoja2 = "Hoja"+Hoja2;


            //Ocultamos todos los controles que se usan visualmente para mostrar el proceso de analisis del archivo Excel

            //Controles city
            progress1.Visibility = Visibility.Hidden;
            progress2.Visibility = Visibility.Hidden;
            progress3.Visibility = Visibility.Hidden;
            progress4.Visibility = Visibility.Hidden;
            progress5.Visibility = Visibility.Hidden;
            progress6.Visibility = Visibility.Hidden;
            progress7.Visibility = Visibility.Hidden;

            lblProgres1.Visibility = Visibility.Hidden;
            lblProgres2.Visibility = Visibility.Hidden;
            lblProgres3.Visibility = Visibility.Hidden;
            lblProgres4.Visibility = Visibility.Hidden;
            lblProgres5.Visibility = Visibility.Hidden;
            lblProgres6.Visibility = Visibility.Hidden;
            lblProgres7.Visibility = Visibility.Hidden;

            lblOk1.Visibility = Visibility.Hidden;
            lblOk2.Visibility = Visibility.Hidden;
            lblOk3.Visibility = Visibility.Hidden;
            lblOk4.Visibility = Visibility.Hidden;
            lblOk5.Visibility = Visibility.Hidden;
            lblOk6.Visibility = Visibility.Hidden;
            lblOk7.Visibility = Visibility.Hidden;

            lblNumber1.Visibility = Visibility.Hidden;
            lblNumber2.Visibility = Visibility.Hidden;
            lblNumber3.Visibility = Visibility.Hidden;
            lblNumber4.Visibility = Visibility.Hidden;
            lblNumber5.Visibility = Visibility.Hidden;
            lblNumber6.Visibility = Visibility.Hidden;
            lblNumber7.Visibility = Visibility.Hidden;

            Seguir.Visibility = Visibility.Hidden;

            //-----------------------------------------------------------------------------------------------------------

            //Controles ETTV
            progress11.Visibility = Visibility.Hidden;
            progress22.Visibility = Visibility.Hidden;
            progress33.Visibility = Visibility.Hidden;
            progress44.Visibility = Visibility.Hidden;
            progress55.Visibility = Visibility.Hidden;
            progress66.Visibility = Visibility.Hidden;
            progress77.Visibility = Visibility.Hidden;

            lblProgres11.Visibility = Visibility.Hidden;
            lblProgres22.Visibility = Visibility.Hidden;
            lblProgres33.Visibility = Visibility.Hidden;
            lblProgres44.Visibility = Visibility.Hidden;
            lblProgres55.Visibility = Visibility.Hidden;
            lblProgres66.Visibility = Visibility.Hidden;
            lblProgres77.Visibility = Visibility.Hidden;

            lblOk11.Visibility = Visibility.Hidden;
            lblOk22.Visibility = Visibility.Hidden;
            lblOk33.Visibility = Visibility.Hidden;
            lblOk44.Visibility = Visibility.Hidden;
            lblOk55.Visibility = Visibility.Hidden;
            lblOk66.Visibility = Visibility.Hidden;
            lblOk77.Visibility = Visibility.Hidden;

            lblNumber11.Visibility = Visibility.Hidden;
            lblNumber22.Visibility = Visibility.Hidden;
            lblNumber33.Visibility = Visibility.Hidden;
            lblNumber44.Visibility = Visibility.Hidden;
            lblNumber55.Visibility = Visibility.Hidden;
            lblNumber66.Visibility = Visibility.Hidden;
            lblNumber77.Visibility = Visibility.Hidden;

            Seguir.Visibility = Visibility.Hidden;
            Seguir2.Visibility = Visibility.Hidden;

            btnOk.Visibility = Visibility.Hidden;

            //-----------------------------------------------------------------------------------------------------------
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Configuracion configuracion = new Configuracion();
            configuracion.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Principal principal = new Principal();
            this.Hide();
            principal.Show();
        }

        private void Cargar_archivo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Ccombo_servicio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void archivo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Seguir_Click(object sender, RoutedEventArgs e)
        {
         //CODIGO PARA ANALISIS DEL LOS ARCHIVOS  

         //Defino el timer


        }

        private void validacion(object sender, EventArgs e)
        {
            //PROCESO DE VALIDACION Y RECODIFICACION DEL ARCHIVO

            #region Extension Xlsx - Xlsm
            if (count == 1)
            {
                //city
                progress1.Visibility = Visibility.Visible;
                lblProgres1.Visibility = Visibility.Visible;


                //codigo de validacion del archivo (FECHA) - City
                try
                {
                    SLDocument SLDocument1 = new SLDocument(rutaExcel, hoja1);

                    while (!string.IsNullOrEmpty(SLDocument1.GetCellValueAsString(Row, 1)))
                    {


                        lblNumber1.Content = Row.ToString();


                        try
                        {
                            //Valido si el contenido de la celda es compatible con un formato fecha

                            string cadena = SLDocument1.GetCellValueAsDateTime(Row, 1).ToString("dd MM yyyy");

                            cadena = Regex.Replace(cadena, " ", "-");
                            DateTime dateTimeFecha = DateTime.Parse(cadena);


                            if (dateTimeFecha == DateTime.Parse("01-01-1900"))
                            {
                                validaFechaCelda = 1;
                                hojaViewModels1.Add(new HojaViewModel() { Date = Row.ToString() });

                                EncabezadoCity.Foreground = Brushes.Red;
                                EncabezadoCity.Content = "ERRORES DETECTADOS";

                            }

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                + ex.Message, "Error de formato de fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        Row++;
                        aux1 = Row;

                    }

                    count++;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al leer el archivo para el servicio Citytv, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }



                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //ETTV
                Row= 2;

                progress11.Visibility = Visibility.Visible;
                lblProgres11.Visibility = Visibility.Visible;


                //codigo de validacion del archivo (FECHA) - City
                try
                {
                    SLDocument SLDocument11 = new SLDocument(rutaExcel, hoja2);

                    while (!string.IsNullOrEmpty(SLDocument11.GetCellValueAsString(Row, 1)))
                    {


                        lblNumber11.Content = Row.ToString();

                        try
                        {
                            //Valido si el contenido de la celda es compatible con un formato fecha

                            string cadena = SLDocument11.GetCellValueAsDateTime(Row, 1).ToString("dd MM yyyy");

                            cadena = Regex.Replace(cadena, " ", "-");
                            DateTime dateTimeFecha = DateTime.Parse(cadena);


                            if (dateTimeFecha == DateTime.Parse("01-01-1900"))
                            {
                                validaFechaCelda1 = 1;
                                hojaViewModels2.Add(new HojaViewModel() { Date = Row.ToString() });

                                EncabezadoET.Foreground = Brushes.Red;
                                EncabezadoET.Content = "ERRORES DETECTADOS";

                            }

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                + ex.Message, "Error de formato de fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        Row++;
                        aux11 = Row;

                    }

                    count1++;
                }
                catch (Exception)
                {
                    MessageBox.Show("Error al leer el archivo para el servicio ETTV, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }



            }
            else if (count == 2)
            {
                //Citytv
                try
                {
                    progress2.Visibility = Visibility.Visible;
                    progress1.IsIndeterminate = false;
                    lblProgres2.Visibility = Visibility.Visible;
                    lblOk1.Visibility = Visibility.Visible;
                    lblNumber1.Visibility = Visibility.Visible;
                    Row = 2;

                   


                    //codigo de validacion del archivo (HORA)

                    SLDocument SLDocument2 = new SLDocument(rutaExcel, hoja1);

                    while (!string.IsNullOrEmpty(SLDocument2.GetCellValueAsString(Row, 2)))
                    {

                        lblNumber2.Content = Row.ToString();

                        try
                        {
                            //Valido si el contenido de la celda es compatible con un formato hora

                            string cadena = SLDocument2.GetCellValueAsString(Row, 2);

                            var stringNumber = cadena;
                            double numericValue;
                            bool isNumber = double.TryParse(stringNumber, out numericValue);

                            if (isNumber == false)
                            {
                                validaHoraCelda = 1;
                                hojaViewModels1.Add(new HojaViewModel() { Time = Row.ToString() });

                                EncabezadoCity.Foreground = Brushes.Red;
                                EncabezadoCity.Content = "ERRORES DETECTADOS";

                            }

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                + ex.Message, "Error de formato de fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        Row++;
                        aux2 = Row;

                    }

                    count++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio Citytv, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //ETTV
                try
                {
                    progress22.Visibility = Visibility.Visible;
                    progress11.IsIndeterminate = false;
                    lblProgres22.Visibility = Visibility.Visible;
                    lblOk11.Visibility = Visibility.Visible;
                    lblNumber11.Visibility = Visibility.Visible;
                    Row = 2;




                    //codigo de validacion del archivo (HORA)

                    SLDocument SLDocument22 = new SLDocument(rutaExcel, hoja2);

                    while (!string.IsNullOrEmpty(SLDocument22.GetCellValueAsString(Row, 2)))
                    {

                        lblNumber22.Content = Row.ToString();

                        try
                        {
                            //Valido si el contenido de la celda es compatible con un formato hora

                            string cadena = SLDocument22.GetCellValueAsString(Row, 2);

                            var stringNumber = cadena;
                            double numericValue;
                            bool isNumber = double.TryParse(stringNumber, out numericValue);

                            if (isNumber == false)
                            {
                                validaHoraCelda1 = 1;
                                hojaViewModels2.Add(new HojaViewModel() { Time = Row.ToString() });

                                EncabezadoET.Foreground = Brushes.Red;
                                EncabezadoET.Content = "ERRORES DETECTADOS";

                            }

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                + ex.Message, "Error de formato de fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        Row++;
                        aux22 = Row;

                    }

                    count1++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio ETTV, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (count == 3)
            {
                //Citytv
                try
                {
                    progress3.Visibility = Visibility.Visible;
                    progress2.IsIndeterminate = false;
                    lblProgres3.Visibility = Visibility.Visible;
                    lblOk2.Visibility = Visibility.Visible;
                    lblNumber2.Visibility = Visibility.Visible;
                    Row = 2;

                   


                    //codigo de validacion del archivo (DURACION)

                    SLDocument SLDocument3 = new SLDocument(rutaExcel, hoja1);

                    while (!string.IsNullOrEmpty(SLDocument3.GetCellValueAsString(Row, 3)))
                    {

                        lblNumber3.Content = Row.ToString();

                        try
                        {
                            //Valido si el contenido de la celda es compatible con un formato fecha

                            string cadena = SLDocument3.GetCellValueAsDateTime(Row, 3).ToString("HH mm ss");
                            string cadena2 = SLDocument3.GetCellValueAsString(Row, 3);

                            var stringNumber = cadena2;
                            double numericValue;
                            bool isNumber = double.TryParse(stringNumber, out numericValue);

                            cadena = Regex.Replace(cadena, " ", ":");
                            DateTime dateTimeFecha = DateTime.Parse(cadena);
                            string Horareal = dateTimeFecha.ToString("HH mm ss");


                            if (Horareal == "00 00 00" || isNumber == false)
                            {
                                validaDuracionCelda = 1;
                                hojaViewModels1.Add(new HojaViewModel() { Duration = Row.ToString() });

                                EncabezadoCity.Foreground = Brushes.Red;
                                EncabezadoCity.Content = "ERRORES DETECTADOS";

                            }

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                + ex.Message, "Error de formato de fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        Row++;
                        aux3 = Row;

                    }

                    count++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio Citytv, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //ETTV
                try
                {
                    progress33.Visibility = Visibility.Visible;
                    progress22.IsIndeterminate = false;
                    lblProgres33.Visibility = Visibility.Visible;
                    lblOk22.Visibility = Visibility.Visible;
                    lblNumber22.Visibility = Visibility.Visible;
                    Row = 2;




                    //codigo de validacion del archivo (DURACION)

                    SLDocument SLDocument33 = new SLDocument(rutaExcel, hoja2);

                    while (!string.IsNullOrEmpty(SLDocument33.GetCellValueAsString(Row, 3)))
                    {

                        lblNumber33.Content = Row.ToString();

                        try
                        {
                            //Valido si el contenido de la celda es compatible con un formato fecha

                            string cadena = SLDocument33.GetCellValueAsDateTime(Row, 3).ToString("HH mm ss");
                            string cadena2 = SLDocument33.GetCellValueAsString(Row, 3);

                            var stringNumber = cadena2;
                            double numericValue;
                            bool isNumber = double.TryParse(stringNumber, out numericValue);

                            cadena = Regex.Replace(cadena, " ", ":");
                            DateTime dateTimeFecha = DateTime.Parse(cadena);
                            string Horareal = dateTimeFecha.ToString("HH mm ss");


                            if (Horareal == "00 00 00" || isNumber == false)
                            {
                                validaDuracionCelda1 = 1;
                                hojaViewModels2.Add(new HojaViewModel() { Duration = Row.ToString() });

                                EncabezadoET.Foreground = Brushes.Red;
                                EncabezadoET.Content = "ERRORES DETECTADOS";

                            }

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                + ex.Message, "Error de formato de fecha", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        Row++;
                        aux33 = Row;

                    }

                    count1++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio ETTV, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (count == 4)
            {
                //Citytv
                try
                {
                    progress4.Visibility = Visibility.Visible;
                    progress3.IsIndeterminate = false;
                    lblProgres4.Visibility = Visibility.Visible;
                    lblOk3.Visibility = Visibility.Visible;
                    lblNumber3.Visibility = Visibility.Visible;
                    Row = 2;


                    //codigo de validacion del archivo (TITULO)

                    SLDocument SLDocument4 = new SLDocument(rutaExcel, hoja1);

                    while (!string.IsNullOrEmpty(SLDocument4.GetCellValueAsString(Row, 4)))
                    {

                        lblNumber4.Content = Row.ToString();
                        Row++;
                        aux4 = Row;

                    }

                    count++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio Citytv, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //ETTV
                try
                {
                    progress44.Visibility = Visibility.Visible;
                    progress33.IsIndeterminate = false;
                    lblProgres44.Visibility = Visibility.Visible;
                    lblOk33.Visibility = Visibility.Visible;
                    lblNumber33.Visibility = Visibility.Visible;
                    Row = 2;


                    //codigo de validacion del archivo (TITULO)

                    SLDocument SLDocument44 = new SLDocument(rutaExcel, hoja2);

                    while (!string.IsNullOrEmpty(SLDocument44.GetCellValueAsString(Row, 4)))
                    {

                        lblNumber44.Content = Row.ToString();
                        Row++;
                        aux44 = Row;

                    }

                    count1++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio ETTV, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (count == 5)
            {
                //Citytv
                try
                {
                    progress5.Visibility = Visibility.Visible;
                    progress4.IsIndeterminate = false;
                    lblProgres5.Visibility = Visibility.Visible;
                    lblOk4.Visibility = Visibility.Visible;
                    lblNumber4.Visibility = Visibility.Visible;
                    Row = 2;

                   


                    //codigo de validacion del archivo (SHORT)

                    SLDocument SLDocument5 = new SLDocument(rutaExcel, hoja1);

                    while (!string.IsNullOrEmpty(SLDocument5.GetCellValueAsString(Row, 5)))
                    {

                        lblNumber5.Content = Row.ToString();
                        Row++;
                        aux5 = Row;

                    }

                    count++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio Citytv, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //Citytv
                try
                {
                    progress55.Visibility = Visibility.Visible;
                    progress44.IsIndeterminate = false;
                    lblProgres55.Visibility = Visibility.Visible;
                    lblOk44.Visibility = Visibility.Visible;
                    lblNumber44.Visibility = Visibility.Visible;
                    Row = 2;




                    //codigo de validacion del archivo (SHORT)

                    SLDocument SLDocument55 = new SLDocument(rutaExcel, hoja2);

                    while (!string.IsNullOrEmpty(SLDocument55.GetCellValueAsString(Row, 5)))
                    {

                        lblNumber55.Content = Row.ToString();
                        Row++;
                        aux55 = Row;

                    }

                    count1++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio ETTV, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (count == 6)
            {
                //Citytv
                try
                {
                    progress6.Visibility = Visibility.Visible;
                    progress5.IsIndeterminate = false;
                    lblProgres6.Visibility = Visibility.Visible;
                    lblOk5.Visibility = Visibility.Visible;
                    lblNumber5.Visibility = Visibility.Visible;
                    lblOk6.Content = "OK";
                    lblOk6.Foreground = Brushes.Green;
                    Row = 2;

                   


                    //codigo de validacion del archivo (SHORT)
                    SLDocument SLDocument6 = new SLDocument(rutaExcel, hoja1);


                    //hago un ciclo adicional con # cantidad de vueltas para que valide columnas nulas despues del contenido, garantizando que no hay nada mas despues
                    //de las colimnas requeridas por cherry

                    var columnasAdicionales = 7;
                    var vueltas = 0;


                    while (columnasAdicionales <= 47)
                    {
                        while (string.IsNullOrEmpty(SLDocument6.GetCellValueAsString(Row, columnasAdicionales)) && vueltas <= 400)
                        {

                            //lblNumber6.Content = Row.ToString();
                            Row++;
                            vueltas++;


                        }


                        if (vueltas < 400)
                        {
                            corroborador++;
                            listErrorCol.Add(new TablaViewModel() { Columna = columnasAdicionales.ToString(), Fila = Row.ToString() });
                            //listErrorRow.Add(Row.ToString());

                            //MessageBox.Show("hay contenido adicional que debe ser borrado en Columna..." + columnasAdicionales.ToString() +  " fila..." + Row.ToString());
                        }

                        columnasAdicionales++;
                        vueltas = 0;
                        Row = 0;
                        aux6 = corroborador;
                    }

                    lblNumber6.Content = corroborador.ToString();
                    if (corroborador == 1)
                    {
                        lblOk6.Content = "Error";
                        lblOk6.Foreground = Brushes.Red;
                    }
                    else if (corroborador != 1 && corroborador != 0)
                    {
                        lblOk6.Content = "Errores";
                        lblOk6.Foreground = Brushes.Red;
                    }


                    count++;

                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio Citytv, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //ETTV
                try
                {
                    progress66.Visibility = Visibility.Visible;
                    progress55.IsIndeterminate = false;
                    lblProgres66.Visibility = Visibility.Visible;
                    lblOk55.Visibility = Visibility.Visible;
                    lblNumber55.Visibility = Visibility.Visible;
                    lblOk66.Content = "OK";
                    lblOk66.Foreground = Brushes.Green;
                    Row = 2;




                    //codigo de validacion del archivo (SHORT)
                    SLDocument SLDocument66 = new SLDocument(rutaExcel, hoja2);


                    //hago un ciclo adicional con # cantidad de vueltas para que valide columnas nulas despues del contenido, garantizando que no hay nada mas despues
                    //de las colimnas requeridas por cherry

                    var columnasAdicionales = 7;
                    var vueltas = 0;


                    while (columnasAdicionales <= 47)
                    {
                        while (string.IsNullOrEmpty(SLDocument66.GetCellValueAsString(Row, columnasAdicionales)) && vueltas <= 400)
                        {

                            //lblNumber6.Content = Row.ToString();
                            Row++;
                            vueltas++;


                        }


                        if (vueltas < 400)
                        {
                            corroborador11++;
                            listErrorCol11.Add(new TablaViewModel() { Columna = columnasAdicionales.ToString(), Fila = Row.ToString() });
                            //listErrorRow.Add(Row.ToString());

                            //MessageBox.Show("hay contenido adicional que debe ser borrado en Columna..." + columnasAdicionales.ToString() +  " fila..." + Row.ToString());
                        }

                        columnasAdicionales++;
                        vueltas = 0;
                        Row = 0;
                        aux66 = corroborador11;
                    }

                    lblNumber66.Content = corroborador11.ToString();
                    if (corroborador11 == 1)
                    {
                        lblOk66.Content = "Error";
                        lblOk66.Foreground = Brushes.Red;
                    }
                    else if (corroborador11 != 1 && corroborador11 != 0)
                    {
                        lblOk66.Content = "Errores";
                        lblOk66.Foreground = Brushes.Red;
                    }


                    count1++;

                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio ETTV, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if (count == 7)
            {
                //Citytv
                try
                {
                    progress7.Visibility = Visibility.Visible;
                    progress6.IsIndeterminate = false;
                    lblProgres7.Visibility = Visibility.Visible;
                    lblOk6.Visibility = Visibility.Visible;
                    lblNumber6.Visibility = Visibility.Visible;
                    Row = 2;

                   


                    //codigo de validacion del archivo (SHORT)

                    SLDocument SLDocument7 = new SLDocument(rutaExcel, hoja1);

                    while (!string.IsNullOrEmpty(SLDocument7.GetCellValueAsString(Row, 5)))
                    {

                        lblNumber7.Content = Row.ToString();
                        Row++;
                        aux7 = Row;

                    }

                    count++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio Citytv, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //ETTV
                try
                {
                    progress77.Visibility = Visibility.Visible;
                    progress66.IsIndeterminate = false;
                    lblProgres77.Visibility = Visibility.Visible;
                    lblOk66.Visibility = Visibility.Visible;
                    lblNumber66.Visibility = Visibility.Visible;
                    Row = 2;




                    //codigo de validacion del archivo (SHORT)

                    SLDocument SLDocument77 = new SLDocument(rutaExcel, hoja2);

                    while (!string.IsNullOrEmpty(SLDocument77.GetCellValueAsString(Row, 5)))
                    {

                        lblNumber77.Content = Row.ToString();
                        Row++;
                        aux77 = Row;

                    }

                    count1++;
                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo para el servicio ETTV, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                //City
                progress7.IsIndeterminate = false;
                lblOk7.Visibility = Visibility.Visible;
                lblNumber7.Visibility = Visibility.Visible;

                //ET
                progress77.IsIndeterminate = false;
                lblOk77.Visibility = Visibility.Visible;
                lblNumber77.Visibility = Visibility.Visible;

                Row = 2;


                //City
                if (aux1 == aux2 && aux2 == aux3 && aux3 == aux4 && aux4 == aux5 && aux1 != 0 && aux2 != 0 && aux3 != 0 && aux4 != 0 && aux5 != 0 && aux6 == 0 && validaFechaCelda == 0 && validaHoraCelda == 0 && validaDuracionCelda == 0)
                {
                    Seguir.Visibility = Visibility.Visible;
                    Seguir.Content = "Correcto";
                    Seguir.IsEnabled = true;

                    aux1 = 0;
                    aux2 = 0;
                    aux3 = 0;
                    aux4 = 0;
                    aux5 = 0;
                    aux6 = 0;
                    aux7 = 0;
                    count = 1;
                }

                else
                {
                    //codigo que muestra si los campso no fueron iguales en numero
                    Seguir.Visibility = Visibility.Visible;
                    Seguir.Content = "Corrija los errores";
                    Seguir.IsEnabled = false;

                    if (aux1 == aux2 && aux2 == aux3 && aux3 == aux4 && aux4 == aux5 && aux1 != 0 && aux2 != 0 && aux3 != 0 && aux4 != 0 && aux5 != 0 && aux6 != 0)
                    {
                        ShowError1 error1 = new ShowError1(listErrorCol,"Citytv");
                        error1.Show();

                    }

                    //codigo que muestra un resume de errores de la tabla

                    if (validaFechaCelda == 1 || validaHoraCelda == 1 || validaDuracionCelda == 1)
                    {
                        ShowError2 showError2 = new ShowError2(hojaViewModels1, "Citytv");
                        showError2.Show();
                    }


                    aux1 = 0;
                    aux2 = 0;
                    aux3 = 0;
                    aux4 = 0;
                    aux5 = 0;
                    aux6 = 0;
                    count = 1;
                }

                //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                //ET
                if (aux11 == aux22 && aux22 == aux33 && aux33 == aux44 && aux44 == aux55 && aux11 != 0 && aux22 != 0 && aux33 != 0 && aux44 != 0 && aux55 != 0 && aux66 == 0 && validaFechaCelda1 == 0 && validaHoraCelda1 == 0 && validaDuracionCelda1 == 0)
                {
                    Seguir2.Visibility = Visibility.Visible;
                    Seguir2.Content = "Correcto";
                    Seguir2.IsEnabled = true;
                    btnOk.Visibility = Visibility.Visible;

                    aux11 = 0;
                    aux22 = 0;
                    aux33 = 0;
                    aux44 = 0;
                    aux55 = 0;
                    aux66 = 0;
                    aux77 = 0;
                    count1 = 1;
                }

                else
                {
                    //codigo que muestra si los campso no fueron iguales en numero
                    Seguir2.Visibility = Visibility.Visible;
                    Seguir2.Content = "Corrija los errores";
                    Seguir2.IsEnabled = false;

                    if (aux11 == aux22 && aux22 == aux33 && aux33 == aux44 && aux44 == aux55 && aux11 != 0 && aux22 != 0 && aux33 != 0 && aux44 != 0 && aux55 != 0 && aux66 != 0)
                    {
                        ShowError1 error1 = new ShowError1(listErrorCol11, "ETTV");
                        error1.Show();

                    }

                    if (validaFechaCelda1 == 1 || validaHoraCelda1 == 1 || validaDuracionCelda1 == 1)
                    {
                        ShowError2 showError2 = new ShowError2(hojaViewModels2, "ETTV");
                        showError2.Show();
                    }

                    aux11 = 0;
                    aux22 = 0;
                    aux33 = 0;
                    aux44 = 0;
                    aux55 = 0;
                    aux66 = 0;
                    count1 = 1;
                }

                timer.Stop();
                count = 1;
                count1 = 1;

            }
            #endregion


        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Dual2 dual2 = new Dual2(rutaExcel,hoja1,hoja2);
            dual2.Show();
        }
    }
}
