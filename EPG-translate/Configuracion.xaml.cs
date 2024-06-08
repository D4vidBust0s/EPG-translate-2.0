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

namespace EPG_translate
{
    /*
     * Las configuraciones del sistema estarán conformadas por las siguientes opciones
     * - GMT
     * - Nombre XML Citytv
     * - Nombre XML ETTV
     * - Servidor IP
     * - Perto
     * - Usuario
     * - Contraseña
     * */
    public partial class Configuracion : Window
    {
        public string dato;

        public Configuracion()
        {
            InitializeComponent();
            CreateFiles();
            GetData();
        }

        private void CreateFiles()
        {
            try
            {
                //CREACION DE LOS ARCHIVOS ARCHIVO

                //paths
                string path1 = "Conf-data1.deivid";
                string path2 = "Conf-data2.deivid";
                string path3 = "Conf-data3.deivid";
                string path4 = "Conf-data4.deivid";
                string path5 = "Conf-data5.deivid";
                string path6 = "Conf-data6.deivid";
                string path7 = "Conf-data7.deivid";


                //creo o no los archivos de configuracion

                if (!File.Exists(path1))
                {
                    TextWriter archivo1 = new StreamWriter("Conf-data1.deivid");
                    archivo1.WriteLine("127.0.0.1");
                    archivo1.Close();
                }

                if (!File.Exists(path2))
                {
                    TextWriter archivo2 = new StreamWriter("Conf-data2.deivid");
                    archivo2.WriteLine("2222");
                    archivo2.Close();
                }


                if (!File.Exists(path3))
                {
                    TextWriter archivo3 = new StreamWriter("Conf-data3.deivid");
                    archivo3.WriteLine("UserCherry");
                    archivo3.Close();
                }

                if (!File.Exists(path4))
                {
                    TextWriter archivo4 = new StreamWriter("Conf-data4.deivid");
                    archivo4.WriteLine("PasswordCherry");
                    archivo4.Close();
                }

                if (!File.Exists(path5))
                {
                    TextWriter archivo5 = new StreamWriter("Conf-data5.deivid");
                    archivo5.WriteLine("+0000");
                    archivo5.Close();
                }

                if (!File.Exists(path6))
                {
                    TextWriter archivo6 = new StreamWriter("Conf-data6.deivid");
                    archivo6.WriteLine("NameFileCity");
                    archivo6.Close();
                }

                if (!File.Exists(path7))
                {
                    TextWriter archivo7 = new StreamWriter("Conf-data7.deivid");
                    archivo7.WriteLine("NameFileETTV");
                    archivo7.Close();
                }

            }
            catch (Exception s)
            {
                MessageBox.Show("No se pudo crear los archivos de configuración, el systema dice..." + s.Message);
            }
        }

        private void GetData()
        {
            try
            {
                //LECTURA DE ARCHIVOS

                TextReader leer1 = new StreamReader("Conf-data1.deivid");
                var read1 = leer1.ReadToEnd();
                txtIp.Text = read1;
                leer1.Close();

                TextReader leer2 = new StreamReader("Conf-data2.deivid");
                var read2 = leer2.ReadToEnd();
                txtPort.Text = read2;
                leer2.Close();

                TextReader leer3 = new StreamReader("Conf-data3.deivid");
                var read3 = leer3.ReadToEnd();
                txtUser.Text = read3;
                leer3.Close();

                TextReader leer4 = new StreamReader("Conf-data4.deivid");
                var read4 = leer4.ReadToEnd();
                txtPassword.Password = read4;
                leer4.Close();

                TextReader leer5 = new StreamReader("Conf-data5.deivid");
                var read5 = leer5.ReadToEnd();
                txtGMT.Text = read5;
                leer5.Close();

                TextReader leer6 = new StreamReader("Conf-data6.deivid");
                var read6 = leer6.ReadToEnd();
                txtNameOne.Text = read6;
                leer6.Close();

                TextReader leer7 = new StreamReader("Conf-data7.deivid");
                var read7 = leer7.ReadToEnd();
                txtNameTwo.Text = read7;
                leer7.Close();


            }
            catch (Exception s)
            {

                MessageBox.Show("No se pudo leer el archivo, el systema dice...\n" + s.Message);
            }
        }

