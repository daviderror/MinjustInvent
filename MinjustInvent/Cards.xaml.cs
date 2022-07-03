using MinjustInvent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace MinjustInvent
{
    /// <summary>
    /// Логика взаимодействия для Cards.xaml
    /// </summary>
    public partial class Cards : Window
    {
        private List<CardModel> dataSource;
        private List<CardModel> beforeOrders;
        private List<Department> allDeps;
        public Cards()
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
                            minjustDb.KartochkiOrder.RemoveRange(minjustDb.KartochkiOrder.Where(_ => itemsForDelete.Contains(_.Id)));

                        StringBuilder indexErrors = new StringBuilder();

                        var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.FirstOrDefault(x => x.Id == _.Id))).ToList();
                        if (itemsForUpdate.Count > 0)
                        {
                            var itemsForUpdateIds = itemsForUpdate.Select(_ => _.Id).ToList();
                            var itemsForUpdateFromDb = minjustDb.KartochkiOrder.Where(_ => itemsForUpdateIds.Contains(_.Id)).ToList();
                            foreach (var item in itemsForUpdateFromDb)
                            {
                                var c = itemsForUpdate.First(_ => _.Id == item.Id);
                                var error = allDeps.FirstOrDefault(x => x.IndexNum == c.DepartmentIndex);
                                if (error == null && !string.IsNullOrEmpty(c.DepartmentIndex))
                                    indexErrors.Append($"Нет отдела с индексом {c.DepartmentIndex}\n");
                                item.Name = c.Name;
                                item.Card = c.Card;
                                item.ReceivedSignature = c.ReceivedSignature;
                                item.IssuedSignature = c.IssuedSignature;
                                item.DepartmentId = allDeps.FirstOrDefault(x => x.IndexNum == c.DepartmentIndex)?.Id;
                                item.Department = allDeps.FirstOrDefault(x => x.IndexNum == c.DepartmentIndex);
                            }
                        }

                        var itemsForAdd = dataSource.Where(_ => _.Id == Guid.Empty).ToList();
                        if (itemsForAdd.Count > 0)
                        {
                            foreach (var added in itemsForAdd)
                            {
                                var error = allDeps.FirstOrDefault(x => x.IndexNum == added.DepartmentIndex);
                                if (error == null && !string.IsNullOrEmpty(added.DepartmentIndex))
                                    indexErrors.Append($"Нет отдела с индексом {added.DepartmentIndex}\n");
                            }

                            var addData = itemsForAdd.Select(_ => new KartochkiOrder()
                            {
                                Name = _.Name,
                                Card = _.Card,
                                IssuedSignature = _.IssuedSignature,
                                ReceivedSignature = _.ReceivedSignature,
                                DepartmentId = allDeps.FirstOrDefault(x => x.IndexNum == _.DepartmentIndex)?.Id,
                                Id = Guid.NewGuid()
                            });
                            minjustDb.KartochkiOrder.AddRange(addData);
                        }

                        minjustDb.SaveChanges();
                        cardsGrid_Loaded(null, null);

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
            if (cardsGrid.SelectedIndex < dataSource.Count)
            {
                dataSource.RemoveAt(cardsGrid.SelectedIndex);
                cardsGrid.ItemsSource = null;
                cardsGrid.ItemsSource = dataSource;
            }
        }

        private void cardsGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using (minjustDBEntities minjustDb = new minjustDBEntities())
                cardsGrid.ItemsSource = dataSource = minjustDb.KartochkiOrder.OrderBy(_ => _.Name).Select(_ =>
                new CardModel
                {
                    Id = _.Id,
                    DepartmentId = _.DepartmentId,
                    DepartmentIndex = _.Department.IndexNum,
                    Card = _.Card,
                    IssuedSignature = _.IssuedSignature,
                    Name = _.Name,
                    ReceivedSignature = _.ReceivedSignature
                }).ToList();
            beforeOrders = new List<CardModel>();

            foreach (var d in dataSource)
                beforeOrders.Add(new CardModel
                {
                    Id = d.Id,
                    Name = d.Name,
                    Card = d.Card,
                    DepartmentId = d.DepartmentId,
                    DepartmentIndex = d.DepartmentIndex,
                    IssuedSignature = d.IssuedSignature,
                    ReceivedSignature = d.ReceivedSignature
                });
        }
    }
}
