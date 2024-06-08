using System;
using System.Collections.Generic;
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
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace EPG_translate
{
    /// <summary>
    /// Lógica de interacción para IngestaD.xaml
    /// </summary>
    public partial class IngestaD : Window
    {
        //inicializo el timer
        DispatcherTimer timer = new DispatcherTimer();
        public int count = 1;

        //Variables locales que reciben los datos de la anterior ventana
        public int Numerodearchivos;

        //Variable que esta a la observacion de cualquier fallo global es decir durante todos los procesos
        public int corroborador = 0;

        //variable que me indica cual servicio debo procesar, de la siguiente manera 1 = Citytv, 2 = El tiempo TV, 3 = los dos a la vez
        public int NumeroDeArchivos = 0;

        //variables para la conexion con el servidor cherryEPG
        public string host = string.Empty;
        public int port = 0;
        public string username = string.Empty;
        public string password = string.Empty;

        //variable que se ejecuta en todos los procesos y si es alterada a 1 describira que hubo errores
        public int validador = 0;


        public IngestaD(int numeroDeArchivos)
        {
            InitializeComponent();
            getServer();
            getPort();
            getUserName();  
            getPassword();
           
            NumeroDeArchivos = numeroDeArchivos;

            //ocultamos todos los controles que se usan visualmente para mostrar el proceso de analisis del archivo Excel
            progress1.Visibility = Visibility.Hidden;
            progress2.Visibility = Visibility.Hidden;
            progress3.Visibility = Visibility.Hidden;
            progress4.Visibility = Visibility.Hidden;
            progress5.Visibility = Visibility.Hidden;

            lblProgres1.Visibility = Visibility.Hidden;
            lblProgres2.Visibility = Visibility.Hidden;
            lblProgres3.Visibility = Visibility.Hidden;
            lblProgres4.Visibility = Visibility.Hidden;
            lblProgres5.Visibility = Visibility.Hidden;

            lblOk1.Visibility = Visibility.Hidden;
            lblOk2.Visibility = Visibility.Hidden;
            lblOk3.Visibility = Visibility.Hidden;
            lblOk4.Visibility = Visibility.Hidden;
            lblOk5.Visibility = Visibility.Hidden;


            finalizar.Visibility = Visibility.Hidden;



            //inicializo el timer
            Numerodearchivos = numeroDeArchivos;
            timer.Tick += new EventHandler(validacion);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();


        }

        //Obtencion de datos de los archivos de configuración
        private void getServer()
        {
            try
            {
                TextReader leer1 = new StreamReader("Conf-data1.deivid");
                string read1 = leer1.ReadLine();
                host = read1;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error intentando leer el archivo de configuración de la direccion ip del servidor, el sistema dice...\n"+ex.Message,"Error de lectura de archivo",MessageBoxButton.OK,MessageBoxImage.Error);
            }   
        }

        private void getPort()
        {
            try
            {
                TextReader leer1 = new StreamReader("Conf-data2.deivid");
                string read1 = leer1.ReadLine();
                port = int.Parse(read1);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error intentando leer el archivo de configuraciön del puerto para la conexión con el servidor, el sistema dice...\n" + ex.Message, "Error de lectura de archivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void getUserName()
        {
            try
            {
                TextReader leer1 = new StreamReader("Conf-data3.deivid");
                string read1 = leer1.ReadLine();
                username = read1;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error intentando leer el archivo de configuraciön del usuario para la conexión con el servidor, el sistema dice...\n" + ex.Message, "Error de lectura de archivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void getPassword()
        {
            try
            {
                TextReader leer1 = new StreamReader("Conf-data4.deivid");
                string read1 = leer1.ReadLine();
                password = read1;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error intentando leer el archivo de configuraciön del password para la conexión con el servidor, el sistema dice...\n" + ex.Message, "Error de lectura de archivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Obtener listado del directorio remoto
        private void showFiles(SftpClient client, string serverFolder)
        {
            var paths = client.ListDirectory(serverFolder);
            foreach ( var path in paths)
            {
                Console.WriteLine(path.IsDirectory?"Directory: "+path.FullName : "file: "+path.FullName);
            }
        }

        private int Test()
        {
            try
            {
                using (var sshClient = new SshClient(host, port, username, password))
                {
                    sshClient.Connect();
                    return 1;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error conectando con el servidor","Error de conexion",MessageBoxButton.OK,MessageBoxImage.Error);
                return 0; 
            }
        }

        private int ClearDir()
        {
            try
            {
                using (var sshClient = new SshClient(host, port, username, password))
                {
                    sshClient.Connect();

                    var cmd = sshClient.RunCommand("/var/lib/cherryepg/cherryTool/bin/./cherryTool -d " + NumeroDeArchivos.ToString());
                    var asyncExecute = cmd.BeginExecute();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int ClearDirFull()
        {
            try
            {
                using (var sshClient = new SshClient(host, port, username, password))
                {
                    sshClient.Connect();

                    var cmd = sshClient.RunCommand("/var/lib/cherryepg/cherryTool/bin/./cherryTool -d ");
                    var asyncExecute = cmd.BeginExecute();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int uploadFileService1()
        {
            try
            {
                //inicializo servicio FTP con el servidor
                using (SftpClient client = new SftpClient(new PasswordConnectionInfo(host, port, username, password)))
                {
                    client.Connect();
                    client.ChangeDirectory("stock");

                    string sourceFile = @"C:\EPG\ScheduleCITY.xml";
                    using (Stream stream = File.OpenRead(sourceFile))
                    {

                        client.UploadFile(stream, System.IO.Path.GetFileName(sourceFile));
                    }

                    client.Disconnect();
                    return 1;
                    
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error establesiendo conexion FTP con el servidor CherryEPG, o la carpeta EPG en el disco local C: en su equipo no esta creada","Error de conexion",MessageBoxButton.OK,MessageBoxImage.Error);
                return 0;
            }
        }

        private int uploadFileService2()
        {
            try
            {
                //inicializo servicio FTP con el servidor
                using (SftpClient client = new SftpClient(new PasswordConnectionInfo(host, port, username, password)))
                {
                    client.Connect();
                    client.ChangeDirectory("stock");

                    string sourceFile = @"C:\EPG\ScheduleET.xml";
                    using (Stream stream = File.OpenRead(sourceFile))
                    {

                        client.UploadFile(stream, System.IO.Path.GetFileName(sourceFile));
                    }

                    client.Disconnect();
                    return 1;

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error establesiendo conexion FTP con el servidor CherryEPG, o la carpeta EPG en el disco local C: en su equipo no esta creada", "Error de conexion", MessageBoxButton.OK, MessageBoxImage.Error);
                return 0;
            }
        }

        private int ingestServicesSingle()
        {
            try
            {
                using (var sshClient = new SshClient(host, port, username, password))
                {
                    sshClient.Connect();

                    var cmd = sshClient.RunCommand("/var/lib/cherryepg/cherryTool/bin/./cherryTool -gi " + NumeroDeArchivos.ToString());
                    var asyncExecute = cmd.BeginExecute();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int ingestServicesAll()
        {
            try
            {
                using (var sshClient = new SshClient(host, port, username, password))
                {
                    sshClient.Connect();

                    var cmd = sshClient.RunCommand("/var/lib/cherryepg/cherryTool/bin/./cherryTool -G all ");
                    var asyncExecute = cmd.BeginExecute();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private int createEIT()
        {
            try
            {
                using (var sshClient = new SshClient(host, port, username, password))
                {
                    sshClient.Connect();

                    var cmd = sshClient.RunCommand("/var/lib/cherryepg/cherryTool/bin/./cherryTool -fB");
                    var asyncExecute = cmd.BeginExecute();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }




        private void validacion(object sender, EventArgs e)
        {
            #region validadores
            if (count == 1)
            {
                //Estableciendo conexion
                progress1.Visibility = Visibility.Visible;
                lblProgres1.Visibility = Visibility.Visible;

               if (NumeroDeArchivos == 1 || NumeroDeArchivos == 2 || NumeroDeArchivos == 3)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = Test();

                    if (res == 1)
                    {
                        lblOk1.Foreground = Brushes.Green;
                        lblOk1.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk1.Visibility = Visibility;
                        lblOk1.Foreground = Brushes.Red;
                        lblOk1.Content = "ERROR";
                        timer.Stop();

                        MessageBox.Show("Imposible conectar con el servidor de cherryEPG, verifique la configuración y vuelva a intentarlo","Error de conexión",MessageBoxButton.OK,MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }

                else
                {
                    validador = 1;
                    MessageBox.Show("Error intentando validar la cantidad de servicios que se deben procesar, el valor fue cero o nulo.","Error",MessageBoxButton.OK,MessageBoxImage.Error);
                }

                count++;
            }
            else if (count == 2)
            {
                //Limpiando archivos anteriores
                progress2.Visibility = Visibility.Visible;
                progress1.IsIndeterminate = false;
                lblProgres2.Visibility = Visibility.Visible;
                lblOk1.Visibility = Visibility.Visible;


                if (NumeroDeArchivos == 1 || NumeroDeArchivos == 2)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = ClearDir();

                    if (res == 1)
                    {
                        lblOk2.Foreground = Brushes.Green;
                        lblOk2.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk2.Foreground = Brushes.Red;
                        lblOk2.Content = "ERROR";

                        MessageBox.Show("Imposible borrar los archivos anteriores en el servidor de cherryEPG, verifique la conexión y vuelva a intentarlo", "Error borrando archivos", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }

               
                //Para cuando es dual la hoja
                else if (NumeroDeArchivos == 3)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = ClearDirFull();

                    if (res == 1)
                    {
                        lblOk2.Foreground = Brushes.Green;
                        lblOk2.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk2.Foreground = Brushes.Red;
                        lblOk2.Content = "ERROR";

                        MessageBox.Show("Imposible borrar los archivos anteriores en el servidor de cherryEPG servicio dual de EPGtranslate, verifique la conexión y vuelva a intentarlo", "Error borrando archivos", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }

                else
                {
                    validador = 1;
                    MessageBox.Show("Error intentando validar la cantidad de servicios que se deben procesar, el valor fue cero o nulo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                count++;
            }
            else if (count == 3)
            {
                //Subiendo nuevos archivos
                progress3.Visibility = Visibility.Visible;
                progress2.IsIndeterminate = false;
                lblProgres3.Visibility = Visibility.Visible;
                lblOk2.Visibility = Visibility.Visible;

                if (NumeroDeArchivos == 1)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = uploadFileService1();

                    if (res == 1)
                    {
                        lblOk3.Foreground = Brushes.Green;
                        lblOk3.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk3.Foreground = Brushes.Red;
                        lblOk3.Content = "ERROR";

                        MessageBox.Show("Imposible subir el archivo xml de Citytv el servidor de cherryEPG, verifique la conexión y vuelva a intentarlo", "Error de actualización", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }

                else if (NumeroDeArchivos == 2)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = uploadFileService2();

                    if (res == 1)
                    {
                        lblOk3.Foreground = Brushes.Green;
                        lblOk3.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk3.Foreground = Brushes.Red;
                        lblOk3.Content = "ERROR";

                        MessageBox.Show("Imposible subir el archivo xml de ETTV el servidor de cherryEPG, verifique la conexión y vuelva a intentarlo", "Error de actualización", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }


                //Para cuando es dual la hoja
                else if (NumeroDeArchivos == 3)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = uploadFileService1();
                    int res2 = uploadFileService2();

                    if (res == 1 && res2 == 1)
                    {
                        lblOk3.Foreground = Brushes.Green;
                        lblOk3.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk3.Foreground = Brushes.Red;
                        lblOk3.Content = "ERROR";

                        MessageBox.Show("Imposible subir el archivo xml de Citytv y ETTV en el servidor de cherryEPG, verifique la conexión y vuelva a intentarlo", "Error de actualización", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }

                else
                {
                    validador = 1;
                    MessageBox.Show("Error intentando validar la cantidad de servicios que se deben procesar, el valor fue cero o nulo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                count++;
            }
            else if (count == 4)
            {
                //notificando a cherryEPG
                progress4.Visibility = Visibility.Visible;
                progress3.IsIndeterminate = false;
                lblProgres4.Visibility = Visibility.Visible;
                lblOk3.Visibility = Visibility.Visible;


                if (NumeroDeArchivos == 1 || Numerodearchivos == 2)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = ingestServicesSingle();

                    if (res == 1)
                    {
                        lblOk4.Foreground = Brushes.Green;
                        lblOk4.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk4.Foreground = Brushes.Red;
                        lblOk4.Content = "ERROR";

                        MessageBox.Show("Imposible ingestar el archivo XMLTV en el servidor de cherryEPG, verifique la conexión y vuelva a intentarlo", "Error de ingesta", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }


                //Para cuando es dual la hoja
                else if (NumeroDeArchivos == 3)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = ingestServicesAll();
                    

                    if (res == 1)
                    {
                        lblOk4.Foreground = Brushes.Green;
                        lblOk4.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk4.Foreground = Brushes.Red;
                        lblOk4.Content = "ERROR";

                        MessageBox.Show("Imposible ingestar el archivo XMLTV de Citytv y XMLTV de ETTV en el servidor de cherryEPG, verifique la conexión y vuelva a intentarlo", "Error de ingesta", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }

                else
                {
                    validador = 1;
                    MessageBox.Show("Error intentando validar la cantidad de servicios que se deben procesar, el valor fue cero o nulo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }


                count++;
            }
            else if (count == 5)
            {
                //validando creando EIT
                progress5.Visibility = Visibility.Visible;
                progress4.IsIndeterminate = false;
                lblProgres5.Visibility = Visibility.Visible;
                lblOk4.Visibility = Visibility.Visible;

                if (NumeroDeArchivos == 1 || Numerodearchivos == 2 || Numerodearchivos == 3)
                {
                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                    int res = createEIT();

                    if (res == 1)
                    {
                        lblOk5.Foreground = Brushes.Green;
                        lblOk5.Content = "OK";
                    }

                    else if (res == 0)
                    {
                        lblOk5.Foreground = Brushes.Red;
                        lblOk5.Content = "ERROR";

                        MessageBox.Show("Imposible actualizar EIT en el servidor de cherryEPG, verifique la conexión y vuelva a intentarlo", "Error de actualización", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    //------------------------------------------------------------------------------------------------------------------------------------------------------
                }

                else
                {
                    validador = 1;
                    MessageBox.Show("Error intentando validar la cantidad de servicios que se deben procesar, el valor fue cero o nulo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                count++;
            }
            else 
            {
                progress5.IsIndeterminate = false;
                lblOk5.Visibility = Visibility.Visible;

                count = 0; ;
                timer.Stop();

                //Valido si en el proceso hubo errores mediante la variable "validador" si fue modificada a 1  ya que en su defecto es 0
                if (validador == 0)
                {
                    this.Hide();
                    procesoCorrecto pc = new procesoCorrecto();
                    pc.Show();
                }
                else if (validador == 1)
                {
                    this.Hide();
                    procesoFallido pf = new procesoFallido();
                    pf.Show();
                }
               
            }

            #endregion
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            IngestaA ingestaA = new IngestaA();
            ingestaA.Show();
            this.Hide();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            IngestaA ingestaA = new IngestaA();
            ingestaA.Show();
            this.Hide();
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Implementar siguiente codigo");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Disparamos el timer
            timer.Interval = new TimeSpan(0, 0, 2);
            timer.Start();
        }

        private void finalizar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
