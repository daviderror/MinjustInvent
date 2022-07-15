using MinjustInvent.Excel;
using MinjustInvent.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;


namespace MinjustInvent
{
    /// <summary>
    /// Логика взаимодействия для Usb.xaml
    /// </summary>
    public partial class Usb : Window
    {

        private List<USBOrder> dataSource;
        private List<USBOrder> beforeOrders;

        BackgroundWorker excelSaver = new BackgroundWorker();

        public Usb()
        {
            InitializeComponent();
            filePathText.Text = ExcelManager.FilePath;

            excelSaver.DoWork += ExcelWork;
            excelSaver.RunWorkerCompleted += ExcelWorkCompleted;
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
                if (MessageBox.Show("Вы уверены что хотите сохранить изменения?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
                        UsbGrid_Loaded(null, null);
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

        private void UsbGrid_Loaded(object sender, RoutedEventArgs e)
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
        void ExcelWork(object sender, DoWorkEventArgs e)
        {
            var excel = new UsbExcelManager();

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

        private void printButton_Click(object sender, RoutedEventArgs e)
        {
            excelSaver.RunWorkerAsync();
        }
    }
}
