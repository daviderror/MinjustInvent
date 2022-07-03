using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MinjustInvent
{
    /// <summary>
    /// Логика взаимодействия для Departments.xaml
    /// </summary>
    public partial class Departments : Window
    {
        private List<Department> dataSource;
        private List<Department> beforeOrders;
        public Departments()
        {
            InitializeComponent();
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
                            minjustDb.Department.RemoveRange(minjustDb.Department.Where(_ => itemsForDelete.Contains(_.Id)));

                        var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.FirstOrDefault(x => x.Id == _.Id))).ToList();
                        if (itemsForUpdate.Count > 0)
                        {
                            var itemsForUpdateIds = itemsForUpdate.Select(_ => _.Id).ToList();
                            var itemsForUpdateFromDb = minjustDb.Department.Where(_ => itemsForUpdateIds.Contains(_.Id)).ToList();
                            foreach (var item in itemsForUpdateFromDb)
                            {
                                var s = itemsForUpdate.First(_ => _.Id == item.Id);
                                item.Name = s.Name;
                                item.IndexNum = s.IndexNum;
                            }
                        }

                        var itemsForAdd = dataSource.Where(_ => _.Id == Guid.Empty).ToList();
                        if (itemsForAdd.Count > 0)
                        {
                            foreach (var i in itemsForAdd)
                                i.Id = Guid.NewGuid();
                            minjustDb.Department.AddRange(itemsForAdd);
                        }

                        minjustDb.SaveChanges();
                        departmentsGrid_Loaded(null, null);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При выполнении произошла ошибка", MessageBoxButton.OK);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (departmentsGrid.SelectedIndex >= dataSource.Count)
                return;
            dataSource.RemoveAt(departmentsGrid.SelectedIndex);
            departmentsGrid.ItemsSource = null;
            departmentsGrid.ItemsSource = dataSource;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void departmentsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using (minjustDBEntities minjustDb = new minjustDBEntities())
                departmentsGrid.ItemsSource = dataSource = minjustDb.Department.OrderBy(_ => _.IndexNum).ToList();
            beforeOrders = new List<Department>();

            foreach (var d in dataSource)
                beforeOrders.Add(new Department
                {
                    Id = d.Id,
                    IndexNum = d.IndexNum,
                    Name = d.Name
                });
        }
    }
}
