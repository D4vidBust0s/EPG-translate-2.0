﻿using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Lógica de interacción para IngestaB.xaml
    /// </summary>
    public partial class IngestaB : Window
    {
        public string rutaExcel = "";
        public string hoja1 = "";
        public string hoja2 = "";
        public int Servicios;
        public int Id;

        public string Canal = "";

        public IngestaB(string rutaArchivo, string sheet1, int servicios, int id)
        {
            InitializeComponent();
            
            rutaExcel = rutaArchivo;
            hoja1 = sheet1;
            Servicios = servicios;
            Id = id;

            if (id == 1)
            {
                Canal = "Citytv";
            }
            else if (id == 2)
            {
                Canal = "El Tiempo Televisión";
            }

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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SLDocument sl = new SLDocument(rutaExcel,hoja1);
            int row = 2;
            DataTable dt = new DataTable();

            List<ArchivoViewModel> list = new List<ArchivoViewModel>();

            while (!string.IsNullOrEmpty(sl.GetCellValueAsString(row,1)))
            {
                ArchivoViewModel archivovm = new ArchivoViewModel();
                archivovm.fecha = (sl.GetCellValueAsDateTime(row, 1)).ToString("dd, MMMM, yyyy");
                archivovm.hora = (sl.GetCellValueAsDateTime(row, 2 )).ToString("H:mm:ss");
                archivovm.duracion = (sl.GetCellValueAsDateTime(row, 3)).ToString("H:mm:ss");
                archivovm.titulo = sl.GetCellValueAsString(row, 4);
                archivovm.shor = sl.GetCellValueAsString(row,5);
                archivovm.synopsis = sl.GetCellValueAsString(row,6);

                list.Add(archivovm);

                row++;
            }

            Datagrid1.ItemsSource = list;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            IngestaA ingestaA = new IngestaA();
            ingestaA.Show();
            this.Hide();
        }

        private void Continuar_Click(object sender, RoutedEventArgs e)
        {
            //valido si tengo que mostrar la otra oja de excel si las dos hojas estan en el mimo documeto de lo contrario sigo
            if(Servicios == 2)
            {
                //Se procesan 2 hojas
                this.Hide();
                IngestaC ingestaC = new IngestaC(rutaExcel, hoja1, Servicios,Id);
                ingestaC.Show();
            }
            else if(Servicios == 1)
            {
                //Se procesa una hoja nada mas
                this.Hide();
                XlsxTOXmlTv xlsxTOXmlTv = new XlsxTOXmlTv(Id.ToString(),Canal,rutaExcel,hoja1,hoja2);    
                xlsxTOXmlTv.Show();
                
            }
            
        }
    }
}
