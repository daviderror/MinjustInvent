using System;
using System.Collections.Generic;
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
        private List<ARMOrder> dataSource;
        private List<ARMOrder> beforeOrders;
        public Computers()
        {
            InitializeComponent();
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (minjustDBEntities minjustDb = new minjustDBEntities())
                {
                    var itemsForDelete = beforeOrders.Where(_ => !dataSource.Any(x => x.Id == _.Id)).Select(_ => _.Id).ToList();
                    if (itemsForDelete.Count > 0)
                        minjustDb.ARMOrder.RemoveRange(minjustDb.ARMOrder.Where(_ => itemsForDelete.Contains(_.Id)));

                    var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.First(x => x.Id == _.Id))).ToList();
                    if (itemsForUpdate.Count > 0)
                    {
                        var itemsForUpdateIds = itemsForUpdate.Select(_ => _.Id).ToList();
                        var itemsForUpdateFromDb = minjustDb.ARMOrder.Where(_ => itemsForUpdateIds.Contains(_.Id)).ToList();
                        foreach (var item in itemsForUpdateFromDb)
                        {
                            var s = itemsForUpdate.First(_ => _.Id == item.Id);
                            item.InventNumber = s.InventNumber;
                            item.IpAdress = s.IpAdress;
                            item.Memory = s.Memory;
                            item.Name = s.Name;
                            item.Num = s.Num;
                            item.OperationSystem = s.OperationSystem;
                            item.Segment = s.Segment;
                            item.Services = s.Services;
                            item.AccountName = s.AccountName;
                            item.ComputerName = s.ComputerName;
                        }
                    }
                    
                    var itemsForAdd = dataSource.Where(_ => _.Id == Guid.Empty).ToList();
                    if (itemsForAdd.Count > 0)
                    {
                        foreach (var i in itemsForAdd)
                            i.Id = Guid.NewGuid();
                        minjustDb.ARMOrder.AddRange(itemsForAdd);
                    }

                    minjustDb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При выполнении произошла ошибка", MessageBoxButton.OK);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (computersGrid.SelectedIndex >= dataSource.Count)
                return;
            dataSource.RemoveAt(computersGrid.SelectedIndex);
            computersGrid.ItemsSource = new List<ARMOrder>(dataSource);
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
                computersGrid.ItemsSource = dataSource = minjustDb.ARMOrder.OrderBy(_ => _.Num).ToList();
            beforeOrders = new List<ARMOrder>();

            foreach (var d in dataSource)
                beforeOrders.Add(new ARMOrder
                {
                    Id = d.Id,
                    AccountName = d.AccountName,
                    ComputerName = d.ComputerName,
                    InventNumber = d.InventNumber,
                    IpAdress = d.IpAdress,
                    Memory = d.Memory,
                    Name = d.Name,
                    Num = d.Num,
                    OperationSystem = d.OperationSystem,
                    Segment = d.Segment,
                    Services = d.Services
                });
        }
    }
}
