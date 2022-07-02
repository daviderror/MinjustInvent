using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MinjustInvent
{
    /// <summary>
    /// Логика взаимодействия для Computers.xaml
    /// </summary>
    public partial class Computers : Window
    {
        public Computers()
        {
            InitializeComponent();
            this.Closing += Computers_Closing;
        }

        private void Computers_Closing(object sender, CancelEventArgs e)
        {
        }

        private void UpdateDB()
        {
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            using (minjustDBEntities minjustDb = new minjustDBEntities())
            {
                
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            using (minjustDBEntities minjustDb = new minjustDBEntities())
                computersGrid.ItemsSource = minjustDb.ARMOrder.ToList();
        }


        private void computersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
