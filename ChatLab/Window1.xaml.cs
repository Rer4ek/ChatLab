using Microsoft.Win32;
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
using Npgsql;
using System.Data.Common;

namespace ChatLab
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            DBConnection();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();     
            Close();
        }

        static void DBConnection()
        {
            var connectionString = "Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=12345;";
            var npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            Console.WriteLine("Succesful connection!");
        }
    }
}
