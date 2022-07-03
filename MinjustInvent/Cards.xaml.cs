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
    /// Логика взаимодействия для Cards.xaml
    /// </summary>
    public partial class Cards : Window
    {
        private List<KartochkiOrder> dataSource;
        private List<KartochkiOrder> beforeOrders;
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

                        var itemsForUpdate = dataSource.Where(_ => _.Id != Guid.Empty && !_.DBEquals(beforeOrders.FirstOrDefault(x => x.Id == _.Id))).ToList();
                        if (itemsForUpdate.Count > 0)
                        {
                            var itemsForUpdateIds = itemsForUpdate.Select(_ => _.Id).ToList();
                            var itemsForUpdateFromDb = minjustDb.KartochkiOrder.Where(_ => itemsForUpdateIds.Contains(_.Id)).ToList();
                            foreach (var item in itemsForUpdateFromDb)
                            {
                                var c = itemsForUpdate.First(_ => _.Id == item.Id);
                                item.Name = c.Name;
                                item.Card = c.Card;
                                item.ReceivedSignature = c.ReceivedSignature;
                                item.IssuedSignature = c.IssuedSignature;
                                item.DepartmentId = allDeps.FirstOrDefault(x => x.IndexNum == c.DepartmentIndex)?.Id;
                            }
                        }

                        var itemsForAdd = dataSource.Where(_ => _.Id == Guid.Empty).ToList();
                        if (itemsForAdd.Count > 0)
                        {
                            var addData = itemsForAdd.Select(_ => new KartochkiOrder()
                            {
                                CabinetNum = _.CabinetNum,
                                Position = _.Position,
                                Num = _.Num,
                                Name = _.Name,

                                DepartmentId = allDeps.FirstOrDefault(x => x.IndexNum == _.DepartmentIndex)?.Id,
                                Id = Guid.NewGuid()
                            });
                            minjustDb.KartochkiOrder.AddRange(addData);
                        }

                        minjustDb.SaveChanges();
                        cardsGrid_Loaded(null, null);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "При выполнении произошла ошибка", MessageBoxButton.OK);
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (cardsGrid_Loaded.SelectedIndex < dataSource.Count)
            {
                dataSource.RemoveAt(phonesGrid.SelectedIndex);
                phonesGrid.ItemsSource = null;
                phonesGrid.ItemsSource = dataSource;
            }
        }

        private void updateButton_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cardsGrid_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
