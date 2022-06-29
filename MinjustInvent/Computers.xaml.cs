using System.Windows;
using System;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Data.Entity;

namespace MinjustInvent
{
    /// <summary>
    /// Логика взаимодействия для Computers.xaml
    /// </summary>
    public partial class Computers : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable computersTable;
        public Computers()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DBMinjust"].ConnectionString;
        }
        private void UpdateDB()
        {
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
            adapter.Update(computersTable);
        }
        private void updateButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (computersGrid.SelectedItems != null)
            {
                for (int i = 0; i < computersGrid.SelectedItems.Count; i++)
                {
                    DataRowView dataRowView = computersGrid.SelectedItems[i] as DataRowView;
                    if (dataRowView != null)
                    {
                        DataRow dataRow = (DataRow)dataRowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //string sql = "SELECT * FROM Computers";
            //computersTable = new DataTable();
            //SqlConnection connection = null;
            //try
            //{
            //    connection = new SqlConnection(connectionString);
            //    SqlCommand command = new SqlCommand(sql, connection);
            //    adapter = new SqlDataAdapter(command);

            //    //установка команды на добавление для вызова хранимой процедуры
            //    adapter.InsertCommand = new SqlCommand("sp_InsertComputer", connection);
            //    adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            //    adapter.InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.Text, 50, "Name"));
            //    adapter.InsertCommand.Parameters.Add(new SqlParameter("@Segment", SqlDbType.Text, 50, "Segment"));
            //    adapter.InsertCommand.Parameters.Add(new SqlParameter("@Ip", SqlDbType.Int, 50, "Ip"));
            //    adapter.InsertCommand.Parameters.Add(new SqlParameter("@OperationSystem", SqlDbType.Text, 50, "OperationSystem"));
            //    adapter.InsertCommand.Parameters.Add(new SqlParameter("@Memory", SqlDbType.Int, 50, "Memory"));
            //    adapter.InsertCommand.Parameters.Add(new SqlParameter("@InventNumber", SqlDbType.NVarChar, 50, "InventNumber"));
            //    adapter.InsertCommand.Parameters.Add(new SqlParameter("@ComputerName", SqlDbType.Text, 50, "ComputerName"));

            //    connection.Open();
            //    adapter.Fill(computersTable);
            //    computersGrid.ItemsSource = computersTable.DefaultView;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    if (connection != null)
            //        connection.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Computers";
            computersTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                //установка команды на добавление для вызова хранимой процедуры
                adapter.InsertCommand = new SqlCommand("sp_InsertComputers", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
              //  adapter.InsertCommand.Parameters.Add(new SqlParameter("@Name", SqlDbType.Text, 50, "Name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Segment", SqlDbType.Text, 50, "Segment"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Ip", SqlDbType.NVarChar, 0, "Ip"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@OperationSystem", SqlDbType.Text, 0, "OperationSystem"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Memory", SqlDbType.Int, 0, "Memory"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@InventNumber", SqlDbType.NVarChar, 0, "InventNumber"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@ComputerName", SqlDbType.Text, 0, "ComputerName");
                parameter.Direction = ParameterDirection.Output;

                connection.Open();
                adapter.Fill(computersTable);
                computersGrid.ItemsSource = computersTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void computersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