        private int UpdateData(string dato, string NameArchivo)
        {
            try
            {
                TextWriter archivo1 = new StreamWriter(NameArchivo);
                archivo1.WriteLine(dato);
                archivo1.Close();

                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

            
        }




        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if(txtIp.Text==string.Empty)
            {
                MessageBox.Show("El campo IP esta vacio, si desea continuar el sistema no funcionará correctamente","CAMPOS VACIOS",MessageBoxButton.OK,MessageBoxImage.Warning);
                txtIp.Focus();
            }
            else if (txtPort.Text==string.Empty)
            {
                MessageBox.Show("El campo Puerto esta vacio, si desea continuar el sistema no funcionará correctamente", "CAMPOS VACIOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPort.Focus();
            }
            else if (txtUser.Text == string.Empty)
            {
                MessageBox.Show("El campo Usuario esta vacio, si desea continuar el sistema no funcionará correctamente", "CAMPOS VACIOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUser.Focus();
            }
            else if (txtPassword.Password == string.Empty)
            {
                MessageBox.Show("El campo Password esta vacio, si desea continuar el sistema no funcionará correctamente", "CAMPOS VACIOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
            }
            else if (txtGMT.Text == string.Empty)
            {
                MessageBox.Show("El campo GMT esta vacio, si desea continuar el sistema no funcionará correctamente", "CAMPOS VACIOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtGMT.Focus();
            }
            else if (txtNameOne.Text == string.Empty)
            {
                MessageBox.Show("El campo nombre archivo Citytv esta vacio, si desea continuar el sistema no funcionará correctamente", "CAMPOS VACIOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNameOne.Focus();
            }
            else if (txtNameTwo.Text == string.Empty)
            {
                MessageBox.Show("El campo nombre archivo ETTV esta vacio, si desea continuar el sistema no funcionará correctamente", "CAMPOS VACIOS", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNameTwo.Focus();
            }
            this.Hide();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (txtIp.IsEnabled == false)
            {
                txtIp.IsEnabled = true;
                btn1.IsEnabled = true;
                btn1.Cursor = Cursors.Hand;
            }
            else
            {
                txtIp.IsEnabled = false;
                btn1.Cursor = Cursors.No;
            }
            
        }

        private void txtIp_GotFocus(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show("Realizará cambios en el sistema esto puede afectar el funcionamiento, ¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta == MessageBoxResult.No)
            {
                txtIp.IsEnabled = false;
            }
        }

        private void Image_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (txtPort.IsEnabled == false)
            {
                txtPort.IsEnabled = true;
                btn2.Cursor = Cursors.Hand;
            }
            else
            {
                txtPort.IsEnabled = false;
                btn2.Cursor = Cursors.No;
            }
        }

        private void txtPort_GotFocus(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show("Realizará cambios en el sistema esto puede afectar el funcionamiento, ¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta == MessageBoxResult.No)
            {
                txtPort.IsEnabled = false;
            }
        }

        private void Image_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            if (txtUser.IsEnabled == false)
            {
                txtUser.IsEnabled = true;
                btn3.Cursor = Cursors.Hand;
            }
            else
            {
                txtUser.IsEnabled = false;
                btn3.Cursor = Cursors.No;
            }
        }

        private void txtUser_GotFocus(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show("Realizará cambios en el sistema esto puede afectar el funcionamiento, ¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta == MessageBoxResult.No)
            {
                txtUser.IsEnabled = false;
            }
        }

        private void Image_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            if (txtPassword.IsEnabled == false)
            {
                txtPassword.IsEnabled = true;
                btn4.Cursor = Cursors.Hand;
            }
            else
            {
                txtPassword.IsEnabled = false;
                btn4.Cursor = Cursors.No;
            }
        }

        private void txtPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show("Realizará cambios en el sistema esto puede afectar el funcionamiento, ¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta == MessageBoxResult.No)
            {
                txtPassword.IsEnabled = false;
            }
        }

        private void Image_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            if (txtGMT.IsEnabled == false)
            {
                txtGMT.IsEnabled = true;
                btn5.Cursor = Cursors.Hand;
            }
            else
            {
                txtGMT.IsEnabled = false;
                btn5.Cursor = Cursors.No;
            }
        }

        private void txtGMT_GotFocus(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show("Realizará cambios en el sistema esto puede afectar el funcionamiento, ¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta == MessageBoxResult.No)
            {
                txtGMT.IsEnabled = false;
            }
        }

        private void Image_MouseDown_5(object sender, MouseButtonEventArgs e)
        {
            if (txtNameOne.IsEnabled == false)
            {
                txtNameOne.IsEnabled = true;
                btn6.Cursor = Cursors.Hand;
            }
            else
            {
                txtNameOne.IsEnabled = false;
                btn6.Cursor = Cursors.No;
            }
        }

        private void txtNameOne_GotFocus(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show("Realizará cambios en el sistema esto puede afectar el funcionamiento, ¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta == MessageBoxResult.No)
            {
                txtNameOne.IsEnabled = false;
            }
        }

        private void Image_MouseDown_6(object sender, MouseButtonEventArgs e)
        {
            if (txtNameTwo.IsEnabled == false)
            {
                txtNameTwo.IsEnabled = true;
                btn7.Cursor = Cursors.Hand;
            }
            else
            {
                txtNameTwo.IsEnabled = false;
                btn7.Cursor = Cursors.No;
            }
        }

        private void txtNameTwo_GotFocus(object sender, RoutedEventArgs e)
        {
            var respuesta = MessageBox.Show("Realizará cambios en el sistema esto puede afectar el funcionamiento, ¿Desea continuar?", "Advertencia", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (respuesta == MessageBoxResult.No)
            {
                txtNameTwo.IsEnabled = false;
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar la direccion IP de conexión al servidor?","ACTUALIZAR CAMPO",MessageBoxButton.OKCancel,MessageBoxImage.Question);

            if(respuesta == MessageBoxResult.OK)
            {
                
                dato = txtIp.Text;
                var res = UpdateData(txtIp.Text, "Conf-data1.deivid");

                if (res == 1)
                {
                    txtIp.Text = dato;
                }

                this.Close();
                Configuracion configuracion = new Configuracion();
                configuracion.Show();
                
            }
            
        }

        private void TextBlock_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar el puerto de conexión al servidor?", "ACTUALIZAR CAMPO", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.OK)
            {
                dato = txtPort.Text;
                var res = UpdateData(txtPort.Text, "Conf-data2.deivid");

                if (res == 1)
                {
                    txtPort.Text = dato;
                }

                this.Hide();
                Configuracion configuracion = new Configuracion();
                configuracion.Show();
                
            }
        }

        private void TextBlock_MouseDown_2(object sender, MouseButtonEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar el nombre de usuario?", "ACTUALIZAR CAMPO", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.OK)
            {
                dato = txtUser.Text;
                var res = UpdateData(txtUser.Text, "Conf-data3.deivid");

                if (res == 1)
                {
                    txtUser.Text = dato;
                }

                this.Hide();
                Configuracion configuracion = new Configuracion();
                configuracion.Show();
                
            }
        }

        private void TextBlock_MouseDown_3(object sender, MouseButtonEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar el password?", "ACTUALIZAR CAMPO", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.OK)
            {
                dato = txtPassword.Password;
                var res = UpdateData(txtPassword.Password, "Conf-data4.deivid");

                if (res == 1)
                {
                    txtPassword.Password= dato;
                }

                this.Hide();
                Configuracion configuracion = new Configuracion();
                configuracion.Show();
                
            }
        }

        private void TextBlock_MouseDown_4(object sender, MouseButtonEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar el GMT?, esto modificará sus archivos XMLTV", "ACTUALIZAR CAMPO", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.OK)
            {
                dato = txtGMT.Text;
                var res = UpdateData(txtGMT.Text, "Conf-data5.deivid");

                if (res == 1)
                {
                    txtGMT.Text = dato;
                }

                this.Hide();
                Configuracion configuracion = new Configuracion();
                configuracion.Show();
                
            }
        }

        private void TextBlock_MouseDown_5(object sender, MouseButtonEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar el nombre del archivo para Citytv?, recuerde que este nombre debe ser el mismo en la configuración de cherryEPG", "ACTUALIZAR CAMPO", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.OK)
            {
                dato = txtNameOne.Text;
                var res = UpdateData(txtNameOne.Text, "Conf-data6.deivid");

                if (res == 1)
                {
                    txtNameOne.Text = dato;
                }

                this.Hide();
                Configuracion configuracion = new Configuracion();
                configuracion.Show();
             
            }
        }

        private void TextBlock_MouseDown_6(object sender, MouseButtonEventArgs e)
        {
            var respuesta = MessageBox.Show("¿Desea actualizar el nombre del archivo para ETTV?, recuerde que este nombre debe ser el mismo en la configuración de cherryEPG", "ACTUALIZAR CAMPO", MessageBoxButton.OKCancel, MessageBoxImage.Question);

            if (respuesta == MessageBoxResult.OK)
            {
                dato = txtNameTwo.Text;
                var res = UpdateData(txtNameTwo.Text, "Conf-data7.deivid");

                if (res == 1)
                {
                    txtNameTwo.Text = dato;
                }

                this.Hide();
                Configuracion configuracion = new Configuracion();
                configuracion.Show();

            }
        }
    }
}
