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
    /// Lógica de interacción para ShowError1.xaml
    /// </summary>
    public partial class ShowError1 : Window
    {
        public ShowError1(List<TablaViewModel> tablaFull, string canal)
        {
            InitializeComponent();

            datagrid1.ItemsSource = tablaFull;
            lblencabezado.Content = "Errores de contenido detectados en la hoja de Excel para. "+canal;
        }
    }
}
