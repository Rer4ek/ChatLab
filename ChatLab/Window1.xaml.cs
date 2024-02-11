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
using System.Collections.ObjectModel;
using Npgsql.Internal;

namespace ChatLab
{
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            //dataBase.DBInputCommand($"INSERT INTO users (name, id) VALUES ('{SignText.Text}', ROW_NUMBER() over())");
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();     
            Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GoToRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
            Close();
        }
    }

    
}
