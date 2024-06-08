using System;
using System.Collections.Generic;
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
using System.IO;

namespace EPG_translate
{
    /// <summary>
    /// Lógica de interacción para Principal.xaml
    /// </summary>
    public partial class Principal : Window
    {
       
        IngestaA ingestaA= new IngestaA();

        //inicializo el timer para el splash
        DispatcherTimer timer = new DispatcherTimer();
        public int count = 0;

        //Colore del led
        public int colorLedCity = 1;
        public int colorLedET = 1;

        //Variables locales y globales de las fechas en los archivos. deiv
        public string TxtInicioCity;
        public string TxtIFinalCity;
        public string TxtInicioET;
        public string TxtFinalET;

        //variable global para la fecha actual



        public Principal()
        {
            InitializeComponent();

            //Defino el timer
            timer.Tick += new EventHandler(WaitingEvent);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();

            getInitCity();
        }


        public void WaitingEvent(object Source, EventArgs e)
        {
            if (count == 0)
            {
                IndicadorCity.Fill = System.Windows.Media.Brushes.White; 
                IndicadorEltiempo.Fill = System.Windows.Media.Brushes.White;
                count++;
            }

            else
            {
                
                count = 0;
                getInitCity();

                //actualizo fecha actual
                txtFecha.Text = DateTime.Now.Date.ToString("dd MMMM yyyy");


                if (colorLedCity == 0)
                {
                    IndicadorCity.Fill = System.Windows.Media.Brushes.Green;
                }

                else if (colorLedCity == 1)
                {
                    IndicadorCity.Fill = System.Windows.Media.Brushes.Red;
                }

                //validando Fecha actual con la fecha guardada en el archivo.deiv

                DateTime Actual = DateTime.Now.Date;
                DateTime FechaInicioCity = Convert.ToDateTime(TxtInicioCity);
                DateTime FechaFinalCity = Convert.ToDateTime(TxtIFinalCity);
                DateTime FechaInicioET = Convert.ToDateTime(TxtInicioET);
                DateTime FechaFinalET = Convert.ToDateTime(TxtFinalET);

                int datoIntDiasCity = int.Parse(txtDiasCity.Text);
                int datoIntDiasET = int.Parse(txtDiasET.Text);

                if (FechaInicioCity <= Actual && FechaFinalCity <= Actual && FechaInicioCity < FechaFinalCity && datoIntDiasCity >= 0  || FechaInicioCity <= Actual && FechaFinalCity > Actual && FechaInicioCity < FechaFinalCity && datoIntDiasCity >= 0)
                {
                    colorLedCity = 0;
                }
                else
                {
                    colorLedCity = 1;
                }


                //codigo que trae los dias restantes en el servicio Citytv
                TimeSpan diasrestantesCity = FechaFinalCity.Subtract(Actual);
                var diasCity = diasrestantesCity.Days;
                txtDiasCity.Text = diasCity.ToString();

                if (diasCity <= 0)
                {
                    txtDiasCity.Foreground = System.Windows.Media.Brushes.Red;
                }
                else { txtDiasCity.Foreground = System.Windows.Media.Brushes.Green; }

                //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


                if (colorLedET == 0)
                {
                    IndicadorEltiempo.Fill = System.Windows.Media.Brushes.Green;
                }

                else if (colorLedET == 1)
                {
                    IndicadorEltiempo.Fill = System.Windows.Media.Brushes.Red;
                }

               

                if (FechaInicioET <= Actual && FechaFinalET <= Actual && FechaInicioET < FechaFinalET && datoIntDiasET >= 0 || FechaInicioET <= Actual && FechaFinalET > Actual && FechaInicioET < FechaFinalET && datoIntDiasET >= 0)
                {
                    colorLedET = 0;
                }
                else
                {
                    colorLedET = 1;
                }

                //codigo que trae los dias restantes en el servicio ETTV
                TimeSpan diasrestantesET = FechaFinalET.Subtract(Actual);
                var diasET = diasrestantesET.Days;
                txtDiasET.Text = diasET.ToString();

                if (diasET <= 0)
                {
                    txtDiasET.Foreground = System.Windows.Media.Brushes.Red;
                }
                else { txtDiasET.Foreground = System.Windows.Media.Brushes.Green; }

                //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            }
        }

        private void getInitCity()
        {
            //logica para traer de un archivo de texto plano la informacion de fecha de inicio en el servicio Citytv

            try
            {
                //CREACION DE LOS ARCHIVOS ARCHIVO

                //paths
                string path1 = "Conf-InitCity.deivid";
                string path2 = "Conf-FinalCity.deivid";
                string path3 = "Conf-InitET.deivid";
                string path4 = "Conf-FinalET.deivid";

                //Datos a Escribir
                string fechaPorDefecto = DateTime.Now.Date.ToString("dd MMMM yyyy");
                
                //creo o no los archivos de configuracion

                if (!File.Exists(path1))
                {
                    TextWriter archivo1 = new StreamWriter(path1);
                    archivo1.WriteLine(fechaPorDefecto);
                    archivo1.Close();
                }

                if (!File.Exists(path2))
                {
                    TextWriter archivo2 = new StreamWriter(path2);
                    archivo2.WriteLine(fechaPorDefecto);
                    archivo2.Close();
                }


                if (!File.Exists(path3))
                {
                    TextWriter archivo3 = new StreamWriter(path3);
                    archivo3.WriteLine(fechaPorDefecto);
                    archivo3.Close();
                }

                if (!File.Exists(path4))
                {
                    TextWriter archivo4 = new StreamWriter(path4);
                    archivo4.WriteLine(fechaPorDefecto);
                    archivo4.Close();
                }
              
            }
            catch (Exception s)
            {
                MessageBox.Show("No se pudo crear los archivos de configuración, el sistema dice...\n" + s.Message,"Error de escritura de archivos",MessageBoxButton.OK,MessageBoxImage.Error);
            }

            try
            {
                //LECTURA DE ARCHIVOS
                TextReader leer1 = new StreamReader("Conf-InitCity.deivid");
                var read1 = leer1.ReadLine();
                txtInicioCity.Text = Convert.ToDateTime(read1).ToString("dd MMMM yyyy");
                TxtInicioCity = read1;
                leer1.Close();

                TextReader leer2 = new StreamReader("Conf-FinalCity.deivid");
                var read2 = leer2.ReadLine();
                txtIFinalCity.Text = Convert.ToDateTime(read2).ToString("dd MMMM yyyy");
                TxtIFinalCity = read2;
                leer2.Close();

                TextReader leer3 = new StreamReader("Conf-InitET.deivid");
                var read3 = leer3.ReadLine();
                txtInicioET.Text = Convert.ToDateTime(read3).ToString("dd MMMM yyyy");
                TxtInicioET = read3;
                leer3.Close();

                TextReader leer4 = new StreamReader("Conf-FinalET.deivid");
                var read4 = leer4.ReadLine();
                txtFinalET.Text = Convert.ToDateTime(read4).ToString("dd MMMM yyyy");
                TxtFinalET = read4;
                leer4.Close();
            }
            catch (Exception s)
            {

                MessageBox.Show("No se pudo tener acceso a uno o más archivos de configuración, compruebe que dichos archivos existan. El sistema dice...\n" + s.Message,"Error de lectura de archivos de configuración",MessageBoxButton.OK,MessageBoxImage.Error);
            }

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Configuracion configuracion = new Configuracion();
            configuracion.Show();
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            App.Current.Shutdown();
        
        }

        private void Image_MouseLeftButtonDown_2(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            ingestaA.Show();
        }


    }
}
