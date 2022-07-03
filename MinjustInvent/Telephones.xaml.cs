using MinjustInvent.Model;
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
        private List<TelephonyModel> dataSource;
        private List<TelephonyModel> beforeOrders;
        private List<Department> allDeps;
        public Telephones()
        {
            InitializeComponent();
            using (minjustDBEntities minjustDb = new minjustDBEntities())
                allDeps = minjustDb.Department.ToList();
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
                            minjustDb.TelephonyOrder.RemoveRange(minjustDb.TelephonyOrder.Where(_ => itemsForDelete.Contains(_.Id)));

                        StringBuilder indexErrors = new StringBuilder();

                        var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.FirstOrDefault(x => x.Id == _.Id))).ToList();
                        if (itemsForUpdate.Count > 0)
                        {
                            var itemsForUpdateIds = itemsForUpdate.Select(_ => _.Id).ToList();
                            var itemsForUpdateFromDb = minjustDb.TelephonyOrder.Where(_ => itemsForUpdateIds.Contains(_.Id)).ToList();
                            foreach (var item in itemsForUpdateFromDb)
                            {
                                
                                var s = itemsForUpdate.First(_ => _.Id == item.Id);
                                var error = allDeps.FirstOrDefault(x => x.IndexNum == s.DepartmentIndex);
                                if (error == null && !string.IsNullOrEmpty(s.DepartmentIndex))
                                    indexErrors.Append($"Нет отдела с индексом {s.DepartmentIndex}\n");
                                item.Name = s.Name;
                                item.CityPhone = s.CityPhone;
                                item.CabinetNum = s.CabinetNum;
                                item.InternalPhone = s.InternalPhone;
                                item.Position = s.Position;
                                item.Num = s.Num;
                                item.DepartmentId = error?.Id;
                            }
                        }

                        var itemsForAdd = dataSource.Where(_ => _.Id == Guid.Empty).ToList();
                        if (itemsForAdd.Count > 0)
                        {
                            foreach(var added in itemsForAdd)
                            {
                                var error = allDeps.FirstOrDefault(x => x.IndexNum == added.DepartmentIndex);
                                if (error == null && !string.IsNullOrEmpty(added.DepartmentIndex))
                                    indexErrors.Append($"Нет отдела с индексом {added.DepartmentIndex}\n");
                            }

                            var addData = itemsForAdd.Select(_ => new TelephonyOrder()
                            {
                                CabinetNum = _.CabinetNum,
                                Position = _.Position,
                                Num = _.Num,
                                Name = _.Name,
                                InternalPhone = _.InternalPhone,
                                CityPhone = _.CityPhone,
                                DepartmentId = allDeps.FirstOrDefault(x => x.IndexNum == _.DepartmentIndex)?.Id,
                                Id = Guid.NewGuid()
                            });
                            minjustDb.TelephonyOrder.AddRange(addData);
                        }

                        minjustDb.SaveChanges();
                        phonesGrid_Loaded(null, null);
                        if (!string.IsNullOrEmpty(indexErrors.ToString()))
                            MessageBox.Show(indexErrors.ToString(), "Не удалось сохранить данные об отделах", MessageBoxButton.OK);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При выполнении произошла ошибка", MessageBoxButton.OK);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Вы уверены что хотите удалить строку?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    using (minjustDBEntities minjustDb = new minjustDBEntities())
                    {
                        var itemsForDelete = beforeOrders.Where(_ => !dataSource.Any(x => x.Id == _.Id)).Select(_ => _.Id).ToList();
                        if (itemsForDelete.Count > 0)
                            minjustDb.TelephonyOrder.RemoveRange(minjustDb.TelephonyOrder.Where(_ => itemsForDelete.Contains(_.Id)));

                        var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.FirstOrDefault(x => x.Id == _.Id))).ToList();
                        if (itemsForUpdate.Count > 0)
                        {
                            var itemsForUpdateIds = itemsForUpdate.Select(_ => _.Id).ToList();
                            var itemsForUpdateFromDb = minjustDb.TelephonyOrder.Where(_ => itemsForUpdateIds.Contains(_.Id)).ToList();
                            foreach (var item in itemsForUpdateFromDb)
                            {
                                var s = itemsForUpdate.First(_ => _.Id == item.Id);
                                item.Name = s.Name;
                                item.CityPhone = s.CityPhone;
                                item.CabinetNum = s.CabinetNum;
                                item.InternalPhone = s.InternalPhone;
                                item.Position = s.Position;
                                item.Num = s.Num;
                                item.DepartmentId = allDeps.FirstOrDefault(x => x.IndexNum == s.DepartmentIndex)?.Id;
                            }
                        }

                        var itemsForAdd = dataSource.Where(_ => _.Id == Guid.Empty).ToList();
                        if (itemsForAdd.Count > 0)
                        {
                            var addData = itemsForAdd.Select(_ => new TelephonyOrder()
                            {
                                CabinetNum = _.CabinetNum,
                                Position = _.Position,
                                Num = _.Num,
                                Name = _.Name,
                                InternalPhone = _.InternalPhone,
                                CityPhone = _.CityPhone,
                                DepartmentId = allDeps.FirstOrDefault(x => x.IndexNum == _.DepartmentIndex)?.Id,
                                Id = Guid.NewGuid()
                            });
                            minjustDb.TelephonyOrder.AddRange(addData);
                        }

                        minjustDb.SaveChanges();
                        phonesGrid_Loaded(null, null);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При выполнении произошла ошибка", MessageBoxButton.OK);
            }
            if (phonesGrid.SelectedIndex >= dataSource.Count)
                return;
            dataSource.RemoveAt(phonesGrid.SelectedIndex);
            phonesGrid.ItemsSource = null;
            phonesGrid.ItemsSource = dataSource;


        }

        private void phonesGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using (minjustDBEntities minjustDb = new minjustDBEntities())
                phonesGrid.ItemsSource = dataSource = minjustDb.TelephonyOrder.OrderBy(_ => _.Num).Select(_ => 
                new TelephonyModel { Id = _.Id, CabinetNum = _.CabinetNum, CityPhone = _.CityPhone, DepartmentId = _.DepartmentId,
                                     DepartmentIndex = _.Department.IndexNum, InternalPhone = _.InternalPhone, Name = _.Name,
                                     Num = _.Num, Position = _.Position}).ToList();
            beforeOrders = new List<TelephonyModel>();

            foreach (var d in dataSource)
                beforeOrders.Add(new TelephonyModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    CabinetNum = d.CabinetNum,
                    CityPhone = d.CityPhone,
                    DepartmentIndex = d.DepartmentIndex,
                    InternalPhone = d.InternalPhone,
                    DepartmentId = d.DepartmentId,
                    Num = d.Num,
                    Position = d.Position,
                });
        }
    }
}
