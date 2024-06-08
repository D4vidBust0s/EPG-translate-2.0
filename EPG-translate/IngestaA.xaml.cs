using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Win32;
using SpreadsheetLight;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace EPG_translate
{
    /// <summary>
    /// Lógica de interacción para IngestaA.xaml
    /// </summary>
    public partial class IngestaA : Window
    {
        //inicializo el timer
        DispatcherTimer timer = new DispatcherTimer();
        public int count = 1;
        

        //Configuracion de la libreria SpreadSheetLight
        string rutaExcel = @"";
        int Row = 2;

        //Validacion si la fecha fueValida o no celda por celda 
        public int validaFechaCelda = 0;
        public int validaHoraCelda = 0;
        public int validaDuracionCelda = 0;

        //Variables para validar si todas las celdas son iguales en numero
        public int aux1 = 0;
        public int aux2 = 0;
        public int aux3 = 0;
        public int aux4 = 0;
        public int aux5 = 0;
        public int aux6 = 0;
        public int aux7 = 0;

        public int corroborador = 0;

        public string hoja = "";
        public int cantidadDeServicios = 1;
        public int id = 1;
        public int extArchivo = 0;

        List<TablaViewModel> listErrorCol = new List<TablaViewModel>();
        List<TablaViewModel> listErrorRow = new List<TablaViewModel>();
        List<HojaViewModel> hojaViewModels = new List<HojaViewModel>();

        //lista que recibe las celdas de la fila 1 que no son validas
        public List<int> celdasFechasNulas = new List<int>();


        public IngestaA()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(validacion);

            comboEltiempo.IsEnabled = false;
            combo_servicio.SelectedIndex = 0;
            cantidadDeServicios = 1;
            

            //ocultamos todos los controles que se usan visualmente para mostrar el proceso de analisis del archivo Excel
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


          
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Configuracion configuracion = new Configuracion();
            configuracion.Show();
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Principal principal = new Principal();
            this.Hide();
            principal.Show();
        }

        private void archivo_Click(object sender, RoutedEventArgs e)
        {
            //este simple codigo es para restablecer el encabezado de errores del documento
            encabezadoAn.Foreground = Brushes.White;
            encabezadoAn.Content = "validaciónalidación de documento";

            //vacio las listas usadas
            listErrorCol.Clear();
            listErrorRow.Clear();
            hojaViewModels.Clear();

            //ocultamos todos los controles que se usan visualmente para mostrar el proceso de analisis del archivo Excel
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

            lblOk1.Foreground = Brushes.Green;
            lblOk2.Foreground = Brushes.Green;
            lblOk3.Foreground = Brushes.Green;
            lblOk4.Foreground = Brushes.Green;
            lblOk5.Foreground = Brushes.Green;
            lblOk6.Foreground = Brushes.Green;
            lblOk7.Foreground = Brushes.Green;


            Seguir.Visibility = Visibility.Hidden;

            corroborador = 0;


            try
            {
                if (Cargar_archivo.Text == string.Empty)
                {
                    MessageBox.Show("Se debe especificar un archivo Excel para hacer el análisis", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                else if (combo_servicio.Text == string.Empty)
                {
                    MessageBox.Show("Se debe especificar el servicio hacia el cual será ingestado el archivo seleccionado", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                else
                {
                    //codigo para el analisis del archivo, segun sea el servicio redirecciono 

                    if (combo_servicio.SelectedIndex==0 || combo_servicio.SelectedIndex == 1)
                    {
                        //Inicializo el timer
                        timer.Interval = new TimeSpan(0, 0, 1);
                        timer.Start();
                    }

                    else if (combo_servicio.SelectedIndex == 2)
                    {
                        Dual1 dual1 = new Dual1(rutaExcel, comboCity.Text,comboEltiempo.Text);
                        dual1.Show();
                        this.Hide();
                    }

                   

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error no especificado por el desarrollador, póngase en contacto con el administrador del sistema", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
           
        }

        private void validacion(object sender, EventArgs e)
        {
            //PROCESO DE VALIDACION Y RECODIFICACION DEL ARCHIVO
            
            #region Extension Xlsx - Xlsm
            if (count == 1)
                {

                    progress1.Visibility = Visibility.Visible;
                    lblProgres1.Visibility = Visibility.Visible;

                    if (combo_servicio.Text == "Citytv")
                    {
                        hoja = "Hoja" + comboCity.Text;
                    }

                    else if (combo_servicio.Text == "El Tiempo Televisión")
                    {
                        hoja = "Hoja" + comboEltiempo.Text;
                    }


                    //codigo de validacion del archivo (FECHA)
                    try
                    {
                        SLDocument SLDocument1 = new SLDocument(rutaExcel, hoja);
                        

                        while (!string.IsNullOrEmpty(SLDocument1.GetCellValueAsString(Row, 1)))
                        {


                            lblNumber1.Content = Row.ToString();

                            try
                            {
                            //Valido si el contenido de la celda es compatible con un formato fecha

                                string cadena = SLDocument1.GetCellValueAsDateTime(Row, 1).ToString("dd MM yyyy");
                                
                                cadena = Regex.Replace(cadena, " ","-");
                                DateTime dateTimeFecha = DateTime.Parse(cadena);
                         

                            if (dateTimeFecha == DateTime.Parse("01-01-1900"))
                                {
                                  validaFechaCelda = 1;
                                  hojaViewModels.Add(new HojaViewModel() { Date = Row.ToString() });

                                  encabezadoAn.Foreground = Brushes.Red;
                                  encabezadoAn.Content = "ERRORES DETECTADOS";

                                }
                        
                            }

                            catch (Exception ex)
                            {

                                MessageBox.Show(" error intentando leer y convertir La celda # "+Row+ " En la fila # 1.\nAvise al dessarollador sobre este problema\n" 
                                    + ex.Message,"Error de formato de fecha",MessageBoxButton.OK,MessageBoxImage.Warning);
                            }


                            Row++;
                            aux1 = Row;

                        }

                        count++;
                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Error al leer el archivo, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                    }


                }
            else if (count == 2)
                {
                    try
                    {
                        progress2.Visibility = Visibility.Visible;
                        progress1.IsIndeterminate = false;
                        lblProgres2.Visibility = Visibility.Visible;
                        lblOk1.Visibility = Visibility.Visible;
                        lblNumber1.Visibility = Visibility.Visible;
                        Row = 2;

                        if (combo_servicio.Text == "Citytv")
                        {
                            hoja = "Hoja" + comboCity.Text;
                        }

                        else if (combo_servicio.Text == "El Tiempo Televisión")
                        {
                            hoja = "Hoja" + comboEltiempo.Text;
                        }


                        //codigo de validacion del archivo (HORA)

                        SLDocument SLDocument2 = new SLDocument(rutaExcel, hoja);

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
                                   hojaViewModels.Add(new HojaViewModel() { Time = Row.ToString() });

                                   encabezadoAn.Foreground = Brushes.Red;
                                   encabezadoAn.Content = "ERRORES DETECTADOS"; 

                                }

                            }

                            catch (Exception ex)
                            {

                                MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                    + ex.Message, "Error de formato de hora", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }


                        Row++;
                            aux2 = Row;

                        }

                        count++;
                        
                }
                    catch (Exception)
                    {

                        MessageBox.Show("Error al leer el archivo, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            else if (count == 3)
                {
                    try
                    {
                        progress3.Visibility = Visibility.Visible;
                        progress2.IsIndeterminate = false;
                        lblProgres3.Visibility = Visibility.Visible;
                        lblOk2.Visibility = Visibility.Visible;
                        lblNumber2.Visibility = Visibility.Visible;
                        Row = 2;

                        if (combo_servicio.Text == "Citytv")
                        {
                            hoja = "Hoja" + comboCity.Text;
                        }

                        else if (combo_servicio.Text == "El Tiempo Televisión")
                        {
                            hoja = "Hoja" + comboEltiempo.Text;
                        }


                        //codigo de validacion del archivo (DURACION)

                        SLDocument SLDocument3 = new SLDocument(rutaExcel, hoja);

                        while (!string.IsNullOrEmpty(SLDocument3.GetCellValueAsString(Row, 3)))
                        {

                            lblNumber3.Content = Row.ToString();

                        try
                        {
                            //Valido si el contenido de la celda es compatible con un formato hora

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
                                hojaViewModels.Add(new HojaViewModel() { Duration = Row.ToString() });

                                encabezadoAn.Foreground = Brushes.Red;
                                encabezadoAn.Content = "ERRORES DETECTADOS";
                            }

                        }

                        catch (Exception ex)
                        {

                            MessageBox.Show(" error intentando leer y convertir La celda # " + Row + " En la fila # 1.\nAvise al dessarollador sobre este problema\n"
                                + ex.Message, "Error de formato de hora", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }

                        Row++;
                        aux3 = Row;

                        }

                        count++;
                       
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Error al leer el archivo, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            else if (count == 4)
                {
                    try
                    {
                        progress4.Visibility = Visibility.Visible;
                        progress3.IsIndeterminate = false;
                        lblProgres4.Visibility = Visibility.Visible;
                        lblOk3.Visibility = Visibility.Visible;
                        lblNumber3.Visibility = Visibility.Visible;
                        Row = 2;

                        if (combo_servicio.Text == "Citytv")
                        {
                            hoja = "Hoja" + comboCity.Text;
                        }

                        else if (combo_servicio.Text == "El Tiempo Televisión")
                        {
                            hoja = "Hoja" + comboEltiempo.Text;
                        }

                        //codigo de validacion del archivo (TITULO)

                        SLDocument SLDocument4 = new SLDocument(rutaExcel, hoja);

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

                        MessageBox.Show("Error al leer el archivo, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            else if (count == 5)
                {
                    try
                    {
                        progress5.Visibility = Visibility.Visible;
                        progress4.IsIndeterminate = false;
                        lblProgres5.Visibility = Visibility.Visible;
                        lblOk4.Visibility = Visibility.Visible;
                        lblNumber4.Visibility = Visibility.Visible;
                        Row = 2;

                        if (combo_servicio.Text == "Citytv")
                        {
                            hoja = "Hoja" + comboCity.Text;
                        }

                        else if (combo_servicio.Text == "El Tiempo Televisión")
                        {
                            hoja = "Hoja" + comboEltiempo.Text;
                        }


                        //codigo de validacion del archivo (SHORT)

                        SLDocument SLDocument5 = new SLDocument(rutaExcel, hoja);

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

                        MessageBox.Show("Error al leer el archivo, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            else if (count == 6)
            {
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

                    if (combo_servicio.Text == "Citytv")
                    {
                        hoja = "Hoja" + comboCity.Text;
                    }

                    else if (combo_servicio.Text == "El Tiempo Televisión")
                    {
                        hoja = "Hoja" + comboEltiempo.Text;
                    }


                    //codigo de validacion del archivo (SHORT)
                    SLDocument SLDocument6 = new SLDocument(rutaExcel, hoja);


                    //hago un ciclo adicional con # cantidad de vueltas para que valide columnas nulas despues del contenido, garantizando que no hay nada mas despues
                    //de las colimnas requeridas por cherry

                    var columnasAdicionales = 7;
                    var vueltas = 0;
                   

                    while (columnasAdicionales<=47)
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
                            listErrorCol.Add(new TablaViewModel() { Columna = columnasAdicionales.ToString(), Fila = Row.ToString()});
                            //listErrorRow.Add(Row.ToString());
                            
                            //MessageBox.Show("hay contenido adicional que debe ser borrado en Columna..." + columnasAdicionales.ToString() +  " fila..." + Row.ToString());
                        }
                        
                        columnasAdicionales++;
                        vueltas = 0;
                        Row = 0;
                        aux6 = corroborador;
                    }

                    lblNumber6.Content = corroborador.ToString();
                    if (corroborador==1)
                    {
                        lblOk6.Content = "Error";
                        lblOk6.Foreground = Brushes.Red;
                    }
                    else if(corroborador != 1 && corroborador != 0)
                    { 
                        lblOk6.Content = "Errores"; 
                        lblOk6.Foreground = Brushes.Red; 
                    }
                   

                    count++;

                }
                catch (Exception)
                {

                    MessageBox.Show("Error al leer el archivo, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (count == 7)
            {
                try
                {
                    progress7.Visibility = Visibility.Visible;
                    progress6.IsIndeterminate = false;
                    lblProgres7.Visibility = Visibility.Visible;
                    lblOk6.Visibility = Visibility.Visible;
                    lblNumber6.Visibility = Visibility.Visible;
                    Row = 2;

                    if (combo_servicio.Text == "Citytv")
                    {
                        hoja = "Hoja" + comboCity.Text;
                    }

                    else if (combo_servicio.Text == "El Tiempo Televisión")
                    {
                        hoja = "Hoja" + comboEltiempo.Text;
                    }


                    //codigo de validacion del archivo (SHORT)

                    SLDocument SLDocument7 = new SLDocument(rutaExcel, hoja);

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

                    MessageBox.Show("Error al leer el archivo, compruebe que el documento Excel no este abierto, de estar abierto ciérrelo para que el programa continúe con la operación de validación.", "EPG-Translate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                {
                    progress7.IsIndeterminate = false;
                    lblOk7.Visibility = Visibility.Visible;
                    lblNumber7.Visibility = Visibility.Visible;

                    Row = 2;


                    if (aux1 == aux2 && aux2 == aux3 && aux3 == aux4 && aux4 == aux5 && aux1 != 0 && aux2 != 0 && aux3 != 0 && aux4 != 0 && aux5 != 0 && aux6 == 0 && validaFechaCelda == 0 && validaHoraCelda == 0 && validaDuracionCelda == 0)
                    {
                        Seguir.Visibility = Visibility.Visible;
                        Seguir.Content = "Continuar";
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

                        if(aux1 == aux2 && aux2 == aux3 && aux3 == aux4 && aux4 == aux5 && aux1 != 0 && aux2 != 0 && aux3 != 0 && aux4 != 0 && aux5 != 0 && aux6 != 0)
                        {
                            ShowError1 error1 = new ShowError1(listErrorCol,combo_servicio.Text);
                            error1.Show();
                            
                        }

                        //codigo que muestra un resume de errores de la tabla

                        if (validaFechaCelda == 1 || validaHoraCelda == 1 || validaDuracionCelda == 1)
                        {
                            ShowError2 showError2 = new ShowError2(hojaViewModels, combo_servicio.Text);
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

                    timer.Stop();
                    count = 1;

                }
            #endregion
            

        }

        private void Cargar_archivo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //ocultamos todos los controles que se usan visualmente para mostrar el proceso de analisis del archivo Excel
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

            lblOk1.Foreground = Brushes.Green;
            lblOk2.Foreground = Brushes.Green;
            lblOk3.Foreground = Brushes.Green;
            lblOk4.Foreground = Brushes.Green;
            lblOk5.Foreground = Brushes.Green;
            lblOk6.Foreground = Brushes.Green;
            lblOk7.Foreground = Brushes.Green;


            Seguir.Visibility = Visibility.Hidden;

            corroborador = 0;
           



            //Codigo para cargar el archivo excel
            OpenFileDialog openfiledialog = new OpenFileDialog();
            openfiledialog.Filter = "Archivos Excel xlsx (*.xlsx)|*.xlsx|Archivos Excel xlsm(*.xlsm)|*.xlsm";
            openfiledialog.Title = "seleccione el archivo de Excel";
            openfiledialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            

            if (openfiledialog.ShowDialog() == true)
            {
                Cargar_archivo.Text = openfiledialog.FileName;
                rutaExcel = openfiledialog.FileName;
                extArchivo = openfiledialog.FilterIndex;
                lblNombreArchivo.Content = openfiledialog.SafeFileName;
            }

        }

        private void Seguir_Click(object sender, RoutedEventArgs e)
        {
            if (id == 1)
            {
                IngestaB ingestaB = new IngestaB(rutaExcel, hoja, cantidadDeServicios, id);
                ingestaB.Show();
                this.Hide();
            }

            else if (id == 2)
            {
                IngestaC ingestaC = new IngestaC(rutaExcel, hoja, cantidadDeServicios, id);
                ingestaC.Show();
                this.Hide();
            }

           

        }

        private void Ccombo_servicio_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            try
            {

                if(combo_servicio.SelectedIndex == 0)
                {
                    comboCity.IsEnabled = true;
                    comboEltiempo.IsEnabled = false;
                    cantidadDeServicios = 1;

                    comboCity.Text = "1";
                    comboEltiempo.Text = "1";

                    id = 1;
                }
                else if (combo_servicio.SelectedIndex == 1)
                {
                    comboCity.IsEnabled = false;
                    comboEltiempo.IsEnabled = true;
                    cantidadDeServicios = 1;

                    comboCity.Text = "1";
                    comboEltiempo.Text = "1";

                    id = 2;
                }

                else if (combo_servicio.SelectedIndex == 2)
                {
                    comboCity.IsEnabled = true;
                    comboEltiempo.IsEnabled = true;
                    cantidadDeServicios = 2;

                    comboCity.Text = "1";
                    comboEltiempo.Text = "2";

                    id = 3;
                }
            }
            catch (Exception)
            {

                
            }
           
        }
    }
}
