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

namespace EPG_translate
{
    /// <summary>
    /// Lógica de interacción para procesoCorrecto.xaml
    /// </summary>
    public partial class procesoCorrecto : Window
    {
        public procesoCorrecto()
        {
            InitializeComponent();
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Principal principal = new Principal();
            principal.Show();
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
    }
}
