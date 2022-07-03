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
    /// Логика взаимодействия для Telephones.xaml
    /// </summary>
    public partial class Telephones : Window
    {
        private List<TelephonyOrder> dataSource;
        private List<TelephonyOrder> beforeOrders;
        public Telephones()
        {
            InitializeComponent();
        }
        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void phonesGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using (minjustDBEntities minjustDb = new minjustDBEntities())
                phonesGrid.ItemsSource = dataSource = minjustDb.TelephonyOrder.OrderBy(_ => _.Num).ToList();
            beforeOrders = new List<TelephonyOrder>();

            foreach (var d in dataSource)
                beforeOrders.Add(new TelephonyOrder
                {
                    Id = d.Id,
                    Name = d.Name,
                    CabinetNum = d.CabinetNum,
                    CityPhone = d.CityPhone,
                    Department = d.Department,
                    InternalPhone = d.InternalPhone,
                    Num = d.Num,
                    Position = d.Position,
                });
        }
    }
}
