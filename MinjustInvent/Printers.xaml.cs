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

namespace MinjustInvent
{
    /// <summary>
    /// Логика взаимодействия для Printers.xaml
    /// </summary>
    public partial class Printers : Window
    {
        public Printers()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void AddPrinter(object sender, RoutedEventArgs e)
        {

        }

        private void DeletePrinter(object sender, RoutedEventArgs e)
        {

        }
    }
}
