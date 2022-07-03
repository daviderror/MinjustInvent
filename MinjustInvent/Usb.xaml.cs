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
    /// Логика взаимодействия для Usb.xaml
    /// </summary>
    public partial class Usb : Window
    {

        private List<USBOrder> dataSource;
        private List<USBOrder> beforeOrders;

        public Usb()
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
            try
            {
                using (minjustDBEntities minjustDb = new minjustDBEntities())
                {
                    var itemsForDelete = beforeOrders.Where(_ => !dataSource.Any(x => x.Id == _.Id)).Select(_ => _.Id).ToList();
                    if (itemsForDelete.Count > 0)
                        minjustDb.USBOrder.RemoveRange(minjustDb.USBOrder.Where(_ => itemsForDelete.Contains(_.Id)));

                    var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.FirstOrDefault(x => x.Id == _.Id))).ToList();
                    if (itemsForUpdate.Count > 0)
                    {
                        var itemsForUpdateIds = itemsForUpdate.Select(_ => _.Id).ToList();
                        var itemsForUpdateFromDb = minjustDb.USBOrder.Where(_ => itemsForUpdateIds.Contains(_.Id)).ToList();
                        foreach (var item in itemsForUpdateFromDb)
                        {
                            var s = itemsForUpdate.First(_ => _.Id == item.Id);
                            item.Name = s.Name;
                            item.SerialNumber = s.SerialNumber;
                            item.Size = s.Size;
                        }
                    }

                    var itemsForAdd = dataSource.Where(_ => _.Id == Guid.Empty).ToList();
                    if (itemsForAdd.Count > 0)
                    {
                        foreach (var i in itemsForAdd)
                            i.Id = Guid.NewGuid();
                        minjustDb.USBOrder.AddRange(itemsForAdd);
                    }
                    minjustDb.SaveChanges();
                    phonesGrid_Loaded(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При выполнении произошла ошибка", MessageBoxButton.OK);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (usbsGrid.SelectedIndex >= dataSource.Count)
                return;
            dataSource.RemoveAt(usbsGrid.SelectedIndex);
            usbsGrid.ItemsSource = null;
            usbsGrid.ItemsSource = dataSource;
        }

        private void phonesGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using (minjustDBEntities minjustDb = new minjustDBEntities())
                usbsGrid.ItemsSource = dataSource = minjustDb.USBOrder.OrderBy(_ => _.Name).ToList();
            beforeOrders = new List<USBOrder>();

            foreach (var d in dataSource)
                beforeOrders.Add(new USBOrder
                {
                    Id = d.Id,
                    Name = d.Name,
                    SerialNumber = d.SerialNumber,
                    Size = d.Size
                });
        }
    }
}
