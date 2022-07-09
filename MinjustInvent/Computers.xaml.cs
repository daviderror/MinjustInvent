using MinjustInvent.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace MinjustInvent
{
    /// <summary>
    /// Логика взаимодействия для Computers.xaml
    /// </summary>
    public partial class Computers : Window
    {
        private List<ARMOrder> dataSource;
        private List<ARMOrder> beforeOrders;

        BackgroundWorker excelSaver = new BackgroundWorker();

        public Computers()
        {
            InitializeComponent();
            filePathText.Text = ExcelManager.FilePath;

            excelSaver.DoWork += ExcelWork;
            excelSaver.RunWorkerCompleted += ExcelWorkCompleted;
        }

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите сохранить изменения?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    using (minjustDBEntities minjustDb = new minjustDBEntities())
                    {
                        var itemsForDelete = beforeOrders.Where(_ => !dataSource.Any(x => x.Id == _.Id)).Select(_ => _.Id).ToList();
                        if (itemsForDelete.Count > 0)
                            minjustDb.ARMOrder.RemoveRange(minjustDb.ARMOrder.Where(_ => itemsForDelete.Contains(_.Id)));

                        var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.FirstOrDefault(x => x.Id == _.Id))).ToList();
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
                        Grid_Loaded(null, null);
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
            computersGrid.ItemsSource = null;
            computersGrid.ItemsSource = dataSource;
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

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            excelSaver.RunWorkerAsync();
        }

        void ExcelWork(object sender, DoWorkEventArgs e)
        {
            var excel = new ARMExcelManager();

            e.Result = excel.SaveExcel(dataSource).Result;
        }

        void ExcelWorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result as bool? == true)
                MessageBox.Show("Файл excel сохранен", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Не удалось сохранить excel-файл", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            excelSaver.DoWork -= ExcelWork;
            excelSaver.RunWorkerCompleted -= ExcelWorkCompleted;
        }

        private void setFileNameButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDlg = new System.Windows.Forms.FolderBrowserDialog();
            var result = openFileDlg.ShowDialog();
            if (result.ToString() != string.Empty)
            {
                ExcelManager.FilePath = openFileDlg.SelectedPath;
                filePathText.Text = openFileDlg.SelectedPath;
            }
        }
    }
}
