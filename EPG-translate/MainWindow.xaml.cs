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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;

namespace EPG_translate
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //inicializo el timer para el splash
        DispatcherTimer timer = new DispatcherTimer();
        public int count = 0;

        Principal principal =  new Principal();

        public MainWindow()
        {
            InitializeComponent();

            //Defino el timer
            timer.Tick += new EventHandler(WaitingEvent);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        //Metodo para cerrar el splash despues de 4 segundos y dar paso a la nueva ventana
        public void WaitingEvent(object Source, EventArgs e)
        {
            if (count <= 4)
            {
                count++;
            }

            else
            {
                this.Close();
                principal.Show();
                timer.Stop();
            }
        }

        private void progres_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }
    }
}
